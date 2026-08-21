# Getting started

Install the package, pick a target framework, and check that your runtime model (JIT, AOT, Mono, Unity, WebAssembly) is covered.

## Installation

```bash
dotnet add package Rxmxnx.PInvoke.Extensions
```

```xml
<PackageReference Include="Rxmxnx.PInvoke.Extensions" Version="*" />
```

Then:

```csharp
using Rxmxnx.PInvoke;
```

A first program:

```csharp
CString hello = new(() => "Hello"u8);
Console.WriteLine(hello);                // Hello
Console.WriteLine(hello.IsNullTerminated); // True

Span<Int32> numbers = stackalloc Int32[] { 1, 2, 3, 4 };
numbers.WithSafeFixed(static (in IFixedContext<Int32> ctx) =>
{
    Console.WriteLine(ctx.Values.Length); // 4
    Console.WriteLine(ctx.Pointer != IntPtr.Zero);
});
```

Next: [Capabilities](capabilities.md) for a tour, [Use cases](use-cases.md) for recipes, [API reference](api/README.md) for types.

The lambda passed to `WithSafeFixed` is the compatibility form. New code often uses a `readonly struct` that implements `IFixedContextAction<T>` (or a read-only counterpart) so the callback can hold state without allocating. See [Functional interfaces](api/functional-interfaces.md).

## Support policy

This package **officially supports .NET 8.0 and later**. It also ships assemblies for older frameworks so existing projects keep compiling:

| Target | Support |
| --- | --- |
| .NET 10.0 | Current LTS |
| .NET 9.0 | Current — generic `ref struct` on pointers and many APIs |
| .NET 8.0 | LTS — no extra package dependencies |
| .NET 7.0 | Extended — static virtual members, source-generated marshalling |
| .NET 6.0 | Extended LTS |
| .NET 5.0 / .NET Core 3.x | Legacy |
| .NET Standard 2.1 | Limited — shims for newer APIs, extra dependencies |

The package guarantees **binary and source compatibility** across those targets. Newer TFMs inherit the older surface and add features; they do not break existing call sites.

## Framework support

<details>
<summary><strong>.NET Standard 2.1</strong> — Limited Support</summary>

- Static virtual members: No. AOT detection should be performed via reflection.
- Generic `ref struct`: No.
- MemoryMarshal shims: `CreateReadOnlySpanFromNullTerminated`, `GetArrayDataReference`. Retrieving references to multidimensional array data should use static delegates; managed buffer registration should use buffer binding.
- Rune shims: `EncodeToUtf8`, `DecodeFromUtf8`, `DecodeFromUtf16` (CoreCLR implementations from .NET 6.0; simpler alternatives may be substituted).
- Enum shim: `Enum.GetName<T>` internally uses `Enum.GetName(Type, Object)`.
- Convert shim: `ToHexString`.
- Dependencies: `System.Runtime.CompilerServices.Unsafe` 5.0, `System.Collections.Immutable` 5.0.

</details>

<details>
<summary><strong>.NET Core 3.0</strong> — Legacy (Limited)</summary>

- Inherits from .NET Standard 2.1.
- Adds `System.Text.Json` and `NativeLibrary`.
- Rune: native implementation.
- Dependencies: same as .NET Standard 2.1, plus `System.Text.Json` 5.0.2.

</details>

<details>
<summary><strong>.NET Core 3.1</strong> — Legacy</summary>

- Inherits from .NET Core 3.0.
- Updated dependencies: `System.Runtime.CompilerServices.Unsafe` 6.0, `System.Collections.Immutable` 6.0, `System.Text.Json` 6.0.11.

</details>

<details>
<summary><strong>.NET 5.0</strong> — Legacy</summary>

- Inherits from .NET Core 3.1.
- Enum and Convert: native implementations.

</details>

<details>
<summary><strong>.NET 6.0</strong> — LTS (Extended)</summary>

- Inherits from .NET 5.0.
- Updated dependencies: `System.Runtime.CompilerServices.Unsafe` 6.1.2, `System.Collections.Immutable` 8.0, `System.Text.Json` 8.0.5.

