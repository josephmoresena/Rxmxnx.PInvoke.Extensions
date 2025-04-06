namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for concatenating <see cref="CString"/> instances.
/// </summary>
internal sealed class CStringConcatenator : BinaryConcatenator<CString>
{
	/// <summary>
	/// Indicates whether the empty values should be ignored during the
	/// concatenation process.
	/// </summary>
	private readonly Boolean _ignoreEmpty;

	/// <summary>
	/// Initializes a new instance of the <see cref="CStringConcatenator"/> class
	/// without a specified separator.
	/// </summary>
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests.
	/// The default value is <see cref="CancellationToken.None"/>.
	/// </param>
	public CStringConcatenator(CancellationToken cancellationToken = default) : this(default, cancellationToken) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringConcatenator"/> class
	/// with a specified separator.
	/// </summary>
	/// <param name="separator">
	/// The <see cref="CString"/> to use as a separator in the concatenation.
	/// </param>
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests.
	/// The default value is <see cref="CancellationToken.None"/>.
	/// </param>
	public CStringConcatenator(CString? separator, CancellationToken cancellationToken = default) :
		base(separator, cancellationToken)
		=> this._ignoreEmpty = !CString.IsNullOrEmpty(separator);

	/// <inheritdoc/>
	protected override void WriteValue(CString? value) => value?.Write(this.Stream, false);
	/// <inheritdoc/>
	protected override Task WriteValueAsync(CString? value)
		=> value?.WriteAsync(this.Stream, false, this.CancellationToken) ?? Task.CompletedTask;
	/// <inheritdoc/>
	protected override Boolean IsEmpty(CString? value) => CString.IsNullOrEmpty(value) && !this._ignoreEmpty;
	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	protected override Boolean IsEmpty(ReadOnlySpan<Byte> value) => base.IsEmpty(value) && !this._ignoreEmpty;
}