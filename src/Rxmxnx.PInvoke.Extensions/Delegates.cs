using System;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Encapsulates a method that receives a read-only span of <see cref="CString"/> instances and a state 
    /// object of type <typeparamref name="TArg"/>.
    /// </summary>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="values">A <see cref="ReadOnlySpan{CString}"/> of objects of type T.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    public delegate void CStringSequenceAction<TArg>(ReadOnlySpan<CString> values, TArg arg);
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
    public delegate TResult CStringSequenceFunc<TArg, TResult>(ReadOnlySpan<CString> values, TArg arg);
}
