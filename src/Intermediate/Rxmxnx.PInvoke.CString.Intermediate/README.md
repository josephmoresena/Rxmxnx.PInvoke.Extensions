`Rxmxnx.PInvoke.Extensions` supports UTF-8/ASCII text handling through the `CString` class, inspired by
C-style strings. These strings provide a simple and safe way to access UTF-8/ASCII units, regardless of their nature.

There are three types of `CString`:

1. **Managed Buffer-Based**: This type is based on a managed buffer containing UTF-8/ASCII units.
2. **Delegate-Based**: This type relies on a delegate that returns a `Span<Byte>`. The delegate can represent a UTF-8 or
   ASCII literal, or an arbitrary function, with or without a state object.
3. **Unmanaged Pointer-Based**: This type is based on an unmanaged pointer and a specified length. When use this type,
   the developer have to ensure that the memory address holding the UTF-8/ASCII units remains valid for the
   lifetime of the `CString` instance.

---

## CString Initialization

The `CString` class offers various constructors and operators for creating `CString` instances:

### Implicit operator

```csharp
Byte[] utf8Data = [ (Byte)'H', (Byte)'e', (Byte)'l', (Byte)'l', (Byte)'o', (Byte)'\0', ];
Byte[] utf8Data2 = utf8Data[..^1];
CString cstring = (CString)utf8Data;

Console.WriteLine(cstring);                     // Output: "Hello"
Console.WriteLine(cstring.IsNullTerminated);    // Output: True

cstring = utf8Data[..^1];

Console.WriteLine(cstring);                     // Output: "Hello"
Console.WriteLine(cstring.IsNullTerminated);    // Output: False
```

This operator converts a `Byte[]` containing UTF-8/ASCII units into a `CString` instance. If
the last element of the array is a null character (`0x0`), it is excluded from the length count, and the
`IsNullTerminated` property is set to `true`.

### Explicit operator

```csharp
String text = new("Hello".AsSpan());
CString cstring = (CString)text;

Console.WriteLine(cstring);                     // Output: "Hello"
Console.WriteLine(cstring.IsNullTerminated);    // Output: True
```

This operator encodes a UTF-16 string into UTF-8, creating a managed buffer to store the
resulting bytes. It always reserves space for a null terminator, ensuring the `IsNullTerminated` property is always
`true`.

**Note:** This operator is considered *inefficient* when used with string literals.

### Literal constructor

```csharp
CString cstring = new(()=> "Hello"u8);

Console.WriteLine(cstring);                     // Output: "Hello"
Console.WriteLine(cstring.IsNullTerminated);    // Output: True
```

This constructor enables the creation of `CString` instances
using [UTF-8/ASCII literals](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-11.0/utf8-string-literals).

**Note:** This constructor is considered **primary** because these literals are constants, ensuring their memory
location
remains fixed.

### Span constructor

```csharp
ReadOnlySpan<Byte> utf8Data = [ (Byte)'H', (Byte)'e', (Byte)'l', (Byte)'l', (Byte)'o', (Byte)'\0', ];
CString cstring = new(utf8Data);

Console.WriteLine(cstring);                     // Output: "Hello"
Console.WriteLine(cstring.IsNullTerminated);    // Output: True

cstring = new(utf8Data[..^1]);

Console.WriteLine(cstring);                     // Output: "Hello"
Console.WriteLine(cstring.IsNullTerminated);    // Output: True
```

This constructor creates a `CString` instance by copying the content of a
`ReadOnlySpan<Byte>` containing UTF-8/ASCII units into a managed buffer. It always appends a null character to the
buffer, and the `IsNullTerminated` property is set to `true`.

### Creation with span

```csharp
ReadOnlySpan<Byte> utf8Data = [ (Byte)'H', (Byte)'e', (Byte)'l', (Byte)'l', (Byte)'o', (Byte)'\0', ];
CString cstring = CString.Create(utf8Data);

Console.WriteLine(cstring);                     // Output: "Hello\0"
Console.WriteLine(cstring.IsNullTerminated);    // Output: False

cstring = CString.Create(utf8Data[..^1]);

Console.WriteLine(cstring);                     // Output: "Hello"
Console.WriteLine(cstring.IsNullTerminated);    // Output: False
```

