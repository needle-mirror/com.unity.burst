using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Mathematics;
using UnityBenchShared;

namespace Burst.Compiler.IL.Tests
{
    /// <summary>
    /// Tests types
    /// </summary>
    internal class NotSupported
    {
        [TestCompiler(1, ExpectCompilerException = true)]
        public static int TestDelegate(int data)
        {
            return ProcessData(i => i + 1, data);
        }

        // TODO: Try to find a way to re-enable this test.
        // For now it's disabled because it's not testing the right thing -
        // it fails because of the `object` parameter, not because of the
        // `as` expression.

        //[TestCompiler(1, ExpectCompilerException = true)]
        //public static bool TestIsOfType(object data)
        //{
        //    var check = data as NotSupported;
        //    return (check != null);
        //}

        private static int ProcessData(Func<int, int> yo, int value)
        {
            return yo(value);
        }

        public struct HasMarshalAttribute
        {
            [MarshalAs(UnmanagedType.U1)] public bool A;
        }

        //[TestCompiler(ExpectCompilerException = true)]
        [TestCompiler()] // Because MarshalAs is used in mathematics we cannot disable it for now
        public static void TestStructWithMarshalAs()
        {
#pragma warning disable 0219
            var x = new HasMarshalAttribute();
#pragma warning restore 0219
        }

        [TestCompiler(true, ExpectCompilerException = true)]
        public static void TestMethodWithMarshalAsParameter([MarshalAs(UnmanagedType.U1)] bool x)
        {
        }

        [TestCompiler(ExpectCompilerException = true)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static bool TestMethodWithMarshalAsReturnType()
        {
            return true;
        }

        private static float3 a = new float3(1, 2, 3);

        [TestCompiler(ExpectCompilerException = true)]
        public static bool TestStaticLoad()
        {
            var cmp = a == new float3(1, 2, 3);

            return cmp.x && cmp.y && cmp.z;
        }

        [TestCompiler(ExpectCompilerException = true)]
        public static void TestStaticStore()
        {
            a.x = 42;
        }

        public delegate char CharbyValueDelegate(char c);

        public static char CharbyValue(char c)
        {
            return c;
        }

        public struct CharbyValueFunc : IFunctionPointer
        {
            public FunctionPointer<CharbyValueDelegate> FunctionPointer;

            public IFunctionPointer FromIntPtr(IntPtr ptr)
            {
                return new CharbyValueFunc() { FunctionPointer = new FunctionPointer<CharbyValueDelegate>(ptr) };
            }
        }

        [TestCompiler("CharbyValue", 0x1234, ExpectCompilerException = true)]
        public static int TestCharbyValue(ref CharbyValueFunc fp, int i)
        {
            var c = (char)i;
            return fp.FunctionPointer.Invoke(c);
        }

        static private readonly half3 h3_h = new half3(new half(42.0f));
        static private readonly half3 h3_d = new half3(0.5);
        static private readonly half3 h3_v2s = new half3(new half2(new half(1.0f), new half(2.0f)), new half(0.5f));
        static private readonly half3 h3_sv2 = new half3(new half(0.5f), new half2(new half(1.0f), new half(2.0f)));
        static private readonly half3 h3_v3 = new half3(new half(0.5f), new half(42.0f), new half(13.0f));

        [TestCompiler(ExpectCompilerException = true)]
        public static float TestStaticHalf3()
        {
            var result = (float3)h3_h + h3_d + h3_v2s + h3_sv2 + h3_v3;
            return result.x + result.y + result.z;
        }
    }
}
