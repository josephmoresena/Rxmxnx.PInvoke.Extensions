namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
internal static unsafe partial class MemoryMarshalCompat
#else
internal static partial class MemoryMarshalCompat
#endif
{
#pragma warning disable CS8500
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