#if !PACKAGE || (PACKAGE || !NET6_0) && !NET7_0_OR_GREATER
namespace Rxmxnx.PInvoke;

public static partial class BufferManager
{
	/// <summary>
	/// Metadata cache.
	/// </summary>
	private static readonly ConcurrentDictionary<Type, BufferTypeMetadata> metadataCache = new();

	/// <summary>
	/// Retrieves metadata required for a buffer of <paramref name="bufferType"/> type.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer</typeparam>
	/// <param name="bufferType">Type of buffer.</param>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	private static BufferTypeMetadata<T> GetMetadata<T>(
#if NET5_0_OR_GREATER
		[DynamicallyAccessedMembers(BufferManager.DynamicallyAccessedMembers)]
#endif
		Type bufferType)
	{
		if (BufferManager.metadataCache.TryGetValue(bufferType, out BufferTypeMetadata? result))
			return (BufferTypeMetadata<T>)result;

		try
		{
			if (!AotInfo.IsNativeAot || !AotInfo.IsReflectionDisabled)
			{
				FieldInfo? typeMetadataInfo =
					bufferType.GetField(BufferManager.TypeMetadataName, BufferManager.GetMetadataFlags);
				result = (BufferTypeMetadata?)typeMetadataInfo?.GetValue(null);
			}
		}
		catch (TargetInvocationException tie)
		{
			if (tie.InnerException is not null)
				throw tie.InnerException;
		}
		result ??= IManagedBinaryBuffer<T>.GetMetadata(bufferType);
		return BufferManager.Cache(bufferType, result as BufferTypeMetadata<T>);
	}
#nullable disable
	/// <summary>
	/// Caches <paramref name="typeMetadata"/> for <paramref name="bufferType"/>.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer</typeparam>
	/// <param name="bufferType">Type of buffer.</param>
	/// <param name="typeMetadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	private static BufferTypeMetadata<T> Cache<T>(
#if NET5_0_OR_GREATER
		[DynamicallyAccessedMembers(BufferManager.DynamicallyAccessedMembers)]
#endif
		Type bufferType, BufferTypeMetadata<T> typeMetadata)
	{
		ValidationUtilities.ThrowIfNullMetadata(bufferType, typeMetadata is null);
		BufferManager.metadataCache.TryAdd(bufferType, typeMetadata);
		return typeMetadata;
	}
#nullable restore
}
#endif