namespace Rxmxnx.PInvoke;

/// <summary>
/// Encapsulates a method that receives a buffer of objects of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the objects in the buffer.</typeparam>
/// <param name="buffer">A buffer of objects of type <typeparamref name="T"/>.</param>
public delegate void ScopedBufferAction<T>(ScopedBuffer<T> buffer);

/// <summary>
/// Encapsulates a method that receives a buffer of objects of type <typeparamref name="T"/> and a
/// state object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="T">The type of the elements in the buffer.</typeparam>
/// <typeparam name="TArg">The type of the state object passed to the method.</typeparam>
/// <param name="buffer">A buffer of objects of type <typeparamref name="T"/>.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
public delegate void ScopedBufferAction<T, in TArg>(ScopedBuffer<T> buffer, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
	;

/// <summary>
/// Encapsulates a method that receives a buffer of objects of type <typeparamref name="T"/> and
/// returns a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">The type of the objects in the buffer.</typeparam>
/// <typeparam name="TResult">The type of the result produced by the method that this delegate encapsulates.</typeparam>
/// <param name="buffer">A buffer of objects of type <typeparamref name="T"/>.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult ScopedBufferFunc<T, out TResult>(ScopedBuffer<T> buffer);

/// <summary>
/// Encapsulates a method that receives a buffer of objects of type <typeparamref name="T"/> and a
/// state object of type <typeparamref name="TArg"/>, and returns a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="T">The type of the elements in the buffer.</typeparam>
/// <typeparam name="TArg">The type of the state object passed to the method.</typeparam>
/// <typeparam name="TResult">The type of the result produced by the method that this delegate encapsulates.</typeparam>
/// <param name="buffer">A buffer of objects of type <typeparamref name="T"/>.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult ScopedBufferFunc<T, in TArg, out TResult>(ScopedBuffer<T> buffer, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
	;