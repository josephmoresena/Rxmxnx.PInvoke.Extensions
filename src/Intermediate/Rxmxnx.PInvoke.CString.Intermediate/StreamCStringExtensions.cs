namespace Rxmxnx.PInvoke;

/// <summary>
/// <see cref="CString"/> extensions for <see cref="Stream"/> class.
/// </summary>
public static class StreamCStringExtensions
{
    /// <summary>
    /// Writes the sequence of bytes in <paramref name="cstr"/> to the current stream and advances the current position within this
    /// stream by the number of bytes written.
    /// </summary>
    /// <param name="stream">Current <see cref="Stream"/> instance.</param>
    /// <param name="cstr">A <see cref="CString"/> instance.</param>
    /// <param name="writeNullTermination">
    /// Indicates whether the UTF-8 text must be written with a null-termination character.
    /// </param>
    public static void Write(this Stream stream, CString cstr, Boolean writeNullTermination = false)
        => cstr.Write(stream, writeNullTermination);

    /// <summary>
    /// Writes the sequence of bytes in <paramref name="cstr"/> to the current stream and advances the current position within this
    /// stream by the number of bytes written.
    /// </summary>
    /// <param name="stream">Current <see cref="Stream"/> instance.</param>
    /// <param name="cstr">A <see cref="CString"/> instance.</param>
    /// <param name="startIndex">
    /// The zero-based offset in <paramref name="cstr"/> at which to begin copying bytes to current stream.
    /// </param>
    /// <param name="count">The number of bytes to be wirtten to the current stream.</param>
    public static void Write(this Stream stream, CString cstr, Int32 startIndex, Int32 count)
        => cstr.Write(stream, startIndex, count);

    /// <summary>
    /// Asynchronously writes a sequence of bytes to the current stream and advances the current position within this stream by
    /// the number of bytes written.
    /// </summary>
    /// <param name="stream">Current <see cref="Stream"/> instance.</param>
    /// <param name="cstr">A <see cref="CString"/> instance.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static Task WriteAsync(this Stream stream, CString cstr, CancellationToken cancellationToken = default)
        => cstr.WriteAsync(stream, false, cancellationToken);

    /// <summary>
    /// Asynchronously writes a sequence of bytes to the current stream and advances the current position within this stream by
    /// the number of bytes written.
    /// </summary>
    /// <param name="stream">Current <see cref="Stream"/> instance.</param>
    /// <param name="cstr">A <see cref="CString"/> instance.</param>
    /// <param name="writeNullTermination">
    /// Indicates whether the UTF-8 text must be written with a null-termination character.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static Task WriteAsync(this Stream stream, CString cstr, Boolean writeNullTermination, CancellationToken cancellationToken = default)
        => cstr.WriteAsync(stream, writeNullTermination, cancellationToken);

    /// <summary>
    /// Asynchronously writes a sequence of bytes to the current stream and advances the current position within this stream by
    /// the number of bytes written.
    /// </summary>
    /// <param name="stream">Current <see cref="Stream"/> instance.</param>
    /// <param name="cstr">A <see cref="CString"/> instance.</param>
    /// <param name="startIndex">
    /// The zero-based offset in <paramref name="cstr"/> at which to begin copying bytes to current stream.
    /// </param>
    /// <param name="count">The number of bytes to be wirtten to the current stream.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static Task WriteAsync(this Stream stream, CString cstr, Int32 startIndex, Int32 count, CancellationToken cancellationToken = default)
        => cstr.WriteAsync(stream, startIndex, count, cancellationToken);
}
