namespace Rxmxnx.PInvoke;

/// <summary>
/// Encapsulates a method that has no parameters and returns a <see cref="ReadOnlySpan{T}"/> instance.
/// </summary>
/// <typeparam name="T">The type of objects in the resulting span.</typeparam>
/// <returns>A <see cref="ReadOnlySpan{T}"/> instance.</returns>
public delegate ReadOnlySpan<T> ReadOnlySpanFunc<T>();

/// <summary>
/// Encapsulates a method that receives a span of objects of type <typeparamref name="T"/> and a 
/// state object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="T">The type of the objects in the span.</typeparam>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <param name="span">A span of objects of type <typeparamref name="T"/>.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult SpanFunc<T, in TArg, out TResult>(Span<T> span, TArg arg) where T : unmanaged;

/// <summary>
/// Encapsulates a method that receives a read-only span of objects of type <typeparamref name="T"/> and a 
/// state object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="T">The type of the objects in the span.</typeparam>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <param name="span">A read-only span of objects of type <typeparamref name="T"/>.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult ReadOnlySpanFunc<T, in TArg, out TResult>(ReadOnlySpan<T> span, TArg arg) where T : unmanaged;

/// <summary>
/// Encapsulates a method that receives a <see cref="IFixedMemory"/> instance.
/// </summary>
/// <param name="fmem">Fixed memory instance.</param>
public delegate void FixedAction(in IFixedMemory fmem);

/// <summary>
/// Encapsulates a method that receives a <see cref="IFixedMemory"/> instance and a 
/// state object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <param name="fmem">Fixed memory instance.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
public delegate void FixedAction<TArg>(in IFixedMemory fmem, TArg arg);

/// <summary>
/// Encapsulates a method that receives a <see cref="IReadOnlyFixedMemory"/> instance.
/// </summary>
/// <param name="fmem">Read-only fixed memory instance.</param>
public delegate void ReadOnlyFixedAction(in IReadOnlyFixedMemory fmem);

/// <summary>
/// Encapsulates a method that receives a <see cref="IReadOnlyFixedMemory"/> instance and a 
/// state object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <param name="fmem">Read-only fixed memory instance.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
public delegate void ReadOnlyFixedAction<TArg>(in IReadOnlyFixedMemory fmem, TArg arg);

/// <summary>
/// Encapsulates a method that receives a <see cref="IFixedMemory"/> instance.
/// </summary>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <param name="fmem">Fixed memory instance.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult FixedFunc<out TResult>(in IFixedMemory fmem);

/// <summary>
/// Encapsulates a method that receives a <see cref="IFixedMemory"/> instance.
/// </summary>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <param name="fmem">Fixed memory instance.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult FixedFunc<TArg, out TResult>(in IFixedMemory fmem, TArg arg);

/// <summary>
/// Encapsulates a method that receives a <see cref="IReadOnlyFixedMemory"/> instance.
/// </summary>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <param name="fmem">Read-only fixed memory instance.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult ReadOnlyFixedFunc<out TResult>(in IReadOnlyFixedMemory fmem);

/// <summary>
/// Encapsulates a method that receives a <see cref="IReadOnlyFixedMemory"/> instance.
/// </summary>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <param name="fmem">Read-only fixed memory instance.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult ReadOnlyFixedFunc<TArg, out TResult>(in IReadOnlyFixedMemory fmem, TArg arg);

/// <summary>
/// Encapsulates a method that receives a <see cref="IFixedContext{T}"/> instance.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <param name="ctx">Fixed context instance.</param>
public delegate void FixedContextAction<T>(in IFixedContext<T> ctx) where T : unmanaged;

/// <summary>
/// Encapsulates a method that receives a <see cref="IFixedContext{T}"/> instance and a 
/// state object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <param name="ctx">Fixed context instance.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
public delegate void FixedContextAction<T, TArg>(in IFixedContext<T> ctx, TArg arg) where T : unmanaged;

/// <summary>
/// Encapsulates a method that receives a <see cref="IReadOnlyFixedContext{T}"/> instance.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <param name="ctx">Read-only fixed context instance.</param>
public delegate void ReadOnlyFixedContextAction<T>(in IReadOnlyFixedContext<T> ctx) where T : unmanaged;

/// <summary>
/// Encapsulates a method that receives a <see cref="IReadOnlyFixedContext{T}"/> instance and a 
/// state object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <param name="ctx">Read-only fixed context instance.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
public delegate void ReadOnlyFixedContextAction<T, TArg>(in IReadOnlyFixedContext<T> ctx, TArg arg) where T : unmanaged;

/// <summary>
/// Encapsulates a method that receives a <see cref="IFixedContext{T}"/> instance.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <param name="ctx">Fixed context instance.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult FixedContextFunc<T, out TResult>(in IFixedContext<T> ctx) where T : unmanaged;

/// <summary>
/// Encapsulates a method that receives a <see cref="IFixedContext{T}"/> instance.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <param name="ctx">Fixed context instance.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult FixedContextFunc<T, TArg, out TResult>(in IFixedContext<T> ctx, TArg arg) where T : unmanaged;

