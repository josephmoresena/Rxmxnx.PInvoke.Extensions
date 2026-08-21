# Functional interfaces

Functional interfaces are **callable structs**. They replace the delegate overloads of `WithSafeFixed`, `BufferManager.Alloc`, and related APIs so that:

- State lives on the struct, not in a compiler-generated display class.
- The call can be fully inlined in many cases.
- On .NET 9+, the operation can accept `ref struct` values (`scoped FixedContextValue<T>`).

This is the preferred callback style in this generation of the package.

## How they look

```csharp
readonly struct PrintBytes : IFixedContextAction<Byte>
{
    public void Accept(scoped FixedContextValue<Byte> ctx)
        => Console.WriteLine(ctx.Values.Length);
}

span.WithSafeFixed(new PrintBytes());
```

Actions use `Accept(...)`. Functions use `Apply(...)` and return a value. Some `BufferManager` function overloads write the result to an `out` parameter so the function struct can be a `ref struct`.

## Fixed-memory operations

| Interface | Replaces | Parameter |
| --- | --- | --- |
| `IFixedAction` | `FixedAction`, `ReadOnlyFixedAction`, and stateful variants | `scoped FixedPointerValue` |
| `IFixedFunction<TResult>` | `FixedFunc<TResult>` and variants | `scoped FixedPointerValue` |
| `IFixedContextAction<T>` | `FixedContextAction<T>` and variants | `scoped FixedContextValue<T>` |
| `IFixedContextFunction<T, TResult>` | `FixedContextFunc<T, TResult>` and variants | `scoped FixedContextValue<T>` |
| `IReadOnlyFixedContextAction<T>` | `ReadOnlyFixedContextAction<T>` and variants | `scoped ReadOnlyFixedContextValue<T>` |
| `IReadOnlyFixedContextFunction<T, TResult>` | `ReadOnlyFixedContextFunc<T, TResult>` and variants | `scoped ReadOnlyFixedContextValue<T>` |
| `IFixedPointerListAction` | `FixedListAction`, `ReadOnlyFixedListAction` | `scoped FixedPointerValueList` |
| `IFixedPointerListFunction<TResult>` | `FixedListFunc<TResult>` and variants | `scoped FixedPointerValueList` |

Implementations can store whatever state they need as fields. There is no separate `TArg` type parameter; put the argument on the struct.

## Buffer operations

| Interface | Replaces | Notes |
| --- | --- | --- |
| `IScopedBufferAction<T>` | `ScopedBufferAction<T>` / `ScopedBufferAction<T, TState>` | `Count` is the requested length; `IsMinimalCount` allows a larger allocation whose extra elements are not exposed. |
| `IScopedBufferFunction<T, TResult>` | `ScopedBufferFunc<T, TResult>` and stateful variant | Same count contract; `Apply` returns `TResult`. |

```csharp
readonly struct Fill : IScopedBufferAction<Int32>
{
    public UInt16 Count => 8;
    public Boolean IsMinimalCount => false;

    public void Accept(scoped ScopedBuffer<Int32> buffer)
        => buffer.Span.Clear();
}

BufferManager<Int32>.Alloc(new Fill());
```

`BufferManager<T>.AllocWithReference` passes the action/function as a managed reference, which helps when the struct is large.

On .NET Standard 2.1, `IsMinimalCount` is a required property. On newer TFMs it has a default of `false`.

## Other functional contracts

These are not “delegate replacements” in the same sense, but they follow the same idea: a type that supplies behavior without a heap delegate.

### `IUtf8FunctionState<TSelf>` (.NET 7+)

A struct that can produce UTF-8 for `CString.Create<TState>(state)`.

| Member | Role |
| --- | --- |
| `Alloc` | Static virtual function that allocates a state instance. |
| `IsNullTerminated` | Whether the resulting text includes a terminator. |
| `GetSpan(TSelf)` | Returns the UTF-8 span. |
| `GetLength(in TSelf)` | Returns the length (excluding the terminator when present). |

On .NET 6 and earlier, pass a state object plus `ReadOnlySpanFunc<Byte, TState>` instead.

### `IEnumerableSequence<T>`

Index-based sequence (`GetItem`, `GetSize`) that still implements `IEnumerable<T>`. Used by `CString` (bytes) and `CStringSequence` (items). `CreateEnumerator` builds the enumerator; `DisposeEnumeration` runs on dispose (ignored from .NET Core 3.0 except when a netstandard2.1 assembly runs on Mono). From .NET 9, `T` may be a `ref struct`.

### `IManagedBuffer<T>` / `IManagedBinaryBuffer<T>`

Implemented by buffer structs (`Atomic<T>`, `Composite<…>`, `NonBinarySpace<…>`). Not intended for public implementation. `GetMetadata<TBuffer>()` retrieves `BufferTypeMetadata<T>`.

## When to keep using delegates

Delegates are still the right tool when:

- The callback is a one-off lambda and allocation does not matter.
- You are on an API that has not been duplicated as a functional interface.
- Visual Basic consumes the API (`Rxmxnx.PInvoke.VisualBasic` delegates).

On some TFMs the delegate overloads are `[Obsolete]` to steer new code toward structs. The behavior is unchanged.

## See also

- [Fixed memory](fixed-memory.md)
- [Buffers](buffers.md)
- [Use case: functional interface pinning](../use-cases.md#pin-managed-memory-only-for-the-native-call)
