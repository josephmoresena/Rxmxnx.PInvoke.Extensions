#if !NETCOREAPP
namespace Rxmxnx.PInvoke.Tests;

/// <summary>
/// Replacement class for XUnit.TheoryAttribute
/// </summary>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Method)]
public sealed class TheoryAttribute : Attribute;
#endif