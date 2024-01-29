[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=bugs)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=coverage)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![NuGet](https://img.shields.io/nuget/v/Rxmxnx.PInvoke.Extensions)](https://www.nuget.org/packages/Rxmxnx.PInvoke.Extensions/)

## Description

Rxmxnx.PInvoke.Extensions is a library that provides a set of extensions and utilities that simplify the process of data
exchange to and from .NET P/Invoke methods, eliminating the necessity for native pointers, memory fixes, and operations
that require unsafe compilation.
This library also includes native handling for UTF-8 strings in both managed and unmanaged memory.

## Types

### Rxmxnx.PInvoke.IWrapper

The `Rxmxnx.PInvoke.IWrapper` interface represents a wrapper for a value. It provides methods for creating instances of
the wrapper.

#### Static

- `Create<TValue>(in TValue)`: Creates a new instance of `IWrapper<TValue>` with the specified value.
- `CreateNullable<TValue>(in TValue?)`: Creates a new instance of `IWrapper<TValue?>` with the specified nullable
  value.
- `CreateObject<TObject>(TObject)`: Creates a new instance of `IWrapper<TObject>` with the specified object.

### Rxmxnx.PInvoke.IWrapper<T>

The `Rxmxnx.PInvoke.IWrapper<T>` interface represents a generic wrapper for a value of type `T`.

- `Value`: Gets the value stored in the `IWrapper<T>`.

#### Static

- `Create(T?)`: Creates a new instance of `IWrapper<T?>` with the specified value.

### Rxmxnx.PInvoke.IReferenceableWrapper

The `Rxmxnx.PInvoke.IReferenceableWrapper` interface represents a referenceable wrapper for a value. It provides methods
for creating instances of the wrapper.

#### Static

- `Create<TValue>(in TValue)`: Creates a new instance of `IReferenceableWrapper<TValue>` with the specified value.
- `CreateNullable<TValue>(in TValue?)`: Creates a new instance of `IReferenceableWrapper<TValue?>` with the
  specified nullable value.
- `CreateObject<TObject>(TObject)`: Creates a new instance of `IReferenceableWrapper<TObject>` with the specified
  object.

### Rxmxnx.PInvoke.IReferenceableWrapper<T>

The `Rxmxnx.PInvoke.IReferenceableWrapper<T>` interface represents a referenceable wrapper for a value of type `T`.

- `Reference`: Gets a read-only reference to the value of type `T`.
- `Value`: Gets the value stored in the `IWrapper<T>`.

#### Static

- `Create(T?)`: Creates a new instance of `IReferenceableWrapper<T?>` with the specified value.

### Rxmxnx.PInvoke.IMutableWrapper

The `Rxmxnx.PInvoke.IMutableWrapper` interface represents a mutable wrapper for a value. It provides methods for
creating instances of the wrapper.

#### Static

- `Create<TValue>(in TValue)`: Creates an instance of the `IMutableWrapper<TValue>` with the specified value.
- `CreateNullable<TValue>(in TValue?)`: Creates an instance of the `IMutableWrapper<TValue?>` with the specified
  nullable value.
- `CreateObject<TObject>(TObject)`: Creates an instance of the `IMutableWrapper<TObject>` with the specified object.

### Rxmxnx.PInvoke.IMutableWrapper<T>

The `Rxmxnx.PInvoke.IMutableWrapper<T>` interface represents a mutable wrapper for a value of type `T`.

- `Value`: Gets or sets the value of type `T`.

#### Static

- `Create(T?)`: Creates a new instance of `IMutableWrapper<T?>` with the specified value.

### Rxmxnx.PInvoke.IMutableReference

The `Rxmxnx.PInvoke.IMutableReference` interface represents a mutable reference. It provides methods for creating
instances of the wrapper.

#### Static

- `Create<TValue>(in TValue)`: Creates an instance of the `IMutableReference<TValue>` with the specified value.
- `CreateNullable<TValue>(in TValue?)`: Creates an instance of the `IMutableReference<TValue?>` with the specified
  nullable value.
- `CreateObject<TObject>(TObject)`: Creates an instance of the `IMutableReference<TObject>` with the specified object.

### Rxmxnx.PInvoke.IMutableReference<T>

The `Rxmxnx.PInvoke.IMutableReference<T>` interface represents a mutable reference to a value of type `T`.

- `Reference`: Gets the reference to the value of type `T`.
- `Value`: Gets or sets the value of type `T`.

#### Static

- `Create(T?)`: Creates a new instance of `IMutableReference<T?>` with the specified value.

### Rxmxnx.PInvoke.ValueRegion<T>

`Rxmxnx.PInvoke.ValueRegion<T>` is a class representing a region of values.

- `IsMemorySlice`: Gets a value indicating whether the value region represents a memory slice.
- `Item(Int32)`: Gets the value at the specified index. This property is an indexer.
- `ToArray()`: Converts the value region to an array of values.
- `Slice(Int32, Int32)`: Creates a slice of the value region with the specified range.
- `Slice(Int32)`: Creates a slice of the value region starting from the specified index.

#### Static

- `Create(ReadOnlySpanFunc<T>)`: Creates a new `ValueRegion<T>` instance by invoking the
  specified `ReadOnlySpanFunc<T>` delegate.
- `Create(T[])`: Creates a new `ValueRegion<T>` instance from the specified array.
- `Create(IntPtr, Int32)`: Creates a new `ValueRegion<T>` instance from the memory pointed to by the
  specified pointer with the specified length.

### Rxmxnx.PInvoke.FuncPtr<TDelegate>

`Rxmxnx.PInvoke.FuncPtr<TDelegate>` is a value type representing a pointer to a function of type TDelegate. It is used for interop scenarios where a function pointer is required.

- `IsZero`: Indicates whether the current pointer is zero (null).
- `Pointer`: Gets the internal pointer as an `IntPtr`.
- `Invoke`: Gets a delegate of type `TDelegate` for the function pointer.

#### Operators

- `==`: Determines whether two `FuncPtr<TDelegate>` instances are equal.
- `!=`: Determines whether two `FuncPtr<TDelegate>` instances are not equal.

### Rxmxnx.PInvoke.ReadOnlyValPtr<T>

`Rxmxnx.PInvoke.ReadOnlyValPtr<T>` is a value type representing a pointer to a read-only value of type T. It is used for interop scenarios where a read-only T pointer is required.

- `IsZero`: Indicates whether the current pointer is zero (null).
- `Pointer`: Gets the internal pointer as an `IntPtr`.
- `Reference`: Gets a read-only reference to the pointed T value.
- `GetUnsafeFixedReference(IDisposable?)`: Creates an unsafe `IReadOnlyFixedReference<T>` instance from the current pointer. It is designed to be used when you have a guarantee that the memory being referenced is already fixed and immovable. This method does not pin the memory itself; it only provides a way to work with memory that is assumed to be already pinned.
- `GetUnsafeFixedContext(Int32, IDisposable?)`: Creates an unsafe `IReadOnlyFixedContext<T>` instance with the specified number of items in the memory block from the current pointer. It is designed to be used when you have a guarantee that the memory being referenced is already fixed and immovable. This method does not pin the memory itself; it only provides a way to work with memory that is assumed to be already pinned.

#### Operators

- `==`: Determines whether two `ReadOnlyValPtr<T>` objects are equal.
- `!=`: Determines whether two `ReadOnlyValPtr<T>` objects are not equal.
- `<`: Determines whether one `ReadOnlyValPtr<T>` object is less than another.
- `>`: Determines whether one `ReadOnlyValPtr<T>` object is greater than another.
- `<=`: Determines whether one `ReadOnlyValPtr<T>` object is less than or equal to another.
- `>=`: Determines whether one `ReadOnlyValPtr<T>` object is greater than or equal to another.

### Rxmxnx.PInvoke.ValPtr<T>

`Rxmxnx.PInvoke.ValPtr<T>` is a value type representing a pointer to a value of type T. It is used for interop scenarios where a T pointer is required.

- `IsZero`: Indicates whether the current pointer is zero (null).
- `Pointer`: Gets the internal pointer as an `IntPtr`.
- `Reference`: Gets a reference to the pointed T value.
- `GetUnsafeFixedReference(IDisposable?)`: Creates an unsafe `IFixedReference<T>` instance from the current pointer. It is designed to be used when you have a guarantee that the memory being referenced is already fixed and immovable. This method does not pin the memory itself; it only provides a way to work with memory that is assumed to be already pinned.
- `GetUnsafeFixedContext(Int32, IDisposable?)`: Creates an unsafe `IFixedContext<T>` instance with the specified number of items in the memory block from the current pointer. It is designed to be used when you have a guarantee that the memory being referenced is already fixed and immovable. This method does not pin the memory itself; it only provides a way to work with memory that is assumed to be already pinned.

#### Operators

- `==`: Determines whether two `ValPtr<T>` objects are equal.
- `!=`: Determines whether two `ValPtr<T>` objects are not equal.
- `<`: Determines whether one `ValPtr<T>` object is less than another.
- `>`: Determines whether one `ValPtr<T>` object is greater than another.
- `<=`: Determines whether one `ValPtr<T>` object is less than or equal to another.
- `>=`: Determines whether one `ValPtr<T>` object is greater than or equal to another.

### Rxmxnx.PInvoke.CString

The `Rxmxnx.PInvoke.CString` class represents a UTF-8 string.

- `Unit(Int32)`: Gets the UTF-8 unit at the specified index. This property is an indexer.
- `IsFunction`: Gets a value indicating whether the `CString` represents a function.
- `IsNullTerminated`: Gets a value indicating whether the `CString` is null-terminated.
- `IsReference`: Gets a value indicating whether the `CString` is a reference.
- `IsSegmented`: Gets a value indicating whether the `CString` is segmented.
- `Length`: Gets the length of the `CString`.
- `Clone()`: Creates a new object that is a copy of the current instance.
- `CompareTo(string)`: Compares the current instance with the specified string and returns an integer that indicates
  their relative position in the sort order.
- `CompareTo(CString)`: Compares the current instance with the specified `CString` object and returns an integer that
  indicates their relative position in the sort order.
- `CompareTo(Object)`: Compares the current instance with the specified object and returns an integer that indicates
  their relative position in the sort order.
- `Slice(Int32, Int32)`: Returns a new `CString` that represents a slice of the current string starting from the
  specified index with the specified length.
- `Slice(Int32)`: Returns a new `CString` that represents a slice of the current string starting from the specified
  index.
- `ToArray()`: Converts the `CString` to an array of `Byte` values.
- `ToHexString()`: Converts the `CString` to a hexadecimal string.
- `ToString()`: Returns a string that represents the current instance.
- `WithSafeFixed<TResult>(ReadOnlyFixedFunc<TResult>)`: Executes the specified function with a fixed list of `Byte`
  values and returns the result.
- `WithSafeFixed(ReadOnlyFixedAction)`: Executes the specified action with a fixed list of `Byte` values.
- `WithSafeFixed<TArg, TResult>(TArg, ReadOnlyFixedFunc<TArg, TResult>)`: Executes the specified function with a fixed
  list of `Byte` values and the specified argument, and returns the result.
- `WithSafeFixed<TArg>(TArg, ReadOnlyFixedAction<TArg>)`: Executes the specified action with a fixed list of `Byte`
  values and the specified argument.

#### Constructors

- `CString(Byte, Int32)`: Initializes a new instance of the `CString` class with a single character repeated for the
  specified length.
- `CString(Byte, Byte, Int32)`: Initializes a new instance of the `CString` class with two characters repeated in
  alternating positions for the specified length.
- `CString(Byte, Byte, Byte, Int32)`: Initializes a new instance of the `CString` class with three characters repeated
  in a cyclic pattern for the specified length.
- `CString(Byte, Byte, Byte, Byte, Int32)`: Initializes a new instance of the `CString` class with four characters
  repeated in a cyclic pattern for the specified length.
- `CString(ReadOnlySpan<Byte>)`: Initializes a new instance of the `CString` class with the contents of
  a `ReadOnlySpan<Byte>`.
- `CString(ReadOnlySpanFunc<Byte>)`: Initializes a new instance of the `CString` class with the specified function
  delegate to get the content. This constructor is specialized for UTF-8 literals support.

#### Operators

- `==`: Determines whether two `CString` objects are equal.
- `!=`: Determines whether two `CString` objects are not equal.
- `<`: Determines whether one `CString` object is less than another.
- `>`: Determines whether one `CString` object is greater than another.
- `<=`: Determines whether one `CString` object is less than or equal to another.
- `>=`: Determines whether one `CString` object is greater than or equal to another.
- `explicit operator CString(String)`: Converts a string to a `CString` object.
- `implicit operator CString(Byte[])`: Converts a `Byte` array to a `CString` object.
- `implicit operator ReadOnlySpan<Byte>(CString)`: Converts a `CString` object to a `ReadOnlySpan<Byte>`.

#### Static

- `Empty`: Represents an empty `CString` object.
- `Zero`: Represents a `CString` object pointing to null.
- `Create(ReadOnlySpanFunc<Byte>)`: Creates a new instance of the `CString` class by invoking the specified function to
  generate the content.
- `Create(ReadOnlySpan<Byte>)`: Creates a new instance of the `CString` class with the contents of
  a `ReadOnlySpan<Byte>`.

- `Compare(CString, CString)`: Compares two `CString` objects and returns an integer that indicates their relative
  position in the sort order.
- `Compare(CString, CString, Boolean)`: Compares two `CString` objects and returns an integer that indicates their
  relative position in the sort order, optionally ignoring case.
- `Compare(CString, CString, Boolean, CultureInfo)`: Compares two `CString` objects using a specified `CultureInfo` and
  returns an integer that indicates their relative position in the sort order, optionally ignoring case.
- `Compare(CString, String, StringComparison)`: Compares a `CString` object with a string using a
  specified `StringComparison` and returns an integer that indicates their relative position in the sort order.
- `Compare(CString, String, Boolean, CultureInfo)`: Compares a `CString` object with a string using a
  specified `CultureInfo` and returns an integer that indicates their relative position in the sort order, optionally
  ignoring case.
- `Compare(CString, String, Boolean)`: Compares a `CString` object with a string and returns an integer that indicates
  their relative position in the sort order, optionally ignoring case.
- `Compare(CString, String)`: Compares a `CString` object with a string and returns an integer that indicates their
  relative position in the sort order.
- `Compare(CString, CString, StringComparison)`: Compares two `CString` objects using a specified `StringComparison` and
  returns an integer that indicates their relative position in the sort order.

- `Concat(String[])`: Concatenates an array of strings into a single `CString` object.
- `Concat(Byte[][])`: Concatenates an array of byte arrays into a single `CString` object.

- `Join(Char, String[], Int32, Int32)`: Concatenates an array of strings, using the specified separator character,
  starting from the specified index and including the specified number of elements, into a single `CString` object.
- `Join(Byte, CString[])`: Concatenates an array of `CString` objects, using the specified byte separator, into a
  single `CString` object.
- `Join(Byte, IEnumerable<CString>)`: Concatenates a sequence of `CString` objects, using the specified byte separator,
  into a single `CString` object.
- `Join(Byte, CString[], Int32, Int32)`: Concatenates a specified number of `CString` objects, using the specified byte
  separator, starting from the specified index, into a single `CString` object.
- `Join(Char, String[])`: Concatenates an array of strings, using the specified separator character, into a
  single `CString` object.
- `Join(ReadOnlySpan<Byte>, IEnumerable<CString>)`: Concatenates a sequence of `CString` objects, using the specified
  byte separator, into a single `CString` object.
- `Join(ReadOnlySpan<Byte>, CString[], Int32, Int32)`: Concatenates a specified number of `CString` objects, using the
  specified byte separator, starting from the specified index, into a single `CString` object.
- `Join(CString, CString[])`: Concatenates an array of `CString` objects, using the specified `CString` separator, into
  a single `CString` object.
- `Join(CString, IEnumerable<CString>)`: Concatenates a sequence of `CString` objects, using the specified `CString`
  separator, into a single `CString` object.
- `Join(CString, CString[], Int32, Int32)`: Concatenates a specified number of `CString` objects, using the
  specified `CString` separator, starting from the specified index, into a single `CString` object.
- `Join(String, String[])`: Concatenates an array of strings, using the specified separator string, into a
  single `CString` object.
- `Join(String, IEnumerable<String>)`: Concatenates a sequence of strings, using the specified separator string, into a
  single `CString` object.

- `ConcatAsync(CString[])`: Asynchronously concatenates an array of `CString` objects into a single `CString` object.
- `ConcatAsync(CancellationToken, CString[])`: Asynchronously concatenates an array of `CString` objects into a
  single `CString` object, while observing a cancellation token.
- `ConcatAsync(Byte[][])`: Asynchronously concatenates an array of byte arrays into a single `CString` object.
- `ConcatAsync(CancellationToken, Byte[][])`: Asynchronously concatenates an array of byte arrays into a
  single `CString` object, while observing a cancellation token.
- `ConcatAsync(String[])`: Concatenates multiple strings asynchronously and returns a `Task<CString>` representing the
  result.
- `ConcatAsync(CancellationToken, String[])`: Asynchronously concatenates an array of strings into a single `CString`
  object, while observing a cancellation token.

- `JoinAsync(CString, CancellationToken, CString[])`: Asynchronously concatenates an array of `CString` objects, using
  the specified `CString` separator, into a single `CString` object, while observing a cancellation token.
- `JoinAsync(CString, IEnumerable<CString>, CancellationToken)`: Asynchronously concatenates a sequence of `CString`
  objects, using the specified `CString` separator, into a single `CString` object, while observing a cancellation
  token.
- `JoinAsync(CString, CString[], Int32, Int32, CancellationToken)`: Asynchronously concatenates a specified number
  of `CString` objects, using the specified `CString` separator, starting from the specified index, into a
  single `CString` object, while observing a cancellation token.
- `JoinAsync(Byte, CString[])`: Asynchronously concatenates an array of `CString` objects, using the specified byte
  separator, into a single `CString` object.
- `JoinAsync(Byte, CancellationToken, CString[])`: Asynchronously concatenates an array of `CString` objects, using the
  specified byte separator, into a single `CString` object, while observing a cancellation token.
- `JoinAsync(Byte, IEnumerable<CString>, CancellationToken)`: Asynchronously concatenates a sequence of `CString`
  objects, using the specified byte separator, into a single `CString` object, while observing a cancellation token.
- `JoinAsync(Byte, CString[], Int32, Int32, CancellationToken)`: Asynchronously concatenates a specified number
  of `CString` objects, using the specified byte separator, starting from the specified index, into a single `CString`
  object, while observing a cancellation token.
- `JoinAsync(String, String[], Int32, Int32, CancellationToken)`: Asynchronously concatenates a specified number of
  strings, using the specified separator string, starting from the specified index, into a single `CString` object,
  while observing a cancellation token.
- `JoinAsync(String, IEnumerable<String>, CancellationToken)`: Asynchronously concatenates a sequence of strings, using
  the specified separator string, into a single `CString` object, while observing a cancellation token.
- `JoinAsync(String, CancellationToken, String[])`: Asynchronously concatenates an array of strings, using the specified
  separator string, into a single `CString` object, while observing a cancellation token.
- `JoinAsync(Char, String[], Int32, Int32, CancellationToken)`: Asynchronously concatenates a specified number of
  strings, using the specified separator character, starting from the specified index, into a single `CString` object,
  while observing a cancellation token.
- `JoinAsync(Char, IEnumerable<String>, CancellationToken)`: Asynchronously concatenates a sequence of strings, using
  the specified separator character, into a single `CString` object, while observing a cancellation token.
- `JoinAsync(Char, CancellationToken, String[])`: Asynchronously concatenates an array of strings, using the specified
  separator character, into a single `CString` object, while observing a cancellation token.
- `JoinAsync(CString, String[], Int32, Int32, CancellationToken)`: Asynchronously concatenates a specified number
  of `CString` objects, using the specified `CString` separator, starting from the specified index, into a
  single `CString` object, while observing a cancellation token.
- `JoinAsync(CString, IEnumerable<CString>, CancellationToken)`: Asynchronously concatenates a sequence of `CString`
  objects, using the specified `CString` separator, into a single `CString` object, while observing a cancellation
  token.
- `JoinAsync(CString, CancellationToken, CString[])`: Asynchronously concatenates an array of `CString` objects, using
  the specified `CString` separator, into a single `CString` object, while observing a cancellation token.

### Rxmxnx.PInvoke.CStringSequence

The `Rxmxnx.PInvoke.CStringSequence` class represents a sequence of `CString` objects.

- `Count`: Gets the number of elements in the `CStringSequence`.
- `Item(Int32)`: Gets the `CString` at the specified index in the `CStringSequence`. This property is an indexer.
- `Clone()`: Creates a new object that is a copy of the current instance.
- `Equals(CStringSequence)`: Determines whether the specified `CStringSequence` is equal to the current instance.
- `Equals(Object)`: Determines whether the specified object is equal to the current instance.
- `Slice(Int32)`: Returns a new `CStringSequence` that represents a slice of the current sequence starting from the
  specified index.
- `Slice(Int32, Int32)`: Returns a new `CStringSequence` that represents a slice of the current sequence starting from
  the specified index with the specified length.
- `ToCString()`: Converts the `CStringSequence` to a single `CString` object.
- `GetFixedPointer()`: This method creates a disposable, read-only fixed pointer instance by pinning current instance. It ensures that the memory block referenced by this sequence is fixed (immovable) in memory, allowing for safe direct access.
- `WithSafeFixed<TState, TResult>(TState, ReadOnlyFixedListFunc<TState, TResult>)`: Executes the specified function with
  a fixed list of `CString` objects and returns the result.
- `WithSafeFixed(ReadOnlyFixedListAction)`: Executes the specified action with a fixed list of `CString` objects.
- `WithSafeFixed<TState>(TState, ReadOnlyFixedListAction<TState>)`: Executes the specified action with a fixed list
  of `CString` objects.
- `WithSafeFixed<TResult>(ReadOnlyFixedListFunc<TResult>)`: Executes the specified function with a fixed list
  of `CString` objects and returns the result.
- `WithSafeTransform(CStringSequenceAction)`: Executes the specified action with the `CStringSequence` object.
- `WithSafeTransform<TState>(TState, CStringSequenceAction<TState>)`: Executes the specified action with
  the `CStringSequence` object and additional state.
- `WithSafeTransform<TResult>(CStringSequenceFunc<TResult>)`: Executes the specified function with the `CStringSequence`
  object and returns the result.
- `WithSafeTransform<TState, TResult>(TState, CStringSequenceFunc<TState, TResult>)`: Executes the specified function
  with the `CStringSequence` object and additional state.

#### Constructors

- `CStringSequence(CString[])`: Initializes a new instance of the `CStringSequence` class with an array of `CString`
  objects.
- `CStringSequence(IEnumerable<String>)`: Initializes a new instance of the `CStringSequence` class with an enumerable
  collection of strings.
- `CStringSequence(IEnumerable<CString>)`: Initializes a new instance of the `CStringSequence` class with an enumerable
  collection of `CString` objects.
- `CStringSequence(String[])`: Initializes a new instance of the `CStringSequence` class with an array of strings.
- `CStringSequence(ReadOnlySpan<Byte>...)`: Initializes a new instance of the `CStringSequence` class with UTF-8
  read-only spans.

#### Static

- `Create<TState>(TState, CStringSequenceCreationAction<TState>, Nullable<Int32>[]?)`: Creates an instance
  of `CStringSequence` using the specified creation action and optional array of nullable integers.

## Extensions

### Rxmxnx.PInvoke.BinaryExtensions

`Rxmxnx.PInvoke.BinaryExtensions` is an extension methods class that provides a set of useful methods when working with
bytes and byte arrays in a PInvoke context.

- `AsHexString(Byte[])`: Returns a hexadecimal string representation of a byte array.
- `AsHexString(Byte)`: Returns a hexadecimal string representation of a single byte.
- `AsValue<T>(ReadOnlySpan<Byte>)`: Transforms the byte span into a value of the provided value type.
- `ToValue<T>(ReadOnlySpan<Byte>)`: Creates a new value of the provided type from a byte span.
- `WithSafeFixed(Span<Byte>, FixedAction)`: Performs a fixed action on a span of bytes.
- `WithSafeFixed(ReadOnlySpan<Byte>, ReadOnlyFixedAction)`: Performs a fixed action on a readonly span of bytes.
- `WithSafeFixed<TArg>(Span<Byte>, TArg, FixedAction<TArg>)`: Performs a fixed action on a span of bytes with an
  additional argument.
- `WithSafeFixed<TArg>(ReadOnlySpan<Byte>, TArg, ReadOnlyFixedAction<TArg>)`: Performs a fixed action on a readonly span
  of bytes with an additional argument.
- `WithSafeFixed<TResult>(Span<Byte>, FixedFunc<TResult>)`: Performs a fixed function on a span of bytes and returns a
  value.
- `WithSafeFixed<TResult>(ReadOnlySpan<Byte>, ReadOnlyFixedFunc<TResult>)`: Performs a fixed function on a readonly span
  of bytes and returns a value.
- `WithSafeFixed<TArg, TResult>(Span<Byte>, TArg, FixedFunc<TArg, TResult>)`: Performs a fixed function on a span of
  bytes with an additional argument and returns a value.
- `WithSafeFixed<TArg, TResult>(ReadOnlySpan<Byte>, TArg, ReadOnlyFixedFunc<TArg, TResult>)`: Performs a fixed function
  on a readonly span of bytes with an additional argument and returns a value.

### Rxmxnx.PInvoke.DelegateExtensions

`Rxmxnx.PInvoke.DelegateExtensions` is an extension methods class that provides additional functionality for working
with delegates.

- `GetUnsafeFuncPtr<TDelegate>(TDelegate)`: Returns the unmanaged FuncPtr<TDelegate> representation of the specified delegate.
- `GetUnsafeIntPtr<TDelegate>(TDelegate)`: Returns the unmanaged IntPtr representation of the specified delegate.
- `GetUnsafeUIntPtr<TDelegate>(TDelegate)`: Returns the unmanaged UIntPtr representation of the specified delegate.
- `WithSafeFixed<TDelegate, TArg>(TDelegate, TArg, FixedMethodAction<TDelegate, TArg>)`: Performs a fixed method action
  on the specified delegate and an additional argument.
- `WithSafeFixed<TDelegate, TResult>(TDelegate, FixedMethodFunc<TDelegate, TResult>)`: Performs a fixed method function
  on the specified delegate and returns a value.
- `WithSafeFixed<TDelegate, TArg, TResult>(TDelegate, TArg, FixedMethodFunc<TDelegate, TArg, TResult>)`: Performs a
  fixed method function on the specified delegate with an additional argument and returns a value.
- `WithSafeFixed<TDelegate>(TDelegate, FixedMethodAction<TDelegate>)`: Performs a fixed method action on the specified
  delegate.

### Rxmxnx.PInvoke.MemoryBlockExtensions

`Rxmxnx.PInvoke.MemoryBlockExtensions` is an extension methods class that provides additional functionality for working
with memory blocks.

- `AsBytes<TSource>(Span<TSource>)`: Returns a span of bytes representing the memory block.
- `AsBytes<TSource>(ReadOnlySpan<TSource>)`: Returns a readonly span of bytes representing the memory block.
- `AsValues<TSource, TDestination>(ReadOnlySpan<TSource>)`: Converts the memory block to a readonly span of values of
  the specified destination type.
- `AsValues<TSource, TDestination>(Span<TSource>, Span<byte>&)`: Converts the memory block to a span of values of the
  specified destination type, also returning the underlying byte span.
- `AsValues<TSource, TDestination>(Span<TSource>, ReadOnlySpan<byte>&)`: Converts the memory block to a readonly span of
  values of the specified destination type, also returning the underlying byte span.
- `AsValues<TSource, TDestination>(ReadOnlySpan<TSource>, ReadOnlySpan<byte>&)`: Converts the memory block to a readonly
  span of values of the specified destination type, also returning the underlying byte span.
- `GetUnsafeValPtr<T>(ReadOnlySpan<T>)`: Returns the unmanaged ReadOnlyValPtr<T> representation of the specified memory block.
- `GetUnsafeValPtr<T>(Span<T>)`: Returns the unmanaged ValPtr<T> representation of the specified memory block.
- `GetUnsafeIntPtr<T>(ReadOnlySpan<T>)`: Returns the unmanaged IntPtr representation of the specified memory block.
- `GetUnsafeIntPtr<T>(Span<T>)`: Returns the unmanaged IntPtr representation of the specified memory block.
- `GetUnsafeUIntPtr<T>(Span<T>)`: Returns the unmanaged UIntPtr representation of the specified memory block.
- `GetUnsafeUIntPtr<T>(ReadOnlySpan<T>)`: Returns the unmanaged UIntPtr representation of the specified memory block.
- `GetFixedContext<T>(ReadOnlyMemory<T>)`: This method creates a disposable, read-only fixed context instance by pinning the provided `ReadOnlyMemory<T>` instance. It ensures that the memory block referenced by `ReadOnlyMemory<T>` is fixed (immovable) in memory, allowing for safe direct access.
- `GetFixedContext<T>(Memory<T>)`: This method creates a disposable, fixed context instance by pinning the provided `Memory<T>` instance. It ensures that the memory block referenced by `Memory<T>` is fixed (immovable) in memory, allowing for safe direct access.
- `WithSafeFixed<T>(Span<T>, FixedContextAction<T>)`: Performs a fixed context action on the specified memory block.
- `WithSafeFixed<T>(Span<T>, ReadOnlyFixedContextAction<T>)`: Performs a readonly fixed context action on the specified
  memory block.
- `WithSafeFixed<T>(ReadOnlySpan<T>, ReadOnlyFixedContextAction<T>)`: Performs a readonly fixed context action on the
  specified memory block.
- `WithSafeFixed<T, TArg>(Span<T>, TArg, FixedContextAction<T, TArg>)`: Performs a fixed context action on the specified
  memory block with an additional argument.
- `WithSafeFixed<T, TArg>(Span<T>, TArg, ReadOnlyFixedContextAction<T, TArg>)`: Performs a readonly fixed context action
  on the specified memory block with an additional argument.
- `WithSafeFixed<T, TArg>(ReadOnlySpan<T>, TArg, ReadOnlyFixedContextAction<T, TArg>)`: Performs a readonly fixed
  context action on the specified memory block with an additional argument.
- `WithSafeFixed<T, TResult>(Span<T>, FixedContextFunc<T, TResult>)`: Performs a fixed context function on the specified
  memory block and returns a value.
- `WithSafeFixed<T, TResult>(Span<T>, ReadOnlyFixedContextFunc<T, TResult>)`: Performs a readonly fixed context function
  on the specified memory block and returns a value.
- `WithSafeFixed<T, TResult>(ReadOnlySpan<T>, ReadOnlyFixedContextFunc<T, TResult>)`: Performs a readonly fixed context
  function on the specified memory block and returns a value.
- `WithSafeFixed<T, TArg, TResult>(Span<T>, TArg, FixedContextFunc<T, TArg, TResult>)`: Performs a fixed context
  function on the specified memory block with an additional argument and returns a value.
- `WithSafeFixed<T, TArg, TResult>(Span<T>, TArg, ReadOnlyFixedContextFunc<T, TArg, TResult>)`: Performs a readonly
  fixed context function on the specified memory block with an additional argument and returns a value.
- `WithSafeFixed<T, TArg, TResult>(ReadOnlySpan<T>, TArg, ReadOnlyFixedContextFunc<T, TArg, TResult>)`: Performs a
  readonly fixed context function on the specified memory block with an additional argument and returns a value.

### Rxmxnx.PInvoke.PointerExtensions

`Rxmxnx.PInvoke.PointerExtensions` is an extension methods class that provides additional functionality for working with
pointers.

- `GetUnsafeArray<T>(IntPtr, Int32)`: Returns an array of type `T` from the specified memory pointer and length.
- `GetUnsafeArray<T>(UIntPtr, Int32)`: Returns an array of type `T` from the specified memory pointer and length.
- `GetUnsafeDelegate<T>(IntPtr)`: Returns a delegate of type `T` from the specified function pointer.
- `GetUnsafeDelegate<T>(UIntPtr)`: Returns a delegate of type `T` from the specified function pointer.
- `GetUnsafeReadOnlyReference<T>(UInt32)`: Returns a readonly reference of type `T` from the specified memory pointer.
- `GetUnsafeReadOnlyReference<T>(Int32)`: Returns a readonly reference of type `T` from the specified memory pointer.
- `GetUnsafeReadOnlySpan<T>(UIntPtr, Int32)`: Returns a readonly span of type `T` from the specified memory pointer and
  length.
- `GetUnsafeReadOnlySpan<T>(IntPtr, Int32)`: Returns a readonly span of type `T` from the specified memory pointer and
- `GetUnsafeReadOnlySpan<T>(MemoryHandle, Int32)`: Returns a readonly span of type `T` from the specified memory handle and length.
- `GetUnsafeReference<T>(UInt32)`: Returns a reference of type `T` from the specified memory pointer.
- `GetUnsafeReference<T>(Int32)`: Returns a reference of type `T` from the specified memory pointer.
- `GetUnsafeSpan<T>(UIntPtr, Int32)`: Returns a span of type `T` from the specified memory pointer and length.
- `GetUnsafeSpan<T>(IntPtr, Int32)`: Returns a span of type `T` from the specified memory pointer and length.
- `GetUnsafeSpan<T>(MemoryHandle, Int32)`: Returns a span of type `T` from the specified memory handle and length.
- `GetUnsafeString(Int32, Int32)`: Returns a string from the specified memory pointer and length.
- `GetUnsafeString(UInt32)`: Returns a string from the specified memory pointer.
- `GetUnsafeString(Int32)`: Returns a string from the specified memory pointer.
- `GetUnsafeString(UInt32, Int32)`: Returns a string from the specified memory pointer and length.
- `GetUnsafeValue<T>(IntPtr)`: Returns a nullable value of type `T` from the specified memory pointer.
- `GetUnsafeValue<T>(UIntPtr)`: Returns a nullable value of type `T` from the specified memory pointer.
- `IsZero(UInt32)`: Checks if the specified memory pointer is zero.
- `IsZero(Int32)`: Checks if the specified memory pointer is zero.
- `ToIntPtr(UInt32)`: Converts the specified unsigned integer memory pointer to a signed integer memory pointer.
- `ToUIntPtr(Int32)`: Converts the specified signed integer memory pointer to an unsigned integer memory pointer.

### Rxmxnx.PInvoke.PointerCStringExtensions

`Rxmxnx.PInvoke.PointerCStringExtensions` is an extension methods class that provides additional functionality for
working with UTF-8 encoded strings represented by pointers.

- `GetUnsafeCString(UInt32, Int32)`: Retrieves a UTF-8 encoded string from the specified pointer and length.
- `GetUnsafeCString(Int32, Int32)`: Retrieves a UTF-8 encoded string from the specified pointer and length.

### Rxmxnx.PInvoke.ReferenceExtensions

`Rxmxnx.PInvoke.ReferenceExtensions` is an extension methods class that provides additional functionality for working
with references.

- `AsBytes<TSource>(ref TSource)`: Returns a span of bytes from the specified reference.
- `GetUnsafeValPtr<T>(ref T)`: Returns the unsafe ValPtr<T> representation of the specified reference.
- `GetUnsafeIntPtr<T>(ref T)`: Returns the unsafe IntPtr representation of the specified reference.
- `GetUnsafeUIntPtr<T>(ref T)`: Returns the unsafe UIntPtr representation of the specified reference.
- `Transform<TSource, TDestination>(ref TSource)`: Transforms the specified reference to a reference of
  type `TDestination`.
- `WithSafeFixed<T>(ref T, FixedReferenceAction<T>)`: Executes a fixed reference action on the specified reference.
- `WithSafeFixed<T>(ref T, ReadOnlyFixedReferenceAction<T>)`: Executes a readonly fixed reference action on the
  specified reference.
- `WithSafeFixed<T, TArg>(ref T, TArg, FixedReferenceAction<T, TArg>)`: Executes a fixed reference action with an
  additional argument on the specified reference.
- `WithSafeFixed<T, TArg>(ref T, TArg, ReadOnlyFixedReferenceAction<T, TArg>)`: Executes a readonly fixed reference
  action with an additional argument on the specified reference.
- `WithSafeFixed<T, TResult>(ref T, FixedReferenceFunc<T, TResult>)`: Executes a fixed reference function on the
  specified reference and returns a value.
- `WithSafeFixed<T, TResult>(ref T, ReadOnlyFixedReferenceFunc<T, TResult>)`: Executes a readonly fixed reference
  function on the specified reference and returns a value.
- `WithSafeFixed<T, TArg, TResult>(ref T, TArg, FixedReferenceFunc<T, TArg, TResult>)`: Executes a fixed reference
  function with an additional argument on the specified reference and returns a value.
- `WithSafeFixed<T, TArg, TResult>(ref T, TArg, ReadOnlyFixedReferenceFunc<T, TArg, TResult>)`: Executes a readonly
  fixed reference function with an additional argument on the specified reference and returns a value.

### Rxmxnx.PInvoke.UnmanagedValueExtensions

`Rxmxnx.PInvoke.UnmanagedValueExtensions` is an extension methods class that provides additional functionality for
working with unmanaged values.

- `ToBytes<TSource>(TSource[])`: Converts an array of values to a byte array representation.
- `ToBytes<T>(T)`: Converts a single value to a byte array representation.
- `ToValues<TSource, TDestination>(TSource[], Byte[])`: Converts an array of source values to an array of destination
  values using a byte array as an intermediary.
- `ToValues<TSource, TDestination>(TSource[])`: Converts an array of source values to an array of destination values.
- `WithSafeFixed<T>(T[], FixedContextAction<T>)`: Performs a fixed context action on the specified memory block.
- `WithSafeFixed<T>(T[], ReadOnlyFixedContextAction<T>)`: Performs a readonly fixed context action on the specified
  memory block.
- `WithSafeFixed<T, TArg>(T[], TArg, FixedContextAction<T, TArg>)`: Performs a fixed context action on the specified
  memory block with an additional argument.
- `WithSafeFixed<T, TArg>(T[], TArg, ReadOnlyFixedContextAction<T, TArg>)`: Performs a readonly fixed context action on
  the specified memory block with an additional argument.
- `WithSafeFixed<T, TResult>(T[], FixedContextFunc<T, TResult>)`: Performs a fixed context function on the specified
  memory block and returns a value.
- `WithSafeFixed<T, TResult>(T[], ReadOnlyFixedContextFunc<T, TResult>)`: Performs a readonly fixed context function on
  the specified memory block and returns a value.
- `WithSafeFixed<T, TArg, TResult>(T[], TArg, FixedContextFunc<T, TArg, TResult>)`: Performs a fixed context function on
  the specified memory block with an additional argument and returns a value.
- `WithSafeFixed<T, TArg, TResult>(T[], TArg, ReadOnlyFixedContextFunc<T, TArg, TResult>)`: Performs a readonly fixed
  context function on the specified memory block with an additional argument and returns a value.

### Rxmxnx.PInvoke.StreamCStringExtensions

`Rxmxnx.PInvoke.StreamCStringExtensions` is an extension methods class that provides additional functionality for
working with UTF-8 encoded strings in streams.

- `Write(Stream, CString, Int32, Int32)`: Writes a portion of the UTF-8 encoded string to the specified stream.
- `Write(Stream, CString, Boolean)`: Writes the UTF-8 encoded string to the specified stream, optionally including the
  null terminator.
- `WriteAsync(Stream, CString, Boolean, CancellationToken)`: Asynchronously writes the UTF-8 encoded string to the
  specified stream, optionally including the null terminator, and allows cancellation.
- `WriteAsync(Stream, CString, Int32, Int32, CancellationToken)`: Asynchronously writes a portion of the UTF-8 encoded
  string to the specified stream and allows cancellation.
- `WriteAsync(Stream, CString, CancellationToken)`: Asynchronously writes the UTF-8 encoded string to the specified
  stream and allows cancellation.

### Rxmxnx.PInvoke.StringExtensions

`Rxmxnx.PInvoke.MemoryBlockExtensions` is an extension methods class that provides additional functionality for working
with strings.

- `WithSafeFixed<T>(String, ReadOnlyFixedContextAction<T>)`: Performs a readonly fixed context action on the specified
  memory block.
- `WithSafeFixed<T, TArg>(String, TArg, ReadOnlyFixedContextAction<T, TArg>)`: Performs a readonly fixed context action
  on the specified memory block with an additional argument.
- `WithSafeFixed<T, TResult>(String, ReadOnlyFixedContextFunc<T, TResult>)`: Performs a readonly fixed context function
  on the specified memory block and returns a value.
- `WithSafeFixed<T, TArg, TResult>(String, TArg, ReadOnlyFixedContextFunc<T, TArg, TResult>)`: Performs a readonly fixed
  context function on the specified memory block with an additional argument and returns a value.

## Utilities

### Rxmxnx.PInvoke.NativeUtilities

`Rxmxnx.PInvoke.NativeUtilities` is a utility class that provides various methods for working with native code.

- `PointerSize`: Size in bytes of a memory pointer.

- `AsBytes<T>(in T)`: Retrieves the bytes representing the specified value.
- `AsBinarySpan<T>(ref T)`: Converts the specified reference to a span of bytes.
- `CopyBytes<T>(in T, Span<Byte>, Int32)`: Copies the bytes from the specified span to the specified value.
- `CreateArray<T, TState>(Int32, TState, SpanAction<T, TState>)`: Creates an array of the specified type and size,
  applying the specified action for each element.
- `GetNativeMethod<TDelegate>(IntPtr, String)`: Retrieves a native method as a delegate of the specified type.
- `GetUnsafeValPtr<T>(in T)`: Retrieves the read-only value pointer from the specified reference.
- `GetUnsafeIntPtr<T>(in T)`: Retrieves the pointer value from the specified reference.
- `GetUnsafeUIntPtr<T>(in T)`: Retrieves the unsigned pointer value from the specified reference.
- `GetUnsafeFuncPtr<TDelegate>(TDelegate)`: Returns the unmanaged FuncPtr<TDelegate> representation of the specified delegate. 
- `LoadNativeLib(String, DllImportSearchPath?)`: Loads a native library and returns the handle.
- `LoadNativeLib(String, ref EventHandler?, DllImportSearchPath?)`: Loads a native library and returns the handle,
  raising an event upon load or failure.
- `SizeOf<T>()`: Retrieves the size, in bytes, of the specified value type.
- `ToBytes<T>(in T)`: Retrieves the bytes representing the specified value.
- `Transform<TSource, TDestination>(in TSource)`: Converts the specified value from the source type to the destination
  type.
- `TransformReference<TSource, TDestination>(ref TSource)`: Converts the specified reference from the source type to the
  destination type.

- `WithSafeFixed<T0..T7>(Span<T0>...Span<T7>, FixedListAction)`: Executes the specified action with the specified fixed
  spans.
- `WithSafeFixed<T0..T7, TArg>(Span<T0>...Span<T7>, FixedListAction)`: Executes the specified action with the specified
  fixed spans and an additional argument.

- `WithSafeFixed<T0..T7>(ReadOnlySpan<T0>...ReadOnlySpan<T7>, ReadOnlyFixedListAction)`: Executes the specified action
  with the specified fixed spans.
- `WithSafeFixed<T0..T7, TArg>(ReadOnlySpan<T0>...ReadOnlySpan<T7>, ReadOnlyFixedListAction)`: Executes the specified
  action with the specified fixed spans and an additional argument.

- `WithSafeReadOnlyFixed<T0..T7>(Span<T0>...Span<T7>, ReadOnlyFixedListAction)`: Executes the specified action with the
  specified fixed spans.
- `WithSafeReadOnlyFixed<T0..T7, TArg>(Span<T0>...Span<T7>, ReadOnlyFixedListAction)`: Executes the specified action
  with the specified fixed spans and an additional argument.

- `WithSafeFixed<T0..T7>(Span<T0>...Span<T7>, FixedListFunc)`: Executes the specified function with the specified fixed
  spans.
- `WithSafeFixed<T0..T7, TArg>(Span<T0>...Span<T7>, FixedListFunc)`: Executes the specified function with the specified
  fixed spans and an additional argument.

- `WithSafeFixed<T0..T7>(ReadOnlySpan<T0>...ReadOnlySpan<T7>, ReadOnlyFixedListFunc)`: Executes the specified function
  with the specified fixed spans.
- `WithSafeFixed<T0..T7, TArg>(ReadOnlySpan<T0>...ReadOnlySpan<T7>, ReadOnlyFixedListFunc)`: Executes the specified
  function with the specified fixed spans and an additional argument.

- `WithSafeReadOnlyFixed<T0..T7>(Span<T0>...Span<T7>, ReadOnlyFixedListFunc)`: Executes the specified function with the
  specified fixed spans.
- `WithSafeReadOnlyFixed<T0..T7, TArg>(Span<T0>...Span<T7>, ReadOnlyFixedListFunc)`: Executes the specified function
  with the specified fixed spans and an additional argument.