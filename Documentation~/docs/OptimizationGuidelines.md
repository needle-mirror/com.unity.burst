# Optimization Guidelines

# Loop Vectorization

Loop vectorization is one of the ways that Burst improves performance. Let's say you have code like this:

``` c#
[MethodImpl(MethodImplOptions.NoInlining)]
private static unsafe void Bar([NoAlias] int* a, [NoAlias] int* b, int count)
{
    for (var i = 0; i < count; i++)
    {
        a[i] += b[i];
    }
}

public static unsafe void Foo(int count)
{
    var a = stackalloc int[count];
    var b = stackalloc int[count];

    Bar(a, b, count);
}
```

The compiler is able to convert that scalar loop in `Bar` into a vectorized loop. Instead of looping over a single value at a time,
the compiler generates code that loops over multiple values at the same time, producing faster code essentially for free. Here is the 
`x64` assembly generated for `AVX2` for the loop in `Bar` above:

```
.LBB1_4:
    vmovdqu    ymm0, ymmword ptr [rdx + 4*rax]
    vmovdqu    ymm1, ymmword ptr [rdx + 4*rax + 32]
    vmovdqu    ymm2, ymmword ptr [rdx + 4*rax + 64]
    vmovdqu    ymm3, ymmword ptr [rdx + 4*rax + 96]
    vpaddd     ymm0, ymm0, ymmword ptr [rcx + 4*rax]
    vpaddd     ymm1, ymm1, ymmword ptr [rcx + 4*rax + 32]
    vpaddd     ymm2, ymm2, ymmword ptr [rcx + 4*rax + 64]
    vpaddd     ymm3, ymm3, ymmword ptr [rcx + 4*rax + 96]
    vmovdqu    ymmword ptr [rcx + 4*rax], ymm0
    vmovdqu    ymmword ptr [rcx + 4*rax + 32], ymm1
    vmovdqu    ymmword ptr [rcx + 4*rax + 64], ymm2
    vmovdqu    ymmword ptr [rcx + 4*rax + 96], ymm3
    add        rax, 32
    cmp        r8, rax
    jne        .LBB1_4
```

As can be seen above, the loop has been unrolled and vectorized so that it is 4 `vpaddd` instructions, each calculating 8 integer additions,
for a total of **32 integer additions per loop iteration**.

This is great! However, loop vectorization is notoriously brittle. As an example, let's introduce a seemingly innocuous branch like this:

``` c#
[MethodImpl(MethodImplOptions.NoInlining)]
private static unsafe void Bar([NoAlias] int* a, [NoAlias] int* b, int count)
{
    for (var i = 0; i < count; i++)
    {
        if (a[i] > b[i])
        {
            break;
        }

        a[i] += b[i];
    }
}
```

Now the assembly changes to this:

```
.LBB1_3:
    mov        r9d, dword ptr [rcx + 4*r10]
    mov        eax, dword ptr [rdx + 4*r10]
    cmp        r9d, eax
    jg        .LBB1_4
    add        eax, r9d
    mov        dword ptr [rcx + 4*r10], eax
    inc        r10
    cmp        r8, r10
    jne        .LBB1_3
```

This loop is completely scalar and only has 1 integer addition per loop iteration. This is not good! In this simple case,
an experienced developer would probably spot that adding the branch will break auto-vectorization. But in more complex real-life code
it can be difficult to spot.

To help with this problem, Burst includes, at present, **experimental** intrinsics (`Loop.ExpectVectorized()` and `Loop.ExpectNotVectorized()`) to express loop vectorization
assumptions, and have them validated at compile-time. For example, we can change the original `Bar` implementation to:

``` c#
[MethodImpl(MethodImplOptions.NoInlining)]
private static unsafe void Bar([NoAlias] int* a, [NoAlias] int* b, int count)
{
    for (var i = 0; i < count; i++)
    {
        Unity.Burst.CompilerServices.Loop.ExpectVectorized();

        a[i] += b[i];
    }
}
```

