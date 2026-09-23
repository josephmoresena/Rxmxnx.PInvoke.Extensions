#if !NET6_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal;

// ReSharper disable once ClassCannotBeInstantiated
internal partial class ArrayMemoryManager<T>
{
	/// <summary>
	/// Returns a reference to the 0th element of <paramref name="array"/>.
	/// If the array is empty, returns a <see langword="null "/>reference.
	/// </summary>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	/// <returns>Managed reference to <paramref name="array"/> data.</returns>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset2(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,])![lb[0], lb[1]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset3(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,])![lb[0], lb[1], lb[2]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset4(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,])![lb[0], lb[1], lb[2], lb[3]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset5(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset6(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset7(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset8(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset9(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef =
			ref (array as T[,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset10(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef =
			ref (array as T[,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8], lb[9]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset11(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef =
			ref (array as T[,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8], lb[9], lb[10]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset12(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef =
			ref (array as T[,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8], lb[9], lb[10],
			                               lb[11]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset13(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7],
		                                                         lb[8], lb[9], lb[10], lb[11], lb[12]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset14(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6],
		                                                          lb[7], lb[8], lb[9], lb[10], lb[11], lb[12], lb[13]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset15(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6],
		                                                           lb[7], lb[8], lb[9], lb[10], lb[11], lb[12], lb[13],
		                                                           lb[14]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset16(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6],
		                                                            lb[7], lb[8], lb[9], lb[10], lb[11], lb[12], lb[13],
		                                                            lb[14], lb[15]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset17(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6],
		                                                             lb[7], lb[8], lb[9], lb[10], lb[11], lb[12],
		                                                             lb[13], lb[14], lb[15], lb[16]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset18(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6],
		                                                              lb[7], lb[8], lb[9], lb[10], lb[11], lb[12],
		                                                              lb[13], lb[14], lb[15], lb[16], lb[17]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset19(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6],
		                                                               lb[7], lb[8], lb[9], lb[10], lb[11], lb[12],
		                                                               lb[13], lb[14], lb[15], lb[16], lb[17], lb[18]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset20(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6],
		                                                                lb[7], lb[8], lb[9], lb[10], lb[11], lb[12],
		                                                                lb[13], lb[14], lb[15], lb[16], lb[17], lb[18],
		                                                                lb[19]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset21(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,,,])![
			lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8], lb[9], lb[10], lb[11], lb[12], lb[13],
			lb[14], lb[15], lb[16], lb[17], lb[18], lb[19], lb[20]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset22(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,,,,])![
			lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8], lb[9], lb[10], lb[11], lb[12], lb[13],
			lb[14], lb[15], lb[16], lb[17], lb[18], lb[19], lb[20], lb[21]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset23(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,,,,,])![
			lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8], lb[9], lb[10], lb[11], lb[12], lb[13],
			lb[14], lb[15], lb[16], lb[17], lb[18], lb[19], lb[20], lb[21], lb[22]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset24(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,])![
			lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8], lb[9], lb[10], lb[11], lb[12], lb[13],
			lb[14], lb[15], lb[16], lb[17], lb[18], lb[19], lb[20], lb[21], lb[22], lb[23]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset25(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,])![
			lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8], lb[9], lb[10], lb[11], lb[12], lb[13],
			lb[14], lb[15], lb[16], lb[17], lb[18], lb[19], lb[20], lb[21], lb[22], lb[23], lb[24]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset26(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,])![
			lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8], lb[9], lb[10], lb[11], lb[12], lb[13],
			lb[14], lb[15], lb[16], lb[17], lb[18], lb[19], lb[20], lb[21], lb[22], lb[23], lb[24], lb[25]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset27(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,])![
			lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8], lb[9], lb[10], lb[11], lb[12], lb[13],
			lb[14], lb[15], lb[16], lb[17], lb[18], lb[19], lb[20], lb[21], lb[22], lb[23], lb[24], lb[25], lb[26]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset28(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
			lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8], lb[9], lb[10], lb[11], lb[12], lb[13],
			lb[14], lb[15], lb[16], lb[17], lb[18], lb[19], lb[20], lb[21], lb[22], lb[23], lb[24], lb[25], lb[26],
			lb[27]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset29(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
			lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8], lb[9], lb[10], lb[11], lb[12], lb[13],
			lb[14], lb[15], lb[16], lb[17], lb[18], lb[19], lb[20], lb[21], lb[22], lb[23], lb[24], lb[25], lb[26],
			lb[27], lb[28]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset30(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
			lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8], lb[9], lb[10], lb[11], lb[12], lb[13],
			lb[14], lb[15], lb[16], lb[17], lb[18], lb[19], lb[20], lb[21], lb[22], lb[23], lb[24], lb[25], lb[26],
			lb[27], lb[28], lb[29]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset31(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
			lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8], lb[9], lb[10], lb[11], lb[12], lb[13],
			lb[14], lb[15], lb[16], lb[17], lb[18], lb[19], lb[20], lb[21], lb[22], lb[23], lb[24], lb[25], lb[26],
			lb[27], lb[28], lb[29], lb[30]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset32(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ReadOnlySpan<Int32> lb = ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
			lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8], lb[9], lb[10], lb[11], lb[12], lb[13],
			lb[14], lb[15], lb[16], lb[17], lb[18], lb[19], lb[20], lb[21], lb[22], lb[23], lb[24], lb[25], lb[26],
			lb[27], lb[28], lb[29], lb[30], lb[31]];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
}
#endif