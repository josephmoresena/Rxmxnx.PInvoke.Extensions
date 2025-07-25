#if !NET6_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal;

internal partial class ArrayMemoryManager<T>
{
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference2;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference3;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference4;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference5;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference6;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference7;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference8;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference9;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference10;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference11;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference12;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference13;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference14;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference15;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference16;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference17;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference18;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference19;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference20;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference21;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference22;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference23;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference24;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference25;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference26;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference27;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference28;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference29;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference30;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference31;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,]? array)
	{
		if (array is null) return default;
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2] ??=
			ArrayMemoryManager<T>.GetArrayDataReference32;
		ref T managedRef = ref getArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
}
#endif