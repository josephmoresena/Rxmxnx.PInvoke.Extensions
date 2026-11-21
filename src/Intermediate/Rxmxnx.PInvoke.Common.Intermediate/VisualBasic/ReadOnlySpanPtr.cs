namespace Rxmxnx.PInvoke.VisualBasic;

/// <summary>
/// Represents a platform-specific type used to manage a pointer to a <see cref="ReadOnlySpan{T}"/> value.
/// </summary>
/// <typeparam name="T">The type of items in the <see cref="ReadOnlySpan{T}"/>.</typeparam>
#if NETFRAMEWORK || NETSTANDARD2_0
[SecurityCritical]
#endif
[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct ReadOnlySpanPtr<T>
{
	/// <summary>
	/// Indicates if <typeparamref name="T"/> is an <see langword="unmanaged"/> type.
	/// </summary>
	private readonly void* _ptr;

#pragma warning disable CS8500
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="refSpan">A managed <see cref="ReadOnlySpan{T}"/> reference.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpanPtr(ref ReadOnlySpan<T> refSpan)
	{
		fixed (void* ptr = &refSpan)
			this._ptr = ptr;
	}

	/// <summary>
	/// Returns the read-only span pointed by the current pointer.
	/// </summary>
	/// <returns>The read-only span pointed by the current pointer.</returns>
	/// <exception cref="NullReferenceException">Thrown when the pointer is <see langword="null"/>.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<T> GetSpan()
	{
		if (this._ptr == default)
			return default;
		return *(ReadOnlySpan<T>*)this._ptr;
	}
#pragma warning restore CS8500

#if NET9_0_OR_GREATER
	/// <summary>
	/// Defines an implicit conversion of a given <see cref="ReadOnlySpanPtr{T}"/> to a value pointer.
	/// </summary>
	/// <param name="spanPtr">A <see cref="ReadOnlySpanPtr{T}"/> to implicitly convert.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator ValPtr<ReadOnlySpan<T>>(ReadOnlySpanPtr<T> spanPtr)
		=> (ValPtr<ReadOnlySpan<T>>)spanPtr._ptr;
#endif
}