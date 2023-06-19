namespace Rxmxnx.PInvoke;

/// <summary>
/// <see cref="CString"/> extensions for <see cref="Stream"/> class.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class StreamCStringExtensions
{
    /// <summary>
    /// Writes the sequence of bytes represented by the <paramref name="cstr"/> to the current <paramref name="stream"/>, 
    /// and advances the current position within the stream by the number of bytes written.
    /// </summary>
    /// <param name="stream">The current <see cref="Stream"/> instance to which bytes will be written.</param>
    /// <param name="cstr">The <see cref="CString"/> instance that represents the sequence of bytes to write.</param>
    /// <param name="writeNullTermination">
    /// A boolean value that indicates whether the UTF-8 text should be written with a null-termination character.
    /// </param>
    public static void Write(this Stream stream, CString cstr, Boolean writeNullTermination = false)
        => cstr.Write(stream, writeNullTermination);
    /// <summary>
    /// Writes a specific number of bytes from the <paramref name="cstr"/> to the current <paramref name="stream"/>, 
    /// starting at the specified index, and advances the current position within the stream by the number of bytes written.
    /// </summary>
    /// <param name="stream">The current <see cref="Stream"/> instance to which bytes will be written.</param>
    /// <param name="cstr">The <see cref="CString"/> instance that represents the sequence of bytes to write.</param>
    /// <param name="startIndex">
    /// The zero-based offset in the <paramref name="cstr"/> at which to begin copying bytes to the current stream.
    /// </param>
    /// <param name="count">The number of bytes to be written to the current stream.</param>
    public static void Write(this Stream stream, CString cstr, Int32 startIndex, Int32 count)
        => cstr.Write(stream, startIndex, count);

    /// <summary>
    /// Asynchronously writes the sequence of bytes represented by the <paramref name="cstr"/> to the current <paramref name="stream"/>, 
    /// and advances the current position within the stream by the number of bytes written.
    /// </summary>
    /// <param name="stream">The current <see cref="Stream"/> instance to which bytes will be written.</param>
    /// <param name="cstr">The <see cref="CString"/> instance that represents the sequence of bytes to write.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static Task WriteAsync(this Stream stream, CString cstr, CancellationToken cancellationToken = default)
        => cstr.WriteAsync(stream, false, cancellationToken);
    /// <summary>
    /// Asynchronously writes the sequence of bytes represented by the <paramref name="cstr"/> to the current <paramref name="stream"/>, 
    /// and advances the current position within the stream by the number of bytes written, including a null-termination character if specified.
    /// </summary>
    /// <param name="stream">The current <see cref="Stream"/> instance to which bytes will be written.</param>
    /// <param name="cstr">The <see cref="CString"/> instance that represents the sequence of bytes to write.</param>
    /// <param name="writeNullTermination">
    /// A boolean value that indicates whether the UTF-8 text should be written with a null-termination character.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static Task WriteAsync(this Stream stream, CString cstr, Boolean writeNullTermination, CancellationToken cancellationToken = default)
        => cstr.WriteAsync(stream, writeNullTermination, cancellationToken);
    /// <summary>
    /// Asynchronously writes a specific number of bytes from the <paramref name="cstr"/> to the current <paramref name="stream"/>, 
    /// starting at the specified index, and advances the current position within the stream by the number of bytes written.
    /// </summary>
    /// <param name="stream">The current <see cref="Stream"/> instance to which bytes will be written.</param>
    /// <param name="cstr">The <see cref="CString"/> instance that represents the sequence of bytes to write.</param>
    /// <param name="startIndex">
    /// The zero-based offset in the <paramref name="cstr"/> at which to begin copying bytes to the current stream.
    /// </param>
    /// <param name="count">The number of bytes to be written to the current stream.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static Task WriteAsync(this Stream stream, CString cstr, Int32 startIndex, Int32 count, CancellationToken cancellationToken = default)
        => cstr.WriteAsync(stream, startIndex, count, cancellationToken);
}
