namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents a non-generic wrapper around an <see cref="IReadOnlyFixedContextAction{T}"/> implementation.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
[Preserve(AllMembers = true)]
internal readonly struct NonGenericReadOnlyFixedContextAction<T> : IReadOnlyFixedContextAction<T>
{
	/// <summary>
	/// Internal value.
	/// </summary>
	private readonly IReadOnlyFixedContextAction<T> _action;

	/// <summary>
	/// Initializes a new instance of the <see cref="NonGenericReadOnlyFixedContextAction{T}"/> struct.
	/// </summary>
	/// <param name="action">An <see cref="IReadOnlyFixedContextAction{T}"/> instance.</param>
	private NonGenericReadOnlyFixedContextAction(IReadOnlyFixedContextAction<T> action) => this._action = action;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	void IReadOnlyFixedContextAction<T>.Accept(scoped ReadOnlyFixedContextValue<T> fixedContext)
		=> this._action.Accept(fixedContext);

	/// <summary>
	/// Creates a new <see cref="NonGenericReadOnlyFixedContextAction{T}"/> instance.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="action">Action to perform with the fixed context.</param>
	/// <returns>A new <see cref="NonGenericReadOnlyFixedContextAction{T}"/> instance.</returns>
	internal static NonGenericReadOnlyFixedContextAction<T> Create<TAction>(TAction action)
#if !NET9_0_OR_GREATER
		where TAction : IReadOnlyFixedContextAction<T>
	{
		Debug.Assert(!typeof(TAction).IsValueType);
		return new(action);
	}
#else
		where TAction : IReadOnlyFixedContextAction<T>, allows ref struct
	{
		Debug.Assert(!typeof(TAction).IsValueType);
		return new(Unsafe.As<TAction, IReadOnlyFixedContextAction<T>>(ref action));
	}
#endif
}

/// <summary>
/// Represents a non-generic wrapper around an <see cref="IReadOnlyFixedContextFunction{T,TResult}"/> implementation.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TResult">The type of the value returned by the function.</typeparam>
[Preserve(AllMembers = true)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2436)]
#endif
internal readonly struct NonGenericReadOnlyFixedContextFunction<T, TResult> : IReadOnlyFixedContextFunction<T, TResult>
{
	/// <summary>
	/// Internal value.
	/// </summary>
	private readonly IReadOnlyFixedContextFunction<T, TResult> _func;

	/// <summary>
	/// Initializes a new instance of the <see cref="NonGenericReadOnlyFixedContextFunction{T,TResult}"/> struct.
	/// </summary>
	/// <param name="func">An <see cref="IReadOnlyFixedContextFunction{T,TResult}"/> instance.</param>
	private NonGenericReadOnlyFixedContextFunction(IReadOnlyFixedContextFunction<T, TResult> func) => this._func = func;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	TResult IReadOnlyFixedContextFunction<T, TResult>.Apply(scoped ReadOnlyFixedContextValue<T> fixedContext)
		=> this._func.Apply(fixedContext);

	/// <summary>
	/// Creates a new <see cref="NonGenericReadOnlyFixedContextFunction{T,TResult}"/> instance.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IReadOnlyFixedContextFunction{T,TResult}"/>.</typeparam>
	/// <param name="func">Function to execute with the fixed context.</param>
	/// <returns>A new <see cref="NonGenericReadOnlyFixedContextFunction{T,TResult}"/> instance.</returns>
	internal static NonGenericReadOnlyFixedContextFunction<T, TResult> Create<TFunction>(TFunction func)
#if !NET9_0_OR_GREATER
		where TFunction : IReadOnlyFixedContextFunction<T, TResult>
	{
		Debug.Assert(!typeof(TFunction).IsValueType);
		return new(func);
	}
#else
		where TFunction : IReadOnlyFixedContextFunction<T, TResult>, allows ref struct
	{
		Debug.Assert(!typeof(TFunction).IsValueType);
		return new(Unsafe.As<TFunction, IReadOnlyFixedContextFunction<T, TResult>>(ref func));
	}
#endif
}