Burst will now validate, at compile-time, that the loop has indeed been vectorized. If the loop is not vectorized, Burst will
emit a compiler error. For example, if we do this:

``` c#
[MethodImpl(MethodImplOptions.NoInlining)]
private static unsafe void Bar([NoAlias] int* a, [NoAlias] int* b, int count)
{
    for (var i = 0; i < count; i++)
    {
        Unity.Burst.CompilerServices.Loop.ExpectVectorized();

        if (a[i] > b[i])
        {
            break;
        }

        a[i] += b[i];
    }
}
```

then Burst will emit the following error at compile-time:

```
LoopIntrinsics.cs(6,9): Burst error BC1321: The loop is not vectorized where it was expected that it is vectorized.
```

As these intrinsics are **experimental**, they need to be enabled with the `UNITY_BURST_EXPERIMENTAL_LOOP_INTRINSICS` preprocessor define.

> Note that these loop intrinsics should not be used inside `if` statements. Burst does not currently prevent this from happening, but in a future release this will be a compile-time error.

# Compiler Options

When compiling a job, you can change the behavior of the compiler:

- Using a different accuracy for the math functions (sin, cos...)
- Allowing the compiler to re-arrange the floating point calculations by relaxing the order of the math computations.
- Forcing a synchronous compilation of the Job (only for the Editor/JIT case)
- Using internal compiler options (not yet detailed)

These flags can be set through the `[BurstCompile]` attribute, for example `[BurstCompile(FloatPrecision.Med, FloatMode.Fast)]`

## FloatPrecision

The accuracy is defined by the following enumeration:

``` c#
    public enum FloatPrecision
    {
        /// <summary>
        /// Use the default target floating point precision - <see cref="FloatPrecision.Medium"/>.
        /// </summary>
        Standard = 0,
        /// <summary>
        /// Compute with an accuracy of 1 ULP - highly accurate, but increased runtime as a result, should not be required for most purposes.
        /// </summary>
        High = 1,
        /// <summary>
        /// Compute with an accuracy of 3.5 ULP - considered acceptable accuracy for most tasks.
        /// </summary>
        Medium = 2,
        /// <summary>
        /// Compute with an accuracy lower than or equal to <see cref="FloatPrecision.Medium"/>, with some range restrictions (defined per function).
        /// </summary>
        Low = 3,
    }
```

Currently, the implementation is only providing the following accuracy:

- `FloatPrecision.Standard` is equivalent to `FloatPrecision.Medium` providing an accuracy of 3.5 ULP. This is the **default value**.
- `FloatPrecision.High` provides an accuracy of 1.0 ULP.
- `FloatPrecision.Medium` provides an accuracy of 3.5 ULP.
- `FloatPrecision.Low` has an accuracy defined per function, and functions may specify a restricted range of valid inputs.

Using the `FloatPrecision.Standard` accuracy should be largely enough for most games.

An ULP (unit in the last place or unit of least precision) is the spacing between floating-point numbers, i.e., the value the least significant digit represents if it is 1.

Note: The `FloatPrecision` Enum was named `Accuracy` in early versions of the Burst API.

### FloatPrecision.Low

The following table describes the precision and range restrictions for using the `FloatPrecision.Low` mode. Any function **not** described in the table will inherit the ULP requirement from `FloatPrecision.Medium`.

<br/>
<table>
  <tr><th>Function</th><th>Precision</th><th>Range</th></tr>
  <tr><td>Unity.Mathematics.math.sin(x)</td><td>350.0 ULP</td><td></td></tr>
  <tr><td>Unity.Mathematics.math.cos(x)</td><td>350.0 ULP</td><td></td></tr>
  <tr><td>Unity.Mathematics.math.exp(x)</td><td>350.0 ULP</td><td></td></tr>
  <tr><td>Unity.Mathematics.math.exp2(x)</td><td>350.0 ULP</td><td></td></tr>
  <tr><td>Unity.Mathematics.math.exp10(x)</td><td>350.0 ULP</td><td></td></tr>
  <tr><td>Unity.Mathematics.math.log(x)</td><td>350.0 ULP</td><td></td></tr>
  <tr><td>Unity.Mathematics.math.log2(x)</td><td>350.0 ULP</td><td></td></tr>
  <tr><td>Unity.Mathematics.math.log10(x)</td><td>350.0 ULP</td><td></td></tr>
  <tr><td>Unity.Mathematics.math.pow(x, y)</td><td>350.0 ULP</td><td>Negative `x` to the power of a fractional `y` are not supported.</td></tr>
