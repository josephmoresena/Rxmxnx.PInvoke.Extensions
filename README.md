[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=bugs)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=coverage)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![NuGet](https://img.shields.io/nuget/v/Rxmxnx.PInvoke.Extensions)](https://www.nuget.org/packages/Rxmxnx.PInvoke.Extensions/)
[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/josephmoresena/Rxmxnx.PInvoke.Extensions)

---

# Table of Contents

- [Description](#description)
    - [Features](#features)
- [Getting Started](#getting-started)
    - [Installation](#installation)
    - [Framework Support](#framework-support)
    - [AOT Support](#aot-support)
    - [Visual Basic .NET Support](#visual-basic-net-support)
- [Abstractions](#abstractions)
    - [Reference Interfaces](#reference-interfaces)
    - [Wrapper Interfaces](#wrapper-interfaces)
    - [Fixed memory Interfaces](#fixed-memory-interfaces)
    - [Functional Interfaces](#functional-interfaces)
    - [Span Delegates](#span-delegates)
    - [Fixed memory Delegates](#fixed-memory-delegates)
    - [Fixed memory context Delegates](#fixed-memory-context-delegates)
    - [Fixed memory reference Delegates](#fixed-memory-reference-delegates)
    - [Fixed address method Delegates](#fixed-address-method-delegates)
    - [Fixed memory list Delegates](#fixed-memory-list-delegates)
    - [UTF-8/ASCII Delegates](#utf-8ascii-delegates)
    - [Buffers Delegates](#buffers-delegates)
- [Enums](#enums)
    - [Iso639P1](#iso639p1)
- [Structs](#structs)
    - [Pointers](#pointers)
    - [Fixed memory lists](#fixed-memory-lists)
    - [Managed buffer types](#managed-buffer-types)
- [Classes](#classes)
- [Extensions](#extensions)
- [Utilities](#utilities)
- [License](#license)
    - [Key Highlights](#key-highlights)
    - [Disclaimer](#disclaimer)
- [Contributing](#contributing)
    - [Reporting Issues](#reporting-issues)
    - [Proposing Improvements](#proposing-improvements)
    - [Contributing Code](#contributing-code)
    - [Translations](#translations)
    - [Collaboration Guidelines](#collaboration-guidelines)

---

# Description

`Rxmxnx.PInvoke.Extensions` is a comprehensive library designed to streamline and enhance the
interaction between .NET and native P/Invoke methods.

## Features

- **UTF-8/ASCII String Handling**: Seamlessly work with UTF-8 encoded strings in interop
  contexts. [More info](src/Intermediate/Rxmxnx.PInvoke.CString.Intermediate/README.md)
- **Managed Buffers**: Dynamically allocate object references on the stack with minimal
  effort.  [More info](src/Intermediate/Rxmxnx.PInvoke.Buffers.Intermediate/README.md)
- **Safe Memory Manipulation**: Eliminate direct pointer manipulation and unsafe code
  requirements. [More info](src/Intermediate/Rxmxnx.PInvoke.Extensions.Intermediate/README.md)

---

# Getting Started

## Installation

Install the library via NuGet:

```cmd
dotnet add package Rxmxnx.PInvoke.Extensions
```

**Note:** This package officially supports .NET 8.0 and later. However, this package offers limited support for
.NET Standard 2.1-compatible runtimes and legacy support for .NET 7.0 and earlier.

## Framework Support

This package guarantees both **binary and source compatibility across all supported target frameworks**, from **.NET
Standard 2.1** up to **.NET 9.0**.

Here is a detailed description of the specific characteristics of how this package is compiled for each of the supported
target frameworks, along with the type of support provided for each one.

<details>
<summary><strong>.NET Standard 2.1</strong> — Limited Support</summary>

- **Static Virtual Members:** No [¹]
- **Generic `ref struct`:** No
- **MemoryMarshal (shim implemented):**
    - `CreateReadOnlySpanFromNullTerminated` [²]
    - `GetArrayDataReference` [¹]
- **Rune (shim implemented):**
    - `EncodeToUtf8`, `DecodeFromUtf8`, `DecodeFromUtf16` [²]
- **Enum (shim implemented):**
    - `Enum.GetName<T>` [³]
- **Convert (shim implemented):**
    - `ToHexString` [²]
- **Dependencies:**
    - `System.Runtime.CompilerServices.Unsafe` 5.0
    - `System.Collections.Immutable` 5.0

</details>
<details>
<summary><strong>.NET Core 3.0</strong> — Legacy (Limited)</summary>

- Inherits from .NET Standard 2.1
- Adds support for:
    - **System.Text.Json**
    - **NativeLibrary**
- **Rune (native implemented):** Yes
- **Dependencies:**
    - Same as .NET Standard 2.1
    - `System.Text.Json` 5.0.2

</details>
<details>
<summary><strong>.NET Core 3.1</strong> — Legacy</summary>

- Inherits from .NET Core 3.0
- **Updated Dependencies:**
    - `System.Runtime.CompilerServices.Unsafe` 6.0
    - `System.Collections.Immutable` 6.0
    - `System.Text.Json` 6.0.11

</details>
<details>
<summary><strong>.NET 5.0</strong> — Legacy</summary>

- Inherits from .NET Core 3.1
- **Enum (native implemented):** Yes
- **Convert (native implemented):** Yes

</details>
<details>
<summary><strong>.NET 6.0</strong> — LTS (Extended)</summary>

- Inherits from .NET 5.0
- **Updated Dependencies:**
    - `System.Runtime.CompilerServices.Unsafe` 6.1.2
    - `System.Collections.Immutable` 8.0
    - `System.Text.Json` 8.0.5

</details>
<details>
<summary><strong>.NET 7.0</strong> — Extended</summary>

- Inherits from .NET 6.0
- **Static Virtual Members:** Yes [¹]
- Adds support for:
    - **Marshalling**

</details>
<details>
<summary><strong>.NET 8.0</strong> — LTS</summary>

- Inherits from .NET 7.0
- **No dependencies required**

</details>
<details>
<summary><strong>.NET 9.0</strong> — Current</summary>

- Inherits from .NET 8.0
- **Generic `ref struct`:** Yes [⁴]

</details>

1. AOT detection should be performed via reflection. Retrieving references to multidimensional array data should be done
   using static delegates, and managed buffer registration should be handled through buffer allocation.
2. Uses CoreCLR implementations from .NET 6.0. Simpler alternatives may be substituted in .NET Standard 2.1.
3. Internally relies on `Enum.GetName(Type, Object)`.
4. Value-type pointers support `ref struct` generics, but due to C# compiler restrictions, some methods must be
   implemented in IL.

## AOT Support

This package is AOT-friendly, all of its features support both Mono AOT and Native AOT, including full AOT and the
obsolete reflection-free mode.

The only features that require reflection are:

* Native AOT detection when targeting .NET 5.0 or earlier.

Starting with .NET 7.0, the use of reflection can be completely avoided.

## Visual Basic .NET Support

Some APIs in `Rxmxnx.PInvoke.Extensions` are not directly compatible with Visual Basic .NET due to language
limitations. The `Rxmxnx.PInvoke.VisualBasic` namespace provides equivalent delegate definitions specifically designed
for use in **Visual Basic .NET**.

These delegates provide VB.NET-compatible access to selected non-compliant `Rxmxnx.PInvoke.Extensions` APIs.

---

# Abstractions

`Rxmxnx.PInvoke.Extensions` provides abstractions for managed handling of references and
fixed memory segments.

## Reference Interfaces

These interfaces represent a safe way to access a managed reference of a specific type.

<details>
  <summary>IReadOnlyReferenceable&lt;T&gt;</summary>

This interface exposes a read-only reference to an object of type `T`, allowing the object
to be used without modification.

**Notes:**

- This interface inherits from `IEquatable<IReadOnlyReferenceable<T>>`.
- This type allows public implementation or inheritance.
- Starting with .NET 9.0, `T` can be a `ref struct`.

#### Properties:

- <details>
  <summary>Reference</summary>

  Gets the read-only reference to the instance of an object of type `T`.
  </details>

</details>

<details>
  <summary>
    IReferenceable&lt;T&gt;
  </summary>

This interface exposes a reference to an object of type `T`, allowing the object to be used and potentially modified.

**Notes:**

- This interface inherits from `IReadOnlyReferenceable<T>` and `IEquatable<IReferenceable<T>>`.
- This type allows public implementation or inheritance.
- Starting with .NET 9.0, `T` can be a `ref struct`.

#### Properties:

- <details>
  <summary>Reference</summary>

  Gets the reference to the instance of an object of type `T`.
  </details>

</details>

## Wrapper Interfaces

These interfaces represent a safe way to access a value or managed object of a specific type.

<details>
  <summary>IWrapper&lt;T&gt;</summary>

This interface defines a wrapper for a `T` object.

**Notes:**

- This interface inherits from `IEquatable<T>`. This type allows public implementation or inheritance.

#### Properties:

- <details>
  <summary>Value</summary>

  The wrapped `T` object.
  </details>

#### Static Methods:

- <details>
  <summary>Create(T?)</summary>

  Creates a new instance of an object that implements `IWrapper<T>` interface.
  </details>

#### Non-generic interface

`IWrapper` is a non-generic interface that exposes static methods for creating specific types of
`IWrapper<T>` for concrete cases of value types, nullable values, and non-nullable reference types.

**Note:** `IWrapper` contains a public interface `IBase<T>` that allows covariance. Starting with .NET 9.0, `T` can be a
`ref struct`.

##### Static Methods:

- <details>
  <summary>Create&lt;TValue&gt;(TValue)</summary>

  Creates a new instance of an object that implements `IWrapper<TValue>` interface.

  **Note:** `TValue` generic type is `struct`.
  </details>
- <details>
  <summary>CreateNullable&lt;TValue&gt;(TValue?)</summary>

  Creates a new instance of an object that implements `IWrapper<TValue?>` interface.

  **Note:** `TValue` generic type is `struct`.
  </details>
- <details>
  <summary>CreateObject&lt;TObject&gt;(TObject)</summary>

  Creates a new instance of an object that implements `IWrapper<TObject>` interface.

  **Note:** `TObject` generic type is a `reference type`.
  </details>

</details>

<details>
  <summary>IMutableWrapper&lt;T&gt;</summary>

This interface defines a wrapper for an object whose value can be modified.

**Note:** This interface inherits from `IWrapper<T>`. This type allows public implementation or inheritance.

#### Properties:

- <details>
  <summary>Value</summary>

  The wrapped `T` object.
  </details>

#### Static Methods:

- <details>
  <summary>Create(T?)</summary>

  Creates a new instance of an object that implements `IMutableWrapper<T>` interface.
  </details>

#### Non-generic interface

`IMutableWrapper` is a non-generic interface that exposes static methods for creating specific types of
`IMutableWrapper<T>` for concrete cases of value types, nullable values, and non-nullable reference types.

##### Static Methods:

- <details>
  <summary>Create&lt;TValue&gt;(TValue)</summary>

  Creates a new instance of an object that implements `IMutableWrapper<TValue>` interface.

  **Note:** `TValue` generic type is `struct`.
  </details>
- <details>
  <summary>CreateNullable&lt;TValue&gt;(TValue?)</summary>

  Creates a new instance of an object that implements `IMutableWrapper<TValue?>` interface.

  **Note:** `TValue` generic type is `struct`.
  </details>
- <details>
  <summary>CreateObject&lt;TObject&gt;(TObject)</summary>

  Creates a new instance of an object that implements `IMutableWrapper<TObject>` interface.

  **Note:** `TObject` generic type is a `reference type`.
  </details>

</details>

<details>
  <summary>IMutableReference&lt;T&gt;</summary>

This interface exposes a wrapper for `T` object that can be referenced and whose value can be modified.

**Note:** This interface inherits from `IMutableWrapper<T>` and `IReferenceable<T>`. This type allows public
implementation or inheritance.

#### Properties:

- <details>
  <summary>Reference</summary>

  Reference to `T` wrapped instance.
  </details>

#### Static Methods:

- <details>
  <summary>Create(T?)</summary>

  Creates a new instance of an object that implements `IMutableReference<T>` interface.
  </details>

#### Non-generic interface

`IMutableReference` is a non-generic interface that exposes static methods for creating specific types of
`IMutableReference<T>` for concrete cases of value types, nullable values, and non-nullable reference types.

##### Static Methods:

- <details>
  <summary>Create&lt;TValue&gt;(TValue)</summary>

  Creates a new instance of an object that implements `IMutableReference<TValue>` interface.

  **Note:** `TValue` generic type is `struct`.
  </details>
- <details>
  <summary>CreateNullable&lt;TValue&gt;(TValue?)</summary>

  Creates a new instance of an object that implements `IMutableReference<TValue?>` interface.

  **Note:** `TValue` generic type is `struct`.
  </details>
- <details>
  <summary>CreateObject&lt;TObject&gt;(TObject)</summary>

  Creates a new instance of an object that implements `IMutableReference<TObject>` interface.

  **Note:** `TObject` generic type is a `reference type`.
  </details>

</details>

## Fixed memory Interfaces

These interfaces represent a safe way to access a fixed address of native or managed memory.

<details>
  <summary>IFixedPointer</summary>

Interface representing a pointer to a fixed block of memory.

**Note:** This type allows public implementation or inheritance.

#### Properties:

- <details>
  <summary>Pointer</summary>
  Gets the pointer to the fixed block of memory.
  </details>

#### Disposable interface

`IFixedPointer.IDisposable` representing a disposable `IFixedPointer` object.
This interface is used for managing fixed memory blocks that require explicit resource cleanup.

**Note:** This interface inherits from `IFixedPointer` and `System.IDisposable`. This type allows public implementation
or inheritance.

</details>

<details>
  <summary>IFixedMethod&lt;TMethod&gt;</summary>

Interface representing a method whose memory address is fixed in memory.

**Note:** This interface inherits from `IFixedPointer`. This type allows public implementation or inheritance.

#### Properties:

- <details>
  <summary>Method</summary>
   Gets the delegate that points to the fixed method in memory.
  </details>

</details>

<details>
  <summary>IReadOnlyFixedMemory</summary>

Interface representing a read-only fixed block of memory.

**Note:** This interface inherits from `IFixedPointer`. This type allows public implementation or inheritance.

#### Properties:

- <details>
  <summary>Bytes</summary>
   Gets a read-only binary span over the fixed block of memory.
  </details>
- <details>
  <summary>Objects</summary>
   Gets a read-only object span over the fixed block of memory.
  </details>

#### Methods:

- <details>
  <summary>AsBinaryContext()</summary>

  Creates a new instance of `IReadOnlyFixedContext<Byte>` from the current instance.
  </details>
- <details>
  <summary>AsObjectContext()</summary>

  Creates a new instance of `IReadOnlyFixedContext<Object>` from the current instance.
  </details>

#### Disposable interface

`IReadOnlyFixedMemory.IDisposable` representing a disposable `IReadOnlyFixedMemory` object.
This interface is used for managing fixed memory blocks that require explicit resource cleanup.

**Note:** This interface inherits from `IReadOnlyFixedMemory` and `IFixedPointer.IDisposable`. This type allows public
implementation or inheritance.

</details>

<details>
  <summary>IReadOnlyFixedMemory&lt;T&gt;</summary>

Interface representing a read-only fixed block of memory for a specific type.

**Note:** This interface inherits from `IReadOnlyFixedMemory`. This type allows public implementation or inheritance.

#### Properties:

- <details>
  <summary>ValuePointer</summary>
   Gets the value pointer to the read-only fixed block of memory.
  </details>

- <details>
  <summary>Values</summary>

  Gets a read-only `T` span over the fixed block of memory.
  </details>

#### Disposable interface

`IReadOnlyFixedMemory<T>.IDisposable` representing a disposable `IReadOnlyFixedMemory<T>` object.
This interface is used for managing fixed memory blocks that require explicit resource cleanup.

**Note:** This interface inherits from `IReadOnlyFixedMemory<T>` and `IReadOnlyFixedMemory.IDisposable`. This type
allows public implementation or inheritance.

</details>

<details>
  <summary>IFixedMemory</summary>

Interface representing a fixed block of memory.

**Note:** This interface inherits from `IReadOnlyFixedMemory`. This type allows public implementation or inheritance.

#### Properties:

- <details>
  <summary>Bytes</summary>
   Gets a binary span over the fixed block of memory.
  </details>
- <details>
  <summary>Objects</summary>
   Gets an object span over the fixed block of memory.
  </details>

#### Methods:

- <details>
  <summary>AsBinaryContext()</summary>

  Creates a new instance of `IFixedContext<Byte>` from the current instance.
  </details>
- <details>
  <summary>AsObjectContext()</summary>

  Creates a new instance of `IFixedContext<Object>` from the current instance.
  </details>

#### Disposable interface

`IFixedMemory.IDisposable` representing a disposable `IFixedMemory` object.
This interface is used for managing fixed memory blocks that require explicit resource cleanup.

**Note:** This interface inherits from `IFixedMemory` and `IReadOnlyFixedMemory.IDisposable`. This type allows public
implementation or inheritance.

</details>

<details>
  <summary>IFixedMemory&lt;T&gt;</summary>

Interface representing a fixed block of memory for a specific type.

**Note:** This interface inherits from `IReadOnlyFixedMemory<T>` and `IFixedMemory<T>`. This type allows public
implementation or inheritance.

#### Properties:

- <details>
  <summary>ValuePointer</summary>
   Gets the value pointer to the fixed block of memory.
  </details>

- <details>
  <summary>Values</summary>

  Gets a `T` span over the fixed block of memory.
  </details>

#### Disposable interface

`IFixedMemory<T>.IDisposable` representing a disposable `IFixedMemory<T>` object.
This interface is used for managing fixed memory blocks that require explicit resource cleanup.

**Note:** This interface inherits from `IReadOnlyFixedMemory<T>` and `IFixedMemory.IDisposable`. This type allows public
implementation or inheritance.

</details>

<details>
  <summary>IReadOnlyFixedReference&lt;T&gt;</summary>

This interface exposes a read-only reference to an object of type `T`, allowing the object to be used without
modification.

**Note:** This interface inherits from `IReadOnlyReferenceable<T>` and `IReadOnlyFixedMemory`. This type allows public
implementation or inheritance. Starting with .NET 9.0, `T` can be a `ref struct`.

#### Methods:

- <details>
  <summary>Transformation(out IReadOnlyFixedMemory)</summary>

  Reinterprets the read-only `T` fixed memory reference as a read-only `TDestination` memory reference.
  </details>

#### Disposable interface

`IReadOnlyFixedReference<T>.IDisposable` representing a disposable `IReadOnlyFixedReference<T>` object.
This interface is used for managing fixed memory blocks that require explicit resource cleanup.

**Note:** This interface inherits from `IReadOnlyFixedReference<T>` and `IReadOnlyFixedMemory.IDisposable`. This type
allows public implementation or inheritance.

</details>

<details>
  <summary>IFixedReference&lt;T&gt;</summary>

This interface represents a mutable reference to a fixed memory location.

**Note:** This interface inherits from `IReferenceable<T>`, `IReadOnlyFixedReference<T>` and `IFixedMemory`. This type
allows public implementation or inheritance. Starting with .NET 9.0, `T` can be a `ref struct`.

#### Methods:

- <details>
  <summary>Transformation(out IFixedMemory)</summary>

  Reinterprets the `T` fixed memory reference as a `TDestination` memory reference.
  </details>

- <details>
  <summary>Transformation(out IReadOnlyFixedMemory)</summary>

  Reinterprets the `T` fixed memory reference as a `TDestination` memory reference.
  </details>

#### Disposable interface

`IFixedReference<T>.IDisposable` representing a disposable `IFixedReference<T>` object.
This interface is used for managing fixed memory blocks that require explicit resource cleanup.

**Note:** This interface inherits from `IFixedReference<T>` and `IReadOnlyFixedReference<T>.IDisposable`. This type
allows public implementation or inheritance.

</details>

<details>
  <summary>IReadOnlyFixedContext&lt;T&gt;</summary>

Interface representing a context from a read-only block of fixed memory.

**Note:** This interface inherits from `IReadOnlyFixedMemory<T>`. This type allows public implementation or inheritance.

#### Methods:

- <details>
  <summary>Transformation(out IReadOnlyFixedMemory)</summary>

  Reinterprets the `T` fixed memory block as `TDestination` memory block.
  </details>

#### Disposable interface

`IReadOnlyFixedContext<T>.IDisposable` representing a disposable `IReadOnlyFixedContext<T>` object.
This interface is used for managing fixed memory blocks that require explicit resource cleanup.

**Note:** This interface inherits from `IReadOnlyFixedContext<T>` and `IReadOnlyFixedMemory<T>.IDisposable`. This type
allows public implementation or inheritance.

</details>

<details>
  <summary>IFixedContext&lt;T&gt;</summary>

Interface representing a context from a block of fixed memory.

**Note:** This interface inherits from `IReadOnlyFixedContext<T>` and `IFixedMemory<T>`. This type allows public
implementation or inheritance.

#### Methods:

- <details>
  <summary>Transformation(out IFixedMemory)</summary>

  Reinterprets the `T` fixed memory block as a `TDestination` memory block.
  </details>

- <details>
  <summary>Transformation(out IReadOnlyFixedMemory)</summary>
   Reinterprets the `T` fixed memory block as a `TDestination` memory block.
  </details>

#### Disposable interface

`IFixedContext<T>.IDisposable` representing a disposable `IFixedContext<T>` object.
This interface is used for managing fixed memory blocks that require explicit resource cleanup.

**Note:** This interface inherits from `IFixedContext<T>` and `IFixedMemory<T>.IDisposable`. This type allows public
implementation or inheritance.

</details>

## Functional Interfaces

These interfaces expose functionalities for internal types or default functional implementations.

<details>
  <summary>IEnumerableSequence&lt;T&gt;</summary>

Defines methods to support a simple iteration over a sequence of a specified type.

**Notes:**

- This interface inherits from `IEnumerable<T>`. This type allows public implementation or inheritance.
- Starting with .NET 9.0, `T` can be a `ref struct`.

#### Methods:

- <details>
  <summary>GetItem(Int32)</summary>
   Retrieves the element at the specified index.
  </details>

- <details>
  <summary>GetSize()</summary>
   Retrieves the total number of elements in the sequence.
  </details>

#### Protected Methods:

- <details>
  <summary>DisposeEnumeration(Int32)</summary>

  Method to call when `IEnumerator<T>` is disposing.

  **Note:** This method is not available in .NET Standard 2.1 library.
  </details>

#### Protected Static Methods:

- <details>
  <summary>CreateEnumerator(IEnumerableSequence&lt;T&gt;, Action&lt;IEnumerableSequence&lt;T&gt;gt;?)</summary>

  This static method allows the creation of an internal `IEnumerator<T>` instance using an `IEnumerableSequence<T>`.
  The `Action` delegate is used during the enumerator's disposal.

  Starting with .NET Core 3.0, the implementation of the `DisposeEnumeration(Int32)` method is ignored.
  However, this method may still be required when targeting a .NET Standard 2.1 library on the Mono framework.
  </details>

</details>

<details>
  <summary>IUtf8FunctionState&lt;TSelf&gt;</summary>

Interface representing a value state for functional CString creation.

**Notes:**

- `TSelf` generic type is `struct`. This type allows public implementation or inheritance.
- This type is available only on .NET 7.0 and later.

#### Static Abstract/Virtual Properties:

- <details>
  <summary>Alloc</summary>
    Function that allocates a state instance.
  </details>

#### Properties:

- <details>
  <summary>IsNullTerminated</summary>
   Indicates whether resulting UTF-8 text is null-terminated.
  </details>

#### Static Abstract/Virtual Methods:

- <details>
  <summary>GetSpan(TSelf)</summary>
   Retrieves the span from state.
  </details>

- <details>
  <summary>GetLength(in TSelf)</summary>
   Retrieves the span length from state.
  </details>

</details>

<details>
  <summary>IManagedBuffer&lt;T&gt;</summary>

This interfaces exposes a managed buffer.

**Note:** This type does not allow public implementation or inheritance.

#### Static Methods:

- <details>
  <summary>GetMetadata&lt;TBuffer&gt;()</summary>

  Retrieves the `BufferTypeMetadata<T>` instance from `TBuffer`.
  </details>

</details>

<details>
  <summary>IManagedBinaryBuffer&lt;T&gt;</summary>

This interfaces exposes a binary managed buffer.

**Note:** This interface inherits from `IManagedBuffer<T>`. This type does not allow public implementation or
inheritance.

#### Properties:

- <details>
  <summary>Metadata&lt;TBuffer&gt;()</summary>

  Retrieves the `BufferTypeMetadata<T>` instance from current instance.
  </details>

</details>

<details>
  <summary>IManagedBinaryBuffer&lt;TBuffer, T&gt;</summary>

This interfaces exposes a binary managed buffer.

**Note:** `TBuffer` generic type is `struct`. This interface inherits from `IManagedBinaryBuffer<T>`. This type not
allows public implementation or inheritance.

</details>

## Span Delegates

These delegates encapsulate methods that operate with Span<T> instances.

<details>
  <summary>ReadOnlySpan&lt;T&gt; ReadOnlySpanFunc&lt;T&gt;()</summary>

Encapsulates a method that has no parameters and returns a read-only span of type `T`.

</details>

<details>
  <summary>ReadOnlySpan&lt;T&gt; ReadOnlySpanFunc&lt;T, in TState&gt;(TState)</summary>

Encapsulates a method that has a `TState` parameter and returns a read-only span of type T.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

<details>
  <summary>TResult SpanFunc&lt;T, in TArg, out TResult&gt;(Span&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives a span of type `T`, a state object of type `TArg` and returns a result of type
`TResult`.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

<details>
  <summary>TResult ReadOnlySpanFunc&lt;T, in TArg, out TResult&gt;(Span&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives a read-only span of type `T`, a state object of type `TArg` and returns a result
of type `TResult`.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

## Fixed memory Delegates

These delegates encapsulate methods that operate with fixed memory blocks.

<details>
  <summary>void FixedAction(in IFixedMemory)</summary>

Represents an action that operates on a fixed memory instance.

</details>

<details>
  <summary>void FixedAction&lt;in TArg&gt;(in IFixedMemory, TArg)</summary>

Represents an action that operates on a fixed memory instance using an additional state object.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

<details>
  <summary>void ReadOnlyFixedAction(in IReadOnlyFixedMemory)</summary>

Represents an action that operates on a read-only fixed memory instance.

</details>

<details>
  <summary>void ReadOnlyFixedAction&lt;in TArg&gt;(in IReadOnlyFixedMemory, TArg)</summary>

Represents an action that operates on a read-only fixed memory instance using an additional state object.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

<details>
  <summary>TResult FixedFunc&lt;out TResult&gt;(in IFixedMemory)</summary>

Represents a function that operates on a fixed memory instance.

</details>

<details>
  <summary>TResult FixedFunc&lt;in TArg, out TResult&gt;(in IFixedMemory, TArg)</summary>

Represents a function that operates on a fixed memory instance using an additional state object.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

<details>
  <summary>TResult ReadOnlyFixedFunc&lt;out TResult&gt;(in IReadOnlyFixedMemory&lt;T&gt;)</summary>

Represents a function that operates on a read-only fixed memory instance.

</details>

<details>
  <summary>TResult ReadOnlyFixedFunc&lt;in TArg, out TResult&gt;(in IReadOnlyFixedMemory&lt;T&gt;, TArg)</summary>

Represents a function that operates on a read-only fixed memory instance using an additional state object.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

## Fixed memory context Delegates

These delegates encapsulate methods that operate on fixed memory blocks of a specific type.

<details>
  <summary>void FixedContextAction&lt;T&gt;(in IFixedContext&lt;T&gt;)</summary>

Encapsulates a method that receives an instance of `IFixedContext<T>`.

</details>

<details>
  <summary>void FixedContextAction&lt;T, in TArg,&gt;(in IFixedContext&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives an instance of `IFixedContext<T>` and a state object of type `TArg`.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

<details>
  <summary>void ReadOnlyFixedContextAction&lt;T&gt;(in IReadOnlyFixedContext&lt;T&gt;)</summary>

Encapsulates a method that receives an instance of `IReadOnlyFixedContext<T>`.

</details>

<details>
  <summary>void ReadOnlyFixedContextAction&lt;T, in TArg,&gt;(in IFixedContext&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives an instance of `IReadOnlyFixedContext<T>` and a state object of type `TArg`.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

<details>
  <summary>TResult FixedContextFunc&lt;T, out TResult&gt;(in IFixedContext&lt;T&gt;)</summary>

Encapsulates a method that receives an instance of `IFixedContext<T>` and returns a value of type `TResult`.

</details>

<details>
  <summary>TResult FixedContextFunc&lt;T, in TArg, out TResult&gt;(in IFixedContext&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives an instance of `IFixedContext<T>` and returns a value of type `TResult`.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

<details>
  <summary>TResult ReadOnlyFixedContextFunc&lt;T, out TResult&gt;(in IReadOnlyFixedContext&lt;T&gt;)</summary>

Encapsulates a method that receives an instance of `IReadOnlyFixedContext<T>` and returns a value of type `TResult`.

</details>

<details>
  <summary>TResult FixedContextFunc&lt;T, in TArg, out TResult&gt;(in IReadOnlyFixedContext&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives an instance of `IReadOnlyFixedContext<T>` and returns a value of type `TResult`.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

## Fixed memory reference Delegates

These delegates encapsulate methods that operate on fixed memory references of a specific type.

<details>
  <summary>void FixedReferenceAction&lt;T&gt;(in IFixedReference&lt;T&gt;)</summary>

Encapsulates a method that receives an instance of `IFixedReference<T>`.

**Note:** Starting with .NET 9.0, both `T` and `TArg` can be `ref struct` types.
</details>

<details>
  <summary>void FixedReferenceAction&lt;T, in TArg,&gt;(in IFixedReference&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives an instance of `IFixedReference<T>` and a state object of type `TArg`.

**Note:** Starting with .NET 9.0, both `T` and `TArg` can be `ref struct` types.
</details>

<details>
  <summary>void ReadOnlyFixedReferenceAction&lt;T&gt;(in IReadOnlyFixedReference&lt;T&gt;)</summary>

Encapsulates a method that receives an instance of `IReadOnlyFixedReference<T>`.

**Note:** Starting with .NET 9.0, `T` can be a `ref struct`.
</details>

<details>
  <summary>void ReadOnlyFixedReferenceAction&lt;T, in TArg,&gt;(in IFixedReference&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives an instance of `IReadOnlyFixedReference<T>` and a state object of type `TArg`.

**Note:** Starting with .NET 9.0, both `T` and `TArg` can be `ref struct` types.
</details>

<details>
  <summary>TResult FixedReferenceFunc&lt;T, out TResult&gt;(in IFixedReference&lt;T&gt;)</summary>

Encapsulates a method that receives an instance of `IFixedReference<T>` and returns a value of type `TResult`.

**Note:** Starting with .NET 9.0, `T` can be a `ref struct`.
</details>

<details>
  <summary>TResult FixedReferenceFunc&lt;T, in TArg, out TResult&gt;(in IFixedReference&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives an instance of `IFixedReference<T>` and returns a value of type `TResult`.

**Note:** Starting with .NET 9.0, both `T` and `TArg` can be `ref struct` types.
</details>

<details>
  <summary>TResult ReadOnlyFixedReferenceFunc&lt;T, out TResult&gt;(in IReadOnlyFixedReference&lt;T&gt;)</summary>

Encapsulates a method that receives an instance of `IReadOnlyFixedReference<T>` and returns a value of type `TResult`.

**Note:** Starting with .NET 9.0, `T` can be a `ref struct`.
</details>

<details>
  <summary>TResult FixedReferenceFunc&lt;T, in TArg, out TResult&gt;(in IReadOnlyFixedReference&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives an instance of `IReadOnlyFixedReference<T>` and returns a value of type `TResult`.

**Note:** Starting with .NET 9.0, both `T` and `TArg` can be `ref struct` types.
</details>

## Fixed address method Delegates

These delegates encapsulate methods that operate on methods with a fixed memory address.

<details>
  <summary>void FixedMethodAction&lt;T&gt;(in IFixedMethod&lt;T&gt;)</summary>

Encapsulates a method that receives an instance of `IFixedMethod<T>`.

</details>

<details>
  <summary>void FixedMethodAction&lt;T, in TArg,&gt;(in IFixedMethod&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives an instance of `IFixedMethod<T>` and a state object of type `TArg`.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

<details>
  <summary>TResult FixedMethodFunc&lt;T, out TResult&gt;(in IFixedMethod&lt;T&gt;)</summary>

Encapsulates a method that receives an instance of `IFixedMethod<T>` and returns a value of type `TResult`.

</details>

<details>
  <summary>TResult FixedMethodFunc&lt;T, in TArg, out TResult&gt;(in IFixedMethod&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives an instance of `IFixedMethod<T>` and returns a value of type `TResult`.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

## Fixed memory list Delegates

These delegates encapsulate methods that operate on a list of fixed memory blocks.

<details>
  <summary>void FixedListAction(FixedMemoryList)</summary>

Encapsulates a method that receives an instance of `FixedMemoryList`.

</details>

<details>
  <summary>void FixedListAction&lt;in TArg,&gt;(FixedMemoryList, TArg)</summary>

Encapsulates a method that receives an instance of `FixedMemoryList` and a state object of type `TArg`.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

<details>
  <summary>void ReadOnlyFixedListAction(ReadOnlyFixedMemoryList)</summary>

Encapsulates a method that receives an instance of `ReadOnlyFixedMemoryList`.

</details>

<details>
  <summary>void ReadOnlyFixedListAction&lt;in TArg,&gt;(FixedMemoryList, TArg)</summary>

Encapsulates a method that receives an instance of `ReadOnlyFixedMemoryList` and a state object of type `TArg`.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

<details>
  <summary>TResult FixedListFunc&lt;out TResult&gt;(FixedMemoryList)</summary>

Encapsulates a method that receives an instance of `FixedMemoryList` and returns a value of type `TResult`.

</details>

<details>
  <summary>TResult FixedListFunc&lt;in TArg, out TResult&gt;(FixedMemoryList, TArg)</summary>

Encapsulates a method that receives an instance of `FixedMemoryList` and returns a value of type `TResult`.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

<details>
  <summary>TResult ReadOnlyFixedListFunc&lt;out TResult&gt;(ReadOnlyFixedMemoryList)</summary>

Encapsulates a method that receives an instance of `ReadOnlyFixedMemoryList` and returns a value of type `TResult`.

</details>

<details>
  <summary>TResult FixedListFunc&lt;in TArg, out TResult&gt;(ReadOnlyFixedMemoryList, TArg)</summary>

Encapsulates a method that receives an instance of `ReadOnlyFixedMemoryList` and returns a value of type `TResult`.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

## UTF-8/ASCII Delegates

These delegates encapsulate methods that allow creating and operating with UTF-8/ASCII text.

<details>
  <summary>void CStringSequenceCreationAction&lt;in TArg&gt;(Span&lt;Byte&gt;, Int32, TArg)</summary>

Encapsulates a method that receives a span of bytes, an index and a state object of type `TArg`.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

<details>
  <summary>void CStringSequenceAction(FixedCStringSequence)</summary>

Encapsulates a method that operates on a `FixedCStringSequence` instance.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

<details>
  <summary>void CStringSequenceAction&lt;in TArg&gt;(FixedCStringSequence, TArg)</summary>

Encapsulates a method that operates on a `FixedCStringSequence` instance and a state object of type `TArg`.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

<details>
  <summary>TResult CStringSequenceFunc&lt;out TResult&gt;(FixedCStringSequence)</summary>

Encapsulates a method that operates on a `FixedCStringSequence` instance and returns a value of type `TResult`.

</details>

<details>
  <summary>TResult CStringSequenceFunc&lt;in TArg, out TResult&gt;(FixedCStringSequence, TArg)</summary>


Encapsulates a method that operates on a `FixedCStringSequence` instance, a state object of type `TArg` and returns a
value of type `TResult`.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

## Buffers Delegates

These delegates encapsulate methods that operate on instances of managed buffers.

<details>
  <summary>void ScopedBufferAction&lt;T&gt;(ScopedBuffer)</summary>

Encapsulates a method that receives a buffer of objects of type `T`.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

<details>
  <summary>void ScopedBufferAction&lt;T, in TArg&gt;(ScopedBuffer, TArg)</summary>

Encapsulates a method that receives a buffer of objects of type `T` and a state object of type `TArg`.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

<details>
  <summary>TResult ScopedBufferFunc&lt;T, out TResult&gt;(ScopedBuffer)</summary>

Encapsulates a function that receives a buffer of objects of type `T` and returns a result of type `TResult`.
</details>

<details>
  <summary>TResult ScopedBufferFunc&lt;T, in TArg, out TResult&gt;(ScopedBuffer, TArg)</summary>


Encapsulates a method that receives a buffer of objects of type `T`, a state object of type `TArg` and returns a value
of type `TResult`.

**Note:** Starting with .NET 9.0, `TState` can be a `ref struct`.

</details>

---

# Enums

`Rxmxnx.PInvoke.Extensions` provides enums that simplify the classification of various types of values.

## Iso639P1

<details>
<summary>Enumeration of ISO 639-1 (two letter) language codes.</summary>

| Code | Value | Language                   |
|------|-------|----------------------------|
| Iv   | 0x00  | .NET Invariant             |
| Ab   | 0x01  | Abkhazian                  |
| Aa   | 0x02  | Afar                       |
| Af   | 0x03  | Afrikaans                  |
| Ak   | 0x04  | Akan                       |
| Sq   | 0x05  | Albanian                   |
| Am   | 0x06  | Amharic                    |
| Ar   | 0x07  | Arabic                     |
| An   | 0x08  | Aragonese                  |
| Hy   | 0x09  | Armenian                   |
| As   | 0x0A  | Assamese                   |
| Av   | 0x0B  | Avaric                     |
| Ae   | 0x0C  | Avestan                    |
| Ay   | 0x0D  | Aymara                     |
| Az   | 0x0E  | Azerbaijani                |
| Bm   | 0x0F  | Bambara                    |
| Ba   | 0x10  | Bashkir                    |
| Eu   | 0x11  | Basque                     |
| Be   | 0x12  | Belaurian                  |
| Bn   | 0x13  | Bengali                    |
| Bi   | 0x14  | Bislama                    |
| Bs   | 0x15  | Bosnian                    |
| Br   | 0x16  | Breton                     |
| Bg   | 0x17  | Bulgarian                  |
| My   | 0x18  | Burmese                    |
| Ca   | 0x19  | Catalan; Valencian         |
| Ch   | 0x1A  | Chamorro                   |
| Ce   | 0x1B  | Chechen                    |
| Ny   | 0x1C  | Chichewa; Chewa; Nyanja    |
| Zh   | 0x1D  | Chinese                    |
| Cv   | 0x1E  | Chuvash                    |
| Kw   | 0x1F  | Cornish                    |
| Co   | 0x20  | Corsican                   |
| Cr   | 0x21  | Cree                       |
| Cs   | 0x22  | Czech                      |
| Da   | 0x23  | Danish                     |
| Dv   | 0x24  | Divehi; Dhivehi; Maldivian |
| Nl   | 0x25  | Dutch; Flemish             |
| Dz   | 0x26  | Dzongkha                   |
| En   | 0x27  | English                    |
| Eo   | 0x28  | Esperanto                  |
| Et   | 0x29  | Estonian                   |
| Ee   | 0x2A  | Ewe                        |
| Fo   | 0x2B  | Faroese                    |
| Fj   | 0x2C  | Fijian                     |
| Fi   | 0x2D  | Finnish                    |
| Fr   | 0x2E  | French                     |
| Ff   | 0x2F  | Fulah                      |
| Gl   | 0x30  | Galician                   |
| Ka   | 0x31  | Georgian                   |
| De   | 0x32  | German                     |
| El   | 0x33  | Greek, Modern              |
| Gn   | 0x34  | Guarani                    |
| Gu   | 0x35  | Gujarati                   |
| Ht   | 0x36  | Haitian; Haitian Creole    |
| Ha   | 0x37  | Hausa                      |
| He   | 0x38  | Hebrew                     |
| Hz   | 0x39  | Herero                     |
| Hi   | 0x3A  | Hindi                      |
| Ho   | 0x3B  | Hiri Motu                  |
| Hu   | 0x3C  | Hungarian                  |
| Is   | 0x3D  | Icelandic                  |
| Io   | 0x3E  | Ido                        |
| Ig   | 0x3F  | Igbo                       |
| Id   | 0x40  | Indonesian                 |
| Ia   | 0x41  | Interlingua                |
| Ie   | 0x42  | Interlingue                |
| Iu   | 0x43  | Inuktitut                  |
| Ik   | 0x44  | Inupiaq                    |
| Ga   | 0x45  | Irish                      |
| It   | 0x46  | Italian                    |
| Ja   | 0x47  | Japanese                   |
| Jv   | 0x48  | Javanese                   |
| Kl   | 0x49  | Kalaallisut                |
| Kn   | 0x4A  | Kannada                    |
| Kr   | 0x4B  | Kanuri                     |
| Ks   | 0x4C  | Kashmiri                   |
| Kk   | 0x4D  | Kazakh                     |
| Ki   | 0x4E  | Kikuyu                     |
| Rw   | 0x4F  | Kinyarwanda                |
| Ky   | 0x50  | Kyrgyz                     |
| Kv   | 0x51  | Komi                       |
| Kg   | 0x52  | Kongo                      |
| Ko   | 0x53  | Korean                     |
| Kj   | 0x54  | Kuanyama                   |
| Ku   | 0x55  | Kurdish                    |
| Lo   | 0x56  | Lao                        |
| La   | 0x57  | Latin                      |
| Lv   | 0x58  | Latvian                    |
| Li   | 0x59  | Limburgish                 |
| Ln   | 0x5A  | Lingala                    |
| Lt   | 0x5B  | Lithuanian                 |
| Lu   | 0x5C  | Luba-Katanga               |
| Lb   | 0x5D  | Luxembourgish              |
| Mk   | 0x5E  | Macedonian                 |
| Mg   | 0x5F  | Malagasy                   |
| Ms   | 0x60  | Malay                      |
| Ml   | 0x61  | Malayalam                  |
| Mt   | 0x62  | Maltese                    |
| Gv   | 0x63  | Manx                       |
| Mi   | 0x64  | Maori                      |
| Mr   | 0x65  | Marathi                    |
| Mh   | 0x66  | Marshallese                |
| Mn   | 0x67  | Mongolian                  |
| Na   | 0x68  | Nauru                      |
| Nv   | 0x69  | Navajo                     |
| Ng   | 0x6A  | Ndonga                     |
| Ne   | 0x6B  | Nepali                     |
| Nd   | 0x6C  | North Ndebele              |
| Se   | 0x6D  | Northern Sami              |
| No   | 0x6E  | Norwegian                  |
| Nb   | 0x6F  | Norwegian Bokmål           |
| Nn   | 0x70  | Norwegian Nynorsk          |
| Ii   | 0x71  | Nuosu                      |
| Oc   | 0x72  | Occitan                    |
| Oj   | 0x73  | Ojibwa                     |
| Or   | 0x74  | Oriya                      |
| Om   | 0x75  | Oromo                      |
| Os   | 0x76  | Ossetian                   |
| Pa   | 0x77  | Panjabi                    |
| Pi   | 0x78  | Pali                       |
| Fa   | 0x79  | Persian                    |
| Pl   | 0x7A  | Polish                     |
| Ps   | 0x7B  | Pashto                     |
| Pt   | 0x7C  | Portuguese                 |
| Qu   | 0x7D  | Quechua                    |
| Ro   | 0x7E  | Romanian                   |
| Rm   | 0x7F  | Romansh                    |
| Rn   | 0x80  | Rundi                      |
| Ru   | 0x81  | Russian                    |
| Sm   | 0x82  | Samoan                     |
| Sg   | 0x83  | Sango                      |
| Sa   | 0x84  | Sanskrit                   |
| Sc   | 0x85  | Sardinian                  |
| Gd   | 0x86  | Scottish Gaelic            |
| Sr   | 0x87  | Serbian                    |
| Sn   | 0x88  | Shona                      |
| Sd   | 0x89  | Sindhi                     |
| Si   | 0x8A  | Sinhalese                  |
| Sk   | 0x8B  | Slovak                     |
| Sl   | 0x8C  | Slovenian                  |
| So   | 0x8D  | Somali                     |
| St   | 0x8E  | Southern Sotho             |
| Nr   | 0x8F  | South Ndebele              |
| Es   | 0x90  | Spanish                    |
| Su   | 0x91  | Sundanese                  |
| Sw   | 0x92  | Swahili                    |
| Ss   | 0x93  | Swati                      |
| Sv   | 0x94  | Swedish                    |
| Tl   | 0x95  | Tagalog                    |
| Ty   | 0x96  | Tahitian                   |
| Tg   | 0x97  | Tajik                      |
| Ta   | 0x98  | Tamil                      |
| Tt   | 0x99  | Tatar                      |
| Te   | 0x9A  | Telugu                     |
| Th   | 0x9B  | Thai                       |
| Bo   | 0x9C  | Tibetan                    |
| Ti   | 0x9D  | Tigrinya                   |
| To   | 0x9E  | Tonga                      |
| Ts   | 0x9F  | Tsonga                     |
| Tn   | 0xA0  | Tswana                     |
| Tr   | 0xA1  | Turkish                    |
| Tk   | 0xA2  | Turkmen                    |
| Tw   | 0xA3  | Twi                        |
| Ug   | 0xA4  | Uighur                     |
| Uk   | 0xA5  | Ukrainian                  |
| Ur   | 0xA6  | Urdu                       |
| Uz   | 0xA7  | Uzbek                      |
| Ve   | 0xA8  | Venda                      |
| Vi   | 0xA9  | Vietnamese                 |
| Vo   | 0xAA  | Volapük                    |
| Wa   | 0xAB  | Walloon                    |
| Cy   | 0xAC  | Welsh                      |
| Fy   | 0xAD  | Western Frisian            |
| Wo   | 0xAE  | Wolof                      |
| Xh   | 0xAF  | Xhosa                      |
| Yi   | 0xB0  | Yiddish                    |
| Yo   | 0xB1  | Yoruba                     |
| Za   | 0xB2  | Zhuang                     |
| Zu   | 0xB3  | Zulu                       |

</details>

# Structs

`Rxmxnx.PInvoke.Extensions` provides value types for the managed handling of references, fixed memory segments, and
stack-allocated buffers

## Pointers

These structures safely represent memory addresses for references or methods of a specific type.

<details>
  <summary>ReadOnlyValPtr&lt;T&gt;</summary>

Represents a platform-specific type used to manage a pointer to a read-only value of type `T`.

**Notes:**

- This struct implements `IComparable`, `ISpanFormattable`, `ISerializable`, `IWrapper<IntPtr>`,
  `IEquatable<ReadOnlyValPtr<T>>` and `IComparable<ReadOnlyValPtr<T>>` interfaces.
- Starting with .NET 7.0, this struct implements `IParsable<ReadOnlyValPtr<T>>` interface.
- Starting with .NET 9.0, `T` can be a `ref struct`.

#### Static Properties:

- <details>
  <summary>Zero</summary>
  A read-only field that represents a pointer that has been initialized to zero.
  </details>
- <details>
  <summary>IsUnmanaged</summary>

  Indicates if `T` type is an `unmanaged` type.
  </details>

#### Properties:

- <details>
  <summary>Pointer</summary>

  Internal pointer as an `IntPtr`.
  </details>
- <details>
  <summary>IsZero</summary>

  Indicates whether the current pointer is `null`.
  </details>
- <details>
  <summary>Reference</summary>
  A read-only reference to the value pointed to by this instance.
  </details>

#### Methods:

- <details>
  <summary>GetUnsafeFixedReference(IDisposable?)</summary>

  Retrieves an unsafe `IReadOnlyFixedReference<T>.IDisposable` instance from current read-only reference pointer.
  </details>
- <details>
  <summary>GetUnsafeFixedContext(IDisposable?)</summary>

  Retrieves an unsafe `IReadOnlyFixedContext<T>.IDisposable` instance from current read-only reference pointer.

  **Note:** In .NET 9.0 and later, invoking this method on an instance whose generic type argument is a `ref struct`
  will cause a runtime error.
  </details>

</details>

<details>
  <summary>ValPtr&lt;T&gt;</summary>

Represents a platform-specific type used to manage a pointer to a mutable value of type `T`.

**Notes:**

- This struct implements `IComparable`, `ISpanFormattable`, `ISerializable`, `IWrapper<IntPtr>`,
  `IEquatable<ValPtr<T>>` and `IComparable<ValPtr<T>>` interfaces.
- Starting with .NET 7.0, this struct implements `IParsable<ValPtr<T>>` interface.
- Starting with .NET 9.0, `T` can be a `ref struct`.

#### Static Properties:

- <details>
  <summary>Zero</summary>
  A read-only field that represents a pointer that has been initialized to zero.
  </details>
- <details>
  <summary>IsUnmanaged</summary>

  Indicates if `T` type is an `unmanaged` type.
  </details>

#### Properties:

- <details>
  <summary>Pointer</summary>

  Internal pointer as an `IntPtr`.
  </details>
- <details>
  <summary>IsZero</summary>
  Indicates whether the current pointer is `null`.
  </details>
- <details>
  <summary>Reference</summary>
  A reference to the value pointed to by this instance.
  </details>

#### Methods:

- <details>
  <summary>GetUnsafeFixedReference(IDisposable?)</summary>

  Retrieves an unsafe `IReadOnlyFixedReference<T>.IDisposable` instance from current reference pointer.
  </details>
- <details>
  <summary>GetUnsafeFixedContext(IDisposable?)</summary>

  Retrieves an unsafe `IReadOnlyFixedContext<T>.IDisposable` instance from current reference pointer.

  **Note:** In .NET 9.0 and later, invoking this method on an instance whose generic type argument is a `ref struct`
  will cause a runtime error.
  </details>

</details>

<details>
  <summary>FuncPtr&lt;TDelegate&gt;</summary>

Represents a platform-specific type used to handle a pointer to a method of type `TDelegate`.

**Note:** This struct implements `ISpanFormattable`, `ISerializable`, `IWrapper<IntPtr>` and `IEquatable<FuncPtr<T>>`
interfaces.

#### Static Properties:

- <details>
  <summary>Zero</summary>
  A read-only field representing a null-initialized function pointer.
  </details>

#### Properties:

- <details>
  <summary>Pointer</summary>

  Internal pointer as an `IntPtr`.
  </details>
- <details>
  <summary>IsZero</summary>
  Indicates whether the current pointer is `null`.
  </details>
- <details>
  <summary>Invoke</summary>
  A managed delegate using the method address pointed to by this instance.
  </details>

</details>

**Note:** Starting with .NET 7.0, these structs can be used with source-generated P/Invoke marshalling.

## Fixed memory lists

These structures represent lists of fixed memory block addresses.

<details>
  <summary>ReadOnlyFixedMemoryList</summary>

Represents a list of `IReadOnlyFixedMemory` instances.

**Note:** This type is a `ref struct`. Instances of current type can be used with `foreach`.

#### Properties:

- <details>
  <summary>Count</summary>
  Gets the total number of elements in the list.
  </details>
- <details>
  <summary>IsEmpty</summary>
  Indicates whether the current list is empty.
  </details>

#### Indexer:

- <details>
  <summary>Item</summary>

  Gets the `IReadOnlyFixedMemory` at the specified index.
  </details>

#### Methods:

- <details>
  <summary>ToArray()</summary>

  Creates an array from the current `ReadOnlyFixedMemoryList` instance.
  </details>

</details>

<details>
  <summary>FixedMemoryList</summary>

Represents a list of `IFixedMemory` instances.

**Note:** This type is a `ref struct`. Instances of current type can be used with `foreach`.

#### Properties:

- <details>
  <summary>Count</summary>
  Gets the total number of elements in the list.
  </details>
- <details>
  <summary>IsEmpty</summary>
  Indicates whether the current list is empty.
  </details>

#### Indexer:

- <details>
  <summary>Item</summary>

  Gets the `IFixedMemory` at the specified index.
  </details>

#### Methods:

- <details>
  <summary>ToArray()</summary>

  Creates an array from the current `FixedMemoryList` instance.
  </details>

</details>

<details>
  <summary>FixedCStringSequence</summary>

Represents a `CStringSequence` that is fixed in memory.

**Note:** This type is a `ref struct`. Instances of current type can be used with `foreach`.

#### Properties:

- <details>
  <summary>Values</summary>

  Gets the list of `CString` values in the sequence.
  </details>

#### Indexer:

- <details>
  <summary>Item</summary>
  Gets the element at the given index in the sequence.
  </details>

#### Methods:

- <details>
  <summary>ToArray()</summary>

  Creates an array of `IReadOnlyFixedMemory` instances from the current instance.
  </details>

</details>

## Managed buffer types

These structures represent managed buffers that can be stored on the stack.

<details>
  <summary>Atomic&lt;T&gt;</summary>

Atomic binary buffer.

**Note:** This type represents the binary unit 2<sup>0</sup> space.

</details>

<details>
  <summary>Composite&lt;TBufferA, TBufferB,T&gt;</summary>

Composite binary buffer.

**Notes:**

- This type represents a composite binary buffer of size A + B.
- `TBufferA` and `TBufferB` must be binary buffers.
- `TBufferB` must always be a space of 2<sup>n</sup>.
- `TBufferA` must be a buffer smaller than or equal to `TBufferB`.
- If `TBufferA` and `TBufferB` are equal, the current type will represent a binary space of 2<sup>n + 1</sup>.

</details>

<details>
  <summary>NonBinarySpace&lt;TArray, T&gt;</summary>

Non-binary buffer space.

**Notes:**

- This type represents a non-binary buffer based on a custom structure `TArray`.
- `TArray` and `T` must be compatible.
- The size of this space is the amount of elements of type `T` can be stored in a `TArray` instance.

</details>

<details>
  <summary>CStringSequence.Utf8View</summary>

A view over the UTF-8 items in a CStringSequence instance, allowing filtering of empty and null items.

**Notes:**

- This type is a `ref struct` type. Instances of current type can be used with `foreach`.
- The enumerator of this type is also a `ref struct` type.
- The type of the enumeration of this type is `ReadOnlySpan<Byte>` items.
- Each item in the enumeration is guaranteed to be null-terminated.
- By default, the UTF-8 data is not pinned in memory. To pin it during enumeration, the source `CStringSequence`
  instance must be pinned, and the` CString.Empty` instance must also be pinned if the enumeration includes empty items.

#### Properties:

- <details>
  <summary>Source</summary>

  Enumeration source `CStringSequence` instance.
  </details>
- <details>
  <summary>EmptyItemsIncluded</summary>

  Indicates whether current enumeration includes empty items from the source `CStringSequence` instance.
  </details>
- <details>
  <summary>Count</summary>
  Gets the total number of elements in the current enumeration.
  </details>

</details>

---

# Classes

`Rxmxnx.PInvoke.Extensions` provides reference types for the managed handling of UTF8/ASCII texts, memory regions, and
metadata for stack-allocated buffer management.

<details>
  <summary>ValueRegion&lt;T&gt;</summary>

This class represents a region of memory that contains a sequence of `T` values.

**Note:** This type does not allow public inheritance.

### Properties:

- <details>
  <summary>IsMemorySlice</summary>
  Indicates whether the current instance represents a subregion of a memory region.
  </details>

### Indexer:

- <details>
  <summary>Item</summary>
  Retrieves an item from the memory region at the specified zero-based index.
  </details>

### Methods:

- <details>
  <summary>ToArray()</summary>
  Copies the contents of this memory region into a new array.
  </details>
- <details>
  <summary>TryAlloc(GCHandleType, out GCHandle)</summary>
  Tries to create a new GC CHandle for current value region.
  </details>
- <details>
  <summary>GetPinnable(out Int32)</summary>
  Retrieves the pinnable instance for current region.
  </details>

### Operators:

- <details>
  <summary>ReadOnlySpan&lt;T&gt;(ValueRegion&lt;T&gt;)</summary>

  Converts the value of the current `ValueRegion<T>` to its equivalent read-only span representation.
  </details>
- <details>
  <summary>T[]?(ValueRegion&lt;T&gt;)</summary>

  Converts the value of the current `ValueRegion<T>` to its equivalent array representation.
  </details>

### Static Methods:

- <details>
  <summary>Create(T[])</summary>

  Creates a new `ValueRegion<T>` instance from an array of `T` values.
  </details>
- <details>
  <summary>Create(IntPtr, Int32)</summary>

  Creates a new `ValueRegion<T>` instance from a pointer to a native memory region.
  </details>
- <details>
  <summary>Create(ReadOnlySpanFunc&lt;T&gt;)</summary>

  Creates a new `ValueRegion<T>` instance from a `ReadOnlySpanFunc<T>` function.
  </details>
- <details>
  <summary>Create&lt;TState&gt;(TState, ReadOnlySpanFunc&lt;T, TState&gt;)</summary>

  Creates a new `ValueRegion<T>` instance from a `ReadOnlySpanFunc<T, TState>` function and a TState instance.
  </details>
- <details>
  <summary>Create&lt;TState&gt;(TState, ReadOnlySpanFunc&lt;T, TState&gt;, Func&lt;TState, GCHandleType, GCHandle>&gt;)</summary>

  Creates a new `ValueRegion<T>` instance from a `ReadOnlySpanFunc<T, TState>` function and a GC-allocatable instance.
  </details>

</details>

<details>
  <summary>BufferTypeMetadata</summary>

Represents the metadata of a managed buffer type.

**Note:** Implements `IEnumerableSequence<BufferTypeMetadata>`. This type does not allow public inheritance.

### Properties:

- <details>
  <summary>IsBinary</summary>
  Indicates whether current type is binary space.
  </details>
- <details>
  <summary>Size</summary>
  Buffer capacity.
  </details>
- <details>
  <summary>IsBinary</summary>
  Number of components.
  </details>
- <details>
  <summary>BufferType</summary>
  Buffer type.
  </details>

### Indexer:

- <details>
  <summary>Item</summary>
  Retrieves a component from current metadata at the specified zero-based index.
  </details>

### Generic class

`BufferTypeMetadata<T>` representing a generic `BufferTypeMetadata` object.

**Note:** Inherits from `BufferTypeMetadata`. This type does not allow public inheritance.

</details>

<details>
  <summary>CString</summary>

Represents a sequence of UTF-8 encoded characters.

**Notes:**

- Implements `ICloneable`, `IComparable`, `IComparable<CString>`, `IComparable<String>`, `IEquatable<CString>`,
  `IEquatable<String>` and `IEnumerableSequence<Byte>`.
- This type is sealed.
- Instances of this type can be fixed but not pinned.
- Instances of this type can be iterated using both `IEnumerable<Byte>` and `ReadOnlySpan<Byte>.Enumerator`.
- Range operations can be used on instances of this type.
- This type exposes APIs to Join, Concat and Compare `CString` instances.
- This type exposes operators of comparison and equality of `CString` and `String` instances.
- Starting with .NET 9.0, `params` is used with `ReadOnlySpan<>` arguments instead of `[]` arguments.
- Starting with .NET 7.0, this type supports source-generated P/Invoke marshalling.
- When marshalling, the instances of this type are represented as null-terminated UTF-8 strings.
- Starting with .NET Core 3.0, the nested `JsonConverter` class can be used to support serialization and
  deserialization with `System.Text.Json`.

### Static Fields:

- <details>
  <summary>Empty</summary>
  Represents an empty UTF-8 string. This field is read-only.
  </details>
- <details>
  <summary>Zero</summary>
  Represents a null-pointer UTF-8 string. This field is read-only.
  </details>

### Properties:

- <details>
  <summary>IsNullTerminated</summary>

  Gets a value indicating whether the text in the current `CString` instance ends with a null-termination character.
  </details>
- <details>
  <summary>IsReference</summary>

  Gets a value indicating whether the UTF-8 text is referenced by, and not contained within, the current `CString`
  instance.
  </details>
- <details>
  <summary>IsSegmented</summary>

  Gets a value indicating whether the current `CString` instance is a segment (or slice) of another `CString` instance.
  </details>
- <details>
  <summary>IsFunction</summary>

  Gets a value indicating whether the current `CString` instance is a function.
  </details>
- <details>
  <summary>IsZero</summary>

  Gets a value indicating whether the current `CString` instance references to the null UTF-8 text.
  </details>

### Indexer:

- <details>
  <summary>Item</summary>

  Gets the `Byte` value at a specified position in the current `CString` object.
  </details>

### Constructors:

- <details>
  <summary>CString(Byte, Int32)</summary>

  Initializes a new instance of the `CString` class to the value indicated by a specified UTF-8 character repeated a
  specified number of times.
  </details>
- <details>
  <summary>CString(Byte, Byte, Int32)</summary>

  Initializes a new instance of the `CString` class to the value indicated by a specified UTF-8 sequence repeated a
  specified number of times.
  </details>
- <details>
  <summary>CString(Byte, Byte, Byte, Int32)</summary>

  Initializes a new instance of the `CString` class to the value indicated by a specified UTF-8 sequence repeated a
  specified number of times.
  </details>
- <details>
  <summary>CString(Byte, Byte, Byte, Byte, Int32)</summary>

  Initializes a new instance of the `CString` class to the value indicated by a specified UTF-8 sequence repeated a
  specified number of times.
  </details>
- <details>
  <summary>CString(ReadOnlySpan&lt;Byte&gt;)</summary>

  Initializes a new instance of the `CString` class using the UTF-8 characters indicated in the specified read-only
  span.
  </details>
- <details>
  <summary>CString(ReadOnlySpanFunc&lt;Byte&gt;)</summary>

  Initializes a new instance of the `CString` class that contains the UTF-8 string returned by the specified
  `ReadOnlySpanFunc<Byte>`.
  </details>

### Methods:

- <details>
  <summary>ToArray()</summary>

  Copies the UTF-8 text of the current `CString` instance into a new byte array.
  </details>
- <details>
  <summary>AsSpan()</summary>

  Retrieves the UTF-8 units of the current `CString` as a read-only span of bytes.
  </details>
- <details>
  <summary>ToHexString()</summary>

  Returns a `String` that represents the current UTF-8 text as a hexadecimal value.
  </details>
- <details>
  <summary>TryPin()</summary>

  Tries to pin the current UTF-8 memory block.
  </details>

### Operators:

- <details>
  <summary>CString?(Byte[]?)</summary>

  Defines an implicit conversion of a given `Byte` array to `CString`.
  </details>
- <details>
  <summary>CString?(String?)</summary>

  Defines an explicit conversion of a given `String` to `CString`.
  </details>
- <details>
  <summary>ReadOnlySpan&lt;Byte&gt;(CString?)</summary>

  Defines an implicit conversion of a given `CString` to a read-only span of bytes.
  </details>

### Static Methods:

- <details>
  <summary>IsNullOrEmpty(CString?)</summary>

  Determines whether the specified `CString` is `null` or an empty UTF-8 string.
  </details>
- <details>
  <summary>Create(ReadOnlySpan&lt;Byte&gt;)</summary>

  Creates a new instance of the `CString` class using the UTF-8 characters provided in the specified read-only span.
  </details>
- <details>
  <summary>Create(ReadOnlySpanFunc&lt;Byte&gt;?)</summary>

  Creates a new instance of the `CString` class using the `ReadOnlySpanFunc<Byte>` delegate provided.
  </details>
- <details>
  <summary>Create(Byte[]?)</summary>

  Creates a new instance of the `CString` class using the binary internal information provided.
  </details>
- <details>
  <summary>CreateCreate&lt;TState&gt;(TState)</summary>

  Creates a new instance of the `CString` class using a `TState` instance.

  **Note:** This method is available only on .NET 6.0 and later.
  </details>
- <details>
  <summary>CreateCreate&lt;TState&gt;(TState, ReadOnlySpanFunc&lt;Byte, TState&gt;, Func&lt;TState, GCHandleType, GCHandle&gt;)</summary>

  Creates a new instance of the `CString` class using a `TState` instance.
  </details>
- <details>
  <summary>CreateUnsafe(IntPtr, Int32, useFullLength)</summary>

  Creates a new instance of the `CString` class using the pointer to a UTF-8 character array and length provided.
  </details>
- <details>
  <summary>CreateNullTerminatedUnsafe(IntPtr)</summary>

  Creates a new instance of the `CString` class using the pointer to a UTF-8 character null-terminated array.
  </details>

</details>

<details>
  <summary>CStringSequence</summary>

Represents a sequence of null-terminated UTF-8 text strings.

**Notes:**

- Implements `ICloneable`, `IEquatable<CString>`, `IComparable<CStringSequence>`, `IReadOnlyList<CString>` and
  `IEnumerableSequence<CString>`.
- This type is sealed.
- The instances of this type can be fixed but not pinned.
- Range operations can be used on instances of this type.
- This type exposes constructors to create sequences from up to 8 instances of `ReadOnlySpan<Byte>` in order to optimize
  memory usage.
- Starting with .NET 9.0, `params` is used with `ReadOnlySpan<>` arguments instead of `[]` arguments.
- Starting with .NET 7.0, this type supports source-generated P/Invoke marshalling.
- When marshalling, instances of this type are represented as null-terminated arrays of null-terminated UTF-8
  strings. Therefore, only non-empty items will be included.
- Starting with .NET Core 3.0, the nested `JsonConverter` class can be used to support serialization and
  deserialization with `System.Text.Json`.
- The nested `ref struct` `Utf8View` allows iteration over UTF-8 items represented as `ReadOnlySpan<byte>`, providing
- control over whether empty elements are included during enumeration.
- When marshalling in .NET 7.0 and later, `Utf8View` instances are represented as arrays of null-terminated UTF-8
  strings. Empty elements are omitted only if they were not included in the enumeration view.

### Static Properties:

- <details>
  <summary>Empty</summary>
  Represents an empty sequence.
  </details>

### Properties:

- <details>
  <summary>Count</summary>

  Gets the number of `CString` instances contained in this `CStringSequence`.
  </details>
- <details>
  <summary>NonEmptyCount</summary>

  Gets the number of non-empty `CString` instances contained in the buffer of this `CStringSequence`.
  </details>

### Indexer:

- <details>
  <summary>Item</summary>

  Gets the `CString` at the specified index.
  </details>

### Constructors:

- <details>
  <summary>CStringSequence(params String?[])</summary>

  Initializes a new instance of the `CStringSequence` class from a collection of strings.
  </details>
- <details>
  <summary>CStringSequence(params CString?[])</summary>

  Initializes a new instance of the `CStringSequence` class from a collection of UTF-8 strings.
  </details>
- <details>
  <summary>CStringSequence(ReadOnlySpan&lt;CString&gt;)</summary>

  Initializes a new instance of the `CStringSequence` class from a read-only span of UTF-8 strings.
  </details>
- <details>
  <summary>CStringSequence(ReadOnlySpan&lt;String&gt;)</summary>

  Initializes a new instance of the `CStringSequence` class from a read-only span of strings.
  </details>
- <details>
  <summary>CStringSequence(IReadOnlySpan&lt;String?&gt;)</summary>

  Initializes a new instance of the `CStringSequence` class from a collection of strings.
  </details>
- <details>
  <summary>CStringSequence(IReadOnlySpan&lt;CString?&gt;)</summary>

  Initializes a new instance of the `CStringSequence` class from a collection of UTF-8 strings.
  </details>

### Methods:

- <details>
  <summary>ToCString()</summary>

  Returns a `CString` that represents the current sequence.
  </details>
- <details>
  <summary>GetFixedPointer()</summary>

  Creates an `IFixedPointer.IDisposable` instance by pinning the current instance, allowing safe access to the fixed
  memory region.
  </details>
- <details>
  <summary>WithSafeTransform(CStringSequenceAction)</summary>

  Executes a specified action using the current instance treated as a `FixedCStringSequence`.
  </details>
- <details>
  <summary>WithSafeTransform&lt;TState&gt;(TState, CStringSequenceAction)</summary>

  Executes a specified action on the current instance treated as a `FixedCStringSequence`, using an additional parameter
  passed to the action.
  </details>
- <details>
  <summary>WithSafeTransform&lt;TResult&gt;(CStringSequenceFunc&lt;TResult&gt;)</summary>

  Executes a specified function using the current instance treated as a `FixedCStringSequence`.
  </details>
- <details>
  <summary>WithSafeTransform&lt;TState, TResult&gt;(TState, CStringSequenceFunc&lt;TResult&gt;)</summary>

  Executes a specified function using the current instance treated as a `FixedCStringSequence`, and an additional
  parameter passed to the function.
  </details>
- <details>
  <summary>GetOffsets(Span&lt;Int32&gt;)</summary>

  Fills the provided span with the starting byte offsets of each UTF-8 encoded `CString' segment within the current
  buffer.
  </details>

### Static Methods:

- <details>
  <summary>Create&lt;TState&gt;(TState, CStringSequenceCreationAction&lt;TState&gt;, params Int32?[])</summary>
  Creates a new UTF-8 text sequence with specific lengths, and initializes each UTF-8 text string in it after creation using the specified callback.
  </details>
- <details>
  <summary>Create(ReadOnlySpan&lt;Byte&gt;)</summary>

  Creates a new `CStringSequence` instance from a UTF-8 buffer.
  </details>
- <details>
  <summary>Create(ReadOnlySpan&lt;Char&gt;)</summary>

  Creates a new `CStringSequence` instance from a UTF-8 buffer represented by a span of `Char`.
  </details>
- <details>
  <summary>GetUnsafe(ReadOnlySpan&lt;ReadOnlyValPtr&lt;Byte&gt;&gt;)</summary>

  Creates a new `CStringSequence` instance from a UTF-8 null-terminated text pointer span.
  </details>
- <details>
  <summary>Parse(String?)</summary>

  Converts the buffer of a UTF-8 sequence to a `CStringSequence` instance.
  </details>

</details>

---

# Extensions

`Rxmxnx.PInvoke.Extensions` provides static classes that expose APIs to facilitate the management of managed memory,
pointer usage, and this package's internal resources.

<details>
  <summary>BinaryExtensions</summary>

Set of useful methods when working with bytes, byte arrays, and byte spans in a PInvoke context.

- <details>
  <summary>ToValue&lt;T&gt;(this Byte[])</summary>

  Retrieves a `T` value from the given byte array.

  **Note:** ´T´ is ´unmanaged´.
  </details>
- <details>
  <summary>ToValue&lt;T&gt;(this Span&lt;Byte&gt;)</summary>

  Retrieves a `T` value from the given byte span.

  **Note:** ´T´ is ´unmanaged´.
  </details>
- <details>
  <summary>ToValue&lt;T&gt;(this ReadOnlySpan&lt;Byte&gt;)</summary>

  Retrieves a `T` value from the given read-only byte span.

  **Note:** ´T´ is ´unmanaged´.
  </details>
- <details>
  <summary>AsValue&lt;T&gt;(this ReadOnlySpan&lt;Byte&gt;)</summary>

  Retrieves a read-only reference to a `T` value from the given read-only byte span.

  **Note:** ´T´ is ´unmanaged´.
  </details>
- <details>
  <summary>AsValue&lt;T&gt;(this Span&lt;Byte&gt;)</summary>

  Retrieves a reference to a `T` value from the given byte span.

  **Note:** ´T´ is ´unmanaged´.
  </details>
- <details>
  <summary>AsHexString(this Byte[])</summary>
  Gets the hexadecimal string representation of a byte array.
  </details>
- <details>
  <summary>AsHexString(this Byte)</summary>
  Gets the hexadecimal string representation of a byte.
  </details>
- <details>
  <summary>WithSafeFixed(this Span&lt;Byte&gt;, FixedAction)</summary>
  Prevents the garbage collector from relocating the current span by pinning its memory address until the specified action has completed.
  </details>
- <details>
  <summary>WithSafeFixed(this Span&lt;Byte&gt;, ReadOnlyFixedAction)</summary>
  Prevents the garbage collector from relocating the current span by pinning its memory address until the specified action has completed.
  </details>
- <details>
  <summary>WithSafeFixed(this ReadOnlySpan&lt;Byte&gt;, ReadOnlyFixedAction)</summary>
  Prevents the garbage collector from relocating the current read-only span by pinning its memory address until the specified action has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TArg&gt;(this Span&lt;Byte&gt;, FixedAction&lt;TArg&gt;)</summary>
  Prevents the garbage collector from relocating the current span by pinning its memory address until the specified action has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TArg&gt;(this Span&lt;Byte&gt;, ReadOnlyFixedAction&lt;TArg&gt;)</summary>
  Prevents the garbage collector from relocating the current span by pinning its memory address until the specified action has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TArg&gt;(this ReadOnlySpan&lt;Byte&gt;, ReadOnlyFixedAction&lt;TArg&gt;)</summary>
  Prevents the garbage collector from relocating the current read-only span by pinning its memory address until the specified action has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TResult&gt;(this Span&lt;Byte&gt;, FixedFunc&lt;TResult&gt;)</summary>
  Prevents the garbage collector from relocating the current span by pinning its memory address until the specified function has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TResult&gt;(this Span&lt;Byte&gt;, ReadOnlyFixedFunc&lt;TResult&gt;)</summary>
  Prevents the garbage collector from relocating the current span by pinning its memory address until the specified function has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TResult&gt;(this ReadOnlySpan&lt;Byte&gt;, ReadOnlyFixedFunc&lt;TResult&gt;)</summary>
  Prevents the garbage collector from relocating the current read-only span by pinning its memory address until the specified function has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TArg, TResult&gt;(this Span&lt;Byte&gt;, TArg, FixedFunc&lt;TArg, TResult&gt;)</summary>
  Prevents the garbage collector from relocating the current span by pinning its memory address until the specified function has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TArg, TResult&gt;(this Span&lt;Byte&gt;, TArg, ReadOnlyFixedFunc&lt;TArg, TResult&gt;)</summary>
  Prevents the garbage collector from relocating the current span by pinning its memory address until the specified function has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TArg, TResult&gt;(this ReadOnlySpan&lt;Byte&gt;, TArg, ReadOnlyFixedFunc&lt;TArg, TResult&gt;)</summary>
  Prevents the garbage collector from relocating the current read-only span by pinning its memory address until the specified function has completed.
  </details>

</details>

<details>
  <summary>DelegateExtensions</summary>

Additional functionality for working with delegates.

- <details>
  <summary>GetUnsafeFuncPtr&lt;TDelegate&gt;(this TDelegate)</summary>

  Creates a `FuncPtr<TDelegate>` from a memory reference to a `TDelegate` delegate instance.
  </details>
- <details>
  <summary>GetUnsafeIntPtr&lt;TDelegate&gt;(this TDelegate)</summary>

  Retrieves an `IntPtr` from a memory reference to a `TDelegate` delegate instance.
  </details>
- <details>
  <summary>GetUnsafeUIntPtr&lt;TDelegate&gt;(this TDelegate)</summary>

  Retrieves a `UIntPtr` from a memory reference to a `TDelegate` delegate instance.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TDelegate&gt;(this TDelegate, FixedMethodAction&lt;TDelegate&gt;)</summary>
  Prevents the garbage collector from relocating a delegate in memory and fixes its address while an action is being performed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TDelegate, TArg&gt;(this TDelegate, TArg, FixedMethodAction&lt;TDelegate, TArg&gt;)</summary>
  Prevents the garbage collector from relocating a delegate in memory and fixes its address while an action is being performed, passing an additional 
  argument to the action.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TDelegate, TResult&gt;(this TDelegate, TArg, FixedMethodFunc&lt;TDelegate, TResult&gt;)</summary>

  Prevents the garbage collector from relocating a delegate in memory, fixes its address, and invokes the function that
  returns a `TResult` value.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TDelegate, TArg, TResult&gt;(this TDelegate, TArg, FixedMethodFunc&lt;TDelegate, TArg, TResult&gt;)</summary>

  Prevents the garbage collector from relocating a delegate in memory, fixes its address, invokes the function that
  returns a `TResult` value, passing an additional argument to the function.
  </details>
- <details>
  <summary>GetFixedMethod&lt;TDelegate&gt;(this TDelegate?)</summary>

  Creates an `IFixedMethod<TDelegate>.IDisposable` instance by marshalling the current `TDelegate` instance,
  ensuring a safe interop context.
  </details>

</details>

<details>
  <summary>MemoryBlockExtensions</summary>

Additional functionality for working with memory blocks.

- <details>
  <summary>IsLiteral&lt;T&gt;(this Span&lt;T&gt;)</summary>

  Indicates whether the given `Span<T>` instance represents a literal or hardcoded memory region.

  **Note:** This function is currently available only on Windows, Linux, macOS and FreeBSD.
  </details>
- <details>
  <summary>IsLiteral&lt;T&gt;(this ReadOnlySpan&lt;T&gt;)</summary>

  Indicates whether the given `ReadOnlySpan<T>` instance represents a literal or hardcoded memory region.

  **Note:** This function is currently available only on Windows, Linux, macOS and FreeBSD.
  </details>
- <details>
  <summary>MayBeNonLiteral&lt;T&gt;(this ReadOnlySpan&lt;T&gt;)</summary>

  Indicates whether the given `ReadOnlySpan<T>` instance represents memory that is not part of a hardcoded literal.

  **Note:** If the platform is unsupported or an inspection error occurs, the method returns true.
  </details>

- <details>
  <summary>GetUnsafeValPtr&lt;T&gt;(this Span&lt;T&gt;)</summary>

  Retrieves an unsafe `ValPtr<T>` pointer from `Span<T>` instance.
  </details>
- <details>
  <summary>GetUnsafeValPtr&lt;T&gt;(this ReadOnlySpan&lt;T&gt;)</summary>

  Retrieves an unsafe `ValPtr<T>` pointer from `ReadOnlySpan<T>` instance.
  </details>
- <details>
  <summary>GetUnsafePtr&lt;T&gt;(this Span&lt;T&gt;)</summary>

  Retrieves an unsafe `IntPtr` pointer from `Span<T>` instance.
  </details>
- <details>
  <summary>GetUnsafePtr&lt;T&gt;(this ReadOnlySpan&lt;T&gt;)</summary>

  Retrieves an unsafe `IntPtr` pointer from `ReadOnlySpan<T>` instance.
  </details>
- <details>
  <summary>GetUnsafeUIntPtr&lt;T&gt;(this Span&lt;T&gt;)</summary>

  Retrieves an unsafe `UIntPtr` pointer from `Span<T>` instance.
  </details>
- <details>
  <summary>GetUnsafeUIntPtr&lt;T&gt;(this ReadOnlySpan&lt;T&gt;)</summary>

  Retrieves an unsafe `UIntPtr` pointer from `ReadOnlySpan<T>` instance.
  </details>
- <details>
  <summary>AsBytes&lt;TSource&gt;(this Span&lt;TSource&gt;)</summary>

  Reinterprets the span of `TSource` as a binary span.

  **Note:** `TSource` is `unmanaged`.
  </details>
- <details>
  <summary>AsBytes&lt;TSource&gt;(this ReadOnlySpan&lt;TSource&gt;)</summary>

  Reinterprets the read-only span of `TSource` as a read-only binary span.

  **Note:** `TSource` is `unmanaged`.
  </details>
- <details>
  <summary>AsValues&lt;TSource, TDestination&gt;(this Span&lt;TSource&gt;)</summary>

  Reinterprets the span of `TSource` as a span of `TDestination`.

  **Note:** `TSource` is `unmanaged`. `TDestination` is `unmanaged`.
  </details>
- <details>
  <summary>AsValues&lt;TSource, TDestination&gt;(this ReadOnlySpan&lt;TSource&gt;)</summary>

  Reinterprets the read-only span of `TSource` as a read-only span of `TDestination`.

  **Note:** `TSource` is `unmanaged`. `TDestination` is `unmanaged`.
  </details>
- <details>
  <summary>AsValues&lt;TSource, TDestination&gt;(this Span&lt;TSource&gt;, Span&lt;Byte&gt;)</summary>

  Reinterprets the span of `TSource` as a span of `TDestination`.

  **Note:** `TSource` is `unmanaged`. `TDestination` is `unmanaged`.
  </details>
- <details>
  <summary>AsValues&lt;TSource, TDestination&gt;(this Span&lt;TSource&gt;, ReadOnlySpan&lt;Byte&gt;)</summary>

  Reinterprets the span of `TSource` as a read-only span of `TDestination`.

  **Note:** `TSource` is `unmanaged`. `TDestination` is `unmanaged`.
  </details>
- <details>
  <summary>AsValues&lt;TSource, TDestination&gt;(this ReadOnlySpan&lt;TSource&gt;, ReadOnlySpan&lt;Byte&gt;)</summary>

  Reinterprets the read-only span of `TSource` as a read-only span of `TDestination`.

  **Note:** `TSource` is `unmanaged`. `TDestination` is `unmanaged`.
  </details>
- <details>
  <summary>GetFixedContext&lt;T&gt;(this ReadOnlyMemory&lt;T&gt;)</summary>

  Creates an `IReadOnlyFixedContext<T>.IDisposable` instance by pinning the current `ReadOnlyMemory<T>` instance,
  ensuring a safe context for accessing the fixed memory.

  **Note:** `T` is `unmanaged`.
  </details>
- <details>
  <summary>GetFixedContext&lt;T&gt;(this Memory&lt;T&gt;)</summary>

  Creates an `IFixedContext<T>.IDisposable` instance by pinning the current `Memory<T>` instance, ensuring a safe
  context for accessing the fixed memory.

  **Note:** `T` is `unmanaged`.
  </details>
- <details>
  <summary>GetFixedMemory&lt;T&gt;(this ReadOnlyMemory&lt;T&gt;)</summary>

  Creates an `IReadOnlyFixedMemory<T>.IDisposable` instance by pinning the current `ReadOnlyMemory<T>` instance,
  ensuring a safe context for accessing the fixed memory.

  **Note:** `T` is `unmanaged`.
  </details>
- <details>
  <summary>GetFixedMemory&lt;T&gt;(this Memory&lt;T&gt;)</summary>

  Creates an `IFixedMemory<T>.IDisposable` instance by pinning the current `Memory<T>` instance, ensuring a safe context
  for accessing the fixed memory.

  **Note:** `T` is `unmanaged`.
  </details>
- <details>
  <summary>AsMemory&lt;T&gt;(this T[...])</summary>
  Creates a new memory region over the target array.

  **Note:** In .NET 5.0 and earlier, this method uses reflection.
  </details>
- <details>
  <summary>AsSpan&lt;T&gt;(this T[...])</summary>
  Creates a new span over a target array.

  **Note:** In .NET 5.0 and earlier, this method uses reflection.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T&gt;(this Span&lt;T&gt;, FixedContextAction&lt;T&gt;)</summary>
  Prevents the garbage collector from relocating the current span by pinning its memory address until the specified action has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T&gt;(this Span&lt;T&gt;, ReadOnlyFixedContextAction&lt;T&gt;)</summary>
  Prevents the garbage collector from relocating the current span by pinning its memory address until the specified action has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T&gt;(this ReadOnlySpan&lt;T&gt;, ReadOnlyFixedContextAction&lt;T&gt;)</summary>
  Prevents the garbage collector from relocating the current read-only span by pinning its memory address until the specified action has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg&gt;(this Span&lt;T&gt;, FixedContextAction&lt;T, TArg&gt;)</summary>
  Prevents the garbage collector from relocating the current span by pinning its memory address until the specified action has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg&gt;(this Span&lt;T&gt;, ReadOnlyFixedContextAction&lt;T, TArg&gt;)</summary>
  Prevents the garbage collector from relocating the current span by pinning its memory address until the specified action has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg&gt;(this ReadOnlySpan&lt;T&gt;, ReadOnlyFixedContextAction&lt;T, TArg&gt;)</summary>
  Prevents the garbage collector from relocating the current read-only span by pinning its memory address until the specified action has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TResult&gt;(this Span&lt;T&gt;, FixedContextFunc&lt;T, TResult&gt;)</summary>
  Prevents the garbage collector from relocating the current span by pinning its memory address until the specified function has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TResult&gt;(this Span&lt;T&gt;, ReadOnlyFixedContextFunc&lt;T, TResult&gt;)</summary>
  Prevents the garbage collector from relocating the current span by pinning its memory address until the specified function has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TResult&gt;(this ReadOnlySpan&lt;T&gt;, ReadOnlyFixedContextFunc&lt;T, TResult&gt;)</summary>
  Prevents the garbage collector from relocating the current read-only span by pinning its memory address until the specified function has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg, TResult&gt;(this Span&lt;T&gt;, TArg, FixedContextFunc&lt;T, TArg, TResult&gt;)</summary>
  Prevents the garbage collector from relocating the current span by pinning its memory address until the specified function has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg, TResult&gt;(this Span&lt;T&gt;, TArg, ReadOnlyFixedContextFunc&lt;T, TArg, TResult&gt;)</summary>
  Prevents the garbage collector from relocating the current span by pinning its memory address until the specified function has completed.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg, TResult&gt;(this ReadOnlySpan&lt;T&gt;, TArg, ReadOnlyFixedContextFunc&lt;T, TArg, TResult&gt;)</summary>
  Prevents the garbage collector from relocating the current read-only span by pinning its memory address until the specified function has completed.
  </details>

</details>

<details>
  <summary>PointerExtensions</summary>

Set of extensions for basic operations with `IntPtr` and `UIntPtr` instances.

- <details>
  <summary>IsZero(this IntPtr)</summary>

  Determines if the `IntPtr` instance is zero.
  </details>
- <details>
  <summary>IsZero(this UIntPtr)</summary>

  Determines if the `UIntPtr` instance is zero.
  </details>
- <details>
  <summary>ToUIntPtr(this IntPtr)</summary>

  Converts the specified `IntPtr` instance to a `UIntPtr` instance.
  </details>
- <details>
  <summary>ToUIntPtr(this MemoryHandle)</summary>
  Converts the specified `MemoryHandle` instance to a `UIntPtr` instance.
  </details>
- <details>
  <summary>ToIntPtr(this UIntPtr)</summary>

  Converts the specified `UIntPtr` instance to a `IntPtr` instance.
  </details>
- <details>
  <summary>ToIntPtr(this MemoryHandle)</summary>

  Converts the specified `MemoryHandle` instance to a `IntPtr` instance.
  </details>
- <details>
  <summary>GetUnsafeString(this IntPtr)</summary>

  Generates a `String` instance from the memory at the given `IntPtr`, interpreting the contents as UTF-16 text.
  </details>
- <details>
  <summary>GetUnsafeString(this UIntPtr)</summary>

  Generates a `String` instance from the memory at the given `UIntPtr`, interpreting the contents as UTF-16 text.
  </details>
- <details>
  <summary>GetUnsafeString(this MemoryHandle)</summary>

  Generates a `String` instance from the memory at the given `MemoryHandle`, interpreting the contents as UTF-16 text.
  </details>
- <details>
  <summary>GetUnsafeString(this IntPtr, Int32)</summary>

  Generates a `String` instance from the memory at the given `IntPtr`, interpreting the contents as UTF-16 text.
  </details>
- <details>
  <summary>GetUnsafeString(this UIntPtr, Int32)</summary>

  Generates a `String` instance from the memory at the given `UIntPtr`, interpreting the contents as UTF-16 text.
  </details>
- <details>
  <summary>GetUnsafeString(this MemoryHandle, Int32)</summary>

  Generates a `String` instance from the memory at the given `MemoryHandle`, interpreting the contents as UTF-16 text.
  </details>
- <details>
  <summary>GetUnsafeArray&lt;T&gt;(this IntPtr, Int32)</summary>

  Generates a `T` array by copying values from memory starting at the location referenced by an `IntPtr`.

  **Note:** ´T´ is ´unmanaged´.
  </details>
- <details>
  <summary>GetUnsafeArray&lt;T&gt;(this UIntPtr, Int32)</summary>

  Generates a `T` array by copying values from memory starting at the location referenced by a `UIntPtr`.

  **Note:** ´T´ is ´unmanaged´.
  </details>
- <details>
  <summary>GetUnsafeArray&lt;T&gt;(this MemoryHandle, Int32)</summary>

  Generates a `T` array by copying values from memory starting at the location referenced by a `MemoryHandle`.

  **Note:** ´T´ is ´unmanaged´.
  </details>
- <details>
  <summary>GetUnsafeSpan&lt;T&gt;(this IntPtr, Int32)</summary>

  Generates a `Span<T>` instance from an `IntPtr`, interpreting the memory at the specified location as a sequence of
  `unmanaged` values.

  **Note:** ´T´ is ´unmanaged´.
  </details>
- <details>
  <summary>GetUnsafeSpan&lt;T&gt;(this UIntPtr, Int32)</summary>

  Generates a `Span<T>` instance from a `UIntPtr`, interpreting the memory at the specified location as a sequence of
  `unmanaged` values.

  **Note:** ´T´ is ´unmanaged´.
  </details>
- <details>
  <summary>GetUnsafeSpan&lt;T&gt;(this MemoryHandle, Int32)</summary>

  Generates a `Span<T>` instance from a `MemoryHandle`, interpreting the memory at the specified location as a sequence
  of `unmanaged` values.

  **Note:** ´T´ is ´unmanaged´.
  </details>
- <details>
  <summary>GetUnsafeReadOnlySpan&lt;T&gt;(this IntPtr, Int32)</summary>

  Generates a `ReadOnlySpan<T>` instance from an `IntPtr`, interpreting the memory at the specified location as a
  sequence of `unmanaged` values.

  **Note:** ´T´ is ´unmanaged´.
  </details>
- <details>
  <summary>GetUnsafeReadOnlySpan&lt;T&gt;(this UIntPtr, Int32)</summary>

  Generates a `ReadOnlySpan<T>` instance from a `UIntPtr`, interpreting the memory at the specified location as a
  sequence of `unmanaged` values.

  **Note:** ´T´ is ´unmanaged´.
  </details>
- <details>
  <summary>GetUnsafeReadOnlySpan&lt;T&gt;(this MemoryHandle, Int32)</summary>

  Generates a `ReadOnlySpan<T>` instance from a `MemoryHandle`, interpreting the memory at the specified location as a
  sequence of `unmanaged` values.

  **Note:** ´T´ is ´unmanaged´.
  </details>
- <details>
  <summary>GetUnsafeDelegate&lt;TDelegate&gt;(this IntPtr)</summary>

  Generates a delegate of type `TDelegate` from an `IntPtr`.
  </details>
- <details>
  <summary>GetUnsafeDelegate&lt;TDelegate&gt;(this UIntPtr)</summary>

  Creates a delegate of type `TDelegate` from a `UIntPtr`.
  </details>
- <details>
  <summary>GetUnsafeReference&lt;T&gt;(this IntPtr)</summary>

  Generates a memory reference to an `unmanaged` value of type `T` from `IntPtr`.
  </details>
- <details>
  <summary>GetUnsafeReference&lt;T&gt;(this UIntPtr)</summary>

  Generates a memory reference to an `unmanaged` value of type `T` from `UIntPtr`.
  </details>
- <details>
  <summary>GetUnsafeReadOnlyReference&lt;T&gt;(this IntPtr)</summary>

  Generates a read-only memory reference to an `unmanaged` value of type `T` from `IntPtr`.
  </details>
- <details>
  <summary>GetUnsafeReadOnlyReference&lt;T&gt;(this UIntPtr)</summary>

  Generates a read-only memory reference to an `unmanaged` value of type `T` from `UIntPtr`.
  </details>
- <details>
  <summary>GetUnsafeReadOnlySpanFromNullTerminated(this ReadOnlyValPtr&lt;Char&gt;)</summary>

  Generates a read-only span for a UTF-16 null-terminated string from `ReadOnlyValPtr<Char>`.
  </details>
- <details>
  <summary>GetUnsafeReadOnlySpanFromNullTerminated(this ReadOnlyValPtr&lt;Byte&gt;)</summary>

  Generates a read-only span for a UTF-8 null-terminated string from `ReadOnlyValPtr<Byte>`.
  </details>

</details>

<details>
  <summary>PointerCStringExtensions</summary>

Set of extensions for `CString` operations with `IntPtr` and `UIntPtr` instances.

- <details>
  <summary>GetUnsafeCString(this IntPtr, Int32)</summary>

  Generates a `CString` instance using the memory reference pointed to by the given `IntPtr`, considering it as the
  start of a UTF-8 encoded string.
  </details>
- <details>
  <summary>GetUnsafeCString(this UIntPtr, Int32)</summary>

  Generates a `CString` instance using the memory reference pointed to by the given `UIntPtr`, considering it as the
  start of a UTF-8 encoded string.
  </details>
- <details>
  <summary>GetUnsafeCString(this MemoryHandle, Int32)</summary>

  Generates a `CString` instance using the memory reference pointed to by the given `MemoryHandle`, considering it as
  the start of a UTF-8 encoded string.
  </details>

</details>


<details>
  <summary>Utf8ViewExtensions</summary>

Set of extensions for `CStringSequence` viewing operations.

- <details>
  <summary>CreateView(this CStringSequence?, Boolean)</summary>

  Creates a new `CStringSequence.Utf8View` from the given `CStringSequence`instance, with an additional parameter
  to control the inclusion of empty items in the resulting enumeration.

  **Note:** This extension method can be safely called on null instances of `CStringSequence`; it does not throw a
  `NullReferenceException`.
  </details>

</details>

<details>
  <summary>ReferenceExtensions</summary>

Set of extensions for basic operations with references to `unmanaged` values.

- <details>
  <summary>GetUnsafeValPtr&lt;T&gt;(ref this T)</summary>

  Obtains an unsafe pointer of type `ValPtr<T>` from a reference to an `unmanaged` value of type `T`.
  </details>
- <details>
  <summary>GetUnsafeIntPtr&lt;T&gt;(ref this T)</summary>

  Obtains an unsafe pointer of type `IntPtr` from a reference to an `unmanaged` value of type `T`.
  </details>
- <details>
  <summary>GetUnsafeUIntPtr&lt;T&gt;(ref this T)</summary>

  Obtains an unsafe pointer of type `UIntPtr` from a reference to an `unmanaged` value of type `T`.
  </details>
- <details>
  <summary>Transform&lt;TSource, TDestination&gt;(ref this TSource)</summary>

  Generates a reference for an `unmanaged` value of type `TDestination` from an existing reference to an `unmanaged`
  value
  of type `TSource`.
  </details>
- <details>
  <summary>AsBytes&lt;TSource&gt;(ref this TSource)</summary>

  Creates a `Span<Byte>` from a reference to an `unmanaged` value of type `TSource`.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T&gt;(ref this T, FixedReferenceAction&lt;T&gt;)</summary>
  Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a provided action.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T&gt;(ref this T, ReadOnlyFixedReferenceAction&lt;T&gt;)</summary>
  Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a provided read-only action.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg&gt;(ref this T, TArg, FixedReferenceAction&lt;T, TArg&gt;)</summary>
  Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a provided action along with an argument.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg&gt;(ref this T, TArg, ReadOnlyFixedReferenceAction&lt;T, TArg&gt;)</summary>
  Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a provided read-only action along with an argument.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TResult&gt;(ref this T, FixedReferenceFunc&lt;T, TResult&gt;)</summary>
  Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a provided function.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TResult&gt;(ref this T, ReadOnlyFixedReferenceFunc&lt;T, TResult&gt;)</summary>
  Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a provided read-only function.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg, TResult&gt;(ref this T, TArg, FixedReferenceFunc&lt;T, TArg, TResult&gt;)</summary>
  Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a provided function along with an argument.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg, TResult&gt;(ref this T, TArg, ReadOnlyFixedReferenceFunc&lt;T, TArg, TResult&gt;)</summary>
  Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a provided read-only function along with an argument.
  </details>

</details>

<details>
  <summary>StringExtensions</summary>

Set of extensions for basic operations with `String` instances.

- <details>
  <summary>WithSafeFixed(this String?, ReadOnlyFixedContextAction&lt;Char&gt;)</summary>
  Pins the current string to prevent the garbage collector from relocating its memory address during the execution of the specified action.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TArg&gt;(this String?, TArg, ReadOnlyFixedContextAction&lt;Char, TArg&gt;)</summary>
  Pins the current string to prevent the garbage collector from relocating its memory address during the execution of the specified action along with an argument.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TResult&gt;(this String?, ReadOnlyFixedContextFunc&lt;Char, TResult&gt;)</summary>
  Pins the current string to prevent the garbage collector from relocating its memory address during the execution of the specified function.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TArg, TResult&gt;(this String?, TArg, ReadOnlyFixedContextFunc&lt;Char, TArg, TResult&gt;)</summary>
  Pins the current string to prevent the garbage collector from relocating its memory address during the execution of the specified function along with an argument.
  </details>

</details>

<details>
  <summary>UnmanagedValueExtensions</summary>

Set of extensions for basic operations with `unmanaged` values.

- <details>
  <summary>RentFixed&lt;T&gt;(this ArrayPool&lt;T&gt;, Int32, Boolean)</summary>

  Rents and pins an array of minimum number of `T` elements from given array pool,
  ensuring a safe context for accessing the fixed memory.
  </details>
- <details>
  <summary>RentFixed&lt;T&gt;(this ArrayPool&lt;T&gt;, Int32, Boolean, out Int32)</summary>

  Rents and pins an array of minimum number of `T` elements from given array pool,
  ensuring a safe context for accessing the fixed memory.
  </details>
- <details>
  <summary>ToBytes&lt;T&gt;(this T)</summary>

  Converts a given `unmanaged` value of type `T` into an array of `Byte`.
  </details>
- <details>
  <summary>ToBytes&lt;TSource&gt;(this TSource[]?)</summary>

  Converts an array of `unmanaged` values of type `TSource` into an array of `Byte`.
  </details>
- <details>
  <summary>ToValues&lt;TSource, TDestination&gt;(this TSource[]?)</summary>

  Converts an array of `unmanaged` values of type `TSource` into an array of another `unmanaged` value type
  `TDestination`.
  </details>
- <details>
  <summary>ToValues&lt;TSource, TDestination&gt;(this TSource[]?, out Byte[]?)</summary>

  Converts an array of `unmanaged` values of type `TSource` into an array of another `unmanaged` value type
  `TDestination`
  and provides the residual binary array of the reinterpretation.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T&gt;(ref this T, FixedReferenceAction&lt;T&gt;)</summary>
  Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a provided action.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T&gt;(ref this T, ReadOnlyFixedReferenceAction&lt;T&gt;)</summary>
  Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a provided read-only action.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg&gt;(ref this T, TArg, FixedReferenceAction&lt;T, TArg&gt;)</summary>
  Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a provided action along with an argument.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg&gt;(ref this T, TArg, ReadOnlyFixedReferenceAction&lt;T, TArg&gt;)</summary>
  Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a provided read-only action along with an argument.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TResult&gt;(ref this T, FixedReferenceFunc&lt;T, TResult&gt;)</summary>
  Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a provided function.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TResult&gt;(ref this T, ReadOnlyFixedReferenceFunc&lt;T, TResult&gt;)</summary>
  Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a provided read-only function.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg, TResult&gt;(ref this T, TArg, FixedReferenceFunc&lt;T, TArg, TResult&gt;)</summary>
  Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a provided function along with an argument.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg, TResult&gt;(ref this T, TArg, ReadOnlyFixedReferenceFunc&lt;T, TArg, TResult&gt;)</summary>
  Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a provided read-only function along with an argument.
  </details>

</details>

---

# Utilities

`Rxmxnx.PInvoke.Extensions` provides static classes that complement the APIs facilitating the management of read-only
managed memory, enable interaction with the runtime environment, and allow the use of the package's internal resources.

<details>
  <summary>AotInfo</summary>

Provides information about the Ahead-of-Time compilation.

#### Static Properties:

- <details>
  <summary>IsReflectionDisabled</summary>
  Indicates whether runtime reflection is disabled.
  </details>
- <details>
  <summary>IsCodeGenerationSupported</summary>
  Indicates whether the current runtime supports the emission of dynamic IL code.
  </details>
- <details>
  <summary>IsPlatformTrimmed</summary>
  Indicates whether the current runtime is trimmed for the current platform.

  **Note:** Starting with .NET 5.0, this property enables trimming by allowing the linker to remove unreachable code.
  </details>
- <details>
  <summary>IsNativeAot</summary>
  Indicates whether the current runtime is NativeAOT.

  **Note:** Starting with .NET 6.0, this property enables trimming by allowing the linker to remove unreachable code on
  desktop platforms.
  </details>

</details>

<details>
  <summary>SystemInfo</summary>

Provides information about the runtime system.

#### Static Properties:

- <details>
  <summary>IsMonoRuntime</summary>
  Indicates whether the current runtime is Mono.
  </details>
- <details>
  <summary>IsWebRuntime</summary>
  Indicates whether the current runtime is Web.

  **Note:** Starting with .NET 8.0, this property enables trimming by allowing the linker to remove unreachable code.
  </details>
- <details>
  <summary>IsWindows</summary>
  Indicates whether the current runtime is running on a Windows System.

  **Note:** Starting with .NET 5.0, this property enables trimming by allowing the linker to remove unreachable code.
  </details>
- <details>
  <summary>IsLinux</summary>
  Indicates whether the current runtime is running on a Linux System.

  **Note:** Starting with .NET 5.0, this property enables trimming by allowing the linker to remove unreachable code.
  </details>
- <details>
  <summary>IsMac</summary>
  Indicates whether the current runtime is running on a macOS System.

  **Note:** Starting with .NET 6.0, this property enables trimming by allowing the linker to remove unreachable code.
  </details>
- <details>
  <summary>IsFreeBsd</summary>
  Indicates whether the current runtime is running on a FreeBSD System.

  **Note:** Starting with .NET 5.0, this property enables trimming by allowing the linker to remove unreachable code.
  </details>
- <details>
  <summary>IsNetBsd</summary>
  Indicates whether the current runtime is running on a NetBSD System.
  </details>
- <details>
  <summary>IsSolaris</summary>
  Indicates whether the current runtime is running on a Solaris System.
  </details>

#### Static Methods:

- <details>
  <summary>IsOsPlatform(String?)</summary>
  Indicates whether the current runtime is running on the specified platform.
  </details>
- <details>
  <summary>IsOsPlatform(String?[])</summary>
  Indicates whether the current runtime is running on one of the specified platforms.
  </details>
- <details>
  <summary>IsOsPlatform(ReadOnlySpan&lt;String?&gt;)</summary>
  Indicates whether the current runtime is running on one of the specified platforms.
  </details>

**Note:** Starting with .NET 9.0, `params` is used with `ReadOnlySpan<String?>` arguments instead of `String?[]`
arguments.
</details>

<details>
  <summary>BufferManager</summary>

This class allows to allocate buffers on stack if possible.

#### Static Properties:

- <details>
  <summary>BufferAutoCompositionEnabled</summary>
  Indicates whether metadata for any required buffer is auto-composed.

  **Note:** This property will always be false if compiled with `IlcDisableReflection=true` or if the
  `PInvoke.DisableBufferAutoComposition` feature switch is enabled.
  </details>

#### Static Methods:

- <details>
  <summary>Alloc&lt;T&gt;(UInt16, ScopedBufferAction&lt;T&gt;, Boolean)</summary>

  Allocates a buffer with `count` elements and executes `action`.
  </details>
- <details>
  <summary>Alloc&lt;T, TState&gt;(UInt16, TState, ScopedBufferAction&lt;T, TState&gt;, Boolean)</summary>

  Allocates a buffer with `count` elements and executes `action`.
  </details>
- <details>
  <summary>Alloc&lt;T, TResult&gt;(UInt16, ScopedBufferFunc&lt;T, TResult&gt;, Boolean)</summary>

  Allocates a buffer with `count` elements and executes `func`.
  </details>
- <details>
  <summary>Alloc&lt;T, TState, TResult&gt;(UInt16, TState, ScopedBufferFunc&lt;T, TState, TResult&gt;, Boolean)</summary>

  Allocates a buffer with `count` elements and executes `func`.
  </details>
- <details>
  <summary>Register&lt;TBuffer&gt;()</summary>
  Registers object buffer.

  **Note:** `TBuffer` is `struct` and `IManagedBuffer<Object>`. In .NET 6.0 and earlier, this method uses reflection.
  </details>
- <details>
  <summary>Register&lt;T, TBuffer&gt;()</summary>

  Registers `T` buffer.

  **Note:** `T` is `struct`. `TBuffer` is `struct` and `IManagedBuffer<T>`. In .NET 6.0 and earlier, this method uses
  reflection.
  </details>
- <details>
  <summary>RegisterNullable&lt;T, TBuffer&gt;()</summary>

  Registers `T?` buffer.

  **Note:** `T` is `struct`. `TBuffer` is `struct` and `IManagedBuffer<T?>`. In .NET 6.0 and earlier, this method uses
  reflection.
  </details>
- <details>
  <summary>PrepareBinaryBuffer(UInt16)</summary>
  Prepares the binary buffer metadata needed to allocate given number of objects.

  **Note:** Reflection is always used by this method, and the buffer auto-Composition feature must be enabled.
  </details>
- <details>
  <summary>PrepareBinaryBuffer&lt;T&gt;(UInt16)</summary>

  Prepares the binary buffer metadata needed to allocate given number of `T` items.

  **Note:** `T` is `struct`. Reflection is always used by this method, and the buffer auto-Composition feature must
  be enabled.
  </details>
- <details>
  <summary>PrepareBinaryBufferNullable&lt;T&gt;()</summary>

  Prepares the binary buffer metadata needed to allocate given number of `T?` items.

  **Note:** `T` is `struct`. Reflection is always used by this method, and BufferAutoCompositionEnabled must be enabled.
  </details>

**Note:** The nested static class `VisualBasic` provides VB.NET-compatible `Alloc` methods. These are designed to
accommodate language constraints in Visual Basic and **are not recommended for use in other .NET languages**, due to
minor performance overhead.

</details>

<details>
  <summary>NativeUtilities</summary>

Set of utilities for exchange data within the P/Invoke context.

#### Static Fields:

- <details>
  <summary>PointerSize</summary>
  Size in bytes of a memory pointer.
  </details>
- <details>
  <summary>GlobalizationInvariantModeEnabled</summary>
  Indicates whether globalization-invariant mode is enabled.

  **Note:** This property enables trimming by allowing the linker to remove unreachable code.
  </details>
- <details>
  <summary>UserInterfaceIso639P1</summary>

  Retrieves the `Iso639P1` enum value corresponding to the current user interface culture.
  </details>

#### Static Methods:

- <details>
  <summary>SizeOf&lt;T&gt;()</summary>

  Gets the memory size of `T` structure.

  **Note:** `T` is `unmanaged`.
  </details>
- <details>
  <summary>LoadNativeLib(String?, DllImportSearchPath?)</summary>
  Provides a high-level API for loading a native library.

  **Note:** This method is available only on .NET Core 3.0 and later.
  </details>
- <details>
  <summary>LoadNativeLib(String?, ref EventHandler?, DllImportSearchPath?)</summary>
  Provides a high-level API for loading a native library.

  **Note:** This method is available only on .NET Core 3.0 and later.
  </details>
- <details>
  <summary>GetNativeMethod&lt;TDelegate&gt;(IntPtr, String?)</summary>

  Gets the `TDelegate` delegate of an exported symbol.

  **Note:** This method is available only on .NET Core 3.0 and later.
  </details>
- <details>
  <summary>GetNativeMethod&lt;TDelegate&gt;(IntPtr, String?)</summary>

  Gets a function pointer of type `TDelegate` of an exported symbol.

  **Note:** This method is available only on .NET Core 3.0 and later.
  </details>
- <details>
  <summary>GetIso639P1(CultureInfo)</summary>

  Retrieves the `Iso639P1` enum value corresponding to the specified culture.
  </details>
- <details>
  <summary>GetUnsafeFuncPtr&lt;TDelegate&gt;(TDelegate)</summary>

  Creates an `FuncPtr<TDelegate>` from a memory reference to a `TDelegate` delegate instance.
  </details>
- <details>
  <summary>GetUnsafeValPtr&lt;T&gt;(in T)</summary>

  Retrieves an unsafe `ReadOnlyValPtr<T>` pointer from a read-only reference to a `T` value.

  **Note:** Starting with .NET 9.0, `T` can be a `ref struct`.
  </details>
- <details>
  <summary>GetUnsafeValPtrFromRef&lt;T&gt;(ref T)</summary>

  Retrieves an unsafe pointer of type `ValPtr<T>` from a reference to a value of type T.

  **Note:** Starting with .NET 9.0, `T` can be a `ref struct`.
  </details>
- <details>
  <summary>GetUnsafeIntPtr&lt;T&gt;(in T)</summary>

  Retrieves an unsafe `IntPtr` pointer from a read-only reference to a `T` `unmanaged` value.
  </details>
- <details>
  <summary>GetUnsafeUIntPtr&lt;T&gt;(in T)</summary>

  Retrieves an unsafe `UIntPtr` pointer from a read-only reference to a `T` `unmanaged` value.
  </details>
- <details>
  <summary>Transform&lt;TSource, TDestination&gt;(in TSource)</summary>

  Transforms a read-only reference of an `unmanaged` value of type `TSource` into a read-only reference of an
  `unmanaged`
  value of type `TDestination`.
  </details>
- <details>
  <summary>TransformReference&lt;TSource, TDestination&gt;(ref TSource)</summary>

  Transforms a reference of an `unmanaged` value of type `TSource` into a reference of an `unmanaged` value of type
  `TDestination`.
  </details>
- <details>
  <summary>ToBytes&lt;T&gt;(in TSource)</summary>

  Retrieves a `Byte` array from a read-only reference to a `TSource` value.

  **Note:** `TSource` is `unmanaged`.
  </details>
- <details>
  <summary>AsBytes&lt;TSource&gt;(in TSource)</summary>

  Creates a `ReadOnlySpan<Byte>` from an exising read-only reference to a `TSource` `unmanaged` value.
  </details>
- <details>
  <summary>AsBinarySpan&lt;TSource&gt;(ref TSource)</summary>

  Creates a `Span<Byte>` from an exising reference to a `TSource` value.

  **Note:** `TSource` is `unmanaged`.
  </details>
- <details>
  <summary>CreateArray&lt;T, TState&gt;(Int32, TState, SpanAction&lt;T, TState&gt;)</summary>

  Creates a new `T` array with a specific length and initializes it after creation by using the specified callback.

  **Note:** `T` is `unmanaged`.
  </details>
- <details>
  <summary>CopyBytes&lt;T&gt;(in TSource, Span&lt;Byte&gt;, Int32)</summary>

  Performs a binary copy of the given `TSource` to the destination span.

  **Note:** `TSource` is `unmanaged`.
  </details>

- <details>
  <summary>WithSafeReadOnlyFixed&lt;T&gt;(ref T, ReadOnlyFixedReferenceAction&lt;T&gt;)</summary>

  Prevents the garbage collector from relocating a given reference and fixes its memory address until action finishes.

  **Note:** Starting with .NET 9.0, `T` can be a `ref struct`.
  </details>
- <details>
  <summary>WithSafeReadOnlyFixed&lt;T, TArg&gt;(ref T, TArg, ReadOnlyFixedReferenceAction&lt;T, TArg&gt;)</summary>

  Prevents the garbage collector from relocating a given reference and fixes its memory address until action finishes.

  **Note:** Starting with .NET 9.0, both `T` and `TArg` can be `ref struct` types.
  </details>
- <details>
  <summary>WithSafeReadOnlyFixed&lt;T, TResult&gt;(ref T, ReadOnlyFixedReferenceFunc&lt;T, TResult&gt;)</summary>

  Prevents the garbage collector from relocating a given reference and fixes its memory address until func finishes.

  **Note:** Starting with .NET 9.0, `T` can be a `ref struct`.
  </details>
- <details>
  <summary>WithSafeReadOnlyFixed&lt;T, TArg, TResult&gt;(ref T, TArg, ReadOnlyFixedReferenceFunc&lt;T, TArg, TResult&gt;)</summary>

  Prevents the garbage collector from relocating a given reference and fixes its memory address until func finishes.

  **Note:** Starting with .NET 9.0, both `T` and `TArg` can be `ref struct` types.
  </details>

- <details>
  <summary>WithSafeFixed&lt;T&gt;(in T, ReadOnlyFixedReferenceAction&lt;T&gt;)</summary>

  Prevents the garbage collector from relocating a given read-only reference and fixes its memory address until action
  finishes.

  **Note:** Starting with .NET 9.0, `T` can be a `ref struct`.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T&gt;(ref T, FixedReferenceAction&lt;T&gt;)</summary>

  Prevents the garbage collector from relocating a given reference and fixes its memory address until action finishes.

  **Note:** Starting with .NET 9.0, `T` can be a `ref struct`.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg&gt;(in T, TArg, ReadOnlyFixedReferenceAction&lt;T, TArg&gt;)</summary>

  Prevents the garbage collector from relocating a given read-only reference and fixes its memory address until action
  finishes.

  **Note:** Starting with .NET 9.0, both `T` and `TArg` can be `ref struct` types.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg&gt;(ref T, TArg, FixedReferenceAction&lt;T, TArg&gt;)</summary>

  Prevents the garbage collector from relocating a given reference and fixes its memory address until action finishes.

  **Note:** Starting with .NET 9.0, both `T` and `TArg` can be `ref struct` types.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TResult&gt;(in T, ReadOnlyFixedReferenceFunc&lt;T, TResult&gt;)</summary>

  Prevents the garbage collector from relocating a given read-only reference and fixes its memory address until func
  finishes.

  **Note:** Starting with .NET 9.0, `T` can be a `ref struct`.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TResult&gt;(ref T, FixedReferenceFunc&lt;T, TResult&gt;)</summary>

  Prevents the garbage collector from relocating a given reference and fixes its memory address until func finishes.

  **Note:** Starting with .NET 9.0, `T` can be a `ref struct`.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg, TResult&gt;(in T, TArg, ReadOnlyFixedReferenceFunc&lt;T, TArg, TResult&gt;)</summary>

  Prevents the garbage collector from relocating a given read-only reference and fixes its memory address until func
  finishes.

  **Note:** Starting with .NET 9.0, both `T` and `TArg` can be `ref struct` types.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg, TResult&gt;(ref T, TArg, FixedReferenceFunc&lt;T, TArg, TResult&gt;)</summary>

  Prevents the garbage collector from relocating a given reference and fixes its memory address until func finishes.

  **Note:** Starting with .NET 9.0, both `T` and `TArg` can be `ref struct` types.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TDelegate&gt;(TDelegate, FixedMethodAction&lt;TDelegate&gt;)</summary>
  Prevents the garbage collector from relocating a given method delegate and fixes its memory address until action finishes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TDelegate, TArg&gt;(TDelegate, TArg, FixedMethodAction&lt;TDelegate, TArg&gt;)</summary>

  Prevents the garbage collector from relocating a given method delegate and fixes its memory address until action
  finishes.

  **Note:** Starting with .NET 9.0, `TArg` can be a `ref struct`.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TDelegate, TResult&gt;(TDelegate, FixedMethodFunc&lt;TDelegate, TResult&gt;)</summary>
  Prevents the garbage collector from relocating a given method delegate and fixes its memory address until func finishes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TDelegate, TArg, TResult&gt;(TDelegate, TArg, FixedMethodFunc&lt;TDelegate, TArg, TResult&gt;)</summary>

  Prevents the garbage collector from relocating a given method delegate and fixes its memory address until func
  finishes.

  **Note:** Starting with .NET 9.0, `TArg` can be a `ref struct`.
  </details>

- <details>
  <summary>WithSafeFixed&lt;T0, ..., T7&gt;(Span&lt;T0&gt;, ..., Span&lt;T7&gt;, FixedListAction)</summary>
  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until action completes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T0, ..., T7, TArg&gt;(Span&lt;T0&gt;, ..., Span&lt;T7&gt;, TArg, FixedListAction&lt;TArg&gt;)</summary>

  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until action completes.

  **Note:** Starting with .NET 9.0, `TArg` can be a `ref struct`.
  </details>

- <details>
  <summary>WithSafeFixed&lt;T0, ..., T7&gt;(ReadOnlySpan&lt;T0&gt;, ..., ReadOnlySpan&lt;T7&gt;, ReadOnlyFixedListAction)</summary>
  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until action completes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T0, ..., T7, TArg&gt;(ReadOnlySpan&lt;T0&gt;, ..., ReadOnlySpan&lt;T7&gt;, TArg, ReadOnlyFixedListAction&lt;TArg&gt;)</summary>

  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until action completes.

  **Note:** Starting with .NET 9.0, `TArg` can be a `ref struct`.
  </details>

- <details>
  <summary>WithSafeFixed&lt;T0, ..., T7, TResult&gt;(Span&lt;T0&gt;, ..., Span&lt;T7&gt;, FixedListFunc&lt;TResult&gt;)</summary>
  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until func completes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T0, ..., T7, TArg, TResult&gt;(Span&lt;T0&gt;, ..., Span&lt;T7&gt;, TArg, FixedListFunc&lt;TArg, TResult&gt;)</summary>

  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until func completes.

  **Note:** Starting with .NET 9.0, `TArg` can be a `ref struct`.
  </details>

- <details>
  <summary>WithSafeFixed&lt;T0, ..., T7, TResult&gt;(ReadOnlySpan&lt;T0&gt;, ..., ReadOnlySpan&lt;T7&gt;, ReadOnlyFixedListFunc&lt;TResult&gt;)</summary>
  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until func completes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T0, ..., T7, TArg, TResult&gt;(Span&lt;T0&gt;, ..., Span&lt;T7&gt;, TArg, ReadOnlyFixedListFunc&lt;TArg, TResult&gt;)</summary>

  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until func completes.

  **Note:** Starting with .NET 9.0, `TArg` can be a `ref struct`.
  </details>

- <details>
  <summary>WithSafeReadOnlyFixed&lt;T0, ..., T7&gt;(Span&lt;T0&gt;, ..., Span&lt;T7&gt;, ReadOnlyFixedListAction)</summary>
  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until action completes.
  </details>
- <details>
  <summary>WithSafeReadOnlyFixed&lt;T0, ..., T7, TArg&gt;(Span&lt;T0&gt;, ..., Span&lt;T7&gt;, TArg, ReadOnlyFixedListAction&lt;TArg&gt;)</summary>

  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until action completes.

  **Note:** Starting with .NET 9.0, `TArg` can be a `ref struct`.
  </details>

- <details>
  <summary>WithSafeReadOnlyFixed&lt;T0, ..., T7, TResult&gt;(Span&lt;T0&gt;, ..., Span&lt;T7&gt;, ReadOnlyFixedListFunc&lt;TResult&gt;)</summary>
  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until func completes.
  </details>
- <details>
  <summary>WithSafeReadOnlyFixed&lt;T0, ..., T7, TArg, TResult&gt;(Span&lt;T0&gt;, ..., Span&lt;T7&gt;, TArg, ReadOnlyFixedListFunc&lt;TArg, TResult&gt;)</summary>

  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until func completes.

  **Note:** Starting with .NET 9.0, `TArg` can be a `ref struct`.
  </details>
- <details>
  <summary>GetFixedMethod&lt;TDelegate&gt;(TDelegate?)</summary>

  Creates an `IFixedMethod<TDelegate>.IDisposable` instance by marshalling the current `TDelegate` instance,
  ensuring a safe interop context.
  </details>

</details>

---

# License

This project is licensed under the **MIT License**, one of the most permissive and widely-used open-source licenses.

## Key Highlights:

- **Freedom of Use**: The package can be used in both open-source and closed-source projects without restrictions.
- **Modification and Distribution**: You are free to modify, distribute, and even sublicense the software as needed.
- **Attribution**: The only requirement is to include the original copyright notice and license text in any copies or
  substantial portions of the software.

## Disclaimer:

> The software is provided "as is," without warranty of any kind. The authors are not liable for any damages or issues
> that may arise from its use.

For more details, refer to the full license text included in the [LICENSE](LICENSE) file.

---

# Contributing

We warmly welcome contributions to this open-source project! Whether you're here to report issues, propose enhancements,
or contribute directly to the codebase, your help is greatly appreciated. Below are some ways you can get involved:

## Reporting Issues

If you encounter a bug, experience unexpected behavior, or have suggestions for improvement, feel free to open an issue.
Please include as much detail as possible, such as:

- Steps to reproduce the problem
- Your environment (e.g., OS, software version)
- Any relevant logs or screenshots

## Proposing Improvements

Have an idea for a new feature or enhancement? Open an issue with a clear description of your proposal and why you think
it would benefit the project.

## Contributing Code

You can directly contribute to the project by:

- Fixing bugs
- Adding new features
- Improving existing functionality
- Enhancing documentation

To contribute code:

1. Fork the repository.
2. Create a new branch for your changes.
3. Make your changes and commit them with clear and concise messages.
4. Submit a pull request for review.

Feel free to discuss your ideas or changes in an issue or pull request to align with the project's goals.

## Translations

We currently support only a few languages, but we are open to adding more! If you'd like to help with translations,
please open an issue or reach out to us. Your contributions to expanding the project's accessibility are highly valued.

This library currently supports translations for the following languages:

* **English**
* **Arabic**
* **Chinese**
* **French**
* **German**
* **Italian**
* **Japanese**
* **Portuguese**
* **Russian**
* **Spanish**

## Collaboration Guidelines

When contributing, please be respectful and constructive. We aim to create an inclusive and welcoming environment for
everyone.

Thank you for considering contributing to this project! Your involvement, whether through reporting, coding, or
translating, helps make this project better for everyone. 🚀
