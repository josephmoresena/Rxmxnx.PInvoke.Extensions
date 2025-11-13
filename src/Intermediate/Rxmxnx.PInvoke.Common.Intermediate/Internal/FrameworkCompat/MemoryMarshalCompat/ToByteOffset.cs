#if NETCOREAPP && (!PACKAGE || !NET6_0_OR_GREATER)
// ReSharper disable BuiltInTypeReferenceStyle

using UIntPtr = nuint;

namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal static partial class MemoryMarshalCompat
{
	/// <summary>
	/// Converts a <see cref="UIntPtr"/> value to be valid for use with <see cref="Unsafe"/> class.
	/// </summary>
	/// <param name="ptr">A <see cref="UIntPtr"/> value.</param>
	/// <returns>A <see cref="System.IntPtr"/> instance.</returns>
#if !NETCOREAPP3_1_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe IntPtr ToByteOffset(UIntPtr ptr) => new((void*)ptr);
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UIntPtr ToByteOffset(UIntPtr ptr) => ptr;
#endif
}
#endif