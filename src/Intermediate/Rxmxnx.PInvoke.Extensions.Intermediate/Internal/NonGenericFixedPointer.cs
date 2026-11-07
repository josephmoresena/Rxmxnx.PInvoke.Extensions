namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents a non-generic wrapper around an <see cref="IFixedAction"/> implementation.
/// </summary>
[Preserve(AllMembers = true)]
internal readonly struct NonGenericFixedAction : IFixedAction
{
	/// <summary>
	/// Internal value.
	/// </summary>
	private readonly IFixedAction _action;

	/// <summary>
	/// Initializes a new instance of the <see cref="NonGenericFixedAction"/> struct.
	/// </summary>
	/// <param name="action">An <see cref="IFixedAction"/> instance.</param>
	private NonGenericFixedAction(IFixedAction action) => this._action = action;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	void IFixedAction.Accept(scoped FixedPointerValue fixedPointer) => this._action.Accept(fixedPointer);

	/// <summary>
	/// Creates a new <see cref="NonGenericFixedAction"/> instance.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedAction"/>.</typeparam>
	/// <param name="action">Action to perform with the fixed pointer.</param>
	/// <returns>A new <see cref="NonGenericFixedAction"/> instance.</returns>
	internal static NonGenericFixedAction Create<TAction>(TAction action)
#if !NET9_0_OR_GREATER
		where TAction : IFixedAction
	{
		Debug.Assert(!typeof(TAction).IsValueType);
		return new(action);
	}
#else
		where TAction : IFixedAction, allows ref struct
	{
		Debug.Assert(!typeof(TAction).IsValueType);
		return new(Unsafe.As<TAction, IFixedAction>(ref action));
	}
#endif
}

/// <summary>
/// Represents a non-generic wrapper around an <see cref="IFixedFunction{TResult}"/> implementation.
/// </summary>
/// <typeparam name="TResult">The type of the value returned by the function.</typeparam>
[Preserve(AllMembers = true)]
internal readonly struct NonGenericFixedFunction<TResult> : IFixedFunction<TResult>
{
	/// <summary>
	/// Internal value.
	/// </summary>
	private readonly IFixedFunction<TResult> _func;

	/// <summary>
	/// Initializes a new instance of the <see cref="NonGenericFixedFunction{TResult}"/> struct.
	/// </summary>
	/// <param name="func">An <see cref="IFixedFunction{TResult}"/> instance.</param>
	private NonGenericFixedFunction(IFixedFunction<TResult> func) => this._func = func;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	TResult IFixedFunction<TResult>.Apply(scoped FixedPointerValue fixedPointer) => this._func.Apply(fixedPointer);

	/// <summary>
	/// Creates a new <see cref="NonGenericFixedFunction{TResult}"/> instance.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedFunction{TResult}"/>.</typeparam>
	/// <param name="func">Function to execute with the fixed pointer.</param>
	/// <returns>A new <see cref="NonGenericFixedFunction{TResult}"/> instance.</returns>
	internal static NonGenericFixedFunction<TResult> Create<TFunction>(TFunction func)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedFunction<TResult>
	{
		Debug.Assert(!typeof(TFunction).IsValueType);
		return new(func);
	}
#else
		where TFunction : IFixedFunction<TResult>, allows ref struct
	{
		Debug.Assert(!typeof(TFunction).IsValueType);
		return new(Unsafe.As<TFunction, IFixedFunction<TResult>>(ref func));
	}
#endif
}