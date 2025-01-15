`Rxmxnx.PInvoke.Extensions` supports UTF-8/ASCII text handling through the `CString` class, inspired by
C-style strings. These strings provide a simple and safe way to access UTF-8/ASCII units, regardless of their nature.

There are three types of `CString`:

1. **Managed Buffer-Based**: This type is based on a managed buffer, which can be either a `String` instance or a
   `Byte[]` array containing UTF-8/ASCII units.
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

This method allows creating a `CString` instance using an object of type
`IUtf8FunctionState<TState>`, which provides UTF-8/ASCII units using a delegate.

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