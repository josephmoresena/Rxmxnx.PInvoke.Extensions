﻿namespace Rxmxnx.PInvoke;

public partial class CString
{
    /// <summary>
    /// Writes the sequence of bytes to the given <see cref="Stream"/> and advances
    /// the current position within this stream by the number of bytes written.
    /// </summary>
    /// <param name="strm">
    /// The <see cref="Stream"/> to which the contents of the current <see cref="CString"/> 
    /// will be copied.
    /// </param>
    /// <param name="writeNullTermination">
    /// Indicates whether the UTF-8 text must be written with a null-termination character 
    /// into the <see cref="Stream"/>.
    /// </param>
    internal void Write(Stream strm, Boolean writeNullTermination)
    {
        strm.Write(this.AsSpan());
        if (writeNullTermination)
            strm.Write(empty);
    }

    /// <summary>
    /// Writes the sequence of bytes to the given <see cref="Stream"/>, starting with the
    /// byte located at the <paramref name="startIndex"/> position, and concatenating up
    /// to <paramref name="count"/> bytes.
    /// </summary>
    /// <param name="strm">
    /// The <see cref="Stream"/> to which the contents of the current <see cref="CString"/> 
    /// will be copied.
    /// </param>
    /// <param name="startIndex">The first byte in current instance to write to.</param>
    /// <param name="count">The number of bytes of current instance to write to.</param>
    internal void Write(Stream strm, Int32 startIndex, Int32 count)
        => strm.Write(this.AsSpan().Slice(startIndex, count));

    /// <summary>
    /// Asynchronously writes the sequence of bytes to the given <see cref="Stream"/> and advances
    /// the current position within this stream by the number of bytes written.
    /// </summary>
    /// <param name="strm">
    /// The <see cref="Stream"/> to which the contents of the current <see cref="CString"/> 
    /// will be copied.
    /// </param>
    /// <param name="writeNullTermination">
    /// Indicates whether the UTF-8 text must be written with a null-termination character 
    /// into the <see cref="Stream"/>.
    /// </param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    internal async Task WriteAsync(Stream strm, Boolean writeNullTermination)
    {
        await this.GetWriteTask(strm, 0, this._length).ConfigureAwait(false);
        if (writeNullTermination)
            await strm.WriteAsync(empty);
    }

    /// <summary>
    /// Asynchronously writes the sequence of bytes to the given <see cref="Stream"/>,
    /// starting with the byte located at the <paramref name="startIndex"/> position, and
    /// concatenating up to <paramref name="count"/> bytes.
    /// </summary>
    /// <param name="strm">
    /// The <see cref="Stream"/> to which the contents of the current <see cref="CString"/> 
    /// will be copied.
    /// </param>
    /// <param name="startIndex">The first byte in current instance to write to.</param>
    /// <param name="count">The number of bytes of current instance to write to.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    internal async Task WriteAsync(Stream strm, Int32 startIndex, Int32 count)
        => await this.GetWriteTask(strm, startIndex, count).ConfigureAwait(false);
}