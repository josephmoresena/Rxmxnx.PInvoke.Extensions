#if !NETCOREAPP
namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class CallerArgumentExpressionAttribute(String parameterName) : Attribute
{
	public String ParameterName { get; } = parameterName;
}
#endif