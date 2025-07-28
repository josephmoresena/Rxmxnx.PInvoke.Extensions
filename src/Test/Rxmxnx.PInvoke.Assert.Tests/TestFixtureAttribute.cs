#if NETCOREAPP
namespace Rxmxnx.PInvoke.Tests;

/// <summary>
/// Replacement class for NUnit.Framework.TestFixtureAttribute
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class TestFixtureAttribute : Attribute;
#endif