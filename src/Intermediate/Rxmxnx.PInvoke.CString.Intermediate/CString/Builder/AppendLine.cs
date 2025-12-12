namespace Rxmxnx.PInvoke;

public partial class CString
{
	public sealed partial class Builder
	{
		/// <summary>
		/// Appends the default line terminator to the end of the current instance.
		/// </summary>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder AppendLine()
		{
#if NET9_0_OR_GREATER
			using (this._lock.EnterScope())
#else
			lock (this._lock)
#endif
				this._chunk = this._chunk.Append(CString.NewLine);
			return this;
		}
		/// <summary>
		/// Appends the UTF-8 representation of the specified string followed by the default line terminator to the
		/// end of the current instance.
		/// </summary>
		/// <param name="value">The string to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder AppendLine(String? value) => this.AppendLine((ReadOnlySpan<Char>)value);
		/// <summary>
		/// Appends the UTF-8 representation of the characters in the specified array followed by the default line
		/// terminator to the end of the current instance.
		/// </summary>
		/// <param name="value">The array of characters to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder AppendLine(Char[]? value) => this.AppendLine((ReadOnlySpan<Char>)value);
		/// <summary>
		/// Appends the specified UTF-8 text followed by the default line terminator to the end of
		/// the current instance.
		/// </summary>
		/// <param name="value">The UTF-8 text to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder AppendLine(CString? value) => this.Append((ReadOnlySpan<Byte>)value);
		/// <summary>
		/// Appends the specified UTF-8 units array followed by the default line terminator to the end of
		/// the current instance.
		/// </summary>
		/// <param name="value">The UTF-8 units array to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder AppendLine(Byte[]? value) => this.Append((ReadOnlySpan<Byte>)value);
		/// <summary>
		/// Appends the UTF-8 representation of the specified character span followed by the default line terminator to
		/// the end of the current instance.
		/// </summary>
		/// <param name="value">The read-only span of characters to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder AppendLine(ReadOnlySpan<Char> value)
		{
#if NET9_0_OR_GREATER
			using (this._lock.EnterScope())
#else
			lock (this._lock)
#endif
				this._chunk = this._chunk.Append(value).Append(CString.NewLine);
			return this;
		}
		/// <summary>
		/// Appends the specified UTF-8 units read-only span followed by the default line terminator to the end of
		/// the current instance.
		/// </summary>
		/// <param name="value">The UTF-8 units read-only span to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder AppendLine(ReadOnlySpan<Byte> value)
		{
#if NET9_0_OR_GREATER
			using (this._lock.EnterScope())
#else
			lock (this._lock)
#endif
				this._chunk = this._chunk.Append(value).Append(CString.NewLine);
			return this;
		}
	}
}