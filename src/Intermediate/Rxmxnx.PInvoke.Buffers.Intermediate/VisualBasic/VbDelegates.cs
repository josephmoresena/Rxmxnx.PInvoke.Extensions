namespace Rxmxnx.PInvoke.VisualBasic;

/// <inheritdoc cref="ScopedBufferAction{T}"/>
public delegate void VbScopedBufferAction<T>(VbScopedBuffer<T> buffer);

/// <inheritdoc cref="ScopedBufferAction{T, TArg}"/>
public delegate void VbScopedBufferAction<T, in TArg>(VbScopedBuffer<T> buffer, TArg arg);

/// <inheritdoc cref="ScopedBufferFunc{T, TResult}"/>
public delegate TResult VbScopedBufferFunc<T, out TResult>(VbScopedBuffer<T> buffer);

/// <inheritdoc cref="ScopedBufferFunc{T, TArg, TResult}"/>
public delegate TResult VbScopedBufferFunc<T, in TArg, out TResult>(VbScopedBuffer<T> buffer, TArg arg);