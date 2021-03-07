using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Pdb;
using Mono.Cecil.Rocks;

namespace zzzUnity.Burst.CodeGen
{
    /// <summary>
    /// Transforms a direct invoke on a burst function pointer into an calli, avoiding the need to marshal the delegate back.
    /// </summary>
    internal class FunctionPointerInvokeTransform
    {
        private struct CaptureInformation
        {
            public MethodReference Operand;

            public List<Instruction> Captured;
        }

        private Dictionary<MethodDefinition, TypeReference> _needsIl2cppInvoke;
        private Dictionary<MethodDefinition, List<CaptureInformation>> _capturedSets;
        private MethodDefinition _monoPInvokeAttributeCtorDef;
        private MethodDefinition _nativePInvokeAttributeCtorDef;

        private TypeReference _burstFunctionPointerType;
        private TypeReference _burstCompilerType;
        private TypeReference _systemType;

        private LogDelegate _debugLog;
        private int _logLevel;

#if UNITY_DOTSPLAYER
        public readonly static bool IsEnabled = true;
#else
        public readonly static bool IsEnabled = false;		// For now only run the pass on dots player/tiny
#endif

        public FunctionPointerInvokeTransform(LogDelegate log, int logLevel = 0)
        {
            _needsIl2cppInvoke = new Dictionary<MethodDefinition, TypeReference>();
            _capturedSets = new Dictionary<MethodDefinition, List<CaptureInformation>>();
            _monoPInvokeAttributeCtorDef = null;
            _nativePInvokeAttributeCtorDef = null;  // Only present on DOTS_PLAYER
            _burstFunctionPointerType = null;
            _burstCompilerType = null;
            _systemType = null;
            _debugLog = log;
            _logLevel = logLevel;
        }

        private AssemblyDefinition GetAsmDefinitionFromFile(AssemblyLoader loader, string filename)
        {
            foreach (var folder in loader.GetSearchDirectories())
            {
                var path = Path.Combine(folder, filename);
                if (File.Exists(path))
                    return loader.LoadFromFile(path);
            }
            return null;
        }

        public void Initialize(AssemblyLoader loader, AssemblyDefinition assemblyDefinition, TypeSystem typeSystem)
        {
            if (!IsEnabled)
                return;

            if (_monoPInvokeAttributeCtorDef == null)
            {
                var burstAssembly = GetAsmDefinitionFromFile(loader, "Unity.Burst.dll");

                _burstFunctionPointerType = burstAssembly.MainModule.GetType("Unity.Burst.FunctionPointer`1");
                _burstCompilerType = burstAssembly.MainModule.GetType("Unity.Burst.BurstCompiler");

                var corLibrary = loader.Resolve(typeSystem.CoreLibrary as AssemblyNameReference);
                _systemType = corLibrary.MainModule.Types.FirstOrDefault(x => x.FullName == "System.Type"); // Only needed for MonoPInvokeCallback constructor in Unity

#if UNITY_DOTSPLAYER
                var asmDef = assemblyDefinition;
                if (asmDef.Name.Name != "Unity.Runtime")
                    asmDef = GetAsmDefinitionFromFile(loader, "Unity.Runtime.dll");

                if (asmDef == null)
                    return;

                var monoPInvokeAttribute = asmDef.MainModule.GetType("Unity.Jobs.MonoPInvokeCallbackAttribute");
                _monoPInvokeAttributeCtorDef = monoPInvokeAttribute.GetConstructors().First();

                var nativePInvokeAttribute = asmDef.MainModule.GetType("NativePInvokeCallbackAttribute");
                _nativePInvokeAttributeCtorDef = nativePInvokeAttribute.GetConstructors().First();
#else
                var asmDef = GetAsmDefinitionFromFile(loader, "UnityEngine.CoreModule.dll");

                var monoPInvokeAttribute = asmDef.MainModule.GetType("AOT.MonoPInvokeCallbackAttribute");
                _monoPInvokeAttributeCtorDef = monoPInvokeAttribute.GetConstructors().First();
#endif
            }

        }

        public void CollectDelegateInvokesFromType(TypeDefinition type)
        {
            if (!IsEnabled)
                return;

            foreach (var m in type.Methods)
            {
                if (m.HasBody)
                {
                    CollectDelegateInvokes(m);
                }
            }
        }

