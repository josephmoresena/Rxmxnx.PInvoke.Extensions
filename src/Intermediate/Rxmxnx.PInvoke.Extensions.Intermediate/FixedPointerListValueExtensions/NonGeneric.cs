namespace Rxmxnx.PInvoke;

public static partial class FixedPointerListValueExtensions
{
	/// <summary>
	/// Represents a non-generic wrapper around an <see cref="IFixedPointerListAction"/> implementation.
	/// </summary>
	[Preserve(AllMembers = true)]
	private readonly struct NonGenericAction : IFixedPointerListAction
	{
		/// <summary>
		/// Internal value.
		/// </summary>
		private readonly IFixedPointerListAction _action;

		/// <summary>
		/// Initializes a new instance of the <see cref="NonGenericAction"/> struct.
		/// </summary>
		/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
		private NonGenericAction(IFixedPointerListAction action) => this._action = action;

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void IFixedPointerListAction.Accept(scoped FixedPointerValueList list) => this._action.Accept(list);

		/// <summary>
		/// Creates a new <see cref="NonGenericAction"/> instance.
		/// </summary>
		/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
		/// <param name="action">Action to perform with the fixed pointer list.</param>
		/// <returns>A new <see cref="NonGenericAction"/> instance.</returns>
		internal static NonGenericAction Create<TAction>(TAction action)
#if !NET9_0_OR_GREATER
			where TAction : IFixedPointerListAction
		{
			Debug.Assert(!typeof(TAction).IsValueType);
			return new(action);
		}
#else
			where TAction : IFixedPointerListAction, allows ref struct
		{
			Debug.Assert(!typeof(TAction).IsValueType);
			return new(Unsafe.As<TAction, IFixedPointerListAction>(ref action));
		}
#endif
	}

	/// <summary>
	/// Represents a non-generic wrapper around an <see cref="IFixedPointerListFunction{TResult}"/> implementation.
	/// </summary>
	/// <typeparam name="TResult">The type of the value returned by the function.</typeparam>
	[Preserve(AllMembers = true)]
	private readonly struct NonGenericFunction<TResult> : IFixedPointerListFunction<TResult>
	{
		/// <summary>
		/// Internal value.
		/// </summary>
		private readonly IFixedPointerListFunction<TResult> _func;

		/// <summary>
		/// Initializes a new instance of the <see cref="NonGenericFunction{TResult}"/> struct.
		/// </summary>
		/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
		private NonGenericFunction(IFixedPointerListFunction<TResult> func) => this._func = func;

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		TResult IFixedPointerListFunction<TResult>.Apply(scoped FixedPointerValueList list) => this._func.Apply(list);

		/// <summary>
		/// Creates a new <see cref="NonGenericFunction{TResult}"/> instance.
		/// </summary>
		/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
		/// <param name="func">Function to execute with the fixed pointer list.</param>
		/// <returns>A new <see cref="NonGenericFunction{TResult}"/> instance.</returns>
		internal static NonGenericFunction<TResult> Create<TFunction>(TFunction func)
#if !NET9_0_OR_GREATER
			where TFunction : IFixedPointerListFunction<TResult>
		{
			Debug.Assert(!typeof(TFunction).IsValueType);
			return new(func);
		}
#else
			where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
		{
			Debug.Assert(!typeof(TFunction).IsValueType);
			return new(Unsafe.As<TFunction, IFixedPointerListFunction<TResult>>(ref func));
		}
#endif
	}
}