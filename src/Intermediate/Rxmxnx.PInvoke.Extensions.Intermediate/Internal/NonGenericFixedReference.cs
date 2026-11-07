namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents a non-generic wrapper around an <see cref="IFixedReferenceAction{T}"/> implementation.
/// Used only when the action is a reference type.
/// </summary>
/// <typeparam name="T">Type of the fixed reference.</typeparam>
[Preserve(AllMembers = true)]
internal readonly struct NonGenericFixedReferenceAction<T> : IFixedReferenceAction<T>
#if NET9_0_OR_GREATER
	where T : allows ref struct
#endif
{
	/// <summary>
	/// Internal value.
	/// </summary>
	private readonly IFixedReferenceAction<T> _action;

	/// <summary>
	/// Initializes a new instance of the <see cref="NonGenericFixedReferenceAction{T}"/> struct.
	/// </summary>
	/// <param name="action">An <see cref="IFixedReferenceAction{T}"/> instance.</param>
	private NonGenericFixedReferenceAction(IFixedReferenceAction<T> action) => this._action = action;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	void IFixedReferenceAction<T>.Accept(scoped FixedReferenceValue<T> fixedReference)
		=> this._action.Accept(fixedReference);

	/// <summary>
	/// Creates a new <see cref="NonGenericFixedReferenceAction{T}"/> instance.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedReferenceAction{T}"/>.</typeparam>
	/// <param name="action">Action to perform with the fixed reference.</param>
	/// <returns>A new <see cref="NonGenericFixedReferenceAction{T}"/> instance.</returns>
	internal static NonGenericFixedReferenceAction<T> Create<TAction>(TAction action)
#if !NET9_0_OR_GREATER
		where TAction : IFixedReferenceAction<T>
	{
		Debug.Assert(!typeof(TAction).IsValueType);
		return new(action);
	}
#else
		where TAction : IFixedReferenceAction<T>, allows ref struct
	{
		Debug.Assert(!typeof(TAction).IsValueType);
		return new(Unsafe.As<TAction, IFixedReferenceAction<T>>(ref action));
	}
#endif
}

/// <summary>
/// Represents a non-generic wrapper around an <see cref="IReadOnlyFixedReferenceAction{T}"/> implementation.
/// Used only when the action is a reference type.
/// </summary>
/// <typeparam name="T">Type of the fixed reference.</typeparam>
[Preserve(AllMembers = true)]
internal readonly struct NonGenericReadOnlyFixedReferenceAction<T> : IReadOnlyFixedReferenceAction<T>
#if NET9_0_OR_GREATER
	where T : allows ref struct
#endif
{
	/// <summary>
	/// Internal value.
	/// </summary>
	private readonly IReadOnlyFixedReferenceAction<T> _action;

	/// <summary>
	/// Initializes a new instance of the <see cref="NonGenericReadOnlyFixedReferenceAction{T}"/> struct.
	/// </summary>
	/// <param name="action">An <see cref="IReadOnlyFixedReferenceAction{T}"/> instance.</param>
	private NonGenericReadOnlyFixedReferenceAction(IReadOnlyFixedReferenceAction<T> action) => this._action = action;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	void IReadOnlyFixedReferenceAction<T>.Accept(scoped ReadOnlyFixedReferenceValue<T> fixedReference)
		=> this._action.Accept(fixedReference);

	/// <summary>
	/// Creates a new <see cref="NonGenericReadOnlyFixedReferenceAction{T}"/> instance.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedReferenceAction{T}"/>.</typeparam>
	/// <param name="action">Action to perform with the fixed reference.</param>
	/// <returns>A new <see cref="NonGenericReadOnlyFixedReferenceAction{T}"/> instance.</returns>
	internal static NonGenericReadOnlyFixedReferenceAction<T> Create<TAction>(TAction action)
#if !NET9_0_OR_GREATER
		where TAction : IReadOnlyFixedReferenceAction<T>
	{
		Debug.Assert(!typeof(TAction).IsValueType);
		return new(action);
	}
#else
		where TAction : IReadOnlyFixedReferenceAction<T>, allows ref struct
	{
		Debug.Assert(!typeof(TAction).IsValueType);
		return new(Unsafe.As<TAction, IReadOnlyFixedReferenceAction<T>>(ref action));
	}
#endif
}