        private bool ProcessIl2cppInvokeFixups()
        {
            if (_monoPInvokeAttributeCtorDef == null)
                return false;

            bool modified = false;
            foreach (var invokeNeeded in _needsIl2cppInvoke)
            {
                var declaringType = invokeNeeded.Value;
                var implementationMethod = invokeNeeded.Key;

#if UNITY_DOTSPLAYER

                // At present always uses monoPInvokeAttribute because we don't currently know if the burst is enabled
                var attribute = new CustomAttribute(implementationMethod.Module.ImportReference(_monoPInvokeAttributeCtorDef));
                implementationMethod.CustomAttributes.Add(attribute);
                modified = true;

#else
                // Unity requires a type parameter for the attributecallback
                if (declaringType == null)
                {
                    _debugLog?.Invoke($"FunctionPtrInvoke.LocateFunctionPointerTCreation: Unable to automatically append CallbackAttribute due to missing declaringType for {implementationMethod}");
                    continue;
                }

                var attribute = new CustomAttribute(implementationMethod.Module.ImportReference(_monoPInvokeAttributeCtorDef));
                attribute.ConstructorArguments.Add(new CustomAttributeArgument(implementationMethod.Module.ImportReference(_systemType), implementationMethod.Module.ImportReference(declaringType)));
                implementationMethod.CustomAttributes.Add(attribute);
                modified = true;

                if (_logLevel > 1) _debugLog?.Invoke($"FunctionPtrInvoke.LocateFunctionPointerTCreation: Added InvokeCallbackAttribute to  {implementationMethod}");
#endif
            }

            return modified;
        }

        private bool ProcessFunctionPointerInvokes()
        {
            var madeChange = false;
            foreach (var capturedData in _capturedSets)
            {
                var latePatchMethod = capturedData.Key;
                var capturedList = capturedData.Value;

                latePatchMethod.Body.SimplifyMacros();  // De-optimise short branches, since we will end up inserting instructions

                foreach(var capturedInfo in capturedList)
                {
                    var captured = capturedInfo.Captured;
                    var operand = capturedInfo.Operand;

                    if (captured.Count<3)
                    {
                        _debugLog?.Invoke($"FunctionPtrInvoke.Finish: 2 or fewer instructions captured - Unable to optimise this reference");
                        continue;
                    }

                    if (_logLevel > 1) _debugLog?.Invoke($"FunctionPtrInvoke.Finish:{Environment.NewLine}  latePatchMethod:{latePatchMethod}{Environment.NewLine}  captureList:{capturedList}{Environment.NewLine}  capture0:{captured[0]}{Environment.NewLine}  operand:{operand}");

                    var processor = latePatchMethod.Body.GetILProcessor();

                    var callsite = new CallSite(operand.ReturnType) { CallingConvention = MethodCallingConvention.C };

                    for (int oo = 0; oo < operand.Parameters.Count; oo++)
                    {
                        callsite.Parameters.Add(operand.Parameters[oo]);
                    }

                    var calli = processor.Create(OpCodes.Calli, callsite);
                    processor.Replace(captured[captured.Count - 1], calli);


                    // take first 2/3 and move...
                    int numToMove = 2;
                    if (captured[1].OpCode == OpCodes.Ldflda)
                        numToMove = 3;
                    for (int rr = 0; rr < numToMove; rr++)
                    {
                        // We do it this way so that if one of the instructions we are moving happens to be a branch target, the target continues to
                        //point to the correct place, without having to search out the branch instructions and repoint them to the next instruction
                        var replacementNop = processor.Create(OpCodes.Nop);
                        var newInstruction = processor.Create(OpCodes.Nop);
                        newInstruction.OpCode = captured[rr].OpCode;
                        newInstruction.Operand = captured[rr].Operand;
                        processor.InsertBefore(calli, newInstruction);

                        processor.Replace(captured[rr], replacementNop);
                    }

                    var originalGetInvoke = calli.Previous;

                    if (originalGetInvoke.Operand is MethodReference mmr)
                    {
                        var genericMethodDef = mmr.Resolve();

                        var genericInstanceType = mmr.DeclaringType as GenericInstanceType;
                        var genericInstanceDef = genericInstanceType.Resolve();

                        // Locate the correct instance method - we know already at this point we have an instance of Function
                        MethodReference mr = default;
                        bool failed = true;
                        foreach (var m in genericInstanceDef.Methods)
                        {
                            if (m.FullName.Contains("get_Value"))
                            {
                                mr = m;
                                failed = false;
                                break;
                            }
                        }
                        if (failed)
                        {
                            _debugLog?.Invoke($"FunctionPtrInvoke.Finish: failed to locate get_Value method on {genericInstanceDef} - Unable to optimise this reference");
                            continue;
                        }

                        var newGenericRef = new MethodReference(mr.Name, mr.ReturnType, genericInstanceType)
                        {
                            HasThis = mr.HasThis,
                            ExplicitThis = mr.ExplicitThis,
                            CallingConvention = mr.CallingConvention
                        };
                        foreach (var param in mr.Parameters)
                            newGenericRef.Parameters.Add(new ParameterDefinition(param.ParameterType));
                        foreach (var gparam in mr.GenericParameters)
                            newGenericRef.GenericParameters.Add(new GenericParameter(gparam.Name, newGenericRef));
                        var importRef = latePatchMethod.Module.ImportReference(newGenericRef);
                        var newMethodCall = processor.Create(OpCodes.Call, importRef);
                        processor.Replace(originalGetInvoke, newMethodCall);

                        if (_logLevel > 1) _debugLog?.Invoke($"FunctionPtrInvoke.Finish: Optimised {originalGetInvoke} with {newMethodCall}");

                        madeChange = true;
                    }
                }

                latePatchMethod.Body.OptimizeMacros();  // Re-optimise branches
            }
            return madeChange;
        }

