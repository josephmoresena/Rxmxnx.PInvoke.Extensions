# Rxmxnx.PInvoke.Json

This project is an example implementation demonstrating how to integrate `System.Text.Json` support for
`Rxmxnx.PInvoke.CString` and `Rxmxnx.PInvoke.CStringSequence` while preserving maximum compatibility with
**Mono Framework** environments.

Since **.NET Core 3.0+**, `Rxmxnx.PInvoke.Extensions` can work directly with `System.Text.Json`. However,
including a direct dependency in the core library would require mixing **.NET Standard** 2.0 and 2.1 assemblies, which
conflicts with the compatibility model used to support **Mono Framework 4.5-based runtimes**.

This source code is compatible with **Xamarin** and **Unity**, but the project itself is primarily designed to
run on **Mono Framework** 6.4 or later.

---

## Why isn’t there a built-in option?

One of the main goals of `Rxmxnx.PInvoke.Extensions` is to remain fully compatible with any platform that supports
**.NET Standard 2.1**. However, the available implementations of `System.Text.Json` target **.NET Standard 2.0**, which
introduces a 2.0 and 2.1 assembly mix when referenced from a **.NET Standard 2.1** library.

Platforms such as **Xamarin.Mac**, **Xamarin.Android**, **Xamarin.iOS**, and **Unity** support this version mixing. In
contrast, **Mono Framework** does not, due to its long-term goal of maintaining compatibility with
**.NET Framework 4.x**.

To preserve **Mono Framework** compatibility, `Rxmxnx.PInvoke.Extensions` intentionally avoids referencing any external
packages that rely on APIs unavailable in **.NET Framework 4.5**.

However, the library itself targets **.NET Standard 2.1** and therefore exposes APIs defined by that specification.
As a result, mixing it with dependencies targeting **.NET Standard 2.0** may introduce assembly binding conflicts on
**Mono Framework** runtimes.

When using `Rxmxnx.PInvoke.Extensions` with **Mono Framework** (excluding **Xamarin** and **Unity**), the build should
reference only the following packages:

* `System.Runtime.CompilerServices.Unsafe`
* `System.Collections.Immutable`

At compile time:

* The **.NET Framework 4.5** assembly of `System.Runtime.CompilerServices.Unsafe` is used.
* The **.NET Standard 1.0** assembly of `System.Collections.Immutable` is used.

These assemblies do not rely on APIs unavailable in **.NET Framework 4.5**, ensuring that no additional framework
requirements are introduced.

---

## How does this work?

This project is designed to compile against **.NET Standard 2.1** reference assemblies while running on custom
**.NET Standard 2.1** facades.

This approach is necessary because the official **.NET Standard 2.1** reference assemblies are strong-name signed,
whereas the custom facades are not.

---

# Mono Facades

These facades enable non-built-in `System.Text.Json` support when using `Rxmxnx.PInvoke.Extensions` on Mono runtimes,
where mixing **.NET Standard 2.0** and **.NET Standard 2.1** is otherwise problematic.

* When building with **.NET Core** or **.NET**, the projects target **.NET Standard 2.1** and forward all types to the
  `netstandard` assembly.
* When building with **Mono Framework**, the projects target **.NET Framework 4.5** and forward all types to the
  `mscorlib` assembly.

Below are the facades used:

---

### System.Buffers

This facade forwards the type:

* `System.Buffers.ArrayPool<>`

**Source Code:**
[mcs/class/Facades/System.Buffers](https://gitlab.winehq.org/mono/mono/-/tree/main/mcs/class/Facades/System.Buffers)

---

### System.Memory

This facade forwards the following types:

* `System.MemoryExtensions`
* `System.Memory<>`
* `System.ReadOnlyMemory<>`
* `System.ReadOnlySpan<>`
* `System.SequencePosition`
* `System.Span<>`
* `System.Buffers.BuffersExtensions`
* `System.Buffers.IBufferWriter<>`
* `System.Buffers.IMemoryOwner<>`
* `System.Buffers.IPinnable`
* `System.Buffers.MemoryManager<>`
* `System.Buffers.MemoryHandle`
* `System.Buffers.MemoryPool<>`
* `System.Buffers.OperationStatus`
* `System.Buffers.ReadOnlySequenceSegment<>`
* `System.Buffers.ReadOnlySequence<>`
* `System.Buffers.StandardFormat`
* `System.Buffers.Binary.BinaryPrimitives`
* `System.Buffers.Text.Base64`
* `System.Buffers.Text.Utf8Formatter`
* `System.Buffers.Text.Utf8Parser`
* `System.Runtime.InteropServices.MemoryMarshal`
* `System.Runtime.InteropServices.SequenceMarshal`

**Source Code:**
[mcs/class/Facades/System.Memory](https://gitlab.winehq.org/mono/mono/-/tree/main/mcs/class/Facades/System.Memory)

---

### System.Threading.Tasks.Extensions

This facade forwards the following types:

* `System.Runtime.CompilerServices.AsyncMethodBuilderAttribute`
* `System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder`
* `System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder<>`
* `System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable`
* `System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable<>`
* `System.Runtime.CompilerServices.ValueTaskAwaiter`
* `System.Runtime.CompilerServices.ValueTaskAwaiter<>`
* `System.Threading.Tasks.ValueTask`
* `System.Threading.Tasks.ValueTask<>`
* `System.Threading.Tasks.Sources.IValueTaskSource`
* `System.Threading.Tasks.Sources.IValueTaskSource<>`
* `System.Threading.Tasks.Sources.ValueTaskSourceOnCompletedFlags`
* `System.Threading.Tasks.Sources.ValueTaskSourceStatus`

**Source Code:**
[mcs/class/Facades/System.Threading.Tasks.Extensions](https://gitlab.winehq.org/mono/mono/-/tree/main/mcs/class/Facades/System.Threading.Tasks.Extensions)

---

These facades are required only when targeting **Mono Framework**.
**Xamarin** and **Unity** already include built-in facades that allow mixing **.NET Standard** 2.0 and 2.1 assemblies.