</details>

<details>
<summary><strong>.NET 7.0</strong> — Extended</summary>

- Inherits from .NET 6.0.
- Static virtual members: yes.
- Source-generated marshalling for `CString`, `CStringSequence`, `ValPtr<T>`, `ReadOnlyValPtr<T>`, and `FuncPtr<TDelegate>`.

</details>

<details>
<summary><strong>.NET 8.0</strong> — LTS</summary>

- Inherits from .NET 7.0.
- No extra package dependencies.

</details>

<details>
<summary><strong>.NET 9.0</strong> — Current</summary>

- Inherits from .NET 8.0.
- Generic `ref struct` (`allows ref struct`) on value-type pointers and many generic APIs.
- Value-type pointers support `ref struct` generics; some methods are implemented in IL because of C# compiler restrictions.

</details>

<details>
<summary><strong>.NET 10.0</strong> — LTS (Current)</summary>

- Inherits from .NET 9.0.

</details>

### Runtimes and platforms

<details>
<summary><strong>.NET (CoreCLR and Mono VM)</strong></summary>

Assemblies are compiled for each target framework from .NET 5.0 onward. The library adapts to the platforms those specifications support.

Guaranteed runtimes:

- **CoreCLR** — default for .NET / .NET Core desktop.
- **Mono VM** — default for mobile platforms and Blazor WebAssembly.

</details>

<details>
<summary><strong>Unity</strong></summary>

Use the .NET Standard 2.1 assembly. It adapts to the internal Mono runtime and supported platforms.

Requirement: `System.Runtime.CompilerServices.Unsafe` **6.0 or later**. The .NET Standard 2.0 build of that package is recommended.

