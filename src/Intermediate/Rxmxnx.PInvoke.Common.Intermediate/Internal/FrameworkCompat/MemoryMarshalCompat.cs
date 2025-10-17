namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// <see cref="MemoryMarshal"/> compatibility utilities for internal use.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
#if NETCOREAPP && (!PACKAGE || !NET6_0_OR_GREATER)
internal static unsafe partial class MemoryMarshalCompat
#else
internal static unsafe class MemoryMarshalCompat
#endif
{
	/// <summary>
	/// Creates a new read-only span for a null-terminated UTF8 string.
	/// </summary>
	/// <param name="value">The pointer to the null-terminated string of bytes.</param>
	/// <returns>
	/// A read-only span representing the specified null-terminated string, or an empty span if the pointer is null.
	/// </returns>
	/// <remarks>
	/// The returned span does not include the null terminator, nor does it validate the well-formedness of the UTF8 data.
	/// </remarks>
	/// <exception cref="ArgumentException">The string is longer than <see cref="Int32.MaxValue"/>.</exception>
	public static ReadOnlySpan<Byte> CreateReadOnlySpanFromNullTerminated(Byte* value)
	{
#if !PACKAGE || !NET6_0_OR_GREATER
		if (value == IntPtr.Zero.ToPointer())
			return default;

		ref Byte ref0 = ref *value;
		Int32 length = MemoryMarshalCompat.IndexOfNull(ref *value);
		if (length < 0)
			throw new ArgumentException(null, nameof(value));
		return MemoryMarshal.CreateReadOnlySpan(ref ref0, length);
#else
		return MemoryMarshal.CreateReadOnlySpanFromNullTerminated(value);
#endif
	}
	/// <summary>
	/// Creates a new read-only span for a null-terminated string.
	/// </summary>
	/// <param name="value">The pointer to the null-terminated string of characters.</param>
	/// <returns>
	/// A read-only span representing the specified null-terminated string, or an empty span if the pointer is null.
	/// </returns>
	/// <remarks>The returned span does not include the null terminator.</remarks>
	/// <exception cref="ArgumentException">The string is longer than <see cref="int.MaxValue"/>.</exception>
	public static ReadOnlySpan<Char> CreateReadOnlySpanFromNullTerminated(Char* value)
	{
#if !PACKAGE || !NET6_0_OR_GREATER
		if (value == IntPtr.Zero.ToPointer())
			return default;

		ref Char ref0 = ref *value;
		Int32 length = MemoryMarshalCompat.IndexOfNull(ref ref0);
		if (length < 0)
			throw new ArgumentException(null, nameof(value));
		return MemoryMarshal.CreateReadOnlySpan(ref ref0, length);
#else
		return MemoryMarshal.CreateReadOnlySpanFromNullTerminated(value);
#endif
	}
	/// <summary>
	/// Retrieves the index of the first null occurrence in the buffer represented by <paramref name="buffer"/>.
	/// </summary>
	/// <typeparam name="T">Type of buffer element.</typeparam>
	/// <param name="buffer">Buffer reference.</param>
	/// <returns>Index of</returns>
	/// <remarks>This method is used only on .Net Standard build.</remarks>
	public static Int32 IndexOfNull<T>(ref T buffer) where T : unmanaged, IEquatable<T>
	{
		if (Unsafe.IsNullRef(ref buffer))
			return 0;

		UInt32 result = 0;
		T nullValue = default;
		while (!Unsafe.Add(ref buffer, new IntPtr((void*)result)).Equals(nullValue))
		{
			result++;
			if (result >= Int32.MaxValue)
				return -1;
		}

		return (Int32)result;
	}
}