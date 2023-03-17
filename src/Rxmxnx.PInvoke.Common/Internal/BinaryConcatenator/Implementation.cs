namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for Binary concatenation.
/// </summary>
internal sealed class BinaryConcatenator : BinaryConcatenator<Byte?>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public BinaryConcatenator() : base(default, IsNullByte)
    {
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="separator"><see cref="Byte"/> separator instance.</param>
    public BinaryConcatenator(Byte separator) : base(separator, IsNullByte)
    {
    }

    /// <inheritdoc/>
    protected override void WriteValue(Byte? value)
        => this.Stream.WriteByte(value!.Value);

    /// <inheritdoc/>
    protected override Task WriteValueAsync(Byte? value)
        => Task.Run(() => this.Stream.WriteByte(value!.Value));

    /// <summary>
    /// Retrieves the binary data of UTF-8 text.
    /// </summary>
    /// <param name="nullTerminated">Indicates whether the UTF-8 text must be null-terminated.</param>
    /// <returns>Binary data of UTF-8 text</returns>
    public new Byte[]? ToArray(Boolean nullTerminated) => base.ToArray(nullTerminated);

    /// <summary>
    /// Indicates whether <paramref name="value"/> is <see langword="null"/>.
    /// </summary>
    /// <param name="value">Value to check.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is <see langword="null"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    private static Boolean IsNullByte([NotNullWhen(false)] Byte? value) => !value.HasValue;
}
