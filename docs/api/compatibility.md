# Target frameworks and public API surface

**2.9.5** is the current published package: .NET Standard 2.1, .NET Core 3.0, and later, including the historical delegate and `IFixed*` helpers.

The next major — likely 3.0 — is the same library after more of that surface has matured: functional interfaces and value-type contexts on every TFM, plus assemblies that let you **move between frameworks and runtimes** without dropping the package. Those extra targets do **not** replay the 2.9.5 delegate helpers; they were never made retrocompatible with .NET Framework, .NET Standard 2.0, or UWP.

Officially supported for new work: **.NET 8.0 and later**. Everything else exists so a project can keep compiling while it changes TFM or runtime.

## Why the extra targets exist

They are not a branded add-on. They are what the package can ship once the implementation is mature enough to honor each TFM’s **declared surface** and still use what the **executing runtime** actually provides.

| Kind of assembly | TFMs | Role |
| --- | --- | --- |
| Portable | `netstandard2.1`, `netstandard2.0` | One binary for **Xamarin, Unity, and Mono**. 2.1 is the 2.9.5 portable target; 2.0 is the same hosts when the player or SDK is still on Standard 2.0. |
| Dedicated .NET / .NET Core | `netcoreapp2.1`, `netcoreapp3.0`–`net10.0` | Own `lib/` per TFM. |
| Dedicated UWP | `uap10.0.16299` | Own binary, aimed at the UWP runtime — including **fast span**, even when the public `Span<T>` API does not advertise it. |
| Dedicated .NET Framework | `net452`, `net46`, `net461`, `net462`, `net472` (`net470` / `net471` restore only) | Own binaries. The public API stays within that TFM. Fast-span operations run when the **runtime** has them — typically **Mono** hosting a netfx app. |

`net470` and `net471` are listed as package targets so those projects restore. They do **not** ship their own `lib/` (`IncludeBuildOutput=false`); NuGet falls back to a nearby framework.

There is also a **.NET Framework 4.5** Mono path used with facades. It is not a Library.props TFM.

## Transition along .NET Framework

.NET Framework 4.5.2 and 4.6 sit **before** that product line implemented .NET Standard 2.0. Those two assemblies are a **transition** step: they keep you on the package while you move off classic Framework, and they **do not** take a native `System.Text.Json` dependency (so they do not pull Standard 2.0 JSON into a 4.5-era Mono story).

.NET Framework **4.6.1** already accepts `System.Text.Json`. Its public extras line up more closely with the **.NET Standard 2.0** package (JSON converter on `CString` / `CStringSequence`, later memory/hash packages from 4.6.2). From there, net462+ are full dedicated Framework binaries (`Microsoft.Bcl.Memory`, current JSON).

.NET Standard **2.0 and 2.1** stay portable on purpose: Xamarin, Unity, and Mono should not be forced onto a netfx or UWP `lib/`. The core package still avoids referencing Standard 2.0-only JSON from the 2.1 assembly, because mixing those two on classic **Mono Framework** breaks the 4.5 compatibility model. Use [`Rxmxnx.PInvoke.Json`](../../src/MonoFacades/README.md) when you need JSON there.

## Span layout vs TFM surface

`Span<T>` on paper is not always `Span<T>` at runtime.

- **Fast span** is the compact two-field layout (byref + length). UWP Fall Creators and Mono with Standard 2.1-class runtimes use it even if the compile-time reference looks like `System.Memory`.
- **Slow span** is the three-field layout (pinnable object + offset + length) used by desktop .NET Framework plus `System.Memory` 4.5.x.

The netfx and UWP assemblies compile to their TFM contracts, then detect the runtime layout. If the process is Mono executing a netfx TFM — or UWP with fast span — casts and span creation follow the fast path. The public API does not change; the operations get cheaper when the runtime allows it.

That is the same idea as the rest of the package: **help the transition** between frameworks *and* between runtimes, without pretending every host is CoreCLR.

## Two public API surfaces (package versions)

Most of the callback split is one preprocessor gate: `NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER`. That is the 2.9.5 set of targets.

