using System;

namespace Rxmxnx.PInvoke.Extensions
{
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
    /// Encapsulates a method that receives a read-only span of <see cref="CString"/> instances and a state 
    /// object of type <typeparamref name="TArg"/>.
    /// </summary>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="values">A <see cref="ReadOnlySpan{CString}"/> of objects of type T.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    public delegate void CStringSequenceAction<in TArg>(ReadOnlySpan<CString> values, TArg arg);

    /// <summary>
    /// Encapsulates a method that receives a read-only span of <see cref="CString"/> instances and a state 
    /// object of type <typeparamref name="TArg"/>.
    /// </summary>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span">A binary span.</param>
    /// <param name="index">Index of current sequence intem.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    public delegate void CStringSequenceCreationAction<in TArg>(Span<Byte> span, Int32 index, TArg arg);

    /// <summary>
    /// Encapsulates a method that receives a read-only span of <see cref="CString"/> instances and a state 
    /// object of type <typeparamref name="TArg"/> and returns a value of the type specified by the 
    /// <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
    /// <param name="values">A <see cref="ReadOnlySpan{CString}"/> of objects of type T.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <returns>The return value of the method that this delegate encapsulates.</returns>
    public delegate TResult CStringSequenceFunc<in TArg, out TResult>(ReadOnlySpan<CString> values, TArg arg);

    /// <summary>
    /// Encapsulates a method that receives a span of <typeparamref name="T"/> values and a state 
    /// object of type <typeparamref name="TArg"/>.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the span.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span">A span of objects of type <typeparamref name="T"/>.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="residue">The residual span of bytes.</param>
    public delegate void SpanTransformAction<T, in TArg>(Span<T> span, TArg arg, Span<Byte> residue) where T : unmanaged;

    /// <summary>
    /// Encapsulates a method that receives a span of <typeparamref name="T"/> values and a state 
    /// object of type <typeparamref name="TArg"/> and returns a value of the type specified by the 
    /// <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the span.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
    /// <param name="span">A span of objects of type <typeparamref name="T"/>.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="residue">The residual span of bytes.</param>
    /// <returns>The return value of the method that this delegate encapsulates.</returns>
    public delegate TResult SpanTransfromFunc<T, in TArg, out TResult>(Span<T> span, TArg arg, Span<Byte> residue) where T : unmanaged;

    /// <summary>
    /// Encapsulates a method that receives a read-only span of <typeparamref name="T"/> values and a state 
    /// object of type <typeparamref name="TArg"/>.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the span.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span">A read-only span of objects of type <typeparamref name="T"/>.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="residue">The residual span of bytes.</param>
    public delegate void ReadOnlySpanTransformAction<T, in TArg>(ReadOnlySpan<T> span, TArg arg, ReadOnlySpan<Byte> residue) where T : unmanaged;

    /// <summary>
    /// Encapsulates a method that receives a read-only span of <typeparamref name="T"/> values and a state 
    /// object of type <typeparamref name="TArg"/> and returns a value of the type specified by the 
    /// <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the span.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
    /// <param name="span">A read-only span of objects of type <typeparamref name="T"/>.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="residue">The residual span of bytes.</param>
    /// <returns>The return value of the method that this delegate encapsulates.</returns>
    public delegate TResult ReadOnlySpanTransfromFunc<T, in TArg, out TResult>(ReadOnlySpan<T> span, TArg arg, ReadOnlySpan<Byte> residue) where T : unmanaged;

    /// <summary>
    /// Encapsulates a method that receives a binary span and a state object of type 
    /// <typeparamref name="TArg"/>.
    /// </summary>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span">A binary span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    public delegate void BinarySpanTransformAction<in TArg>(Span<Byte> span, TArg arg);

    /// <summary>
    /// Encapsulates a method that receives a binary read-only span and a state object of type 
    /// <typeparamref name="TArg"/>.
    /// </summary>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span">A binary read-only span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    public delegate void BinaryReadOnlySpanTransformAction<in TArg>(ReadOnlySpan<Byte> span, TArg arg);

    /// <summary>
    /// ncapsulates a method that receives a binary span and a state object of type 
    /// <typeparamref name="TArg"/> and returns a value of the type specified by the 
    /// <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
    /// <param name="span">A binary span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <returns>The return value of the method that this delegate encapsulates.</returns>
    public delegate TResult BinarySpanTransfromFunc<in TArg, out TResult>(Span<Byte> span, TArg arg);

    /// <summary>
    /// Encapsulates a method that receives a binary read-only span and a state object of type 
    /// <typeparamref name="TArg"/> and returns a value of the type specified by the 
    /// <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
    /// <param name="span">A binary read-only span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <returns>The return value of the method that this delegate encapsulates.</returns>
    public delegate TResult BinaryReadOnlySpanTransfromFunc<in TArg, out TResult>(ReadOnlySpan<Byte> span, TArg arg);

}