</table>

## Compiler floating point math mode

The compiler floating point math mode is defined by the following enumeration:

```c#
    /// <summary>
    /// Represents the floating point optimization mode for compilation.
    /// </summary>
    public enum FloatMode
    {
        /// <summary>
        /// Use the default target floating point mode - <see cref="FloatMode.Strict"/>.
        /// </summary>
        Default = 0,
        /// <summary>
        /// No floating point optimizations are performed.
        /// </summary>
        Strict = 1,
        /// <summary>
        /// Reserved for future.
        /// </summary>
        Deterministic = 2,
        /// <summary>
        /// Allows algebraically equivalent optimizations (which can alter the results of calculations), it implies :
        /// <para/> optimizations can assume results and arguments contain no NaNs or +/- Infinity and treat sign of zero as insignificant.
        /// <para/> optimizations can use reciprocals - 1/x * y  , instead of  y/x.
        /// <para/> optimizations can use fused instructions, e.g. madd.
        /// </summary>
        Fast = 3,
    }
```

- `FloatMode.Default` is defaulting to `FloatMode.Strict`
- `FloatMode.Strict`: The compiler is not performing any re-arrangement of the calculation and will be careful at respecting special floating point values (denormals, NaN...). This is the **default value**.
- `FloatMode.Fast`: The compiler can perform instruction re-arrangement and/or using dedicated/less precise hardware SIMD instructions.
- `FloatMode.Deterministic`: Reserved for future, when Burst will provide support for deterministic mode

Typically, some hardware can support Multiply and Add (e.g mad `a * b + c`) into a single instruction. These optimizations can be allowed by using the Fast calculation.
The reordering of these instructions can lead to a lower accuracy.

The `FloatMode.Fast` compiler floating point math mode can be used for many scenarios where the exact order of the calculation and the uniform handling of NaN values are not strictly required.

# Assume Intrinsics

Being able to tell the compiler that an integer lies within a certain range can open up optimization opportunities. The `AssumeRange` attribute allows users to tell the compiler that a given scalar-integer lies within a certain constrained range:

```c#
[return:AssumeRange(0u, 13u)]
static uint WithConstrainedRange([AssumeRange(0, 26)] int x)
{
    return (uint)x / 2u;
}
```

The above code makes two promises to the compiler:

- That the variable `x` is in the closed-interval range `[0..26]`, or more plainly that `x >= 0 && x <= 26`.
- That the return value from `WithConstrainedRange` is in the closed-interval range `[0..13]`, or more plainly that `x >= 0 && x <= 13`.

These assumptions are fed into the optimizer and allow for better codegen as a result. There are some restrictions:

- You can **only** place these on scalar-integer (signed or unsigned) types.
- The type of the range arguments **must match** the type being attributed.

We've also added in some deductions for the `.Length` property of `NativeArray` and `NativeSlice` to tell the optimizer that these always return non-negative integers.

```c#
static bool IsLengthNegative(NativeArray<float> na)
{
    // The compiler will always replace this with the constant false!
    return na.Length < 0;
}
```

Let's assume you have your own container:

```c#
struct MyContainer
{
    public int Length;
    
    // Some other data...
}
```

And you wanted to tell Burst that `Length` was always a positive integer. You would do that like so:

```c#
struct MyContainer
{
    private int _length;

    [return: AssumeRange(0, int.MaxValue)]
    private int LengthGetter()
    {
        return _length;
    }

    public int Length
    {
        get => LengthGetter();
        set => _length = value;
    }

    // Some other data...
}
```

