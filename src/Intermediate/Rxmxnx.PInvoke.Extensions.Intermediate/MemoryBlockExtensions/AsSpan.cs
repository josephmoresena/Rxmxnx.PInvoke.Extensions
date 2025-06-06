#if NET6_0_OR_GREATER
namespace Rxmxnx.PInvoke;

public static partial class MemoryBlockExtensions
{
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if !PACKAGE && !NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
}
#endif