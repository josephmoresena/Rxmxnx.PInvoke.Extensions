[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=alert_status)![Bugs](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=bugs)![Coverage](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=coverage)![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=ncloc)![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=reliability_rating)![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=sqale_rating)![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=josephmoresena_PInvoke.Extensions&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=josephmoresena_PInvoke.Extensions)

---

# Description

`Rxmxnx.PInvoke.Extensions` is a library for safe, typed, and allocation-conscious interop on .NET — from Native AOT to Mono, Unity, and WebAssembly.

Work with UTF-8 the way native APIs already do, keep pointer intent in your signatures, and pin memory only for as long as a callback or `using` scope lasts.

## Features

- **UTF-8 / ASCII strings** — `CString`, `CStringSequence`, and `CStringBuilder` for interop and binary pipelines.
- **Typed pointers** — `ValPtr<T>`, `ReadOnlyValPtr<T>`, and `FuncPtr<TDelegate>` without spreading `unsafe`.
- **Scoped fixed memory** — pin spans and references for a callback, including struct-based functional interfaces (the form used on .NET Framework, .NET Standard 2.0, and UWP). Delegate overloads remain on 2.9.5 targets (.NET Standard 2.1 / .NET Core 3.0+).
- **Managed buffers** — stack-first temporary storage for values and object references.
- **Runtime awareness** — `AotInfo` and `SystemInfo` for Native AOT, Mono, and OS checks.
- **Transitions** — stay on the package while you move between Framework, Standard, .NET, Mono, UWP, Unity, and Xamarin.

---

# Documentation

The repository README is a short landing page. Guides for **capabilities**, **use cases**, and **APIs** live in the `docs/` folder, including which members exist on .NET Framework / UWP / .NET Standard 2.0.

[Documentation hub on GitHub](https://github.com/josephmoresena/Rxmxnx.PInvoke.Extensions#documentation)

XML comments in the source remain the complete member-level reference.

---

# License

This project is licensed under the **MIT License**.
