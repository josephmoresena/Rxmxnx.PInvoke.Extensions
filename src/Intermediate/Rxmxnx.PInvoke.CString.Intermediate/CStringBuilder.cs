namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a mutable <see cref="CString"/> instance.
/// </summary>
[DebuggerTypeProxy(typeof(CStringBuilderDebugView))]
public sealed partial class CStringBuilder
{
	/// <summary>
	/// The default capacity of a <see cref="CStringBuilder"/>.
	/// </summary>
	internal const UInt16 DefaultCapacity = 16;

	/// <summary>
	/// Lock object.
	/// </summary>
#if NET9_0_OR_GREATER
	private readonly Lock _lock = new();
#else
	private readonly Object _lock = new();
#endif
	/// <summary>
	/// Initial capacity.
	/// </summary>
	private readonly UInt16 _capacity;
	/// <summary>
	/// Current string chunk.
	/// </summary>
	private Chunk _chunk;

	/// <summary>
	/// Gets the length of the current <see cref="CStringBuilder"/> object.
	/// </summary>
	public Int32 Length
	{
		get
		{
#if NET9_0_OR_GREATER
			using (this._lock.EnterScope())
#else
			lock (this._lock)
#endif
				return this._chunk.Count;
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CStringBuilder"/> class.
	/// </summary>
	public CStringBuilder() : this(CStringBuilder.DefaultCapacity) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringBuilder"/> class.
	/// </summary>
	// <param name="value">The initial contents of this builder.</param>
	public CStringBuilder(String? value) : this((ReadOnlySpan<Char>)value) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringBuilder"/> class.
	/// </summary>
	// <param name="value">The initial contents of this builder.</param>
	public CStringBuilder(CString? value) : this((ReadOnlySpan<Byte>)value) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringBuilder"/> class.
	/// </summary>
	// <param name="value">The initial contents of this builder.</param>
	// ReSharper disable once MemberCanBePrivate.Global
	public CStringBuilder(ReadOnlySpan<Byte> value) : this(CStringBuilder.GetCapacityFor(value))
		=> this._chunk = this._chunk.Append(value);
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringBuilder"/> class.
	/// </summary>
	// <param name="value">The initial contents of this builder.</param>
	// ReSharper disable once MemberCanBePrivate.Global
	public CStringBuilder(ReadOnlySpan<Char> value) : this(CStringBuilder.GetCapacityFor(value))
		=> this._chunk = this._chunk.Append(value);
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringBuilder"/> class using the specified capacity.
	/// </summary>
	/// <param name="capacity">The suggested starting size of this instance.</param>
	public CStringBuilder(UInt16 capacity)
	{
		this._capacity = Math.Max(capacity, CStringBuilder.DefaultCapacity);
		this._chunk = new(capacity);
	}

	/// <summary>
	/// Removes all units from the current <see cref="CStringBuilder"/> instance.
	/// </summary>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder Clear()
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
			this._chunk.Reset(this._capacity);
		return this;
	}
	/// <summary>
	/// Removes the specified range of UTF-8 from this instance.
	/// </summary>
	/// <param name="startIndex">The zero-based position in this instance where removal begins.</param>
	/// <param name="length">The number of UTF-8 units to remove.</param>
	/// <returns>A reference to this instance after the excise operation has completed.</returns>
	public CStringBuilder Remove(Int32 startIndex, Int32 length)
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
			this._chunk.Remove(startIndex, length);
		return this;
	}
	/// <summary>
	/// Copies UTF-8 units starting at the specified index of this instance into the provided destination
	/// <see cref="Byte"/> span and returns the total number of units actually copied.
	/// </summary>
	/// <param name="index">
	/// The zero-based starting position in this instance from which UTF-8 units will be copied.
	/// </param>
	/// <param name="destination">
	/// The destination span that receives the copied UTF-8 units. The number of units copied is limited
	/// by the length of this span.
	/// </param>
	/// <returns>
	/// The number of UTF-8 units copied to <paramref name="destination"/>. This value is the lesser of
	/// the available UTF-8 units starting at <paramref name="index"/> and the length of the destination span.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public Int32 CopyTo(Int32 index, Span<Byte> destination)
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
		{
			Int32 length = Math.Min(destination.Length, this._chunk.Count - index);
			this._chunk.CopyTo(index, destination[..length]);
			return length;
		}
	}
	/// <summary>
	/// Converts the value of this instance to a <see cref="CString"/>.
	/// </summary>
	/// <returns>A <see cref="CString"/> whose value is the same as this instance.</returns>
	public CString ToCString() => this.ToCString(true);
	/// <summary>
	/// Converts the value of this instance to a <see cref="CString"/>.
	/// </summary>
	/// <param name="nullTerminated">
	/// Indicates whether the resulting <see cref="CString"/> is null-terminated.
	/// </param>
	/// <returns>A <see cref="CString"/> whose value is the same as this instance.</returns>
	// ReSharper disable once MemberCanBePrivate.Global
	public CString ToCString(Boolean nullTerminated)
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
		{
			if (this._chunk.Count == 0) return CString.Empty;
			Byte[] bytes = this.GetDataBytes(nullTerminated);
			return nullTerminated ? bytes : CString.Create(bytes);
		}
	}
	/// <inheritdoc/>
	public override String ToString() => this.ToCString(false).ToString();

	/// <summary>
	/// Retrieves the current debug information.
	/// </summary>
	/// <param name="length">Output. Builder length.</param>
	/// <param name="chunks">Output. Chunks information array.</param>
	/// <returns>Current instance data.</returns>
	internal String GetDebugInfo(out Int32 length, out CStringBuilderDebugView.ChunkInfo[] chunks)
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
		{
			length = this._chunk.Count;
			chunks = this._chunk.EnumerateInformation().Reverse().ToArray();
			return Encoding.UTF8.GetString(this.GetDataBytes(false));
		}
	}

	/// <summary>
	/// Retrieves a byte array containing all the usable bytes from current instance.
	/// </summary>
	/// <param name="nullTerminated">Indicates whether the returning array is UTF-8 null-terminated.</param>
	/// <returns>A byte array containing all the usable bytes from current instance.</returns>
	private Byte[] GetDataBytes(Boolean nullTerminated)
	{
		Int32 length = nullTerminated ? 1 : 0;
		Byte[] bytes = CString.CreateByteArray(this._chunk.Count + length);
		Span<Byte> span = bytes.AsSpan()[..^length];
		this._chunk.CopyTo(0, span);
		if (nullTerminated)
			bytes.AsSpan()[span.Length..].Clear();
		return bytes;
	}

	/// <summary>
	/// Retrieves the capacity for a <see cref="CStringBuilder"/> initialized with <paramref name="initialValue"/>.
	/// </summary>
	/// <param name="initialValue">Initial builder value.</param>
	/// <returns>Builder capacity.</returns>
	private static UInt16 GetCapacityFor(ReadOnlySpan<Byte> initialValue)
	{
		if (initialValue.Length < CStringBuilder.DefaultCapacity)
			return CStringBuilder.DefaultCapacity;
		return (UInt16)Math.Min(initialValue.Length, Chunk.MaxLength);
	}
	/// <summary>
	/// Retrieves the capacity for a <see cref="CStringBuilder"/> initialized with <paramref name="initialValue"/>.
	/// </summary>
	/// <param name="initialValue">Initial builder value.</param>
	/// <returns>Builder capacity.</returns>
	private static UInt16 GetCapacityFor(ReadOnlySpan<Char> initialValue)
	{
		Int32 byteCount = Encoding.UTF8.GetByteCount(initialValue);
		if (byteCount < CStringBuilder.DefaultCapacity)
			return CStringBuilder.DefaultCapacity;
		return (UInt16)Math.Min(byteCount, Chunk.MaxLength);
	}
}