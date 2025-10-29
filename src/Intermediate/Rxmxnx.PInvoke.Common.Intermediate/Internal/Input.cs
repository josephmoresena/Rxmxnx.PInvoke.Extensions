namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Encapsulates an immutable <typeparamref name="T"/> object within a record type, allowing for efficient
/// retrieval and update of its value.
/// </summary>
/// <typeparam name="T">Type of the object being wrapped.</typeparam>
internal class Input<T> : IWrapper<T>
{
	/// <summary>
	/// The encapsulated <typeparamref name="T"/> object.
	/// </summary>
	private T _instance;

	/// <summary>
	/// Initializes a new instance of the <see cref="Input{T}"/> record with the specified initial value.
	/// </summary>
	/// <param name="instance">The initial value of the encapsulated object.</param>
	public Input(in T instance) => this._instance = instance;

	T IWrapper<T>.Value => this._instance;

	/// <summary>
	/// Retrieves the value of the encapsulated <typeparamref name="T"/> object.
	/// </summary>
	/// <returns>The value of the encapsulated <typeparamref name="T"/> object.</returns>
	protected T GetInstance() => this._instance;
	/// <summary>
	/// Retrieves the reference to the encapsulated <typeparamref name="T"/> object.
	/// </summary>
	/// <returns>The reference to the encapsulated <typeparamref name="T"/> object.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected ref T GetReference() => ref this._instance;
	/// <summary>
	/// Updates the encapsulated <typeparamref name="T"/> object.
	/// </summary>
	/// <param name="writeLock">The lock object used for write synchronization.</param>
	/// <param name="newValue">The new <typeparamref name="T"/> object to set as the encapsulated instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET9_0_OR_GREATER
	protected void SetInstance(Lock writeLock, in T newValue)
#else
	protected void SetInstance(Object writeLock, in T newValue)
#endif
	{
#if NET9_0_OR_GREATER
		using (writeLock.EnterScope())
#else
		lock (writeLock)
#endif
			this._instance = newValue;
	}
}