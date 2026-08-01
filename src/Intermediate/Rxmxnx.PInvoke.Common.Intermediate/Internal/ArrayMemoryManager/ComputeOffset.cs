#if !NET6_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal;

internal partial class ArrayMemoryManager<T>
{
	/// <summary>
	/// Returns a reference to the 0th element of <paramref name="array"/>.
	/// If the array is empty, returns a <see langword="null "/>reference.
	/// </summary>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	/// <returns>Managed reference to <paramref name="array"/> data.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset2(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,])![0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset3(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,])![0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset4(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,])![0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset5(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,,])![0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset6(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,,,])![0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset7(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,,,,])![0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset8(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset9(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset10(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset11(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset12(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset13(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset14(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset15(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset16(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset17(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset18(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef =
			ref (array as T[,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset19(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef =
			ref (array as T[,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset20(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef =
			ref (array as T[,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset21(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef =
			ref (array as T[,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset22(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef =
			ref (array as T[,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset23(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef =
			ref (array as T[,,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			                                          0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset24(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef =
			ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			                                           0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset25(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef =
			ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			                                            0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset26(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef =
			ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			                                             0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset27(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef =
			ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			                                              0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset28(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset29(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset30(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset31(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.ComputeOffset2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr ComputeOffset32(Array array)
	{
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		ref readonly T dataRef = ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
		return Unsafe.ByteOffset(ref pinnableRef.Data, ref Unsafe.AsRef(in dataRef));
	}
}
#endif