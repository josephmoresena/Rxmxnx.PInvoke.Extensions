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

`MemoryMarshal.CreateSpan` / `CreateReadOnlySpan` assume the **fast** (native, two-field) `Span<T>` layout. Desktop .NET Framework plus `System.Memory` often uses the **slow** (three-field: pinnable + offset + length) layout. A two-field view built from a raw address is the wrong fallback there.

`TryCreateSpan` / `TryCreateReadOnlySpan` are the transition helpers. They compile on every TFM and **separate** the two runtimes at the call site:

| Method | Role |
| --- | --- |
| `TryCreateSpan<T>(ref T, Int32, out Span<T>)` | Fast-span view over a mutable reference. |
| `TryCreateReadOnlySpan<T>(in T, Int32, out ReadOnlySpan<T>)` | Fast-span view over a read-only reference. |

They return `true` and fill the `out` span when the **executing** runtime can host that view:

- Always on .NET Standard 2.1 / .NET Core 2.1 and later (`MemoryMarshal.CreateSpan` / `CreateReadOnlySpan`).
- On .NET Framework and UWP, only when the process is already on native span (Mono hosting a Framework TFM, modern UWP). Desktop CLR typically returns `false` and a default span.

Prefer these over reading `SystemInfo.UsesNativeSpan` and then calling `MemoryMarshal` yourself:

- One call site. You do not `#if` around `CreateSpan` or around `CreateReadOnlySpan(in T)` (.NET 8.0+ only).
- Success **is** the fast path: the span is already in the `out` argument.
- Failure **is** the slow path: keep array copies, pinning helpers, or other APIs that already understand three-field span. Do not invent a two-field span from a raw address.

`UsesNativeSpan` stays useful for diagnostics, logging, and a one-time strategy. It is not the API to thread through every view. See [span efficiency](compatibility.md#span-efficiency) and the [use case](../use-cases.md#create-a-span-only-when-the-runtime-is-fast-span).

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
| `UsesNativeSpan` | Whether the process uses the built-in (fast, two-field) `Span<T>` layout. Always `true` on .NET Standard 2.1 / .NET Core 2.1+. On desktop .NET Framework this is typically `false`. A fact about the process — prefer `TryCreateSpan` / `TryCreateReadOnlySpan` when you need a span, not a boolean. |
| `IsWindows` / `IsLinux` / `IsMac` / `IsFreeBsd` / `IsNetBsd` / `IsSolaris` | OS. Windows/Linux/FreeBSD hint the trimmer from .NET 5.0; macOS from .NET 6.0. |

`IsOsPlatform(String?)` and params/span overloads test one or more platform names. From .NET 9.0, `params` is `ReadOnlySpan<String?>`.

## See also

- [Getting started: AOT](../getting-started.md#aot-support)
- [Use case: AOT branching](../use-cases.md#adapt-behavior-for-native-aot-or-mono)
- [Use case: HeapAlloc](../use-cases.md#allocate-native-memory-and-free-it-with-dispose)
- [Use case: TryCreateSpan](../use-cases.md#create-a-span-only-when-the-runtime-is-fast-span)
- [TFM / API surface](compatibility.md)
