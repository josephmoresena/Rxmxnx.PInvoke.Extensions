# UTF-8 text

`CString`, `CStringSequence`, and `CStringBuilder` are the UTF-8 counterparts of `String`, `IReadOnlyList<String>`, and `StringBuilder`, designed for interop and binary pipelines.

Worked initialization examples also live in the [CString intermediate README](../../src/Intermediate/Rxmxnx.PInvoke.CString.Intermediate/README.md).

## `CString`

A sequence of UTF-8 units. Sealed. Can be enumerated as `IEnumerable<Byte>` or as `ReadOnlySpan<Byte>.Enumerator`. Range operators work. `GetHashCode()` matches `String` for the equivalent UTF-16 text.

Instances can be **fixed** (the library pins the backing) but not **GCHandle-pinned** as a `CString` object itself.

### Backing kinds

| Kind | How you get it | Notes |
| --- | --- | --- |
| Managed buffer | implicit `Byte[]`, `new CString(span)`, `Create(span)` | Copies into a managed buffer when needed. |
| Function / literal | `new CString(() => "Hi"u8)`, `Create(ReadOnlySpanFunc<Byte>)` | Preferred for UTF-8 literals; memory stays in the image. |
| Unmanaged pointer | `CreateUnsafe(ptr, length)`, `CreateNullTerminatedUnsafe(ptr)` | You keep the address valid. |

There is also `CString.Backing`, an abstract type you can subclass to plug in a custom store.

### Properties

| Property | Meaning |
| --- | --- |
| `Length` | Number of UTF-8 units, excluding a terminator that is not part of the length. |
| `IsNullTerminated` | Text ends with a UTF-8 null. |
| `IsReference` | UTF-8 lives elsewhere; this instance only points at it. |
| `IsSegmented` | This instance is a slice of another `CString`. |
| `IsFunction` | Backing is a span-returning function. |
| `IsZero` | Null-pointer UTF-8 (`CString.Zero`). |

Static fields: `Empty` (empty literal), `Zero` (null pointer), `NewLine` (platform newline as UTF-8).

### Construction cheatsheet

| API | Terminator | Copy |
| --- | --- | --- |
| implicit `Byte[]` | `true` if the last byte is `0x00` (that byte is excluded from length) | No extra copy of the array |
| explicit `String` | Always `true` | Encodes to a new buffer — inefficient for literals |
| `new CString(ReadOnlySpan<Byte>)` | Always `true` (appends a null) | Copy |
| `CString.Create(ReadOnlySpan<Byte>)` | Always `false` | Copy, no terminator check |
| `new CString(ReadOnlySpanFunc<Byte>)` | Depends on the span | No copy |
| `CString.Create<TState>(state)` | From `IUtf8FunctionState<TSelf>` (.NET 7+) | No copy |
| `CreateUnsafe(IntPtr, Int32, useFullLength)` | `false` if `useFullLength` is true | No copy |

`CString.Unescape` builds from escaped UTF-8 (`\\n`, `\\t`, …).

### Members worth knowing

- Indexer `this[Int32]` → `Byte`
- `AsSpan()`, `ToArray()`, `ToHexString()`, `TryPin()`
- `WithSafeFixed(TAction)` extension — every TFM. Instance `WithSafeFixed(ReadOnlyFixedAction)` — .NET Standard 2.1 / .NET Core 3.0+ only.
- `Concat` / `Join` / `Compare` (static), comparison operators with `CString` and `String`
- `+` with `CString`, `String`, `ReadOnlySpan<Byte>`, `ReadOnlySpan<Char>` — result is a new null-terminated instance when both sides are non-empty
- `IsNullOrEmpty`, `IsImagePersistent`, `GetAssociatedSequence`, `GetHashCode(ReadOnlySpan<Byte>)` (.NET Standard 2.1 / .NET Core 3.0+)
- Repeat constructors: a UTF-8 unit (or 2–4 units) repeated `count` times; UTF-16 `Char` / `ReadOnlySpan<Char>` constructors encode to UTF-8

