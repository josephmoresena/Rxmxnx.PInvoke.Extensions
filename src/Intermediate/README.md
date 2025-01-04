# Intermediate Libraries

To enhance the maintainability of `Rxmxnx.PInvoke.Extensions`, intermediary projects were created to modularize
functionality and separate it from the final assembly:

- **`Common`**: Contains foundational structures, interfaces, delegates, classes, and utilities that are shared across
  all components of `Rxmxnx.PInvoke.Extensions`.
- **`Buffers`**: Provides structures and classes for working with managed buffers.
- **`CString`**: Includes classes for handling UTF-8 and ASCII strings.
- **`Extensions`**: Offers classes containing extension methods.