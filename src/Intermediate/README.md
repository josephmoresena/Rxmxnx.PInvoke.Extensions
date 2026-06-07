# Intermediate Libraries

To enhance the maintainability of `Rxmxnx.PInvoke.Extensions`, intermediary projects were created to modularize
functionality and separate it from the final assembly:

- **`Common`**: Contains foundational structures, interfaces, delegates, classes, and utilities that are shared across
  all components of `Rxmxnx.PInvoke.Extensions`.
- **`Buffers`**: Provides structures and classes for working with managed buffers.
- **`CString`**: Includes classes for handling UTF-8 and ASCII strings.
- **`Extensions`**: Offers classes containing extension methods.

The intermediary projects are compiled using the latest supported version of the C# language for each target framework,
ranging from .NET Standard 2.1 to the latest publicly available version of .NET.

# Package Patcher Library

Starting with .NET 9.0, `ref struct` types are allowed in generics, and many generic types in
`Rxmxnx.PInvoke.Extensions` support their usage. However, some features are not compatible with `ref struct` types and
may cause errors when maintaining code originally designed for .NET 8 or earlier.

For this reason, **`IlPatcher`** library is compiled alongside the final assembly targeting versions
later than .NET 8.0. Using MSBuild and Mono.Cecil, it injects the missing IL code to ensure both source and binary
compatibility with these target frameworks.