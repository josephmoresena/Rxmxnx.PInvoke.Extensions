namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents a non-generic wrapper around an <see cref="IFixedContextAction{T}"/> implementation.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
[Preserve(AllMembers = true)]
internal readonly struct NonGenericFixedContextAction<T> : IFixedContextAction<T>
{
	/// <summary>
	/// Internal value.
	/// </summary>
	private readonly IFixedContextAction<T> _action;

	/// <summary>
	/// Initializes a new instance of the <see cref="NonGenericFixedContextAction{T}"/> struct.
	/// </summary>
	/// <param name="action">An <see cref="IFixedContextAction{T}"/> instance.</param>
	private NonGenericFixedContextAction(IFixedContextAction<T> action) => this._action = action;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	void IFixedContextAction<T>.Accept(scoped FixedContextValue<T> fixedContext) => this._action.Accept(fixedContext);

	/// <summary>
	/// Creates a new <see cref="NonGenericFixedContextAction{T}"/> instance.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedContextAction{T}"/>.</typeparam>
	/// <param name="action">Action to perform with the fixed context.</param>
	/// <returns>A new <see cref="NonGenericFixedContextAction{T}"/> instance.</returns>
	internal static NonGenericFixedContextAction<T> Create<TAction>(TAction action)
#if !NET9_0_OR_GREATER
		where TAction : IFixedContextAction<T>
	{
		Debug.Assert(!typeof(TAction).IsValueType);
		return new(action);
	}
#else
		where TAction : IFixedContextAction<T>, allows ref struct
	{
		Debug.Assert(!typeof(TAction).IsValueType);
		return new(Unsafe.As<TAction, IFixedContextAction<T>>(ref action));
	}
#endif
}

/// <summary>
/// Represents a non-generic wrapper around an <see cref="IFixedContextFunction{T,TResult}"/> implementation.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TResult">The type of the value returned by the function.</typeparam>
[Preserve(AllMembers = true)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2436)]
#endif
internal readonly struct NonGenericFixedContextFunction<T, TResult> : IFixedContextFunction<T, TResult>
{
	/// <summary>
	/// Internal value.
	/// </summary>
	private readonly IFixedContextFunction<T, TResult> _func;

	/// <summary>
	/// Initializes a new instance of the <see cref="NonGenericFixedContextFunction{T,TResult}"/> struct.
	/// </summary>
	/// <param name="func">An <see cref="IFixedContextFunction{T,TResult}"/> instance.</param>
	private NonGenericFixedContextFunction(IFixedContextFunction<T, TResult> func) => this._func = func;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	TResult IFixedContextFunction<T, TResult>.Apply(scoped FixedContextValue<T> fixedContext)
		=> this._func.Apply(fixedContext);

	/// <summary>
	/// Creates a new <see cref="NonGenericFixedContextFunction{T,TResult}"/> instance.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedContextFunction{T,TResult}"/>.</typeparam>
	/// <param name="func">Function to execute with the fixed context.</param>
	/// <returns>A new <see cref="NonGenericFixedContextFunction{T,TResult}"/> instance.</returns>
	internal static NonGenericFixedContextFunction<T, TResult> Create<TFunction>(TFunction func)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedContextFunction<T, TResult>
	{
		Debug.Assert(!typeof(TFunction).IsValueType);
		return new(func);
	}
#else
		where TFunction : IFixedContextFunction<T, TResult>, allows ref struct
	{
		Debug.Assert(!typeof(TFunction).IsValueType);
		return new(Unsafe.As<TFunction, IFixedContextFunction<T, TResult>>(ref func));
	}
#endif
}