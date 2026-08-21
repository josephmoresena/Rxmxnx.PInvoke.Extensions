# Fixed memory

Fixed memory APIs pin a managed object (or wrap native memory) and expose it as spans, references, and typed pointers for a **bounded lifetime**. When the callback returns or `Dispose` runs, the pin is released and the pointers must not be used.

This generation introduces value-type contexts (`FixedPointerValue`, `FixedContextValue<T>`) and [functional interfaces](functional-interfaces.md). The older `IFixed*` interfaces remain for compatibility.

## Contexts you will hold

### `FixedPointerValue`

A `ref struct` for “a pinned address of some size”. You get `Pointer`, size, and validity checks. `FixedPointerValue.UnsafeDisposable` is a no-op `IDisposable` for null or already-unmanaged cases.

### `FixedContextValue<T>` / `ReadOnlyFixedContextValue<T>`

A `ref struct` for “a pinned block of `T`”:

| Member | Meaning |
| --- | --- |
| `Pointer` | Address as `IntPtr`. |
| `ValuePointer` | `ValPtr<T>` or `ReadOnlyValPtr<T>`. |
| `Values` | `Span<T>` / `ReadOnlySpan<T>` over the block. |
| `Bytes` | Byte view when `T` is unmanaged. |
| `Objects` | `Span<Object>` when `T` is a reference type. |
| `IsNullOrEmpty` | Null or zero-length block. |

Read-only variants hide mutation at the C# level. Native code can still write through the raw pointer.

### Interface contexts (`IFixedContext<T>` and friends)

The historical surface:

| Interface | Role |
| --- | --- |
| `IReadOnlyFixedMemory` / `IFixedMemory` | Untyped pinned block (`Pointer`, `Bytes`, `Objects`). |
| `IReadOnlyFixedMemory<T>` / `IFixedMemory<T>` | Typed values plus the untyped view. |
| `IReadOnlyFixedContext<T>` / `IFixedContext<T>` | Adds `Transformation<TDestination>()` and `ValuePointer`. |
| `IReadOnlyFixedReference<T>` / `IFixedReference<T>` | A single pinned `T` (`Reference`). |
| `IFixedMethod<TDelegate>` | A pinned / marshalled managed method. |
| `*.IDisposable` | Same contract, released with `using`. |

`Transformation<TDestination>(out residual)` reinterprets the block as another unmanaged type and returns any leftover bytes as residual memory.

On current TFMs these interfaces may be marked obsolete in favor of the value-type contexts. New code should prefer `FixedContextValue<T>` plus a functional interface.

## Lists of pinned blocks

When several spans must stay pinned together:

| Type | Contents |
| --- | --- |
| `FixedPointerValueList` | Value-type list of `FixedPointerValue` (this generation). |
| `FixedMemoryList` / `ReadOnlyFixedMemoryList` | Interface-based lists of `IFixedMemory` / `IReadOnlyFixedMemory`. |
| `FixedCStringSequence` | A `CStringSequence` whose UTF-8 buffer is fixed. |

All list types are `ref struct`s and can be enumerated with `foreach`. `Count`, `IsEmpty`, an indexer, and `ToArray()` are available on the interface-based lists.

Functional-interface overloads pin between two and eight spans. The **action is the receiver**: `action.WithSafeFixed(span0, span1, ...)`. Delegate overloads keep the historical `span.WithSafeFixed(...)` shape. Both pass the pinned list into `IFixedPointerListAction` / `IFixedPointerListFunction<TResult>` (or the older list delegates).

## How pinning is requested

### `WithSafeFixed` (preferred)

Extension methods on spans, strings, references, delegates, and memory blocks. The memory stays pinned until the callback finishes.

Shapes:

- Action / function delegates (`FixedAction`, `FixedContextAction<T>`, `FixedFunc<TResult>`, …) — compatibility.
- Functional interfaces (`IFixedAction`, `IFixedContextAction<T>`, …) — preferred.
- Optional extra argument `TArg` (may be a `ref struct` on .NET 9+).

There are read-only variants (`WithSafeReadOnlyFixed`) when you want a read-only context from a mutable span.

### `GetFixedContext` / `GetFixedMemory`

Turns `Memory<T>` / `ReadOnlyMemory<T>` into an `IDisposable` context. Use `using` when the pin must last beyond a single lambda — for example, across several native calls in one method.

```csharp
using IReadOnlyFixedContext<Char>.IDisposable ctx = text.AsMemory().GetFixedContext();
CallNative(ctx.ValuePointer);
CallNativeAgain(ctx.Pointer);
```

### `NativeUtilities.HeapAlloc<T>`

Allocates **unmanaged** memory and exposes it as a fixed context. `Dispose` frees the block. This is not a GC pin; it is an ownership handle for native heap.

```csharp
using IDisposable _ = NativeUtilities.HeapAlloc<Byte>(64, out FixedContextValue<Byte> ctx);
```

### `GetFixedMethod<TDelegate>`

Marshals a managed delegate to a function pointer and keeps it alive until `Dispose`. Prefer this over `GetUnsafeFuncPtr` when the native side will call back later.

## Older delegate families

These delegates remain in the public API. Functional interfaces replace them in new code.

| Family | Operates on |
| --- | --- |
| `ReadOnlyFixedAction` / `FixedAction` (+ `TArg`, `Func`) | `IReadOnlyFixedMemory` / `IFixedMemory` or `FixedPointerValue` |
| `ReadOnlyFixedContextAction<T>` / `FixedContextAction<T>` | Typed contexts |
| `ReadOnlyFixedReferenceAction<T>` / `FixedReferenceAction<T>` | A single reference |
| `FixedMethodAction<TDelegate>` / `FixedMethodFunc<…>` | A pinned method |
| `ReadOnlyFixedListAction` / `FixedListAction` | Lists of pinned spans |
| `ReadOnlySpanFunc<T>` / `ReadOnlySpanFunc<T, TState>` | Span-returning factories (used by `CString` and `ValueRegion<T>`) |

From .NET 9, `TArg` / `TState` on many of these may be a `ref struct`.

## Validity rules

1. Do not store `Pointer` or `ValuePointer` on a field that outlives the callback or `using`.
2. Do not call into code that can `await` while holding a `ref struct` context across the await.
3. `GetUnsafe*` helpers do not pin. Use them on literals, `stackalloc`, or memory you already fixed.
4. After `Dispose`, operations on a handle-backed `FixedPointerValue` throw.

## See also

- [Functional interfaces](functional-interfaces.md)
- [Pointers](pointers.md)
- [Use case: pin for a native call](../use-cases.md#pin-managed-memory-only-for-the-native-call)
- [Use case: several buffers](../use-cases.md#pin-several-buffers-for-one-native-call)
