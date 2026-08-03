#if !NETSTANDARD2_0_OR_GREATER && !NETCOREAPP && !NETFRAMEWORK && !UAP10_0_16299
namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.All, Inherited = false)]
internal sealed class ExcludeFromCodeCoverageAttribute : Attribute;
#endif