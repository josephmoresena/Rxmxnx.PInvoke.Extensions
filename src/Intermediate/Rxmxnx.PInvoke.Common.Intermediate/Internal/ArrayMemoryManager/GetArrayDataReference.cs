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
	private static ref T GetArrayDataReference2(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,])![0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference3(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,])![0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference4(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,])![0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference5(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,,])![0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference6(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,,,])![0, 0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference7(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,,,,])![0, 0, 0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference8(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference9(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference10(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference11(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference12(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference13(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference14(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference15(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference16(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference17(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference18(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(
			in (array as T[,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference19(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(
			in (array as T[,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference20(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(
			in (array as T[,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference21(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(
			in (array as T[,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference22(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(
			in (array as T[,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference23(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(
			in (array as T[,,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			                                         0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference24(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(
			in (array as T[,,,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			                                          0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference25(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(
			in (array as T[,,,,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			                                           0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference26(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(
			in (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			                                            0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference27(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(
			in (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			                                             0, 0, 0, 0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference28(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
			                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			                        0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference29(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
			                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			                        0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference30(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
			                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			                        0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference31(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
			                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			                        0, 0, 0]);
	}
	/// <inheritdoc cref="ArrayMemoryManager{T}.GetArrayDataReference2(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference32(Array array)
	{
		if (array.Length == 0) return ref Unsafe.NullRef<T>();
		return ref Unsafe.AsRef(in (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
			                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			                        0, 0, 0, 0]);
	}
}
#endif