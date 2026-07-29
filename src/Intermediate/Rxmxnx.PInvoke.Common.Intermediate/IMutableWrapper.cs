namespace Rxmxnx.PInvoke;

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
/// <summary>
/// This interface defines a wrapper for an object whose value can be modified.
/// </summary>
public interface IMutableWrapper : IWrapper
{
	/// <seealso cref="WrapperFactory.CreateMutable{TValue}(in TValue)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public new static IMutableWrapper<TValue> Create<TValue>(in TValue value = default) where TValue : struct
		=> WrapperFactory.CreateMutable(in value);
	/// <seealso cref="WrapperFactory.CreateMutableNullable{TValue}"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public new static IMutableWrapper<TValue?> CreateNullable<TValue>(in TValue? value = default) where TValue : struct
		=> WrapperFactory.CreateMutableNullable(in value);
	/// <seealso cref="WrapperFactory.CreateMutableObject{TObject}"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public new static IMutableWrapper<TObject> CreateObject<TObject>(TObject instance) where TObject : class
		=> WrapperFactory.CreateMutableObject(instance);
}
#endif

/// <summary>
/// This interface defines a wrapper for a <typeparamref name="T"/> object whose value can be modified.
/// </summary>
/// <typeparam name="T">The type of value to be wrapped.</typeparam>
public interface IMutableWrapper<T> : IWrapper<T>, IStrongBox
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	, IMutableWrapper
#endif
{
	/// <summary>
	/// The wrapped <typeparamref name="T"/> object.
	/// </summary>
	new T Value { get; set; }

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	Object? IStrongBox.Value
	{
		get => this.Value;
		set => this.Value = (T)value!;
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
#endif
}