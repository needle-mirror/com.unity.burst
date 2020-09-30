# Known Issues

- The maximum target CPU is currently hardcoded per platform. For standalone builds that target desktop platforms (Windows/Linux/macOS) you can choose the supported targets via [Burst AOT Settings](docs/StandalonePlayerSupport.md#burst-aot-settings). For other platforms see the table above.
- Building iOS player from Windows will not use Burst, (see [Burst AOT Requirements](docs/StandalonePlayerSupport.md#burst-aot-requirements))
- Building Android player from Linux will not use Burst, (see [Burst AOT Requirements](docs/StandalonePlayerSupport.md#burst-aot-requirements))
- If you change the burst package version (via update for example), you need to close and restart the editor. A warning is now displayed to this effect.
- Function pointers are not working in playmode tests before 2019.3. The feature will be backported.
- Struct with explicit layout can generate non optimal native code
- `BurstCompiler.SetExecutionMode` does not affect the runtime yet for deterministic mode
- Burst does not support scheduling generic Jobs through generic methods. While this would work in the editor, it will not work in the standalone player.
- Output of Debug.Log is temporarily disabled when used in Burst Function Pointers/Jobs to avoid a deadlock on a domain reload.

Some of these issues may be resolved in a future release of Burst.

## Known issues with `DllImport`

- `DllImport` is not available on 32-bit or Arm platforms, with the exception that `DllImport("__Internal")` *is* supported on statically linked platforms (iOS).
- `DllImport` is only supported for [native plugins](https://docs.unity3d.com/Manual/NativePlugins.html), not platform-dependent libraries like `kernel32.dll`.

## Known issues with debugging/profiling

- Lambda captures on `Entity.ForEach()` are not discovered for debugging data, so you won't be able to inspect variables originating from these.
- Structs that utilize `LayoutKind=Explicit`, and have overlapping fields, are represented by a struct that will hide one of the overlaps. In the future they will be represented as a union of structs, to allow inspection of fields that overlap.
- Function parameters are currently readonly from a debugging point of view. They are recorded to a stack argument during the prologue. Changing their value in the debugger may not have an affect.
- Due to the way we build code for standalone players, you will need to instruct the profiling/debugging tool where to locate the symbols (Point the tool to the folder containing the lib_burst_generated files (usually Plugins))
