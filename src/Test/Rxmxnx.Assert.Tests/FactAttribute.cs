#if !NETCOREAPP && !NETFRAMEWORK && !UAP
namespace Rxmxnx.PInvoke.Tests;

/// <summary>
/// Replacement class for XUnit.FactAttribute
/// </summary>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Method)]
public sealed class FactAttribute : TestAttribute;
#endif