        public bool Finish()
        {
            if (!IsEnabled)
                return false;

            bool madeChange = ProcessIl2cppInvokeFixups();
            madeChange |= ProcessFunctionPointerInvokes();

            return madeChange;
        }

        private bool IsBurstFunctionPointerMethod(MethodReference methodRef, string method, out GenericInstanceType methodInstance)
        {
            methodInstance = methodRef?.DeclaringType as GenericInstanceType;
            return (methodInstance != null && methodInstance.ElementType.FullName == _burstFunctionPointerType.FullName && methodRef.Name == method);
        }

        private bool IsBurstCompilerMethod(MethodReference methodRef, string method, out TypeReference methodInstance)
        {
            methodInstance = methodRef?.DeclaringType as TypeReference;
            return (methodInstance != null && methodInstance.FullName == _burstCompilerType.FullName && methodRef.Name == method);
        }

        private void LocateFunctionPointerTCreation(MethodDefinition m, Instruction i)
        {
            if (i.OpCode == OpCodes.Call)
            {
                if (!IsBurstCompilerMethod(i.Operand as MethodReference, "CompileFunctionPointer", out var methodInstance)) return;

                // Currently only handles the following pre-pattern (which should cover most common uses)
                // ldftn ...
                // newobj ...

                if (i.Previous?.OpCode != OpCodes.Newobj)
                {
                    _debugLog?.Invoke($"FunctionPtrInvoke.LocateFunctionPointerTCreation: Unable to automatically append CallbackAttribute due to not finding NewObj {i.Previous}");
                    return;
                }

                var newObj = i.Previous;
                if (newObj.Previous?.OpCode != OpCodes.Ldftn)
                {
                    _debugLog?.Invoke($"FunctionPtrInvoke.LocateFunctionPointerTCreation: Unable to automatically append CallbackAttribute due to not finding LdFtn {newObj.Previous}");
                    return;
                }

                var ldFtn = newObj.Previous;

                // Determine the delegate type
                var methodDefinition = newObj.Operand as MethodDefinition;
                var declaringType = methodDefinition?.DeclaringType;

                // Fetch the implementation method
                var implementationMethod = ldFtn.Operand as MethodDefinition;

                var hasInvokeAlready = implementationMethod?.CustomAttributes.FirstOrDefault(x =>
                     (x.AttributeType.FullName == _monoPInvokeAttributeCtorDef.DeclaringType.FullName)
                     || (_nativePInvokeAttributeCtorDef != null && x.AttributeType.FullName == _nativePInvokeAttributeCtorDef.DeclaringType.FullName));

                if (hasInvokeAlready != null)
                {
                    if (_logLevel > 2) _debugLog?.Invoke($"FunctionPtrInvoke.LocateFunctionPointerTCreation: Skipping appending Callback Attribute as already present {hasInvokeAlready}");
                    return;
                }

                if (implementationMethod == null)
                {
                    _debugLog?.Invoke($"FunctionPtrInvoke.LocateFunctionPointerTCreation: Unable to automatically append CallbackAttribute due to missing method from {ldFtn} {ldFtn.Operand}");
                    return;
                }

                if (implementationMethod.CustomAttributes.FirstOrDefault(x => x.Constructor.DeclaringType.Name == "BurstCompileAttribute")==null)
                {
                    _debugLog?.Invoke($"FunctionPtrInvoke.LocateFunctionPointerTCreation: Unable to automatically append CallbackAttribute due to missing burst attribute from {implementationMethod}");
                    return;
                }

                // Need to add the custom attribute
                if (!_needsIl2cppInvoke.ContainsKey(implementationMethod))
                {
                    _needsIl2cppInvoke.Add(implementationMethod, declaringType);
                }
            }
        }

