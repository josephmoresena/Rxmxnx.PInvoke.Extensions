namespace Rxmxnx.PInvoke;

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
/// <summary>
/// This interface exposes a wrapper for an object that can be referenced and whose value can be modified.
/// </summary>
public interface IMutableReference : IMutableWrapper
{
	/// <seealso cref="WrapperFactory.CreateReferenceable{TValue}(in TValue)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public new static IMutableReference<TValue> Create<TValue>(in TValue value = default) where TValue : struct
		=> WrapperFactory.CreateReferenceable(in value);
	/// <seealso cref="WrapperFactory.CreateReferenceableNullable{TValue}(in Nullable{TValue})"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public new static IMutableReference<TValue?> CreateNullable<TValue>(in TValue? value = default)
		where TValue : struct
		=> WrapperFactory.CreateReferenceableNullable(in value);
	/// <seealso cref="WrapperFactory.CreateReferenceableObject{TObject}(TObject)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public new static IMutableReference<TObject> CreateObject<TObject>(TObject instance) where TObject : class
		=> WrapperFactory.CreateReferenceableObject(instance);
}
#endif

/// <summary>
/// This interface exposes a wrapper for <typeparamref name="T"/> object that can be
/// referenced and whose value can be modified.
/// </summary>
/// <typeparam name="T">Type of both wrapped and referenced value.</typeparam>
/// <remarks>The provided reference is mutable, allowing changes to the value.</remarks>
// ReSharper disable once PossibleInterfaceMemberAmbiguity
public interface IMutableReference<T> : IReferenceableWrapper<T>, IMutableWrapper<T>, IReferenceable<T>
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	, IMutableReference
#endif
{
	/// <summary>
	/// Reference to <typeparamref name="T"/> wrapped instance.
	/// </summary>
	new ref T Reference { get; }

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	ref T IReferenceable<T>.Reference => ref this.Reference;
	ref readonly T IReadOnlyReferenceable<T>.Reference => ref this.Reference;

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
	public new static IMutableReference<T?> Create(T? instance = default)
		=> WrapperFactory.CreateReferenceable(instance);
#endif
}