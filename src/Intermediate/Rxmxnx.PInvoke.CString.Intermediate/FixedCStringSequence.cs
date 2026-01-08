namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a <see cref="CStringSequence"/> that is fixed in memory.
/// </summary>
[DebuggerTypeProxy(typeof(CStringSequenceDebugView))]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1121)]
#endif
public readonly unsafe ref struct FixedCStringSequence
{
	/// <summary>
	/// The <see cref="CString"/> representation of UTF-8 sequence.
	/// </summary>
	private readonly CString? _value;
	/// <summary>
	/// Array of <see cref="CString"/> values.
	/// </summary>
	private readonly CString[]? _values;
	/// <summary>
	/// Indicates whether the current instance remains valid.
	/// </summary>
	private readonly IMutableWrapper<Boolean>? _isValid;

	/// <summary>
	/// Gets the list of <see cref="CString"/> values in the sequence.
	/// </summary>
	public IReadOnlyList<CString> Values => this._values ?? [];
	/// <summary>
	/// Gets the element at the given index in the sequence.
	/// </summary>
	/// <param name="index">A position in the current instance.</param>
	/// <returns>The object at position <paramref name="index"/>.</returns>
	/// <exception cref="IndexOutOfRangeException">
	/// Thrown when <paramref name="index"/> is greater than or equal to the length of this object or less than zero.
	/// </exception>
	[IndexerName("Item")]
	public IReadOnlyFixedMemory this[Int32 index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			ValidationUtilities.ThrowIfInvalidSequenceIndex(index, this.Values.Count);
			return this.GetFixedCString(index);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FixedCStringSequence"/> struct.
	/// </summary>
	/// <param name="values">Array of <see cref="CString"/> values.</param>
	/// <param name="value">The <see cref="CString"/> representation of UTF-8 sequence.</param>
	internal FixedCStringSequence(CString[] values, CString value)
	{
		this._values = values;
		this._value = value;
		this._isValid = IMutableWrapper.Create(true);
	}

	/// <summary>
	/// Creates an array of <see cref="IReadOnlyFixedMemory"/> instances from the current instance.
	/// </summary>
	/// <returns>An array of <see cref="IReadOnlyFixedMemory"/> instances.</returns>
	public IReadOnlyFixedMemory[] ToArray()
	{
		IReadOnlyFixedMemory[] result = new IReadOnlyFixedMemory[this.Values.Count];
		for (Int32 i = 0; i < result.Length; i++)
			result[i] = this[i];
		return result;
	}

	/// <inheritdoc/>
	public override String? ToString() => this._value?.ToString();

	/// <summary>
	/// Implicitly converts a <see cref="FixedCStringSequence"/> to a <see cref="ReadOnlyFixedMemoryList"/>.
	/// </summary>
	/// <param name="fseq">A <see cref="FixedCStringSequence"/> instance.</param>
	public static implicit operator ReadOnlyFixedMemoryList(FixedCStringSequence fseq)
	{
		ReadOnlyFixedMemory[] memories = new ReadOnlyFixedMemory[fseq.Values.Count];
		for (Int32 i = 0; i < memories.Length; i++)
			memories[i] = fseq.GetFixedCString(i);
		return new(memories);
	}

	/// <summary>
	/// Retrieves read-only span enumerator from current instance.
	/// </summary>
	/// <returns>A read-only span enumerator from current instance.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public ReadOnlySpan<CString?>.Enumerator GetEnumerator()
	{
		ReadOnlySpan<CString?> span = this._values;
		return span.GetEnumerator();
	}

	/// <summary>
	/// Invalidates the current sequence.
	/// </summary>
	internal void Unload() => this._isValid?.Value = false;

	/// <summary>
	/// Retrieves the <see cref="ReadOnlyFixedContext{Byte}"/> for the element at the specified <paramref name="index"/>.
	/// </summary>
	/// <param name="index">The index of the element to retrieve.</param>
	/// <returns>A <see cref="ReadOnlyFixedContext{Byte}"/> for the element at the specified <paramref name="index"/>.</returns>
	private ReadOnlyFixedContext<Byte> GetFixedCString(Int32 index)
	{
		CString cstr = this._values![index];
		fixed (void* ptr = cstr) // Use Pinnable reference
			return new(ptr, cstr.Length, this._isValid!);
	}
}