namespace Rxmxnx.PInvoke.Internal;

// ReSharper disable once ClassCannotBeInstantiated
internal partial class ArrayMemoryManager<T>
{
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset2(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset3(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset4(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset5(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset6(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset7(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset8(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset9(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset10(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset11(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset12(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset13(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset14(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset15(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset16(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset17(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset18(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset19(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset20(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset21(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset22(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset23(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset24(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset25(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset26(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset27(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset28(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset29(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset30(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset31(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
#if !NET6_0_OR_GREATER
		if (array.Length == 0) return default;
		Array nonGenericArray = array;
		IntPtr offset = ArrayMemoryManager<T>.offsets[array.Rank - 2] ??=
 ArrayMemoryManager<T>.ComputeOffset32(nonGenericArray);
		ref Pinnable<T> pinnableRef = ref Unsafe.As<Array, Pinnable<T>>(ref nonGenericArray);
		ref T managedRef = ref Unsafe.AddByteOffset(ref pinnableRef.Data, offset);
#else
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
#else
		Span<T> span = ArrayMemoryManager<T>.CreateSpan(array, ref managedRef);
#endif
		return span;
	}

#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
	/// <summary>
	/// Creates a new span using <paramref name="array"/>.
	/// </summary>
	/// <param name="array">Array instance.</param>
	/// <param name="managedRef">Reference to data.</param>
	/// <returns>Created span.</returns>
	private static Span<T> CreateSpan(Array array, ref T managedRef)
	{
		ref Pinnable<T> refPinnable = ref Unsafe.As<Array, Pinnable<T>>(ref array);
		return MemoryMarshalCompat.CreateSafeSpan(refPinnable, ref managedRef, array.Length);
	}
#endif
}