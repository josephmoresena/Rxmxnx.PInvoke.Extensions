# Use cases

Short recipes for the problems this library is meant to solve. Each example is complete enough to copy; names and signatures match the public API. For the “why”, see [Capabilities](capabilities.md). For members, see the [API reference](api/README.md).

## Call a native function with a typed pointer

Typed pointers keep P/Invoke signatures honest. `ReadOnlyValPtr<T>` is a convention for “this memory should not be written”; `ValPtr<T>` is the writable counterpart.

```csharp
[DllImport("user32.dll", EntryPoint = "MessageBoxW")]
static extern Int32 MessageBox(
    IntPtr hWindow,
    ReadOnlyValPtr<Char> textPtr,
    ReadOnlyValPtr<Char> captionPtr,
    UInt32 type);

using IDisposable pin = "Hello".AsMemory().GetFixedContext(out ReadOnlyFixedContextValue<Char> text);
_ = MessageBox(IntPtr.Zero, text.ValuePointer, "Greeting".AsSpan().GetUnsafeValPtr(), 0);
```

`GetUnsafeValPtr()` is appropriate for string literals and other memory that will not move. For heap memory, pin it first with `GetFixedContext(out …)` or `WithSafeFixed`.

On .NET Standard 2.1 / .NET Core 3.0+ you can also write `using IReadOnlyFixedContext<Char>.IDisposable text = "Hello".AsMemory().GetFixedContext();`.

## Pin managed memory only for the native call

`WithSafeFixed` pins a span, runs your callback, and unpins when the callback returns. The pointer is not stored; it cannot outlive the scope. The form that compiles on every TFM is a functional interface:

```csharp
Span<Byte> utf8 = "Hello world"u8.ToArray();

readonly struct PrintUtf8 : IFixedContextAction<Byte>
{
    public void Accept(scoped FixedContextValue<Byte> ctx)
        => Console.WriteLine(Marshal.PtrToStringUTF8(ctx.Pointer));
}

utf8.WithSafeFixed(new PrintUtf8());
```

On .NET Standard 2.1 / .NET Core 3.0+ a delegate lambda is also public:

```csharp
utf8.WithSafeFixed(static (in IFixedContext<Byte> ctx) =>
{
    Console.WriteLine(Marshal.PtrToStringUTF8(ctx.Pointer));
});
```

## Allocate native memory and free it with Dispose

When a native API needs a buffer that survives a single callback, allocate it on the native heap and let `IDisposable` release it.

```csharp
using IDisposable _ = NativeUtilities.HeapAlloc<Byte>(256, out FixedContextValue<Byte> buffer);
buffer.Values.Clear();
// Pass buffer.ValuePointer to native code.
// Dispose frees the block.
```

On .NET Standard 2.1 / .NET Core 3.0+ there is also `NativeUtilities.HeapAlloc<Byte>(256)` returning `IFixedContext<Byte>.IDisposable`.

## Build UTF-8 once, marshal many times

UTF-8 literals and `CString` avoid encoding the same text on every P/Invoke.

```csharp
CString resource = new(() => "devices/usb0"u8);

[DllImport("libexample")]
static extern Int32 OpenResource(ReadOnlyValPtr<Byte> path);

resource.WithSafeFixed(new Open());

readonly struct Open : IReadOnlyFixedContextAction<Byte>
{
    public void Accept(scoped ReadOnlyFixedContextValue<Byte> ctx)
        => OpenResource(ctx.ValuePointer);
}
```

On .NET 7+, `CString` supports source-generated marshalling as a null-terminated UTF-8 string, so a `[LibraryImport]` declaration can take `CString` directly.

Creating a `CString` from a `String` with the explicit operator encodes to UTF-8 and always null-terminates. Prefer the literal constructor for constants; it does not copy.

## Pass an argument vector to native code

`CStringSequence` stores consecutive null-terminated UTF-8 strings in one buffer — the same layout as `char* argv[]` after marshalling.

```csharp
CStringSequence args = CStringSequence.CreateBuilder()
    .Append("bash"u8)
    .Append("-c"u8)
    .Append("echo \"Hello $ENV_USER\""u8)
    .Build();

CStringSequence env = CStringSequence.CreateBuilder()
    .Append($"ENV_USER={Environment.UserName}")
    .Append($"PATH={Environment.GetEnvironmentVariable("PATH")}")
    .Build();
```

On .NET 7+, marshalling a `CStringSequence` produces a null-terminated array of null-terminated UTF-8 strings. Empty items are omitted. `CStringSequence.Utf8View` enumerates items as `ReadOnlySpan<Byte>` without allocating `CString` wrappers.

Hardcoded sequences can skip a copy when the UTF-16 string already contains the UTF-8 bytes:

```csharp
const String packed = "效汬o潗汲d"; // UTF-16 whose bytes are "Hello\0World\0"
CStringSequence seq = CStringSequence.Parse(packed);
```

## Grow UTF-8 text like StringBuilder

`CStringBuilder` appends strings, UTF-8 spans, bytes, and numbers, then materializes a `CString` that is null-terminated by default.

```csharp
CStringBuilder csb = new();
csb.Append("Hello");
csb.Append((Byte)' ');
csb.Append("World"u8);
csb.AppendLine();
csb.Append(2026);

CString result = csb.ToCString();          // null-terminated, ready for interop
CString slice = csb.ToCString(false);      // no extra terminator
csb.CopyTo(0, stackalloc Byte[5]);         // copy into an existing span
```

