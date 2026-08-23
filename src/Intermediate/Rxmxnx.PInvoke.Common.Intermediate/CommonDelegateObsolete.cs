#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
namespace Rxmxnx.PInvoke;
#pragma warning disable CS0618
/// <summary>
/// Represents an action that operates on a fixed memory instance.
/// </summary>
/// <param name="fixedMemory">The fixed memory instance to operate on.</param>
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
public delegate void FixedAction(in IFixedMemory fixedMemory);

/// <summary>
/// Represents an action that operates on a fixed memory instance using an additional state object.
/// </summary>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <param name="fixedMemory">The fixed memory instance to operate on.</param>
/// <param name="arg">The state object used by the action.</param>
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
public delegate void FixedAction<in TArg>(in IFixedMemory fixedMemory, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
;

/// <summary>
/// Represents an action that operates on a read-only fixed memory instance.
/// </summary>
/// <param name="readOnlyFixedMemory">The read-only fixed memory instance to operate on.</param>
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
public delegate void ReadOnlyFixedAction(in IReadOnlyFixedMemory readOnlyFixedMemory);

/// <summary>
/// Represents an action that operates on a read-only fixed memory instance using an additional state object.
/// </summary>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <param name="readOnlyFixedMemory">The read-only fixed memory instance to operate on.</param>
/// <param name="arg">The state object used by the action.</param>
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
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
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
public delegate TResult FixedFunc<out TResult>(in IFixedMemory fixedMemory);

/// <summary>
/// Represents a function that operates on a fixed memory instance using an additional state object.
/// </summary>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <typeparam name="TResult">The type of the return value of the function.</typeparam>
/// <param name="fixedMemory">The fixed memory instance to operate on.</param>
/// <param name="arg">The state object used by the function.</param>
/// <returns>The return value of the function.</returns>
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
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
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
public delegate TResult ReadOnlyFixedFunc<out TResult>(in IReadOnlyFixedMemory readOnlyFixedMemory);

/// <summary>
/// Represents a function that operates on a read-only fixed memory instance using an additional state object.
/// </summary>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <typeparam name="TResult">The type of the return value of the function.</typeparam>
/// <param name="readOnlyFixedMemory">The read-only fixed memory instance to operate on.</param>
/// <param name="arg">The state object used by the function.</param>
/// <returns>The return value of the function.</returns>
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
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
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
public delegate void FixedContextAction<T>(in IFixedContext<T> context);

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IFixedContext{T}"/> and a state object of
/// type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <param name="context">The fixed context instance.</param>
/// <param name="arg">The state object.</param>
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
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
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
public delegate void ReadOnlyFixedContextAction<T>(in IReadOnlyFixedContext<T> context);

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="IReadOnlyFixedContext{T}"/> and a state
/// object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <param name="context">The read-only fixed context instance.</param>
/// <param name="arg">The state object.</param>
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
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
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
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
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
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
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
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
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
public delegate TResult ReadOnlyFixedContextFunc<T, in TArg, out TResult>(in IReadOnlyFixedContext<T> context, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="FixedMemoryList"/>.
/// </summary>
/// <param name="memoryList">The <see cref="FixedMemoryList"/> instance.</param>
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
public delegate void FixedListAction(FixedMemoryList memoryList);

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="FixedMemoryList"/> and a
/// state object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <param name="memoryList">The <see cref="FixedMemoryList"/> instance.</param>
/// <param name="arg">The state object of type <typeparamref name="TArg"/>.</param>
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
public delegate void FixedListAction<in TArg>(FixedMemoryList memoryList, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
;

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="ReadOnlyFixedMemoryList"/>.
/// </summary>
/// <param name="memoryList">The <see cref="ReadOnlyFixedMemoryList"/> instance.</param>
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
public delegate void ReadOnlyFixedListAction(ReadOnlyFixedMemoryList memoryList);

/// <summary>
/// Encapsulates a method that receives an instance of <see cref="ReadOnlyFixedMemoryList"/> and a
/// state object of type <typeparamref name="TArg"/>.
/// </summary>
/// <typeparam name="TArg">The type of the state object.</typeparam>
/// <param name="memoryList">The <see cref="ReadOnlyFixedMemoryList"/> instance.</param>
/// <param name="arg">The state object of type <typeparamref name="TArg"/>.</param>
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
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
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
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
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
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
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
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
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete(ObsoleteConstants.ObsoleteDelegateTypes, ObsoleteConstants.ErrorDelegate)]
#endif
public delegate TResult ReadOnlyFixedListFunc<in TArg, out TResult>(ReadOnlyFixedMemoryList memoryList, TArg arg)
#if NET9_0_OR_GREATER
	where TArg : allows ref struct
#endif
;
#pragma warning restore CS0618
#endif