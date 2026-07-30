namespace Rxmxnx.PInvoke;

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
/// <summary>
/// This interface exposes a wrapper for an object that can be referenced and whose value
/// can be modified.
/// </summary>
public interface IReferenceableWrapper : IWrapper
{
	/// <seealso cref="WrapperFactory.CreateReadOnlyReferenceable{TValue}(in TValue)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public new static IReferenceableWrapper<TValue> Create<TValue>(in TValue value) where TValue : struct
		=> WrapperFactory.CreateReadOnlyReferenceable(in value);
	/// <seealso cref="WrapperFactory.CreateReadOnlyReferenceableNullable{TValue}"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public new static IReferenceableWrapper<TValue?> CreateNullable<TValue>(in TValue? value) where TValue : struct
		=> WrapperFactory.CreateReadOnlyReferenceableNullable(in value);
	/// <seealso cref="WrapperFactory.CreateReadOnlyReferenceableObject{TObject}"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public new static IReferenceableWrapper<TObject> CreateObject<TObject>(TObject instance) where TObject : class
		=> WrapperFactory.CreateReadOnlyReferenceableObject(instance);
}
#endif

/// <summary>
/// This interface exposes a wrapper for <typeparamref name="T"/> object that can be
/// referenced and whose value can be modified. The provided reference is mutable, allowing changes to the value.
/// </summary>
/// <typeparam name="T">Type of both the wrapped and referenced value.</typeparam>
/// <remarks>While the value of the object can be accessed through this reference, it cannot be modified.</remarks>
public interface IReferenceableWrapper<T> : IWrapper<T>, IReadOnlyReferenceable<T>
#if !NETSTANDARD2_1 && !NETCOREAPP3_0_OR_GREATER
;
#else
	, IReferenceableWrapper
{
	/// <summary>
	/// Creates a new instance of an object that implements <see cref="IReferenceableWrapper{T}"/> interface.
	/// </summary>
	/// <param name="instance">The value to be wrapped.</param>
	/// <returns>An instance of an object that implements <see cref="IReferenceableWrapper{T}"/> interface.</returns>
	/// <remarks>
	/// The newly created object wraps a value of <typeparamref name="T"/> type provided by
	/// <paramref name="instance"/>.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public new static IReferenceableWrapper<T?> Create(T? instance)
		=> WrapperFactory.CreateReadOnlyReferenceable(instance);
}
#endif