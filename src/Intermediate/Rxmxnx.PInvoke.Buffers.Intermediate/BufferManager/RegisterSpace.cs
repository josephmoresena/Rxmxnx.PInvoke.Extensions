#if NET7_0_OR_GREATER && BINARY_SPACES
namespace Rxmxnx.PInvoke;

public static partial class BufferManager
{
	/// <summary>
	/// Registers object space.
	/// </summary>
	/// <typeparam name="TSpace">Type of object space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace<TSpace>() where TSpace : struct, IManagedBinaryBuffer<TSpace, Object>
		=> MetadataManager<Object>.RegisterBufferSpace<TSpace>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TSpace">Type of <typeparamref name="T"/> buffer.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T> where T : struct
		=> MetadataManager<T>.RegisterBufferSpace<TSpace>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of nullable items in the space.</typeparam>
	/// <typeparam name="TSpace">Type of <see name="Nullable{T}"/> space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterNullableSpace<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T?> where T : struct
		=> MetadataManager<T?>.RegisterBufferSpace<TSpace>();
	/// <summary>
	/// Registers object space.
	/// </summary>
	/// <typeparam name="TSpace">Type of object space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace2<TSpace>() where TSpace : struct, IManagedBinaryBuffer<TSpace, Object>
		=> BufferManager.RegisterSpace<Composite<TSpace, TSpace, Object>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TSpace">Type of <typeparamref name="T"/> buffer.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace2<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T> where T : struct
		=> BufferManager.RegisterSpace<T, Composite<TSpace, TSpace, T>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of nullable items in the space.</typeparam>
	/// <typeparam name="TSpace">Type of <see name="Nullable{T}"/> space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterNullableSpace2<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T?> where T : struct
		=> BufferManager.RegisterNullableSpace<T, Composite<TSpace, TSpace, T?>>();
	/// <summary>
	/// Registers object space.
	/// </summary>
	/// <typeparam name="TSpace">Type of object space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace4<TSpace>() where TSpace : struct, IManagedBinaryBuffer<TSpace, Object>
		=> BufferManager.RegisterSpace2<Composite<TSpace, TSpace, Object>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TSpace">Type of <typeparamref name="T"/> buffer.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace4<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T> where T : struct
		=> BufferManager.RegisterSpace2<T, Composite<TSpace, TSpace, T>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of nullable items in the space.</typeparam>
	/// <typeparam name="TSpace">Type of <see name="Nullable{T}"/> space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterNullableSpace4<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T?> where T : struct
		=> BufferManager.RegisterNullableSpace2<T, Composite<TSpace, TSpace, T?>>();
	/// <summary>
	/// Registers object space.
	/// </summary>
	/// <typeparam name="TSpace">Type of object space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace8<TSpace>() where TSpace : struct, IManagedBinaryBuffer<TSpace, Object>
		=> BufferManager.RegisterSpace4<Composite<TSpace, TSpace, Object>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TSpace">Type of <typeparamref name="T"/> buffer.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace8<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T> where T : struct
		=> BufferManager.RegisterSpace4<T, Composite<TSpace, TSpace, T>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of nullable items in the space.</typeparam>
	/// <typeparam name="TSpace">Type of <see name="Nullable{T}"/> space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterNullableSpace8<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T?> where T : struct
		=> BufferManager.RegisterNullableSpace4<T, Composite<TSpace, TSpace, T?>>();
	/// <summary>
	/// Registers object space.
	/// </summary>
	/// <typeparam name="TSpace">Type of object space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace16<TSpace>() where TSpace : struct, IManagedBinaryBuffer<TSpace, Object>
		=> BufferManager.RegisterSpace8<Composite<TSpace, TSpace, Object>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TSpace">Type of <typeparamref name="T"/> buffer.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace16<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T> where T : struct
		=> BufferManager.RegisterSpace8<T, Composite<TSpace, TSpace, T>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of nullable items in the space.</typeparam>
	/// <typeparam name="TSpace">Type of <see name="Nullable{T}"/> space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterNullableSpace16<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T?> where T : struct
		=> BufferManager.RegisterNullableSpace8<T, Composite<TSpace, TSpace, T?>>();

