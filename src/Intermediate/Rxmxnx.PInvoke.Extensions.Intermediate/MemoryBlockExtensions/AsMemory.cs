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
	public static Memory<T> AsMemory<T>(this T[,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);
	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,]? arr) => ArrayMemoryManager<T>.GetMemory(arr);

	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
		=> ArrayMemoryManager<T>.GetMemory(arr);
	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
		=> ArrayMemoryManager<T>.GetMemory(arr);
	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
		=> ArrayMemoryManager<T>.GetMemory(arr);
	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
		=> ArrayMemoryManager<T>.GetMemory(arr);
	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
		=> ArrayMemoryManager<T>.GetMemory(arr);
	/// <inheritdoc cref="MemoryExtensions.AsMemory{T}(T[])"/>
	/// <remarks>This method is incompatible with the first versions of .NET Native.</remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> AsMemory<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? arr)
		=> ArrayMemoryManager<T>.GetMemory(arr);
}