| Surface | TFMs | What you get |
| --- | --- | --- |
| **2.9.5** | `netstandard2.1`, `netcoreapp3.0`, `netcoreapp3.1`, `net5.0`–`net10.0` | Full 2.9.5 API **plus** value-type contexts and functional interfaces. |
| **After 2.9.5** | `netstandard2.0`, `netcoreapp2.1`, `net452`, `net46`, `net461`, `net462`, `net470`, `net471`, `net472`, `uap10.0.16299` | Value-type contexts and functional interfaces. Delegate pinning/buffer APIs and nested `IFixedMemory` / `IFixedContext<T>` `IDisposable` helpers were **not** brought over. |

Delegate `WithSafeFixed` / `BufferManager.Alloc` overloads and the `IFixed*` interfaces remain **public and supported** on 2.9.5 targets. Prefer functional interfaces and `FixedContextValue<T>` in new code if you also multi-target a TFM added after 2.9.5.

## APIs on 2.9.5 targets only

These types and overloads exist on .NET Standard 2.1, .NET Core 3.0, and later. They are absent from .NET Standard 2.0, .NET Core 2.1, .NET Framework, and UWP:

| Area | 2.9.5-only API |
| --- | --- |
| Pinning delegates | `FixedAction`, `ReadOnlyFixedAction`, `FixedFunc<TResult>`, `FixedContextAction<T>`, `ReadOnlyFixedContextAction<T>`, `FixedReferenceAction<T>`, `FixedMethodAction<TDelegate>`, `FixedListAction`, and the corresponding `Func` variants, including stateful `TArg` overloads |
| Buffer delegates | `ScopedBufferAction<T>`, `ScopedBufferFunc<T, TResult>`, and stateful variants; `BufferManager.Alloc(... delegate ...)` |
| UTF-8 pinning (instance) | `CString.WithSafeFixed(ReadOnlyFixedAction)` and related instance overloads; `CStringSequence.WithSafeFixed(ReadOnlyFixedListAction)` |
| String pinning (delegate) | `StringExtensions.WithSafeFixed` delegate overloads |
| Nested disposables | `IFixedMemory.IDisposable`, `IFixedContext<T>.IDisposable`, `IReadOnlyFixedMemory.IDisposable`, `IReadOnlyFixedContext<T>.IDisposable`, `IFixedReference<T>.IDisposable`, `IReadOnlyFixedReference<T>.IDisposable` |
| Interface-returning helpers | `Memory<T>.GetFixedContext()` → `IFixedContext<T>.IDisposable`; `NativeUtilities.HeapAlloc<T>(count)` → `IFixedContext<T>.IDisposable`; `GetValuesFixedContext<TEnum>()` without an `out` context |
| Lists | `FixedMemoryList`, `ReadOnlyFixedMemoryList` (use `FixedPointerValueList` on the later TFMs) |
| Wrapper factories | Non-generic `IWrapper` / `IMutableWrapper` / `IMutableReference` / `IReferenceableWrapper` with static `Create*` methods (default interface methods). Use `WrapperFactory` instead. |
| UTF-8 hashing | `CString.GetHashCode(ReadOnlySpan<Byte>)` |
| Native libraries | `NativeUtilities.LoadNativeLib`, `GetNativeMethod<TDelegate>` (.NET Core 3.0+ only, not on .NET Standard 2.1) |

The `IFixed*` interfaces themselves exist on every TFM. What the later assemblies omit is the **helpers that return nested `IDisposable` contexts** and the **delegate types that take those interfaces**. On those TFMs, call:

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

`IScopedBufferAction<T>.IsMinimalCount` is a required property on TFMs added after 2.9.5. On 2.9.5 targets it has a default of `false`. The same pattern applies to a few other members that are default interface methods on 2.9.5 and required later: `IFixedMemory<T>.ValuePointer`, `IReadOnlyFixedMemory<T>.ValuePointer`, and `IFixedReference<T>.Transformation<TDestination>()` without a residual. Callers do not need to distinguish those; only custom interface implementations do.

