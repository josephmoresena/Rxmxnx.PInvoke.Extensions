# Wrappers and regions

Small contracts for “a value I can pass around” and “a block of `T` that might live in an array, a pointer, or a function”.

## Reference interfaces

| Interface | Exposes | Notes |
| --- | --- | --- |
| `IReadOnlyReferenceable<T>` | `ref readonly T Reference` | `IEquatable<IReadOnlyReferenceable<T>>`. Publicly implementable. |
| `IReferenceable<T>` | `ref T Reference` | Extends the read-only interface. Publicly implementable. |

From .NET 9, `T` may be a `ref struct`.

These are the “there is a live managed reference here” contracts. Fixed-reference contexts implement them.

## Wrapper interfaces

| Interface | Exposes | Mutability |
| --- | --- | --- |
| `IWrapper<T>` | `T Value { get; }` | Immutable wrapper. `IEquatable<T>`. |
| `IReferenceableWrapper<T>` | wrapper + `ref readonly T` | Value plus a live read-only reference. |
| `IMutableWrapper<T>` | `T Value { get; set; }` | Mutable value. |
| `IMutableReference<T>` | wrapper + `ref T Reference` | Mutable and referenceable. |

Each generic interface has a **non-generic companion** (`IWrapper`, `IReferenceableWrapper`, `IMutableWrapper`, `IMutableReference`) with static factories:

| Factory | `T` |
| --- | --- |
| `Create<TValue>(TValue)` | `struct` |
| `CreateNullable<TValue>(TValue?)` | `struct?` |
| `CreateObject<TObject>(TObject)` | reference type |

`IWrapper.IBase<T>` is a covariant view of `Value`. From .NET 9, `T` on that view may be a `ref struct`.

Generic `Create(T?)` methods also exist on the generic interfaces themselves.

Use wrappers when an API should accept “some `T`” without caring whether it is boxed, nullable, or a class — logging, callback payloads, adapter layers.

## `ValueRegion<T>`

A region of `T` values. Not inheritable (except through the nested `Memory` helper). This is the backing store of `CString`.

| Member | Meaning |
| --- | --- |
| `IsMemorySlice` | This instance is a subregion. |
| indexer | `T` at a zero-based index. |
| `ToArray()` | Copy into a new array. |
| `TryAlloc(GCHandleType, out GCHandle)` | Try to allocate a GC handle. |
| `GetPinnable(out Int32)` | Object to pin, plus offset. |
| implicit `ReadOnlySpan<T>` | Span view. |
| implicit `T[]?` | Array view when the backing is an array. |

**Factories:**

- `Create(T[])` — array backing.
- `Create(IntPtr, Int32)` — native memory.
- `Create(ReadOnlySpanFunc<T>)` — function backing.
- `Create<TState>(state, ReadOnlySpanFunc<T, TState>)` — stateful function.
- `Create<TState>(state, func, gcAlloc)` — stateful function that can produce a `GCHandle`.

Nested abstract class `ValueRegion<T>.Memory` lets you build a custom region from `ReadOnlyMemory<T>`.

## See also

- [Capabilities: wrappers](../capabilities.md#wrappers-and-references-as-contracts)
- [UTF-8 text](cstring.md) — `CString` is a `ValueRegion<Byte>` with extra rules