From .NET 9, `params` uses `ReadOnlySpan<>` instead of arrays.

### Marshalling and JSON

- **.NET 7+:** `[NativeMarshalling]` — P/Invoke sees a null-terminated UTF-8 pointer.
- **.NET Core or net461+:** nested `JsonConverter` for `System.Text.Json`. Not on .NET Standard 2.0/2.1 or net452/net46. Deserialization produces null-terminated instances; the terminator is not guaranteed to be a single `0x00` byte.

## `CStringSequence`

A contiguous buffer of null-terminated UTF-8 strings, itself null-terminated. Sealed. Implements `IReadOnlyList<CString>`, `IEnumerableSequence<CString>`, `ICloneable`, and comparison interfaces.

This is the managed form of `char* argv[]` / environment blocks.

### Creating a sequence

```csharp
new CStringSequence("Hello", "World");
new CStringSequence("Hello"u8, "World"u8);          // up to 8 spans, low allocation
CStringSequence.Parse(hardcodedUtf16ThatIsUtf8Bytes);
CStringSequence.CreateBuilder().Append("Hello").Append("World"u8).Build();
CStringSequence.GetUnsafe(spanOfReadOnlyValPtr);    // from native char*[]
```

`Parse(String)` reuses the string as the buffer when it already has the right layout.

### Properties and views

| Member | Meaning |
| --- | --- |
| `Empty` | Empty sequence. |
| `Count` | Item count, including empty/null entries. |
| `NonEmptyCount` | Items that are not empty. |
| indexer | `CString` at `index`. |
| `Utf8View` | Nested `ref struct` view of items as `ReadOnlySpan<Byte>`. Prefer `sequence.CreateView(includeEmptyItems)`. |
| `Builder` | Fluent `struct` builder (`Append`, `AppendEscaped`, `Insert`, `RemoveAt`, `Build`). |

`Utf8View` does not pin by default. Pin the source sequence (and `CString.Empty` if empty items are included) if you need a stable address during enumeration.

`CStringSequence.WithSafeFixed` instance methods that take list delegates are **.NET Standard 2.1 / .NET Core 3.0+**. The functional-interface extensions (`seq.WithSafeFixed(new MyAction())`) exist on every TFM.

Marshalling on .NET 7+: a null-terminated array of null-terminated UTF-8 strings. Empty items are omitted unless a `Utf8View` included them. Nested `JsonConverter` on .NET Core or net461+.

`FixedCStringSequence` is the pinned `ref struct` form used when the sequence must be passed to native code as a list of pointers.

## `CStringBuilder`

Mutable UTF-8 text. Not a `CString` until you call `ToCString()`.

| Operation | Notes |
| --- | --- |
| `Append` | `String`, `CString`, `Byte`, UTF-8 span, UTF-16 span, numbers, line terminator |
| `AppendJoin` | Separator plus a sequence or params list |
| `AppendEscaped` | Writes the unescaped form into the buffer |
| `Insert` / `Remove` | Character-level, like `StringBuilder` |
| `Clear` | Drops all units |
| `ToCString()` / `ToCString(false)` | Snapshot; default is null-terminated |
| `CopyTo` | Writes a slice into a destination `Span<Byte>` |
| `Length` | Current unit count |

Default capacity is 32 UTF-8 units. Construction from `String`, `CString`, or spans sizes the first chunk accordingly.

## Delegates used by UTF-8 types

- `ReadOnlySpanFunc<Byte>` / `ReadOnlySpanFunc<Byte, TState>` — function-backed `CString`
- `IUtf8FunctionState<TSelf>` — .NET 7+ typed state ([functional interfaces](functional-interfaces.md))

## See also

- [Capabilities: UTF-8](../capabilities.md#utf-8-as-a-first-class-net-type)
- [Use cases: UTF-8](../use-cases.md#build-utf-8-once-marshal-many-times)
