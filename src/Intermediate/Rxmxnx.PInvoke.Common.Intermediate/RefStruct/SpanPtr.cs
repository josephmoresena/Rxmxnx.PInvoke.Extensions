namespace Rxmxnx.PInvoke.RefStruct;

/// <summary>
/// Ordinary struct holding the address of a <see cref="Span{T}"/> variable.
/// </summary>
/// <typeparam name="T">The type of items in the <see cref="Span{T}"/>.</typeparam>
/// <remarks>
/// Exists for runtimes without <c>allows ref struct</c>, where <see cref="ValPtr{T}"/> cannot point at a
/// <see cref="Span{T}"/>. On .NET 9 and later, <see cref="ValPtr{T}"/> of <see cref="Span{T}"/> replaces it.
/// Taking the address requires a live span variable. The returned pointer is unsafe once that variable is gone.
/// </remarks>
#if NET9_0_OR_GREATER
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
#endif
#if !NET5_0_OR_GREATER
[Preserve(AllMembers = true)]
#endif
[StructLayout(LayoutKind.Sequential)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public readonly unsafe struct SpanPtr<T>
{
	/// <summary>
	/// Address of the referenced <see cref="Span{T}"/>.
	/// </summary>
	private readonly void* _ptr;

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="refSpan">A managed <see cref="Span{T}"/> reference.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private SpanPtr(ref Span<T> refSpan)
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
	/// <param name="refSpan">The span variable to address.</param>
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
	public static SpanPtr<T> CreateUnsafePtr(ref Span<T> refSpan) => new(ref refSpan);

	/// <summary>
	/// Returns the span pointed by the current pointer.
	/// </summary>
	/// <returns>The span pointed by the current pointer, or the default value when the pointer is zero.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Span<T> GetSpan()
	{
		if (this._ptr == default)
			return default;
#pragma warning disable CS8500
		return *(Span<T>*)this._ptr;
#pragma warning restore CS8500
	}

#if NET9_0_OR_GREATER
	/// <summary>
	/// Transitions to <see cref="ValPtr{T}"/> where <c>allows ref struct</c> is available.
	/// </summary>
	/// <param name="spanPtr">A <see cref="SpanPtr{T}"/> to implicitly convert.</param>
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator ValPtr<Span<T>>(SpanPtr<T> spanPtr)
		=> Unsafe.As<SpanPtr<T>, ValPtr<Span<T>>>(ref spanPtr);
	/// <summary>
	/// Transitions from <see cref="ValPtr{T}"/> back to <see cref="SpanPtr{T}"/>.
	/// </summary>
	/// <param name="valPtr">A <see cref="ValPtr{T}"/> to explicitly convert.</param>
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator SpanPtr<T>(ValPtr<Span<T>> valPtr)
		=> Unsafe.As<ValPtr<Span<T>>, SpanPtr<T>>(ref valPtr);
#endif
}