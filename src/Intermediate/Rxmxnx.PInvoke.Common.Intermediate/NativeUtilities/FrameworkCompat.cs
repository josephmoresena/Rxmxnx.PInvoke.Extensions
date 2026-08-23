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
	/// <remarks>
	/// <para>
	/// This is the portable stand-in for <c>MemoryMarshal.CreateSpan</c> when the same source must run on
	/// fast (native, two-field) and slow (three-field) <see cref="Span{T}"/> layouts.
	/// </para>
	/// <para>
	/// On .NET Standard 2.1 / .NET Core 2.1 and later the call always succeeds and wraps
	/// <c>MemoryMarshal.CreateSpan</c>. On .NET Framework and UWP it succeeds only when the
	/// <em>executing</em> runtime uses native span (for example Mono hosting a Framework TFM, or
	/// modern UWP). Desktop .NET Framework typically returns <see langword="false"/> and a default
	/// <paramref name="span"/>.
	/// </para>
	/// <para>
	/// Prefer this over reading <see cref="SystemInfo.UsesNativeSpan"/> and then calling
	/// <c>MemoryMarshal.CreateSpan</c> yourself. Success <em>is</em> the fast-span path (the view is
	/// already in <paramref name="span"/>). Failure <em>is</em> the slow-span path: keep array copies,
	/// pinning helpers, or other APIs that already understand the three-field layout. Do not invent a
	/// two-field span from a raw address when this method returns <see langword="false"/>.
	/// </para>
	/// </remarks>
	/// <seealso cref="TryCreateReadOnlySpan{T}"/>
	/// <seealso cref="SystemInfo.UsesNativeSpan"/>
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
	/// <remarks>
	/// <para>
	/// Read-only counterpart of <see cref="TryCreateSpan{T}"/>. Use this when the source is
	/// <see langword="in"/> / <see langword="ref readonly"/> and you want a
	/// <see cref="ReadOnlySpan{T}"/> on the fast-span path.
	/// </para>
	/// <para>
	/// On modern TFMs this hides the <c>CreateReadOnlySpan(in T)</c> versus
	/// <c>CreateReadOnlySpan(ref T)</c> split (.NET 8.0+). On slow-span runtimes it returns
	/// <see langword="false"/> instead of synthesizing an invalid two-field view.
	/// </para>
	/// </remarks>
	/// <seealso cref="TryCreateSpan{T}"/>
	/// <seealso cref="SystemInfo.UsesNativeSpan"/>
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