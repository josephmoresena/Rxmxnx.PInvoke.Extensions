using System;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Encapsulates a methtod that receives a <see cref="FixedContext{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">Type of the fixed context.</typeparam>
    /// <param name="ctx">Fixed context instance.</param>
    public delegate void FixedAction<T>(in FixedContext<T> ctx) where T : unmanaged;

    /// <summary>
    /// Encapsulates a methtod that receives a <see cref="FixedContext{T, TArg}"/> instance.
    /// </summary>
    /// <typeparam name="T">Type of the fixed context.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="ctx">Fixed context instance.</param>
    public delegate void FixedAction<T, TArg>(in FixedContext<T, TArg> ctx) where T : unmanaged;

    /// <summary>
    /// Encapsulates a methtod that receives a <see cref="ReadOnlyFixedContext{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">Type of the fixed context.</typeparam>
    /// <param name="ctx">Read-only fixed context instance.</param>
    public delegate void ReadOnlyFixedAction<T>(in ReadOnlyFixedContext<T> ctx) where T : unmanaged;

    /// <summary>
    /// Encapsulates a methtod that receives a <see cref="ReadOnlyFixedContext{T, TArg}"/> instance.
    /// </summary>
    /// <typeparam name="T">Type of the fixed context.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="ctx">Read-only fixed context instance.</param>
    public delegate void ReadOnlyFixedAction<T, TArg>(in ReadOnlyFixedContext<T, TArg> ctx) where T : unmanaged;

    /// <summary>
    /// Encapsulates a methtod that receives a <see cref="FixedContext{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">Type of the fixed context.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
    /// <param name="ctx">Fixed context instance.</param>
    /// <returns>The return value of the method that this delegate encapsulates.</returns>
    public delegate void FixedFunc<T, out TResult>(in FixedContext<T> ctx) where T : unmanaged;

    /// <summary>
    /// Encapsulates a methtod that receives a <see cref="FixedContext{T, TArg}"/> instance.
    /// </summary>
    /// <typeparam name="T">Type of the fixed context.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
    /// <param name="ctx">Fixed context instance.</param>
    /// <returns>The return value of the method that this delegate encapsulates.</returns>
    public delegate void FixedFunc<T, TArg, out TResult>(in FixedContext<T, TArg> ctx) where T : unmanaged;

    /// <summary>
    /// Encapsulates a methtod that receives a <see cref="ReadOnlyFixedContext{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">Type of the fixed context.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
    /// <param name="ctx">Read-only fixed instance.</param>
    /// <returns>The return value of the method that this delegate encapsulates.</returns>
    public delegate void ReadOnlyFixedFunc<T, out TResult>(in ReadOnlyFixedContext<T> ctx) where T : unmanaged;

    /// <summary>
    /// Encapsulates a methtod that receives a <see cref="ReadOnlyFixedContext{T, TArg}"/> instance.
    /// </summary>
    /// <typeparam name="T">Type of the fixed context.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
    /// <param name="ctx">Read-only fixed instance.</param>
    /// <returns>The return value of the method that this delegate encapsulates.</returns>
    public delegate void ReadOnlyFixedFunc<T, TArg, out TResult>(in ReadOnlyFixedContext<T, TArg> ctx) where T : unmanaged;

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
    /// Encapsulates a method that receives a read-only span of <see cref="CString"/> instances.
    /// </summary>
    /// <param name="values">A <see cref="ReadOnlySpan{CString}"/> of objects of type T.</param>
    public delegate void CStringSequenceAction(ReadOnlySpan<CString> values);

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
    /// Encapsulates a method that receives a read-only span of <see cref="CString"/> instances and 
    /// returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
    /// <param name="values">A <see cref="ReadOnlySpan{CString}"/> of objects of type T.</param>
    /// <returns>The return value of the method that this delegate encapsulates.</returns>
    public delegate TResult CStringSequenceFunc<out TResult>(ReadOnlySpan<CString> values);

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

    /// <summary>
    /// Encapsulates a methtod that receives a <see cref="TransformationContext{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">Type of the fixed context.</typeparam>
    /// <param name="ctx">Transformation context instance.</param>
    public delegate void TransformationAction<T>(TransformationContext<T> ctx) where T : unmanaged;

    /// <summary>
    /// Encapsulates a methtod that receives a <see cref="TransformationContext{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">Type of the fixed context.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="ctx">Transformation context instance.</param>
    public delegate void TransformationAction<T, TArg>(TransformationContext<T, TArg> ctx) where T : unmanaged;

    /// <summary>
    /// Encapsulates a methtod that receives a <see cref="ReadOnlyTransformationContext{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">Type of the fixed context.</typeparam>
    /// <param name="ctx">Read-only transformation context instance.</param>
    public delegate void ReadOnlyTransformationAction<T>(ReadOnlyTransformationContext<T> ctx) where T : unmanaged;

    /// <summary>
    /// Encapsulates a methtod that receives a <see cref="ReadOnlyTransformationContext{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">Type of the fixed context.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="ctx">Read-only transformation context instance.</param>
    public delegate void ReadOnlyTransformationAction<T, TArg>(ReadOnlyTransformationContext<T, TArg> ctx) where T : unmanaged;

}