/// <summary>
/// Encapsulates a method that receives a <see cref="IReadOnlyFixedContext{T}"/> instance.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <param name="ctx">Read-only fixed context instance.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult ReadOnlyFixedContextFunc<T, out TResult>(in IReadOnlyFixedContext<T> ctx) where T : unmanaged;

/// <summary>
/// Encapsulates a method that receives a <see cref="IReadOnlyFixedContext{T}"/> instance.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <param name="ctx">Read-only fixed context instance.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult ReadOnlyFixedContextFunc<T, TArg, out TResult>(in IReadOnlyFixedContext<T> ctx, TArg arg) where T : unmanaged;

/// <summary>
/// Encapsulates a method that receives a <see cref="IFixedReference{T}"/> instance.
/// </summary>
/// <typeparam name="T">Type of the fixed reference.</typeparam>
/// <param name="fref">Fixed reference instance.</param>
public delegate void FixedReferenceAction<T>(in IFixedReference<T> fref) where T : unmanaged;

/// <summary>
/// Encapsulates a method that receives a <see cref="IFixedReference{T}"/> instance and a 
/// state object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed reference.</typeparam>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <param name="fref">Fixed reference instance.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
public delegate void FixedReferenceAction<T, TArg>(in IFixedReference<T> fref, TArg arg) where T : unmanaged;

/// <summary>
/// Encapsulates a method that receives a <see cref="IReadOnlyFixedReference{T}"/> instance.
/// </summary>
/// <typeparam name="T">Type of the fixed reference.</typeparam>
/// <param name="fref">Read-only fixed reference instance.</param>
public delegate void ReadOnlyFixedReferenceAction<T>(in IReadOnlyFixedReference<T> fref) where T : unmanaged;

/// <summary>
/// Encapsulates a method that receives a <see cref="IReadOnlyFixedReference{T}"/> instance and a 
/// state object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed reference.</typeparam>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <param name="fref">Read-only fixed reference instance.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
public delegate void ReadOnlyFixedReferenceAction<T, TArg>(in IReadOnlyFixedReference<T> fref, TArg arg) where T : unmanaged;

/// <summary>
/// Encapsulates a method that receives a <see cref="IFixedReference{T}"/> instance.
/// </summary>
/// <typeparam name="T">Type of the fixed reference.</typeparam>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <param name="fref">Fixed reference instance.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult FixedReferenceFunc<T, out TResult>(in IFixedReference<T> fref) where T : unmanaged;

/// <summary>
/// Encapsulates a method that receives a <see cref="IFixedReference{T}"/> instance.
/// </summary>
/// <typeparam name="T">Type of the fixed reference.</typeparam>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <param name="fref">Fixed reference instance.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult FixedReferenceFunc<T, TArg, out TResult>(in IFixedReference<T> fref, TArg arg) where T : unmanaged;

/// <summary>
/// Encapsulates a method that receives a <see cref="IReadOnlyFixedReference{T}"/> instance.
/// </summary>
/// <typeparam name="T">Type of the fixed reference.</typeparam>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <param name="fref">Read-only fixed reference instance.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult ReadOnlyFixedReferenceFunc<T, out TResult>(in IReadOnlyFixedReference<T> fref) where T : unmanaged;

/// <summary>
/// Encapsulates a method that receives a <see cref="IReadOnlyFixedReference{T}"/> instance.
/// </summary>
/// <typeparam name="T">Type of the fixed reference.</typeparam>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <param name="fref">Read-only fixed reference instance.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult ReadOnlyFixedReferenceFunc<T, TArg, out TResult>(in IReadOnlyFixedReference<T> fref, TArg arg) where T : unmanaged;

/// <summary>
/// Encapsulates a method that receives a <see cref="IFixedMethod{T}"/> instance.
/// </summary>
/// <typeparam name="T"><see cref="Delegate"/> type of fixed method.</typeparam>
/// <param name="fmethod">Fixed method instance.</param>
public delegate void FixedMethodAction<T>(in IFixedMethod<T> fmethod) where T : Delegate;

/// <summary>
/// Encapsulates a method that receives a <see cref="IFixedMethod{T}"/> instance and a 
/// state object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="T"><see cref="Delegate"/> type of fixed method.</typeparam>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <param name="fmethod">Fixed method instance.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
public delegate void FixedMethodAction<T, TArg>(in IFixedMethod<T> fmethod, TArg arg) where T : Delegate;

/// <summary>
/// Encapsulates a method that receives a <see cref="IFixedMethod{T}"/> instance.
/// </summary>
/// <typeparam name="T"><see cref="Delegate"/> type of fixed method.</typeparam>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <param name="fmethod">Fixed method instance.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult FixedMethodFunc<T, out TResult>(in IFixedMethod<T> fmethod) where T : Delegate;

/// <summary>
/// Encapsulates a method that receives a <see cref="IFixedMethod{T}"/> instance.
/// </summary>
/// <typeparam name="T"><see cref="Delegate"/> type of fixed method.</typeparam>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <param name="fmethod">Fixed method instance.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult FixedMethodFunc<T, TArg, out TResult>(in IFixedMethod<T> fmethod, TArg arg) where T : Delegate;