namespace Rxmxnx.PInvoke.RefStruct;

/// <summary>
/// Ordinary struct holding the address of a <see cref="ReadOnlySpan{T}"/> variable.
/// </summary>
/// <typeparam name="T">The type of items in the <see cref="ReadOnlySpan{T}"/>.</typeparam>
/// <remarks>
/// Exists for runtimes without <c>allows ref struct</c>, where <see cref="ReadOnlyValPtr{T}"/> cannot point at a
/// <see cref="ReadOnlySpan{T}"/>. On .NET 9 and later, <see cref="ReadOnlyValPtr{T}"/> of
/// <see cref="ReadOnlySpan{T}"/> replaces it.
/// Taking the address requires a live span variable. The returned pointer is unsafe once that variable is gone.
/// </remarks>
#if NET9_0_OR_GREATER
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
#endif
[StructLayout(LayoutKind.Sequential)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public readonly unsafe struct ReadOnlySpanPtr<T>
{
	/// <summary>
	/// Address of the referenced <see cref="ReadOnlySpan{T}"/>.
	/// </summary>
	private readonly void* _ptr;

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="refSpan">A managed <see cref="ReadOnlySpan{T}"/> reference.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private ReadOnlySpanPtr(ref ReadOnlySpan<T> refSpan)
	{
		// &refSpan is the address of the span value. Without '&', fixed pins the span's buffer.
#pragma warning disable CS8500
		fixed (void* ptr = &refSpan)
			this._ptr = ptr;
#pragma warning restore CS8500
	}

	/// <summary>
	/// Creates an unsafe pointer to <paramref name="refSpan"/>.
	/// </summary>
	/// <param name="refSpan">The read-only span variable to address.</param>
	/// <returns>A pointer to <paramref name="refSpan"/>.</returns>
	/// <remarks>
	/// The span variable must stay alive for as long as the pointer is used.
	/// </remarks>
#if NET9_0_OR_GREATER
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ReadOnlySpanPtr<T> CreateUnsafePtr(ref ReadOnlySpan<T> refSpan) => new(ref refSpan);

	/// <summary>
	/// Returns the read-only span pointed by the current pointer.
	/// </summary>
	/// <returns>The read-only span pointed by the current pointer, or the default value when the pointer is zero.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<T> GetSpan()
	{
		if (this._ptr == default)
			return default;
#pragma warning disable CS8500
		return *(ReadOnlySpan<T>*)this._ptr;
#pragma warning restore CS8500
	}

#if NET9_0_OR_GREATER
	/// <summary>
	/// Transitions to <see cref="ReadOnlyValPtr{T}"/> where <c>allows ref struct</c> is available.
	/// </summary>
	/// <param name="spanPtr">A <see cref="ReadOnlySpanPtr{T}"/> to implicitly convert.</param>
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator ReadOnlyValPtr<ReadOnlySpan<T>>(ReadOnlySpanPtr<T> spanPtr)
		=> Unsafe.As<ReadOnlySpanPtr<T>, ReadOnlyValPtr<ReadOnlySpan<T>>>(ref spanPtr);
	/// <summary>
	/// Transitions from <see cref="ReadOnlyValPtr{T}"/> back to <see cref="ReadOnlySpanPtr{T}"/>.
	/// </summary>
	/// <param name="valPtr">A <see cref="ReadOnlyValPtr{T}"/> to explicitly convert.</param>
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator ReadOnlySpanPtr<T>(ReadOnlyValPtr<ReadOnlySpan<T>> valPtr)
		=> Unsafe.As<ReadOnlyValPtr<ReadOnlySpan<T>>, ReadOnlySpanPtr<T>>(ref valPtr);
#endif
}