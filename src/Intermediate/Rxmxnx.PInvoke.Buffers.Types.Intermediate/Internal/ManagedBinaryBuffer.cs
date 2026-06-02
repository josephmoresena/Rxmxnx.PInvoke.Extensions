namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for <see cref="IManagedBinaryBuffer{T}"/> implementations.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
internal static class ManagedBinaryBuffer<T>
{
#if NET7_0_OR_GREATER
	/// <summary>
	/// <see cref="MethodInfo"/> to retrieve buffer metadata.
	/// </summary>
	// ReSharper disable once StaticMemberInGenericType
	private static MethodInfo? getMetadataInfo;

	/// <summary>
	/// <see cref="MethodInfo"/> to retrieve buffer metadata.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static MethodInfo? GetMetadataInfo
	{
		get
		{
			try
			{
				if (ManagedBinaryBuffer<T>.getMetadataInfo is null && BuffersHelper.BufferAutoCompositionEnabled)
					ManagedBinaryBuffer<T>.getMetadataInfo = ManagedBinaryBuffer<T>.ReflectGetMetadataMethod();
			}
			catch (Exception)
			{
				// ignored
			}
			return ManagedBinaryBuffer<T>.getMetadataInfo;
		}
	}
#endif
	/// <summary>
	/// Retrieves the <see cref="BufferTypeMetadata{T}"/> instance from <paramref name="bufferType"/>.
	/// </summary>
	/// <param name="bufferType">Type of buffer</param>
	/// <returns>The <see cref="BufferTypeMetadata{T}"/> instance from <paramref name="bufferType"/>.</returns>
	/// <remarks>
	/// This method allocates in heap a <paramref name="bufferType"/> instance to retrieve the
	/// <see cref="BufferTypeMetadata{T}"/> instance.
	/// </remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[UnconditionalSuppressMessage("Trimming", "IL2067")]
	public static BufferTypeMetadata<T>? GetMetadata(
		[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] Type? bufferType)
	{
		if (bufferType is null) return default;
		try
		{
			// This allocates a buffer in heap temporally.
			IManagedBinaryBuffer<T> binaryBuffer = (IManagedBinaryBuffer<T>)Activator.CreateInstance(bufferType)!;
			return binaryBuffer.Metadata;
		}
		catch (Exception)
		{
			// ignored
		}
		return default;
	}

#if NET7_0_OR_GREATER
	/// <summary>
	/// Retrieves the reflected <see cref="IManagedBuffer{T}.GetMetadata{TBuffer}()"/> method.
	/// </summary>
	/// <returns>A <see cref="MethodInfo"/> instance.</returns>
	private static MethodInfo ReflectGetMetadataMethod()
	{
		Type typeofT = typeof(IManagedBuffer<T>);
		return typeofT.GetMethod(nameof(IManagedBuffer<>.GetMetadata), BuffersHelper.GetMetadataFlags)!;
	}
#endif
}