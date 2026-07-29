#if !NETSTANDARD2_1 && !NETCOREAPP3_0_OR_GREATER
namespace Rxmxnx.PInvoke;

public readonly partial struct ValPtr<T> : IEquatable<IntPtr>
{
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	Boolean IEquatable<IntPtr>.Equals(IntPtr other) => this.Pointer.Equals(other);
}
#endif