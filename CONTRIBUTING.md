# Reporting Issues

If you encounter a bug, experience unexpected behavior, or have suggestions for improvement, feel free to open an issue.
Please include as much detail as possible, such as:

- Steps to reproduce the problem
- Your environment (e.g., OS, software version)
- Any relevant logs or screenshots

# Proposing Improvements

Have an idea for a new feature or enhancement? Open an issue with a clear description of your proposal and why you think
it would benefit the project.

# Contributing Code

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

## Contributor License Agreement

By submitting a contribution to **this project**, you grant **the Project Owner** a perpetual, worldwide, non-exclusive,
royalty-free, irrevocable, and sublicensable license to use, modify, reproduce, distribute, and license your
contribution as part of this project under the MIT License or any later version.

You grant **the Project Owner** a perpetual, worldwide, non-exclusive, royalty-free, and irrevocable patent license for
any patent claims you own or control that are necessarily infringed by your contribution or by its use within this
project.

You retain ownership of your contribution.

You confirm that you have the legal authority to submit the contribution and grant this license. If your employer or
another entity has rights to your work, you confirm that all required permissions have been obtained.

You represent that the contribution is your original work and does not knowingly infringe the rights of any third party.

All contributions are provided **"AS IS"**, without warranties or conditions of any kind, express or implied.

---

# How this repository is put together

The published unit is one package: `Rxmxnx.PInvoke.Extensions`. The source is not one project. Work happens in four
intermediate libraries; tests bind to those four; the NuGet assembly is a **recompilation of the same sources** into a
single module. There is no ILMerge / ILRepack step.

## Two product lines, one package

The package ships one modern line for runtimes that support **.NET Standard 2.1**, and extra assemblies for frameworks
that are not compatible with it. Those assemblies are not a second library. Functional interfaces and
`FixedContextValue<T>` are the callback shape on every TFM. Which members stayed on the modern line is in
[`docs/api/compatibility.md`](docs/api/compatibility.md).

Extra TFMs are justified when they change what the **declared contract** or the **executing runtime** can do — not
merely because a framework number exists. `net470` and `net471` already restore without their own `lib/`. A later
runtime is worth a dedicated `lib/` when the implementation or the public surface must differ: BCL or runtime internals
that the library already special-cases per TFM, APIs that simply do not exist yet, or generic constraints the older
binary cannot express. The library adapts to those internal changes; do not treat a new framework number as an automatic
extra assembly.

## Dependencies

All package versions that vary by TFM live in [`Packages.props`](Packages.props). Do not add a `PackageReference` in an
intermediate `.csproj` “because this TFM needs it”. Put the pin next to the others so the matrix stays reviewable.

Rules that are easy to break:

- **The portable assemblies must not reference `System.Text.Json`.** .NET Standard 2.1 depends only on
  `System.Runtime.CompilerServices.Unsafe`. .NET Standard 2.0 adds `System.Memory` and
  `System.Reflection.Emit.Lightweight`. The graph runs from 2.0 to 2.1, so neither assembly may depend on a JSON API the
  next standard does not provide; that also keeps the two assemblies binary-compatible on this point. Mixing a .NET
  Standard 2.0 JSON package into the 2.1 assembly also fights **Mono Framework**’s long-term .NET Framework 4.5
  compatibility model. Dedicated Core / .NET Framework TFMs that can take JSON do so in `Packages.props`. JSON for
  `CString` on **Mono Framework** lives in the optional facade [`src/MonoFacades`](src/MonoFacades/README.md)
  (`Rxmxnx.PInvoke.Json`), not in the core package.
- **`netstandard2.0` is the portable binary for runtimes that are not compatible with 2.1.** Do not choose 2.0 on a host
  that already supports 2.1. Do not choose it for **.NET Core 2.1** either: that runtime does not implement .NET
  Standard 2.1, but it does expose native `Span<T>` on its public API.
