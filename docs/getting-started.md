# Getting started

Install the package, pick a target framework, and check that your runtime model (JIT, AOT, Mono, Unity, WebAssembly) is
covered.

## Installation

```bash
dotnet add package Rxmxnx.PInvoke.Extensions
```

```xml

<PackageReference Include="Rxmxnx.PInvoke.Extensions" Version="*"/>
```

Then:

```csharp
using Rxmxnx.PInvoke;
```

A first program in the **portable callback style** (this API shape compiles on every TFM the package ships). The snippet
uses **C# 11** (`u8`, `scoped`); see [language versions](#language-versions) if your project is older:

```csharp
CString hello = new(() => "Hello"u8);
Console.WriteLine(hello);                // Hello
Console.WriteLine(hello.IsNullTerminated); // True

readonly struct PrintLength : IFixedContextAction<Int32>
{
    public void Accept(scoped FixedContextValue<Int32> ctx)
    {
        Console.WriteLine(ctx.Values.Length); // 4
        Console.WriteLine(ctx.Pointer != IntPtr.Zero);
    }
}

Span<Int32> numbers = stackalloc Int32[] { 1, 2, 3, 4 };
numbers.WithSafeFixed(new PrintLength());
```

Without C# 11, build UTF-8 from a `Byte[]` or `ReadOnlySpan<Byte>` and omit `scoped` on `Accept` — that is how the
sample apps stay on C# 9 for Mono/Xamarin SDKs.

Next: [Capabilities](capabilities.md) for a tour, [Use cases](use-cases.md) for recipes, [API reference](api/README.md)
for types. Which members exist on which TFM is spelled out
in [Target frameworks and public API surface](api/compatibility.md).

## Language versions

Public APIs use generic constraints that older C# cannot express (`unmanaged`, `Enum`, `Delegate`, and related
combinations). That — not the package version — is the language floor.

| When                   | Language           | Why                                                                                                       |
|------------------------|--------------------|-----------------------------------------------------------------------------------------------------------|
| Any TFM                | **C# 7.3** minimum | `where T : unmanaged`, `where TEnum : unmanaged, Enum`, and similar constraints appear on public members. |
| Preferred on every TFM | **C# 11**          | UTF-8 `u8` literals (`new CString(() => "Hi"u8)`), `scoped` parameters on `ref struct` callbacks.         |
| **.NET 9.0 and later** | **C# 13**          | Many generics `allows ref struct` (`ValPtr<T>`, wrappers, callbacks).                                     |

