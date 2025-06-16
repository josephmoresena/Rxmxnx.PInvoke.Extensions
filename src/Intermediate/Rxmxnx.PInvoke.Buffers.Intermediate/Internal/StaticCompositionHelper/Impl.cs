namespace Rxmxnx.PInvoke.Internal;

#if (!PACKAGE && NET6_0 || NET7_0_OR_GREATER) && BINARY_SPACES
internal partial class StaticCompositionHelper<T>
{
	/// <summary>
	/// Creates new <see cref="StaticCompositionHelper{T}"/> instance for <typeparamref name="TBuffer"/>.
	/// </summary>
	/// <typeparam name="TBuffer">A <see cref="IManagedBuffer{T}"/> type.</typeparam>
	/// <returns>A new <see cref="StaticCompositionHelper{T}"/> instance for <typeparamref name="TBuffer"/>.</returns>
	public static StaticCompositionHelper<T> Create<TBuffer>() where TBuffer : struct, IManagedBinaryBuffer<TBuffer, T>
		=> new Impl<TBuffer>();

	/// <summary>
	/// Generic implementation of <see cref="StaticCompositionHelper{T}"/>
	/// </summary>
	/// <typeparam name="TBuffer">A <see cref="IManagedBuffer{T}"/> type.</typeparam>
	private sealed class Impl<TBuffer> : StaticCompositionHelper<T>
		where TBuffer : struct, IManagedBinaryBuffer<TBuffer, T>
	{
		/// <summary>
		/// Buffer type metadata.
		/// </summary>
		private static readonly BufferTypeMetadata<T> metadata = IManagedBuffer<T>.GetMetadata<TBuffer>();

		/// <summary>
		/// Constructor.
		/// </summary>
		public Impl() : base(Impl<TBuffer>.metadata.Size) => this.Add(Impl<TBuffer>.metadata);
	}
}
#endif