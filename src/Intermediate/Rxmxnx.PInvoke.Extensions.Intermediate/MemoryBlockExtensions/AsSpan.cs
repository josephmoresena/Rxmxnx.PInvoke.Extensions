namespace Rxmxnx.PInvoke;

public static partial class MemoryBlockExtensions
{
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

#if !UAP || UAP10_0_16299
	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);

	/// <inheritdoc cref="MemoryExtensions.AsSpan{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Span<T> AsSpan<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetSpan(arr);
#endif
}