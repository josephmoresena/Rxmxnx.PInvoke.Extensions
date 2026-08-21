# Target frameworks and public API surface

Version **2.9.5** is the current published surface: .NET Standard 2.1, .NET Core 3.0, and later, with the historical delegate and `IFixed*` helpers. The next major — likely 3.0 — keeps that surface, adds functional interfaces and value-type contexts on every TFM, and introduces **Reach**.

**Reach** is the compatibility extension that takes the package to **.NET Framework**, **.NET Standard 2.0**, and **UWP** (`uap10.0.16299`), plus .NET Core 2.1. Reach assemblies compile, pack, and run, but they do **not** ship the full 2.9.5 callback surface.

Officially supported for new work: **.NET 8.0 and later**. Everything else is compatibility.

## What the published package exposes

Delegate-based `WithSafeFixed` / `BufferManager.Alloc` overloads and the `IFixed*` fixed-memory interfaces are **public and supported** on the 2.9.5 TFMs. Prefer functional interfaces and `FixedContextValue<T>` in new code, especially if you also target a Reach TFM. The older overloads remain part of the public API on .NET Standard 2.1 / .NET Core 3.0 and later.

Those 2.9.5 helpers were **not** made retrocompatible with Reach: .NET Framework, .NET Standard 2.0, and UWP. .NET Core 2.1 shares the same narrower surface.

## Two public surfaces

Most of the difference is one preprocessor gate: `NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER`.

| Surface | TFMs | What you get |
| --- | --- | --- |
| **2.9.5** | `netstandard2.1`, `netcoreapp3.0`, `netcoreapp3.1`, `net5.0`–`net10.0` | Full 2.9.5 API **plus** value-type contexts and functional interfaces. |
| **Reach** | `netstandard2.0`, `netcoreapp2.1`, `net452`, `net46`, `net461`, `net462`, `net470`, `net471`, `net472`, `uap10.0.16299` | Value-type contexts and functional interfaces. Delegate pinning/buffer APIs and nested `IFixedMemory` / `IFixedContext<T>` `IDisposable` helpers were **not** brought over. |

`net470` and `net471` are listed as package targets so those projects restore cleanly. They do **not** ship their own `lib/` assemblies (`IncludeBuildOutput=false`); NuGet falls back to a nearby framework.

There is also a **.NET Framework 4.5** Mono path used with facades. It is not one of the Library.props TFMs.

## APIs on the 2.9.5 surface only

These types and overloads exist on .NET Standard 2.1, .NET Core 3.0, and later. They are absent from Reach (.NET Standard 2.0, .NET Core 2.1, .NET Framework, and UWP):

| Area | 2.9.5-only API |
| --- | --- |
| Pinning delegates | `FixedAction`, `ReadOnlyFixedAction`, `FixedFunc<TResult>`, `FixedContextAction<T>`, `ReadOnlyFixedContextAction<T>`, `FixedReferenceAction<T>`, `FixedMethodAction<TDelegate>`, `FixedListAction`, and the corresponding `Func` variants, including stateful `TArg` overloads |
| Buffer delegates | `ScopedBufferAction<T>`, `ScopedBufferFunc<T, TResult>`, and stateful variants; `BufferManager.Alloc(... delegate ...)` |
| UTF-8 pinning (instance) | `CString.WithSafeFixed(ReadOnlyFixedAction)` and related instance overloads; `CStringSequence.WithSafeFixed(ReadOnlyFixedListAction)` |
| String pinning (delegate) | `StringExtensions.WithSafeFixed` delegate overloads |
| Nested disposables | `IFixedMemory.IDisposable`, `IFixedContext<T>.IDisposable`, `IReadOnlyFixedMemory.IDisposable`, `IReadOnlyFixedContext<T>.IDisposable`, `IFixedReference<T>.IDisposable`, `IReadOnlyFixedReference<T>.IDisposable` |
| Interface-returning helpers | `Memory<T>.GetFixedContext()` → `IFixedContext<T>.IDisposable`; `NativeUtilities.HeapAlloc<T>(count)` → `IFixedContext<T>.IDisposable`; `GetValuesFixedContext<TEnum>()` without an `out` context |
| Lists | `FixedMemoryList`, `ReadOnlyFixedMemoryList` (use `FixedPointerValueList` on Reach) |
| Wrapper factories | Non-generic `IWrapper` / `IMutableWrapper` / `IMutableReference` / `IReferenceableWrapper` with static `Create*` methods (default interface methods). Use `WrapperFactory` instead. |
| UTF-8 hashing | `CString.GetHashCode(ReadOnlySpan<Byte>)` |
| Native libraries | `NativeUtilities.LoadNativeLib`, `GetNativeMethod<TDelegate>` (.NET Core 3.0+ only, not on .NET Standard 2.1) |

