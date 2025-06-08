#if !PACKAGE || !NET6_0_OR_GREATER

namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// <see cref="MemoryMarshal"/> compatibility utilities for internal use.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal static unsafe partial class MemoryMarshalCompat
{
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
		ref Byte ref0 = ref *value;
		Int32 length = MemoryMarshalCompat.IndexOfNull(ref *value);
		if (length < 0)
			throw new ArgumentException(null, nameof(value));
		return MemoryMarshal.CreateReadOnlySpan(ref ref0, length);
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
		ref Char ref0 = ref *value;
		Int32 length = MemoryMarshalCompat.IndexOfNull(ref ref0);
		if (length < 0)
			throw new ArgumentException(null, nameof(value));
		return MemoryMarshal.CreateReadOnlySpan(ref ref0, length);
	}
	/// <summary>
	/// Indicates whether <paramref name="utfSpan"/> points to null UTF-8 text-
	/// </summary>
	/// <param name="utfSpan">A UTF-8 span.</param>
	/// <returns>
	/// <see langword="true"/> if the dynamic type was successfully emitted in the current runtime; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	public static Boolean IsNullText(ReadOnlySpan<Byte> utfSpan)
	{
		fixed (Byte* ptr = &MemoryMarshal.GetReference(utfSpan))
			return ptr == IntPtr.Zero.ToPointer();
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

		ValidationUtilities.ThrowIfNoReflection();

		Type typeofRuntimeHelpers = typeof(RuntimeHelpers);

		ref MMethodTable mtpRef =
#if PACKAGE && !NETCOREAPP || NETCOREAPP3_1_OR_GREATER
			ref Unsafe.NullRef<MMethodTable>();
#else
			ref MemoryMarshal.GetReference(Span<MMethodTable>.Empty);
#endif
		if (typeof(MemoryMarshal).GetMethod("GetArrayDataReference", 0, [typeof(Array),]) is { } methodInfoNet6)
		{
			GetArrayDataReferenceDelegate del =
				(GetArrayDataReferenceDelegate)methodInfoNet6.CreateDelegate(typeof(GetArrayDataReferenceDelegate));
			return ref Unsafe.As<Byte, T>(ref del(array));
		}

		Object[] methodParam = [array,];
		if (typeofRuntimeHelpers.GetMethod("GetMethodTable", BindingFlags.Static | BindingFlags.NonPublic) is
		    { } methodInfoNet5)
		{
			Pointer mtpPtr = (Pointer)methodInfoNet5.Invoke(null, methodParam)!;
			mtpRef = ref Unsafe.AsRef<MMethodTable>(Pointer.Unbox(mtpPtr));
		}
		else if (typeofRuntimeHelpers.GetMethod("GetObjectMethodTablePointer",
		                                        BindingFlags.Static | BindingFlags.NonPublic) is { } methodInfoCore)
		{
			IntPtr mtpPtr = (IntPtr)methodInfoCore.Invoke(null, methodParam)!;
			mtpRef = ref Unsafe.AsRef<MMethodTable>(mtpPtr.ToPointer());
		}
		else
		{
			return ref Unsafe.As<Byte, T>(ref Unsafe.As<MonoRawData>(array).Data);
		}

		return ref Unsafe.As<Byte, T>(ref MemoryMarshalCompat.GetArrayDataReference(mtpRef, array));
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

#endif