This method creates a `CString` instance by copying the content of a
`ReadOnlySpan<Byte>` into a managed buffer without verifying the presence of a null terminator. The
`IsNullTerminated` property is always set to `false`.

### Creation with state

```csharp
private readonly struct BytesSlice : IUtf8FunctionState<BytesSlice>
{
   public Byte[] Bytes { get; init; }
   public Int32 Start { get; init; }
   public Int32? End { get; init; }
   
   public Boolean IsNullTerminated => (!this.End.HasValue || this.End == this.Bytes.Length) && this.Bytes.Length > 0 && this.Bytes[^1] == 0x0;
   
   public static Int32 GetLength(in BytesSlice state) {
      Int32 length = state.End ?? state.Bytes.Length;
      if (state.IsNullTerminated)
         length--;
      return length - state.Start;
   }
     
   public static ReadOnlySpan<Byte> GetSpan(BytesSlice state) {
      return state.Bytes.AsSpan().Slice(state.Start, GetLength(state));
   }
}
...
Byte[] utf8Data = [ (Byte)'H', (Byte)'e', (Byte)'l', (Byte)'l', (Byte)'o', (Byte)'\0', ]; 
CString cstring = CString.Create(new BytesSlice() {
   Bytes = utf8Data,
   Start = 0,
});

Console.WriteLine(cstring);                     // Output: "Hello"
Console.WriteLine(cstring.IsNullTerminated);    // Output: True

cstring = CString.Create(new BytesSlice() {
   Bytes = utf8Data,
   Start = 2,
});

Console.WriteLine(cstring);                     // Output: "llo"
Console.WriteLine(cstring.IsNullTerminated);    // Output: True

cstring = CString.Create(new BytesSlice() {
   Bytes = utf8Data,
   Start = 0,
   End = 4,
});

Console.WriteLine(cstring);                     // Output: "Hell"
Console.WriteLine(cstring.IsNullTerminated);    // Output: False
```

This method allows creating a `CString` instance using an object of type `IUtf8FunctionState<TState>`, which provides
UTF-8/ASCII units using a delegate.

In .NET 6.0 and earlier, this interface does not exist, but the same functionality can be achieved using a
state-and-delegate-based approach.

### Creation with pointer

```csharp
const Int32 length = 6;
Byte* utf8Data = stackalloc Byte[length] { (Byte)'H', (Byte)'e', (Byte)'l', (Byte)'l', (Byte)'o', (Byte)'\0', };
IntPtr ptr = (IntPtr)utf8Data;  // Get stack pointer (safe).
CString? cstring = CString.CreateUnsafe(ptr, length, false);

Console.WriteLine(cstring);                     // Output: "Hello"
Console.WriteLine(cstring.IsNullTerminated);    // Output: True

cstring = CString.CreateUnsafe(ptr, length, true);

Console.WriteLine(cstring);                     // Output: "Hello\0"
Console.WriteLine(cstring.IsNullTerminated);    // Output: False

cstring = null; // Discard unsafe instance
```

This method creates a `CString` instance that points to an unmanaged
memory block containing UTF-8/ASCII units of a specified length. If the full length is used, the `IsNullTerminated`
property is set to `false`; otherwise, it is set to `true`.

**Note:** This method is considered *unsafe* because the pinning of the memory block pointed to by the resulting
instance must be done manually.

---

## CString Sequences

`Rxmxnx.PInvoke.Extensions` natively supports the creation of null-terminated UTF-8/ASCII sequences to optimize the use
of managed buffers via the `CStringSequence` class.

