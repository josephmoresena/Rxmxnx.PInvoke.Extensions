[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=alert_status)![Bugs](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=bugs)![Coverage](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=coverage)![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=ncloc)![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=reliability_rating)![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=sqale_rating)![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)
[![Build Status](https://img.shields.io/github/actions/workflow/status/josephmoresena/Rxmxnx.PInvoke.Extensions/build.yml?style=flat-square)](https://github.com/josephmoresena/Rxmxnx.PInvoke.Extensions/actions/workflows/build.yml)
[![License](https://img.shields.io/github/license/josephmoresena/Rxmxnx.PInvoke.Extensions?style=flat-square)](LICENSE.md)
[![NuGet](https://img.shields.io/nuget/v/Rxmxnx.PInvoke.Extensions)![Downloads](https://img.shields.io/nuget/dt/Rxmxnx.PInvoke.Extensions?style=flat-square&color=blue)](https://www.nuget.org/packages/Rxmxnx.PInvoke.Extensions/)
[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/josephmoresena/Rxmxnx.PInvoke.Extensions)

# Rxmxnx.PInvoke.Extensions

Safe, typed, and allocation-conscious interop for .NET — from Native AOT to Mono, Unity, and WebAssembly.

`Rxmxnx.PInvoke.Extensions` is the next generation of a library that makes native memory, UTF-8 text, and P/Invoke feel like regular .NET code. You keep pointer intent in your signatures, pin memory only for as long as a callback or `using` scope lasts, and work with UTF-8 the way native APIs already do.

The goal is simple: **interop without spreading `unsafe`**, and **performance without giving up lifetime safety**.

```csharp
CString message = new(() => "Hello from .NET"u8);
Console.WriteLine(message); // Hello from .NET

message.WithSafeFixed(new UseUtf8());

readonly struct UseUtf8 : IReadOnlyFixedContextAction<Byte>
{
    public void Accept(scoped ReadOnlyFixedContextValue<Byte> ctx)
    {
        // ctx.Pointer stays valid for this entire call.
    }
}
```

## Why this generation

Earlier versions already offered typed pointers, UTF-8 strings, and scoped pinning. This generation goes further:

- **Functional interfaces** replace most delegate-based callbacks. State lives on a `readonly struct`, so hot paths avoid extra allocations and work naturally with `ref struct` values.
- **Value-type fixed contexts** (`FixedContextValue<T>`, `FixedPointerValue`) keep pinning, spans, and typed pointers in a single scoped value.
- **Native heap with .NET lifetimes** through `NativeUtilities.HeapAlloc<T>()`, released by `IDisposable` instead of paired alloc/free calls.
- **First-class UTF-8** with `CString`, `CStringSequence`, and `CStringBuilder` — including source-generated marshalling on .NET 7+.
- **Stack-first temporary storage** with `BufferManager` and `ScopedBuffer<T>` for parsers, serializers, and high-throughput pipelines.

If your code talks to native libraries, serializes UTF-8, reinterprets binary layouts, or has to stay trim/AOT-friendly, this package is built for that job.

## Start here

| I want to… | Go to |
| --- | --- |
| Install the package and see a first example | [Getting started](docs/getting-started.md) |
| Understand what the library can do | [Capabilities](docs/capabilities.md) |
| See real scenarios | [Use cases](docs/use-cases.md) |
| Look up types and APIs | [API reference](docs/api/README.md) |
| Browse the full documentation index | [Documentation](docs/README.md) |

```bash
dotnet add package Rxmxnx.PInvoke.Extensions
```

Officially supported on **.NET 8.0 and later**. Binary and source compatibility is maintained from **.NET Standard 2.1** through **.NET 10.0**, including CoreCLR, Mono, Native AOT, Unity, Xamarin, and Blazor WebAssembly. See [framework and AOT support](docs/getting-started.md#framework-support) for the details.

## Capabilities at a glance

- **UTF-8 that matches native APIs.** `CString` wraps managed buffers, UTF-8 literals, or unmanaged pointers. `CStringSequence` stores null-terminated argument/environment lists. `CStringBuilder` is the UTF-8 counterpart of `StringBuilder`.
- **Typed pointers without `unsafe`.** `ValPtr<T>`, `ReadOnlyValPtr<T>`, and `FuncPtr<TDelegate>` keep pointer meaning visible in P/Invoke signatures.
- **Scoped fixed memory.** `WithSafeFixed` pins spans, strings, and references only for the duration of a callback or `using` block, then exposes them as spans, pointers, or typed contexts.
- **Binary views with no extra copies.** `AsBytes`, `AsValues`, `ToBytes`, and `ToValue` reinterpret memory you already own.
- **Stack-backed references.** `BufferManager` allocates object or value buffers on the stack when possible, falling back to the heap only when needed.
- **Runtime awareness.** `AotInfo` and `SystemInfo` help you adapt to Native AOT, Mono, WebAssembly, and OS differences without scattering `#if` everywhere.

A longer tour lives in [Capabilities](docs/capabilities.md). Concrete recipes live in [Use cases](docs/use-cases.md).

## When to use it

Use this library when the problem benefits from explicit memory intent, scoped lifetimes, or low-allocation data access:

| Scenario | Start with |
| --- | --- |
| UTF-8 text for native APIs, JSON, gRPC, or ASP.NET | `CString`, `CStringSequence`, `CStringBuilder` |
| Typed native pointers without spreading `unsafe` | `ValPtr<T>`, `ReadOnlyValPtr<T>`, `FuncPtr<TDelegate>` |
| Pin memory only for a callback or `using` scope | `WithSafeFixed`, `IFixedContext<T>`, `FixedContextValue<T>` |
| Native heap with .NET disposal | `NativeUtilities.HeapAlloc<T>()` |
| Reinterpret or hash binary layouts | `AsBytes`, `AsValues`, `ToBytes`, `ToValue` |
| Stack-first temporary storage | `BufferManager`, `ScopedBuffer<T>` |
| AOT / Mono / platform checks | `AotInfo`, `SystemInfo`, `IsImageMethod`, `IsLiteral` |

Skip it when those flows are already covered, you prefer writing `unsafe` by hand, or UTF-8 and pointer contracts are not part of the workload.

## Quick example

Call a native UTF-8 API without encoding on every invocation, and without leaving pointers dangling:

```csharp
CString path = new(() => "/usr/lib/libexample.so"u8);

[DllImport("libexample", EntryPoint = "open_resource")]
static extern Int32 OpenResource(ReadOnlyValPtr<Byte> path);

path.AsSpan().WithSafeFixed(static (in IReadOnlyFixedContext<Byte> ctx) =>
{
    _ = OpenResource(ctx.ValuePointer);
});
```

On .NET 7+, `CString` also supports source-generated marshalling as a null-terminated UTF-8 string, so many P/Invoke declarations can take `CString` directly.

## Documentation

- [Documentation hub](docs/README.md) — map of every guide
- [Getting started](docs/getting-started.md) — install, target frameworks, AOT, Visual Basic
- [Capabilities](docs/capabilities.md) — what each area of the library is for
- [Use cases](docs/use-cases.md) — recipes for interop, UTF-8 pipelines, binary views, and AOT
- [API reference](docs/api/README.md) — types, members, and contracts

The XML documentation in the source remains the complete member-level reference. The guides above are written for reading, not for scanning a catalog.

## License

This project is licensed under the **MIT License**. Use it in open-source or closed-source projects; the only requirement is to keep the copyright notice. See [LICENSE.md](LICENSE.md).

## Contributing

Issues, ideas, translations, and pull requests are welcome. See [CONTRIBUTING.md](CONTRIBUTING.md).

The library currently ships messages in English, Arabic, Chinese, French, German, Italian, Japanese, Portuguese, Russian, and Spanish.
