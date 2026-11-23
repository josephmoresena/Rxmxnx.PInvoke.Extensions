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
	public CStringConcatenator() : this(default) { }
#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringConcatenator"/> class
	/// with a specified separator.
	/// </summary>
	/// <param name="separator">
	/// The <see cref="CString"/> to use as a separator in the concatenation.
	/// </param>
	public CStringConcatenator(CString? separator) : base(separator)
		=> this._ignoreEmpty = !CString.IsNullOrEmpty(separator);
#else
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringConcatenator"/> class
	/// without a specified separator.
	/// </summary>
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests.
	/// The default value is <see cref="CancellationToken.None"/>.
	/// </param>
	public CStringConcatenator(CancellationToken cancellationToken) : this(default, cancellationToken) { }
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
#endif

	/// <inheritdoc/>
	protected override void WriteValue(CString? value) => value?.Write(this.Stream, false);
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
	/// <inheritdoc/>
	protected override Task WriteValueAsync(CString? value)
		=> value?.WriteAsync(this.Stream, false, this.CancellationToken) ?? Task.CompletedTask;
#endif
	/// <inheritdoc/>
	protected override Boolean IsEmpty(CString? value) => CString.IsNullOrEmpty(value) && !this._ignoreEmpty;
	/// <inheritdoc/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	protected override Boolean IsEmpty(ReadOnlySpan<Byte> value) => base.IsEmpty(value) && !this._ignoreEmpty;
}