# Target frameworks and public API surface

Until version **2.9.5**, package compatibility was limited to modern runtimes that support **.NET Standard 2.1** (.NET Standard 2.1, .NET Core 3.0, and later). That ceiling stays true no matter how many versions follow: later releases still include that original surface, and they add more targets *in addition*.

From versions after 2.9.5 the package also ships dedicated and portable assemblies for older frameworks. The goal is **modern, retrocompatible code** — the same types and the same mental model on Native AOT, Mono, Unity, UWP, and .NET Framework. A production product on those runtimes is a valid use of the library.

Officially supported for new work: **.NET 8.0 and later**. The other assemblies exist so you can keep that modern style while the host framework or runtime changes.

## Why the extra targets exist

They are what the package can ship once the implementation is mature enough to honor each TFM’s **declared surface** and still use what the **executing runtime** actually provides. Each binary **adapts to BCL and runtime internals** of that target; you write to the public API.

| Kind of assembly | TFMs | Role |
| --- | --- | --- |
| Portable | `netstandard2.1`, `netstandard2.0` | One binary for **Xamarin, Unity, and Mono**. 2.1 is the portable target that existed until 2.9.5. Use **2.0 only when the engine cannot target 2.1**. |
| Dedicated .NET / .NET Core | `netcoreapp2.1`, `netcoreapp3.0`–`net10.0` | Own `lib/` per TFM. |
| Dedicated UWP | `uap10.0.16299` | Own binary for the UWP runtime. A product that ships on UWP is a first-class host, not a stopgap. |
| Dedicated .NET Framework | `net452`, `net46`, `net461`, `net462`, `net472` (`net470` / `net471` restore only) | Own binaries. 4.5.2 / 4.6 sit before Standard 2.0 (see below). **4.6.1 and later are production Framework targets** that also happen to prepare a path toward modern runtimes such as Mono. |

`net470` and `net471` are listed as package targets so those projects restore. They do **not** ship their own `lib/` (`IncludeBuildOutput=false`); NuGet falls back to a nearby framework.

There is also a **.NET Framework 4.5** Mono path used with facades. It is not a Library.props TFM.

## .NET Standard 2.0 vs 2.1

**.NET Standard 2.1** is the portable assembly to use whenever the engine supports it (Xamarin, Unity, Mono, original Blazor WebAssembly).

**.NET Standard 2.0** exists for the same hosts when they still cannot take 2.1. Do not choose 2.0 on an engine that already supports 2.1: you pick up extra dependencies (`System.Memory`, `Reflection.Emit.Lightweight`) and you lose the original modern callback helpers that 2.1 still has.

The core package does **not** reference `System.Text.Json` from either Standard assembly. Mixing Standard 2.0 JSON into a Standard 2.1 library breaks classic **Mono Framework**’s 4.5 compatibility model. Use [`Rxmxnx.PInvoke.Json`](../../src/MonoFacades/README.md) when you need JSON there.

## .NET Framework: transition vs dedicated

**.NET Framework 4.5.2 and 4.6** sit **before** that product line implemented .NET Standard 2.0. Those two assemblies are a **transition** step: they keep the package available on pre-Standard Framework, and they **do not** take `System.Text.Json` in the core package (so a 4.5-era Mono story is not mixed with Standard 2.0 JSON).

**.NET Framework 4.6.1 and later** are not “transition-only.” They are dedicated Framework binaries for products that still run there. 4.6.1 already accepts `System.Text.Json`; from 4.6.2 the extras include `Microsoft.Bcl.Memory` and current JSON. They also make it easier to move that same code onto Mono or current .NET later — modern APIs, older host.

UWP follows the same idea: a dedicated, production-valid binary that can still use a **fast** `Span<T>` layout at runtime.

## Span efficiency

`Span<T>` on paper is not always `Span<T>` at runtime. **Span operations can be less efficient on desktop .NET Framework than on modern .NET.**

- **Fast span** is the compact two-field layout (byref + length). Current .NET, UWP Fall Creators, and Mono with Standard 2.1-class runtimes use it — even if the compile-time reference looks like `System.Memory`.
- **Slow span** is the three-field layout (pinnable object + offset + length) used by desktop .NET Framework plus `System.Memory` 4.5.x.

The .NET Framework and UWP assemblies compile to their TFM contracts, then detect the runtime layout. If the process is Mono executing a .NET Framework TFM — or UWP with fast span — casts, views, and pinning follow the fast path. The public API does not change; the operations get cheaper when the runtime allows it.

`SystemInfo.UsesNativeSpan` is the public property for that fact. On .NET Standard 2.1 / .NET Core 2.1+ it is always `true`. On .NET Framework and UWP it reflects the executing layout. Use it when you still choose **array versus span**.

