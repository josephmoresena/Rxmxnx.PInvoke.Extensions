#if !NETCOREAPP
namespace Rxmxnx.PInvoke.Tests;

/// <summary>
/// Replacement class for XUnit.SkippableTheoryAttribute
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class SkippableTheoryAttribute : Attribute;
#endif