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

## Xamarin.Mac on Apple Silicon

`MacAppTest` can target `x86_64` on an arm64 Mac with Xcode 27. `AppleSilicon.targets`, next to `MacAppTest.csproj`, is
imported only when the system dyld cache `dyld_shared_cache_arm64e` exists. That file belongs to the OS, not to
Rosetta, so the import still happens when Rosetta is absent and MSBuild is the arm64 Mono. On an Intel Mac the cache is
not there, and the targets is never imported.

When `XamMacArch` is `x86_64`, the targets does two things:

* It passes `--link_flags=-Wl,-rpath,/usr/lib/swift` through `MonoBundlingExtraArgs`. With `LinkMode` set to `None`,
  Xcode 27 records `@rpath/libswiftCoreMedia.dylib` and `mmp` writes no `LC_RPATH`.
* It prepends `MacAppTest/tools` to `PATH` before the native compile. Mono assembles with `-arch x86_64` and links with
  `clang --shared` without `-arch`, which on arm64 produces an empty dylib. `tools/clang` leaves a command unchanged
  when `-arch` is already present, and adds `-arch x86_64` when linking an x86_64 object. Everything else runs through
  `/usr/bin/clang`.

`mmp` still calls `lipo -extract_family`, which Xcode 27 no longer accepts. From `MacAppTest`:

```bash
./tools/patch-lipo
```

The script runs only on arm64 with Xcode 27 and asks for an administrator password to write into the toolchain. It
copies the binary to `lipo.real` and leaves a script in place of `lipo`. If the patch is already applied, or this
machine is not that case, it exits without changing anything. It is harmless because it only intercepts the call:
`-extract_family` is replaced with `-thin`, and `lipo.real` runs with the rest of the command line unchanged.

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