namespace Rxmxnx.PInvoke.VisualBasic;

/// <summary>
/// Represents a platform-specific type used to manage a pointer to a <see cref="Span{T}"/> value.
/// </summary>
/// <typeparam name="T">The type of items in the <see cref="Span{T}"/>.</typeparam>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct SpanPtr<T>
{
	/// <summary>
	/// Indicates if <typeparamref name="T"/> is an <see langword="unmanaged"/> type.
	/// </summary>
	private void* _ptr;

#pragma warning disable CS8500
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="refSpan">A managed <see cref="Span{T}"/> reference.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public SpanPtr(ref Span<T> refSpan)
	{
		fixed (void* ptr = refSpan)
			this._ptr = ptr;
	}

	/// <summary>
	/// Returns the span  pointed by the current pointer.
	/// </summary>
	/// <returns>The span  pointed by the current pointer.</returns>
	/// <exception cref="NullReferenceException">Thrown when the pointer is <see langword="null"/>.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Span<T> GetSpan()
	{
		if (this._ptr == default)
			return default;
		return *(Span<T>*)this._ptr;
	}
#pragma warning restore CS8500

#if NET9_0_OR_GREATER
	/// <summary>
	/// Defines an implicit conversion of a given <see cref="SpanPtr{T}"/> to a value pointer.
	/// </summary>
	/// <param name="spanPtr">A <see cref="SpanPtr{T}"/> to implicitly convert.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator ValPtr<Span<T>>(SpanPtr<T> spanPtr) => (ValPtr<Span<T>>)spanPtr._ptr;
#endif
}