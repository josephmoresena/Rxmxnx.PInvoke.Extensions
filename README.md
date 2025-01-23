[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=bugs)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=coverage)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![NuGet](https://img.shields.io/nuget/v/Rxmxnx.PInvoke.Extensions)](https://www.nuget.org/packages/Rxmxnx.PInvoke.Extensions/)
[![fuget.org](https://www.fuget.org/packages/Rxmxnx.PInvoke.Extensions/badge.svg)](https://www.fuget.org/packages/Rxmxnx.PInvoke.Extensions)

---

# Table of Contents

- [Description](#description)
    - [Features](#features)
- [Getting Started](#getting-started)
    - [Installation](#installation)
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

**Note:** This package currently supports .NET 6 and higher. Ensure your project targets a compatible framework before
installing.

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

**Note:** This interface inherits from `IEquatable<T>`. This type allows public implementation or inheritance.

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

This interface exposes a reference to an object of type `T`,
allowing the object to be used and potentially modified.

**Note:** This interface inherits from `IReadOnlyReferenceable<T>`. This type allows public implementation or
inheritance.

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

**Note:** This interface inherits from `IEquatable<T>`. This type allows public implementation or inheritance.

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
implementation or
inheritance.

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
or
inheritance.

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
allows
public implementation or inheritance.

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
implementation
or inheritance.

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

This interface exposes a read-only reference to an object of type T, allowing the object to be used without
modification.

**Note:** This interface inherits from `IReadOnlyReferenceable<T>` and `IReadOnlyFixedMemory`. This type allows public
implementation or inheritance.

#### Methods:

- <details>
  <summary>Transformation(out IReadOnlyFixedMemory)</summary>

  Reinterprets the read-only `T` fixed memory reference as a read-only `TDestination` memory reference.
  </details>

#### Disposable interface

`IReadOnlyFixedReference<T>.IDisposable` representing a disposable `IReadOnlyFixedReference<T>` object.
This interface is used for managing fixed memory blocks that require explicit resource cleanup.

**Note:** This interface inherits from `IReadOnlyFixedReference<T>` and `IReadOnlyFixedMemory.IDisposable`. This type
allows
public implementation or inheritance.

</details>

<details>
  <summary>IFixedReference&lt;T&gt;</summary>

This interface represents a mutable reference to a fixed memory location.

**Note:** This interface inherits from `IReferenceable<T>`, `IReadOnlyFixedReference<T>` and `IFixedMemory`. This type
allows
public implementation or inheritance.

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
allows
public implementation or inheritance.

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
allows
public implementation or inheritance.

</details>

<details>
  <summary>IFixedContext&lt;T&gt;</summary>

Interface representing a context from a block of fixed memory.

**Note:** This interface inherits from `IReadOnlyFixedContext<T>` and `IFixedMemory<T>`. This type allows public
implementation
or inheritance.

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

**Note:** This interface inherits from `IEnumerable<T>`. This type allows public implementation or inheritance.

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
  </details>

</details>

<details>
  <summary>IUtf8FunctionState&lt;TSelf&gt;</summary>

Interface representing a value state for functional CString creation.

**Note:** `TSelf` generic type is `struct`. This type allows public implementation or inheritance.

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

**Note:** This type not allow public implementation or inheritance.

#### Static Methods:

- <details>
  <summary>GetMetadata&lt;TBuffer&gt;()</summary>

  Retrieves the `BufferTypeMetadata<T>` instance from `TBuffer`.
  </details>

</details>

<details>
  <summary>IManagedBinaryBuffer&lt;T&gt;</summary>

This interfaces exposes a binary managed buffer.

**Note:** This interface inherits from `IManagedBuffer<T>`. This type not allow public implementation or inheritance.

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
allow
public implementation or inheritance.

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

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>TResult SpanFunc&lt;T, in TArg, out TResult&gt;(Span&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives a span of type `T`, a state object of type `TArg` and returns a result of type
`TResult`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>TResult ReadOnlySpanFunc&lt;T, in TArg, out TResult&gt;(Span&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives a read-only span of type `T`, a state object of type `TArg` and returns a result
of type `TResult`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

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

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>void ReadOnlyFixedAction(in IReadOnlyFixedMemory)</summary>

Represents an action that operates on a read-only fixed memory instance.

</details>

<details>
  <summary>void ReadOnlyFixedAction&lt;in TArg&gt;(in IReadOnlyFixedMemory, TArg)</summary>

Represents an action that operates on a read-only fixed memory instance using an additional state object.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>TResult FixedFunc&lt;out TResult&gt;(in IFixedMemory)</summary>

Represents a function that operates on a fixed memory instance.

</details>

<details>
  <summary>TResult FixedFunc&lt;in TArg, out TResult&gt;(in IFixedMemory, TArg)</summary>

Represents a function that operates on a fixed memory instance using an additional state object.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>TResult ReadOnlyFixedFunc&lt;out TResult&gt;(in IReadOnlyFixedMemory&lt;T&gt;)</summary>

Represents a function that operates on a read-only fixed memory instance.

</details>

<details>
  <summary>TResult ReadOnlyFixedFunc&lt;in TArg, out TResult&gt;(in IReadOnlyFixedMemory&lt;T&gt;, TArg)</summary>

Represents a function that operates on a read-only fixed memory instance using an additional state object.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

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

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>void ReadOnlyFixedContextAction&lt;T&gt;(in IReadOnlyFixedContext&lt;T&gt;)</summary>

Encapsulates a method that receives an instance of `IReadOnlyFixedContext<T>`.

</details>

<details>
  <summary>void ReadOnlyFixedContextAction&lt;T, in TArg,&gt;(in IFixedContext&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives an instance of `IReadOnlyFixedContext<T>` and a state object of type `TArg`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>TResult FixedContextFunc&lt;T, out TResult&gt;(in IFixedContext&lt;T&gt;)</summary>

Encapsulates a method that receives an instance of `IFixedContext<T>` and returns a value of type `TResult`.

</details>

<details>
  <summary>TResult FixedContextFunc&lt;T, in TArg, out TResult&gt;(in IFixedContext&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives an instance of `IFixedContext<T>` and returns a value of type `TResult`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>TResult ReadOnlyFixedContextFunc&lt;T, out TResult&gt;(in IReadOnlyFixedContext&lt;T&gt;)</summary>

Encapsulates a method that receives an instance of `IReadOnlyFixedContext<T>` and returns a value of type `TResult`.

</details>

<details>
  <summary>TResult FixedContextFunc&lt;T, in TArg, out TResult&gt;(in IReadOnlyFixedContext&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives an instance of `IReadOnlyFixedContext<T>` and returns a value of type `TResult`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

## Fixed memory reference Delegates

These delegates encapsulate methods that operate on fixed memory references of a specific type.

<details>
  <summary>void FixedReferenceAction&lt;T&gt;(in IFixedReference&lt;T&gt;)</summary>

Encapsulates a method that receives an instance of `IFixedReference<T>`.

</details>

<details>
  <summary>void FixedReferenceAction&lt;T, in TArg,&gt;(in IFixedReference&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives an instance of `IFixedReference<T>` and a state object of type `TArg`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>void ReadOnlyFixedReferenceAction&lt;T&gt;(in IReadOnlyFixedReference&lt;T&gt;)</summary>

Encapsulates a method that receives an instance of `IReadOnlyFixedReference<T>`.

</details>

<details>
  <summary>void ReadOnlyFixedReferenceAction&lt;T, in TArg,&gt;(in IFixedReference&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives an instance of `IReadOnlyFixedReference<T>` and a state object of type `TArg`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>TResult FixedReferenceFunc&lt;T, out TResult&gt;(in IFixedReference&lt;T&gt;)</summary>

Encapsulates a method that receives an instance of `IFixedReference<T>` and returns a value of type `TResult`.

</details>

<details>
  <summary>TResult FixedReferenceFunc&lt;T, in TArg, out TResult&gt;(in IFixedReference&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives an instance of `IFixedReference<T>` and returns a value of type `TResult`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>TResult ReadOnlyFixedReferenceFunc&lt;T, out TResult&gt;(in IReadOnlyFixedReference&lt;T&gt;)</summary>

Encapsulates a method that receives an instance of `IReadOnlyFixedReference<T>` and returns a value of type `TResult`.

</details>

<details>
  <summary>TResult FixedReferenceFunc&lt;T, in TArg, out TResult&gt;(in IReadOnlyFixedReference&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives an instance of `IReadOnlyFixedReference<T>` and returns a value of type `TResult`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

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

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>TResult FixedMethodFunc&lt;T, out TResult&gt;(in IFixedMethod&lt;T&gt;)</summary>

Encapsulates a method that receives an instance of `IFixedMethod<T>` and returns a value of type `TResult`.

</details>

<details>
  <summary>TResult FixedMethodFunc&lt;T, in TArg, out TResult&gt;(in IFixedMethod&lt;T&gt;, TArg)</summary>

Encapsulates a method that receives an instance of `IFixedMethod<T>` and returns a value of type `TResult`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

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

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>void ReadOnlyFixedListAction(ReadOnlyFixedMemoryList)</summary>

Encapsulates a method that receives an instance of `ReadOnlyFixedMemoryList`.

</details>

<details>
  <summary>void ReadOnlyFixedListAction&lt;in TArg,&gt;(FixedMemoryList, TArg)</summary>

Encapsulates a method that receives an instance of `ReadOnlyFixedMemoryList` and a state object of type `TArg`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>TResult FixedListFunc&lt;out TResult&gt;(FixedMemoryList)</summary>

Encapsulates a method that receives an instance of `FixedMemoryList` and returns a value of type `TResult`.

</details>

<details>
  <summary>TResult FixedListFunc&lt;in TArg, out TResult&gt;(FixedMemoryList, TArg)</summary>

Encapsulates a method that receives an instance of `FixedMemoryList` and returns a value of type `TResult`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>TResult ReadOnlyFixedListFunc&lt;out TResult&gt;(ReadOnlyFixedMemoryList)</summary>

Encapsulates a method that receives an instance of `ReadOnlyFixedMemoryList` and returns a value of type `TResult`.

</details>

<details>
  <summary>TResult FixedListFunc&lt;in TArg, out TResult&gt;(ReadOnlyFixedMemoryList, TArg)</summary>

Encapsulates a method that receives an instance of `ReadOnlyFixedMemoryList` and returns a value of type `TResult`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

## UTF-8/ASCII Delegates

These delegates encapsulate methods that allow creating and operating with UTF-8/ASCII text.

<details>
  <summary>void CStringSequenceCreationAction&lt;in TArg&gt;(Span&lt;Byte&gt;, Int32, TArg)</summary>

Encapsulates a method that receives a span of bytes, an index and a state object of type `TArg`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>void CStringSequenceAction(FixedCStringSequence)</summary>

Encapsulates a method that operates on a `FixedCStringSequence` instance.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>void CStringSequenceAction&lt;in TArg&gt;(FixedCStringSequence, TArg)</summary>

Encapsulates a method that operates on a `FixedCStringSequence` instance and a state object of type `TArg`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>TResult CStringSequenceFunc&lt;out TResult&gt;(FixedCStringSequence)</summary>

Encapsulates a method that operates on a `FixedCStringSequence` instance and returns a value of type `TResult`.

</details>

<details>
  <summary>TResult CStringSequenceFunc&lt;in TArg, out TResult&gt;(FixedCStringSequence, TArg)</summary>


Encapsulates a method that operates on a `FixedCStringSequence` instance, a state object of type `TArg` and returns a
value of type `TResult`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

## Buffers Delegates

These delegates encapsulate methods that operate on instances of managed buffers.

<details>
  <summary>void ScopedBufferAction&lt;T&gt;(ScopedBuffer)</summary>

Encapsulates a method that receives a buffer of objects of type `T`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>void ScopedBufferAction&lt;T, in TArg&gt;(ScopedBuffer, TArg)</summary>

Encapsulates a method that receives a buffer of objects of type `T` and a state object of type `TArg`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

<details>
  <summary>TResult ScopedBufferFunc&lt;T, out TResult&gt;(ScopedBuffer)</summary>

Encapsulates a function that receives a buffer of objects of type `T` and returns a result of type `TResult`.
</details>

<details>
  <summary>TResult ScopedBufferFunc&lt;T, in TArg, out TResult&gt;(ScopedBuffer, TArg)</summary>


Encapsulates a method that receives a buffer of objects of type `T`, a state object of type `TArg` and returns a value
of type `TResult`.

**Note:** In .NET 9.0+ `TState` allows `ref struct`.

</details>

---

# Structs

`Rxmxnx.PInvoke.Extensions` provides value types for the managed handling of references, fixed memory segments, and
stack-allocated buffers

## Pointers

These structures safely represent memory addresses for references or methods of a specific type.

<details>
  <summary>ReadOnlyValPtr&lt;T&gt;</summary>

Represents a platform-specific type used to manage a pointer to a read-only value of type `T`.

**Note:** This struct implements `IComparable`, `ISpanFormattable`, `ISerializable`, `IWrapper<IntPtr>`,
`IEquatable<ReadOnlyValPtr<T>>` and `IComparable<ReadOnlyValPtr<T>>` interfaces.

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
  </details>

</details>

<details>
  <summary>ValPtr&lt;T&gt;</summary>

Represents a platform-specific type used to manage a pointer to a mutable value of type `T`.

**Note:** This struct implements `IComparable`, `ISpanFormattable`, `ISerializable`, `IWrapper<IntPtr>`,
`IEquatable<ValPtr<T>>` and `IComparable<ValPtr<T>>` interfaces.

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

---

# Classes

`Rxmxnx.PInvoke.Extensions` provides reference types for the managed handling of UTF8/ASCII texts, memory regions, and
metadata for stack-allocated buffer management.

<details>
  <summary>ValueRegion&lt;T&gt;</summary>

This class represents a region of memory that contains a sequence of `T` values.

**Note:** `T` must be `unmanaged`. This type not allow public inheritance.

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

  Creates a new `ValueRegion<T>` instance from a `ReadOnlySpanFunc<T, TState>` function and TState instance.
  </details>

</details>

<details>
  <summary>BufferTypeMetadata</summary>

Represents the metadata of a managed buffer type.

**Note:** Implements `IEnumerableSequence<BufferTypeMetadata>`. This type not allow public inheritance.

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

**Note:** Inherits from `BufferTypeMetadata`. This type not allow public inheritance.

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
- In .NET 9.0+ `params` is used with `ReadOnlySpan<>` arguments instead of `[]` arguments.

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
  </details>
- <details>
  <summary>CreateUnsafe(IntPtr, Int32, useFullLength)</summary>

  Creates a new instance of the `CString` class using the pointer to a UTF-8 character array and length provided.
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
- In .NET 9.0+ `params` is used with `ReadOnlySpan<>` arguments instead of `[]` arguments.

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
- <details>
  <summary>ReadOnlySpan&lt;Byte&gt;(CString?)</summary>

  Defines an implicit conversion of a given `CString` to a read-only span of bytes.
  </details>

### Static Methods:

- <details>
  <summary>Create&lt;TState&gt;(TState, CStringSequenceCreationAction&lt;TState&gt;, params Int32?[])</summary>
  Creates a new UTF-8 text sequence with specific lengths, and initializes each UTF-8 text string in it after creation using the specified callback.
  </details>
- <details>
  <summary>Create(ReadOnlySpan&lt;Char&gt;)</summary>

  Creates a new `CStringSequence` instance from a UTF-8 buffer.
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
  Prevents the garbage collector from relocating a delegate in memory and fixes its address while an action is being performed, passing an additional argument to the action.
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

</details>

<details>
  <summary>MemoryBlockExtensions</summary>

Additional functionality for working with memory blocks.

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

  Retrieves an unsafe `IntPtr` pointer from `Span<TZ` instance.
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
  </details>
- <details>
  <summary>AsSpan&lt;T&gt;(this T[...])</summary>
  Creates a new span over a target array.
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
  <summary>IsNativeAot</summary>
  Indicates whether the current runtime is NativeAOT.
  </details>

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

  **Note:** `TBuffer` is `struct` and `IManagedBuffer<Object>`.
  </details>
- <details>
  <summary>Register&lt;T, TBuffer&gt;()</summary>

  Registers `T` buffer.

  **Note:** `T` is `struct`. `TBuffer` is `struct` and `IManagedBuffer<T>`.
  </details>
- <details>
  <summary>RegisterNullable&lt;T, TBuffer&gt;()</summary>

  Registers `T?` buffer.

  **Note:** `T` is `struct`. `TBuffer` is `struct` and `IManagedBuffer<T?>`.
  </details>
- <details>
  <summary>PrepareBinaryBuffer(UInt16)</summary>
  Prepares the binary buffer metadata needed to allocate given number of objects.
  </details>
- <details>
  <summary>PrepareBinaryBuffer&lt;T&gt;(UInt16)</summary>

  Prepares the binary buffer metadata needed to allocate given number of `T` items.

  **Note:** `T` is `struct`.
  </details>
- <details>
  <summary>PrepareBinaryBufferNullable&lt;T&gt;()</summary>

  Prepares the binary buffer metadata needed to allocate given number of `T?` items.

  **Note:** `T` is `struct`.
  </details>

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
  </details>
- <details>
  <summary>LoadNativeLib(String?, ref EventHandler?, DllImportSearchPath?)</summary>
  Provides a high-level API for loading a native library.
  </details>
- <details>
  <summary>GetNativeMethod&lt;TDelegate&gt;(IntPtr, String?)</summary>

  Gets the `TDelegate` delegate of an exported symbol.
  </details>
- <details>
  <summary>GetNativeMethod&lt;TDelegate&gt;(IntPtr, String?)</summary>

  Gets a function pointer of type `TDelegate` of an exported symbol.
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
  </details>
- <details>
  <summary>GetUnsafeValPtrFromRef&lt;T&gt;(ref T)</summary>

  Retrieves an unsafe pointer of type `ValPtr<T>` from a reference to a value of type T.
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
  </details>
- <details>
  <summary>WithSafeReadOnlyFixed&lt;T, TArg&gt;(ref T, TArg, ReadOnlyFixedReferenceAction&lt;T, TArg&gt;)</summary>
  Prevents the garbage collector from relocating a given reference and fixes its memory address until action finishes.
  </details>
- <details>
  <summary>WithSafeReadOnlyFixed&lt;T, TResult&gt;(ref T, ReadOnlyFixedReferenceFunc&lt;T, TResult&gt;)</summary>
  Prevents the garbage collector from relocating a given reference and fixes its memory address until func finishes.
  </details>
- <details>
  <summary>WithSafeReadOnlyFixed&lt;T, TArg, TResult&gt;(ref T, TArg, ReadOnlyFixedReferenceFunc&lt;T, TArg, TResult&gt;)</summary>
  Prevents the garbage collector from relocating a given reference and fixes its memory address until func finishes.
  </details>

- <details>
  <summary>WithSafeFixed&lt;T&gt;(in T, ReadOnlyFixedReferenceAction&lt;T&gt;)</summary>
  Prevents the garbage collector from relocating a given read-only reference and fixes its memory address until action finishes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T&gt;(ref T, FixedReferenceAction&lt;T&gt;)</summary>
  Prevents the garbage collector from relocating a given reference and fixes its memory address until action finishes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg&gt;(in T, TArg, ReadOnlyFixedReferenceAction&lt;T, TArg&gt;)</summary>
  Prevents the garbage collector from relocating a given read-only reference and fixes its memory address until action finishes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg&gt;(ref T, TArg, FixedReferenceAction&lt;T, TArg&gt;)</summary>
  Prevents the garbage collector from relocating a given reference and fixes its memory address until action finishes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TResult&gt;(in T, ReadOnlyFixedReferenceFunc&lt;T, TResult&gt;)</summary>
  Prevents the garbage collector from relocating a given read-only reference and fixes its memory address until func finishes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TResult&gt;(ref T, FixedReferenceFunc&lt;T, TResult&gt;)</summary>
  Prevents the garbage collector from relocating a given reference and fixes its memory address until func finishes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg, TResult&gt;(in T, TArg, ReadOnlyFixedReferenceFunc&lt;T, TArg, TResult&gt;)</summary>
  Prevents the garbage collector from relocating a given read-only reference and fixes its memory address until func finishes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T, TArg, TResult&gt;(ref T, TArg, FixedReferenceFunc&lt;T, TArg, TResult&gt;)</summary>
  Prevents the garbage collector from relocating a given reference and fixes its memory address until func finishes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TDelegate&gt;(TDelegate, FixedMethodAction&lt;TDelegate&gt;)</summary>
  Prevents the garbage collector from relocating a given method delegate and fixes its memory address until action finishes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TDelegate, TArg&gt;(TDelegate, TArg, FixedMethodAction&lt;TDelegate, TArg&gt;)</summary>
  Prevents the garbage collector from relocating a given method delegate and fixes its memory address until action finishes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TDelegate, TResult&gt;(TDelegate, FixedMethodFunc&lt;TDelegate, TResult&gt;)</summary>
  Prevents the garbage collector from relocating a given method delegate and fixes its memory address until func finishes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;TDelegate, TArg, TResult&gt;(TDelegate, TArg, FixedMethodFunc&lt;TDelegate, TArg, TResult&gt;)</summary>
  Prevents the garbage collector from relocating a given method delegate and fixes its memory address until func finishes.
  </details>

- <details>
  <summary>WithSafeFixed&lt;T0, ..., T7&gt;(Span&lt;T0&gt;, ..., Span&lt;T7&gt;, FixedListAction)</summary>
  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until action completes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T0, ..., T7, TArg&gt;(Span&lt;T0&gt;, ..., Span&lt;T7&gt;, TArg, FixedListAction&lt;TArg&gt;)</summary>
  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until action completes.
  </details>

- <details>
  <summary>WithSafeFixed&lt;T0, ..., T7&gt;(ReadOnlySpan&lt;T0&gt;, ..., ReadOnlySpan&lt;T7&gt;, ReadOnlyFixedListAction)</summary>
  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until action completes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T0, ..., T7, TArg&gt;(ReadOnlySpan&lt;T0&gt;, ..., ReadOnlySpan&lt;T7&gt;, TArg, ReadOnlyFixedListAction&lt;TArg&gt;)</summary>
  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until action completes.
  </details>

- <details>
  <summary>WithSafeFixed&lt;T0, ..., T7, TResult&gt;(Span&lt;T0&gt;, ..., Span&lt;T7&gt;, FixedListFunc&lt;TResult&gt;)</summary>
  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until func completes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T0, ..., T7, TArg, TResult&gt;(Span&lt;T0&gt;, ..., Span&lt;T7&gt;, TArg, FixedListFunc&lt;TArg, TResult&gt;)</summary>
  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until func completes.
  </details>

- <details>
  <summary>WithSafeFixed&lt;T0, ..., T7, TResult&gt;(ReadOnlySpan&lt;T0&gt;, ..., ReadOnlySpan&lt;T7&gt;, ReadOnlyFixedListFunc&lt;TResult&gt;)</summary>
  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until func completes.
  </details>
- <details>
  <summary>WithSafeFixed&lt;T0, ..., T7, TArg, TResult&gt;(Span&lt;T0&gt;, ..., Span&lt;T7&gt;, TArg, ReadOnlyFixedListFunc&lt;TArg, TResult&gt;)</summary>
  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until func completes.
  </details>

- <details>
  <summary>WithSafeReadOnlyFixed&lt;T0, ..., T7&gt;(Span&lt;T0&gt;, ..., Span&lt;T7&gt;, ReadOnlyFixedListAction)</summary>
  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until action completes.
  </details>
- <details>
  <summary>WithSafeReadOnlyFixed&lt;T0, ..., T7, TArg&gt;(Span&lt;T0&gt;, ..., Span&lt;T7&gt;, TArg, ReadOnlyFixedListAction&lt;TArg&gt;)</summary>
  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until action completes.
  </details>

- <details>
  <summary>WithSafeReadOnlyFixed&lt;T0, ..., T7, TResult&gt;(Span&lt;T0&gt;, ..., Span&lt;T7&gt;, ReadOnlyFixedListFunc&lt;TResult&gt;)</summary>
  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until func completes.
  </details>
- <details>
  <summary>WithSafeReadOnlyFixed&lt;T0, ..., T7, TArg, TResult&gt;(Span&lt;T0&gt;, ..., Span&lt;T7&gt;, TArg, ReadOnlyFixedListFunc&lt;TArg, TResult&gt;)</summary>
  Prevents the garbage collector from reallocating given spans and fixes their memory addresses until func completes.
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

## Collaboration Guidelines

When contributing, please be respectful and constructive. We aim to create an inclusive and welcoming environment for
everyone.

Thank you for considering contributing to this project! Your involvement, whether through reporting, coding, or
translating, helps make this project better for everyone. 🚀