/// <summary>
/// Represents a non-generic wrapper around an <see cref="IFixedReferenceFunction{T,TResult}"/> implementation.
/// Used only when the function is a reference type.
/// </summary>
/// <typeparam name="T">Type of the fixed reference.</typeparam>
/// <typeparam name="TResult">The type of the value returned by the function.</typeparam>
[Preserve(AllMembers = true)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2436)]
#endif
internal readonly struct NonGenericFixedReferenceFunction<T, TResult> : IFixedReferenceFunction<T, TResult>
#if NET9_0_OR_GREATER
	where T : allows ref struct
#endif
{
	/// <summary>
	/// Internal value.
	/// </summary>
	private readonly IFixedReferenceFunction<T, TResult> _func;

	/// <summary>
	/// Initializes a new instance of the <see cref="NonGenericFixedReferenceFunction{T,TResult}"/> struct.
	/// </summary>
	/// <param name="func">An <see cref="IFixedReferenceFunction{T,TResult}"/> instance.</param>
	private NonGenericFixedReferenceFunction(IFixedReferenceFunction<T, TResult> func) => this._func = func;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	TResult IFixedReferenceFunction<T, TResult>.Apply(scoped FixedReferenceValue<T> fixedReference)
		=> this._func.Apply(fixedReference);

	/// <summary>
	/// Creates a new <see cref="NonGenericFixedReferenceFunction{T,TResult}"/> instance.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedReferenceFunction{T,TResult}"/>.</typeparam>
	/// <param name="func">Function to execute with the fixed reference.</param>
	/// <returns>A new <see cref="NonGenericFixedReferenceFunction{T,TResult}"/> instance.</returns>
	internal static NonGenericFixedReferenceFunction<T, TResult> Create<TFunction>(TFunction func)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedReferenceFunction<T, TResult>
	{
		Debug.Assert(!typeof(TFunction).IsValueType);
		return new(func);
	}
#else
		where TFunction : IFixedReferenceFunction<T, TResult>, allows ref struct
	{
		Debug.Assert(!typeof(TFunction).IsValueType);
		return new(Unsafe.As<TFunction, IFixedReferenceFunction<T, TResult>>(ref func));
	}
#endif
}

/// <summary>
/// Represents a non-generic wrapper around an <see cref="IReadOnlyFixedReferenceFunction{T,TResult}"/> implementation.
/// Used only when the function is a reference type.
/// </summary>
/// <typeparam name="T">Type of the fixed reference.</typeparam>
/// <typeparam name="TResult">The type of the value returned by the function.</typeparam>
[Preserve(AllMembers = true)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2436)]
#endif
internal readonly struct
	NonGenericReadOnlyFixedReferenceFunction<T, TResult> : IReadOnlyFixedReferenceFunction<T, TResult>
#if NET9_0_OR_GREATER
	where T : allows ref struct
#endif
{
	/// <summary>
	/// Internal value.
	/// </summary>
	private readonly IReadOnlyFixedReferenceFunction<T, TResult> _func;

	/// <summary>
	/// Initializes a new instance of the <see cref="NonGenericReadOnlyFixedReferenceFunction{T,TResult}"/> struct.
	/// </summary>
	/// <param name="func">An <see cref="IReadOnlyFixedReferenceFunction{T,TResult}"/> instance.</param>
	private NonGenericReadOnlyFixedReferenceFunction(IReadOnlyFixedReferenceFunction<T, TResult> func)
		=> this._func = func;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	TResult IReadOnlyFixedReferenceFunction<T, TResult>.Apply(scoped ReadOnlyFixedReferenceValue<T> fixedReference)
		=> this._func.Apply(fixedReference);

	/// <summary>
	/// Creates a new <see cref="NonGenericReadOnlyFixedReferenceFunction{T,TResult}"/> instance.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IReadOnlyFixedReferenceFunction{T,TResult}"/>.</typeparam>
	/// <param name="func">Function to execute with the fixed reference.</param>
	/// <returns>A new <see cref="NonGenericReadOnlyFixedReferenceFunction{T,TResult}"/> instance.</returns>
	internal static NonGenericReadOnlyFixedReferenceFunction<T, TResult> Create<TFunction>(TFunction func)
#if !NET9_0_OR_GREATER
		where TFunction : IReadOnlyFixedReferenceFunction<T, TResult>
	{
		Debug.Assert(!typeof(TFunction).IsValueType);
		return new(func);
	}
#else
		where TFunction : IReadOnlyFixedReferenceFunction<T, TResult>, allows ref struct
	{
		Debug.Assert(!typeof(TFunction).IsValueType);
		return new(Unsafe.As<TFunction, IReadOnlyFixedReferenceFunction<T, TResult>>(ref func));
	}
#endif
}