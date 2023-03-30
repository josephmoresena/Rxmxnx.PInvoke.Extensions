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

/// <summary>
/// Encapsulates a method that recieves a read-only span of <typeparamref name="T"/> values and retrieves its 
/// <see cref="String"/> representation.
/// </summary>
/// <typeparam name="T">The type of values in the span.</typeparam>
/// <param name="text">
/// A read-only span of <typeparamref name="T"/> values that represents a text.
/// </param>
/// <returns>A <see cref="String"/> representation of <paramref name="text"/>.</returns>
internal delegate String GetStringDelegate<T>(ReadOnlySpan<T> text) where T : unmanaged;

/// <summary>
/// Decodes the <see cref="Rune"/> at the beginning of the provided unicode source buffer.
/// </summary>
/// <returns>
/// <para>
/// If the source buffer begins with a valid unicode encoded scalar value, returns 
/// <see cref="OperationStatus.Done"/>, and outs via <paramref name="result"/> the decoded 
/// <see cref="Rune"/> and via <paramref name="charsConsumed"/> the number of chars used in the input 
/// buffer to encode the <see cref="Rune"/>.
/// </para>
/// <para>
/// If the source buffer is empty or contains only a standalone unicode high surrogate character, 
/// returns <see cref="OperationStatus.NeedMoreData"/>, and outs via <paramref name="result"/> and via 
/// <paramref name="charsConsumed"/> the length of the input buffer.
/// </para>
/// <para>
/// If the source buffer begins with an ill-formed unicode encoded scalar value, returns 
/// <see cref="OperationStatus.InvalidData"/>, and outs via <paramref name="result"/> and via 
/// <paramref name="charsConsumed"/> the number of chars used in the input buffer to encode the 
/// ill-formed sequence.
/// </para>
/// </returns>
/// <remarks>
/// The general calling convention is to call this method in a loop, slicing the <paramref name="source"/> 
/// buffer by <paramref name="charsConsumed"/> elements on each iteration of the loop. On each iteration of 
/// the loop <paramref name="result"/> will contain the real scalar value if successfully decoded. 
/// This pattern provides convenient automatic U+FFFD substitution of invalid sequences while iterating
/// through the loop.
/// </remarks>
internal delegate OperationStatus DecodeRuneFrom<T>(ReadOnlySpan<T> source, out Rune result, out Int32 charsConsumed) where T : unmanaged;