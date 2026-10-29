namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// This interface exposes a binary managed buffer.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
public interface IManagedBinaryBuffer<T> : IManagedBuffer<T>
{
	/// <summary>
	/// Buffer type metadata.
	/// </summary>
	BufferTypeMetadata<T> Metadata { get; }
}

/// <summary>
/// This interface exposes a binary managed buffer.
/// </summary>
/// <typeparam name="TBuffer">The type of buffer.</typeparam>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS107)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2436)]
#endif
public interface
	IManagedBinaryBuffer<[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBuffer,
		T> : IManagedBinaryBuffer<T> where TBuffer : struct, IManagedBinaryBuffer<TBuffer, T>
{
#if NET7_0_OR_GREATER
	/// <summary>
	/// Indicates whether the buffer type satisfies the binary composition rule.
	/// </summary>
	internal static abstract Boolean IsStaticBinary { get; }
#endif
}