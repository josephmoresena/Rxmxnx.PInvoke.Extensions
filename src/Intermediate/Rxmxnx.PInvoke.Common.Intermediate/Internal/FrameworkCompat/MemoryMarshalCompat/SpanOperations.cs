namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal static unsafe partial class MemoryMarshalCompat
{
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
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
#pragma warning disable CS8500
}