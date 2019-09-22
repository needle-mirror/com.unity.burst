using System.Runtime.CompilerServices;
using Unity.Burst;
using UnityBenchShared;

namespace Burst.Compiler.IL.Tests
{
    internal class Aliasing
    {
        public unsafe struct NoAliasField
        {
            [NoAlias]
            public int* ptr1;

            public int* ptr2;

            public class Provider : IArgumentProvider
            {
                public object Value => new NoAliasField { ptr1 = null, ptr2 = null };
            }
        }

#if UNITY_2020_1_OR_NEWER || UNITY_BURST_EXPERIMENTAL_FEATURE_ALIASING
        [TestCompiler(typeof(NoAliasField.Provider))]
        public unsafe static void CheckNoAliasFieldWithItself(ref NoAliasField s)
        {
            // Check that they correctly alias with themselves.
            Unity.Burst.Aliasing.ExpectAlias(s.ptr1, s.ptr1);
            Unity.Burst.Aliasing.ExpectAlias(s.ptr2, s.ptr2);
        }

        [TestCompiler(typeof(NoAliasField.Provider), ExpectCompilerException = true)]
        public unsafe static void CheckNoAliasFieldWithItselfBadPtr1(ref NoAliasField s)
        {
            // Check that they correctly alias with themselves.
            Unity.Burst.Aliasing.ExpectNoAlias(s.ptr1, s.ptr1);
        }

        [TestCompiler(typeof(NoAliasField.Provider), ExpectCompilerException = true)]
        public unsafe static void CheckNoAliasFieldWithItselfBadPtr2(ref NoAliasField s)
        {
            // Check that they correctly alias with themselves.
            Unity.Burst.Aliasing.ExpectNoAlias(s.ptr2, s.ptr2);
        }

        [TestCompiler(typeof(NoAliasField.Provider))]
        public unsafe static void CheckNoAliasFieldWithAnotherPointer(ref NoAliasField s)
        {
            // Check that they do not alias each other because of the [NoAlias] on the ptr1 field above.
            Unity.Burst.Aliasing.ExpectNoAlias(s.ptr1, s.ptr2);
        }

        [TestCompiler(typeof(NoAliasField.Provider))]
        public unsafe static void CheckNoAliasFieldWithNull(ref NoAliasField s)
        {
            // Check that comparing a pointer with null is no alias.
            Unity.Burst.Aliasing.ExpectNoAlias(s.ptr1, null);
        }

        [TestCompiler(typeof(NoAliasField.Provider))]
        public unsafe static void CheckAliasFieldWithNull(ref NoAliasField s)
        {
            // Check that comparing a pointer with null is no alias.
            Unity.Burst.Aliasing.ExpectNoAlias(s.ptr2, null);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private unsafe static void NoAliasInfoSubFunctionAlias(int* a, int* b)
        {
            Unity.Burst.Aliasing.ExpectAlias(a, b);
        }

        [TestCompiler(typeof(NoAliasField.Provider), ExpectCompilerException = true)]
        public unsafe static void CheckNoAliasFieldSubFunctionAlias(ref NoAliasField s)
        {
            NoAliasInfoSubFunctionAlias(s.ptr1, s.ptr1);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private unsafe static void NoAliasInfoSubFunctionNoAlias(int* a, int* b)
        {
            Unity.Burst.Aliasing.ExpectNoAlias(a, b);
        }

        [TestCompiler(typeof(NoAliasField.Provider), ExpectCompilerException = true)]
        public unsafe static void CheckNoAliasFieldSubFunctionNoAlias(ref NoAliasField s)
        {
            NoAliasInfoSubFunctionNoAlias(s.ptr1, s.ptr1);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private unsafe static void AliasInfoSubFunctionNoAlias([NoAlias] int* a, int* b)
        {
            Unity.Burst.Aliasing.ExpectNoAlias(a, b);
        }

        [TestCompiler(typeof(NoAliasField.Provider))]
        public unsafe static void CheckNoAliasFieldSubFunctionWithNoAliasParameter(ref NoAliasField s)
        {
            AliasInfoSubFunctionNoAlias(s.ptr1, s.ptr1);
        }
#endif
    }
}
