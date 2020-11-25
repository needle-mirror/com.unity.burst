# Language Support

Burst supports most of the expressions and statements:

- Regular C# control flows:
  - `if`/`else`/`switch`/`case`/`for`/`while`/`break`/`continue`.
- Extension methods.
- Unsafe code, pointer manipulation...etc.
- Instance methods of structs.
- By ref/out parameters.
- [`DllImport` and internal calls](CSharpLanguageSupport_BurstIntrinsics.md#dllimport-and-internal-calls).
- Limited support for `throw` expressions, assuming simple throw patterns (e.g `throw new ArgumentException("Invalid argument")`). In that case, we will try to extract the static string exception message to include it in the generated code.
- Some special IL opcodes like `cpblk`, `initblk`, `sizeof`.
- Loading from static readonly fields.
- Support for `fixed` statements.
- Support for `try`/`finally` and associated IDisposable patterns (`using` and `foreach`)
  - Note that if an exception occurs, the behavior will differ from .NET. In .NET, if an exception occurs inside a `try` block, control flow would go to the `finally` block. In Burst, if an exception occurs whether inside or outside a `try` block, the currently running job or function pointer will terminate immediately.
  - [Partial support for strings and `Debug.Log`](#partial-support-for-strings-and-debuglog).

Burst also provides alternatives for some C# constructions not directly accessible to HPC#:

- [Function pointers](AdvancedUsages.md#function-pointers) as an alternative to using delegates within HPC#
- [Shared Static](AdvancedUsages.md#shared-static) to access static mutable data from both C# and HPC#

Burst does not support:

- Catching exceptions `catch` in a `try`/`catch`.
- Storing to static fields except via [Shared Static](AdvancedUsages.md#shared-static)
- Any methods related to managed objects (e.g string methods...etc.)

## Throw and Exceptions

Burst supports `throw` expressions for exceptions in the Editor (`2019.3+` only), but crucially **does not** in Standalone Player builds. Exceptions in Burst are to be used solely for _exceptional_ behavior. To ensure that code does not end up relying on exceptions for things like general control flow, Burst will produce the following warning on code that tries to `throw` within a method not attributed by `[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]`:

> Burst warning BC1370: An exception was thrown from a function without the correct [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")] guard. Exceptions only work in the editor and so should be protected by this guard

## Partial support for strings and `Debug.Log`

Burst provides partial support for using strings in the following two main scenarios:

- `Debug.Log` (see below)
- Assignment of a string to various FixedString structs provided by `Unity.Collections` (e.g `FixedString128`)

A string can be either:
-  A string literal (e.g `"This is a string literal"`).
-  An interpolated string (using `$"This is an integer {value}` or using `string.Format`) where the string to format is also a string literal

For example, the following constructions are supported:

- Logging with a string literal:

```c#
Debug.Log("This a string literal");
```

- Logging using string interpolation:

```c#
int value = 256;
Debug.Log($"This is an integer value {value}"); 
```

Which is equivalent to using `string.Format` directly:

```c#
int value = 256;
Debug.Log(string.Format("This is an integer value {0}", value));
```

While it is possible to pass a managed `string` literal or an interpolated string directly to `Debug.Log`, it is not possible to pass a string to a user method or to use them as fields in a struct. In order to pass or store strings around, you need to use one of the `FixedString` structs provided by the `Unity.Collections` package:

```c#
int value = 256;
FixedString128 text = $"This is an integer value {value} used with FixedString128";
MyCustomLog(text);

// ...

// String can be passed as an argument to a method using a FixedString, 
// but not using directly a managed `string`:
public static void MyCustomLog(in FixedString128 log)
{
    Debug.Log(text);
}
```

Burst has limited support for string format arguments and specifiers:

```c#
int value = 256;

// Padding left: "This value `  256`
Debug.Log($"This value `{value,5}`");

// Padding right: "This value `256  `
Debug.Log($"This value `{value,-5}`");

// Hexadecimal uppercase: "This value `00FF`
Debug.Log($"This value `{value:X4}`");

// Hexadecimal lowercase: "This value `00ff`
Debug.Log($"This value `{value:x4}`");

// Decimal with leading-zero: "This value `0256`
Debug.Log($"This value `{value:D4}`");
```

What is supported currently:

- The following `Debug.Log` methods:
  - `Debug.Log(object)`
  - `Debug.LogWarning(object)`
  - `Debug.LogError(object)`
- String interpolation is working with the following caveats:
  - The string to format must be a literal.
  - Only the following `string.Format` methods are supported:
    - `string.Format(string, object)`, `string.Format(string, object, object)`, `string.Format(string, object, object, object)` and more if the .NET API provides specializations with object arguments.
    - `string.Format(string, object[])`: which can happen for a string interpolation that would contain more than 3 arguments (e.g `$"{arg1} {arg2} {arg3} {arg4} {arg5}..."`). In that case, we expect the object[] array to be of a constant size and no arguments should involve control flows (e.g `$"This is a {(cond ? arg1 : arg2)}"`)
  - Only value types.
  - Only primitive type arguments: `char`, `boolean`, `byte`, `sbyte`, `ushort`, `short`, `uint`, `int`, `ulong`, `long`, `float`, `double`.
  - All vector types e.g `int2`, `float3` are supported, except `half` vector types.
    ```c#
    var value = new float3(1.0f, 2.0f, 3.0f);
    // Logs "This value float3(1f, 2f, 3f)"
    Debug.Log($"This value `{value}`");
    ```
  - Does not support `ToString()` of structs. It will display the full name of the struct instead.
- The supported string format specifiers are only `G`, `g`, `D`, `d` and `X`, `x`, with padding and specifier. For more details:
  - See the [.NET documentation - string interpolation](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated).
  - See the [.NET documentation - Standard numeric format strings](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings).