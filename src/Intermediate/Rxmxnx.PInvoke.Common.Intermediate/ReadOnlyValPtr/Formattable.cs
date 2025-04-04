namespace Rxmxnx.PInvoke;

public readonly partial struct ReadOnlyValPtr<T> : ISpanFormattable
{
	/// <inheritdoc cref="IntPtr.ToString(IFormatProvider?)"/>
	public String ToString(IFormatProvider? formatProvider) => this.Pointer.ToString(formatProvider);
	/// <inheritdoc cref="IntPtr.ToString(String, IFormatProvider?)"/>
	public String ToString(String? format, IFormatProvider? formatProvider)
		=> this.Pointer.ToString(format, formatProvider);
	/// <inheritdoc/>
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1006)]
	public Boolean TryFormat(Span<Char> destination, out Int32 charsWritten, ReadOnlySpan<Char> format = default,
		IFormatProvider? provider = default)
		=> this.Pointer.TryFormat(destination, out charsWritten, format, provider);
}