- **.NET 8.0+ has no extra package dependencies** on the official path. Older dedicated TFMs pin `Unsafe`, `Memory`,
  `ValueTuple`, `Microsoft.Bcl.Memory`, and JSON at versions that match that host. Bump a pin only after restoring **and
  running** tests on that TFM: a newer `System.Memory` can change span layout or reject pointer-containing `T` in
  `Span<T>(void*, int)`.
- **Test hosts are not library TFMs.** `netcoreapp2.0` appears in test props so the **netstandard2.0** assembly is
  exercised on that runtime (OpenSSL 1.0, empty-array identity, `Span<T>` over pointer types). The package does not
  ship `netcoreapp2.0`.
- **Mono Framework 4.5-era references** use `ExcludeAssets` / `PrivateAssets` so a .NET Framework compiler does not pull
  a Core implementation of `Unsafe` into a 4.5 process. [`src/PackageReference.props`](src/PackageReference.props) is
  how sample and test apps consume either the four intermediates (`UsePackage` unset) or the packed DLL (GitHub
  Actions).

If a new dependency cannot be expressed under these rules, it does not belong in the core package. Facades, a companion
package, or a TFM-only reference are the outlets that already exist.

## Four modules, then one assembly

### Why four projects exist

Under [`src/Intermediate`](src/Intermediate/README.md):

| Project        | What it is for                                                                                                                                                                                                                                   |
|----------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Common**     | Shared kernel: typed pointers, fixed memory, functional interfaces, wrappers, `NativeUtilities`, `AotInfo` / `SystemInfo`, framework compatibility shims, localization, process-map inspection. Other modules are not allowed to duplicate this. |
| **Buffers**    | `BufferManager`, binary / non-binary spaces, metadata storage. Needs Common; nothing else should grow a second buffer allocator.                                                                                                                 |
| **CString**    | UTF-8 / ASCII `CString`, `CStringSequence`, `CStringBuilder`, marshallers. Needs Common (and buffer types where UTF-8 concatenation uses them).                                                                                                  |
| **Extensions** | Extension methods on BCL types (`Span<T>`, `String`, pointers, streams). Needs Common; must not become a second kernel.                                                                                                                          |

Tests map 1:1 (`Common.Tests`, `Buffers.Tests`, `CString.Tests`, `Extensions.Tests`). That isolation is why a change to
`ValPtr<T>` does not require re-running the multilingual `CString` corpus to get a first signal.

**Common looks large because it is the kernel**, not because four modules failed. Pointers, pinning, wrappers, OS/AOT
detection, and the shims that make `Span<T>` / `HashCode` / `Index` compile on older TFMs all have to live in one place
or every other module reimplements them. Treat new types as: does this belong to UTF-8, to stack buffers, or to “any
consumer of the package”? Only the last group goes in Common.

Intermediate projects compile with `InternalsVisibleTo` toward each other and toward the test assemblies. They define
`PACKAGE` **unset**, so `#if !PACKAGE` members (test helpers, extra coverage exclusions, .NET 9.0 extension methods that
the package later patches as instance methods) exist in the development build.

### How they become one DLL

[`package/Rxmxnx.PInvoke.Extensions`](package/Rxmxnx.PInvoke.Extensions/Rxmxnx.PInvoke.Extensions.csproj) does **not**
reference the intermediate DLLs as `ProjectReference` for the packed output. [
`package/Intermediate-to-Package.targets`](package/Intermediate-to-Package.targets):

1. Builds intermediates only far enough to generate global usings.
2. Includes every `.cs` file from the four trees into the package compilation (`IncludeIntermediateSources`).
3. Merges each module’s `ILLink.Substitutions.xml`, rewriting the intermediate assembly name to
   `Rxmxnx.PInvoke.Extensions`, and embeds one substitutions file.
4. Compiles with `PACKAGE` defined, signs the assembly, and packs XML docs + symbols.

The consumer therefore sees **one** strong-named assembly, one trimmer view, one XML documentation file. Internals that
were `InternalsVisibleTo` across four modules are internals of a single module; public surface is what ApiCompat
validates (`EnablePackageValidation`).