        [Obsolete("Will be removed in a future Burst verison")]
        public bool IsInstructionForFunctionPointerInvoke(MethodDefinition m, Instruction i)
        {
            throw new NotImplementedException();
        }

        private void CollectDelegateInvokes(MethodDefinition m)
        {
            bool hitGetInvoke = false;
            TypeDefinition delegateType = null;
            List<Instruction> captured = null;

            foreach (var inst in m.Body.Instructions)
            {
                if (_logLevel > 2) _debugLog?.Invoke($"FunctionPtrInvoke.CollectDelegateInvokes: CurrentInstruction {inst} {inst.Operand}");

                // Check for a FunctionPointerT creation
                LocateFunctionPointerTCreation(m, inst);

                if (!hitGetInvoke)
                {
                    if (inst.OpCode != OpCodes.Call) continue;
                    if (!IsBurstFunctionPointerMethod(inst.Operand as MethodReference, "get_Invoke", out var methodInstance)) continue;

                    // At this point we have a call to a FunctionPointer.Invoke
                    hitGetInvoke = true;

                    delegateType = methodInstance.GenericArguments[0].Resolve();

                    captured = new List<Instruction>();

                    if (inst.Previous.OpCode == OpCodes.Ldflda) // need to capture the previous.previous
                    {
                        captured.Add(inst.Previous.Previous);
                        captured.Add(inst.Previous);
                        captured.Add(inst);
                    }
                    else if (inst.Previous.OpCode == OpCodes.Ldloca_S)
                    {
                        captured.Add(inst.Previous);
                        captured.Add(inst);
                    }
                    else
                    {
                        _debugLog?.Invoke($"FunctionPtrInvoke.CollectDelegateInvokes: Unexpected instruction leader {inst.Previous.OpCode} - Unable to optimise this reference");
                        hitGetInvoke = false;
                    }
                }
                else
                {
                    captured.Add(inst);

                    if (!(inst.OpCode.FlowControl == FlowControl.Next || inst.OpCode.FlowControl == FlowControl.Call))
                    {
                        // Don't perform transform across blocks
                        hitGetInvoke = false;
                    }
                    else
                    {
                        if (inst.OpCode == OpCodes.Callvirt)
                        {
                            if (inst.Operand is MethodReference mref)
                            {
                                var method = mref.Resolve();

                                if (method.DeclaringType == delegateType)
                                {
                                    hitGetInvoke = false;

                                    List<CaptureInformation> storage = null;
                                    if (!_capturedSets.TryGetValue(m, out storage))
                                    {
                                        storage = new List<CaptureInformation>();
                                        _capturedSets.Add(m, storage);
                                    }

                                    var captureInfo = new CaptureInformation { Captured = captured, Operand = mref };
                                    if (_logLevel > 1) _debugLog?.Invoke($"FunctionPtrInvoke.CollectDelegateInvokes: captureInfo:{captureInfo}{Environment.NewLine}capture0{captured[0]}");
                                    storage.Add(captureInfo);
                                }
                            }
                            else
                            {
                                hitGetInvoke = false;
                            }
                        }
                    }
                }
            }
        }
    }
}
