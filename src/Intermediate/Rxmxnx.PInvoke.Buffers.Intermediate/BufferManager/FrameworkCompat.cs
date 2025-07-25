namespace Rxmxnx.PInvoke;

public static partial class BufferManager
{
#if !NET7_0_OR_GREATER
	/// <summary>
	/// Metadata cache.
	/// </summary>
	private static readonly ConcurrentDictionary<Type, BufferTypeMetadata> metadataCache = new();

	/// <summary>
	/// Registry count.
	/// </summary>
	[ThreadStatic]
	private static Int32 countRegister;
#endif

	/// <summary>
	/// Retrieves metadata required for a buffer of <paramref name="bufferType"/> type.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer</typeparam>
	/// <param name="bufferType">Type of buffer.</param>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
#if NET5_0_OR_GREATER
	[UnconditionalSuppressMessage("Trimming", "IL2070")]
#endif
	private static BufferTypeMetadata<T> GetMetadata<T>(
#if NET5_0_OR_GREATER
		[DynamicallyAccessedMembers(BufferManager.DynamicallyAccessedMembers)]
#endif
		Type bufferType)
	{
#if !NET7_0_OR_GREATER
		if (BufferManager.metadataCache.TryGetValue(bufferType, out BufferTypeMetadata? result))
			return (BufferTypeMetadata<T>)result;
#else
		BufferTypeMetadata? result = default;
#endif

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
		result ??= ManagedBinaryBuffer<T>.GetMetadata(bufferType);
#if !NET7_0_OR_GREATER
		return BufferManager.Cache(bufferType, result as BufferTypeMetadata<T>);
#else
		ValidationUtilities.ThrowIfNullMetadata(bufferType, result is not BufferTypeMetadata<T>);
		return (result as BufferTypeMetadata<T>)!;
#endif
	}
#if !NET7_0_OR_GREATER
	/// <summary>
	/// Retrieves metadata required for a buffer of <typeparamref name="TBuffer"/> type.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer</typeparam>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	private static BufferTypeMetadata<T> GetMetadata<T, TBuffer>() where TBuffer : struct, IManagedBuffer<T>
	{
		if (BufferManager.countRegister < 0) BufferManager.countRegister = 0;
		if (BufferManager.metadataCache.TryGetValue(typeof(TBuffer), out BufferTypeMetadata? result))
			return (BufferTypeMetadata<T>)result;

		// This method allocates the buffer in the current stack.
		return BufferManager.GetStaticMetadata<T, TBuffer>();
	}
	/// <summary>
	/// Retrieves the static metadata required for a buffer of <typeparamref name="TBuffer"/> type.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer</typeparam>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	private static BufferTypeMetadata<T> GetStaticMetadata<T, TBuffer>() where TBuffer : struct, IManagedBuffer<T>
	{
		BufferManager.countRegister++;
		try
		{
#if !NET7_0_OR_GREATER
			BufferTypeMetadata<T> staticMetadata = new TBuffer().GetStaticTypeMetadata();
			return BufferManager.Cache(staticMetadata.BufferType, staticMetadata);
#else
			return IManagedBuffer<T>.GetMetadata<TBuffer>();
#endif
		}
		finally
		{
			if (BufferManager.countRegister > 0) BufferManager.countRegister--;
		}
	}
#nullable disable
	/// <summary>
	/// Caches <paramref name="typeMetadata"/> for <paramref name="bufferType"/>.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer</typeparam>
	/// <param name="bufferType">Type of buffer.</param>
	/// <param name="typeMetadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	private static BufferTypeMetadata<T> Cache<T>(Type bufferType, BufferTypeMetadata<T> typeMetadata)
	{
		ValidationUtilities.ThrowIfNullMetadata(bufferType, typeMetadata is null);
		BufferManager.metadataCache.TryAdd(bufferType, typeMetadata);
		return typeMetadata;
	}
#nullable restore
#endif
}