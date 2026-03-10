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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference2;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference3;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference4;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference5;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference6;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference7;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference8;
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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference9;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference10;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference11;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference12;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference13;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference14;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference15;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference16;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference17;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference18;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference19;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference20;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference21;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference22;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference23;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference24;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference25;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference26;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference27;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference28;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference29;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference30;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference31;

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
		if (array is null)
			return Memory<T>.Empty;
#if !NET6_0_OR_GREATER
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference32;

#endif
		ArrayMemoryManager<T> memoryManager = new(array);
		return memoryManager.Memory;
	}
}