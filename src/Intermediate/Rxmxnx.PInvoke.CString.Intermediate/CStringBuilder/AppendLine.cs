namespace Rxmxnx.PInvoke;

public sealed partial class CStringBuilder
{
	/// <summary>
	/// Appends the default line terminator to the end of the current instance.
	/// </summary>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder AppendLine()
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
	public CStringBuilder AppendLine(String? value) => this.AppendLine((ReadOnlySpan<Char>)value);
	/// <summary>
	/// Appends the UTF-8 representation of the characters in the specified array followed by the default line
	/// terminator to the end of the current instance.
	/// </summary>
	/// <param name="value">The array of characters to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder AppendLine(Char[]? value) => this.AppendLine((ReadOnlySpan<Char>)value);
	/// <summary>
	/// Appends the specified UTF-8 text followed by the default line terminator to the end of
	/// the current instance.
	/// </summary>
	/// <param name="value">The UTF-8 text to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder AppendLine(CString? value) => this.AppendLine((ReadOnlySpan<Byte>)value);
	/// <summary>
	/// Appends the specified UTF-8 units array followed by the default line terminator to the end of
	/// the current instance.
	/// </summary>
	/// <param name="value">The UTF-8 units array to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder AppendLine(Byte[]? value) => this.AppendLine((ReadOnlySpan<Byte>)value);
	/// <summary>
	/// Appends the UTF-8 representation of the specified character span followed by the default line terminator to
	/// the end of the current instance.
	/// </summary>
	/// <param name="value">The read-only span of characters to append.</param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	// ReSharper disable once MemberCanBePrivate.Global
	public CStringBuilder AppendLine(ReadOnlySpan<Char> value)
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
	public CStringBuilder AppendLine(ReadOnlySpan<Byte> value)
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