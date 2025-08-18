namespace Rxmxnx.PInvoke;

public static partial class MemoryBlockExtensions
{
	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif
	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif
	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif
	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif
	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif
	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif
	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetMemory(arr);
#else
		=> ArrayMemoryManager<T>.GetMemory((Array?)arr);
#endif
}