When you already have a `ref` / `in` to a managed object, the question changes: optimized span versus `unsafe` + a loop over that ref. That is `NativeUtilities.TryCreateSpan` / `TryCreateReadOnlySpan`. Details: [Utilities: fast vs slow span](utilities.md#fast-vs-slow-span).

That is the same idea as the rest of the package: **modern code that stays retrocompatible**, without pretending every host is CoreCLR.

## Two public API surfaces

Most of the callback split is one preprocessor gate: `NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER`. That is the set of TFMs that existed **until 2.9.5**.

| Surface | TFMs | What you get |
| --- | --- | --- |
| **Until 2.9.5** | `netstandard2.1`, `netcoreapp3.0`, `netcoreapp3.1`, `net5.0`–`net10.0` | The original modern API **plus** value-type contexts and functional interfaces. |
| **Added after 2.9.5** | `netstandard2.0`, `netcoreapp2.1`, `net452`, `net46`, `net461`, `net462`, `net470`, `net471`, `net472`, `uap10.0.16299` | Value-type contexts and functional interfaces. Delegate pinning/buffer APIs and nested `IFixedMemory` / `IFixedContext<T>` `IDisposable` helpers were **not** brought over. |

Delegate `WithSafeFixed` / `BufferManager.Alloc` overloads and the `IFixed*` interfaces remain **public and supported** on the original modern TFMs. Prefer functional interfaces and `FixedContextValue<T>` in new code — that form compiles on every TFM the package ships.

## APIs on the original modern TFMs only

These types and overloads exist on .NET Standard 2.1, .NET Core 3.0, and later. They are absent from .NET Standard 2.0, .NET Core 2.1, .NET Framework, and UWP:

| Area | Until-2.9.5-only API |
| --- | --- |
| Pinning delegates | `FixedAction`, `ReadOnlyFixedAction`, `FixedFunc<TResult>`, `FixedContextAction<T>`, `ReadOnlyFixedContextAction<T>`, `FixedReferenceAction<T>`, `FixedMethodAction<TDelegate>`, `FixedListAction`, and the corresponding `Func` variants, including stateful `TArg` overloads |
| Buffer delegates | `ScopedBufferAction<T>`, `ScopedBufferFunc<T, TResult>`, and stateful variants; `BufferManager.Alloc(... delegate ...)` |
| UTF-8 pinning (instance) | `CString.WithSafeFixed(ReadOnlyFixedAction)` and related instance overloads; `CStringSequence.WithSafeFixed(ReadOnlyFixedListAction)` |
| String pinning (delegate) | `StringExtensions.WithSafeFixed` delegate overloads |
| Nested disposables | `IFixedMemory.IDisposable`, `IFixedContext<T>.IDisposable`, `IReadOnlyFixedMemory.IDisposable`, `IReadOnlyFixedContext<T>.IDisposable`, `IFixedReference<T>.IDisposable`, `IReadOnlyFixedReference<T>.IDisposable` |
| Interface-returning helpers | `Memory<T>.GetFixedContext()` → `IFixedContext<T>.IDisposable`; `NativeUtilities.HeapAlloc<T>(count)` → `IFixedContext<T>.IDisposable`; `GetValuesFixedContext<TEnum>()` without an `out` context |
| Lists | `FixedMemoryList`, `ReadOnlyFixedMemoryList` (use `FixedPointerValueList` on the extended TFMs) |
| Wrapper factories | Non-generic `IWrapper` / `IMutableWrapper` / `IMutableReference` / `IReferenceableWrapper` with static `Create*` methods (default interface methods). Use `WrapperFactory` instead. |
| UTF-8 hashing | `CString.GetHashCode(ReadOnlySpan<Byte>)` |
| Native libraries | `NativeUtilities.LoadNativeLib`, `GetNativeMethod<TDelegate>` (.NET Core 3.0+ only, not on .NET Standard 2.1) |

The `IFixed*` interfaces themselves exist on every TFM. What the extended assemblies omit is the **helpers that return nested `IDisposable` contexts** and the **delegate types that take those interfaces**. On those TFMs, call:

```csharp
utf8.WithSafeFixed(new PrintUtf8());                              // IFixedContextAction<Byte>
using IDisposable pin = mem.GetFixedContext(out FixedContextValue<Byte> ctx);
using IDisposable heap = NativeUtilities.HeapAlloc<Byte>(64, out FixedContextValue<Byte> buffer);
BufferManager<String>.Alloc(new CollectNames());                  // IScopedBufferAction<String>
IWrapper<Int32> boxed = WrapperFactory.Create(42);
```

## APIs that exist on both surfaces

These are available from .NET Standard 2.0 / .NET Framework / UWP through current .NET:

- `CString`, `CStringSequence`, `CStringBuilder` (JSON converter attribute: .NET Core or .NET Framework 4.6.1+)
- `ValPtr<T>`, `ReadOnlyValPtr<T>`, `FuncPtr<TDelegate>`
- `FixedPointerValue`, `FixedContextValue<T>`, `ReadOnlyFixedContextValue<T>`, `FixedPointerValueList`
- `IFixedPointer`, `IFixedMemory`, `IFixedContext<T>`, `IFixedReference<T>`, `IFixedMethod<TDelegate>` (and the `IReadOnly*` counterparts). Nested `IDisposable` on `IFixedPointer` / `IFixedMethod<TDelegate>` exists on every TFM; nested `IDisposable` on memory, context, and reference types does not.
- Functional interfaces: `IFixedAction`, `IFixedContextAction<T>`, `IReadOnlyFixedContextAction<T>`, `IFixedPointerListAction`, `IScopedBufferAction<T>`, and the function counterparts. `Accept` / `Apply` take `FixedContextValue<T>` / `FixedPointerValue` / `ScopedBuffer<T>` on every TFM.
- `WithSafeFixed` / `GetFixedContext` / `GetFixedMemory` overloads that take a functional interface or an `out` value context
- `BufferManager<T>.Alloc` / `AllocWithReference` with functional interfaces; `BufferManager.VisualBasic` (every TFM)
- `NativeUtilities.HeapAlloc<T>(count, out FixedContextValue<T>)`
- `NativeUtilities.TryCreateSpan<T>` / `TryCreateReadOnlySpan<T>` (optimized span from a `ref`, or `false` so you stay on `unsafe`)
- `IWrapper<T>`, `ValueRegion<T>`, `AotInfo`, `SystemInfo`

`IScopedBufferAction<T>.IsMinimalCount` is a required property on the TFMs added after 2.9.5. On the original modern TFMs it has a default of `false`. The same pattern applies to a few other members that are default interface methods until 2.9.5 and required later: `IFixedMemory<T>.ValuePointer`, `IReadOnlyFixedMemory<T>.ValuePointer`, and `IFixedReference<T>.Transformation<TDestination>()` without a residual. Callers do not need to distinguish those; only custom interface implementations do.

## Other TFM differences

| Gate | Effect |
| --- | --- |
| .NET Core 3.0+ | `NativeLibrary` helpers; `System.Text.Json` on Core (3.0 uses 5.0.2, 3.1+ uses later versions) |
| .NET 5.0+ | Native `Enum.GetName<T>`, `Convert.ToHexString` |
| .NET 7.0+ | `[NativeMarshalling]` for `CString`, `CStringSequence`, `ValPtr<T>`, `ReadOnlyValPtr<T>`, `FuncPtr<TDelegate>`; `IUtf8FunctionState<TSelf>`; `IParsable` on pointers |
| .NET 8.0+ | No extra package dependencies; buffer storage feature switches |
| .NET 9.0+ | `allows ref struct` on many generics; `params ReadOnlySpan<T>`; some pointer helpers patched in IL. Consumers should use **C# 13**. |
| .NET Core **or** net461+ | `[JsonConverter]` on `CString` / `CStringSequence`. Not on .NET Standard 2.0/2.1 (portable Mono/Xamarin/Unity story) or on the net452/net46 transition assemblies. Classic Mono can use [`Rxmxnx.PInvoke.Json`](../../src/MonoFacades/README.md). |
| .NET Framework / UWP dedicated binaries | `System.Memory` or `Microsoft.Bcl.Memory`; span helpers adapt to fast vs slow layout at runtime |

## Language versions (consumers)

Public signatures use `unmanaged`, `Enum`, and related generic constraints. That sets the floor:

| When | Language |
| --- | --- |
| Any TFM | **C# 7.3** minimum (`unmanaged`, `Enum`, `Delegate`, …) |
| Preferred on every TFM | **C# 11** (UTF-8 `u8` literals, `scoped` parameters) |
| .NET 9.0 and later | **C# 13** (`allows ref struct` on pointers, wrappers, and many callbacks) |

The package is written in C# and remains usable from **Visual Basic .NET** on a small helper surface (`Rxmxnx.PInvoke.VisualBasic`, `BufferManager.VisualBasic`). VB cannot express most `ref`/`span` APIs; that surface is the minimum that still works.

See [Getting started: language versions](../getting-started.md#language-versions).

## Dependencies by TFM family

From the package’s own `Packages.props`:

| Family | TFMs | Extra dependencies |
| --- | --- | --- |
| Transition .NET Framework | `net452`, `net46` | `System.Memory` 4.5.5, `Unsafe` 5.0, `RuntimeInformation` 4.3.0, `ValueTuple` 4.5.0 — no `System.Text.Json` |
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

1. If every TFM is an original modern target (.NET Standard 2.1 / .NET Core 3.0 or later), either callback style compiles. Functional interfaces still avoid an extra heap delegate.
2. If any TFM was added after 2.9.5, write to that narrower surface: functional interfaces, `out FixedContextValue<T>`, `WrapperFactory`.
3. Delegate overloads remain valid on the original modern TFMs. Use them when you already have that call site; do not `#if` them into a multi-target project that includes .NET Framework, UWP, or netstandard2.0.
4. Prefer **netstandard2.1** over **netstandard2.0** whenever the engine allows it.

See [Getting started](../getting-started.md) for install, language, and AOT notes, and [Functional interfaces](functional-interfaces.md) for the callback contracts.
