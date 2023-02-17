using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Represents a sequence of null-terminated UTF-8 texts.
    /// </summary>
    public sealed class CStringSequence : ICloneable, IEquatable<CStringSequence>
    {
        /// <summary>
        /// Size of <see cref="Char"/> value.
        /// </summary>
        internal const Int32 sizeOfChar = sizeof(Char);

        /// <summary>
        /// Internal buffer.
        /// </summary>
        private readonly String _value;
        /// <summary>
        /// Collection of text length for buffer interpretation.
        /// </summary>
        private readonly Int32[] _lengths;

        /// <summary>
        /// Gets the number of <see cref="CString"/> contained in <see cref="CStringSequence"/>.
        /// </summary>
        public Int32 Count => this._lengths.Length;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="values">Text collection.</param>
        public CStringSequence(params CString?[] values)
        {
            this._lengths = values.Select(c => c?.Length ?? default).ToArray();
            this._value = CreateBuffer(this._lengths, values);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sequence"><see cref="CStringSequence"/> instance.</param>
        private CStringSequence(CStringSequence sequence)
        {
            this._lengths = sequence._lengths.ToArray();
            this._value = String.Create(sequence._value.Length, sequence, CopySequence);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">Internal buffer.</param>
        /// <param name="lengths">Collection of text length for buffer interpretation.</param>
        private CStringSequence(String value, Int32[] lengths)
        {
            this._value = value;
            this._lengths = lengths;
        }

        /// <summary>
        /// Retrieves the buffer as an <see cref="ReadOnlySpan{Char}"/> instance and creates a
        /// <see cref="CString"/> array which represents text sequence.
        /// </summary>
        /// <remarks>
        /// <paramref name="output"/> will remain valid only as long as returned 
        /// <see cref="ReadOnlySpan{Char}"/> is on live.
        /// </remarks>
        /// <param name="output"><see cref="CString"/> array.</param>
        /// <returns>Buffer <see cref="ReadOnlySpan{Char}"/>.</returns>
        public ReadOnlySpan<Char> AsSpan(out CString[] output) => this.InternalAsSpan(out output);

        /// <summary>
        /// Use current instance as <see cref="ReadOnlySpan{CString}"/> instance and <paramref name="state"/>
        /// as parameters for <paramref name="action"/> delegate.
        /// </summary>
        /// <typeparam name="TState">The type of the element to pass to <paramref name="action"/>.</typeparam>
        /// <param name="state">The element to pass to <paramref name="action"/>.</param>
        /// <param name="action">A callback to invoke.</param>
        public void Transform<TState>(TState state, CStringSequenceAction<TState> action)
        {
            unsafe
            {
                fixed (Char* ptr = this._value)
                {
                    _ = this.InternalAsSpan(out CString[] output);
                    action(output, state);
                }
            }
        }

        /// <summary>
        /// Use current instance as <see cref="ReadOnlySpan{CString}"/> instance and <paramref name="state"/>
        /// as parameters for <paramref name="func"/> delegate.
        /// </summary>
        /// <typeparam name="TState">The type of the element to pass to <paramref name="func"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="state">The element to pass to <paramref name="func"/>.</param>
        /// <param name="func">A callback to invoke.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public TResult Transform<TState, TResult>(TState state, CStringSequenceFunc<TState, TResult> func)
        {
            unsafe
            {
                fixed (Char* ptr = this._value)
                {
                    _ = this.InternalAsSpan(out CString[] output);
                    return func(output, state);
                }
            }
        }

        /// <summary>
        /// Returns a reference to this instance of <see cref="CStringSequence"/>.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Object Clone() => new CStringSequence(this);

        /// <summary>
        /// Returns a <see cref="CString"/> that represents the current object.
        /// </summary>
        /// <returns>A <see cref="CString"/> that represents the current object.</returns>
        public CString ToCString()
        {
            Int32 bytesLength = this._lengths.Where(x => x > 0).Sum(x => x + 1);
            Byte[] result = new Byte[bytesLength];
            this.Transform(result, BinaryCopyTo);
            return result;
        }

        /// <summary>
        /// Indicates whether the current <see cref="CStringSequence"/> is equal to another <see cref="CStringSequence"/> 
        /// instance.
        /// </summary>
        /// <param name="other">A <see cref="CStringSequence"/> to compare with this object.</param>
        /// <returns>
        /// <see langword="true"/> if the current <see cref="CStringSequence"/> is equal to the <paramref name="other"/> 
        /// parameter; otherwise, <see langword="false"/>.
        /// </returns>
        public Boolean Equals(CStringSequence? other)
            => other != default && this._value.Equals(other._value) &&
            this._lengths.SequenceEqual(other._lengths);

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// <see langword="true"/> if the specified object is equal to the current object; 
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public override Boolean Equals(Object? obj) => obj is CStringSequence cstr && this.Equals(cstr);

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current object.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current object.</returns>
        public override String ToString() => this._value;

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override Int32 GetHashCode() => this._value.GetHashCode();

        /// <summary>
        /// Creates a new UTF-8 text sequence with a specific <paramref name="lengths"/> and initializes each
        /// UTF-8 texts into it after creation by using the specified callback.
        /// </summary>
        /// <typeparam name="TState">The type of the element to pass to <paramref name="action"/>.</typeparam>
        /// <param name="lengths">The lengths of the UTF-8 text sequence to create.</param>
        /// <param name="state">The element to pass to <paramref name="action"/>.</param>
        /// <param name="action">A callback to initialize each <see cref="CString"/>.</param>
        /// <returns>The create UTF-8 text sequence.</returns>
        public static CStringSequence Create<TState>(TState state, CStringSequenceCreationAction<TState> action, params Int32[] lengths)
        {
            Int32 bytesLength = lengths.Where(x => x > 0).Sum(x => x + 1);
            Int32 length = bytesLength / sizeOfChar + (bytesLength % sizeOfChar);
            String buffer = String.Create(length, state, (span, state) =>
            {
                unsafe
                {
                    fixed (void* ptr = &MemoryMarshal.GetReference(span))
                        CreateCStringSequence(new(ptr), lengths, state, action);
                }
            });
            return new(buffer, lengths);
        }


        /// <summary>
        /// Retrieves the buffer as an <see cref="ReadOnlySpan{Char}"/> instance and creates a
        /// <see cref="CString"/> array which represents text sequence.
        /// </summary>
        /// <param name="output"><see cref="CString"/> array.</param>
        /// <returns>Buffer <see cref="ReadOnlySpan{Char}"/>.</returns>
        private ReadOnlySpan<Char> InternalAsSpan(out CString[] output)
        {
            ReadOnlySpan<Char> result = this._value;
            output = this.GetValues(result.AsIntPtr()).ToArray();
            return this._value;
        }

        /// <summary>
        /// Retrieves the sequence of <see cref="CString"/> based on the buffer and lengths.
        /// </summary>
        /// <param name="ptr">Buffer pointer.</param>
        /// <returns>Collection of <see cref="CString"/>.</returns>
        private IEnumerable<CString> GetValues(IntPtr ptr)
        {
            Int32 offset = 0;
            for (Int32 i = 0; i < this._lengths.Length; i++)
            {
                if (this._lengths[i] > 0)
                {
                    yield return new(ptr + offset, this._lengths[i] + 1);
                    offset += this._lengths[i] + 1;
                }
                else
                    yield return CString.Empty;
            }
        }

        /// <summary>
        /// Creates the sequence buffer.
        /// </summary>
        /// <param name="lengths">Text length collection.</param>
        /// <param name="values">Text collection.</param>
        /// <returns>
        /// <see cref="String"/> instance that contains the binary information of the UTF-8 text sequence.
        /// </returns>
        private static String CreateBuffer(Int32[] lengths, CString?[] values)
        {
            Int32 totalBytes = 0;
            for (Int32 i = 0; i < lengths.Length; i++)
                if (values[i] is CString value && value.Length > 0)
                    totalBytes += value.Length + 1;
            Int32 totalChars = (totalBytes / sizeOfChar) + (totalBytes % sizeOfChar);
            return String.Create(totalChars, values, CopyText);
        }

        /// <summary>
        /// Copy the content of <see cref="CString"/> items in <paramref name="values"/> to
        /// <paramref name="charSpan"/> span.
        /// </summary>
        /// <param name="charSpan">A writable <see cref="Char"/> span.</param>
        /// <param name="values">A enumeration of <see cref="CString"/> items.</param>
        private static void CopyText(Span<Char> charSpan, CString?[] values)
        {
            Int32 position = 0;
            unsafe
            {
                fixed (void* charsPtr = &MemoryMarshal.GetReference(charSpan))
                {
                    Span<Byte> byteSpan = new(charsPtr, charSpan.Length * sizeOfChar);
                    for (Int32 i = 0; i < values.Length; i++)
                        if (values[i] is CString value && value.Length > 0)
                        {
                            ReadOnlySpan<Byte> valueSpan = value.AsSpan()[..value.Length];
                            valueSpan.CopyTo(byteSpan[position..]);
                            position += valueSpan.Length;
                            byteSpan[position] = default;
                            position++;
                        }
                }
            }
        }

        /// <summary>
        /// Copy the content of <paramref name="sequence"/> to <paramref name="charSpan"/>.
        /// </summary>
        /// <param name="charSpan">A writable <see cref="Char"/> span.</param>
        /// <param name="sequence">A <see cref="CStringSequence"/> instance.</param>
        private static void CopySequence(Span<Char> charSpan, CStringSequence sequence)
        {
            ReadOnlySpan<Char> chars = sequence._value;
            chars.CopyTo(charSpan);
        }

        /// <summary>
        /// Preforms a binary copy of all non-empty <paramref name="span"/> to 
        /// <paramref name="destination"/> span.
        /// </summary>
        /// <param name="span">A read-only <see cref="CString"/> span instance.</param>
        /// <param name="destination">The destination binary buffer.</param>
        private static void BinaryCopyTo(ReadOnlySpan<CString> span, Byte[] destination)
        {
            Int32 offset = 0;
            foreach (CString value in span)
                if (value.Length > 0)
                {
                    ReadOnlySpan<Byte> bytes = value.AsSpan();
                    bytes.CopyTo(destination.AsSpan().Slice(offset, bytes.Length));
                    offset += bytes.Length;
                }
        }

        /// <summary>
        /// Performs the creation of the UTF-8 text sequence with a specific <paramref name="lengths"/> and 
        /// whose buffer is referenced by <paramref name="bufferPtr"/>. 
        /// Each UTF-8 text is initialized using the specified callback.
        /// </summary>
        /// <typeparam name="TState">The type of the element to pass to <paramref name="action"/>.</typeparam>
        /// <param name="bufferPtr">Pointer to internal <see cref="CStringSequence"/> buffer.</param>
        /// <param name="lengths">The lengths of the UTF-8 text sequence to create.</param>
        /// <param name="state">The element to pass to <paramref name="action"/>.</param>
        /// <param name="action">A callback to initialize each <see cref="CString"/>.</param>
        private static unsafe void CreateCStringSequence<TState>(IntPtr bufferPtr, Int32[] lengths, TState state, CStringSequenceCreationAction<TState> action)
        {
            Int32 offset = 0;
            for (Int32 i = 0; i < lengths.Length; i++)
                if (lengths[i] > 0)
                {
                    IntPtr currentPtr = bufferPtr + offset;
                    Span<Byte> bytes = new(currentPtr.ToPointer(), lengths[i]);
                    action(bytes, i, state);
                    offset += lengths[i] + 1;
                }
        }
    }
}