The `IFixed*` interfaces themselves exist on every TFM. What Reach omits is the **helpers that return nested `IDisposable` contexts** and the **delegate types that take those interfaces**. On Reach TFMs, call:

```csharp
utf8.WithSafeFixed(new PrintUtf8());                              // IFixedContextAction<Byte>
using IDisposable pin = mem.GetFixedContext(out FixedContextValue<Byte> ctx);
using IDisposable heap = NativeUtilities.HeapAlloc<Byte>(64, out FixedContextValue<Byte> buffer);
BufferManager<String>.Alloc(new CollectNames());                  // IScopedBufferAction<String>
IWrapper<Int32> boxed = WrapperFactory.Create(42);
```

## APIs that exist on both surfaces

These are available from .NET Standard 2.0 / .NET Framework / UWP through .NET 10:

- `CString`, `CStringSequence`, `CStringBuilder` (JSON converter attribute: .NET Core or .NET Framework 4.6.1+)
- `ValPtr<T>`, `ReadOnlyValPtr<T>`, `FuncPtr<TDelegate>`
- `FixedPointerValue`, `FixedContextValue<T>`, `ReadOnlyFixedContextValue<T>`, `FixedPointerValueList`
- `IFixedPointer`, `IFixedMemory`, `IFixedContext<T>`, `IFixedReference<T>`, `IFixedMethod<TDelegate>` (and the `IReadOnly*` counterparts). Nested `IDisposable` on `IFixedPointer` / `IFixedMethod<TDelegate>` exists on every TFM; nested `IDisposable` on memory, context, and reference types does not.
- Functional interfaces: `IFixedAction`, `IFixedContextAction<T>`, `IReadOnlyFixedContextAction<T>`, `IFixedPointerListAction`, `IScopedBufferAction<T>`, and the function counterparts. `Accept` / `Apply` take `FixedContextValue<T>` / `FixedPointerValue` / `ScopedBuffer<T>` on every TFM.
- `WithSafeFixed` / `GetFixedContext` / `GetFixedMemory` overloads that take a functional interface or an `out` value context
- `BufferManager<T>.Alloc` / `AllocWithReference` with functional interfaces; `BufferManager.VisualBasic` (every TFM)
- `NativeUtilities.HeapAlloc<T>(count, out FixedContextValue<T>)`
- `IWrapper<T>`, `ValueRegion<T>`, `AotInfo`, `SystemInfo`

`IScopedBufferAction<T>.IsMinimalCount` is a required property on Reach. On the 2.9.5 surface it has a default of `false`. The same pattern applies to a few other members that are default interface methods on 2.9.5 and required on Reach: `IFixedMemory<T>.ValuePointer`, `IReadOnlyFixedMemory<T>.ValuePointer`, and `IFixedReference<T>.Transformation<TDestination>()` without a residual. Callers do not need to distinguish those; only custom interface implementations do.

## Other TFM differences (all surfaces)

