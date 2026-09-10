#if NETFRAMEWORK && !NET46_OR_GREATER
using Array = Rxmxnx.PInvoke.Internal.FrameworkCompat.ArrayCompat;
#endif

namespace Rxmxnx.PInvoke;

#if !PACKAGE && !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public partial class NativeUtilities
{
	/// <summary>
	/// Returns an empty array.
	/// </summary>
	/// <typeparam name="T">The type of the elements of the array.</typeparam>
	/// <returns>An empty array.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	// ReSharper disable once UseCollectionExpression
	public static T[] ArrayEmpty<T>() where T : unmanaged => Array.Empty<T>();

#pragma warning disable CS8500
	/// <summary>
	/// Tries to create a new span over a portion of a regular managed object.
	/// </summary>
	/// <typeparam name="T">The type of the data items.</typeparam>
	/// <param name="reference">A reference to data.</param>
	/// <param name="length">The number of <typeparamref name="T" /> elements that <paramref name="reference" /> contains.</param>
	/// <param name="span">Created span.</param>
	/// <returns>
	/// <see langword="true"/> if the span was successfully created; otherwise, <see langword="false"/>.
	/// </returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean TryCreateSpan<T>(ref T reference, Int32 length, out Span<T> span)
	{
#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
		if (!SystemInfo.UsesNativeSpan)
		{
			span = default;
			return false;
		}
		unsafe
		{
			fixed (void* ptr = &reference)
				span = MemoryMarshalCompat.CreateUnsafeSpan<T>(ptr, length);
		}
#else
		span = MemoryMarshal.CreateSpan(ref reference, length);
#endif
		return true;
	}
	/// <summary>
	/// Tries to create a new read-only span over a portion of a regular managed object.
	/// </summary>
	/// <typeparam name="T">The type of the data items.</typeparam>
	/// <param name="reference">A read-only reference to data.</param>
	/// <param name="length">The number of <typeparamref name="T" /> elements that <paramref name="reference" /> contains.</param>
	/// <param name="span">Created read-only span.</param>
	/// <returns>
	/// <see langword="true"/> if the read-only span was successfully created; otherwise, <see langword="false"/>.
	/// </returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean TryCreateReadOnlySpan<T>(in T reference, Int32 length, out ReadOnlySpan<T> span)
	{
#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
		if (!SystemInfo.UsesNativeSpan)
		{
			span = default;
			return false;
		}
		unsafe
		{
			fixed (void* ptr = &reference)
				span = MemoryMarshalCompat.CreateUnsafeReadOnlySpan<T>(ptr, length);
		}
#elif !NET8_0_OR_GREATER
		span = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in reference), length);
#else
		span = MemoryMarshal.CreateReadOnlySpan(in reference, length);
#endif
		return true;
	}
#pragma warning restore CS8500
}