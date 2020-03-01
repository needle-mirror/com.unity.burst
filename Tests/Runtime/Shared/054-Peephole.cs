using Burst.Compiler.IL.Tests.Helpers;
using Unity.Mathematics;

namespace Burst.Compiler.IL.Tests
{
    internal class Peephole
    {
        [TestCompiler(DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtEqualFast(float f)
        {
            return math.sqrt(f) == 2 ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtNotEqualFast(float f)
        {
            return math.sqrt(f) != 2 ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100)]
        public static int SqrtLessThan(float f)
        {
            return math.sqrt(f) < 2 ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtLessThanFast(float f)
        {
            return math.sqrt(f) < 2 ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtLessThanLargeConstant(float f)
        {
            return math.sqrt(f) < float.MaxValue ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtLessThanFastVector(ref float4 f)
        {
            return math.all(math.sqrt(f) < 2) ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtLessThanLargeConstantVector(ref float4 f)
        {
            return math.all(math.sqrt(f) < new float4(1, 2, 3, float.MaxValue)) ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100)]
        public static int SqrtGreaterThan(float f)
        {
            return math.sqrt(f) > 2 ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtGreaterThanFast(float f)
        {
            return math.sqrt(f) > 2 ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtGreaterThanLargeConstant(float f)
        {
            return math.sqrt(f) > float.MaxValue ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtGreaterThanFastVector(ref float4 f)
        {
            return math.all(math.sqrt(f) > 2) ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtGreaterThanLargeConstantVector(ref float4 f)
        {
            return math.all(math.sqrt(f) > new float4(1, 2, 3, float.MaxValue)) ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100)]
        public static int SqrtLessThanEqual(float f)
        {
            return math.sqrt(f) <= 2 ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtLessThanEqualFast(float f)
        {
            return math.sqrt(f) <= 2 ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtLessThanEqualLargeConstant(float f)
        {
            return math.sqrt(f) <= float.MaxValue ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtLessThanEqualFastVector(ref float4 f)
        {
            return math.all(math.sqrt(f) <= 2) ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtLessThanEqualLargeConstantVector(ref float4 f)
        {
            return math.all(math.sqrt(f) <= new float4(1, 2, 3, float.MaxValue)) ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100)]
        public static int SqrtGreaterThanEqual(float f)
        {
            return math.sqrt(f) >= 2 ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtGreaterThanEqualFast(float f)
        {
            return math.sqrt(f) >= 2 ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtGreaterThanEqualLargeConstant(float f)
        {
            return math.sqrt(f) >= float.MaxValue ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtGreaterThanEqualFastVector(ref float4 f)
        {
            return math.all(math.sqrt(f) >= 2) ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtGreaterThanEqualLargeConstantVector(ref float4 f)
        {
            return math.all(math.sqrt(f) >= new float4(1, 2, 3, float.MaxValue)) ? 42 : 13;
        }

        [TestCompiler(DataRange.ZeroExclusiveTo100, DataRange.ZeroExclusiveTo100, FastMath = true)]
        public static int SqrtAndSqrtFast(ref float4 a, ref float4 b)
        {
            return math.all(math.sqrt(a) >= math.sqrt(b)) ? 42 : 13;
        }
    }
}
