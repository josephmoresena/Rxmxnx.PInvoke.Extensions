[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=bugs)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=coverage)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![NuGet](https://img.shields.io/nuget/v/Rxmxnx.PInvoke.Extensions)](https://www.nuget.org/packages/Rxmxnx.PInvoke.Extensions/)

# Description
Provides a set of extensions and utilites which facilitates the exchange of data from and to .NET P/Invoke methods (or UnmanagedCallersOnly using NativeAOT) avoiding use of both native pointers and unsafe context.

## Binary Extensions
Provides a set of extensions for basic operations with Byte instances.
### Byte
* AsHexString()
Gets the String representation of byte value.
### Byte[]
* **AsValue&lt;T&gt;()** 
Gets a value of generic type which length must match to array lenght.
* **AsHexString()**
Gets the String representation of binary data into the array.
* **ConcatUtf8()**
Concatenates the members of a collection of UTF-8 texts.
* **ConcatUtf8Async()**
Concatenates the members of a collection of UTF-8 texts.
### IEnumerable<String>
* **ConcatUtf8()**
Concatenates the members of a collection of String.

## MemoryBlockExtensions
Provides a set of extensions for basic operations with Span&lt;T&gt; and ReadOnlySpan&lt;T&gt; instances.
### Span&lt;T&gt;
* **AsIntPtr&lt;T&gt;()**
Gets the signed pointer to referenced memory.
* **AsUIntPtr&lt;T&gt;()**
Gets the unsigned pointer to referenced memory.

### ReadOnlySpan&lt;T&gt;
* **AsIntPtr&lt;T&gt;()**
Gets the signed pointer to referenced memory.
* **AsUIntPtr&lt;T&gt;()**
Gets the unsigned pointer to referenced memory.

## PointerExtensions
Provides a set of extensions for basic operations with both signed and unsigned pointers.
### IntPtr
* **IsZero()**
Indicates whether the pointer is a null memory reference.
* **AsUIntPtr()**
Gets the memory reference as an unsigned pointer.
* **AsString(Int32 length = 0)**
Gets a String instance taking the memory reference as the UTF-16 text starting point.
* **AsReadOnlySpan&lt;T&gt;(Int32 length)**
Gets a ReadOnlySpan&lt;T&gt; instance from memory reference.
* **AsDelegate&lt;T&gt;()**
Gets a generic delegate from from memory reference.
* **AsReference&lt;T&gt;()**
Gets a managed reference to a generic unmanaged value from a memory reference.

### UIntPtr
* **IsZero()**
Indicates whether the pointer is a null memory reference.
* **AsIntPtr()**
Gets the memory reference as an signed pointer.
* **AsString(Int32 length = 0)**
Gets a String instance taking the memory reference as the UTF-16 text starting point.
* **AsReadOnlySpan&lt;T&gt;(Int32 length)**
Gets a ReadOnlySpan&lt;T&gt; instance from memory reference.
* **AsDelegate&lt;T&gt;()**
Gets a generic delegate from from memory reference.
* **AsReference&lt;T&gt;()**
Gets a managed reference to a generic unmanaged value from a memory reference.

## ReferenceExtensions
### ref T
* **AsIntPtr&lt;T&gt;()**
Gets a signed pointer to referenced memory by generic managed reference.
* **AsUIntPtr&lt;T&gt;()**
Gets a unsigned pointer to referenced memory by generic managed reference.
* **AsReferenceOf&lt;TSource, TDestination&gt;()**
Creates a memory reference to a TDestination generic type value from an exising memory reference to a TSource generic type value.

## StringExtensions
### String
* **AsUtf8Span()**
Encodes the UTF-16 text using the UTF-8 charset and retrieves the read-only span which references to the UTF-8 text.
* **AsUtf8()**
Encodes the UTF-16 text using the UTF-8 charset and retrieves the Byte array with UTF-8 text.
### IEnumerable<String>
* **ConcatUtf8()**
Concatenates the members of a collection of String.
* **ConcatUtf8Async()**
Concatenates the members of a collection of String.
### IEnumerable<CString>
* **Concat()**
Concatenates the members of a collection of CString.
* **ConcatAsync()**
Concatenates the members of a collection of CString.

## UnmanagedValueExtensions
### T
* **AsBytes&lt;T&gt;()**
Gets the binary data from unmanaged value.
* **AsValues&lt;TSource, TDestination&gt;()**
Creates an array of TDestination generic type from an array of TSource generic type. 

## CString
Represents text as a sequence of UTF-8 code units.
* **Empty**
Represents the empty UTF-8 string. This field is read-only.
* **IsNullOrEmpty(CString value)**
Indicates whether the specified CString is null or an empty UTF-8 text.
* **GetBytes(CString value)**
Retrieves the internal binary data from a given <see cref="CString"/>.
* **AsSpan(out CString[] output)**
Retreives the internal or external information as ReadOnlySpan{Byte} instance.

## CStringSequence
Represents a sequence of null-terminated UTF-8 texts.
* **Count**
Gets the number of CString contained in CStringSequence.
* **AsSpan(out CString[] output)**
Retrieves the buffer as an ReadOnlySpan<Char> instance and creates a CString array which represents 
text sequence. The output CString array will remain valid only as long as returned buffer span is on live.

## InputValue
Supports a value type that can be referenced.
* **CreateInput&lt;TValue&gt;(in TValue instance)**
Creates a new IReferenceableWrapper&lt;TValue&gt; object from a generic value.
* **CreateInput&lt;TValue?&gt;(in TValue? instance)**
Creates a new IReferenceableWrapper&lt;TValue&gt; object from a generic nullable value.
* **CreateReference&lt;TValue&gt;(in TValue instance = default)**
Creates a new IMutableReference&lt;TValue&gt; object from a generic value.
* **CreateReference&lt;TValue?&gt;(in TValue? instance = default)**
Creates a new IMutableReference&lt;TValue&gt; object from a generic nullable value.

## NativeUtilities
Provides a set of utilities for exchange data within the P/Invoke context.
* **SizeOf&lt;T&gt;()**
Retrieves the size of the generic  structure type.
* **LoadNativeLib(String libraryName, DllImportSearchPath? searchPath = default)**
Loads a native library.
* **LoadNativeLib(String libraryName, ref EventHandler unloadEvent, DllImportSearchPath? searchPath = default)**
Loads a native library and appends its unloading to given EventHandler delegate.
* **GetNativeMethod&lt;T&gt;(IntPtr handle, String name)**
Gets a generic delegate which points to a exported symbol into native library.
* **AsBytes&lt;T&gt;(in T value)**
Gets the binary data of an input generic value.

## TextUtilites
Provides a set of utilities for texts.
* **Join(CString separator, params CString[] values)** / **Join(Byte separator, params CString[] values)**
Concatenates an array of CString, using the specified separator between each member.
* **Join(CString separator, IEnumerable<CString> values)** / **Join(Byte separator, IEnumerable<CString> values)**
Concatenates the members of a collection of CString, using the specified separator between each member.

* **JoinUtf8(CString separator, params String[] values)** / **JoinUtf8(Char separator, params String[] values)**
Concatenates an array of strings, using the specified separator between each member.
* **JoinUtf8(String separator, IEnumerable<String> values)** / **JoinUtf8(Char separator, IEnumerable<String> values)**
Concatenates the members of a collection of String, using the specified separator between each member.
* **JoinUtf8(ReadOnlySpan<Byte> separator, params Byte[][] values)** / **JoinUtf8(Byte separator, params Byte[][] values)**
Concatenates an array of UTF-8 texts, using the specified separator between each member.
* **JoinUtf8(ReadOnlySpan<Byte> separator, IEnumerable<Byte[]> values)** / **JoinUtf8(Byte separator, IEnumerable<Byte> values)**
Concatenates the members of a collection of UTF-8 texts, using the specified separator between each member.

* **JoinAsync(CString separator, params CString[] values)** / **JoinAsync(Byte separator, params CString[] values)**
Concatenates an array of CString, using the specified separator between each member.
* **JoinAsync(CString separator, IEnumerable<CString> values)** / **JoinAsync(Byte separator, IEnumerable<String> values)**
Concatenates the members of a collection of CString, using the specified separator between each member.

* **JoinUtf8Async(String separator, params String[] values)** / **JoinUtf8Async(Char separator, params String[] values)**
Concatenates an array of strings, using the specified separator between each member.
* **JoinUtf8Async(String separator, IEnumerable<String> values)** / **JoinUtf8Async(Char separator, IEnumerable<String> values)**
Concatenates the members of a collection of String, using the specified separator between each member.

* **Concat(CString initial, params CString[] values)**
Concatenates all CString parameters passed to this function.

* **ConcatUtf8(String initial, params String[] values)**
Concatenates all text parameters passed to this function.
* **ConcatUtf8(Byte[] initial, params Byte[][] values)**
Concatenates all UTF-8 text parameters passed to this function.

* **ConcatAsync(CString initial, params CString[] values)**
Concatenates all CString parameters passed to this function.

* **ConcatUtf8Async(String initial, params String[] values)**
Concatenates all text parameters passed to this function.
