namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface defines a wrapper object.
/// </summary>
public interface IWrapper
{
	/// <summary>
	/// Creates a new instance of an object that implements <see cref="IWrapper{TValue}"/> interface.
	/// </summary>
	/// <typeparam name="TValue">The <see cref="ValueType"/> of the object to be wrapped.</typeparam>
	/// <param name="value">The value to be wrapped.</param>
	/// <returns>An instance of an object that implements <see cref="IWrapper{TValue}"/> interface.</returns>
	/// <remarks>
	/// The newly created object wraps a value of <typeparamref name="TValue"/> type provided by <paramref name="value"/>.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IWrapper<TValue> Create<TValue>(in TValue value) where TValue : struct
		=> IWrapper<TValue>.Create(value);
	/// <summary>
	/// Creates a new instance of an object that implements <see cref="IWrapper{TValue}"/> interface.
	/// </summary>
	/// <typeparam name="TValue">The <see cref="ValueType"/> of the nullable object to be wrapped.</typeparam>
	/// <param name="value">The nullable value to be wrapped.</param>
	/// <returns>An instance of an object that implements <see cref="IWrapper{TValue}"/> interface.</returns>
	/// <remarks>
	/// The newly created object wraps a nullable value of <typeparamref name="TValue"/> type provided by
	/// <paramref name="value"/>.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IWrapper<TValue?> CreateNullable<TValue>(in TValue? value) where TValue : struct
		=> IWrapper<TValue?>.Create(value);
	/// <summary>
	/// Creates a new instance of an object that implements <see cref="IWrapper{TObject}"/> interface.
	/// </summary>
	/// <typeparam name="TObject">The type of the object to be wrapped.</typeparam>
	/// <param name="instance">The instance to be wrapped.</param>
	/// <returns>An instance of an object that implements <see cref="IWrapper{TObject}"/> interface.</returns>
	/// <remarks>
	/// The newly created object wraps an object of <typeparamref name="TObject"/> type provided by <paramref name="instance"/>
	/// .
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IWrapper<TObject> CreateObject<TObject>(TObject instance) where TObject : class
		=> IWrapper<TObject>.Create(instance)!;

	/// <summary>
	/// This interface defines a wrapper for a <typeparamref name="T"/> object.
	/// </summary>
	/// <typeparam name="T">The type of value to be wrapped.</typeparam>
	/// <remarks>This interface is covariant.</remarks>
	public interface Base<out T> : IWrapper
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
	{
		/// <summary>
		/// The wrapped <typeparamref name="T"/> object.
		/// </summary>
		T Value { get; }
	}
}

/// <summary>
/// This interface defines a wrapper for a <typeparamref name="T"/> object.
/// </summary>
/// <typeparam name="T">The type of value to be wrapped.</typeparam>
public interface IWrapper<T> : IWrapper.Base<T>, IEquatable<T>
{
	/// <summary>
	/// The wrapped <typeparamref name="T"/> object.
	/// </summary>
	new T Value { get; }

	T Base<T>.Value => this.Value;
	Boolean IEquatable<T>.Equals(T? other) => Object.Equals(this.Value, other);

	/// <summary>
	/// Creates a new instance of an object that implements <see cref="IWrapper{T}"/> interface.
	/// </summary>
	/// <param name="instance">The value to be wrapped.</param>
	/// <returns>An instance of an object that implements <see cref="IWrapper{T}"/> interface.</returns>
	/// <remarks>
	/// The newly created object wraps a value of <typeparamref name="T"/> type provided by
	/// <paramref name="instance"/>.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IWrapper<T?> Create(T? instance) => new Input<T?>(instance);
}