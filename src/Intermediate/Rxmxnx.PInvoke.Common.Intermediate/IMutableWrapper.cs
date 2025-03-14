namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface defines a wrapper for an object whose value can be modified.
/// </summary>
public interface IMutableWrapper : IWrapper
{
	/// <summary>
	/// Creates a new instance of an object that implements the <see cref="IMutableWrapper{TValue}"/> interface.
	/// </summary>
	/// <typeparam name="TValue">The <see cref="ValueType"/> of the object to be wrapped.</typeparam>
	/// <param name="value">The value to be wrapped.</param>
	/// <returns>
	/// An instance of an object implementing the <see cref="IMutableWrapper{TValue}"/> interface, wrapping the
	/// value provided by <paramref name="value"/>.
	/// </returns>
	/// <remarks>
	/// The newly created object wraps a value of <typeparamref name="TValue"/> type provided by
	/// <paramref name="value"/>.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public new static IMutableWrapper<TValue> Create<TValue>(in TValue value = default) where TValue : struct
		=> IMutableWrapper<TValue>.Create(value);
	/// <summary>
	/// Creates a new instance of an object that implements the <see cref="IMutableWrapper{TValue}"/> interface.
	/// </summary>
	/// <typeparam name="TValue">The <see cref="ValueType"/> of the nullable object to be wrapped.</typeparam>
	/// <param name="value">The nullable value to be wrapped.</param>
	/// <returns>
	/// An instance of an object implementing the <see cref="IMutableWrapper{TValue}"/> interface, wrapping the nullable
	/// value provided by <paramref name="value"/>.
	/// </returns>
	/// <remarks>
	/// The newly created object wraps a nullable value of <typeparamref name="TValue"/> type provided by
	/// <paramref name="value"/>.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public new static IMutableWrapper<TValue?> CreateNullable<TValue>(in TValue? value = default) where TValue : struct
		=> IMutableWrapper<TValue?>.Create(value);
	/// <summary>
	/// Creates a new instance of an object that implements the <see cref="IMutableWrapper{TObject}"/> interface.
	/// </summary>
	/// <typeparam name="TObject">The type of the object to be wrapped.</typeparam>
	/// <param name="instance">The instance to be wrapped.</param>
	/// <returns>
	/// An instance of an object implementing the <see cref="IMutableWrapper{TObject}"/> interface, wrapping the
	/// object provided by <paramref name="instance"/>.
	/// </returns>
	/// <remarks>
	/// The newly created object wraps an object of <typeparamref name="TObject"/> type provided by
	/// <paramref name="instance"/>.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public new static IMutableWrapper<TObject> CreateObject<TObject>(TObject instance) where TObject : class
		=> IMutableWrapper<TObject>.Create(instance)!;

	/// <summary>
	/// This interface defines a wrapper for a <typeparamref name="T"/> object whose value can be modified.
	/// </summary>
	/// <typeparam name="T">The type of value to be wrapped.</typeparam>
	/// <remarks>This interface is covariant.</remarks>
	public new interface IBase<T> : IMutableWrapper, IWrapper.IBase<T>
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
	{
		/// <summary>
		/// The wrapped <typeparamref name="T"/> object.
		/// </summary>
		new T Value { get; set; }
	}
}

/// <summary>
/// This interface defines a wrapper for a <typeparamref name="T"/> object whose value can be modified.
/// </summary>
/// <typeparam name="T">The type of value to be wrapped.</typeparam>
public interface IMutableWrapper<T> : IMutableWrapper.IBase<T>, IWrapper<T>
{
	/// <summary>
	/// The wrapped <typeparamref name="T"/> object.
	/// </summary>
	new T Value { get; set; }

	T IBase<T>.Value
	{
		get => this.Value;
		set => this.Value = value;
	}

	T IWrapper<T>.Value => this.Value;

	/// <summary>
	/// Creates a new instance of an object that implements the <see cref="IMutableWrapper{T}"/> interface.
	/// </summary>
	/// <param name="instance">The value to be wrapped.</param>
	/// <returns>
	/// An instance of an object implementing the <see cref="IMutableWrapper{T}"/> interface, wrapping the value
	/// provided by <paramref name="instance"/>.
	/// </returns>
	/// <remarks>
	/// The newly created object wraps a value of <typeparamref name="T"/> type provided by
	/// <paramref name="instance"/>.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public new static IMutableWrapper<T?> Create(T? instance = default) => new MutableWrapper<T?>(instance);
}