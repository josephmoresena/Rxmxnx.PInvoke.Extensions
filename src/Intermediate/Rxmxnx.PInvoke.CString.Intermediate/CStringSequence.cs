namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a sequence of null-terminated UTF-8 text strings.
/// </summary>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(CStringSequenceDebugView))]
public sealed partial class CStringSequence : ICloneable, IEquatable<CStringSequence>
{
	/// <summary>
	/// Size of <see cref="Char"/> value in bytes.
	/// </summary>
	internal const Int32 SizeOfChar = sizeof(Char);

	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class
	/// from a collection of strings.
	/// </summary>
	/// <param name="values">The collection of strings.</param>
	public CStringSequence(params String?[] values)
	{
		List<CString?> cvalues = new(values.Length);
		this._lengths = new Int32?[values.Length];
		for (Int32 i = 0; i < values.Length; i++)
		{
			CString? cstr = CStringSequence.GetCString(values[i]);
			cvalues.Add(cstr);
			this._lengths[i] = cstr?.Length;
		}
		this._value = CStringSequence.CreateBuffer(cvalues);
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class from a
	/// collection of <see cref="CString"/> instances.
	/// </summary>
	/// <param name="values">The collection of <see cref="CString"/> instances.</param>
	public CStringSequence(params CString?[] values)
	{
		this._lengths = values.Select(CStringSequence.GetLength).ToArray();
		this._value = CStringSequence.CreateBuffer(values);
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class from an
	/// enumerable collection of strings.
	/// </summary>
	/// <param name="values">The enumerable collection of strings.</param>
	public CStringSequence(IEnumerable<String?> values)
	{
		List<CString?> cvalues = values.Select(CStringSequence.GetCString).ToList();
		this._lengths = cvalues.Select(CStringSequence.GetLength).ToArray();
		this._value = CStringSequence.CreateBuffer(cvalues);
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class from an
	/// enumerable collection of <see cref="CString"/> instances.
	/// </summary>
	/// <param name="values">The enumerable collection of <see cref="CString"/> instances.</param>
	public CStringSequence(IEnumerable<CString?> values) : this(values.ToArray())
	{
		List<CString?> cvalues = values.ToList();
		this._lengths = cvalues.Select(CStringSequence.GetLength).ToArray();
		this._value = CStringSequence.CreateBuffer(cvalues);
	}
	/// <summary>
	/// Creates a copy of this instance of <see cref="CStringSequence"/>.
	/// </summary>
	/// <returns>A new object that is a copy of this instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Object Clone() => new CStringSequence(this);
	/// <summary>
	/// Determines whether the current <see cref="CStringSequence"/> is equal to another
	/// <see cref="CStringSequence"/> instance.
	/// </summary>
	/// <param name="other">The <see cref="CStringSequence"/> to compare with this object.</param>
	/// <returns>
	/// <see langword="true"/> if the current <see cref="CStringSequence"/> is equal to the
	/// <paramref name="other"/> parameter; otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Boolean Equals(CStringSequence? other)
		=> other is not null && this._value.Equals(other._value) && this._lengths.SequenceEqual(other._lengths);

	/// <summary>
	/// Returns a reference to the first UTF-8 byte of the <see cref="CStringSequence"/>.
	/// </summary>
	/// <remarks>
	/// Required to support the use of a <see cref="CStringSequence"/> within a fixed statement.
	/// It should not be used in typical code.
	/// </remarks>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ref readonly Byte GetPinnableReference()
		=> ref MemoryMarshal.GetReference(MemoryMarshal.AsBytes(this._value.AsSpan()));
	/// <summary>
	/// Returns a <see cref="CString"/> that represents the current sequence.
	/// </summary>
	/// <returns>A <see cref="CString"/> that represents the current sequence.</returns>
	public CString ToCString()
	{
		Int32 bytesLength = this._lengths.Sum(CStringSequence.GetSpanLength);
		Byte[] result = new Byte[bytesLength];
		this.WithSafeTransform(result, CStringSequence.BinaryCopyTo);
		return result;
	}

	/// <inheritdoc/>
	public override Boolean Equals(Object? obj) => obj is CStringSequence cstr && this.Equals(cstr);
	/// <inheritdoc/>
	public override String ToString() => this._value;
	/// <inheritdoc/>
	public override Int32 GetHashCode() => this._value.GetHashCode();

	/// <summary>
	/// Creates a new UTF-8 text sequence with specific lengths, and initializes each
	/// UTF-8 text string in it after creation using the specified callback.
	/// </summary>
	/// <typeparam name="TState">The type of the element to pass to the <paramref name="action"/>.</typeparam>
	/// <param name="lengths">The lengths of the UTF-8 text sequence to create.</param>
	/// <param name="state">The element to pass to the <paramref name="action"/>.</param>
	/// <param name="action">A callback to initialize each <see cref="CString"/>.</param>
	/// <returns>The created UTF-8 text sequence.</returns>
	public static CStringSequence Create<TState>(TState state, CStringSequenceCreationAction<TState> action,
		params Int32?[] lengths)
	{
		Int32 bytesLength = lengths.Sum(CStringSequence.GetSpanLength);
		Int32 length = bytesLength / CStringSequence.SizeOfChar + bytesLength % CStringSequence.SizeOfChar;
		String buffer = String.Create(length, new SequenceCreationState<TState>(state, action, lengths),
		                              CStringSequence.CreateCStringSequence);
		return new(buffer, lengths);
	}
}