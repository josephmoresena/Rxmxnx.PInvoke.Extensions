namespace Rxmxnx.PInvoke;

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
/// <summary>
/// This interface defines a wrapper object.
/// </summary>
public interface IWrapper
{
	/// <inheritdoc cref="WrapperFactory.Create{TValue}(in TValue)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IWrapper<TValue> Create<TValue>(in TValue value) where TValue : struct
		=> WrapperFactory.Create(in value);
	/// <inheritdoc cref="WrapperFactory.CreateNullable{TValue}(in Nullable{TValue})"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IWrapper<TValue?> CreateNullable<TValue>(in TValue? value) where TValue : struct
		=> WrapperFactory.CreateNullable(in value);
	/// <inheritdoc cref="WrapperFactory.CreateObject{TObject}(TObject)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IWrapper<TObject> CreateObject<TObject>(TObject instance) where TObject : class
		=> WrapperFactory.CreateObject(instance);

	/// <summary>
	/// This interface defines a wrapper for a <typeparamref name="T"/> object.
	/// </summary>
	/// <typeparam name="T">The type of value to be wrapped.</typeparam>
	/// <remarks>This interface is covariant.</remarks>
	public interface IBase<out T> : IWrapper
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
	{
		/// <summary>
		/// The wrapped <typeparamref name="T"/> object.
		/// </summary>
		// ReSharper disable once UnusedMemberInSuper.Global
		T Value { get; }
	}
}
#endif

/// <summary>
/// This interface defines a wrapper for a <typeparamref name="T"/> object.
/// </summary>
/// <typeparam name="T">The type of value to be wrapped.</typeparam>
// ReSharper disable once TypeParameterCanBeVariant
public interface IWrapper<T>
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	: IWrapper.IBase<T>, IEquatable<T>
#endif
{
	/// <summary>
	/// The wrapped <typeparamref name="T"/> object.
	/// </summary>
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	new T Value { get; }
#else
	T Value { get; }
#endif

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	T IBase<T>.Value => this.Value;
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
	public static IWrapper<T?> Create(T? instance) => WrapperFactory.Create(instance);
#endif
}