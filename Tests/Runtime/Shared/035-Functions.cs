using System.Runtime.CompilerServices;
using Unity.Burst;
using UnityBenchShared;

namespace Burst.Compiler.IL.Tests
{
    internal class Functions
    {
        [TestCompiler]
        public static int CheckFunctionCall()
        {
            return AnotherFunction();
        }

        private static int AnotherFunction()
        {
            return 150;
        }

        [TestCompiler(ExpectCompilerException = true, ExpectedDiagnosticId = DiagnosticId.ERR_UnableToAccessManagedMethod)]
        public static void Boxing()
        {
            var a = new CustomStruct();
            // This will box CustomStruct, so this method should fail when compiling
            a.GetType();
        }

        private struct CustomStruct
        {

        }

        public static int NotDiscardable()
        {
            return 3;
        }

        [BurstDiscard]
        public static void Discardable()
        {
        }

        [TestCompiler]
        public static int TestCallsOfDiscardedMethodRegression()
        {
            // The regression was that we would queue all calls of a method, but if we encountered a discardable one
            // We would stop visiting pending methods. This resulting in method bodies not being visited.
            Discardable();
            return NotDiscardable();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int NoInline(int x)
        {
            return x;
        }

        [TestCompiler(42)]
        public static int TestNoInline(int x)
        {
            return NoInline(x);
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        public static int NoOptimization(int x)
        {
            return x;
        }

        [TestCompiler(42)]
        public static int TestNoOptimization(int x)
        {
            return NoOptimization(x);
        }

        [TestCompiler(42)]
        public static int TestImplicitCapture(int x)
        {
            return SomeFunction();

            int SomeFunction()
            {
                return x;
            }
        }

        public struct Pair
        {
            public int X;
            public int Y;

            public struct Provider : IArgumentProvider
            {
                public object Value => new Pair { X = 13, Y = 42 };
            }
        }

        [TestCompiler(42, typeof(Pair.Provider))]
        public static int TestImplicitCaptureInLoop(int x, ref Pair rp)
        {
            int total = 0;
            Pair p = rp;

            for (int i = 0; i < x; i++)
            {
                total += SomeFunction(42, 42, 42, 42, 42, i);

                int SomeFunction(int a, int b, int c, int d, int e, int otherI)
                {
                    if (p.Y != 0)
                    {
                        return (otherI == i) ? 56 : -13;
                    }

                    return 0;
                }
            }

            return total;
        }
    }
}
