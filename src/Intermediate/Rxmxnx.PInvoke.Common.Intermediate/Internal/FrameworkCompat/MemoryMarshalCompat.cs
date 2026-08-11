namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// <see cref="MemoryMarshal"/> compatibility utilities for internal use.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif

#if NETSTANDARD2_1 || NETCOREAPP2_1
internal static unsafe class MemoryMarshalCompat
#else
internal static unsafe partial class MemoryMarshalCompat
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
		Int32 length = MemoryMarshalCompat.IndexOfNull(ref ref0);
		return length >= 0 ?
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
			MemoryMarshal.CreateReadOnlySpan(ref ref0, length) :
#else
			new(value, length) :
#endif
			throw new ArgumentException(null, nameof(value));
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
		return length >= 0 ?
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
			MemoryMarshal.CreateReadOnlySpan(ref ref0, length) :
#else
			new(value, length) :
#endif
			throw new ArgumentException(null, nameof(value));
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
#pragma warning disable CS8500
	/// <summary>
	/// Creates a new span of <typeparamref name="T"/> items using an <see cref="Pinnable{T}"/> instance.
	/// </summary>
	/// <typeparam name="T">The type of the data items.</typeparam>
	/// <param name="pinnable">A <see cref="Pinnable{T}"/> instance.</param>
	/// <param name="start">The starting <typeparamref name="T"/> reference.</param>
	/// <param name="length">
	/// The number of <typeparamref name="T"/> elements that created span contains.
	/// </param>
	/// <returns>A safe span.</returns>
#if !PACKAGE && (NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER)
	[ExcludeFromCodeCoverage]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static Span<T> CreateSafeSpan<T>(Pinnable<T> pinnable, ref T start, Int32 length)
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		=> MemoryMarshal.CreateSpan(ref start, length);
#else
	{
		if (MemoryMarshalCompat.PinnableOffset == -1)
			// Modern UWP uses fast span, .NET Framework assembly can be used on Mono with .NET Standard 2.1
			fixed (void* pStart = &start)
				return MemoryMarshalCompat.CreateUnsafeSpan<T>(pStart, length);
		Span<Byte> span = new(Unsafe.ByteOffset(ref pinnable.Data, ref start).ToPointer(), length);
		ref Span<Byte> spanRef = ref span;
		fixed (void* p = &spanRef)
		{
			MemoryMarshalCompat.SetPinnableField(pinnable, p);
			Span<T>* spanObjectPtr = (Span<T>*)p;
			return spanObjectPtr[0];
		}
	}
#endif
	/// <summary>
	/// Creates a new span of <typeparamref name="T"/> items using an unmanaged/fixed pointer.
	/// </summary>
	/// <typeparam name="T">The type of the data items.</typeparam>
	/// <param name="ptr">An unmanaged pointer to data.</param>
	/// <param name="length">
	/// The number of <typeparamref name="T"/> elements that <paramref name="ptr"/> contains.
	/// </param>
	/// <returns>An unsafe span.</returns>
#if !PACKAGE && (NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER)
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Span<T> CreateUnsafeSpan<T>(void* ptr, Int32 length)
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		=> MemoryMarshal.CreateSpan(ref Unsafe.AsRef<T>(ptr), length);
#else
	{
		Span<Byte> span = new(ptr, length);
		ref Span<Byte> spanRef = ref span;
		fixed (void* p = &spanRef)
		{
			Span<T>* spanObjectPtr = (Span<T>*)p;
			return spanObjectPtr[0];
		}
	}
#endif
	/// <summary>
	/// Creates a new read-only span <typeparamref name="T"/> items using an unmanaged/fixed pointer.
	/// </summary>
	/// <typeparam name="T">The type of the data items.</typeparam>
	/// <param name="ptr">An unmanaged pointer to data.</param>
	/// <param name="length">The number of <typeparamref name="T"/> elements that <paramref name="ptr"/> contains.</param>
	/// <returns>An unsafe read-only span.</returns>
