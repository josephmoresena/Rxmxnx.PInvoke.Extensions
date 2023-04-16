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
/// Encapsulates a method that receives a <see cref="FixedCStringSequence"/> instance.
/// </summary>
/// <param name="seq">A <see cref="FixedCStringSequence"/> instance.</param>
public delegate void CStringSequenceAction(FixedCStringSequence seq);

/// <summary>
/// Encapsulates a method that receives a <see cref="FixedCStringSequence"/> instance and a state 
/// object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <param name="seq">A <see cref="FixedCStringSequence"/> instance.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
public delegate void CStringSequenceAction<in TArg>(FixedCStringSequence seq, TArg arg);

/// <summary>
/// Encapsulates a method that receives a <see cref="FixedCStringSequence"/> instance and returns
/// a value of the type specified by the <typeparamref name="TResult"/> parameter.
/// </summary>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <param name="seq">A <see cref="FixedCStringSequence"/> instance.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult CStringSequenceFunc<out TResult>(FixedCStringSequence seq);

/// <summary>
/// Encapsulates a method that receives a <see cref="FixedCStringSequence"/> instance and a state 
/// object of type <typeparamref name="TArg"/> and returns a value of the type specified by the 
/// <typeparamref name="TResult"/> parameter.
/// </summary>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <param name="seq">A <see cref="FixedCStringSequence"/> instance.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate TResult CStringSequenceFunc<in TArg, out TResult>(FixedCStringSequence seq, TArg arg);