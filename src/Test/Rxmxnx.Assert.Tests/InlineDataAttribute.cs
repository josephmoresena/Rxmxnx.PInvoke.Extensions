#if !NETCOREAPP
namespace Rxmxnx.PInvoke.Tests;

/// <summary>
/// Replacement class for XUnit.InlineDataAttribute
/// </summary>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class InlineDataAttribute : TestCaseAttribute
{
	public InlineDataAttribute(params Object?[]? arguments) : base(arguments) { }
	public InlineDataAttribute(Object? arg) : base(arg) { }
	public InlineDataAttribute(Object? arg1, Object? arg2) : base(arg1, arg2) { }
	public InlineDataAttribute(Object? arg1, Object? arg2, Object? arg3) : base(arg1, arg2, arg3) { }
}

#endif