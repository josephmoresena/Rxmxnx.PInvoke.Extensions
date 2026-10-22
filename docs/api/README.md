# API reference

This section describes the public surface of `Rxmxnx.PInvoke.Extensions` by area. It is written to be read: types,
contracts, and the members you actually call. Overload-by-overload remarks still live in the XML documentation of the
source.

## Map

| Area                                              | What you will find                                                                    |
|---------------------------------------------------|---------------------------------------------------------------------------------------|
| [Pointers](pointers.md)                           | `ValPtr<T>`, `ReadOnlyValPtr<T>`, `FuncPtr<TDelegate>`                                |
| [Fixed memory](fixed-memory.md)                   | Fixed contexts, pointer values, memory lists, pinning APIs                            |
| [Functional interfaces](functional-interfaces.md) | Struct callbacks used instead of, or alongside, delegates                             |
| [UTF-8 text](cstring.md)                          | `CString`, `CStringSequence`, `CStringBuilder`                                        |
| [Buffers](buffers.md)                             | `BufferManager`, `ScopedBuffer<T>`, binary and non-binary spaces                      |
| [Wrappers and regions](wrappers.md)               | `IWrapper<T>`, `IReferenceable<T>`, `ValueRegion<T>`                                  |
| [Extensions](extensions.md)                       | Span, pointer, string, binary, and delegate helpers                                   |
| [Utilities](utilities.md)                         | `NativeUtilities`, `AotInfo`, `SystemInfo`, `TryCreateSpan` / `TryCreateReadOnlySpan` |
| [Enums](enums.md)                                 | `Iso639P1`                                                                            |
| [TFM / API surface](compatibility.md)             | Until 2.9.5 vs later targets; portable vs dedicated binaries                          |

## Design conventions

A few rules show up everywhere:

- **Read-only vs mutable.** `ReadOnly*` types are a .NET convention, not a runtime write-protect. Native code can still
  be written through the pointer.
- **Unsafe in the name.** `GetUnsafe*`, `CreateUnsafe`, and similar methods do not pin. The caller must guarantee the
  address stays valid.
- **Scoped lifetimes.** `WithSafeFixed` and `IDisposable` fixed contexts invalidate their pointers when the scope ends.
  Do not store `Pointer` on a field.
- **Functional interfaces alongside delegates.** New code should implement `IFixedContextAction<T>` (and friends) as
  callable types. Delegate overloads remain on the modern line. See [compatibility](compatibility.md).
- **Language floor.** **C# 7.3** minimum. See [language versions](../getting-started.md#language-versions).
- **.NET 9.0+ `ref struct`.** Many generic parameters `allows ref struct`. Some pointer helpers are patched in IL so
  those generic uses compile.
- **Marshaling on .NET 7.0+.** `CString`, `CStringSequence`, `ValPtr<T>`, `ReadOnlyValPtr<T>`, and `FuncPtr<TDelegate>`
  participate in source-generated P/Invoke.

If a member is missing on your TFM, start with [compatibility](compatibility.md). XML docs in the source remain the
complete member-level contract.

## Namespaces

Almost everything lives in `Rxmxnx.PInvoke`. Visual Basic helpers live in `Rxmxnx.PInvoke.VisualBasic`. Buffer types
that you register or compose live in `Rxmxnx.PInvoke.Buffers` as well as the root namespace.