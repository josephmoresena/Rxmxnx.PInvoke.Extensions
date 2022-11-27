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
    public sealed class CString : ICloneable, IEquatable<CString>
    {
        /// <summary>
        /// Delegate. Indicates whether <paramref name="current"/> <see cref="CString"/> is equal to 
        /// <paramref name="other"/> <see cref="CString"/> 
        /// </summary>
        /// <param name="current">A <see cref="CString"/> to compare with <paramref name="other"/>.</param>
        /// <param name="other">A <see cref="CString"/> to compare with this <paramref name="current"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="current"/> <see cref="CString"/> is equal to 
        /// <paramref name="other"/> parameter; otherwise, <see langword="false"/>.
        /// </returns>
        private delegate Boolean EqualsDelegate(CString current, CString other);

        /// <summary>
        /// Represents the empty UTF-8 byte array. This field is read-only.
        /// </summary>
        private static readonly Byte[] empty = new Byte[] { default };
        /// <summary>
        /// <see cref="EqualsDelegate"/> delegate for native comparision.
        /// </summary>
        private static readonly EqualsDelegate equals = GetEquals();

        /// <summary>
        /// Represents the empty UTF-8 string. This field is read-only.
        /// </summary>
        public static readonly CString Empty = new(empty);

        /// <summary>
        /// Indicates whether the UTF-8 data is local.
        /// </summary>
        private readonly Boolean _isLocal;
        /// <summary>
        /// Indicates whether the UTF-8 data is a function.
        /// </summary>
        private readonly Boolean _isFunction;
        /// <summary>
        /// Internal object data.
        /// </summary>
        private readonly ValueRegion<Byte> _data;
        /// <summary>
        /// Indicates whether the UTF-8 text is null-terminated.
        /// </summary>
        private readonly Boolean _isNullTerminated;
        /// <summary>
        /// Number of bytes in the current <see cref="CString"/> object.
        /// </summary>
        private readonly Int32 _length;

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
        public Byte this[Int32 index] => this._data[index];
        /// <summary>
        /// Gets a <see cref="CString"/> instance at specified range from current <see cref="CString"/> instace.
        /// </summary>
        /// <param name="range"></param>
        /// <returns><see cref="CString"/> range.</returns>
        [IndexerName("Position")]
        public CString this[Range range]
        {
            get
            {
                (Int32 offset, Int32 length) = range.GetOffsetAndLength(this._length);
                Boolean inRange = offset < this._length && length <= this._length;
                if (inRange && this._length > 0 && length == 0)
                    return CString.Empty;
                else if (!inRange)
                    throw new ArgumentOutOfRangeException(nameof(range));
                return new(this, offset, length);
            }
        }

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
        public Boolean IsReference => !this._isLocal && !this._isFunction;

        /// <summary>
        /// Indicates whether the current instance is segmented.
        /// </summary>
        public Boolean IsSegmented => this._data.IsSegmented;

        /// <summary>
        /// Indicates whether the current instance is a function.
        /// </summary>
        public Boolean IsFunction => this._isFunction;

        /// <summary>
        /// Private constructor.
        /// </summary>
        /// <param name="bytes">Binary internal information.</param>
        private CString([DisallowNull] Byte[] bytes)
        {
            this._isLocal = true;
            this._data = ValueRegion<Byte>.Create(bytes);
            this._isNullTerminated = bytes.Any() && bytes[^1] == default;
            this._length = bytes.Length - (this._isNullTerminated ? 1 : 0);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="func"><see cref="ReadOnlySpanFunc{Byte}"/> delegate.</param>
        /// <param name="isLiteral">Indicates whether returned span is from UTF-8 literal.</param>
        private CString(ReadOnlySpanFunc<Byte> func, Boolean isLiteral)
        {
            this._isLocal = false;
            this._isFunction = true;
            this._data = ValueRegion<Byte>.Create(func);

            ReadOnlySpan<Byte> data = func();
            Boolean isNullTerminatedSpan = IsNullTerminatedSpan(data);
            this._isNullTerminated = isLiteral || isNullTerminatedSpan;
            this._length = data.Length - (isNullTerminatedSpan ? 1 : 0);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">A <see cref="CString"/> value.</param>
        /// <param name="offset">Offset for range.</param>
        /// <param name="length">Length of range.</param>
        private CString(CString value, Int32 offset, Int32 length)
        {
            this._isLocal = value._isLocal;
            this._isFunction = value._isFunction;
            this._data = ValueRegion<Byte>.Create(value._data, offset, value.GetDataLength(offset, length));

            ReadOnlySpan<Byte> data = this._data;
            Boolean isNullTerminatedSpan = IsNullTerminatedSpan(data);
            Boolean isLiteral = value._isFunction && value._isNullTerminated;
            this._isNullTerminated = isLiteral && value._length - offset == length || isNullTerminatedSpan;
            this._length = data.Length - (isNullTerminatedSpan ? 1 : 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified 
        /// UTF-8 character repeated a specified number of times.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="count"></param>
        public CString(Byte c, Int32 count) : this(Enumerable.Repeat(c, count).Concat(empty).ToArray()) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="func"><see cref="ReadOnlySpanFunc{Byte}"/> delegate.</param>
        public CString(ReadOnlySpanFunc<Byte> func) : this(func, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified 
        /// pointer to an array of UTF-8 characters and a length.
        /// </summary>
        /// <param name="ptr">A pointer to a array of UTF-8 characters.</param>
        /// <param name="length">The number of <see cref="Byte"/> within value to use.</param>
        public CString(IntPtr ptr, Int32 length)
        {
            this._isLocal = false;
            this._data = ValueRegion<Byte>.Create(ptr, length);
            ReadOnlySpan<Byte> data = this._data;
            if (data.IsEmpty)
            {
                this._isNullTerminated = false;
                this._length = 0;
            }
            else
            {
                this._isNullTerminated = data[^1] == default;
                this._length = length - (this._isNullTerminated ? 1 : 0);
            }
        }

        /// <summary>
        /// Copies the UTF-8 text into a new array.
        /// </summary>
        /// <returns>An array containing the data in the current UTF-8 text.</returns>
        public Byte[] ToArray() => this._data.ToArray();

        /// <summary>
        /// Retreives the internal or external information as <see cref="ReadOnlySpan{Byte}"/> object.
        /// </summary>
        /// <returns><see cref="ReadOnlySpan{Byte}"/> instance.</returns>
        public ReadOnlySpan<Byte> AsSpan() => this._data;

        /// <summary>
        /// Gets <see cref="String"/> representation of the current UTF-8 text as hexadecimal value.
        /// </summary>
        /// <returns><see cref="String"/> representation of hexadecimal value.</returns>
        public String AsHexString()
        {
            StringBuilder strBuilder = new();
            for (Int32 i = 0; i < this._length; i++)
                strBuilder.Append(this[i].AsHexString());
            return strBuilder.ToString();
        }

        /// <summary>
        /// Prevents the garbage collector from relocating the current instance and fixes its memory 
        /// address until <paramref name="action"/> finish.
        /// </summary>
        /// <param name="action">A <see cref="FixedAction{T}"/> delegate.></param>
        public void WithSafeFixed(ReadOnlyFixedAction<Byte> action)
            => this.AsSpan().WithSafeFixed(action);

        /// <summary>
        /// Prevents the garbage collector from relocating the current instance and fixes its memory 
        /// address until <paramref name="action"/> finish.
        /// </summary>
        /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="action">A <see cref="ReadOnlyFixedAction{Byte, TArg}"/> delegate.</param>
        public void WithSafeFixed<TArg>(TArg arg, ReadOnlyFixedAction<Byte, TArg> action)
            => this.AsSpan().WithSafeFixed(arg, action);

        /// <summary>
        /// Prevents the garbage collector from relocating the current instance and fixes its memory 
        /// address until <paramref name="func"/> finish.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="func">A <see cref="FixedFunc{T, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public TResult WithSafeFixed<TResult>(ReadOnlyFixedFunc<Byte, TResult> func)
            => this.AsSpan().WithSafeFixed(func);

        /// <summary>
        /// Prevents the garbage collector from relocating the current instance and fixes its memory 
        /// address until <paramref name="func"/> finish.
        /// </summary>
        /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="func">A <see cref="FixedFunc{T, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public TResult WithSafeFixed<TArg, TResult>(TArg arg, ReadOnlyFixedFunc<Byte, TArg, TResult> func)
            => this.AsSpan().WithSafeFixed(arg, func);

        /// <summary>
        /// Retrieves an object that can iterate through the individual <see cref="Byte"/> in this 
        /// <see cref="CString"/>.
        /// </summary>
        /// <returns>An enumerator object.</returns>
        public ReadOnlySpan<Byte>.Enumerator GetEnumerator() => this._data.GetEnumerator();

        /// <summary>
        /// Returns a reference to this instance of <see cref="CString"/>.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public Object Clone()
        {
            Byte[] bytes;
            if (this._isNullTerminated && !this._isFunction)
                bytes = this._data.ToArray();
            else
            {
                ReadOnlySpan<Byte> data = this._data;
                if (!this.IsFunction || data.IsEmpty)
                    bytes = new Byte[this._length + 1];
                else
                    bytes = new Byte[data.Length + (!IsNullTerminatedSpan(data) ? 1 : 0)];
                data.CopyTo(bytes);
            }
            return new CString(bytes);
        }

        /// <summary>
        /// Indicates whether the current <see cref="CString"/> is equal to another <see cref="CString"/> 
        /// instance.
        /// </summary>
        /// <param name="other">A <see cref="CString"/> to compare with this object.</param>
        /// <returns>
        /// <see langword="true"/> if the current <see cref="CString"/> is equal to the <paramref name="other"/> 
        /// parameter; otherwise, <see langword="false"/>.
        /// </returns>
        public Boolean Equals(CString? other)
            => other is CString otherNotNull && this._length == otherNotNull._length && equals(this, other);

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// <see langword="true"/> if the specified object is equal to the current object; 
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public override Boolean Equals(Object? obj) => obj is CString cstr && Equals(cstr);

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current instance.
        /// </summary>
        /// <returns>A string that represents the current UTF-8 text.</returns>
        public override String ToString()
        {
            if (this._length == 0)
                return String.Empty;
            else
                return Encoding.UTF8.GetString(this.AsSpan()[0..this._length]);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override Int32 GetHashCode() => this.GetHasCodeLengthNullMatch() ?? new CStringSequence(this).GetHashCode();

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
        public void Write([DisallowNull] Stream strm, Boolean writeNullTermination)
        {
            strm.Write(this.AsSpan()[..this.Length]);
            if (writeNullTermination)
                strm.Write(empty);
        }

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
        public async Task WriteAsync([DisallowNull] Stream strm, Boolean writeNullTermination)
        {
            await GetWriteTask(strm).ConfigureAwait(false);
            if (writeNullTermination)
                await strm.WriteAsync(empty);
        }

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
            => value is not CString valueNotNull || valueNotNull._length == 0;

        /// <summary>
        /// Defines an implicit conversion of a given <see cref="Byte"/> array to <see cref="CString"/>.
        /// </summary>
        /// <param name="bytes">A <see cref="Byte"/> array to implicitly convert.</param>
        [return: NotNullIfNotNull("bytes")]
        public static implicit operator CString?(Byte[]? bytes) => bytes is not null ? new(bytes) : default;

        /// <summary>
        /// Defines an implicit conversion of a given <see cref="String"/> to <see cref="CString"/>.
        /// </summary>
        /// <param name="str">A <see cref="String"/> to implicitly convert.</param>
        [return: NotNullIfNotNull("str")]
        public static implicit operator CString?(String? str)
            => str is not null ? new(GetUtf8Bytes(str).Concat<Byte>(empty).ToArray<Byte>()) : default;

        /// <summary>
        /// Defines an implicit conversion of a given <see cref="CString"/> to a read-only span of bytes.
        /// </summary>
        /// <param name="value">A <see cref="CString"/> to implicitly convert.</param>
        public static implicit operator ReadOnlySpan<Byte>(CString? value) => value != default ? value.AsSpan() : default;

        /// <summary>
        /// Determines whether two specified <see cref="CString"/> have the same value.
        /// </summary>
        /// <param name="a">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
        /// <param name="b">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="a"/> is the same as the value 
        /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static Boolean operator ==(CString? a, CString? b) => a?.Equals(b) ?? b?.Equals(a) ?? true;

        /// <summary>
        /// Determines whether two specified <see cref="CString"/> have different values.
        /// </summary>
        /// <param name="a">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
        /// <param name="b">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="a"/> is different from the value  
        /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static Boolean operator !=(CString? a, CString? b) => !(a == b);

        /// <summary>
        /// Retrieves the internal binary data from a given <see cref="CString"/>.
        /// </summary>
        /// <param name="value">A non-reference <see cref="CString"/> instance.</param>
        /// <returns>A <see cref="Byte"/> array with UTF-8 text.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="value"/> does not contains the UTF-8 text.</exception>
        public static Byte[] GetBytes([DisallowNull] CString value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if ((Byte[]?)value._data is Byte[] array)
                return array;
            else
                throw new InvalidOperationException(nameof(value) + " does not contains the UTF-8 text.");
        }

        /// <summary>
        /// Creates a new <see cref="CString"/> instance from <paramref name="func"/>.
        /// </summary>
        /// <param name="func">A <see cref="ReadOnlySpanFunc{Byte}"/> delegate that returns a Utf8 string non-literal.</param>
        /// <returns>A <see cref="CString"/> instance from <paramref name="func"/>.</returns>
        public static CString? Create(ReadOnlySpanFunc<Byte>? func) => func is not null ? new(func, false) : default;

        /// <summary>
        /// Calculates the data length of a segment of current <see cref="CString"/> whose 
        /// offset is <paramref name="offset"/> and whose length is <paramref name="length"/>.
        /// </summary>
        /// <param name="offset">Offset for segment.</param>
        /// <param name="length">Initial length for segment.</param>
        /// <returns>Final length for segment.</returns>
        private Int32 GetDataLength(Int32 offset, Int32 length)
        {
            ReadOnlySpan<Byte> bytes = this._data;
            bytes = bytes[offset..];
            if (bytes.Length > length && bytes[length] == default)
                length++;
            return length;
        }

        /// <summary>
        /// Hash calculation for instances whose length and termination allow it to be treated 
        /// as a sequence of UTF-16 units.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        private Int32? GetHasCodeLengthNullMatch()
        {
            if ((this._length + 1) % CStringSequence.sizeOfChar == 0 && this.IsNullTerminated)
            {
                ReadOnlySpan<Byte> thisSpan = this;
                IntPtr thisPtr = thisSpan.AsIntPtr();
                Int32 charCount = (this._length + 1) / CStringSequence.sizeOfChar;
                ReadOnlySpan<Char> thisCharSpan = thisPtr.AsReadOnlySpan<Char>(charCount);
                return String.GetHashCode(thisCharSpan);
            }
            return default;
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
            => (Byte[]?)this._data is Byte[] array ? strm.WriteAsync(array, 0, this._length) :
            Task.Run(() => { strm.Write(this.AsSpan()[0..this._length]); });

        /// <summary>
        /// Retrieves the equality parameters for current <see cref="CString"/> instance.
        /// </summary>
        /// <typeparam name="TInteger">Type used for integer comparision.</typeparam>
        /// <param name="offset">Output. Calculated offset.</param>
        /// <param name="count">Output. Calculated count.</param>
        private void GetEqualityParameters<TInteger>(out Int32 offset, out Int32 count) where TInteger : unmanaged
        {
            unsafe
            {
                Int32 sizeofT = sizeof(TInteger);
                offset = this._length % sizeofT;
                count = (this._length - offset) / sizeofT;
            }
        }

        /// <summary>
        /// Retrives a <see cref="EqualsDelegate"/> delegate for native comparision.
        /// </summary>
        /// <returns><see cref="EqualsDelegate"/> delegate.</returns>
        [ExcludeFromCodeCoverage]
        private static EqualsDelegate GetEquals() => Environment.Is64BitProcess ? Equals<Int64> : Equals<Int32>;

        /// <summary>
        /// Encodes the UTF-16 text using the UTF-8 charset and retrieves the <see cref="Byte"/> array with 
        /// UTF-8 text.
        /// </summary>
        /// <param name="str"><see cref="String"/> representation of UTF-16 text.</param>
        /// <returns><see cref="Byte"/> array with UTF-8 text.</returns>
        private static Byte[] GetUtf8Bytes(String str) => str.AsUtf8() ?? Array.Empty<Byte>();

        /// <summary>
        /// Indicates whether <paramref name="current"/> <see cref="CString"/> is equal to 
        /// <paramref name="other"/> <see cref="CString"/> 
        /// instance.
        /// </summary>
        /// <typeparam name="TInteger"><see cref="ValueType"/> for reduce comparation.</typeparam>
        /// <param name="current">A <see cref="CString"/> to compare with <paramref name="other"/>.</param>
        /// <param name="other">A <see cref="CString"/> to compare with this <paramref name="current"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="current"/> <see cref="CString"/> is equal to 
        /// <paramref name="other"/> parameter; otherwise, <see langword="false"/>.
        /// </returns>
        private static Boolean Equals<TInteger>(CString current, CString other)
            where TInteger : unmanaged
        {
            ReadOnlySpan<Byte> thisSpan = current;
            ReadOnlySpan<Byte> otherSpan = other;

            current.GetEqualityParameters<TInteger>(out Int32 offset, out Int32 count);

            for (Int32 i = current._length - offset; i < current._length; i++)
                if (!thisSpan[i].Equals(otherSpan[i]))
                    return false;

            ReadOnlySpan<TInteger> thisTSpan = thisSpan.AsIntPtr().AsReadOnlySpan<TInteger>(count);
            ReadOnlySpan<TInteger> otherTSpan = otherSpan.AsIntPtr().AsReadOnlySpan<TInteger>(count);
            for (Int32 i = 0; i < count; i++)
                if (!thisTSpan[i].Equals(otherTSpan[i]))
                    return false;

            return true;
        }

        /// <summary>
        /// Indicates whether <paramref name="data"/> contains a null-terminated UTF-8 text.
        /// </summary>
        /// <param name="data">A read-only byte span containing UTF-8 text.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="data"/> contains a null-terminated UTF-8 text; 
        /// otherwise, <see langword="false"/>.
        /// </returns>
        private static Boolean IsNullTerminatedSpan(ReadOnlySpan<Byte> data)
            => !data.IsEmpty && data[^1] == default;
    }
}