When you add a file, add it under the intermediate that owns the concept. It will appear in the package automatically.
When you add an `internal` helper that tests must call, remember: tests compile against the **intermediate** assemblies,
not against the packed DLL (except ApplicationTest on GitHub Actions, which restores the package).

### How convergence is achieved

The same sources are compiled for every TFM in [`src/Library.props`](src/Library.props). Behavior that the BCL does not
share is gated with `#if` (`NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER` for the historical modern callback surface,
`NET9_0_OR_GREATER` for `allows ref struct`, `NET7_0_OR_GREATER` for marshalling, and the FrameworkCompat types in
Common for APIs that simply do not exist yet).

That is not enough. **Declared** TFM surface and **executing** runtime diverge: desktop .NET Framework `Span<T>` is the
slow three-field layout; Mono executing a .NET Framework TFM can still be fast span; `System.Memory` on netcoreapp2.0
rejects pointer-containing `T` in `Span<T>(void*, int)` even though the same source is valid on 2.1. Unit tests
therefore run on each .NET / .NET Core host they can (`dotnet test /p:MultipleFrameworkTest=true`). Failures on an
“obsolete” host are how the portable implementation is proven, not a distraction from the modern line.

Application and Native AOT / Mono / WASM paths are a second net: [`src/ApplicationTest`](src/ApplicationTest/README.md)
and [`src/LegacyAppTest`](src/LegacyAppTest/README.md). Locally they reference intermediates; CI packs the branch and
consumes the NuGet so the IL patcher, substitutions, and package assets are what those apps actually load.

## Usings

Namespace imports live in the intermediate project (`<Using Include="..."/>`). A source file does not repeat them.
As a rule, every namespace is global.

A file-level `using` is an alias. Aliases of types in `Rxmxnx.PInvoke.Internal.FrameworkCompat` remain valid even though
that namespace is global: a compatibility type standing in for the framework type (`RuntimeHelpersCompat` as
`RuntimeHelpers`), or a short name for a type the file would otherwise spell in full. An alias that only repeats the
type's own name is unnecessary. An alias may also resolve an ambiguity between two types that share a name
(`System.Collections.IEnumerator` as `IEnumerator`).

The exception is a namespace of this repository that is new on a more recent TFM. It is absent from the older
compilations, so the file that needs it imports it under the same `#if` that declares the namespace.

## Code quality attributes

`SuppressMessage` and `ExcludeFromCodeCoverage` belong to the intermediate build, where Sonar and the coverage tools
run. Every one of them is wrapped in `#if !PACKAGE`. The package compilation defines `PACKAGE`, so those attributes are
absent from the shipped assembly.

`SuppressMessage` uses `SuppressMessageConstants` (`csharpsquid` and the check id). That type is compiled only when
`PACKAGE` is unset. Use the constants; do not hard-code the check id.

Sonar rule **S6640** reviews every `unsafe` type and every `unsafe` member. Mark the type, or the member when the type
is not already marked, with the same `#if`:

```csharp
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
```

## Code Access Security on .NET Framework

Desktop .NET Framework still compiles under transparency level 2. Code is transparent unless a member says otherwise.
Transparent code cannot contain unsafe pointers, cannot P/Invoke, and cannot call critical code. This library’s job is
pointer work, so the .NET Framework binaries and the `netstandard2.0` assembly (that binary also loads on Framework)
annotate the members that would otherwise be illegal. The attributes are omitted on every other TFM, including UAP:
those runtimes do not enforce this model. Gate every annotation with `#if NETFRAMEWORK || NETSTANDARD2_0`.

Two attributes, and they are not interchangeable:

- **`SecuritySafeCritical`** is the public bridge. A caller can use the member, and the member can do unverifiable work
  or call critical code. Put it on public methods, properties, and operators that read or write a pointer, span, or
  other unsafe state: `FuncPtr<TDelegate>.Pointer`, `CString` operations, the pointer extension methods.
