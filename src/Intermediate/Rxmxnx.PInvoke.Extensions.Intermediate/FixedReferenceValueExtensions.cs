namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see cref="FixedReferenceValue{T}"/> instances.
/// </summary>
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
[Browsable(false)]
#endif
[EditorBrowsable(EditorBrowsableState.Never)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
#pragma warning disable CS8500
public static unsafe class FixedReferenceValueExtensions
{
	/// <summary>
	/// Pins <paramref name="value"/> until <paramref name="action"/> finishes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedReferenceAction{T}"/>.</typeparam>
	/// <typeparam name="T">Type of the referenced value.</typeparam>
	/// <param name="action">An <see cref="IFixedReferenceAction{T}"/> instance.</param>
	/// <param name="value">A <typeparamref name="T"/> reference.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T>(this TAction? action, ref T value)
#if !NET9_0_OR_GREATER
		where TAction : IFixedReferenceAction<T>
#else
		where T : allows ref struct where TAction : IFixedReferenceAction<T>, allows ref struct
#endif
	{
		if (action is null) return;
		FixedReferenceValueExtensions.WithSafeFixed(ref action, ref value);
	}

	/// <summary>
	/// Pins <paramref name="value"/> until <paramref name="func"/> finishes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedReferenceFunction{T,TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T">Type of the referenced value.</typeparam>
	/// <param name="func">An <see cref="IFixedReferenceFunction{T,TResult}"/> instance.</param>
	/// <param name="value">A <typeparamref name="T"/> reference.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T>(this TFunction? func, ref T value, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedReferenceFunction<T, TResult>
#else
		where T : allows ref struct where TFunction : IFixedReferenceFunction<T, TResult>, allows ref struct
#endif
	{
		if (func is null)
		{
			Unsafe.SkipInit(out result);
			return;
		}
		FixedReferenceValueExtensions.WithSafeFixed(ref func, ref value, out result);
	}

	/// <summary>
	/// Pins <paramref name="value"/> and executes <paramref name="action"/>.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedReferenceAction{T}"/>.</typeparam>
	/// <typeparam name="T">Type of the referenced value.</typeparam>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	/// <param name="value">A <typeparamref name="T"/> reference.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void WithSafeFixed<TAction, T>(ref TAction action, ref T value)
#if !NET9_0_OR_GREATER
		where TAction : IFixedReferenceAction<T>
#else
		where T : allows ref struct where TAction : IFixedReferenceAction<T>, allows ref struct
#endif
	{
		if (typeof(TAction).IsValueType)
		{
			fixed (void* ptr = &value)
				action.Accept(new(ptr));
			return;
		}
		NonGenericFixedReferenceAction<T> nonGeneric = NonGenericFixedReferenceAction<T>.Create(action);
		FixedReferenceValueExtensions.WithSafeFixed(ref nonGeneric, ref value);
	}

	/// <summary>
	/// Pins <paramref name="value"/> and executes <paramref name="func"/>.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedReferenceFunction{T,TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T">Type of the referenced value.</typeparam>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="value">A <typeparamref name="T"/> reference.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void WithSafeFixed<TFunction, TResult, T>(ref TFunction func, ref T value, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedReferenceFunction<T, TResult>
#else
		where T : allows ref struct where TFunction : IFixedReferenceFunction<T, TResult>, allows ref struct
#endif
	{
		if (typeof(TFunction).IsValueType)
		{
			fixed (void* ptr = &value)
				result = func.Apply(new(ptr));
			return;
		}
		NonGenericFixedReferenceFunction<T, TResult> nonGeneric =
			NonGenericFixedReferenceFunction<T, TResult>.Create(func);
		FixedReferenceValueExtensions.WithSafeFixed(ref nonGeneric, ref value, out result);
	}

	/// <summary>
	/// Pins <paramref name="value"/> until <paramref name="action"/> finishes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedReferenceAction{T}"/>.</typeparam>
	/// <typeparam name="T">Type of the referenced value.</typeparam>
	/// <param name="action">An <see cref="IReadOnlyFixedReferenceAction{T}"/> instance.</param>
	/// <param name="value">A <typeparamref name="T"/> read-only reference.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeReadOnlyFixed<TAction, T>(this TAction? action, in T value)
#if !NET9_0_OR_GREATER
		where TAction : IReadOnlyFixedReferenceAction<T>
#else
		where T : allows ref struct where TAction : IReadOnlyFixedReferenceAction<T>, allows ref struct
#endif
	{
		if (action is null) return;
		FixedReferenceValueExtensions.WithSafeReadOnlyFixed(ref action, in value);
	}

	/// <summary>
	/// Pins <paramref name="value"/> until <paramref name="func"/> finishes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IReadOnlyFixedReferenceFunction{T,TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T">Type of the referenced value.</typeparam>
	/// <param name="func">An <see cref="IReadOnlyFixedReferenceFunction{T,TResult}"/> instance.</param>
	/// <param name="value">A <typeparamref name="T"/> read-only reference.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeReadOnlyFixed<TFunction, TResult, T>(this TFunction? func, in T value,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IReadOnlyFixedReferenceFunction<T, TResult>
#else
		where T : allows ref struct where TFunction : IReadOnlyFixedReferenceFunction<T, TResult>, allows ref struct
#endif
	{
		if (func is null)
		{
			Unsafe.SkipInit(out result);
			return;
		}
		FixedReferenceValueExtensions.WithSafeReadOnlyFixed(ref func, in value, out result);
	}

	/// <summary>
	/// Pins <paramref name="value"/> and executes <paramref name="action"/>.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedReferenceAction{T}"/>.</typeparam>
	/// <typeparam name="T">Type of the referenced value.</typeparam>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	/// <param name="value">A <typeparamref name="T"/> read-only reference.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void WithSafeReadOnlyFixed<TAction, T>(ref TAction action, in T value)
#if !NET9_0_OR_GREATER
		where TAction : IReadOnlyFixedReferenceAction<T>
#else
		where T : allows ref struct where TAction : IReadOnlyFixedReferenceAction<T>, allows ref struct
#endif
	{
		if (typeof(TAction).IsValueType)
		{
			fixed (void* ptr = &value)
				action.Accept(new(ptr));
			return;
		}
		NonGenericReadOnlyFixedReferenceAction<T> nonGeneric = NonGenericReadOnlyFixedReferenceAction<T>.Create(action);
		FixedReferenceValueExtensions.WithSafeReadOnlyFixed(ref nonGeneric, in value);
	}

	/// <summary>
	/// Pins <paramref name="value"/> and executes <paramref name="func"/>.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IReadOnlyFixedReferenceFunction{T,TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T">Type of the referenced value.</typeparam>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="value">A <typeparamref name="T"/> read-only reference.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void WithSafeReadOnlyFixed<TFunction, TResult, T>(ref TFunction func, in T value, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IReadOnlyFixedReferenceFunction<T, TResult>
#else
		where T : allows ref struct where TFunction : IReadOnlyFixedReferenceFunction<T, TResult>, allows ref struct
#endif
	{
		if (typeof(TFunction).IsValueType)
		{
			fixed (void* ptr = &value)
				result = func.Apply(new(ptr));
			return;
		}
		NonGenericReadOnlyFixedReferenceFunction<T, TResult> nonGeneric =
			NonGenericReadOnlyFixedReferenceFunction<T, TResult>.Create(func);
		FixedReferenceValueExtensions.WithSafeReadOnlyFixed(ref nonGeneric, in value, out result);
	}

	/// <summary>
	/// Pins <paramref name="value"/> until <paramref name="action"/> finishes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedReferenceAction{T}"/>.</typeparam>
	/// <typeparam name="T">Type of the referenced value.</typeparam>
	/// <param name="action">An <see cref="IFixedReferenceAction{T}"/> instance.</param>
	/// <param name="value">A <typeparamref name="T"/> reference.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T>(this ref TAction action, ref T value)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedReferenceAction<T>
#else
		where T : allows ref struct where TAction : struct, IFixedReferenceAction<T>, allows ref struct
#endif
		=> FixedReferenceValueExtensions.WithSafeFixed(ref action, ref value);

	/// <summary>
	/// Pins <paramref name="value"/> until <paramref name="func"/> finishes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedReferenceFunction{T,TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T">Type of the referenced value.</typeparam>
	/// <param name="func">An <see cref="IFixedReferenceFunction{T,TResult}"/> instance.</param>
	/// <param name="value">A <typeparamref name="T"/> reference.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T>(this ref TFunction func, ref T value,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedReferenceFunction<T, TResult>
#else
		where T : allows ref struct where TFunction : struct, IFixedReferenceFunction<T, TResult>, allows ref struct
#endif
		=> FixedReferenceValueExtensions.WithSafeFixed(ref func, ref value, out result);

	/// <summary>
	/// Pins <paramref name="value"/> until <paramref name="action"/> finishes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedReferenceAction{T}"/>.</typeparam>
	/// <typeparam name="T">Type of the referenced value.</typeparam>
	/// <param name="action">An <see cref="IReadOnlyFixedReferenceAction{T}"/> instance.</param>
	/// <param name="value">A <typeparamref name="T"/> read-only reference.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeReadOnlyFixed<TAction, T>(this ref TAction action, in T value)
#if !NET9_0_OR_GREATER
		where TAction : struct, IReadOnlyFixedReferenceAction<T>
#else
		where T : allows ref struct where TAction : struct, IReadOnlyFixedReferenceAction<T>, allows ref struct
#endif
		=> FixedReferenceValueExtensions.WithSafeReadOnlyFixed(ref action, in value);

	/// <summary>
	/// Pins <paramref name="value"/> until <paramref name="func"/> finishes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IReadOnlyFixedReferenceFunction{T,TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T">Type of the referenced value.</typeparam>
	/// <param name="func">An <see cref="IReadOnlyFixedReferenceFunction{T,TResult}"/> instance.</param>
	/// <param name="value">A <typeparamref name="T"/> read-only reference.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeReadOnlyFixed<TFunction, TResult, T>(this ref TFunction func, in T value,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IReadOnlyFixedReferenceFunction<T, TResult>
#else
		where T : allows ref struct
		where TFunction : struct, IReadOnlyFixedReferenceFunction<T, TResult>, allows ref struct
#endif
		=> FixedReferenceValueExtensions.WithSafeReadOnlyFixed(ref func, in value, out result);
}
#pragma warning restore CS8500