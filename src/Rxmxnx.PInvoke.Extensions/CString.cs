using System;
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
        /// Indicates whether the UTF-8 data is local.
        /// </summary>
        private readonly Boolean _isLocal;
        /// <summary>
        /// Internal object data.
        /// </summary>
        private readonly Object? _data;
        /// <summary>
        /// Indicates whether the UTF-8 text is null-terminated.
        /// </summary>
        private readonly Boolean _isNullTerminated;
        /// <summary>
        /// Number of bytes in the current <see cref="CString"/> object.
        /// </summary>
        private readonly Int32 _length;

        /// <summary>
        /// Internal <see cref="Byte"/> array for internal <see cref="CString"/>.
        /// </summary>
        private Byte[]? InternalData => this._isLocal ? (Byte[]?)this._data : default;
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
        public Byte this[Int32 index] => this.InternalData?[index] ?? this.ExternalData[index];

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
        /// Indicates whether the UTF-8 text is referenced by the current <see cref="CString"/> and 
        /// not contained by.
        /// </summary>
        public Boolean IsReference => !this._isLocal;

        /// <summary>
        /// Private constructor.
        /// </summary>
        /// <param name="bytes">Binary internal information.</param>
        private CString([DisallowNull] Byte[] bytes)
        {
            this._isLocal = true;
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
            this._isLocal = false;
            NativeArrayReference<Byte> data = new(ptr, length);
            if (data.Length == 0)
            {
                this._data = default;
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
        public override String ToString()
        {
            if (this.Length == 0)
                return String.Empty;
            else
                return Encoding.UTF8.GetString(this.AsSpan()[0..this.Length]);
        }

        /// <summary>
        /// Copies the UTF-8 text into a new array.
        /// </summary>
        /// <returns>An array containing the data in the current UTF-8 text.</returns>
        public Byte[] ToArray() => this.AsSpan().ToArray();

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
        /// <param name="value">A <see cref="CString"/> to implicitly convert.</param>
        public static implicit operator ReadOnlySpan<Byte>(CString? value) => value != default ? value.AsSpan() : default;

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
        public async Task WriteAsync([DisallowNull] Stream strm, Boolean writeNullTermination)
        {
            await GetWriteTask(strm).ConfigureAwait(false);
            if (writeNullTermination)
                await strm.WriteAsync(empty);
        }

        /// <summary>
        /// Retrieves the internal binary data from a given <see cref="CString"/>.
        /// </summary>
        /// <param name="value">A non-reference <see cref="CString"/> instance.</param>
        /// <returns>A <see cref="Byte"/> array with UTF-8 text.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="value"/> does not contains the UTF-8 text.</exception>
        public static Byte[] GetBytes([DisallowNull] CString value)
        {
            if (value == default)
                throw new ArgumentNullException(nameof(value));

            if (!value._isLocal)
                throw new InvalidOperationException(nameof(value) + " does not contains the UTF-8 text.");

#pragma warning disable CS8603
            return value.InternalData;
#pragma warning restore CS8603
        }

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
            => this.InternalData is Byte[] data ?
            strm.WriteAsync(data, 0, this.Length)
            : Task.Run(() => { strm.Write(this.ExternalData.Range(0, this.Length)); });

        /// <summary>
        /// Retreives the internal or external information as <see cref="ReadOnlySpan{Byte}"/> object.
        /// </summary>
        /// <returns><see cref="ReadOnlySpan{Byte}"/> object.</returns>
        private ReadOnlySpan<Byte> AsSpan() => this._isLocal ? this.InternalData : this.ExternalData;

        /// <summary>
        /// Encodes the UTF-16 text using the UTF-8 charset and retrieves the <see cref="Byte"/> array with 
        /// UTF-8 text.
        /// </summary>
        /// <param name="str"><see cref="String"/> representation of UTF-16 text.</param>
        /// <returns><see cref="Byte"/> array with UTF-8 text.</returns>
        private static Byte[] GetUtf8Bytes(String str) => str.AsUtf8() ?? Array.Empty<Byte>();
    }
}
