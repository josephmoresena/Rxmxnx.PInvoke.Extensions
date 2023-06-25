namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// A helper class for binary data concatenation.
/// </summary>
internal sealed class BinaryConcatenator : BinaryConcatenator<Byte?>
{
	/// <summary>
	/// Determines whether empty values should be ignored during
	/// concatenation.
	/// </summary>
	private readonly Boolean _ignoreEmpty;

	/// <summary>
	/// Initializes a new instance of the <see cref="BinaryConcatenator"/> class
	/// without a specified separator byte.
	/// </summary>
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests.
	/// The default value is <see cref="CancellationToken.None"/>.
	/// </param>
	public BinaryConcatenator(CancellationToken cancellationToken = default) : base(default, cancellationToken) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="BinaryConcatenator"/> class
	/// with a specified separator byte.
	/// </summary>
	/// <param name="separator">
	/// The byte value to use as a separator in the concatenation.
	/// </param>
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests.
	/// The default value is <see cref="CancellationToken.None"/>.
	/// </param>
	public BinaryConcatenator(Byte separator, CancellationToken cancellationToken = default) :
		base(separator, cancellationToken)
		=> this._ignoreEmpty = true;

	/// <inheritdoc/>
	protected override void WriteValue(Byte? value) => this.Stream.WriteByte(value!.Value);
	/// <inheritdoc/>
	protected override Task WriteValueAsync(Byte? value)
		=> Task.Run(() => this.Stream.WriteByte(value!.Value), this.CancellationToken);
	/// <inheritdoc/>
	protected override Boolean IsEmpty([NotNullWhen(false)] Byte? value) => !value.HasValue;

	/// <summary>
	/// Retrieves the binary data from the UTF-8 text.
	/// </summary>
	/// <param name="nullTerminated">
	/// Specifies whether the UTF-8 text should be null-terminated.
	/// </param>
	/// <returns>A byte array containing the binary data from the UTF-8 text.</returns>
	public new Byte[]? ToArray(Boolean nullTerminated) => base.ToArray(nullTerminated);

	/// <inheritdoc/>
	protected override Boolean IsEmpty(ReadOnlySpan<Byte> value) => base.IsEmpty(value) && !this._ignoreEmpty;
}