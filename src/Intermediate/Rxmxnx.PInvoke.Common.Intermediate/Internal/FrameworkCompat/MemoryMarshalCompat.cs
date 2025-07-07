namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// <see cref="MemoryMarshal"/> compatibility utilities for internal use.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3011)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal static unsafe
#if !PACKAGE || !NET6_0_OR_GREATER
	partial
#endif
	class MemoryMarshalCompat
{
#if !PACKAGE
	/// <summary>
	/// Target framework for the current build.
	/// </summary>
	public static readonly String TargetFramework =
#if !NETCOREAPP
		".NETStandard 2.1";
#elif NETCOREAPP3_0
		".NETCoreApp 3.0.3";
#elif NETCOREAPP3_1
		".NETCoreApp 3.1.12";
#elif NET5_0
		".NETCoreApp 5.0.17";
#else
		RuntimeInformation.FrameworkDescription;
#endif
#endif

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
	/// Returns a reference to the 0th element of <paramref name="array"/>.
	/// If the array is empty, returns a reference to where the 0th element would have been stored.
	/// </summary>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	/// <returns>Managed reference to <paramref name="array"/> data.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T GetArrayDataReference<T>(Array array)
	{
#if !PACKAGE || !NET6_0_OR_GREATER
		if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
		{
			// Arrays of unmanaged items can be pinned.
			GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);
			try
			{
				return ref Unsafe.As<Byte, T>(
					ref MemoryMarshalCompat.GetArrayDataReference(handle.AddrOfPinnedObject().ToPointer(), array));
			}
			finally
			{
				handle.Free();
			}
		}
		return ref Unsafe.As<Byte, T>(ref ArrayReferenceHelper.Instance.GetArrayDataReference(array));
#else
		return ref Unsafe.As<Byte, T>(ref MemoryMarshal.GetArrayDataReference(array));
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