The package is a C# library and also stays usable from **Visual Basic .NET**, with the smallest surface that language
can consume. See [Visual Basic .NET support](#visual-basic-net-support).

Samples in these guides assume C# 11 unless a snippet is marked otherwise.

## Support policy

For new work, use **.NET 8.0 and later**. If a new project stays on .NET Framework, use **4.7.2**. Which runtimes the
package covers, and which APIs stay on the modern line, is in
[Target frameworks and public API surface](api/compatibility.md).

| Target                     | Support            | Remarks                                                                          |
|----------------------------|--------------------|----------------------------------------------------------------------------------|
| .NET 10.0                  | Current LTS        |                                                                                  |
| .NET 9.0                   | Current            | Supports generic `ref struct` types with pointers and many APIs; requires C# 13. |
| .NET 8.0                   | LTS                | No additional package dependencies.                                              |
| .NET 7.0                   | Extended           | Supports static virtual members and source-generated marshalling.                |
| .NET 6.0                   | Extended LTS       | AOT detection on desktop platforms does not use reflection.                      |
| .NET 5.0 / .NET Core 3.x   | Modern legacy line | Provides support for `NativeLibrary`.                                            |
| .NET Standard 2.1          | Modern Portable    | Provides support for `RuntimeHelpers` and fast-span.                             |
| .NET Standard 2.0          | Portable fallback  |                                                                                  |
| .NET Framework 4.5.2–4.6   | Transition Legacy  |                                                                                  |
| .NET Framework 4.6.1       | Legacy             | Provides support for `System.Text.Json`.                                         |
| .NET Framework 4.6.2–4.7.2 | Supported          |                                                                                  |
| UAP 10.0.16299             | Supported          | Provides support for `System.Text.Json` and `RuntimeHelpers`.                    |
| .NET Core 2.1              | Extended           | Provides support for `System.Text.Json`, `RuntimeHelpers`, and fast-span.        |

## Framework support

What each assembly exposes, including shims and package dependencies, is in
[compatibility](api/compatibility.md). The notes below are only for a specific host.

<details>
<summary><strong>.NET Standard 2.0 / 2.1</strong> — Shims on the portable assembly</summary>

- Static virtual members: No. AOT detection should be performed via reflection.
- Generic `ref struct`: No.
- MemoryMarshal shims: `CreateReadOnlySpanFromNullTerminated`, `GetArrayDataReference`. Retrieving references to
  multidimensional array data should use static delegates; managed buffer registration should use buffer binding.
- Rune shims: `EncodeToUtf8`, `DecodeFromUtf8`, `DecodeFromUtf16` (CoreCLR implementations from .NET 6.0; simpler
  alternatives may be substituted).
- Enum shim: `Enum.GetName<T>` internally uses `Enum.GetName(Type, Object)`.
- Convert shim: `ToHexString`.

</details>

### Runtimes and platforms

<details>
<summary><strong>.NET (CoreCLR and Mono VM)</strong></summary>

Assemblies are compiled for each target framework from .NET 5.0 onward. The library adapts to the platforms those
specifications support.

Guaranteed runtimes:

- **CoreCLR** — default for .NET / .NET Core desktop.
- **Mono VM** — default for mobile platforms and Blazor WebAssembly.

</details>

<details>
<summary><strong>Unity</strong></summary>

Use the .NET Standard **2.1** assembly whenever the player supports it. It adapts to the internal Mono runtime and
supported platforms. Use the .NET Standard **2.0** assembly only if the player is still on that TFM.

Requirement: `System.Runtime.CompilerServices.Unsafe` **6.0 or later**. The .NET Standard 2.0 build of that package is
recommended.

See [AOT support](#unity-il2cpp) if you publish with IL2CPP.

</details>

<details>
<summary><strong>Xamarin (Android, iOS, macOS)</strong></summary>

Add the NuGet package to a legacy Xamarin project. That also references `System.Runtime.CompilerServices.Unsafe` 5.0
(assembly version 6.0). For new projects, use **6.1.2**. Prefer targeting **.NET Standard 2.1**.

Building Xamarin apps requires Visual Studio 2019 or Visual Studio 2019 for Mac.

</details>

<details>
<summary><strong>.NET Core 2.1/ 3.x</strong></summary>

Dedicated assemblies for .NET Core 2.1, 3.0, and 3.1.Native `System.Text.Json` and native library APIs differ slightly
between 2.1/3.0 and 3.1 because they ship different `System.Text.Json` and `System.Runtime.CompilerServices.Unsafe`
versions.

</details>

<details>
<summary><strong>Blazor WebAssembly 3.2</strong></summary>

The .NET Standard 2.1 assembly runs on the original WASM Mono runtime. Add the NuGet package (and
`System.Runtime.CompilerServices.Unsafe` 5.0). Intercept publish so this package’s assembly is used instead of the copy
bundled with the WASM runtime.

</details>

<details>
<summary><strong>Mono Framework</strong></summary>

Compatible via .NET Standard 2.1 (prefer this) or, when 2.1 is unavailable, .NET Standard 2.0 / .NET Framework 4.5.2,
using .NET Framework 4.5 facades and `System.Runtime.CompilerServices.Unsafe` 5.0. See [
`src/MonoFacades/README.md`](../src/MonoFacades/README.md) if you need `System.Text.Json` on classic Mono without mixing
.NET Standard 2.0 dependencies into the core package.

</details>

## AOT support

On every package version, the library is compatible with **Mono AOT**, **IL2CPP**, **Native AOT**, **.NET Native**, and
**ReadyToRun**. ReadyToRun is a first-class CoreCLR mode. Some AOT modes are more optimized than others. .NET Native on
UWP is described with [UAP 10.0.16299](api/compatibility.md#legacy-framework-transition-vs-dedicated).

Native AOT is the production CoreCLR AOT mode from .NET 7.0. The obsolete reflection-free Native AOT mode is supported.

The package avoids reflection. It is used to auto-compose buffers and to optimize the Marvin UTF-8 hash calculation.
Neither blocks AOT. Without reflection, the built-in Marvin algorithm is not used. How to register buffers on AOT is in
[Buffers](api/buffers.md#aot-and-registration).

AOT detection is described with [`AotInfo`](api/utilities.md#aotinfo).

The library favors **statically reachable code** over reflection, so it works with the classic Mono Linker and modern
ILLink.

### Mono AOT

Supported in Full, Hybrid, and LLVM modes, limited only by the target platform and runtime. That includes Mono
Framework, Xamarin, Blazor WebAssembly 3.x, Unity, and newer Mono-based mobile / Blazor runtimes on .NET 5.0+.

### ReadyToRun (R2R)

Supported on CoreCLR from .NET Core 3.0 through current .NET.

### Native AOT

Minimal reflection avoids N+1 patterns; most functionality is statically compiled.

### Unity IL2CPP

The .NET Standard 2.1 assembly works with IL2CPP enabled.

Multidimensional array flattening is a first-class feature on every TFM
(see [Capabilities](capabilities.md#flatten-multidimensional-arrays-without-copying)). It is compiled in statically.
**IL2CPP may generate invalid C++ identifiers for array indices greater than 17** if the linker does not remove those
members. The following utility rewrites the generated C++ in place; recompile in Unity afterward so it reuses the fixed
files.

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

The package is written for C#, but it still tries to remain usable from Visual Basic .NET. VB cannot express most
`ref` / `Span<T>` APIs, so that support is **the smallest practical surface**, not a second language port:

- `Rxmxnx.PInvoke.VisualBasic` redeclares buffer delegates (`VbScopedBufferAction<T>`, `VbScopedBufferFunc<…>`).
- `BufferManager.VisualBasic` exposes `Alloc` methods that wrap those delegates. They add a small overhead and are
  intended for VB only.

Prefer C# for new interop code. Use the VB helpers only where the language cannot call the C# surface.

## Feature switches (.NET 8.0+)

Buffer composition and preload caps are in [Buffers](api/buffers.md#aot-and-registration).

## Next steps

1. Skim [Capabilities](capabilities.md) to match your problem to an area of the library.
2. If you target .NET Framework, UWP, or .NET Standard 2.0, read [compatibility](api/compatibility.md) before copying a
   sample that uses delegate `WithSafeFixed`.
3. Copy a recipe from [Use cases](use-cases.md).
4. Keep the [API map](api/README.md) open while you type.
