# Utilities

Static helpers that do not belong to a single value: native heap, native libraries, transforms, AOT/OS detection, culture.

## `NativeUtilities`

Interop glue. `PointerSize` is `sizeof(IntPtr)`.

### Native libraries (.NET Core 3.0+)

| Method | Role |
| --- | --- |
| `LoadNativeLib(String?, DllImportSearchPath?)` | Load a library. |
| `LoadNativeLib(String?, ref EventHandler?, …)` | Same, with an unload event. |
| `GetNativeMethod<TDelegate>(IntPtr, String?)` | Exported symbol as a managed delegate. |
| `GetNativeMethodPtr<TDelegate>(IntPtr, String?)` | Same export as `FuncPtr<TDelegate>`. |

### Addresses

| Method | Result |
| --- | --- |
| `GetUnsafeFuncPtr<TDelegate>(TDelegate)` | Function pointer to a managed delegate. |
| `GetUnsafeValPtr<T>(in T)` | `ReadOnlyValPtr<T>` |
| `GetUnsafeValPtrFromRef<T>(ref T)` | `ValPtr<T>` |
| `GetUnsafeIntPtr<T>(in T)` / `GetUnsafeUIntPtr<T>(in T)` | Untyped address of an unmanaged value |

From .NET 9.0, `T` on the `ValPtr` helpers may be a `ref struct`.

### Layout

| Method | Result |
| --- | --- |
| `SizeOf<T>()` | Size of an unmanaged `T`. |
| `Transform<TSource, TDestination>(in TSource)` | `ref readonly TDestination` over the same bytes. |
| `TransformReference<TSource, TDestination>(ref TSource)` | Mutable variant. |
| `AsBytes<TSource>(in TSource)` | `ReadOnlySpan<Byte>` over a value. |
| `AsBinarySpan<TSource>(ref TSource)` | `Span<Byte>` over a value. |
| `ToBytes<TSource>(in TSource)` | New `Byte[]` snapshot. |
| `CopyBytes<TSource>(in TSource, Span<Byte>, Int32)` | Copy into an existing span. |
| `CreateArray<T, TState>(Int32, TState, SpanAction<T, TState>)` | Allocate an unmanaged array and initialize it in place. |

### Fast vs slow span

Both keys ask the same kind of question: do I take **fast span** and the **runtime’s own optimizations**, or do I keep **my own / older code**? They apply at different moments.

`SystemInfo.UsesNativeSpan` is the process fact: is this runtime on the built-in two-field layout? Use it when you still choose how to work. Array versus span is the usual case — `true` means a `Span<T>` hot path can use native span; `false` (typical desktop .NET Framework) means your existing array-based or older path may be the one that pays off. See [span efficiency](compatibility.md#span-efficiency).

`TryCreateSpan` / `TryCreateReadOnlySpan` are a later moment: you **already have a `ref` / `in`** to a regular managed object. `true` is the fast-span view (`MemoryMarshal.CreateSpan` on modern TFMs). `false` means stay on whatever you already wrote for that ref. The library does not pick the `else`.

| Method | Role |
| --- | --- |
| `TryCreateSpan<T>(ref T, Int32, out Span<T>)` | Fast-span view over a mutable `ref`, or `false`. |
| `TryCreateReadOnlySpan<T>(in T, Int32, out ReadOnlySpan<T>)` | Same for `in` / `ref readonly`. |

Success is always on .NET Standard 2.1 / .NET Core 2.1+; on Framework / UWP only when the **executing** runtime is already native span (Mono hosting a Framework TFM, modern UWP). Recipe: [use case](../use-cases.md#take-a-fast-span-from-a-ref-or-keep-your-own-path).

### Enums and cultures

| Member | Role |
| --- | --- |
| `GetEnumValuesSpan<TEnum>()` | Span over the enum’s values. |
| `GetEnumNamesSpan<TEnum>()` | Span over the enum’s names. |
| `GetValuesFixedContext<TEnum>(out ReadOnlyFixedContextValue<TEnum>)` | Pin those values for a native call. |
| `GetIso639P1(CultureInfo)` | Map a culture to [`Iso639P1`](enums.md). |
| `UserInterfaceIso639P1` | Current UI culture as `Iso639P1`. |
| `GlobalizationInvariantModeEnabled` | Whether invariant globalization is on (also a trimming hint). |

### Lifetime

| Method | Role |
| --- | --- |
| `GetFixedMethod<TDelegate>(TDelegate?)` | Marshalled method with `IDisposable` lifetime. |
| `HeapAlloc<T>(Int32)` | Native heap as `IFixedContext<T>.IDisposable` — **.NET Standard 2.1 / .NET Core 3.0+**. |
| `HeapAlloc<T>(Int32, out FixedContextValue<T>)` | Native heap as a value context plus `IDisposable` — every TFM. |

`T` on `HeapAlloc` is `unmanaged`. Count `0` yields an empty/null context. Negative counts throw.

## `AotInfo`

Facts about ahead-of-time compilation.

| Property | Meaning |
| --- | --- |
| `IsReflectionDisabled` | Runtime reflection is off. |
| `IsCodeGenerationSupported` | Dynamic IL emission is allowed. |
| `IsPlatformTrimmed` | The runtime is trimmed for this platform. From .NET 5.0 this helps the linker drop unreachable code. |
| `IsNativeAot` | The process is Native AOT. |

`IsNativeAot` details:

- On CoreCLR (R2R, Native AOT, Native AOT-LLVM) detection is reliable and does not inspect process maps.
- On Mono it may depend on when the property is first read, because Mono AOT does not compile the whole assembly at once.
- On Blazor WebAssembly it is generally not possible to distinguish AOT from JIT; the property may only be `true` when IL generation is disallowed.
- On mobile XNU platforms the property is treated as `true`.
- From .NET 6.0, reading it helps the linker on desktop and mobile XNU.

On .NET 5.0 and earlier, AOT detection uses reflection. On .NET 7.0+ it still may on web and mobile.

## `SystemInfo`

Facts about the runtime and OS. Several properties are written so the trimmer can delete the branches you do not take.

| Property | Meaning |
| --- | --- |
| `IsMonoRuntime` | The runtime is Mono. |
| `IsWebRuntime` | The runtime is Web (from .NET 8.0 this enables trimming). |
| `UsesNativeSpan` | Whether the process uses the built-in (fast, two-field) `Span<T>` layout. Always `true` on .NET Standard 2.1 / .NET Core 2.1+. On desktop .NET Framework this is typically `false`. Fast span (runtime optimizations) versus your own / older path — usually array versus span. |
| `IsWindows` / `IsLinux` / `IsMac` / `IsFreeBsd` / `IsNetBsd` / `IsSolaris` | OS. Windows/Linux/FreeBSD hint the trimmer from .NET 5.0; macOS from .NET 6.0. |

`IsOsPlatform(String?)` and params/span overloads test one or more platform names. From .NET 9.0, `params` is `ReadOnlySpan<String?>`.

## See also

- [Getting started: AOT](../getting-started.md#aot-support)
- [Use case: AOT branching](../use-cases.md#adapt-behavior-for-native-aot-or-mono)
- [Use case: HeapAlloc](../use-cases.md#allocate-native-memory-and-free-it-with-dispose)
- [Use case: fast span from a ref](../use-cases.md#take-a-fast-span-from-a-ref-or-keep-your-own-path)
- [TFM / API surface](compatibility.md)
