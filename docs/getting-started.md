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

A first program in the **portable callback style** (this API shape compiles on every TFM the package ships). The snippet uses **C# 11** (`u8`, `scoped`); see [language versions](#language-versions) if your project is older:

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

Without C# 11, build UTF-8 from a `Byte[]` or `ReadOnlySpan<Byte>` and omit `scoped` on `Accept` — that is how the sample apps stay on C# 9 for Mono/Xamarin SDKs.

Next: [Capabilities](capabilities.md) for a tour, [Use cases](use-cases.md) for recipes, [API reference](api/README.md) for types. Which members exist on which TFM is spelled out in [Target frameworks and public API surface](api/compatibility.md).

## Language versions

Public APIs use generic constraints that older C# cannot express (`unmanaged`, `Enum`, `Delegate`, and related combinations). That — not the package version — is the language floor.

| When | Language | Why |
| --- | --- | --- |
| Any TFM | **C# 7.3** minimum | `where T : unmanaged`, `where TEnum : unmanaged, Enum`, and similar constraints appear on public members. |
| Preferred on every TFM | **C# 11** | UTF-8 `u8` literals (`new CString(() => "Hi"u8)`), `scoped` parameters on `ref struct` callbacks. |
| **.NET 9.0 and later** | **C# 13** | Many generics `allows ref struct` (`ValPtr<T>`, wrappers, callbacks). |

The package is a C# library and also stays usable from **Visual Basic .NET**, with the smallest surface that language can consume. See [Visual Basic .NET support](#visual-basic-net-support).

Samples in these guides assume C# 11 unless a snippet is marked otherwise.

## Support policy

This package **officially supports .NET 8.0 and later**. Until **2.9.5**, compatibility was limited to modern runtimes that support **.NET Standard 2.1**. Later versions still include that baseline and add assemblies so existing products on older hosts keep compiling — as **modern, retrocompatible code**, not as a second-class mode.

| Target | Support |
| --- | --- |
| .NET 10.0 | Current LTS |
| .NET 9.0 | Current — generic `ref struct` on pointers and many APIs; consumers should use **C# 13** |
| .NET 8.0 | LTS — no extra package dependencies |
| .NET 7.0 | Extended — static virtual members, source-generated marshalling |
| .NET 6.0 | Extended LTS |
| .NET 5.0 / .NET Core 3.x | Legacy — dedicated Core binaries; original modern API (until 2.9.5) |
| .NET Standard 2.1 | Portable — Xamarin, Unity, Mono; original modern API; shims, `Unsafe` 5.0 |
| .NET Core 2.1 | Dedicated Core binary from versions after 2.9.5 (no delegate pinning helpers) |
| .NET Standard 2.0 | Portable — **only when the engine cannot target 2.1** |
| .NET Framework 4.5.2 / 4.6 | Transition netfx (pre-Standard 2.0); no `System.Text.Json` |
| .NET Framework 4.6.1–4.7.2 | Dedicated netfx binaries for production Framework apps; 4.6.1 already has JSON; `net470`/`net471` have no unique `lib/` |
| UWP (`uap10.0.16299`) | Dedicated UWP binary for production UWP apps; runtime may use fast span |

Until 2.9.5 the TFMs were .NET Standard 2.1 and .NET Core 3.0+. Those assemblies keep the historical delegate overloads and nested `IFixedMemory` / `IFixedContext<T>` `IDisposable` helpers. TFMs added after that ceiling do not; they use functional interfaces and `FixedContextValue<T>`. The `IFixed*` interfaces themselves are still public on every TFM. Details: [compatibility](api/compatibility.md).

Among the original modern TFMs, newer frameworks inherit the older surface and add members; they do not break existing call sites. The extended TFMs are a **narrower** public API on purpose: those helpers were not retrofitted onto netfx, netstandard2.0, or UWP.

## Framework support

<details>
<summary><strong>.NET Standard 2.1</strong> — Portable (Xamarin, Unity, Mono)</summary>

- Prefer this portable TFM whenever the engine supports it.
- Static virtual members: No. AOT detection should be performed via reflection.
- Generic `ref struct`: No.
- MemoryMarshal shims: `CreateReadOnlySpanFromNullTerminated`, `GetArrayDataReference`. Retrieving references to multidimensional array data should use static delegates; managed buffer registration should use buffer binding.
- Rune shims: `EncodeToUtf8`, `DecodeFromUtf8`, `DecodeFromUtf16` (CoreCLR implementations from .NET 6.0; simpler alternatives may be substituted).
- Enum shim: `Enum.GetName<T>` internally uses `Enum.GetName(Type, Object)`.
- Convert shim: `ToHexString`.
- Dependencies: `System.Runtime.CompilerServices.Unsafe` 5.0.

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
- Updated dependencies: `System.Runtime.CompilerServices.Unsafe` 6.0, `System.Text.Json` 6.0.11.

</details>

<details>
<summary><strong>.NET 5.0</strong> — Legacy</summary>

- Inherits from .NET Core 3.1.
- Enum and Convert: native implementations.

</details>

<details>
<summary><strong>.NET 6.0</strong> — LTS (Extended)</summary>

- Inherits from .NET 5.0.
- Updated dependencies: `System.Runtime.CompilerServices.Unsafe` 6.1.2, `System.Text.Json` 8.0.6.

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
- Consumers should use **C# 13**.

</details>

<details>
<summary><strong>.NET 10.0</strong> — LTS (Current)</summary>

- Inherits from .NET 9.0.

</details>

<details>
<summary><strong>.NET Standard 2.0</strong> — Portable (only if 2.1 is unavailable)</summary>