```csharp
const String val = "效汬o潗汲d";    // Hardcoded UTF-8: "Hello\0World\0"
CStringSequence seq0 = new("Hello", "World");
CStringSequence seq1 = new("Hello"u8, "World"u8);
CStringSequence seq2 = new(Encoding.UTF8.GetBytes("Hello"), Encoding.UTF8.GetBytes("World"));
CStringSequence seq3 = new(new CString(() => "Hello"u8), new CString(() => "World"u8));
CStringSequence seq4 = CStringSequence.Parse(val);
CStringSequence seq5 = CStringSequence.Create(val);

Console.WriteLine(seq0.Count);  // Output: 2
Console.WriteLine(seq4.Count);  // Output: 2
Console.WriteLine(seq5.Count);  // Output: 2

Console.WriteLine(seq0.ToString() == seq1.ToString());  // Output: True
Console.WriteLine(seq0.ToString() == seq2.ToString());  // Output: True
Console.WriteLine(seq0.ToString() == seq3.ToString());  // Output: True
Console.WriteLine(seq0.ToString() == seq4.ToString());  // Output: True
Console.WriteLine(seq0.ToString() == seq5.ToString());  // Output: True

Console.WriteLine(Object.ReferenceEquals(val, seq0.ToString()));    // Output: False
Console.WriteLine(Object.ReferenceEquals(val, seq1.ToString()));    // Output: False
Console.WriteLine(Object.ReferenceEquals(val, seq2.ToString()));    // Output: False
Console.WriteLine(Object.ReferenceEquals(val, seq3.ToString()));    // Output: False
Console.WriteLine(Object.ReferenceEquals(val, seq4.ToString()));    // Output: True
Console.WriteLine(Object.ReferenceEquals(val, seq5.ToString()));    // Output: False
```

`CStringSequence` instances can be created from sequences of `String`, `CString`, or up to 8 `ReadOnlySpan<Byte>`
elements. In all cases, a buffer is created to ensure that each element in the sequence is stored consecutively,
separated by a null character. The buffer itself is also null-terminated, ensuring compatibility with interop scenarios.

The `Parse(String)` method is the only one that might avoid creating a new buffer if the input `String` meets the
criteria to serve directly as the buffer for a `CStringSequence` instance. This is particularly useful when constant
UTF-8/ASCII sequences (hardcoded) are required.

The `GetUnsafe(ReadOnlySpan<ReadOnlyValPtr<Byte>>)` method enables the creation of a `CStringSequence` instance from an
unmanaged `char*[]`, as represented by the given span.

## CString Builder

`Rxmxnx.PInvoke.Extensions` provides a mutable sequence of UTF-8 characters through the `CStringBuilder` class. This
class is designed as a UTF-8 equivalent of `System.Text.StringBuilder`.

### Construction and Appending

```csharp
CStringBuilder csb = new();

csb.Append("Hello");                 // Append String
csb.Append((Byte)' ');               // Append Byte character
csb.Append("World"u8);               // Append UTF-8 literal (ReadOnlySpan<Byte>)
csb.AppendLine();                    // Append Line terminator
csb.Append(2023);                    // Append Int32

Console.WriteLine(csb.ToString());   
// Output: 
// Hello World
// 2023
```

This snippet demonstrates how to initialize a `CStringBuilder` and append different data types. The class automatically
handles the UTF-8 encoding of C# strings and numeric values.

### Joining Values

```csharp
CStringBuilder csb = new();
CString[] values = [ new(() => "A"u8), new(() => "B"u8), new(() => "C"u8) ];

csb.AppendJoin(", "u8, values);

Console.WriteLine(csb.ToString());   // Output: "A, B, C"

csb.Clear();
csb.AppendJoin(" - "u8, new(() => "1"u8), new(() => "2"u8), new(() => "3"u8));

Console.WriteLine(csb.ToString());   // Output: "1 - 2 - 3"
```

The `AppendJoin` method simplifies the concatenation of collections or sequences of values, inserting a specified
separator between each element.

### Manipulation