| Gate | Effect |
| --- | --- |
| .NET Core 3.0+ | `NativeLibrary` helpers; `System.Text.Json` on Core (3.0 uses 5.0.2, 3.1+ uses later versions) |
| .NET 5+ | Native `Enum.GetName<T>`, `Convert.ToHexString` |
| .NET 7+ | `[NativeMarshalling]` for `CString`, `CStringSequence`, `ValPtr<T>`, `ReadOnlyValPtr<T>`, `FuncPtr<TDelegate>`; `IUtf8FunctionState<TSelf>`; `IParsable` on pointers |
| .NET 8+ | No extra package dependencies; buffer storage feature switches |
| .NET 9+ | `allows ref struct` on many generics; `params ReadOnlySpan<T>`; some pointer helpers patched in IL |
| .NET Core **or** net461+ | `[JsonConverter]` on `CString` / `CStringSequence`. .NET Standard 2.0/2.1 and net452/net46 do not get the built-in converter. Classic Mono can use [`Rxmxnx.PInvoke.Json`](../../src/MonoFacades/README.md). |
| .NET Standard 2.0 / netfx / UWP | `System.Memory` (or `Microsoft.Bcl.Memory` on net462+ and UWP); shims for `MemoryMarshal`, `Rune`, `Enum.GetName`, `Convert.ToHexString` where the runtime does not have them |

## Dependencies by TFM family

From the package’s own `Packages.props`:

| Family | TFMs | Extra dependencies |
| --- | --- | --- |
| Limited portable | `netstandard2.0`, `net452`, `net46` | `System.Memory` 4.5.5, `System.Runtime.CompilerServices.Unsafe` 5.0, plus `System.ValueTuple` / `RuntimeInformation` on netfx, `System.Reflection.Emit.Lightweight` on netstandard2.0 |
| Limited native | `netstandard2.1` | `System.Runtime.CompilerServices.Unsafe` 5.0 |
| Limited legacy | `netcoreapp2.1`, `netcoreapp3.0` | `System.Text.Json` 5.0.2, `Unsafe` 5.0; netcoreapp2.1 also pins `Microsoft.NETCore.App` 2.1.30 |
| Legacy | `netcoreapp3.1`, `net5.0`, `net461` | `Unsafe` 6.0, `System.Text.Json` 6.0.11; net461 adds `Microsoft.Bcl.AsyncInterfaces`, `System.Memory`, `ValueTuple` |
| Extended | `net6.0`, `net7.0` | `Unsafe` 6.1.2 on net6.0; `System.Text.Json` 8.0.6 on both |
| Current | `net8.0`, `net9.0`, `net10.0` | None |
| Framework support | `net462`, `net470`, `net471`, `net472` | `Microsoft.Bcl.Memory` 10.0.11, `Microsoft.Bcl.HashCode` 6.0.0, `System.Text.Json` 10.0.11 |
| UWP | `uap10.0.16299` | `Microsoft.Bcl.Memory` 9.0.19, `Microsoft.Bcl.HashCode` 6.0.0, `System.Text.Json` 6.0.11, UWP compiler packs (private) |

On Mono with a .NET Framework TFM, `System.Runtime.CompilerServices.Unsafe` 5.0 is referenced and runtime assets are excluded for libraries.

## Choosing an API when you target several TFMs

1. If every TFM is .NET Standard 2.1 / .NET Core 3.0 or later, either callback style compiles. Functional interfaces still avoid an extra heap delegate.
2. If any TFM is a Reach TFM (.NET Standard 2.0, .NET Core 2.1, .NET Framework, or UWP), write to the **Reach** surface: functional interfaces, `out FixedContextValue<T>`, `WrapperFactory`.
3. Delegate overloads remain valid on the 2.9.5 surface. Use them when you already have that call site; do not `#if` them into a multi-target project that includes a Reach TFM.

See [Getting started](../getting-started.md) for install and AOT notes, and [Functional interfaces](functional-interfaces.md) for the callback contracts.