- Same job as netstandard2.1 for **Xamarin, Unity, Mono** when the player or SDK cannot target Standard 2.1. **Do not use this TFM on an engine that already supports 2.1.**
- Functional interfaces and `FixedContextValue<T>`, not delegate `WithSafeFixed` / nested `IFixed*.IDisposable`.
- Dependencies: `System.Memory` 4.5.5, `System.Runtime.CompilerServices.Unsafe` 5.0, `System.Reflection.Emit.Lightweight` 4.7.0.

</details>

<details>
<summary><strong>.NET Core 2.1</strong> — Dedicated Core binary</summary>

- Own `lib/` from versions after 2.9.5; same callback gap as netstandard2.0.
- Adds `System.Text.Json` 5.0.2. No `NativeLibrary` (that arrives in .NET Core 3.0).
- Dependencies: `Microsoft.NETCore.App` 2.1.30 (private), `Unsafe` 5.0, `System.Text.Json` 5.0.2.

</details>

<details>
<summary><strong>.NET Framework 4.5.2 / 4.6</strong> — Transition (pre-Standard 2.0)</summary>

- Dedicated netfx binaries for Framework before it implemented .NET Standard 2.0.
- Public API stays on that TFM; span operations follow the **runtime** layout. On desktop CLR they are typically the slower three-field span; on Mono hosting the same TFM they can be the fast path. See [span efficiency](api/compatibility.md#span-efficiency).
- No built-in `System.Text.Json` — so a 4.5-era Mono story is not mixed with Standard 2.0 JSON.
- Dependencies: `System.Memory` 4.5.5, `Unsafe` 5.0, `System.Runtime.InteropServices.RuntimeInformation` 4.3.0, `System.ValueTuple` 4.5.0.
- No built-in `CString` JSON converter (`[JsonConverter]` starts at net461 / .NET Core).

</details>

<details>
<summary><strong>.NET Framework 4.6.1</strong> — Dedicated netfx</summary>

- A production Framework target, not a stopgap. Also a natural step toward Mono or current .NET with the same APIs.
- `System.Text.Json` 6.0.11 and the `CString` JSON converter. Closer to the **netstandard2.0** extras than 4.5.2/4.6.
- Dependencies: `Microsoft.Bcl.AsyncInterfaces` 6.0.0, `System.Memory` 4.5.5, `Unsafe` 6.0, `RuntimeInformation` 4.3.0, `ValueTuple` 4.5.0.

</details>

<details>
<summary><strong>.NET Framework 4.6.2 / 4.7 / 4.7.1 / 4.7.2</strong> — Dedicated netfx</summary>

- Production Framework binaries. Functional-interface surface (the until-2.9.5 delegates were not brought over).
- Span work is still slower on desktop CLR than on modern .NET; cheaper when the process is Mono.
- Dependencies: `Microsoft.Bcl.Memory` 10.0.11, `Microsoft.Bcl.HashCode` 6.0.0, `System.Text.Json` 10.0.11. net462 also has `ValueTuple` 4.6.2; net462/net470 also have `RuntimeInformation` 4.3.0.
- `net470` and `net471` are package target frameworks only — they do not ship a unique assembly.

</details>

<details>
<summary><strong>UWP 10.0.16299</strong> — Dedicated UWP binary</summary>

- Aimed at production UWP apps. The public API may look like `System.Memory` / `Microsoft.Bcl.Memory`; the runtime can still use **fast span**.
- Functional-interface surface (the until-2.9.5 delegates were not brought over).
- Dependencies: `Microsoft.Bcl.Memory` 9.0.19, `Microsoft.Bcl.HashCode` 6.0.0, `System.Text.Json` 6.0.11, plus private UWP compiler packs.

</details>

The full member-level split is in [Target frameworks and public API surface](api/compatibility.md).

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

Use the .NET Standard **2.1** assembly whenever the player supports it. It adapts to the internal Mono runtime and supported platforms. Use the .NET Standard **2.0** assembly only if the player is still on that TFM.

Requirement: `System.Runtime.CompilerServices.Unsafe` **6.0 or later**. The .NET Standard 2.0 build of that package is recommended.

See [AOT support](#unity-il2cpp) if you publish with IL2CPP.

</details>

<details>
<summary><strong>Xamarin (Android, iOS, macOS)</strong></summary>

Add the NuGet package to a legacy Xamarin project. That also references `System.Runtime.CompilerServices.Unsafe` 5.0 (assembly version 6.0). For new projects, use **6.1.2**. Prefer targeting **.NET Standard 2.1**.

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

Compatible via .NET Standard 2.1 (prefer this) or, when 2.1 is unavailable, netstandard2.0 / net452, using .NET Framework 4.5 facades and `System.Runtime.CompilerServices.Unsafe` 5.0. See [`src/MonoFacades/README.md`](../src/MonoFacades/README.md) if you need `System.Text.Json` on classic Mono without mixing netstandard2.0 dependencies into the core package.

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

The package is written for C#, but it still tries to remain usable from Visual Basic .NET. VB cannot express most `ref` / `Span<T>` APIs, so that support is **the smallest practical surface**, not a second language port:

- `Rxmxnx.PInvoke.VisualBasic` redeclares buffer delegates (`VbScopedBufferAction<T>`, `VbScopedBufferFunc<…>`).
- `BufferManager.VisualBasic` exposes `Alloc` methods that wrap those delegates. They add a small overhead and are intended for VB only.

Prefer C# for new interop code. Use the VB helpers only where the language cannot call the C# surface.

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
2. If you target .NET Framework, UWP, or .NET Standard 2.0, read [compatibility](api/compatibility.md) before copying a sample that uses delegate `WithSafeFixed`.
3. Copy a recipe from [Use cases](use-cases.md).
4. Keep the [API map](api/README.md) open while you type.
