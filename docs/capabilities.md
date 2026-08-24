# Capabilities

This page is a tour of what `Rxmxnx.PInvoke.Extensions` is good at. Each section names the outcome first, then the types that deliver it. For recipes, see [Use cases](use-cases.md). For member lists, see the [API reference](api/README.md).

## UTF-8 as a first-class .NET type

Native APIs, HTTP, JSON, and many binary protocols speak UTF-8. `System.String` is UTF-16. Every round trip through `Encoding.UTF8` allocates, copies, and often adds a terminator you have to remember yourself.

`CString` is a UTF-8 string that can sit on a managed buffer, a UTF-8 literal, or an unmanaged pointer. You choose the backing; the public API stays the same.

What you can do:

- Build UTF-8 from literals (`new CString(() => "Hello"u8)`, **C# 11**), byte arrays, spans, or pointers.
- Know whether the instance is null-terminated, a slice, a function, or a null pointer (`IsNullTerminated`, `IsSegmented`, `IsFunction`, `IsZero`).
- Concatenate, compare, and hash with `String`-compatible hash codes.
- Marshal as a null-terminated UTF-8 pointer on .NET 7.0+ via source-generated P/Invoke.
- Serialize with `System.Text.Json` on .NET Core and on .NET Framework 4.6.1+ without re-encoding on every write (not on .NET Standard 2.0/2.1 or net452/net46).

`CStringSequence` stores several null-terminated UTF-8 strings in one contiguous buffer — the shape native code expects for `argv`, environment blocks, and similar lists. `CStringBuilder` is the mutable UTF-8 counterpart of `StringBuilder`.

Deep dive: [UTF-8 APIs](api/cstring.md) and the [CString intermediate notes](../src/Intermediate/Rxmxnx.PInvoke.CString.Intermediate/README.md).

## Typed pointers that keep their meaning

`IntPtr` is a number. `ValPtr<T>` is a pointer to `T`. That difference shows up in IntelliSense, in marshalling, and in reviews.

What you can do:

- Declare P/Invoke parameters as `ValPtr<Char>`, `ReadOnlyValPtr<Byte>`, or `FuncPtr<MyNativeCallback>`.
- Convert to and from `IntPtr` when a host API still uses untyped pointers.
- Read or write through `.Reference` without an `unsafe` block in your own code.
- On .NET 9.0+, use `T` that is a `ref struct` on `ValPtr<T>` / `ReadOnlyValPtr<T>` (consumers should use **C# 13**).
- Invoke a native function pointer through `FuncPtr<TDelegate>.Invoke`.

A pointer to a `ref struct`, or a pointer obtained from a `TDelegate`, is typically meaningful in **managed** code or under **Native AOT**. Classic native P/Invoke already has an unmanaged function pointer; those extra shapes exist for the managed / Native AOT side of the same APIs.

The library never pretends a typed pointer is “safe” in the GC sense. It makes the contract visible so mistakes are harder to ship.

Deep dive: [Pointers](api/pointers.md).

## Pinning with a lifetime, not a comment

`fixed` and `GCHandle` work, but the pointer they produce can leak into a field, a callback, or another thread. `WithSafeFixed` and `GetFixedContext` pin memory and hand you a context that is only valid inside the callback or `using` scope.

What you can do:

- Pin a `Span<T>`, `ReadOnlySpan<T>`, `String`, array, or single reference for the duration of an action or function.
- Read `Pointer`, `ValuePointer`, `Values`, and `Bytes` from the same context.
- Reinterpret the block as another unmanaged type with `Transformation<TDestination>()`.
- Pin several spans at once (up to eight) and walk them as a `FixedPointerValueList` (every TFM) or `FixedMemoryList` (.NET Standard 2.1 / .NET Core 3.0+).
- Prefer **functional interfaces** (`IFixedContextAction<T>`, `IFixedAction`, …) so the callback is a `readonly struct` that already holds its state.

Prefer value-type contexts (`FixedContextValue<T>`, `FixedPointerValue`) and functional interfaces. The `IFixed*` interfaces remain public on every TFM. Delegate overloads that take those interfaces, and helpers that return nested `IFixedContext<T>.IDisposable`, exist only on the original modern TFMs (.NET Standard 2.1 / .NET Core 3.0+, the set that existed until 2.9.5) — they were not brought to .NET Framework, .NET Standard 2.0, or UWP. See [compatibility](api/compatibility.md).

Whether `Memory<T>` whose `T` contains references can be pinned is a **host** decision. If the runtime allows pinning `String[]` or `String[,,,]`, the library does too; it does not reject managed types up front. `Memory.Pin()` may still throw on runtimes that refuse that pin.

Deep dive: [Fixed memory](api/fixed-memory.md) and [Functional interfaces](api/functional-interfaces.md).

## Native heap with Dispose, not with pairing

`NativeUtilities.HeapAlloc<T>(count, out FixedContextValue<T>)` allocates unmanaged memory and returns an `IDisposable` that frees it. On .NET Standard 2.1 / .NET Core 3.0+ there is also `HeapAlloc<T>(count)` returning `IFixedContext<T>.IDisposable`.

Use this when the buffer must outlive a single callback — for example, a native API that writes into a buffer you later read from managed code — without dropping to `Marshal.AllocHGlobal` and a `try/finally`.

## Binary memory as a view, not a copy

When the layout is already in memory, copying it into a `byte[]` just to hash, serialize, or reinterpret it is wasted work.

What you can do:

- View any unmanaged value or span as `Span<Byte>` / `ReadOnlySpan<Byte>` (`AsBytes`, `AsBinarySpan`).
- View bytes as `Span<T>` (`AsValues`, `AsValue`).
- Copy a value to a new byte array only when you actually need a snapshot (`ToBytes`, `ToValue`).
- Detect whether a span points at a UTF-8/UTF-16 literal (`IsLiteral`) so you can skip pinning or copying.

These helpers operate on GC-managed references internally. They are the “safe” side of the library. Pointer-based counterparts exist for constants, stackalloc, and already-fixed native memory; those are marked unsafe in both name and documentation.

On desktop .NET Framework, `Span<T>` is often the slower three-field layout, so the same views can cost more than they do on current .NET. UWP and Mono can still take the **fast** path even when the TFM’s public `Span<T>` surface looks older. `SystemInfo.UsesNativeSpan` is the check when you choose fast span (runtime optimizations) versus your own / older path — usually array versus span. Once you already have a `ref`, `NativeUtilities.TryCreateSpan` / `TryCreateReadOnlySpan` are that same choice for the ref.

Deep dive: [Extensions](api/extensions.md) and the [memory extension notes](../src/Intermediate/Rxmxnx.PInvoke.Extensions.Intermediate/README.md).

## Flatten multidimensional arrays without copying

The BCL `AsSpan` / `AsMemory` helpers stop at rank-1 arrays. This library exposes the **contiguous backing** of a multidimensional array as `Span<T>` / `Memory<T>` on **every TFM**, including slow (non-fast) span and runtimes older than .NET 5.0.

That is a differential feature, not a convenience wrapper. Getting a correct view over `T[,]`, `T[,,]`, … through `T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]` (ranks 2 through 32) had to work on desktop .NET Framework’s three-field span as well as on modern two-field span. The public API is the same; the implementation follows each runtime’s array layout.

```csharp
Int32[,] matrix = { { 1, 2 }, { 3, 4 } };
Span<Int32> flat = matrix.AsSpan();   // length 4, same storage
Memory<Int32> mem = matrix.AsMemory();
```

No extra copy. The span is a view over the array’s existing data. Pinning, `AsBytes`, and the rest of the memory helpers then apply to that view.

On Unity IL2CPP, flattening is compiled in statically. Indices greater than 17 can produce invalid C++ identifiers if the linker does not remove those members; [Getting started](getting-started.md#unity-il2cpp) has the rewrite utility.

Deep dive: [Extensions](api/extensions.md).

## Stack-first temporary buffers

Heap allocations in a parser or serializer show up in traces. `BufferManager.Alloc` gives you a `ScopedBuffer<T>` for the duration of a callback.

What you can do:

- Allocate a scoped buffer of value types or object references for a callback.
- Register binary buffer metadata ahead of time so Native AOT does not need reflection to compose sizes.
- Use `Atomic<T>`, `Composite<TBufferA, TBufferB, T>`, and `NonBinarySpace<TArray, T>` when you need an explicit stack layout.
- Prefer `IScopedBufferAction<T>` / `IScopedBufferFunction<T, TResult>` so the work is a struct, not a delegate.

**Unmanaged** `T` does not need a managed buffer. `ScopedBuffer<T>` is a **view** over the allocated space; the allocation itself uses `stackalloc`.

**Managed** buffers (reference types and managed structs) have a theoretical maximum of **(2¹⁶) − 1** elements. A single **binary** buffer is at most **2¹⁵** elements. Combining the maximum binary spaces still cannot exceed **(2¹⁶) − 1**. The runtime may offer less, and feature switches (`PInvoke.BootstrapBufferStorage.*`) control how much metadata is preloaded on .NET 8.0+.

Deep dive: [Buffers](api/buffers.md) and the [buffer intermediate notes](../src/Intermediate/Rxmxnx.PInvoke.Buffers.Intermediate/README.md).

## Runtime and AOT awareness

Interop code often has to behave differently on Native AOT, Mono, WebAssembly, or a trimmed mobile build. `AotInfo` and `SystemInfo` expose those facts as regular properties so you can branch in one place.

What you can do:

- Ask whether the process is Native AOT, whether reflection is disabled, and whether dynamic IL is allowed (`AotInfo`).
- Ask whether the runtime is Mono or Web, and which OS you are on (`SystemInfo`), in a way the trimmer can follow.
- Ask whether a delegate or `MethodBase` is backed by image code rather than generated IL (`IsImageMethod`) — especially useful on Mono.
- Ask whether a span is a hardcoded literal (`IsLiteral` / `MayBeNonLiteral`).

The library itself is designed around **minimal reflection and statically reachable code**, which is why it trims cleanly and runs under Mono AOT, ReadyToRun, Native AOT, and Unity IL2CPP (with one documented IL2CPP identifier workaround).

Deep dive: [Utilities](api/utilities.md) and [Getting started](getting-started.md#aot-support).

## Wrappers and references as contracts

Sometimes you need to pass a value, a mutable slot, or a managed reference across an API without exposing the storage. `IWrapper<T>`, `IMutableWrapper<T>`, `IReferenceable<T>`, and `IMutableReference<T>` are small contracts for that. Factory methods on `WrapperFactory` create the right implementation for structs, nullables, and reference types on every TFM. On .NET Standard 2.1 / .NET Core 3.0+ the same factories also live on the non-generic `IWrapper` / `IMutableWrapper` / `IMutableReference` interfaces.

`ValueRegion<T>` is the backing abstraction behind `CString`: an array, a native pointer, or a span-returning function, with slicing and optional pinning.

Deep dive: [Wrappers and regions](api/wrappers.md).

## Modern code on older hosts

Until version **2.9.5**, package compatibility was limited to modern runtimes that support .NET Standard 2.1. Later versions keep that baseline and add more TFMs so the **same modern APIs** can run on Framework, UWP, Standard 2.0 engines, and .NET Core 2.1.

That is not a hint that those hosts are invalid. A production product on .NET Framework 4.6.1+, UWP, Mono, Unity, or Xamarin is a first-class use of the library. The idea is **modern, retrocompatible code** — and, when you eventually move, a path toward Mono or current .NET without rewriting the interop layer.

- **Portable** `netstandard2.1` covers Xamarin, Unity, and Mono. Use **`netstandard2.0` only when the engine cannot target 2.1**.
- **Dedicated** `lib/` assemblies exist for .NET / .NET Core, UWP, and .NET Framework so those runtimes are not forced through Standard.
- **.NET Framework 4.5.2 / 4.6** are a transition step from before Standard 2.0 (no `System.Text.Json` in the core package). **4.6.1 and later** are dedicated Framework binaries for products that still run there.
- Span work follows the **runtime**: cheaper on modern .NET (and often on UWP/Mono) than on desktop Framework. Fast span versus your own / older code: `UsesNativeSpan` while you still choose how to work; `TryCreateSpan` / `TryCreateReadOnlySpan` once you already hold a `ref`.
- Each TFM binary **adapts to internal BCL and runtime changes** of that target. You write to the public API; the library absorbs layout and helper differences.

The map is in [Target frameworks and public API surface](api/compatibility.md). Language floors (C# 7.3, preferred C# 11, C# 13 on .NET 9.0+) are in [Getting started](getting-started.md#language-versions).

## Visual Basic access

The package is C#-first. Visual Basic .NET can still call a **minimal** helper surface: `Rxmxnx.PInvoke.VisualBasic` and `BufferManager.VisualBasic`. VB cannot express most `ref`/`span` APIs, so that is the smallest set that still works, not a full VB port.

## What this library is not

- It is not a replacement for `System.Runtime.InteropServices` or source-generated marshalling. It sits on top of them and makes the resulting code safer to read.
- It is not a general-purpose UTF-8 string library for UI text. `CString` is built for interop, protocols, and binary pipelines.
- It does not make dangling pointers impossible. It makes lifetimes visible: if the context is gone, the pointer is gone.
