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
public delegate ReadOnlySpan<T> ReadOnlySpanFunc<T, in TState>(TState arg)
#if NET9_0_OR_GREATER
	where TState : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives a span of type <typeparamref name="T"/> and a
/// state object of type <typeparamref name="TArg"/>, and returns a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">The type of the elements in the span.</typeparam>
/// <typeparam name="TArg">The type of the state object passed to the method.</typeparam>
/// <typeparam name="TResult">The type of the result produced by the method that this delegate encapsulates.</typeparam>
/// <param name="span">A span of type <typeparamref name="T"/>.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult SpanFunc<T, in TArg, out TResult>(Span<T> span, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives a read-only span of type <typeparamref name="T"/> and a
/// state object of type <typeparamref name="TArg"/>, and returns a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">The type of the elements in the span.</typeparam>
/// <typeparam name="TArg">The type of the state object passed to the method.</typeparam>
/// <typeparam name="TResult">The type of the result produced by the method that this delegate encapsulates.</typeparam>
/// <param name="span">A read-only span of type <typeparamref name="T"/>.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult ReadOnlySpanFunc<T, in TArg, out TResult>(ReadOnlySpan<T> span, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
	;

/// <summary>
/// Represents an action that operates on a fixed memory instance.
/// </summary>
/// <param name="fixedMemory">The fixed memory instance to operate on.</param>
public delegate void FixedAction(in IFixedMemory fixedMemory);

