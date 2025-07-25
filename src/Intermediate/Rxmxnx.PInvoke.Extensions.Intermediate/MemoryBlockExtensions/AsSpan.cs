namespace Rxmxnx.PInvoke;

public static partial class MemoryBlockExtensions
{
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
#if !NET6_0_OR_GREATER
		=> ArrayMemoryManager<T>.GetSpan(arr);
#else
		=> ArrayMemoryManager<T>.GetSpan((Array?)arr);
#endif
}