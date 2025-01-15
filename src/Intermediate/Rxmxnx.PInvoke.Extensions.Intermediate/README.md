The `Rxmxnx.PInvoke.Extensions` library provides various methods and extensions to simplify working with unmanaged
pointers and managed memory handling.

---

## Managed Memory Handling

The managed memory methods and extensions enable operations such as transformations and pinning. These methods are safe
because, although they internally use the `Unsafe` class, they exclusively operate on GC-managed references. This
ensures that the memory adheres to the .NET runtime safety guarantees.

```csharp
Span<Char> chars = "Hello world".AsSpan().ToArray();
Span<Int32> values = chars.AsValues<Char, Int32>(out Span<Byte> bytes);

Console.WriteLine(chars.ToString());    // Output: "Hello world"

foreach(ref Int32 val in values)
    val = val ^ 181;

foreach(ref Byte val in bytes)
    val = (Byte)(val ^ 5);

Console.WriteLine(chars.ToString());    // Output: "ýeÙlÚ ÂoÇlա"

foreach(ref Int32 val in values)
    val = val ^ 181;

foreach(ref Byte val in bytes)
    val = (Byte)(val ^ 5);

Console.WriteLine(chars.ToString());    // Output: "Hello world"
```

Additionally, by using `fixed` contexts, these methods allow managed memory to be accessed via pointers while
guaranteeing that the memory remains fixed in place using the GC. This approach provides both safety and flexibility for
scenarios requiring low-level memory access.

```csharp
Span<Byte> c8 = "Hello world"u8.ToArray();
Int16[] c16 = new Int16[c8.Length];

c8.WithSafeFixed(c16, (in IFixedContext<Byte> ctx, Int16[] state) => {
    
    GC.Collect();
    GC.WaitForFullGCComplete();
    
    String text = Marshal.PtrToStringUTF8(ctx.Pointer);
    
    Console.WriteLine(text);    // Output: "Hello world"
    
    text.AsSpan().AsBytes().CopyTo(state.AsSpan().AsBytes());
});

using (IFixedContext<Int16>.IDisposable ctx = c16.AsMemory().GetFixedContext())
    Console.WriteLine(Marshal.PtrToStringUni(ctx.Pointer)); // Output: "Hello world"
```

---

## Unsafe Methods and Extensions

Unsafe methods and extensions utilize unmanaged pointers. These methods are inherently unsafe because ensuring the
validity of the memory address being pointed to must be handled manually.

```csharp
Int32 i = 10;
ref Int32 refI = ref i;
IntPtr u16Ptr = "Hello world".AsSpan().GetUnsafeIntPtr();
IntPtr u8Ptr = "Hello world"u8.GetUnsafeIntPtr();
IntPtr lPtr = stackalloc Int64[4]
{
    0x502e786e786d7852, // Hardcoded UTF-8: "Rxmxnx.P"
    0x452e656b6f766e49, // Hardcoded UTF-8: "Invoke.E"
    0x6e6f69736e657478, // Hardcoded UTF-8: "xtension"
    0x73,               // Hardcoded UTF-8: "s"
}.GetUnsafeIntPtr();
IntPtr iPtr = refI.GetUnsafeIntPtr();

GC.Collect();
GC.WaitForFullGCComplete();
i = 20;

Console.WriteLine(Marshal.PtrToStringUni(u16Ptr));  // Output: "Hello world"
Console.WriteLine(Marshal.PtrToStringUTF8(u8Ptr));  // Output: "Hello world"
Console.WriteLine(Marshal.PtrToStringUTF8(lPtr));   // Output: "Rxmxnx.PInvoke.Extensions"
Console.WriteLine(Marshal.ReadInt32(iPtr));         // Output: 20
```

These methods should only be used with constants, literals, or pointers to stack-allocated memory,
but they can also be used in scenarios involving native memory (known to be fixed) or managed memory allocated on the
heap, fixed explicitly using `fixed` or `GCHandle.Alloc`.

```csharp
Memory<Int32> values = new Int32[]
{
    0x650048,   // Hardcoded UTF-16: "He"
    0x6c006c,   // Hardcoded UTF-16: "ll"
    0x20006f,   // Hardcoded UTF-16: "o "
    0x6f0057,   // Hardcoded UTF-16: "wo"
    0x6c0072,   // Hardcoded UTF-16: "rl"
    0x64        // Hardcoded UTF-16: "d"
};
using MemoryHandle handle = values.Pin();

GC.Collect();
GC.WaitForFullGCComplete();

IntPtr u16Ptr = values.Span.GetUnsafeIntPtr();
Console.WriteLine(Marshal.PtrToStringUni(u16Ptr));  // Output: "Hello world"
```

---

## Typed Pointers

Typed pointers are structures that encapsulate unmanaged pointers, allowing their use without requiring the `unsafe`
keyword. There are three types of typed pointers:

- **`ValPtr<T>`**: Encapsulates a pointer to an unmanaged reference of type `T`.
- **`ReadOnlyValPtr<T>`**: Similar to `ValPtr<T>` but represents a read-only unmanaged reference (enforced by convention
  within .NET, not by runtime constraints).
- **`FuncPtr<TDelegate>`**: Encapsulates a pointer to a function of type `TDelegate`. The delegate type must not be
  generic because internally the `Marshal.GetDelegateForFunctionPointer<TDelegate>` method is used to invoke the
  function.

```csharp
delegate Int32 QueryFullProcessPath(IntPtr hProcess, UInt32 dwFlags, ValPtr<Char> pathPtr, ValPtr<Int32> pathLengthPtr);
...
if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;

[DllImport("user32.dll", EntryPoint = "MessageBoxW")]
static extern Int32 MessageBox(IntPtr hWindow, ReadOnlyValPtr<Char> textPtr, ReadOnlyValPtr<Char> captionPtr, UInt32 type);

IntPtr libHandle = NativeLibrary.Load("kernel32.dll");
try
{
    FuncPtr<QueryFullProcessPath> queryFullProcessPathPtr = 
        (FuncPtr<QueryFullProcessPath>)NativeLibrary.GetExport(libHandle, "QueryFullProcessImageNameW");
    Int32 pathLength = 50;
    QueryFullProcessPath queryFullProcessPath = queryFullProcessPathPtr.Invoke;
    IntPtr hProcess = Process.GetCurrentProcess().Handle;
    String? processPath;
    while ((processPath = GetProcessPath(hProcess, queryFullProcessPath, ref pathLength)) is null)
        pathLength *= 2;

    using IReadOnlyFixedContext<Char>.IDisposable pathCtx = processPath.AsMemory().GetFixedContext();
    _ = MessageBox(IntPtr.Zero, pathCtx.ValuePointer, "Process Path".AsSpan().GetUnsafeValPtr(), 0);
}
finally
{
    NativeLibrary.Free(libHandle);
}

static String? GetProcessPath(IntPtr hProcess, QueryFullProcessPath queryFullProcessPath, ref Int32 pathLength) 
{
    Span<Char> pathChars = stackalloc Char[pathLength];
    ValPtr<Char> pathPtr = pathChars.GetUnsafeValPtr();
    if (queryFullProcessPath(hProcess, 0, pathPtr, pathLength.GetUnsafeValPtr()) != 0)
        return String.Create(pathLength, pathPtr.Pointer, (span, ptr) => {
            ReadOnlySpan<Char> pathSpan = ptr.GetUnsafeReadOnlySpan<Char>(span.Length);
            pathSpan.CopyTo(span);
        });
    return default;
}
```

These types are ideal for generating strongly-typed declarations of PInvoke functions, ensuring safer and more intuitive
interop with unmanaged code.