- **`SecurityCritical`** is implementation that transparent code must not call. Put it on an internal type whose whole
  job is unverifiable (`ManagedMemoryManager<T>`, `ArrayMemoryManager<T>`, `Utf8Compat`), on a member that hands out a
  raw `ref` or owns a pin or native allocation, and on `ISerializable` constructors and `GetObjectData`. The formatter
  calls those last two; they stay critical even though the type itself is public.

A safe-critical member may call a critical one. A public member that consumers call is safe-critical, not critical. Do
not mark a public type `SecurityCritical`.

P/Invoke declarations that sit inside an already critical type use **`SuppressUnmanagedCodeSecurity`** under the same
`#if` (`MemoryInspector` / `Kernel32`). That attribute belongs on the `DllImport`, not on a public method.

When you add `unsafe`, `Unsafe.*`, or a `DllImport` to a member that compiles for those two symbols, annotate that
member in the `#if`.

## Why .NET 9.0+ IL is patched

Starting with .NET 9.0, `ValPtr<T>` and `ReadOnlyValPtr<T>` declare `where T : allows ref struct`. That is a real
product feature: a typed pointer to a `ref struct` is valid IL and valid C# 13.

`IFixedContext<T>.IDisposable` is a **class-based** nested interface. It cannot represent a context whose `T` is a
`ref struct`. If the instance method `ValPtr<T>.GetUnsafeFixedContext(int, IDisposable)` stayed in C# on .NET 9.0, the
compiler would have to emit a method whose return type is illegal for some `T` the type now allows. On .NET 8.0 and
earlier the method is ordinary C# (`#if !NET9_0_OR_GREATER` in `ValPtr.cs` / `ReadOnlyValPtr.cs`).

The package still needs that method on .NET 9.0+ **for non-ref-struct `T`**, so existing callers and the historical
`IFixed*` surface keep working. After the .NET 9.0+ assembly is compiled,
[`package/Rxmxnx.PInvoke.Extensions.IlPatcher`](package/Rxmxnx.PInvoke.Extensions.IlPatcher) (`ValuePointerPatchTask`)
runs from [`package/Package.targets`](package/Package.targets):

- Mono.Cecil injects `GetUnsafeFixedContext(int, IDisposable)` on `ValPtr<T>` and `ReadOnlyValPtr<T>`.
- The method body calls `FixedContext<T>.CreateDisposable` / `ReadOnlyFixedContext<T>.CreateDisposable` — the same as
  the C# that .NET 8.0 still compiles.
- XML documentation for those members is inserted into the packed `.xml`.
- Using the method with a `ref struct` `T` is a **runtime** `TypeLoadException`, which the remarks already state.

Development builds (`!PACKAGE && NET9_0_OR_GREATER`) expose the same operations as **extension methods** in
`ValuePointerExtensions`, so tests and apps that compile against intermediates do not depend on the patcher.

Do not patch IL for something C# can already express on that TFM. Do not add a second patcher path without a constraint
the compiler refuses. The patcher is the escape hatch for **binary and source compatibility** when a new generic
constraint (`allows ref struct`) makes an old member inexpressible, not a general code generator.

## Where to read next

| If you are changing…                            | Start here                                                                                                                         |
|-------------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------|
| Public API / TFM gates                          | [`docs/api/compatibility.md`](docs/api/compatibility.md), then the matching file under `src/Intermediate`                          |
| UTF-8 types                                     | [`src/Intermediate/Rxmxnx.PInvoke.CString.Intermediate/README.md`](src/Intermediate/Rxmxnx.PInvoke.CString.Intermediate/README.md) |
| Stack buffers / AOT metadata                    | [`src/Intermediate/Rxmxnx.PInvoke.Buffers.Intermediate/README.md`](src/Intermediate/Rxmxnx.PInvoke.Buffers.Intermediate/README.md) |
| Unit tests                                      | [`src/Test/README.md`](src/Test/README.md)                                                                                         |
| Packaged vs intermediate consumption, AOT, Mono | [`src/ApplicationTest/README.md`](src/ApplicationTest/README.md)                                                                   |
| Mono Framework JSON / facades                   | [`src/MonoFacades/README.md`](src/MonoFacades/README.md)                                                                           |
