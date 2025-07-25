#if !NET6_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal;

internal partial class ArrayMemoryManager<T>
{
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference2;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference3;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference4;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference5;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference6;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference7;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference8;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference9;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference10;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference11;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference12;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference13;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference14;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference15;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference16;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference17;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference18;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference19;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference20;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference21;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference22;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference23;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference24;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference25;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference26;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference27;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference28;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference29;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference30;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference31;
		return new ArrayMemoryManager<T>(array).Memory;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null)
			return Memory<T>.Empty;
		ArrayMemoryManager<T>.ranks[array.Rank - 2] ??= ArrayMemoryManager<T>.GetArrayDataReference32;
		return new ArrayMemoryManager<T>(array).Memory;
	}
}
#endif