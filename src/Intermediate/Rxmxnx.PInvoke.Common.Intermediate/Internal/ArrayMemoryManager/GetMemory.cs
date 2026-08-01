namespace Rxmxnx.PInvoke.Internal;

internal partial class ArrayMemoryManager<T>
{
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset2(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset3(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset4(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset5(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset6(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset7(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset8(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset9(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset10(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset11(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset12(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset13(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset14(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset15(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset16(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset17(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset18(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset19(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset20(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset21(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset22(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset23(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset24(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset25(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset26(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset27(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset28(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset29(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset30(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset31(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
#if !PACKAGE && NET6_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
#if !NET6_0_OR_GREATER
		if (array is null)
#else
		if (array is null || array.Length == 0)
#endif
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.offsets[array.Rank - 2] ??= ArrayMemoryManager<T>.ComputeOffset32(array);
#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
}