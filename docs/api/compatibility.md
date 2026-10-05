# Target frameworks and public API surface

Until version **2.9.5**, the package was compatible only with modern runtimes that support **.NET Standard 2.1**: the
portable `netstandard2.1` assembly and a dedicated assembly for each .NET Core and .NET version, through .NET 10.0.

Later versions extend compatibility to frameworks that are not compatible with .NET Standard 2.1, and ship dedicated and
portable assemblies for them. The original modern surface remains. The goal is **modern, backward-compatible code** —
the same types and the same mental model on Native AOT, Mono AOT, IL2CPP, .NET Native, ReadyToRun, Unity, UWP, and .NET
Framework. A production product on those runtimes is a valid use of the library.

For a new project, see [Support policy](../getting-started.md#support-policy).

## Why the extra targets exist

They are what the package can ship once the implementation is mature enough to honor each TFM’s **declared surface** and
still use what the **executing runtime** actually provides. Each binary **adapts to BCL and runtime internals** of that
target; you write to the public API.

| Kind of assembly           | TFMs                                                                               | Role                                                                                                                                                |
|----------------------------|------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------|
| Portable                   | `netstandard2.1`, `netstandard2.0`                                                 | **2.1** is the portable assembly for modern runtimes. **2.0** is for runtimes that are not compatible with 2.1. Both keep the dependency set small. |
| Dedicated .NET / .NET Core | `netcoreapp2.1`, `netcoreapp3.0`–`net10.0`                                         | Own `lib/` per TFM. Features and optimizations are added as the runtime admits them.                                                                |
| Dedicated UAP              | `uap10.0.16299`                                                                    | Modern APIs and runtime-level optimizations. See below.                                                                                             |
| Dedicated .NET Framework   | `net452`, `net46`, `net461`, `net462`, `net472` (`net470` / `net471` restore only) | Transition through 4.6, then dedicated support. See below.                                                                                          |

`net470` and `net471` are listed as package targets so those projects restore. They do **not** ship their own `lib/`
(`IncludeBuildOutput=false`); NuGet falls back to a nearby framework.

There is also a **.NET Framework 4.5** Mono path used with facades. It is not a Library.props TFM.

## .NET Standard 2.0 vs 2.1

**.NET Standard 2.1** keeps external dependencies to `System.Runtime.CompilerServices.Unsafe`.

**.NET Standard 2.0** adds `System.Memory` and `System.Reflection.Emit.Lightweight`. Do not choose 2.0 on a runtime that
already supports 2.1: you pick up those extra packages, and you lose the modern-line helpers that 2.1 still has. Do not
choose it for **.NET Core 2.1** either. That runtime does not implement .NET Standard 2.1, but it exposes native
`Span<T>` on its public API, and the package ships a dedicated `netcoreapp2.1` assembly for it.

Neither Standard assembly references `System.Text.Json`. The dependency graph runs from 2.0 to 2.1, so a built-in JSON
serializer is left out: a 2.0 assembly must not depend on an API the next standard does not provide, and the two
assemblies stay binary-compatible on that point. Mixing a .NET Standard 2.0 JSON package into the 2.1 assembly also
fights **Mono Framework**’s long-term .NET Framework 4.5 compatibility model. **Mono Framework** projects that need JSON
use [`Rxmxnx.PInvoke.Json`](../../src/MonoFacades/README.md).

## Legacy Framework: transition vs. dedicated

**.NET Framework 4.5.2 and 4.6** are a **transition**. Their public surface follows .NET Standard 2.0, and they do not
take `System.Text.Json`.

**.NET Framework 4.6.1** keeps a limited .NET Standard 2.0 surface, and it adds dedicated `System.Text.Json`
compatibility.

**.NET Framework 4.6.2** and later use modern APIs and runtime-level optimizations.

**UAP 10.0.16299** uses modern APIs and runtime-level optimizations. `Microsoft.NETCore.UniversalWindowsPlatform`
**6.2.12 or later** is recommended and ensures compatibility with .NET Native.

## Span efficiency

`Span<T>` on paper is not always `Span<T>` at runtime. **Span operations can be less efficient on desktop .NET Framework
than on modern .NET.**

- **Fast span** is the compact two-field layout (byref + length). Current .NET, UWP Fall Creators, and Mono with
  Standard 2.1-class runtimes use it — even if the compile-time reference looks like `System.Memory`.
- **Slow span** is the three-field layout (pinnable object + offset + length) used by desktop .NET Framework plus
  `System.Memory` 4.5.x.

The .NET Framework and UWP assemblies compile to their TFM contracts, then detect the runtime layout. If the process is
Mono executing a .NET Framework TFM — or UWP with fast span — casts, views, and pinning follow the fast path. The public
API does not change; the operations get cheaper when the runtime allows it.

`SystemInfo.UsesNativeSpan` is the public property for that fact. On .NET Standard 2.1 / .NET Core 2.1+ it is always
`true`. On .NET Framework and UWP it reflects the executing layout. Use it when you still choose **fast span (runtime
optimizations)** versus **your own / older code** — usually array versus span.

When you already have a `ref` / `in`, the same choice is `NativeUtilities.TryCreateSpan` / `TryCreateReadOnlySpan`: the
fast-span view, or whatever path you already have for that ref.
Details: [Utilities: fast vs slow span](utilities.md#fast-vs-slow-span).

That is the same idea as the rest of the package: **modern code that stays backward-compatible**, without pretending
every host is CoreCLR.

## Two public API surfaces

Most of the callback split is one preprocessor gate: `NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER`. That is the set of
TFMs that existed **until 2.9.5**.

| Surface               | TFMs                                                                                                                    | What you get                                                                                                                                                                |
|-----------------------|-------------------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Until 2.9.5**       | `netstandard2.1`, `netcoreapp3.0`, `netcoreapp3.1`, `net5.0`–`net10.0`                                                  | The original modern API **plus** value-type contexts and functional interfaces.                                                                                             |
| **Added after 2.9.5** | `netstandard2.0`, `netcoreapp2.1`, `net452`, `net46`, `net461`, `net462`, `net470`, `net471`, `net472`, `uap10.0.16299` | Value-type contexts and functional interfaces. Delegate pinning/buffer APIs and nested `IFixedMemory` / `IFixedContext<T>` `IDisposable` helpers were **not** brought over. |

Delegate `WithSafeFixed` / `BufferManager.Alloc` overloads and the `IFixed*` interfaces remain **public and supported**
on the original modern TFMs. Prefer functional interfaces and `FixedContextValue<T>` in new code — that form compiles on
every TFM the package ships.

## APIs that stay on the modern line

These types and overloads stay on .NET Standard 2.1, .NET Core 3.0, and later. Some are simply not portable; others are
suboptimal in general.

| Area                        | Modern-line API, not extended                                                                                                                                                                                                                                                  |
|-----------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Pinning delegates           | `FixedAction`, `ReadOnlyFixedAction`, `FixedFunc<TResult>`, `FixedContextAction<T>`, `ReadOnlyFixedContextAction<T>`, `FixedReferenceAction<T>`, `FixedMethodAction<TDelegate>`, `FixedListAction`, and the corresponding `Func` variants, including stateful `TArg` overloads |
| Buffer delegates            | `ScopedBufferAction<T>`, `ScopedBufferFunc<T, TResult>`, and stateful variants; `BufferManager.Alloc(... delegate ...)`                                                                                                                                                        |
| UTF-8 pinning (instance)    | `CString.WithSafeFixed(ReadOnlyFixedAction)` and related instance overloads; `CStringSequence.WithSafeFixed(ReadOnlyFixedListAction)`                                                                                                                                          |
| String pinning (delegate)   | `StringExtensions.WithSafeFixed` delegate overloads                                                                                                                                                                                                                            |
| Nested disposables          | `IFixedMemory.IDisposable`, `IFixedContext<T>.IDisposable`, `IReadOnlyFixedMemory.IDisposable`, `IReadOnlyFixedContext<T>.IDisposable`, `IFixedReference<T>.IDisposable`, `IReadOnlyFixedReference<T>.IDisposable`                                                             |
| Interface-returning helpers | `Memory<T>.GetFixedContext()` → `IFixedContext<T>.IDisposable`; `NativeUtilities.HeapAlloc<T>(count)` → `IFixedContext<T>.IDisposable`; `GetValuesFixedContext<TEnum>()` without an `out` context                                                                              |
| Lists                       | `FixedMemoryList`, `ReadOnlyFixedMemoryList` (use `FixedPointerValueList` on the extended TFMs)                                                                                                                                                                                |
| Wrapper factories           | Non-generic `IWrapper` / `IMutableWrapper` / `IMutableReference` / `IReferenceableWrapper` with static `Create*` methods (default interface methods). Use `WrapperFactory` instead.                                                                                            |
| UTF-8 hashing               | `CString.GetHashCode(ReadOnlySpan<Byte>)`                                                                                                                                                                                                                                      |
| Native libraries            | `NativeUtilities.LoadNativeLib`, `GetNativeMethod<TDelegate>` (.NET Core 3.0+ only, not on .NET Standard 2.1)                                                                                                                                                                  |

The `IFixed*` interfaces themselves exist on every TFM. What the extended assemblies omit is the **helpers that return
nested `IDisposable` contexts** and the **delegate types that take those interfaces**. On those TFMs, call:

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
- `IFixedPointer`, `IFixedMemory`, `IFixedContext<T>`, `IFixedReference<T>`, `IFixedMethod<TDelegate>` (and the
  `IReadOnly*` counterparts). Nested `IDisposable` on `IFixedPointer` / `IFixedMethod<TDelegate>` exists on every TFM;
  nested `IDisposable` on memory, context, and reference types does not.
- Functional interfaces: `IFixedAction`, `IFixedContextAction<T>`, `IReadOnlyFixedContextAction<T>`,
  `IFixedPointerListAction`, `IScopedBufferAction<T>`, and the function counterparts. `Accept` / `Apply` take
  `FixedContextValue<T>` / `FixedPointerValue` / `ScopedBuffer<T>` on every TFM.
- `WithSafeFixed` / `GetFixedContext` / `GetFixedMemory` overloads that take a functional interface or an `out` value
  context
- `BufferManager<T>.Alloc` / `AllocWithReference` with functional interfaces; `BufferManager.VisualBasic` (every TFM)
- `NativeUtilities.HeapAlloc<T>(count, out FixedContextValue<T>)`
- `NativeUtilities.TryCreateSpan<T>` / `TryCreateReadOnlySpan<T>` (fast-span view from a `ref`, or `false` so you keep
  your own path)
- `IWrapper<T>`, `ValueRegion<T>`, `AotInfo`, `SystemInfo`

`IScopedBufferAction<T>.IsMinimalCount` is a required property on the TFMs added after 2.9.5. On the original modern
TFMs it has a default of `false`. A value type should always implement the property. Calling that default on a value
type boxes the value on Native AOT before .NET 10 and on Mono. The same required-or-default split applies to a few
other members that are default interface methods until 2.9.5 and required later: `IFixedMemory<T>.ValuePointer`,
`IReadOnlyFixedMemory<T>.ValuePointer`, and `IFixedReference<T>.Transformation<TDestination>()` without a residual.
Callers do not need to distinguish those; only custom interface implementations do.

## Other TFM differences

| Gate                                    | Effect                                                                                                                                                                                                                                            |
|-----------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| .NET Core 3.0+                          | `NativeLibrary` helpers; `System.Text.Json` on Core (3.0 uses 5.0.2, 3.1+ uses later versions)                                                                                                                                                    |
| .NET 5.0+                               | Native `Enum.GetName<T>`, `Convert.ToHexString`                                                                                                                                                                                                   |
| .NET 7.0+                               | `[NativeMarshalling]` for `CString`, `CStringSequence`, `ValPtr<T>`, `ReadOnlyValPtr<T>`, `FuncPtr<TDelegate>`; `IUtf8FunctionState<TSelf>`; `IParsable` on pointers                                                                              |
| .NET 8.0+                               | No extra package dependencies; buffer storage feature switches                                                                                                                                                                                    |
| .NET 9.0+                               | `allows ref struct` on many generics; `params ReadOnlySpan<T>`; some pointer helpers patched in IL. Consumers should use **C# 13**.                                                                                                               |
| .NET Core **or** net461+                | `[JsonConverter]` on `CString` / `CStringSequence`. Not on .NET Standard 2.0/2.1 (portable Mono/Xamarin/Unity story) or on the net452/net46 transition assemblies. Classic Mono can use [`Rxmxnx.PInvoke.Json`](../../src/MonoFacades/README.md). |
| .NET Framework / UWP dedicated binaries | `System.Memory` or `Microsoft.Bcl.Memory`; span helpers adapt to fast vs slow layout at runtime                                                                                                                                                   |

## Language versions

C# floors and Visual Basic .NET are in [Getting started](../getting-started.md#language-versions).

## Dependencies by TFM family

From the package’s own `Packages.props`:

| Family                    | TFMs                                   | Extra dependencies                                                                                                                                         |
|---------------------------|----------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Transition .NET Framework | `net452`, `net46`                      | `System.Memory` 4.5.5, `Unsafe` 5.0, `RuntimeInformation` 4.3.0, `ValueTuple` 4.5.0 — no `System.Text.Json`                                                |
| Portable 2.0              | `netstandard2.0`                       | `System.Memory` 4.5.5, `Unsafe` 5.0, `System.Reflection.Emit.Lightweight` 4.7.0                                                                            |
| Portable 2.1              | `netstandard2.1`                       | `Unsafe` 5.0                                                                                                                                               |
| Limited Core              | `netcoreapp2.1`, `netcoreapp3.0`       | `System.Text.Json` 5.0.2, `Unsafe` 5.0; netcoreapp2.1 also pins `Microsoft.NETCore.App` 2.1.30                                                             |
| Legacy                    | `netcoreapp3.1`, `net5.0`, `net461`    | `Unsafe` 6.0, `System.Text.Json` 6.0.11; net461 adds `Microsoft.Bcl.AsyncInterfaces`, `System.Memory`, `ValueTuple`                                        |
| Extended                  | `net6.0`, `net7.0`                     | `Unsafe` 6.1.2 on net6.0; `System.Text.Json` 8.0.6 on both                                                                                                 |
| Current                   | `net8.0`, `net9.0`, `net10.0`          | None                                                                                                                                                       |
| Framework 4.6.2+          | `net462`, `net470`, `net471`, `net472` | `Microsoft.Bcl.Memory` 10.0.12, `Microsoft.Bcl.HashCode` 6.0.0, `System.Text.Json` 10.0.12                                                                 |
| UWP / UAP                 | `uap10.0.16299`                        | `Microsoft.Bcl.Memory` 9.0.19, `Microsoft.Bcl.HashCode` 6.0.0, `System.Text.Json` 6.0.11. UWP compiler packs are private and are not package dependencies. |

On Mono with a .NET Framework TFM, `System.Runtime.CompilerServices.Unsafe` 5.0 is referenced and runtime assets are
excluded for libraries.

## Choosing an API when you target several TFMs

1. If every TFM is an original modern target (.NET Standard 2.1 / .NET Core 3.0 or later), either callback style
   compiles. Functional interfaces still avoid an extra delegate allocation.
2. If any TFM was added after 2.9.5, write to that narrower surface: functional interfaces, `out FixedContextValue<T>`,
   `WrapperFactory`.
3. Delegate overloads remain valid on the modern line. Use them when you already have that call site; do not `#if`
   them into a multi-target project that includes .NET Framework, UAP 10.0.16299, .NET Standard 2.0, or .NET Core 2.1.
4. Use **netstandard2.1** on a modern runtime. Use **netstandard2.0** when the runtime is not compatible with 2.1.

See [Getting started](../getting-started.md) for installation, language, and AOT notes,
and [Functional interfaces](functional-interfaces.md) for the callback contracts.
