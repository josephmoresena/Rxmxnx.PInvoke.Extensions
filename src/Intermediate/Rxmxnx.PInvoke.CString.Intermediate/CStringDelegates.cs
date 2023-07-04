namespace Rxmxnx.PInvoke;

/// <summary>
/// Encapsulates a method that receives a span of bytes, an index and a state
/// object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
/// <param name="span">A span of bytes.</param>
/// <param name="index">Index of current sequence item.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
public delegate void CStringSequenceCreationAction<in TArg>(Span<Byte> span, Int32 index, TArg arg);

/// <summary>
/// Encapsulates a method that operates on a <see cref="FixedCStringSequence"/> instance.
/// </summary>
/// <param name="seq">The <see cref="FixedCStringSequence"/> instance to operate on.</param>
public delegate void CStringSequenceAction(FixedCStringSequence seq);

/// <summary>
/// Encapsulates a method that operates on a <see cref="FixedCStringSequence"/> instance and a state
/// object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <param name="seq">The <see cref="FixedCStringSequence"/> instance to operate on.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
public delegate void CStringSequenceAction<in TArg>(FixedCStringSequence seq, TArg arg);

/// <summary>
/// Encapsulates a method that operates on a <see cref="FixedCStringSequence"/> instance and returns
/// a value of the type specified by the <typeparamref name="TResult"/> parameter.
/// </summary>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <param name="seq">The <see cref="FixedCStringSequence"/> instance to operate on.</param>
/// <returns>The result of the method.</returns>
public delegate TResult CStringSequenceFunc<out TResult>(FixedCStringSequence seq);

/// <summary>
/// Encapsulates a method that operates on a <see cref="FixedCStringSequence"/> instance and a state
/// object of type <typeparamref name="TArg"/>, and returns a value of the type specified by the
/// <typeparamref name="TResult"/> parameter.
/// </summary>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <param name="seq">The <see cref="FixedCStringSequence"/> instance to operate on.</param>
/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
/// <returns>The result of the method.</returns>
public delegate TResult CStringSequenceFunc<in TArg, out TResult>(FixedCStringSequence seq, TArg arg);