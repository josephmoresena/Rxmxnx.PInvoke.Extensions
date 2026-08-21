# Buffers

Managed buffers let you place a **known number of values or object references on the stack**. `BufferManager` picks a buffer type, allocates it for a callback, and falls back to the heap when the stack cannot hold it.

Maximum capacity of any buffer is 2¹⁵ elements. The runtime may offer less. Unmanaged values often use `stackalloc`; reference types and managed structs use the buffer types below.

More composition detail lives in the [buffers intermediate README](../../src/Intermediate/Rxmxnx.PInvoke.Buffers.Intermediate/README.md).

## `BufferManager` / `BufferManager<T>`

### Runtime information

| Member | Meaning |
| --- | --- |
| `BufferAutoCompositionEnabled` | Whether missing binary metadata can be composed at runtime. Always `false` if `IlcDisableReflection=true` or `PInvoke.DisableBufferAutoComposition` is on. |
| `MaxBinarySize` | Largest binary buffer that can be composed. `UInt16.MaxValue` when the platform has no cap. On .NET 8+ this follows the `PInvoke.BootstrapBufferStorage.*` switches. |

### Allocation

Delegate form (compatibility):

```csharp
BufferManager.Alloc<String>(4, static buffer => { /* buffer.Span */ });
TResult result = BufferManager.Alloc<String, TResult>(4, static buffer => default);
```

Functional form (preferred):

```csharp
BufferManager<String>.Alloc(new MyAction());                       // IScopedBufferAction<String>
BufferManager<String>.Alloc<TResult, MyFunc>(new MyFunc(), out TResult result);
MyAction action = new();
BufferManager<String>.AllocWithReference(ref action);              // passes the struct by managed reference
```

`isMinimumCount` / `IScopedBufferAction<T>.IsMinimalCount` means “at least this many elements”; extra slots are allocated but not exposed in `ScopedBuffer<T>.Span`.

`BufferManager.VisualBasic` mirrors `Alloc` for Visual Basic .NET and is not recommended from C#.

### Registration and preparation

| Method | Purpose |
| --- | --- |
| `Register<TBuffer>()` | Register an object (`Object`) buffer. |
| `Register<T, TBuffer>()` | Register a `struct` buffer. |
| `RegisterNullable<T, TBuffer>()` | Register a `T?` buffer. |
| `PrepareBinaryBuffer(UInt16)` | Compose and cache object-buffer metadata for a size (uses reflection). |
| `PrepareBinaryBuffer<T>(UInt16)` | Same for `struct` `T`. |
| `PrepareBinaryBufferNullable<T>(UInt16)` | Same for `T?`. |

Registration is static and AOT-friendly. Preparation always uses reflection and requires auto-composition.

## `ScopedBuffer<T>`

The value passed into an alloc callback. A `ref struct` view over the allocated space:

| Member | Meaning |
| --- | --- |
| `Span` | Accessible elements (length is `Count`, not necessarily `FullLength`). |
| `InStack` | Whether the buffer was placed on the stack. |
| `FullLength` | Allocated length, which may be larger when `IsMinimalCount` is true. |
| `BufferMetadata` | Metadata used for the allocation, when a managed buffer was selected. |

Use the span only inside the callback. Do not return it.

## Buffer structs

### Binary buffers

Binary buffers are built from powers of two:

- `Atomic<T>` — capacity 2⁰ (one element).
- `Composite<TBufferA, TBufferB, T>` — capacity A + B. Both sides must be binary; `TBufferA` must be smaller than or equal to `TBufferB`; `TBufferB` must be a 2ⁿ space. Equal sides produce 2ⁿ⁺¹.

Auto-composition combines these at runtime when the feature is enabled and reflection/metadata are available. On Native AOT you must preserve composite metadata (see the [intermediate README](../../src/Intermediate/Rxmxnx.PInvoke.Buffers.Intermediate/README.md#native-aot)) **or** register/prepare sizes you will use.

### Non-binary buffers

`NonBinarySpace<TArray, T>` stores as many `T` as fit in a custom struct `TArray`. Use this when the size is not a binary combination you want to compose.

### Metadata

`BufferTypeMetadata` / `BufferTypeMetadata<T>` describe a buffer type: whether it is binary, its size, its component count, and its `Type`. Indexer walks components. The type implements `IEnumerableSequence<BufferTypeMetadata>` and is not inheritable.

`IManagedBuffer<T>` / `IManagedBinaryBuffer<T>` / `IManagedBinaryBuffer<TBuffer, T>` are implemented by the buffer structs. They are not public extension points.

## AOT and registration

On Native AOT:

1. Prefer `BufferManager.Register…` for every size/type you allocate.
2. If you rely on auto-composition, preserve the composite types in a runtime directives file and keep reflection enabled.
3. `PrepareBinaryBuffer` is a JIT-friendly cache warmer; it is the wrong tool when reflection is trimmed away.

Feature switches on .NET 8+:

| Switch | Cap | Space |
| --- | --- | --- |
| `PInvoke.BootstrapBufferStorage.Minimal` | 31 | 2⁴+…+2⁰ |
| `PInvoke.BootstrapBufferStorage.Medium` | 127 | 2⁶+…+2⁰ |
| `PInvoke.BootstrapBufferStorage.Limited` | 2047 | 2¹⁰+…+2⁰ |
| `PInvoke.BootstrapBufferStorage.Extended` | 2047, extendable | managed-buffer-based space |

Default on .NET 8+ is still the storage system **not** based on managed-buffer binary spaces. Systems that use a 2ᴺ−1 binary space preload `2N−1` object-buffer metadata entries, which helps AOT.

## Buffer delegates

`ScopedBufferAction<T>`, `ScopedBufferAction<T, TState>`, `ScopedBufferFunc<T, TResult>`, `ScopedBufferFunc<T, TState, TResult>` — superseded by [functional interfaces](functional-interfaces.md#buffer-operations) in new code. `TState` may be a `ref struct` from .NET 9.

## See also

- [Capabilities: stack buffers](../capabilities.md#stack-first-temporary-buffers)
- [Use case: hot parser](../use-cases.md#use-a-stack-buffer-in-a-hot-parser)
