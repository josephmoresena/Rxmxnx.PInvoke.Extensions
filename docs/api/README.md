# API reference

This section describes the public surface of `Rxmxnx.PInvoke.Extensions` by area. It is written to be read: types, contracts, and the members you actually call. Overload-by-overload remarks still live in the XML documentation of the source.

## Map

| Area | What you will find |
| --- | --- |
| [Pointers](pointers.md) | `ValPtr<T>`, `ReadOnlyValPtr<T>`, `FuncPtr<TDelegate>` |
| [Fixed memory](fixed-memory.md) | Fixed contexts, pointer values, memory lists, pinning APIs |
| [Functional interfaces](functional-interfaces.md) | Struct callbacks used instead of, or alongside, delegates |
| [UTF-8 text](cstring.md) | `CString`, `CStringSequence`, `CStringBuilder` |
| [Buffers](buffers.md) | `BufferManager`, `ScopedBuffer<T>`, binary and non-binary spaces |
| [Wrappers and regions](wrappers.md) | `IWrapper<T>`, `IReferenceable<T>`, `ValueRegion<T>` |
| [Extensions](extensions.md) | Span, pointer, string, binary, and delegate helpers |
| [Utilities](utilities.md) | `NativeUtilities`, `AotInfo`, `SystemInfo` |
| [Enums](enums.md) | `Iso639P1` |
| [TFM / API surface](compatibility.md) | What exists on .NET Framework, UWP, and .NET Standard 2.0 |

## Design conventions

A few rules show up everywhere:

- **Read-only vs mutable.** `ReadOnly*` types are a .NET convention, not a runtime write-protect. Native code can still write through the pointer.
- **Unsafe in the name.** `GetUnsafe*`, `CreateUnsafe`, and similar methods do not pin. The caller must guarantee the address stays valid.
- **Scoped lifetimes.** `WithSafeFixed` and `IDisposable` fixed contexts invalidate their pointers when the scope ends. Do not store `Pointer` on a field.
- **Functional interfaces alongside delegates.** New code should implement `IFixedContextAction<T>` (and friends) as a `readonly struct`. Delegate overloads remain public on .NET Standard 2.1 / .NET Core 3.0 and later; they were not added to the .NET Framework, .NET Standard 2.0, .NET Core 2.1, or UWP assemblies. See [compatibility](compatibility.md).
- **.NET 9+ `ref struct`.** Many generic parameters `allows ref struct`. Some pointer helpers are patched in IL so those generic uses compile.
- **Marshalling on .NET 7+.** `CString`, `CStringSequence`, `ValPtr<T>`, `ReadOnlyValPtr<T>`, and `FuncPtr<TDelegate>` participate in source-generated P/Invoke.

## Namespaces

Almost everything lives in `Rxmxnx.PInvoke`. Visual Basic helpers live in `Rxmxnx.PInvoke.VisualBasic`. Buffer types that you register or compose live in `Rxmxnx.PInvoke.Buffers` as well as the root namespace.

## Compatibility notes you will see often

- **Support TFMs (this generation).** .NET Standard 2.0, .NET Core 2.1, .NET Framework 4.5.2–4.7.2, and UWP 10.0.16299 expose functional interfaces and value-type contexts. They do **not** include delegate `WithSafeFixed` / `BufferManager.Alloc(delegate)` or nested `IFixedContext<T>.IDisposable` helpers.
- **.NET Standard 2.1 / .NET Core 3.0+.** Full historical API plus the new types. Static factories on non-generic `IWrapper` (default interface methods).
- **.NET 7+.** Static virtual members (`IUtf8FunctionState<TSelf>`), `[NativeMarshalling]`, and `LibraryImport`.
- **.NET 9+.** `params ReadOnlySpan<T>` instead of arrays; `allows ref struct` on pointers, wrappers, and many callbacks.

If a member is missing on your TFM, start with [compatibility](compatibility.md). XML docs in the source remain the complete member-level contract.