```csharp
CStringBuilder csb = new("Hello World"u8);

csb.Remove(5, 6);        // Removes " World"
csb.Insert(5, " C#");    // Inserts " C#" at index 5

Console.WriteLine(csb.ToString());   // Output: "Hello C#"
```

`CStringBuilder` supports `Insert` and `Remove` operations effectively. These operations modify the internal chunks,
shifting data as necessary.

### Conversion to CString

```csharp
CStringBuilder csb = new("Text");

CString cstr1 = csb.ToCString();
CString cstr2 = csb.ToCString(false);

Console.WriteLine(cstr1.IsNullTerminated); // Output: True (Default)
Console.WriteLine(cstr2.IsNullTerminated); // Output: False (Explicit)
```

The `ToCString()` method creates a new `CString` instance containing the current data. By default, this method ensures
the resulting `CString` is null-terminated, making it ready for interop scenarios. The overload
`ToCString(bool nullTerminated)` allows the developer to prevent the addition of the null terminator (by passing
`false`) if it is not required.

### Low-Level Copy

```csharp
CStringBuilder csb = new("12345");
Span<Byte> buffer = stackalloc Byte[3];

csb.CopyTo(1, buffer); // Copy 3 bytes starting from index 1

Console.WriteLine(Encoding.UTF8.GetString(buffer)); // Output: "234"
```

For high-performance scenarios, the `CopyTo` method allows direct copying of the builder's content into a destination
`Span<Byte>`, enabling interaction with other low-level APIs without allocating intermediate objects.

---

## Sequence Builder

`CStringSequence.Builder` is an structure that enables the dynamic, incremental construction of UTF-8 null-terminated
text sequences.
Its usage and design closely mirror `CStringBuilder`, extending the same construction model from single UTF-8 text
values to *sequences* of them.

### Similarities with `CStringBuilder`

`CStringSequence.Builder` follows the same core principles as `CStringBuilder`:

* Fluent, chainable API
* Incremental construction
* UTF-8–centric design
* Support for mixed input sources (`String`, UTF-8 spans, `CString`)
* Deferred materialization into an immutable result.

### Dynamic Appending

Elements can be appended progressively using chained calls:

```csharp
CStringSequence args = CStringSequence.CreateBuilder()
    .Append("bash"u8)
    .Append("-c"u8)
    .Append("echo \"Hello $ENV_USER\""u8)
    .Build();
```

Each `Append` call behaves analogously to `CStringBuilder.Append`, but instead of extending a single string, it appends
a new element to the sequence. Appending a `null` value results in an `CString.Zero` element.

### Runtime Composition

As with `CStringBuilder`, composition can depend entirely on runtime state:

```csharp
CStringSequence envs = CStringSequence.CreateBuilder()
    .Append($"ENV_USER={Environment.UserName}")
    .Append($"PATH={Environment.GetEnvironmentVariable("PATH")}")
    .Build();
```

This pattern allows sequences to be assembled conditionally without pre-allocating buffers or arrays.

### Structural Operations

Insertion and removal operations are conceptually equivalent to their `CStringBuilder` counterparts, but operate at
the **item level** instead of the character level:

```csharp
CStringSequence seq = CStringSequence.CreateBuilder()
    .Append("Hello"u8)
    .Append("World"u8)
    .Insert(1, Environment.UserName)
    .RemoveAt(2)
    .Build();
```

These operations allow reordering and selective exclusion of elements prior to finalization.

### Escaped UTF-8 Handling

Just like `CStringBuilder`, escaped UTF-8 input can be appended and unescaped during construction:

```csharp
CStringSequence seq = CStringSequence.CreateBuilder()
    .AppendEscaped("Line\\nValue"u8)
    .AppendEscaped("Tab\\tValue"u8)
    .Build();
```

The unescaped representation is written directly into the final sequence buffer.

### Finalization

Calling `Build()` captures the current state of `CStringSequence.Builder` and produces the resulting sequence. At this
point, the builder’s role is complete, and the produced sequence is ready for immediate use in native interop scenarios.

