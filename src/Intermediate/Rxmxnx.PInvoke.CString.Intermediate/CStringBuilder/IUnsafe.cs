namespace Rxmxnx.PInvoke;

#if !NET9_0_OR_GREATER
public sealed partial class CStringBuilder : CStringBuilder.IUnsafe<Object>
{
	Object IUnsafe<Object>.Lock => this._lock;
#else
public sealed partial class CStringBuilder : CStringBuilder.IUnsafe<Object>, CStringBuilder.IUnsafe<Lock>
{
	Lock IUnsafe<Lock>.Lock => this._lock;
#pragma warning disable CS9216
	Object IUnsafe<Object>.Lock => this._lock;
#pragma warning restore CS9216
#endif

	Int32 IUnsafe.Count => this._chunk.Count;

	void IUnsafe.Append(ReadOnlySpan<Byte> value) => this._chunk = this._chunk.Append(value);
	void IUnsafe.Append(ReadOnlySequence<Byte> value) => this._chunk = this._chunk.Append(value);
	void IUnsafe.Append(ReadOnlySpan<Char> value) => this._chunk = this._chunk.Append(value);
	void IUnsafe.Insert(Int32 index, ReadOnlySpan<Char> value) => this._chunk.Insert(index, value);
	void IUnsafe.Insert(Int32 index, ReadOnlySpan<Byte> value) => this._chunk.Insert(index, value);
	void IUnsafe.Clear() => this._chunk.Reset(this._capacity);
	void IUnsafe.Remove(Int32 startIndex, Int32 length) => this._chunk.Remove(startIndex, length);
	void IUnsafe.CopyTo(Span<Byte> destination)
	{
		Int32 length = Math.Min(destination.Length, this._chunk.Count);
		this._chunk.CopyTo(0, destination[..length]);
	}

	/// <summary>
	/// Unsafe <see cref="CStringBuilder"/> instance.
	/// </summary>
	internal interface IUnsafe
	{
		/// <summary>
		/// The number of bytes in the entire sequence.
		/// </summary>
		Int32 Count { get; }

		/// <summary>
		/// Appends the specified UTF-8 units read-only span to this instance.
		/// </summary>
		/// <param name="value">The UTF-8 units read-only span to append.</param>
		void Append(ReadOnlySpan<Byte> value);
		/// <summary>
		/// Appends the specified UTF-8 units read-only sequence to this instance.
		/// </summary>
		/// <param name="value">The UTF-8 units read-only sequence to append.</param>
		void Append(ReadOnlySequence<Byte> value);
		/// <summary>
		/// Appends the UTF-8 representation of the characters in the specified read-only span to this instance.
		/// </summary>
		/// <param name="value">The read-only span of characters to append.</param>
		void Append(ReadOnlySpan<Char> value);
		/// <summary>
		/// Inserts UTF-8 representation of the characters in the specified read-only span into this instance at the
		/// specified UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The read-only span of characters to insert.</param>
		void Insert(Int32 index, ReadOnlySpan<Char> value);
		/// <summary>
		/// Inserts the specified UTF-8 units read-only span into this instance at the specified UTF-8 unit position.
		/// </summary>
		/// <param name="index">The position in this instance where insertion begins.</param>
		/// <param name="value">The read-only span of characters to insert.</param>
		void Insert(Int32 index, ReadOnlySpan<Byte> value);
		/// <summary>
		/// Removes all units from the current instance.
		/// </summary>
		void Clear();
		/// <summary>
		/// Removes the specified range of UTF-8 from this instance.
		/// </summary>
		/// <param name="startIndex">The zero-based position in this instance where removal begins.</param>
		/// <param name="length">The number of UTF-8 units to remove.</param>
		void Remove(Int32 startIndex, Int32 length);
		/// <summary>
		/// Copies the units from the beginning of this instance to a destination <see cref="Byte"/> span.
		/// </summary>
		/// <param name="destination">The writable span where units will be copied.</param>
		void CopyTo(Span<Byte> destination);
	}

	/// <summary>
	/// Unsafe <see cref="CStringBuilder"/> instance.
	/// </summary>
	/// <typeparam name="TLock">Type of lock object.</typeparam>
	internal interface IUnsafe<out TLock> : IUnsafe
	{
		/// <summary>
		/// The lock object.
		/// </summary>
		TLock Lock { get; }
	}
}