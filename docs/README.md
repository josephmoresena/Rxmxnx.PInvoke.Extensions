# Documentation

Welcome. These guides explain **what** `Rxmxnx.PInvoke.Extensions` can do, **when** to reach for it, and **which APIs** to use.

Start with a short README, then come here when you need depth.

## Start with a goal

| Goal | Guide |
| --- | --- |
| Install the package, pick a language version and target framework, and check AOT support | [Getting started](getting-started.md) |
| Understand the library by capability, not by type list | [Capabilities](capabilities.md) |
| Copy a pattern for a real problem | [Use cases](use-cases.md) |
| Look up a type, interface, or helper | [API reference](api/README.md) |
| See which APIs existed until 2.9.5 vs later TFMs (.NET Framework / UWP / netstandard2.0) | [Target frameworks and public API surface](api/compatibility.md) |

## How the documentation is organized

- **Capabilities** answer “what can I do?” They group features by outcome: UTF-8 text, typed pointers, scoped pinning, binary views, multidimensional flattening, stack buffers, runtime detection, and fast versus slow span.
- **Use cases** answer “how do I do this?” They are short recipes with working snippets.
- **API reference** answers “what is this type?” It is split by area so you can jump to pointers, fixed memory, functional interfaces, UTF-8, buffers, wrappers, extensions, or utilities.

The README is intentionally short. The old all-in-one API catalog lived there; that content now lives in these guides so each page has a job.

## Complementary notes in the source tree

Some intermediate projects still keep focused notes next to the code:

- [UTF-8 / `CString` examples](../src/Intermediate/Rxmxnx.PInvoke.CString.Intermediate/README.md)
- [Managed buffers](../src/Intermediate/Rxmxnx.PInvoke.Buffers.Intermediate/README.md)
- [Memory extensions and typed pointers](../src/Intermediate/Rxmxnx.PInvoke.Extensions.Intermediate/README.md)
- [How intermediate libraries are assembled](../src/Intermediate/README.md)

XML comments in the source remain the complete member-level contract, including overload-by-overload remarks for `WithSafeFixed` and friends.
