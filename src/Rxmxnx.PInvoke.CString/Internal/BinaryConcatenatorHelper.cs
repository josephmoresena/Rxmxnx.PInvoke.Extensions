namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for <see cref="CString"/> concatenation.
/// </summary>
internal sealed class BinaryConcatenatorHelper : Utf8ConcatenationHelper<Byte?>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="separator"><see cref="CString"/> separator instance.</param>
    public BinaryConcatenatorHelper(Byte separator) :
        base(separator, IsNullByte)
    {
    }

    /// <inheritdoc/>
    protected override void WriteValue(Byte? value)
        => this.Stream.WriteByte(value!.Value);

    /// <inheritdoc/>
    protected override Task WriteValueAsync(Byte? value)
        => Task.Run(() => this.Write(value!.Value));

    /// <summary>
    /// Creates a <see cref="CString"/> instance from concatenation.
    /// </summary>
    /// <returns>
    /// A <see cref="CString"/> instance that represents the UTF-8 concatenation.
    /// </returns>
    public CString ToCString() => base.ToArray(true) ?? CString.Empty;

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