# `Unity.Mathematics`

The `Unity.Mathematics` provides vector types (`float4`, `float3`...) that are directly mapped to hardware SIMD registers.

Also, many functions from the `math` type are also mapped directly to hardware SIMD instructions.

> Note that currently, for an optimal usage of this library, it is recommended to use SIMD 4 wide types (`float4`, `int4`, `bool4`...)

# Generic Jobs

As described in [AOT vs JIT](QuickStart.md#just-in-time-jit-vs-ahead-of-time-aot-compilation), there are currently two modes Burst will compile a Job:

- When in the Editor, it will compile the Job when it is scheduled (sometimes called JIT mode).
- When building a Standalone Player, it will compile the Job as part of the build player (AOT mode).

If the Job is a concrete type (not using generics), the Job will be compiled correctly in both modes.

In case of a generic Job, it can behave more unexpectedly.

While Burst supports generics, it has limited support for using generic Jobs or Function pointers. You could notice that a job scheduled at Editor time is running at full speed with Burst but not when used in a Standalone player. It is usually a problem related to generic Jobs.

A generic Job can be defined like this:

```c#
// Direct Generic Job
[BurstCompile]
struct MyGenericJob<TData> : IJob where TData : struct { 
    public void Execute() { ... }
}
```

or can be nested:

```c#
// Nested Generic Job
public class MyGenericSystem<TData> where TData : struct {
    [BurstCompile]
    struct MyGenericJob  : IJob { 
        public void Execute() { ... }
    }

    public void Run()
    {
        var myJob = new MyGenericJob(); // implicitly MyGenericSystem<TData>.MyGenericJob
        myJob.Schedule();    
    }
}
```

When the previous Jobs are being used like:

```c#
// Direct Generic Job
var myJob = new MyGenericJob<int>();
myJob.Schedule();

// Nested Generic Job
var myJobSystem = new MyGenericSystem<float>();
myJobSystem.Run();
```

In both cases in a standalone-player build, the Burst compiler will be able to detect that it has to compile `MyGenericJob<int>` and `MyGenericJob<float>` because the generic jobs (or the type surrounding it for the nested job) are used with fully resolved generic arguments (`int` and `float`).

But if these jobs are used indirectly through a generic parameter, the Burst compiler won't be able to detect the Jobs it has to compile at standalone-player build time:

```c#
public static void GenericJobSchedule<TData>() where TData: struct {
    // Generic argument: Generic Parameter TData
    // This Job won't be detected by the Burst Compiler at standalone-player build time.
    var job = new MyGenericJob<TData>();
    job.Schedule();
}

// The implicit MyGenericJob<int> will run at Editor time in full Burst speed
// but won't be detected at standalone-player build time.
GenericJobSchedule<int>();
```

Same restriction applies when declaring the Job in the context of generic parameter coming from a type:

```c#
// Generic Parameter TData
public class SuperJobSystem<TData>
{
    // Generic argument: Generic Parameter TData
    // This Job won't be detected by the Burst Compiler at standalone-player build time.
    public MyGenericJob<TData> MyJob;
}
```

> In summary, if you are using generic jobs, they need to be used directly with fully-resolved generic arguments (e.g `int`, `MyOtherStruct`), but can't be used with a generic parameter indirection (e.g `MyGenericJob<TContext>`).

Regarding function pointers, they are more restricted as you can't use a generic delegate through a function pointer with Burst:

```c#
public delegate void MyGenericDelegate<T>(ref TData data) where TData: struct;

var myGenericDelegate = new MyGenericDelegate<int>(MyIntDelegateImpl);
// Will fail to compile this function pointer.
var myGenericFunctionPointer = BurstCompiler.CompileFunctionPointer<MyGenericDelegate<int>>(myGenericDelegate);
```

This limitation is due to a limitation of the .NET runtime to interop with such delegates.