See [AOT support](#unity-il2cpp) if you publish with IL2CPP.

</details>

<details>
<summary><strong>Xamarin (Android, iOS, macOS)</strong></summary>

Add the NuGet package to a legacy Xamarin project. That also references `System.Runtime.CompilerServices.Unsafe` 5.0 (assembly version 6.0). For new projects, use **6.1.2**.

Building Xamarin apps requires Visual Studio 2019 or Visual Studio 2019 for Mac.

</details>

<details>
<summary><strong>.NET Core 3.x</strong></summary>

Dedicated assemblies for .NET Core 3.0 and 3.1. The .NET Standard 2.1 assembly remains a fallback. Native `System.Text.Json` and native library APIs differ slightly between 3.0 and 3.1 because they ship different `System.Text.Json` versions.

</details>

<details>
<summary><strong>Blazor WebAssembly 3.2</strong></summary>

The .NET Standard 2.1 assembly runs on the original WASM Mono runtime. Add the NuGet package (and `System.Runtime.CompilerServices.Unsafe` 5.0). Intercept publish so this package’s assembly is used instead of the copy bundled with the WASM runtime.

</details>

<details>
<summary><strong>Mono Framework</strong></summary>

Compatible via .NET Standard 2.1, using .NET Framework 4.5 facades and `System.Runtime.CompilerServices.Unsafe` 5.0. See [`src/MonoFacades/README.md`](../src/MonoFacades/README.md) if you need `System.Text.Json` on classic Mono without mixing netstandard2.0 dependencies into the core package.

</details>

## AOT support

The package is AOT-friendly. Features support Mono AOT and Native AOT, including full AOT and the obsolete reflection-free mode.

Reflection is required only for:

- Buffer preparation and auto-composition when no binary buffer is registered.
- AOT detection when targeting **.NET 5.0 or earlier**.
- AOT detection on **.NET 7.0 and later** when running on **Web or Mobile platforms** only.

The library favors **statically reachable code** over reflection, so it works with the classic Mono Linker and modern ILLink.

### Mono AOT

Supported in Full, Hybrid, and LLVM modes, limited only by the target platform and runtime. That includes Mono Framework, Xamarin, Blazor WebAssembly 3.x, Unity, and newer Mono-based mobile / Blazor runtimes on .NET 5+.

### ReadyToRun (R2R)

Supported on CoreCLR from .NET Core 3.0 through current .NET.

### Native AOT

Supported on CoreCLR Native AOT from .NET 7.0 onward. Minimal reflection avoids N+1 patterns; most functionality is statically compiled. Early reflection-free mode is supported with limited functionality.

### Unity IL2CPP

The .NET Standard 2.1 assembly works with IL2CPP enabled.

Multidimensional array flattening is compiled in statically. **IL2CPP may generate invalid C++ identifiers for array indices greater than 17** if the linker does not remove those members. The following utility rewrites the generated C++ in place; recompile in Unity afterward so it reuses the fixed files.

```csharp
if (args.Length != 1)
{
    Console.WriteLine("Invalid IL2CPP source code folder.");
    return;
}

Dictionary<Char, String> map = new()
{
    { (Char)(18 + 'i'), "a" },
    { (Char)(19 + 'i'), "b" },
    { (Char)(20 + 'i'), "c" },
    { (Char)(21 + 'i'), "d" },
    { (Char)(22 + 'i'), "e" },
    { (Char)(23 + 'i'), "f" },
    { (Char)(24 + 'i'), "g" },
    { (Char)(25 + 'i'), "h" },
    { (Char)(26 + 'i'), "_i" },
    { (Char)(27 + 'i'), "_j" },
    { (Char)(28 + 'i'), "_k" },
    { (Char)(29 + 'i'), "_l" },
    { (Char)(30 + 'i'), "_m" },
    { (Char)(31 + 'i'), "_n" },
};
String[] replacements = ["l2cpp_array_size_t {0}", "{0}, {0}Bound", "{0}Bound + {0}"];
foreach (String sourceCodeFile in new DirectoryInfo(args[0]).GetFiles("*.cpp").Select(f => f.FullName))
{
    String sourceCodeContent = await File.ReadAllTextAsync(sourceCodeFile);
    Boolean modified = false;
    foreach (Char invalidBound in map.Keys)
    {
        foreach (String replacement in replacements)
        {
            String original = String.Format(replacement, invalidBound);
            if (!sourceCodeContent.Contains(original, StringComparison.OrdinalIgnoreCase)) continue;
            String updated = String.Format(replacement, map[invalidBound]);
            sourceCodeContent = sourceCodeContent.Replace(original, updated, StringComparison.OrdinalIgnoreCase);
            modified = true;
        }
    }
    if (modified)
    {
        await File.WriteAllTextAsync(sourceCodeFile, sourceCodeContent);
        Console.WriteLine($"{sourceCodeFile} fixed.");
    }
}
```

## Visual Basic .NET support

Some APIs are not directly usable from Visual Basic because of language limitations around `ref`/`span`. The `Rxmxnx.PInvoke.VisualBasic` namespace provides equivalent delegates. `BufferManager.VisualBasic` exposes `Alloc` methods with a small overhead; they are intended for VB only.

## Feature switches (.NET 8+)

| Switch | Effect |
| --- | --- |
| `PInvoke.DisableBufferAutoComposition` | Disables runtime composition of binary buffer metadata. |
| `PInvoke.BootstrapBufferStorage.Minimal` | Caps binary capacity at 31 elements. |
| `PInvoke.BootstrapBufferStorage.Medium` | Caps binary capacity at 127 elements. |
| `PInvoke.BootstrapBufferStorage.Limited` | Caps binary capacity at 2047 elements. |
| `PInvoke.BootstrapBufferStorage.Extended` | Uses managed-buffer-based storage of 2047 elements and allows larger metadata. |

On .NET 8.0 and later the default storage is still the mechanism that is **not** based on the managed buffer infrastructure. See [Buffers](api/buffers.md) for when to register or prepare metadata.

## Next steps

1. Skim [Capabilities](capabilities.md) to match your problem to an area of the library.
2. Copy a recipe from [Use cases](use-cases.md).
3. Keep the [API map](api/README.md) open while you type.
