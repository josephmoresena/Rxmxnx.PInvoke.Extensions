# Disclaimer

In these projects, `Rxmxnx.PInvoke.Extensions` is consumed via its *netstandard2.1 assembly* instead of using the
packaged `NuGet` distribution.

---

# Xamarin Apps Test

These applications are designed to showcase the capabilities and potential of using `Rxmxnx.PInvoke.Extensions` in
legacy Xamarin Android, iOS, and macOS applications.

## Considerations

* These projects require Xamarin and Mono SDKs.
* All projects use C# 9.0 syntax to remain compatible with the Xamarin/Mono SDK.
* Due to incompatibilities between the `Rxmxnx.PInvoke.Extensions` source code and Xamarin MSBuild, the C# compiler, and
  the Mono C# compiler, `Rxmxnx.PInvoke.Extensions` must be consumed via its compiled assembly.
* Replacing the `Rxmxnx.PInvoke.Extensions` assembly reference in `LegacyProject.props` with the official `NuGet`
  package works transparently.
* When these applications are built using the `Release` configuration, AOT compilation is enabled.
* AOT detection was deliberately implemented according to the particular characteristics of each runtime.

---

# WebAssembly App Test

This application is designed to showcase the capabilities and potential of using `Rxmxnx.PInvoke.Extensions` in legacy
Blazor WebAssembly applications.

## Considerations

* This project uses C# 9.0 syntax to remain compatible with Mono MSBuild and the Mono C# compiler.
* During the build process, the `_ResolveBlazorInputs` target is intercepted to remove an outdated and incompatible
  version of `System.Runtime.CompilerServices.Unsafe` from the Blazor WASM Base Class Library (BCL).

---

# Legacy App Test Core

The Core library project acts as a bridge between the existing Application Test code and the Legacy App test projects.

---

# LegacyProject.props

`LegacyProject.props` simplifies dependency management for:

* `System.Text.Json`
* `System.Runtime.CompilerServices.Unsafe`
* `Rxmxnx.PInvoke.Extensions`

It centralizes and standardizes how these packages and references are handled across legacy projects.