# Extensions

Extension methods are the everyday surface: pin a span, view bytes as values, turn a pointer into a span, ask whether a delegate is image-backed. They live on the types you already have (`Span<T>`, `IntPtr`, `String`, `Delegate`, unmanaged values).

This page groups them by intent. Every overload is documented in XML; listing each `WithSafeFixed` variant here would recreate the old catalog.

## Binary (`BinaryExtensions`)

Work with `Byte`, `Byte[]`, and byte spans.

| Method | Result |
| --- | --- |
| `ToValue<T>(this Byte[] / Span<Byte> / ReadOnlySpan<Byte>)` | Copy bytes into an unmanaged `T`. |
| `AsValue<T>(this Span<Byte> / ReadOnlySpan<Byte>)` | `ref T` / `ref readonly T` over the same memory. |
| `AsHexString(this Byte / Byte[])` | Hexadecimal text. |
| `WithSafeFixed(...)` | Pin the bytes. Delegate overloads: .NET Standard 2.1 / .NET Core 3.0+. Functional-interface overloads live on `FixedPointerValueExtensions` / `FixedContextValueExtensions` and exist on every TFM. |

`T` is `unmanaged` for the value conversions.

## Memory blocks (`MemoryBlockExtensions`)

Spans, memories, and arrays of any `T`.

| Intent | Methods |
| --- | --- |
| Literal detection | `IsLiteral`, `MayBeNonLiteral` |
| Byte / value views | `AsBytes`, `AsValues<TIn, TOut>` (optional residual `Span<Byte>`) |
| Multidimensional arrays | `AsSpan`, `AsMemory` |
| Pinning | `WithSafeFixed` (functional interfaces on every TFM; delegate overloads on .NET Standard 2.1 / .NET Core 3.0+), `WithSafeReadOnlyFixed`, `GetFixedContext` / `GetFixedMemory` (`out` value context on every TFM; nested `IDisposable` return on the modern surface) |
| Unsafe addresses | `GetUnsafeIntPtr`, `GetUnsafeValPtr`, `GetUnsafeReadOnlyValPtr` (literals / already-fixed memory) |

`AsBytes` / `AsValues` operate on GC-managed references and do not require `unsafe`. They are views, not copies.

## Unmanaged values (`UnmanagedValueExtensions`)

Single values and small sequences of unmanaged `T`.

| Intent | Methods |
| --- | --- |
| Snapshot | `ToBytes`, `ToValues` |
| Pin a local | `WithSafeFixed` |
| Rent a fixed block | `RentFixed` |
| Enum names | `GetName` (uses native `Enum.GetName<T>` when available) |

## References (`ReferenceExtensions`)

`in T` / `ref T` helpers: `Transform` / `TransformReference` to another unmanaged type, `WithSafeFixed` for a single reference, `GetUnsafeValPtr` / `GetUnsafeIntPtr`.

From .NET 9, `T` may be a `ref struct` on several of these.

## Pointers (`PointerExtensions` / `ValuePointerExtensions`)

Turn `IntPtr`, `UIntPtr`, `ValPtr<T>`, or `ReadOnlyValPtr<T>` into managed views.

| Method | Result |
| --- | --- |
| `IsZero` | Null check. |
| `GetUnsafeSpan<T>(length)` / `GetUnsafeReadOnlySpan<T>` | Span over native memory you own. |
| `GetUnsafeReadOnlySpanFromNullTerminated` | Walk a native C string. |
| `GetUnsafeReference<T>` | `ref T` at the address. |
| `GetUnsafeString` | UTF-16 or UTF-8 `String` copy from a native pointer. |
| `GetUnsafeDelegate<TDelegate>` | Delegate for a function pointer. |
| `GetUnsafeStream` | `UnmanagedMemoryStream` over a native block. |

These are unsafe by contract: the pointer must stay valid for as long as the view is used.

## Delegates (`DelegateExtensions`)

| Method | Result |
| --- | --- |
| `GetUnsafeFuncPtr` / `GetUnsafeIntPtr` / `GetUnsafeUIntPtr` | Function pointer to a managed delegate (keep the delegate alive). |
| `WithSafeFixed` | Pin the delegate while a callback runs. |
| `GetFixedMethod` | Marshalled `IFixedMethod<TDelegate>.IDisposable`. |
| `IsImageMethod` | Whether every method in the invocation list is image-backed (not generated IL). Also exists on `MethodBase`. |

`IsImageMethod` is aimed at Mono. It returns `false` for `null`, open generics, and platforms without memory inspection. In reflection-free runtimes, a valid delegate is assumed to be image-backed.

## Strings (`StringExtensions`)

`WithSafeFixed` overloads that pin a `String` as UTF-16. Delegate overloads (`StringExtensions`) are **.NET Standard 2.1 / .NET Core 3.0+**. Functional-interface overloads (`ReadOnlyFixedContextValueExtensions`) exist on every TFM.

## Sequences (`EnumerableSequenceExtensions`)

Helpers for `IEnumerableSequence<T>`: create enumerators and dispose them consistently across TFMs.

## Visual Basic

`Rxmxnx.PInvoke.VisualBasic` redeclares selected delegates so VB can call APIs that C# expresses with `ref`/`span` in a way VB cannot spell.

## See also

- [Fixed memory](fixed-memory.md) — pinning rules
- [Pointers](pointers.md) — typed pointer types
- [Use case: reinterpret memory](../use-cases.md#reinterpret-memory-without-copying)
- [TFM / API surface](compatibility.md)