## Reinterpret memory without copying

XOR, hashing, and serialization often want a byte view of values you already have.

```csharp
Span<Char> chars = "Hello world".ToCharArray();
Span<Int32> values = chars.AsValues<Char, Int32>(out Span<Byte> bytes);

foreach (ref Int32 val in values)
    val ^= 181;

foreach (ref Byte b in bytes)
    b = (Byte)(b ^ 5);

Console.WriteLine(chars.ToString());
```

`AsBytes` / `AsValues` are views. `ToBytes` / `ToValue` make a copy when you need a snapshot that outlives the source.

## Use a stack buffer in a hot parser

`BufferManager` allocates a scoped buffer for a callback. Unmanaged values can live in `stackalloc`; reference types use managed buffer structs placed on the stack when possible.

```csharp
readonly struct CollectNames : IScopedBufferAction<String>
{
    public UInt16 Count => 4;
    public Boolean IsMinimalCount => false;

    public void Accept(scoped ScopedBuffer<String> buffer)
    {
        Span<String> names = buffer.Span;
        names[0] = "alpha";
        names[1] = "beta";
        // Use names inside this scope only.
    }
}

BufferManager<String>.Alloc(new CollectNames());
```

The delegate form of `BufferManager.Alloc` exists on .NET Standard 2.1 / .NET Core 3.0+ only:

```csharp
BufferManager.Alloc<String>(4, static buffer =>
{
    buffer.Span[0] = "alpha";
});
```

On Native AOT, register or prepare binary buffer metadata so composition does not rely on runtime reflection. See [Buffers](api/buffers.md#aot-and-registration).

## Load a native export as a typed function pointer

`NativeLibrary` helpers require **.NET Core 3.0 or later** (`NativeUtilities.LoadNativeLib` / `GetNativeMethod<TDelegate>`). The `FuncPtr<TDelegate>` type itself exists on every TFM.

```csharp
delegate Int32 QueryFullProcessPath(
    IntPtr hProcess,
    UInt32 dwFlags,
    ValPtr<Char> pathPtr,
    ValPtr<Int32> pathLengthPtr);

IntPtr lib = NativeLibrary.Load("kernel32.dll");
try
{
    FuncPtr<QueryFullProcessPath> fn =
        (FuncPtr<QueryFullProcessPath>)NativeLibrary.GetExport(lib, "QueryFullProcessImageNameW");

    Span<Char> path = stackalloc Char[260];
    Int32 length = path.Length;
    if (fn.Invoke(Process.GetCurrentProcess().Handle, 0, path.GetUnsafeValPtr(),
            NativeUtilities.GetUnsafeValPtrFromRef(ref length)) != 0)
    {
        Console.WriteLine(path[..length].ToString());
    }
}
finally
{
    NativeLibrary.Free(lib);
}
```

`NativeUtilities.LoadNativeLib` and `GetNativeMethod<TDelegate>` wrap the same pattern with optional unload events.

## Adapt behavior for Native AOT or Mono

```csharp
if (AotInfo.IsNativeAot)
{
    // Skip reflection-based buffer auto-composition; use registered buffers.
}

if (SystemInfo.IsMonoRuntime && !myDelegate.IsImageMethod())
{
    // The target is generated IL; do not assume a stable function pointer.
}

if ("Hello"u8.IsLiteral())
{
    // The span points at image data; pinning is unnecessary.
}
```

`AotInfo.IsNativeAot` is reliable on CoreCLR (including R2R and Native AOT). On Mono it may depend on when it is first read, because Mono AOT does not compile an entire assembly at once.

## Pin several buffers for one native call

Some native APIs take multiple pointers that must all remain valid for the duration of the call. `WithSafeFixed` can pin up to eight spans and hand them over as a list.

```csharp
readonly struct CallNative : IFixedPointerListAction
{
    public void Accept(scoped FixedPointerValueList list)
    {
        // list[0], list[1], ... stay pinned until Accept returns.
    }
}

Span<Byte> header = stackalloc Byte[16];
Span<Byte> payload = new Byte[1024];
new CallNative().WithSafeFixed(header, payload);
```

## Wrap a value so an API can hold it without knowing the storage

```csharp
IWrapper<Int32> boxed = WrapperFactory.Create(42);
IMutableReference<String> slot = WrapperFactory.CreateReferenceableObject("initial");
slot.Value = "updated";
ref String live = ref slot.Reference;
```

On .NET Standard 2.1 / .NET Core 3.0+ you can also write `IWrapper.Create(42)` and `IMutableReference.CreateObject("initial")`. Those static factories are default interface methods and are not on Reach.

Use wrappers when you need a uniform `T` handle across value types, nullables, and reference types — for example, a callback payload or a diagnostic dump.

## Decide quickly

| If you need… | Use |
| --- | --- |
| A UTF-8 string for a native parameter | `CString` |
| `argv` / env-style lists | `CStringSequence` |
| Growing UTF-8 text | `CStringBuilder` |
| A typed pointer in a signature | `ValPtr<T>` / `ReadOnlyValPtr<T>` / `FuncPtr<T>` |
| A pointer that dies with the callback | `WithSafeFixed` + functional interface |
| A native buffer with `using` | `NativeUtilities.HeapAlloc<T>` |
| A byte view of existing memory | `AsBytes` / `AsValues` |
| Temporary stack storage | `BufferManager` / `IScopedBufferAction<T>` |
| AOT or OS branching | `AotInfo` / `SystemInfo` |