/// <summary>
/// Represents an action that operates on a fixed memory instance using an additional state object.
/// </summary>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <param name="fixedMemory">The fixed memory instance to operate on.</param>
/// <param name="arg">The state object used by the action.</param>
public delegate void FixedAction<in TArg>(in IFixedMemory fixedMemory, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
	;

/// <summary>
/// Represents an action that operates on a read-only fixed memory instance.
/// </summary>
/// <param name="readOnlyFixedMemory">The read-only fixed memory instance to operate on.</param>
public delegate void ReadOnlyFixedAction(in IReadOnlyFixedMemory readOnlyFixedMemory);

/// <summary>
/// Represents an action that operates on a read-only fixed memory instance using an additional state object.
/// </summary>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <param name="readOnlyFixedMemory">The read-only fixed memory instance to operate on.</param>
/// <param name="arg">The state object used by the action.</param>
public delegate void ReadOnlyFixedAction<in TArg>(in IReadOnlyFixedMemory readOnlyFixedMemory, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
	;

/// <summary>
/// Represents a function that operates on a fixed memory instance.
/// </summary>
/// <typeparam name="TResult">The type of the return value of the function.</typeparam>
/// <param name="fixedMemory">The fixed memory instance to operate on.</param>
/// <returns>The return value of the function.</returns>
public delegate TResult FixedFunc<out TResult>(in IFixedMemory fixedMemory);

/// <summary>
/// Represents a function that operates on a fixed memory instance using an additional state object.
/// </summary>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <typeparam name="TResult">The type of the return value of the function.</typeparam>
/// <param name="fixedMemory">The fixed memory instance to operate on.</param>
/// <param name="arg">The state object used by the function.</param>
/// <returns>The return value of the function.</returns>
public delegate TResult FixedFunc<in TArg, out TResult>(in IFixedMemory fixedMemory, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
	;

/// <summary>
/// Represents a function that operates on a read-only fixed memory instance.
/// </summary>
/// <typeparam name="TResult">The type of the return value of the function.</typeparam>
/// <param name="readOnlyFixedMemory">The read-only fixed memory instance to operate on.</param>
/// <returns>The return value of the function.</returns>
public delegate TResult ReadOnlyFixedFunc<out TResult>(in IReadOnlyFixedMemory readOnlyFixedMemory);

/// <summary>
/// Represents a function that operates on a read-only fixed memory instance using an additional state object.
/// </summary>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <typeparam name="TResult">The type of the return value of the function.</typeparam>
/// <param name="readOnlyFixedMemory">The read-only fixed memory instance to operate on.</param>
/// <param name="arg">The state object used by the function.</param>
/// <returns>The return value of the function.</returns>
public delegate TResult ReadOnlyFixedFunc<in TArg, out TResult>(in IReadOnlyFixedMemory readOnlyFixedMemory, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IFixedContext{T}"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <param name="context">The fixed context instance.</param>
public delegate void FixedContextAction<T>(in IFixedContext<T> context);

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IFixedContext{T}"/> and a state object of
/// type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <param name="context">The fixed context instance.</param>
/// <param name="arg">The state object.</param>
public delegate void FixedContextAction<T, in TArg>(in IFixedContext<T> context, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IReadOnlyFixedContext{T}"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <param name="context">The read-only fixed context instance.</param>
public delegate void ReadOnlyFixedContextAction<T>(in IReadOnlyFixedContext<T> context);

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IReadOnlyFixedContext{T}"/> and a state
/// object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <param name="context">The read-only fixed context instance.</param>
/// <param name="arg">The state object.</param>
public delegate void ReadOnlyFixedContextAction<T, in TArg>(in IReadOnlyFixedContext<T> context, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IFixedContext{T}"/> and returns a value of
/// type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <param name="context">The fixed context instance.</param>
/// <returns>The return value of the encapsulated method.</returns>
public delegate TResult FixedContextFunc<T, out TResult>(in IFixedContext<T> context);

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IFixedContext{T}"/>, a state object of
/// type <typeparamref name="TArg"/>, and returns a value of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <param name="context">The fixed context instance.</param>
/// <param name="arg">The state object.</param>
/// <returns>The return value of the encapsulated method.</returns>
public delegate TResult FixedContextFunc<T, in TArg, out TResult>(in IFixedContext<T> context, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IReadOnlyFixedContext{T}"/> and
/// returns a value of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <param name="context">The read-only fixed context instance.</param>
/// <returns>The return value of the encapsulated method.</returns>
public delegate TResult ReadOnlyFixedContextFunc<T, out TResult>(in IReadOnlyFixedContext<T> context);

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IReadOnlyFixedContext{T}"/>, a
/// state object of type <typeparamref name="TArg"/>, and returns a value of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <param name="context">The read-only fixed context instance.</param>
/// <param name="arg">The state object.</param>
/// <returns>The return value of the encapsulated method.</returns>
public delegate TResult ReadOnlyFixedContextFunc<T, in TArg, out TResult>(in IReadOnlyFixedContext<T> context, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives an  instance of <see cref="IFixedReference{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the fixed reference.</typeparam>
/// <param name="reference">An instance of the fixed reference.</param>
public delegate void FixedReferenceAction<T>(in IFixedReference<T> reference);

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
	where TArg : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IReadOnlyFixedReference{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the fixed reference.</typeparam>
/// <param name="reference">A read-only instance of the fixed reference.</param>
public delegate void ReadOnlyFixedReferenceAction<T>(in IReadOnlyFixedReference<T> reference);

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
	where TArg : allows ref struct
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
public delegate TResult FixedReferenceFunc<T, out TResult>(in IFixedReference<T> reference);

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
	where TArg : allows ref struct
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
public delegate TResult ReadOnlyFixedReferenceFunc<T, out TResult>(in IReadOnlyFixedReference<T> reference);

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
	where TArg : allows ref struct
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

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="FixedMemoryList"/>.
/// </summary>
/// <param name="memoryList">The <see cref="FixedMemoryList"/> instance.</param>
public delegate void FixedListAction(FixedMemoryList memoryList);

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="FixedMemoryList"/> and a
/// state object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <param name="memoryList">The <see cref="FixedMemoryList"/> instance.</param>
/// <param name="arg">The state object of type <typeparamref name="TArg"/>.</param>
public delegate void FixedListAction<in TArg>(FixedMemoryList memoryList, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="ReadOnlyFixedMemoryList"/>.
/// </summary>
/// <param name="memoryList">The <see cref="ReadOnlyFixedMemoryList"/> instance.</param>
public delegate void ReadOnlyFixedListAction(ReadOnlyFixedMemoryList memoryList);

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="ReadOnlyFixedMemoryList"/> and a
/// state object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <param name="memoryList">The <see cref="ReadOnlyFixedMemoryList"/> instance.</param>
/// <param name="arg">The state object of type <typeparamref name="TArg"/>.</param>
public delegate void ReadOnlyFixedListAction<in TArg>(ReadOnlyFixedMemoryList memoryList, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="FixedMemoryList"/> and returns a value of type
/// <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <param name="memoryList">The <see cref="FixedMemoryList"/> instance.</param>
/// <returns>The result of the method.</returns>
public delegate TResult FixedListFunc<out TResult>(FixedMemoryList memoryList);

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="FixedMemoryList"/> and a
/// state object of type <typeparamref name="TArg"/>, and returns a value of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <param name="memoryList">The <see cref="FixedMemoryList"/> instance.</param>
/// <param name="arg">The state object of type <typeparamref name="TArg"/>.</param>
/// <returns>The result of the method.</returns>
public delegate TResult FixedListFunc<in TArg, out TResult>(FixedMemoryList memoryList, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="ReadOnlyFixedMemoryList"/> and returns a value of
/// type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <param name="memoryList">The <see cref="ReadOnlyFixedMemoryList"/> instance.</param>
/// <returns>The result of the method.</returns>
public delegate TResult ReadOnlyFixedListFunc<out TResult>(ReadOnlyFixedMemoryList memoryList);

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="ReadOnlyFixedMemoryList"/> and a
/// state object of type <typeparamref name="TArg"/>, and returns a value of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <param name="memoryList">The <see cref="ReadOnlyFixedMemoryList"/> instance.</param>
/// <param name="arg">The state object of type <typeparamref name="TArg"/>.</param>
/// <returns>The result of the method.</returns>
public delegate TResult ReadOnlyFixedListFunc<in TArg, out TResult>(ReadOnlyFixedMemoryList memoryList, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
	;