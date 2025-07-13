# Unit Testing Overview

To ensure the correct functionality of all components, dedicated test projects were created using **xUnit** and
**AutoFixture**, targeting each of the intermediary libraries individually. This allows for isolated testing of the
components integrated into the `Rxmxnx.PInvoke.Extensions` package. However, some alternate execution paths—such as
Native AOT or reflection-free mode—cannot be fully covered by these test projects.

## Test Projects

* **`Common.Tests`**: Contains test cases for all components in the `Common` intermediary library, focusing primarily on
  elements that are not reused by other intermediary components. Tests in this project are considered low-level and
  foundational compared to those in the other test suites.
* **`Buffers.Tests`**: Validates buffer allocation (binary and non-binary) across various data types. These tests are
  more
  advanced as they extensively utilize reflection to avoid undesired behavior during validation. This is necessary
  because the library statically caches metadata used for runtime buffer generation.
* **`CString.Tests`**: Tests UTF-8 text handling and UTF-8 string sequences, covering different internal representations
  through `CString` and `CStringSequence` instances. Powered by the `CString.Tests.SourceGenerator` project, it includes
  a wide set of multilingual text samples (including emoji characters) to verify serialization, comparison, and
  concatenation operations—ensuring they produce results comparable to those using `System.String`. These tests can be
  memory- and CPU-intensive and are therefore considered critical, despite being mostly high-level.
* **`Extensions.Tests`**: Focuses on extension methods applied to types external to `Rxmxnx.PInvoke.Extensions`. Most of
  these validations complement those found in `Common.Tests`, placing this suite at an intermediate complexity level.

## Multi-Target Testing

The test projects support execution across all .NET and .NET Core versions compatible with `Rxmxnx.PInvoke.Extensions`,
enabling framework-specific validations. To run multi-targeted tests, use the following command:

```bash
dotnet test /p:MultipleFrameworkTest=true
```

Due to platform differences, some tests may be skipped, and basic compatibility checks may be added for the `Compat`
components, which ensure proper behavior across all supported frameworks—including **.NET Standard 2.1**.

To limit test execution only to frameworks fully supported by the current version, use:

```bash
dotnet test /p:OnlySupportedFrameworkTest=true
```

**Note:** The test projects themselves only validate builds for **.NET** and **.NET Core**; no tests are executed
specifically for **.NET Standard 2.1** builds.