#if !PACKAGE && (NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER)
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ReadOnlySpan<T> CreateUnsafeReadOnlySpan<T>(void* ptr, Int32 length)
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		=> MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef<T>(ptr), length);
#else
	{
		Span<Byte> span = new(ptr, length);
		ref Span<Byte> spanRef = ref span;
		fixed (void* p = &spanRef)
		{
			Span<T>* spanObjectPtr = (Span<T>*)p;
			return spanObjectPtr[0];
		}
	}
#endif
	/// <summary>
	/// Casts a read-only span of one primitive type to a read-only span of another primitive type.
	/// </summary>
	/// <param name="span">The source slice to convert.</param>
	/// <typeparam name="TFrom">The type of the source span.</typeparam>
	/// <typeparam name="TTo">The type of the target span.</typeparam>
	/// <returns>The converted read-only span.</returns>
	public static ReadOnlySpan<TTo> Cast<TFrom, TTo>(ReadOnlySpan<TFrom> span)
		where TFrom : unmanaged where TTo : unmanaged
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		=> MemoryMarshal.Cast<TFrom, TTo>(span);
#else
	{
		Int32 newLength = span.Length * sizeof(TFrom) / sizeof(TTo);
		Object? pinnable = default;
		if (MemoryMarshalCompat.PinnableOffset != -1)
		{
			ref ReadOnlySpan<TFrom> refSpan = ref span;
			fixed (void* pSpan = &refSpan)
				pinnable = MemoryMarshalCompat.GetReadOnlyPinnableField(pSpan);
		}
		if (pinnable is null)
		{
			// Modern UWP uses fast span, .NET Framework assembly can be used on Mono with .NET Standard 2.1
			fixed (void* pStart = &MemoryMarshal.GetReference(span))
				return MemoryMarshalCompat.CreateUnsafeReadOnlySpan<TTo>(pStart, newLength);
		}
		IntPtr offset = Unsafe.ByteOffset(ref Unsafe.As<Object, Pinnable<TFrom>>(ref pinnable).Data,
		                                  ref MemoryMarshal.GetReference(span));
		Span<Byte> result = new(offset.ToPointer(), newLength);
		ref Span<Byte> spanRef = ref result;
		fixed (void* p = &spanRef)
		{
			MemoryMarshalCompat.SetPinnableField(pinnable, p);
			Span<TTo>* spanObjectPtr = (Span<TTo>*)p;
			return spanObjectPtr[0];
		}
	}
#endif
	/// <summary>
	/// Casts a span of one primitive type to a span of another primitive type.
	/// </summary>
	/// <param name="span">The source slice to convert.</param>
	/// <typeparam name="TFrom">The type of the source span.</typeparam>
	/// <typeparam name="TTo">The type of the target span.</typeparam>
	/// <returns>The converted read-only span.</returns>
	public static Span<TTo> Cast<TFrom, TTo>(Span<TFrom> span) where TFrom : unmanaged where TTo : unmanaged
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		=> MemoryMarshal.Cast<TFrom, TTo>(span);
#else
	{
		Int32 newLength = span.Length * sizeof(TFrom) / sizeof(TTo);
		Object? pinnable = default;
		if (MemoryMarshalCompat.PinnableOffset != -1)
		{
			ref Span<TFrom> refSpan = ref span;
			fixed (void* pSpan = &refSpan)
				pinnable = MemoryMarshalCompat.GetPinnableField(pSpan);
		}
		if (pinnable is null)
		{
			// Modern UWP uses fast span, .NET Framework assembly can be used on Mono with .NET Standard 2.1
			fixed (void* pStart = &MemoryMarshal.GetReference(span))
				return MemoryMarshalCompat.CreateUnsafeSpan<TTo>(pStart, newLength);
		}
		IntPtr offset = Unsafe.ByteOffset(ref Unsafe.As<Object, Pinnable<TFrom>>(ref pinnable).Data,
		                                  ref MemoryMarshal.GetReference(span));
		Span<Byte> result = new(offset.ToPointer(), newLength);
		ref Span<Byte> spanRef = ref result;
		fixed (void* p = &spanRef)
		{
			MemoryMarshalCompat.SetPinnableField(pinnable, p);
			Span<TTo>* spanObjectPtr = (Span<TTo>*)p;
			return spanObjectPtr[0];
		}
	}
#endif
#pragma warning restore CS8500
}