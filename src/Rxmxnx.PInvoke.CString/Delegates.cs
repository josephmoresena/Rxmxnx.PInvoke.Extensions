[assembly: InternalsVisibleTo("Rxmxnx.PInvoke.CString.Tests")]

namespace Rxmxnx.PInvoke;

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
/// Encapsulates a method that receives a read-only span of <see cref="CString"/> instances
/// and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
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