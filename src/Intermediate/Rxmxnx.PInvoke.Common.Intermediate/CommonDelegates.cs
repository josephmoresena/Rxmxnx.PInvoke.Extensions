namespace Rxmxnx.PInvoke;

/// <summary>
/// Encapsulates a method that has no parameters and returns a read-only span of type
/// <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the elements in the returned read-only span.</typeparam>
/// <returns>A <see cref="ReadOnlySpan{T}"/> instance.</returns>
public delegate ReadOnlySpan<T> ReadOnlySpanFunc<T>();

/// <summary>
/// Encapsulates a method that has a <typeparamref name="TState"/> parameter and returns a
/// read-only span of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the elements in the returned read-only span.</typeparam>
/// <typeparam name="TState">The type of the state object passed to the method.</typeparam>
/// <param name="arg">A state object of type <typeparamref name="TState"/>.</param>
/// <returns>A <see cref="ReadOnlySpan{T}"/> instance.</returns>
public delegate ReadOnlySpan<T> ReadOnlySpanFunc<T, in TState>(TState arg);

#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
/// <summary>
/// Encapsulates a method that receives a span of type <typeparamref name="T"/>, a
/// state object of type <typeparamref name="TArg"/> and returns a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">The type of the elements in the span.</typeparam>
/// <typeparam name="TArg">The type of the state object passed to the method.</typeparam>
/// <typeparam name="TResult">The type of the result produced by the method that this delegate encapsulates.</typeparam>
/// <param name="span">A span of type <typeparamref name="T"/>.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
// ReSharper disable once UnusedType.Global
public delegate TResult SpanFunc<T, in TArg, out TResult>(Span<T> span, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives a read-only span of type <typeparamref name="T"/>, a
/// state object of type <typeparamref name="TArg"/> and returns a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">The type of the elements in the span.</typeparam>
/// <typeparam name="TArg">The type of the state object passed to the method.</typeparam>
/// <typeparam name="TResult">The type of the result produced by the method that this delegate encapsulates.</typeparam>
/// <param name="span">A read-only span of type <typeparamref name="T"/>.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
// ReSharper disable once UnusedType.Global
public delegate TResult ReadOnlySpanFunc<T, in TArg, out TResult>(ReadOnlySpan<T> span, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
	;
#endif

/// <summary>
/// Encapsulates a method that receives an  instance of <see cref="IFixedReference{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the fixed reference.</typeparam>
/// <param name="reference">An instance of the fixed reference.</param>
public delegate void FixedReferenceAction<T>(in IFixedReference<T> reference)
#if NET9_0_OR_GREATER
	where T : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives an  instance of <see cref="IFixedReference{T}"/> and a state object of
/// type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="T">The type of the fixed reference.</typeparam>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <param name="reference">An instance of the fixed reference.</param>
/// <param name="arg">A state object of type TArg.</param>
public delegate void FixedReferenceAction<T, in TArg>(in IFixedReference<T> reference, TArg arg)
#if NET9_0_OR_GREATER
	where T : allows ref struct where TArg : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IReadOnlyFixedReference{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the fixed reference.</typeparam>
/// <param name="reference">A read-only instance of the fixed reference.</param>
public delegate void ReadOnlyFixedReferenceAction<T>(in IReadOnlyFixedReference<T> reference)
#if NET9_0_OR_GREATER
	where T : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IReadOnlyFixedReference{T}"/> and a
/// state object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="T">The type of the fixed reference.</typeparam>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <param name="reference">A read-only instance of the fixed reference.</param>
/// <param name="arg">A state object of type TArg.</param>
public delegate void ReadOnlyFixedReferenceAction<T, in TArg>(in IReadOnlyFixedReference<T> reference, TArg arg)
#if NET9_0_OR_GREATER
	where T : allows ref struct where TArg : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IFixedReference{T}"/> and returns a value of
/// type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">The type of the fixed reference.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <param name="reference">An instance of the fixed reference.</param>
/// <returns>The return value from the encapsulated method.</returns>
public delegate TResult FixedReferenceFunc<T, out TResult>(in IFixedReference<T> reference)
#if NET9_0_OR_GREATER
	where T : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IFixedReference{T}"/>, a state object of
/// type <typeparamref name="TArg"/>, and returns a value of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">The type of the fixed reference.</typeparam>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <param name="reference">An instance of the fixed reference.</param>
/// <param name="arg">A state object of type TArg.</param>
/// <returns>The return value from the encapsulated method.</returns>
public delegate TResult FixedReferenceFunc<T, in TArg, out TResult>(in IFixedReference<T> reference, TArg arg)
#if NET9_0_OR_GREATER
	where T : allows ref struct where TArg : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IReadOnlyFixedReference{T}"/> and
/// returns a value of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">The type of the fixed reference.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <param name="reference">A read-only instance of the fixed reference.</param>
/// <returns>The return value from the encapsulated method.</returns>
public delegate TResult ReadOnlyFixedReferenceFunc<T, out TResult>(in IReadOnlyFixedReference<T> reference)
#if NET9_0_OR_GREATER
	where T : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IReadOnlyFixedReference{T}"/>, a
/// state object of type <typeparamref name="TArg"/>, and returns a value of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">The type of the fixed reference.</typeparam>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <param name="reference">A read-only instance of the fixed reference.</param>
/// <param name="arg">A state object of type TArg.</param>
/// <returns>The return value from the encapsulated method.</returns>
public delegate TResult ReadOnlyFixedReferenceFunc<T, in TArg, out TResult>(in IReadOnlyFixedReference<T> reference,
		TArg arg)
#if NET9_0_OR_GREATER
	where T : allows ref struct where TArg : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IFixedMethod{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the fixed method delegate.</typeparam>
/// <param name="method">An instance of the fixed method delegate.</param>
public delegate void FixedMethodAction<T>(in IFixedMethod<T> method) where T : Delegate;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IFixedMethod{T}"/> and a state object of type
/// <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="T">The type of the fixed method delegate.</typeparam>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <param name="method">An instance of the fixed method delegate.</param>
/// <param name="arg">A state object of type TArg.</param>
public delegate void FixedMethodAction<T, in TArg>(in IFixedMethod<T> method, TArg arg) where T : Delegate
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IFixedMethod{T}"/> and returns a value
/// of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">The type of the fixed method delegate.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <param name="method">An instance of the fixed method delegate.</param>
/// <returns>The return value of the encapsulated method.</returns>
public delegate TResult FixedMethodFunc<T, out TResult>(in IFixedMethod<T> method) where T : Delegate;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IFixedMethod{T}"/>, a state object of type
/// <typeparamref name="TArg"/>, and returns a value of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">The type of the fixed method delegate.</typeparam>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <param name="method">An instance of the fixed method delegate.</param>
/// <param name="arg">The state object.</param>
/// <returns>The return value of the encapsulated method.</returns>
public delegate TResult FixedMethodFunc<T, in TArg, out TResult>(in IFixedMethod<T> method, TArg arg) where T : Delegate
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
;