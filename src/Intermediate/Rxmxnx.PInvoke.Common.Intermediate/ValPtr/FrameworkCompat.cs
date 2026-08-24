namespace Rxmxnx.PInvoke;

public readonly partial struct ValPtr<T> 
#if !NETSTANDARD2_1 && !NETCOREAPP3_0_OR_GREATER
	: IEquatable<IntPtr>
#endif
{
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	Boolean IEquatable<IntPtr>.Equals(IntPtr other) => this.Pointer.Equals(other);
}