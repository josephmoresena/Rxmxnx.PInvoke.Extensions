namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of utilities for wrapper types instantiation.
/// </summary>
public static class WrapperFactory
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
#if NETCOREAPP3_0_OR_GREATER
		=> IWrapper<TValue>.Create(value);
#else
		=> new Input<TValue>(value);
#endif
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
#if NETCOREAPP3_0_OR_GREATER
		=> IWrapper<TValue?>.Create(value);
#else
		=> new Input<TValue?>(value);
#endif
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
#if NETCOREAPP3_0_OR_GREATER
		=> IWrapper<TObject>.Create(instance)!;
#else
		=> new Input<TObject>(instance);
#endif
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
	public static IMutableWrapper<TValue> CreateMutable<TValue>(in TValue value = default) where TValue : struct
#if NETCOREAPP3_0_OR_GREATER
		=> IMutableWrapper<TValue>.Create(value);
#else
		=> new MutableWrapper<TValue>(value);
#endif
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
	public static IMutableWrapper<TValue?> CreateMutableNullable<TValue>(in TValue? value = default)
		where TValue : struct
#if NETCOREAPP3_0_OR_GREATER
		=> IMutableWrapper<TValue?>.Create(value);
#else
		=> new MutableWrapper<TValue?>(value);
#endif
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
	public static IMutableWrapper<TObject> CreateMutableObject<TObject>(TObject instance) where TObject : class
#if NETCOREAPP3_0_OR_GREATER
		=> IMutableWrapper<TObject>.Create(instance)!;
#else
		=> new MutableWrapper<TObject>(instance);
#endif
	/// <summary>
	/// Creates a new instance of an object that implements <see cref="IReferenceableWrapper{TValue}"/> interface.
	/// </summary>
	/// <typeparam name="TValue">The <see cref="ValueType"/> of the object to be wrapped.</typeparam>
	/// <param name="value">The value to be wrapped.</param>
	/// <returns>An instance of an object that implements <see cref="IReferenceableWrapper{TValue}"/> interface.</returns>
	/// <remarks>
	/// The newly created object wraps a value of <typeparamref name="TValue"/> type provided by <paramref name="value"/>.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IReferenceableWrapper<TValue> CreateReadOnlyReferenceable<TValue>(in TValue value)
		where TValue : struct
#if NETCOREAPP3_0_OR_GREATER
		=> IReferenceableWrapper<TValue>.Create(value);
#else
		=> new InputReference<TValue>(value);
#endif
	/// <summary>
	/// Creates a new instance of an object that implements <see cref="IReferenceableWrapper{TValue}"/> interface.
	/// </summary>
	/// <typeparam name="TValue">The <see cref="ValueType"/> of the nullable object to be wrapped.</typeparam>
	/// <param name="value">The nullable value to be wrapped.</param>
	/// <returns>An instance of an object that implements <see cref="IReferenceableWrapper{TValue}"/> interface.</returns>
	/// <remarks>
	/// The newly created object wraps a nullable value of <typeparamref name="TValue"/> type provided by
	/// <paramref name="value"/>.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IReferenceableWrapper<TValue?> CreateReadOnlyReferenceableNullable<TValue>(in TValue? value)
		where TValue : struct
#if NETCOREAPP3_0_OR_GREATER
		=> IReferenceableWrapper<TValue?>.Create(value);
#else
		=> new InputReference<TValue?>(value);
#endif
	/// <summary>
	/// Creates a new instance of an object that implements <see cref="IReferenceableWrapper{TObject}"/> interface.
	/// </summary>
	/// <typeparam name="TObject">The type of the object to be wrapped.</typeparam>
	/// <param name="instance">The instance to be wrapped.</param>
	/// <returns>An instance of an object that implements <see cref="IReferenceableWrapper{TObject}"/> interface.</returns>
	/// <remarks>
	/// The newly created object wraps an object of <typeparamref name="TObject"/> type provided by
	/// <paramref name="instance"/>.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IReferenceableWrapper<TObject> CreateReadOnlyReferenceableObject<TObject>(TObject instance)
		where TObject : class
#if NETCOREAPP3_0_OR_GREATER
		=> IReferenceableWrapper<TObject>.Create(instance)!;
#else
		=> new InputReference<TObject>(instance);
#endif
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
	public static IMutableReference<TValue> CreateReferenceable<TValue>(in TValue value = default!)
		where TValue : struct
#if NETCOREAPP3_0_OR_GREATER
		=> IMutableReference<TValue>.Create(value);
#else
		=> new MutableReference<TValue>(value);
#endif
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
	public static IMutableReference<TValue?> CreateReferenceableNullable<TValue>(in TValue? value = default)
		where TValue : struct
#if NETCOREAPP3_0_OR_GREATER
		=> IMutableReference<TValue?>.Create(value);
#else
		=> new MutableReference<TValue?>(value);
#endif
	/// <summary>
	/// Creates a new instance of an object that implements <see cref="IMutableReference{TObject}"/> interface.
	/// </summary>
	/// <typeparam name="TObject">The type of the object to be wrapped.</typeparam>
	/// <param name="instance">The instance to be wrapped.</param>
	/// <returns>An instance of an object that implements <see cref="IMutableReference{TObject}"/> interface.</returns>
	/// <remarks>
	/// The newly created object wraps an object of <typeparamref name="TObject"/> type provided by
	/// <paramref name="instance"/>.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IMutableReference<TObject> CreateReferenceableObject<TObject>(TObject instance) where TObject : class
#if NETCOREAPP3_0_OR_GREATER
		=> IMutableReference<TObject>.Create(instance)!;
#else
		=> new MutableReference<TObject>(instance);
#endif
}