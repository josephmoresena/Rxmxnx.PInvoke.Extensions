# Pointers

Typed pointers wrap an unmanaged address so P/Invoke signatures and managed code can agree on what lives there. None of these types require the `unsafe` keyword at the call site.

They are **not** GC-safe by themselves. Pair them with [fixed memory](fixed-memory.md) when the target is a managed object.

## `ValPtr<T>`

A pointer to a mutable `T`.

**Implements:** `IWrapper<IntPtr>`, `IEquatable<ValPtr<T>>`, plus `IComparable`, `ISpanFormattable`, `ISerializable`, and (from .NET 7) `IParsable<ValPtr<T>>`.

**From .NET 9:** `T` may be a `ref struct`. Consumers targeting .NET 9+ should use **C# 13**.

| Member | Meaning |
| --- | --- |
| `Zero` | Null pointer. |
| `IsUnmanaged` | Whether `T` is an unmanaged type. |
| `Pointer` | The address as `IntPtr`. |
| `IsZero` | Whether the pointer is null. |
| `Reference` | `ref T` to the value at the address. |

Use `Reference` to read or write without an `unsafe` block in your code. The write is as dangerous as any native store: the address must be valid and writable.

On .NET 7+, `[NativeMarshalling]` sends this type across P/Invoke as a pointer to `T`.

## `ReadOnlyValPtr<T>`

Same shape as `ValPtr<T>`, but `Reference` is `ref readonly T`. The read-only contract is enforced by C#, not by the operating system.

Typical use: native APIs that take `const T*` (UTF-16 text, input structs, immutable buffers).

Convert between `ValPtr<T>` and `ReadOnlyValPtr<T>` when a host API is untyped or when you intentionally widen/narrow mutability.

## `FuncPtr<TDelegate>`

A pointer to a native function that can be invoked as `TDelegate`.

**Constraint:** `TDelegate` must be a non-generic `Delegate`. Invocation uses `Marshal.GetDelegateForFunctionPointer<TDelegate>`.

| Member | Meaning |
| --- | --- |
| `Zero` | Null function pointer. |
| `Pointer` | Address as `IntPtr`. |
| `IsZero` | Whether the pointer is null. |
| `Invoke` | The delegate bound to the native function. |

```csharp
FuncPtr<QueryFullProcessPath> query =
    (FuncPtr<QueryFullProcessPath>)NativeLibrary.GetExport(lib, "QueryFullProcessImageNameW");

Int32 result = query.Invoke(hProcess, 0, pathPtr, lengthPtr);
```

On .NET 7+, this type also participates in source-generated marshalling.

## Creating pointers

| Helper | When to use |
| --- | --- |
| `span.GetUnsafeValPtr()` / `GetUnsafeReadOnlyValPtr()` | Literals, `stackalloc`, or memory you have already pinned. |
| `NativeUtilities.GetUnsafeValPtr(in T)` | Address of a local or field you know will not move. |
| `NativeUtilities.GetUnsafeValPtrFromRef(ref T)` | Mutable address of a local or field. |
| `delegate.GetUnsafeFuncPtr()` | Function pointer to a managed delegate (the delegate must be kept alive). |
| Fixed context `.ValuePointer` | Address of memory that is pinned for the current scope. |

If the method name contains `Unsafe`, nothing is pinning the target. For heap memory, pin first.

## Formatting and parsing

All three pointer types format like `IntPtr` (`ISpanFormattable`). From .NET 7 they also parse from strings (`IParsable<T>`), which is useful in logs and diagnostics, not as a security boundary.

## See also

- [Fixed memory](fixed-memory.md) — how to obtain a pointer that is actually pinned
- [Use case: typed pointers](../use-cases.md#call-a-native-function-with-a-typed-pointer)
- [Use case: function pointers](../use-cases.md#load-a-native-export-as-a-typed-function-pointer)
