#if !NETCOREAPP3_0_OR_GREATER
namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Parameter)]
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class CallerArgumentExpressionAttribute(String parameterName) : Attribute
{
	public String ParameterName { get; } = parameterName;
}
#endif