using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace zzzUnity.Burst.CodeGen
{
    internal delegate void LogDelegate(string message);

    /// <summary>
    /// Main class for post processing assemblies. The post processing is currently performing:
    /// - Replace C# call from C# to Burst functions with attributes [BurstCompile] to a call to the compiled Burst function
    ///   In both editor and standalone scenarios. For DOTS Runtime, this is done differently at BclApp level by patching
    ///   DllImport.
    /// </summary>
    internal class ILPostProcessing
    {
        private AssemblyDefinition _burstAssembly;
        private TypeDefinition _burstCompilerTypeDefinition;
        private MethodReference _burstCompilerIsEnabledMethodDefinition;
        private MethodReference _burstCompilerCompileUnsafeStaticMethodMethodDefinition;
        private MethodReference _burstCompilerCompileUnsafeStaticMethodReinitialiseAttributeCtor;
        private TypeSystem _typeSystem;
        private TypeReference _systemType;
        private TypeReference _typeReferenceRuntimeMethodHandle;
        private AssemblyDefinition _assemblyDefinition;
        private bool _modified;
        private bool _isForEditor;

        private readonly Dictionary<MethodDefinition, MethodDefinition> _mapping;
        private readonly HashSet<TypeDefinition> _invokers;

        private const string PostfixBurstDirectCall = "$BurstDirectCall";
        private const string PostfixManaged = "$BurstManaged";
        private const string GetMethodHandleName = "GetMethodHandle";
        private const string InvokeName = "Invoke";

        private FunctionPointerInvokeTransform _functionPointerInvokeTransform;

        public ILPostProcessing(AssemblyLoader loader, bool isForEditor, LogDelegate log=null, int logLevel=0)
        {
            Loader = loader;
            _isForEditor = isForEditor;
            _invokers = new HashSet<TypeDefinition>();
            _mapping = new Dictionary<MethodDefinition, MethodDefinition>();

            _functionPointerInvokeTransform = new FunctionPointerInvokeTransform(log, logLevel);
        }

        public bool IsForEditor => _isForEditor;

        public AssemblyLoader Loader { get; }

        /// <summary>
        /// Checks the specified method is a direct call.
        /// </summary>
        /// <param name="method">The method being called</param>
        public static bool IsDirectCall(MethodDefinition method)
        {
            // Method can be null so we early exit without a NullReferenceException
            return method != null && method.IsStatic && method.Name == InvokeName && method.DeclaringType.Name.EndsWith(ILPostProcessing.PostfixBurstDirectCall);
        }

        /// <summary>
        /// Gets the managed method from the declaring type of a direct call.
        /// </summary>
        /// <param name="typeDefinition">Declaring type of a direct call</param>
        public static MethodReference RecoverManagedMethodFromDirectCall(TypeDefinition typeDefinition)
        {
            var method = typeDefinition.Methods.FirstOrDefault(x => x.Name == GetMethodHandleName);
            Debug.Assert(method != null);
            return (MethodReference) method.Body.Instructions[0].Operand;
        }

        public unsafe bool Run(IntPtr peData, int peSize, IntPtr pdbData, int pdbSize, out AssemblyDefinition assemblyDefinition)
        {
            if (peData == IntPtr.Zero) throw new ArgumentNullException(nameof(peData));
            if (peSize <= 0) throw new ArgumentOutOfRangeException(nameof(peSize));
            if (pdbData != IntPtr.Zero && pdbSize <= 0) throw new ArgumentOutOfRangeException(nameof(pdbSize));

            var peStream = new UnmanagedMemoryStream((byte*)peData, peSize);
            Stream pdbStream = null;
            if (pdbData != IntPtr.Zero)
            {
                pdbStream = new UnmanagedMemoryStream((byte*)pdbData, pdbSize);
            }

            assemblyDefinition = Loader.LoadFromStream(peStream, pdbStream);
            return Run(assemblyDefinition);
        }

        public bool Run(AssemblyDefinition assemblyDefinition)
        {
            _assemblyDefinition = assemblyDefinition;
            _typeSystem = assemblyDefinition.MainModule.TypeSystem;
            _functionPointerInvokeTransform.Initialize(Loader, assemblyDefinition, _typeSystem);

            _modified = false;
            var types = assemblyDefinition.MainModule.GetTypes().ToArray();
            foreach (var type in types)
            {
                _functionPointerInvokeTransform.CollectDelegateInvokesFromType(type);
                ProcessType(type);
            }

            if (_modified)
            {
                foreach (var type in types)
                {
                    PatchMapping(type);
                }
            }

            _modified |= _functionPointerInvokeTransform.Finish();

            return _modified;
        }

        private void PatchMapping(TypeDefinition type)
        {
            // Don't patch invokers
            if (_invokers.Contains(type)) return;

            // Make a copy because we are going to modify it
            var methodCount = type.Methods.Count;
            for (var j = 0; j < methodCount; j++)
            {
                var method = type.Methods[j];
                if (!method.HasBody) return;

                foreach (var instruction in method.Body.Instructions)
                {
                    if (instruction.Operand is MethodDefinition methodDef && _mapping.TryGetValue(methodDef, out var newMethod))
                    {
                        if (!_functionPointerInvokeTransform.IsInstructionForFunctionPointerInvoke(method, instruction))
                        {
                            instruction.Operand = newMethod;
                        }
                    }
                }
            }
        }

        private void ProcessType(TypeDefinition type)
        {
            if (!type.HasGenericParameters && TryGetBurstCompilerAttribute(type, out _))
            {
                // Make a copy because we are going to modify it
                var methodCount = type.Methods.Count;
                for (var j = 0; j < methodCount; j++)
                {
                    var method = type.Methods[j];
                    if (!method.IsStatic || method.HasGenericParameters || !TryGetBurstCompilerAttribute(method, out var methodBurstCompileAttribute)) continue;

                    bool isDirectCallDisabled = false;
                    if (methodBurstCompileAttribute.HasProperties)
                    {
                        foreach (var property in methodBurstCompileAttribute.Properties)
                        {
                            if (property.Name == "DisableDirectCall")
                            {
                                isDirectCallDisabled = (bool)property.Argument.Value;
                                break;
                            }
                        }
                    }

#if !UNITY_DOTSPLAYER       // Direct call is not Supported for dots runtime via this pre-processor, its handled elsewhere, this code assumes a Unity Editor based burst
                    if (!isDirectCallDisabled)
                    {
                        if (_burstAssembly == null)
                        {
                            var resolved = methodBurstCompileAttribute.Constructor.DeclaringType.Resolve();
                            InitializeBurstAssembly(resolved.Module.Assembly);
                        }

                        ProcessMethodForDirectCall(method);
                        _modified = true;
                    }
#endif
                }
            }
        }

        private void ProcessMethodForDirectCall(MethodDefinition managed)
        {
            var declaringType = managed.DeclaringType;

            var originalName = managed.Name;
            managed.Name = $"{managed.Name}{PostfixManaged}";

            // Create a copy of the original method that will be the actual managed method
            // The original method is patched at the end of this method to call
            // the dispatcher that will go to the Burst implementation or the managed method (if in the editor and Burst is disabled)
            var newManaged = new MethodDefinition(originalName, managed.Attributes, managed.ReturnType)
            {
                DeclaringType = declaringType,
                ImplAttributes = managed.ImplAttributes,
                MetadataToken = managed.MetadataToken,
            };
            managed.Attributes &= ~MethodAttributes.Public;
            managed.Attributes |= MethodAttributes.Private;

            declaringType.Methods.Add(newManaged);
            foreach (var parameter in managed.Parameters)
            {
                newManaged.Parameters.Add(parameter);
            }

            foreach (var customAttr in managed.CustomAttributes)
            {
                newManaged.CustomAttributes.Add(customAttr);
            }

            // private static class (Name_RID.$Postfix)
            var cls = new TypeDefinition(declaringType.Namespace, $"{originalName}_{managed.MetadataToken.RID:X8}{PostfixBurstDirectCall}",
                TypeAttributes.NestedPrivate |
                TypeAttributes.AutoLayout |
                TypeAttributes.AnsiClass |
                TypeAttributes.Abstract |
                TypeAttributes.Sealed |
                TypeAttributes.BeforeFieldInit
            )
            {
                DeclaringType = declaringType,
                BaseType = _typeSystem.Object
            };
            declaringType.NestedTypes.Add(cls);
            _invokers.Add(cls);

            // Create GetMethodHandle method:
            //
            // public static RuntimeMethodHandle GetMethodHandle() {
            //   ldtoken managedMethod
            //   ret
            // }
            var getMethodHandleMethod = new MethodDefinition(GetMethodHandleName, MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.Static, _typeReferenceRuntimeMethodHandle)
            {
                ImplAttributes = MethodImplAttributes.IL | MethodImplAttributes.Managed,
                DeclaringType = cls
            };

            var processor = getMethodHandleMethod.Body.GetILProcessor();
            processor.Emit(OpCodes.Ldtoken, managed);
            processor.Emit(OpCodes.Ret);
            cls.Methods.Add(FixDebugInformation(getMethodHandleMethod));

            // Create Field:
            //
            // private static void* Pointer;
            var intPtr = _typeSystem.IntPtr;
            var pointerField = new FieldDefinition("Pointer", FieldAttributes.Static | FieldAttributes.Private, intPtr)
            {
                DeclaringType = cls
            };
            cls.Fields.Add(pointerField);

            // Create the static Constructor Method (called via .cctor and via reflection on burst compilation enable)
            // private static Constructor() {
            //   Pointer = Unity.Burst.BurstCompiler::CompileUnsafeStaticMethod(GetMethodHandle());
            //   ret
            // }

            var constructor = new MethodDefinition("Constructor", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Static, _typeSystem.Void)
            {
                ImplAttributes = MethodImplAttributes.IL | MethodImplAttributes.Managed,
                DeclaringType = cls
            };

            processor = constructor.Body.GetILProcessor();
            processor.Emit(OpCodes.Call, getMethodHandleMethod);
            processor.Emit(OpCodes.Call, _burstCompilerCompileUnsafeStaticMethodMethodDefinition);
            processor.Emit(OpCodes.Stsfld, pointerField);
            processor.Emit(OpCodes.Ret);
            cls.Methods.Add(FixDebugInformation(constructor));

            var asmAttribute = new CustomAttribute(_burstCompilerCompileUnsafeStaticMethodReinitialiseAttributeCtor);
            asmAttribute.ConstructorArguments.Add(new CustomAttributeArgument(_systemType, cls));
            _assemblyDefinition.CustomAttributes.Add(asmAttribute);

            // Create an Initialize method
            //
            // [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.AfterAssembliesLoaded)]
            // [UnityEditor.InitializeOnLoadMethod] // When its an editor assembly
            // private static void Initialize()
            // {
            // }
            var initializeOnLoadMethod = new MethodDefinition("Initialize", MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.Static, _typeSystem.Void)
            {
                ImplAttributes = MethodImplAttributes.IL | MethodImplAttributes.Managed,
                DeclaringType = cls
            };

            processor = initializeOnLoadMethod.Body.GetILProcessor();
            processor.Emit(OpCodes.Ret);
            cls.Methods.Add(FixDebugInformation(initializeOnLoadMethod));

            var attribute = new CustomAttribute(_unityEngineInitializeOnLoadAttributeCtor);
            attribute.ConstructorArguments.Add(new CustomAttributeArgument(_unityEngineRuntimeInitializeLoadType, _unityEngineRuntimeInitializeLoadAfterAssemblies.Constant));
            initializeOnLoadMethod.CustomAttributes.Add(attribute);

            if (IsForEditor)
            {
                // Need to ensure the editor tag for initialize on load is present, otherwise edit mode tests will not call Initialize
                attribute = new CustomAttribute(_unityEditorInitilizeOnLoadAttributeCtor);
                initializeOnLoadMethod.CustomAttributes.Add(attribute);
            }

            // Create the static constructor
            //
            // public static .cctor() {
            //   Constructor();
            //   ret
            // }
            var cctor = new MethodDefinition(".cctor", MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Static, _typeSystem.Void)
            {
                ImplAttributes = MethodImplAttributes.IL | MethodImplAttributes.Managed,
                DeclaringType = cls,
            };

            processor = cctor.Body.GetILProcessor();
            processor.Emit(OpCodes.Call, constructor);
            processor.Emit(OpCodes.Ret);

            cls.Methods.Add(FixDebugInformation(cctor));

            // Create the Invoke method based on the original method (same signature)
            //
            // public static XXX Invoke(...args) {
            //    if (BurstCompiler.IsEnabled)
            //    {
            //        calli(...args)
            //        ret;
            //    }
            //    return OriginalMethod(...args);
            // }
            var invoke = new MethodDefinition(InvokeName, newManaged.Attributes, managed.ReturnType)
            {
                ImplAttributes = MethodImplAttributes.IL | MethodImplAttributes.Managed,
                DeclaringType = cls
            };
            ;
            var signature = new CallSite(managed.ReturnType);
            foreach (var parameter in managed.Parameters)
            {
                invoke.Parameters.Add(parameter);
                signature.Parameters.Add(parameter);
            }

            processor = invoke.Body.GetILProcessor();

            processor.Emit(OpCodes.Call, _burstCompilerIsEnabledMethodDefinition);

            EmitArguments(processor, invoke);
            processor.Emit(OpCodes.Ldsfld, pointerField);
            processor.Emit(OpCodes.Calli, signature);
            processor.Emit(OpCodes.Ret);

            var previousRet = processor.Body.Instructions[processor.Body.Instructions.Count - 1];
            EmitArguments(processor, invoke);
            processor.Emit(OpCodes.Call, managed);
            processor.Emit(OpCodes.Ret);

            // Insert the branch once we have emitted the instructions
            processor.InsertAfter(processor.Body.Instructions[0], Instruction.Create(OpCodes.Brfalse, previousRet.Next));
            cls.Methods.Add(FixDebugInformation(invoke));

            // Final patching of the original method
            // public static XXX OriginalMethod(...args) {
            //      Name_RID.$Postfix.Invoke(...args);
            //      ret;
            // }
            processor = newManaged.Body.GetILProcessor();
            EmitArguments(processor, newManaged);
            processor.Emit(OpCodes.Call, invoke);
            processor.Emit(OpCodes.Ret);


            _mapping.Add(managed, newManaged);
        }

        private static MethodDefinition FixDebugInformation(MethodDefinition method)
        {
            method.DebugInformation.Scope = new ScopeDebugInformation(method.Body.Instructions.First(), method.Body.Instructions.Last());
            return method;
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

        private MethodReference _unityEngineInitializeOnLoadAttributeCtor;
        private TypeReference _unityEngineRuntimeInitializeLoadType;
        private FieldDefinition _unityEngineRuntimeInitializeLoadAfterAssemblies;
        private MethodReference _unityEditorInitilizeOnLoadAttributeCtor;

        private void InitializeBurstAssembly(AssemblyDefinition burstAssembly)
        {
            _burstAssembly = burstAssembly;
            _burstCompilerTypeDefinition = burstAssembly.MainModule.GetType("Unity.Burst", "BurstCompiler");

            // Dots_player version of BurstCompiler does not have IsEnabled property nor CompileUnsafeStaticMethod
            _burstCompilerIsEnabledMethodDefinition = _assemblyDefinition.MainModule.ImportReference(_burstCompilerTypeDefinition.Methods.FirstOrDefault(x => x.Name == "get_IsEnabled"));
            _burstCompilerCompileUnsafeStaticMethodMethodDefinition = _assemblyDefinition.MainModule.ImportReference(_burstCompilerTypeDefinition.Methods.FirstOrDefault(x => x.Name == "CompileUnsafeStaticMethod"));
            var reinitializeAttribute = _burstCompilerTypeDefinition.NestedTypes.FirstOrDefault(x => x.Name == "StaticTypeReinitAttribute");
            _burstCompilerCompileUnsafeStaticMethodReinitialiseAttributeCtor = _assemblyDefinition.MainModule.ImportReference(reinitializeAttribute.Methods.FirstOrDefault(x=>x.Name == ".ctor" && x.HasParameters));

            var corLibrary =  Loader.Resolve((AssemblyNameReference)_typeSystem.CoreLibrary);
            _systemType = _assemblyDefinition.MainModule.ImportReference(corLibrary.MainModule.GetType("System.Type"));
            _typeReferenceRuntimeMethodHandle = _assemblyDefinition.MainModule.ImportReference((corLibrary.MainModule.GetType(typeof(RuntimeMethodHandle).Namespace, typeof(RuntimeMethodHandle).Name)));

            var asmDef = GetAsmDefinitionFromFile(Loader, "UnityEngine.CoreModule.dll");
            var runtimeInitializeOnLoadMethodAttribute =  asmDef.MainModule.GetType("UnityEngine", "RuntimeInitializeOnLoadMethodAttribute");
            var runtimeInitializeLoadType = asmDef.MainModule.GetType("UnityEngine", "RuntimeInitializeLoadType");

            _unityEngineInitializeOnLoadAttributeCtor = _assemblyDefinition.MainModule.ImportReference(runtimeInitializeOnLoadMethodAttribute.Methods.FirstOrDefault(x => x.Name == ".ctor" && x.HasParameters));
            _unityEngineRuntimeInitializeLoadType = _assemblyDefinition.MainModule.ImportReference(runtimeInitializeLoadType);
            _unityEngineRuntimeInitializeLoadAfterAssemblies = runtimeInitializeLoadType.Fields.FirstOrDefault(x => x.Name=="AfterAssembliesLoaded");

            if (IsForEditor)
            {
                asmDef = GetAsmDefinitionFromFile(Loader, "UnityEditor.CoreModule.dll");
                if (asmDef == null)
                    asmDef = GetAsmDefinitionFromFile(Loader, "UnityEditor.dll");
                var initializeOnLoadMethodAttribute = asmDef.MainModule.GetType("UnityEditor", "InitializeOnLoadMethodAttribute");

                _unityEditorInitilizeOnLoadAttributeCtor = _assemblyDefinition.MainModule.ImportReference(initializeOnLoadMethodAttribute.Methods.FirstOrDefault(x => x.Name == ".ctor" && !x.HasParameters));

            }
        }

        private static void EmitArguments(ILProcessor processor, MethodDefinition method)
        {
            for (var i = 0; i < method.Parameters.Count; i++)
            {
                var parameter = method.Parameters[i];
                switch (i)
                {
                    case 0:
                        processor.Emit(OpCodes.Ldarg_0);
                        break;
                    case 1:
                        processor.Emit(OpCodes.Ldarg_1);
                        break;
                    case 2:
                        processor.Emit(OpCodes.Ldarg_2);
                        break;
                    case 3:
                        processor.Emit(OpCodes.Ldarg_3);
                        break;
                    default:
                        if (i <= 255)
                        {
                            processor.Emit(OpCodes.Ldarg_S, (byte)i);
                        }
                        else
                        {
                            processor.Emit(OpCodes.Ldarg, i);
                        }
                        break;
                }
            }
        }

        private static bool TryGetBurstCompilerAttribute(ICustomAttributeProvider provider, out CustomAttribute customAttribute)
        {
            if (provider.HasCustomAttributes)
            {
                foreach (var customAttr in provider.CustomAttributes)
                {
                    if (customAttr.Constructor.DeclaringType.Name == "BurstCompileAttribute")
                    {
                        customAttribute = customAttr;
                        return true;
                    }
                }
            }
            customAttribute = null;
            return false;
        }
    }
}
