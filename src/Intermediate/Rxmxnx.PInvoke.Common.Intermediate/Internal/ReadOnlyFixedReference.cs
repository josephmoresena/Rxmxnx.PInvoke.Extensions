namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Fixed memory reference class, used to hold a fixed read-only memory reference of a specific type.
/// </summary>
/// <typeparam name="T">Type of the fixed memory reference.</typeparam>
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
internal sealed unsafe partial class ReadOnlyFixedReference<T> : ReadOnlyFixedMemory, IReadOnlyFixedReference<T>
{
#pragma warning disable CS8500
	/// <inheritdoc/>
	public override Boolean IsUnmanaged => ReadOnlyValPtr<T>.IsUnmanaged;
	/// <inheritdoc/>
	public override Type Type => typeof(T);
	/// <inheritdoc/>
	public override Int32 BinaryOffset => default;
	/// <inheritdoc/>
	public override Boolean IsFunction => false;

	/// <summary>
	/// Constructor that takes a pointer to a fixed memory reference.
	/// </summary>
	/// <param name="ptr">Pointer to the fixed memory reference.</param>
	public ReadOnlyFixedReference(void* ptr) : base(ptr, sizeof(T), true) { }

	/// <summary>
	/// Constructor that takes a <see cref="FixedMemory"/> instance.
	/// </summary>
	/// <param name="mem">Instance of <see cref="FixedMemory"/> to be referenced.</param>
	private ReadOnlyFixedReference(ReadOnlyFixedMemory mem) : base(mem) { }

	ref readonly T IReadOnlyReferenceable<T>.Reference => ref this.CreateReadOnlyReference<T>();
	IReadOnlyFixedReference<TDestination> IReadOnlyFixedReference<T>.Transformation<TDestination>(
		out IReadOnlyFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		IReadOnlyFixedReference<TDestination> result =
			this.GetTransformation<TDestination>(
				out Unsafe.As<IReadOnlyFixedMemory, ReadOnlyFixedOffset>(ref residual));
		return result;
	}

	/// <summary>
	/// Transforms the current memory reference into a different type and provides a fixed offset that represents the remaining
	/// portion of memory not included in the newly formed reference.
	/// </summary>
	/// <typeparam name="TDestination">The type into which the current memory reference should be transformed.</typeparam>
	/// <param name="fixedOffset">
	/// Output. Provides a fixed offset that represents the remaining portion of memory that is not included in the new
	/// reference.
	/// This is calculated based on the size of the new type compared to the size of the original reference.
	/// </param>
	/// <returns>
	/// A new instance of FixedReference for the destination type, which represents a fixed memory reference of the
	/// new type.
	/// </returns>
	/// <exception cref="InsufficientMemoryException">
	/// Thrown when the size of the current reference is not sufficient to
	/// accommodate the new type. For example, if an attempt is made to transform a 2-byte reference into a 4-byte type.
	/// </exception>
	public ReadOnlyFixedReference<TDestination> GetTransformation<TDestination>(out ReadOnlyFixedOffset fixedOffset)
	{
		this.ValidateOperation(true);
		this.ValidateTransformation(typeof(TDestination), ReadOnlyValPtr<TDestination>.IsUnmanaged);
		Int32 sizeOf = sizeof(TDestination);
		this.ValidateReferenceSize(typeof(TDestination), sizeOf);
		fixedOffset = new(this, sizeOf);
		return new(this);
	}
#pragma warning restore CS8500
}