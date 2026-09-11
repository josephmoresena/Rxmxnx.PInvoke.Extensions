namespace Rxmxnx.PInvoke;

public static partial class MemoryBlockExtensions
{
	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
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