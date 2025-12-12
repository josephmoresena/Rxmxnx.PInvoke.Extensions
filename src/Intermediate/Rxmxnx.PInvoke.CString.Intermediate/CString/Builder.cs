namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Represents a mutable <see cref="CString"/> instance.
	/// </summary>
	public sealed partial class Builder
	{
		/// <summary>
		/// The default capacity of a <see cref="Builder"/>.
		/// </summary>
		private const UInt16 defaultCapacity = 32;

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
		/// Initializes a new instance of the <see cref="Builder"/> class.
		/// </summary>
		public Builder() : this(Builder.defaultCapacity) { }
		/// <summary>
		/// Initializes a new instance of the <see cref="Builder"/> class using the specified capacity.
		/// </summary>
		/// <param name="capacity">The suggested starting size of this instance.</param>
		public Builder(UInt16 capacity)
		{
			this._capacity = capacity;
			this._chunk = new(capacity);
		}

		/// <summary>
		/// Removes all units from the current <see cref="Builder"/> instance.
		/// </summary>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder Clear()
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
		public Builder Remove(Int32 startIndex, Int32 length)
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
		/// Converts the value of this instance to a <see cref="CString"/>.
		/// </summary>
		/// <returns>A <see cref="CString"/> whose value is the same as this instance.</returns>
		public CString ToCString()
		{
#if NET9_0_OR_GREATER
			using (this._lock.EnterScope())
#else
			lock (this._lock)
#endif
			{
				Byte[] bytes = CString.CreateByteArray(this._chunk.Count + 1);
				this._chunk.CopyTo(0, bytes.AsSpan()[..^1]);
				return new(bytes, true);
			}
		}
		/// <inheritdoc/>
		public override String ToString() => this.ToCString().ToString();
	}
}