	/// <summary>
	/// Registers object space.
	/// </summary>
	/// <typeparam name="TSpace">Type of object space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace32<TSpace>() where TSpace : struct, IManagedBinaryBuffer<TSpace, Object>
		=> BufferManager.RegisterSpace16<Composite<TSpace, TSpace, Object>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TSpace">Type of <typeparamref name="T"/> buffer.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace32<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T> where T : struct
		=> BufferManager.RegisterSpace16<T, Composite<TSpace, TSpace, T>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of nullable items in the space.</typeparam>
	/// <typeparam name="TSpace">Type of <see name="Nullable{T}"/> space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterNullableSpace32<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T?> where T : struct
		=> BufferManager.RegisterNullableSpace16<T, Composite<TSpace, TSpace, T?>>();
	/// <summary>
	/// Registers object space.
	/// </summary>
	/// <typeparam name="TSpace">Type of object space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace64<TSpace>() where TSpace : struct, IManagedBinaryBuffer<TSpace, Object>
		=> BufferManager.RegisterSpace32<Composite<TSpace, TSpace, Object>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TSpace">Type of <typeparamref name="T"/> buffer.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace64<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T> where T : struct
		=> BufferManager.RegisterSpace32<T, Composite<TSpace, TSpace, T>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of nullable items in the space.</typeparam>
	/// <typeparam name="TSpace">Type of <see name="Nullable{T}"/> space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterNullableSpace64<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T?> where T : struct
		=> BufferManager.RegisterNullableSpace32<T, Composite<TSpace, TSpace, T?>>();

	/// <summary>
	/// Registers object space.
	/// </summary>
	/// <typeparam name="TSpace">Type of object space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace128<TSpace>() where TSpace : struct, IManagedBinaryBuffer<TSpace, Object>
		=> BufferManager.RegisterSpace64<Composite<TSpace, TSpace, Object>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TSpace">Type of <typeparamref name="T"/> buffer.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace128<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T> where T : struct
		=> BufferManager.RegisterSpace64<T, Composite<TSpace, TSpace, T>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of nullable items in the space.</typeparam>
	/// <typeparam name="TSpace">Type of <see name="Nullable{T}"/> space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterNullableSpace128<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T?> where T : struct
		=> BufferManager.RegisterNullableSpace64<T, Composite<TSpace, TSpace, T?>>();

	/// <summary>
	/// Registers object space.
	/// </summary>
	/// <typeparam name="TSpace">Type of object space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace256<TSpace>() where TSpace : struct, IManagedBinaryBuffer<TSpace, Object>
		=> BufferManager.RegisterSpace128<Composite<TSpace, TSpace, Object>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TSpace">Type of <typeparamref name="T"/> buffer.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace256<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T> where T : struct
		=> BufferManager.RegisterSpace128<T, Composite<TSpace, TSpace, T>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of nullable items in the space.</typeparam>
	/// <typeparam name="TSpace">Type of <see name="Nullable{T}"/> space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterNullableSpace256<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T?> where T : struct
		=> BufferManager.RegisterNullableSpace128<T, Composite<TSpace, TSpace, T?>>();
	/// <summary>
	/// Registers object space.
	/// </summary>
	/// <typeparam name="TSpace">Type of object space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace512<TSpace>() where TSpace : struct, IManagedBinaryBuffer<TSpace, Object>
		=> BufferManager.RegisterSpace256<Composite<TSpace, TSpace, Object>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TSpace">Type of <typeparamref name="T"/> buffer.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace512<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T> where T : struct
		=> BufferManager.RegisterSpace256<T, Composite<TSpace, TSpace, T>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of nullable items in the space.</typeparam>
	/// <typeparam name="TSpace">Type of <see name="Nullable{T}"/> space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterNullableSpace512<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T?> where T : struct
		=> BufferManager.RegisterNullableSpace256<T, Composite<TSpace, TSpace, T?>>();
	/// <summary>
	/// Registers object space.
	/// </summary>
	/// <typeparam name="TSpace">Type of object space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace1024<TSpace>() where TSpace : struct, IManagedBinaryBuffer<TSpace, Object>
		=> BufferManager.RegisterSpace512<Composite<TSpace, TSpace, Object>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TSpace">Type of <typeparamref name="T"/> buffer.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterSpace1024<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T> where T : struct
		=> BufferManager.RegisterSpace512<T, Composite<TSpace, TSpace, T>>();
	/// <summary>
	/// Registers <typeparamref name="T"/> space.
	/// </summary>
	/// <typeparam name="T">Type of nullable items in the space.</typeparam>
	/// <typeparam name="TSpace">Type of <see name="Nullable{T}"/> space.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RegisterNullableSpace1024<T, TSpace>()
		where TSpace : struct, IManagedBinaryBuffer<TSpace, T?> where T : struct
		=> BufferManager.RegisterNullableSpace512<T, Composite<TSpace, TSpace, T?>>();
}
#endif