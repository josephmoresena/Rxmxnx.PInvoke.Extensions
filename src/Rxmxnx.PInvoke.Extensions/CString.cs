using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Rxmxnx.PInvoke.Extensions.Internal;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Represents text as a sequence of UTF-8 code units.
    /// </summary>
    public sealed class CString : ICloneable
    {
        /// <summary>
        /// Represents the empty UTF-8 byte array. This field is read-only.
        /// </summary>
        private static readonly Byte[] empty = new Byte[] { default };

        /// <summary>
        /// Represents the empty UTF-8 string. This field is read-only.
        /// </summary>
        public static readonly CString Empty = new(empty);

        /// <summary>
        /// Internal object data.
        /// </summary>
        private readonly Object _data;
        /// <summary>
        /// Indicates Indicates whether the UTF-8 text is null-terminated.
        /// </summary>
        private readonly Boolean _isNullTerminated;
        /// <summary>
        /// Number of bytes in the current <see cref="CString"/> object.
        /// </summary>
        private readonly Int32 _length;

        /// <summary>
        /// Internal <see cref="Byte"/> array for internal <see cref="CString"/>.
        /// </summary>
        private Byte[]? InternalData => this._data as Byte[];
        /// <summary>
        /// <see cref="NativeArrayReference{Byte}"/> object for external <see cref="CString"/>.
        /// </summary>
        private NativeArrayReference<Byte> ExternalData => this._data as NativeArrayReference<Byte> ?? NativeArrayReference<Byte>.Empty;

        /// <summary>
        /// Gets the <see cref="Byte"/> object at a specified position in the current <see cref="CString"/>
        /// object.
        /// </summary>
        /// <param name="index">A position in the current UTF-8 text.</param>
        /// <returns>The object at position <paramref name="index"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="index"/> is greater than or equal to the length of this object or less than zero.
        /// </exception>
        [IndexerName("Position")]
        public Byte this[Int32 index] => this.InternalData != default ? this.InternalData[index] : this.ExternalData[index];

        /// <summary>
        /// Gets the number of bytes in the current <see cref="CString"/> object.
        /// </summary>
        /// <returns>
        /// The number of characters in the current string.
        /// </returns>
        public Int32 Length => this._length;

        /// <summary>
        /// Indicates whether the ending of text in the current <see cref="CString"/> includes 
        /// a null-termination character.
        /// </summary>
        public Boolean IsNullTerminated => this._isNullTerminated;

        /// <summary>
        /// Private constructor.
        /// </summary>
        /// <param name="bytes">Binary internal information.</param>
        private CString([DisallowNull] Byte[] bytes)
        {
            this._data = bytes;
            this._isNullTerminated = bytes.Any() && bytes[^1] == default;
            this._length = bytes.Length - (this._isNullTerminated ? 1 : 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified 
        /// UTF-8 character repeated a specified number of times.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="count"></param>
        public CString(Byte c, Int32 count) : this(Enumerable.Repeat(c, count).Concat(empty).ToArray()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified 
        /// pointer to an array of UTF-8 characters and a length.
        /// </summary>
        /// <param name="ptr">A pointer to a array of UTF-8 characters.</param>
        /// <param name="length">The number of <see cref="Byte"/> within value to use.</param>
        public CString(IntPtr ptr, Int32 length)
        {
            NativeArrayReference<Byte> data = new(ptr, length);
            if (data.Length == 0)
            {
                this._data = NativeArrayReference<Byte>.Empty;
                this._isNullTerminated = false;
                this._length = 0;
            }
            else
            {
                this._data = data;
                this._isNullTerminated = data[^1] == default;
                this._length = length - (this._isNullTerminated ? 1 : 0);
            }
        }

        /// <summary>
        /// Retrieves an object that can iterate through the individual <see cref="Byte"/> in this 
        /// <see cref="CString"/>.
        /// </summary>
        /// <returns>An enumerator object.</returns>
        public ReadOnlySpan<Byte>.Enumerator GetEnumerator() => this.AsSpan().GetEnumerator();

        /// <summary>
        /// Returns a reference to this instance of <see cref="CString"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Object Clone() => new CString(this.AsSpan().ToArray());

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current instance.
        /// </summary>
        /// <returns>A string that represents the current UTF-8 text.</returns>
        public override String? ToString()
        {
            if (this.Length == 0)
                return String.Empty;
            else
                return Encoding.UTF8.GetString(this.AsSpan()[0..this.Length]);
        }

        /// <summary>
        /// Retreives the internal or external information as <see cref="ReadOnlySpan{Byte}"/> object.
        /// </summary>
        /// <returns><see cref="ReadOnlySpan{Byte}"/> object.</returns>
        public ReadOnlySpan<Byte> AsSpan() => this.InternalData != default ? this.InternalData : this.ExternalData;

        /// <summary>
        /// Indicates whether the specified <see cref="CString"/> is <see langword="null"/> or an 
        /// empty UTF-8 text.
        /// </summary>
        /// <param name="value">The <see cref="CString"/> to test.</param>
        /// <returns>
        /// <see langword="true"/> if the value parameter is <see langword="null"/> or an empty UTF-8 text; 
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static Boolean IsNullOrEmpty([NotNullWhen(false)] CString? value)
            => value == default || value.Length == 0;

        /// <summary>
        /// Defines an implicit conversion of a given <see cref="Byte"/> array to <see cref="CString"/>.
        /// </summary>
        /// <param name="bytes">A <see cref="Byte"/> array to implicitly convert.</param>
        public static implicit operator CString?(Byte[]? bytes) => bytes != default ? new(bytes) : default;
        /// <summary>
        /// Defines an implicit conversion of a given <see cref="String"/> to <see cref="CString"/>.
        /// </summary>
        /// <param name="str">A <see cref="String"/> to implicitly convert.</param>
        public static implicit operator CString?(String? str)
            => str != default ? new(GetUtf8Bytes(str).Concat<byte>(global::Rxmxnx.PInvoke.Extensions.CString.empty).ToArray<byte>()) : default;

        /// <summary>
        /// Defines an implicit conversion of a given <see cref="CString"/> to a read-only span of bytes.
        /// </summary>
        /// <param name="cString">A <see cref="CString"/> to implicitly convert.</param>
        public static implicit operator ReadOnlySpan<Byte>(CString? cString) => cString != default ? cString.AsSpan() : default;

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
        public async Task WriteAsync(Stream strm, Boolean writeNullTermination)
        {
            await GetWriteTask(strm).ConfigureAwait(false);
            if (writeNullTermination)
                await strm.WriteAsync(empty);
        }

        /// <summary>
        /// Concatenates the members of a collection of <see cref="CString"/>.
        /// </summary>
        /// <param name="values">A collection that contains the strings to concatenate.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static CString? Concat(IEnumerable<CString> values)
            => Utf8CStringConcatenation.Concat(values);

        /// <summary>
        /// Concatenates the members of a collection of <see cref="String"/>.
        /// </summary>
        /// <param name="values">A collection that contains the strings to concatenate.</param>
        /// <returns>
        /// A task that represents the asynchronous concat operation. The value of the TResult
        /// parameter contains the concatenation with UTF-8 encoding.
        /// </returns>
        public static async Task<CString?> ConcatAsync(IEnumerable<CString> values)
            => await Utf8CStringConcatenation.ConcatAsync(values);

        /// <summary>
        /// Retrieves the Task for writing the <see cref="CString"/> content into the 
        /// given <see cref="Stream"/>.
        /// </summary>
        /// <param name="strm">
        /// The <see cref="Stream"/> to which the contents of the current <see cref="CString"/> 
        /// will be copied.
        /// </param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        private Task GetWriteTask(Stream strm)
            => this.InternalData != default ?
            strm.WriteAsync(this.InternalData, 0, this.Length)
            : Task.Run(() => { strm.Write(this.ExternalData.Range(0, this.Length)); });

        /// <summary>
        /// Encodes the UTF-16 text using the UTF-8 charset and retrieves the <see cref="Byte"/> array with 
        /// UTF-8 text.
        /// </summary>
        /// <param name="str"><see cref="String"/> representation of UTF-16 text.</param>
        /// <returns><see cref="Byte"/> array with UTF-8 text.</returns>
        private static Byte[] GetUtf8Bytes(String str) => str.AsUtf8() ?? Array.Empty<Byte>();
    }
}
