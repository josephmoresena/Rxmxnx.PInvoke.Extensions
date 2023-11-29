namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Fixed memory reference class, used to hold a fixed memory reference of a specific type.
/// </summary>
/// <typeparam name="T">Type of the fixed memory reference.</typeparam>
internal sealed unsafe partial class FixedReference<T> : FixedMemory, IFixedReference<T>, IEquatable<FixedReference<T>>
	where T : unmanaged
{
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
	public FixedReference(void* ptr) : base(ptr, sizeof(T)) { }

	/// <summary>
	/// Constructor that takes a <see cref="FixedMemory"/> instance.
	/// </summary>
	/// <param name="mem">Instance of <see cref="FixedMemory"/> to be referenced.</param>
	private FixedReference(FixedMemory mem) : base(mem) { }
	/// <inheritdoc/>
	public Boolean Equals(FixedReference<T>? other) => this.Equals(other as FixedMemory);

	ref T IReferenceable<T>.Reference => ref this.CreateReference<T>();
	ref readonly T IReadOnlyReferenceable<T>.Reference => ref this.CreateReadOnlyReference<T>();
	IFixedReference<TDestination> IFixedReference<T>.Transformation<TDestination>(out IFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		IFixedReference<TDestination> result = this.GetTransformation<TDestination>(out Unsafe.As<IFixedMemory, FixedOffset>(ref residual));
		return result;
	}
	IFixedReference<TDestination> IFixedReference<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		IFixedReference<TDestination> result = this.GetTransformation<TDestination>(out Unsafe.As<IReadOnlyFixedMemory, FixedOffset>(ref residual));
		return result;
	}
	IReadOnlyFixedReference<TDestination> IReadOnlyFixedReference<T>.Transformation<TDestination>(
		out IReadOnlyFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		IReadOnlyFixedReference<TDestination> result =
			this.GetTransformation<TDestination>(out Unsafe.As<IReadOnlyFixedMemory, FixedOffset>(ref residual), true);
		return result;
	}

	/// <summary>
	/// Transforms the current memory reference into a different type and provides a fixed offset that represents the remaining
	/// portion of memory not included in the newly formed reference.
	/// </summary>
	/// <typeparam name="TDestination">The type into which the current memory reference should be transformed.</typeparam>
	/// <param name="fixedOffset">
	/// Output. Provides a fixed offset that represents the remaining portion of memory that is not
	/// included in the new reference. This is calculated based on the size of the new type compared to the size of the
	/// original reference.
	/// </param>
	/// <param name="isReadOnly">Indicates whether the transformation operation should be performed as a read-only operation.</param>
	/// <returns>
	/// A new instance of FixedReference for the destination type, which represents a fixed memory reference of the
	/// new type.
	/// </returns>
	/// <exception cref="InsufficientMemoryException">
	/// Thrown when the size of the current reference is not sufficient to
	/// accommodate the new type. For example, if an attempt is made to transform a 2-byte reference into a 4-byte type.
	/// </exception>
	public FixedReference<TDestination> GetTransformation<TDestination>(out FixedOffset fixedOffset,
		Boolean isReadOnly = false) where TDestination : unmanaged
	{
		this.ValidateOperation(isReadOnly);
		this.ValidateReferenceSize<TDestination>();
		fixedOffset = new(this, sizeof(TDestination));
		return new(this);
	}

	/// <inheritdoc/>
	public override Boolean Equals(FixedMemory? other) => base.Equals(other as FixedReference<T>);
	/// <inheritdoc/>
	public override Boolean Equals(Object? obj) => base.Equals(obj as FixedReference<T>);
	/// <inheritdoc/>
	public override Int32 GetHashCode() => base.GetHashCode();
}