## Other TFM differences

| Gate | Effect |
| --- | --- |
| .NET Core 3.0+ | `NativeLibrary` helpers; `System.Text.Json` on Core (3.0 uses 5.0.2, 3.1+ uses later versions) |
| .NET 5+ | Native `Enum.GetName<T>`, `Convert.ToHexString` |
| .NET 7+ | `[NativeMarshalling]` for `CString`, `CStringSequence`, `ValPtr<T>`, `ReadOnlyValPtr<T>`, `FuncPtr<TDelegate>`; `IUtf8FunctionState<TSelf>`; `IParsable` on pointers |
| .NET 8+ | No extra package dependencies; buffer storage feature switches |
| .NET 9+ | `allows ref struct` on many generics; `params ReadOnlySpan<T>`; some pointer helpers patched in IL |
| .NET Core **or** net461+ | `[JsonConverter]` on `CString` / `CStringSequence`. Not on .NET Standard 2.0/2.1 (portable Mono/Xamarin/Unity story) or on the net452/net46 transition assemblies. Classic Mono can use [`Rxmxnx.PInvoke.Json`](../../src/MonoFacades/README.md). |
| netfx / UWP dedicated binaries | `System.Memory` or `Microsoft.Bcl.Memory`; span helpers adapt to fast vs slow layout at runtime |

## Dependencies by TFM family

From the package’s own `Packages.props`:

| Family | TFMs | Extra dependencies |
| --- | --- | --- |
| Transition netfx | `net452`, `net46` | `System.Memory` 4.5.5, `Unsafe` 5.0, `RuntimeInformation` 4.3.0, `ValueTuple` 4.5.0 — no `System.Text.Json` |
| Portable 2.0 | `netstandard2.0` | `System.Memory` 4.5.5, `Unsafe` 5.0, `System.Reflection.Emit.Lightweight` 4.7.0 |
| Portable 2.1 | `netstandard2.1` | `Unsafe` 5.0 |
| Limited Core | `netcoreapp2.1`, `netcoreapp3.0` | `System.Text.Json` 5.0.2, `Unsafe` 5.0; netcoreapp2.1 also pins `Microsoft.NETCore.App` 2.1.30 |
| Legacy | `netcoreapp3.1`, `net5.0`, `net461` | `Unsafe` 6.0, `System.Text.Json` 6.0.11; net461 adds `Microsoft.Bcl.AsyncInterfaces`, `System.Memory`, `ValueTuple` |
| Extended | `net6.0`, `net7.0` | `Unsafe` 6.1.2 on net6.0; `System.Text.Json` 8.0.6 on both |
| Current | `net8.0`, `net9.0`, `net10.0` | None |
| Framework 4.6.2+ | `net462`, `net470`, `net471`, `net472` | `Microsoft.Bcl.Memory` 10.0.11, `Microsoft.Bcl.HashCode` 6.0.0, `System.Text.Json` 10.0.11 |
| UWP | `uap10.0.16299` | `Microsoft.Bcl.Memory` 9.0.19, `Microsoft.Bcl.HashCode` 6.0.0, `System.Text.Json` 6.0.11, UWP compiler packs (private) |

On Mono with a .NET Framework TFM, `System.Runtime.CompilerServices.Unsafe` 5.0 is referenced and runtime assets are excluded for libraries.

## Choosing an API when you target several TFMs

1. If every TFM is a 2.9.5 target (.NET Standard 2.1 / .NET Core 3.0 or later), either callback style compiles. Functional interfaces still avoid an extra heap delegate.
2. If any TFM was added after 2.9.5, write to that narrower surface: functional interfaces, `out FixedContextValue<T>`, `WrapperFactory`.
3. Delegate overloads remain valid on 2.9.5 targets. Use them when you already have that call site; do not `#if` them into a multi-target project that includes netfx, UWP, or netstandard2.0.

See [Getting started](../getting-started.md) for install and AOT notes, and [Functional interfaces](functional-interfaces.md) for the callback contracts.
