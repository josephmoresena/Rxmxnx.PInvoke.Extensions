namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface exposes a wrapper for an object that can be referenced and whose value can be modified.
/// </summary>
public interface IMutableReference : IMutableWrapper
{
	/// <summary>
	/// Creates a new instance of an object that implements <see cref="IMutableReference{TValue}"/> interface.
	/// </summary>
	/// <typeparam name="TValue">The <see cref="ValueType"/> of the object to be wrapped.</typeparam>
	/// <param name="value">The value to be wrapped.</param>
	/// <returns>An instance of an object that implements <see cref="IMutableReference{TValue}"/> interface.</returns>
	/// <remarks>
	/// The newly created object wraps a value of <typeparamref name="TValue"/> type provided by <paramref name="value"/>.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public new static IMutableReference<TValue> Create<TValue>(in TValue value = default!) where TValue : struct
		=> IMutableReference<TValue>.Create(value);
	/// <summary>
	/// Creates a new instance of an object that implements <see cref="IMutableReference{TValue}"/> interface.
	/// </summary>
	/// <typeparam name="TValue">The <see cref="ValueType"/> of the nullable object to be wrapped.</typeparam>
	/// <param name="value">The nullable value to be wrapped.</param>
	/// <returns>An instance of an object that implements <see cref="IMutableReference{TValue}"/> interface.</returns>
	/// <remarks>
	/// The newly created object wraps a nullable value of <typeparamref name="TValue"/> type provided by
	/// <paramref name="value"/>.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public new static IMutableReference<TValue?> CreateNullable<TValue>(in TValue? value = default)
		where TValue : struct
		=> IMutableReference<TValue?>.Create(value);
	/// <summary>
	/// Creates a new instance of an object that implements <see cref="IMutableReference{TObject}"/> interface.
	/// </summary>
	/// <typeparam name="TObject">The type of the object to be wrapped.</typeparam>
	/// <param name="value">The instance to be wrapped.</param>
	/// <returns>An instance of an object that implements <see cref="IMutableReference{TObject}"/> interface.</returns>
	/// <remarks>
	/// The newly created object wraps an object of <typeparamref name="TObject"/> type provided by <paramref name="value"/>.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public new static IMutableReference<TObject> CreateObject<TObject>(TObject value) where TObject : class
		=> IMutableReference<TObject>.Create(value)!;
}

/// <summary>
/// This interface exposes a wrapper for <typeparamref name="T"/> object that can be
/// referenced and whose value can be modified.
/// </summary>
/// <typeparam name="T">Type of both wrapped and referenced value.</typeparam>
/// <remarks>The provided reference is mutable, allowing changes to the value.</remarks>
public interface IMutableReference<T> : IMutableReference, IReferenceableWrapper<T>, IMutableWrapper<T>,
	IReferenceable<T>
{
	/// <summary>
	/// Reference to instance <typeparamref name="T"/> object.
	/// </summary>
	new ref T Reference { get; }

	ref T IReferenceable<T>.Reference => ref this.Reference;

	/// <summary>
	/// Creates a new instance of an object that implements <see cref="IMutableReference{T}"/> interface.
	/// </summary>
	/// <param name="instance">The value to be wrapped.</param>
	/// <returns>An instance of an object that implements <see cref="IMutableReference{T}"/> interface.</returns>
	/// <remarks>
	/// The newly created object wraps a value of <typeparamref name="T"/> type provided by
	/// <paramref name="instance"/>.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public new static IMutableReference<T?> Create(T? instance = default) => new MutableReference<T?>(instance);
}