using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        private const Int32 sizeOfChar = sizeof(Char);

        /// <summary>
        /// Internal buffer.
        /// </summary>
        private readonly String _value;
        /// <summary>
        /// Collection of text length for buffer interpretation.
        /// </summary>
        private readonly Int32[] _lengths;

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
            this._value = new(sequence._value.ToCharArray());
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
        public ReadOnlySpan<Char> AsSpan(out CString[] output)
        {
            ReadOnlySpan<Char> result = this._value;
            output = this.GetValues(result.AsIntPtr()).ToArray();
            return this._value;
        }

        /// <summary>
        /// Returns a reference to this instance of <see cref="CStringSequence"/>.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Object Clone() => new CStringSequence(this);

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
            Int32 count = lengths.Sum() + lengths.Length - 1;
            StringBuilder sb = new(count / sizeOfChar + count % sizeOfChar);
            Boolean first = true;
            Byte previous = default;
            for (Int32 i = 0; i < lengths.Length; i++)
                CopyText(sb, values[i], ref first, ref previous);
            if (previous != default)
            {
                Byte[] lastChar = new Byte[sizeOfChar];
                lastChar[0] = previous;
                lastChar[1] = default;
                sb.Append(lastChar.AsValue<Char>());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Copy the UTF-8 into the buffer builder.
        /// </summary>
        /// <param name="strBuild">Buffer builder.</param>
        /// <param name="cstr">UTF-8 text.</param>
        /// <param name="first">
        /// <list type="table">
        /// <item>
        /// <term>Input</term>
        /// <description>
        /// Indicates whether the current method call should write a pending UTF-8 character from the 
        /// previous method call.
        /// </description>
        /// </item>
        /// <item>
        /// <term>Output</term>
        /// <description>Indicates whether the next call should write a leftover UTF-8 character.</description>
        /// </item>
        /// </list>
        /// </param>
        /// <param name="previous">
        /// <list type="table">
        /// <item>
        /// <term>Input</term>
        /// <description>Pending UTF-8 character from previous method call.</description>
        /// </item>
        /// <item>
        /// <term>Output</term>
        /// <description>Pending UTF-8 character to next method call.</description>
        /// </item>
        /// </list>
        /// </param>
        private static void CopyText(StringBuilder strBuild, CString? cstr, ref Boolean first, ref Byte previous)
        {
            if (!CString.IsNullOrEmpty(cstr))
            {
                Int32 offset = 0;
                ReadOnlySpan<Byte> span = cstr;

                if (!first)
                {
                    Byte[] firstChar = new Byte[sizeOfChar];
                    Boolean previousDefault = previous == default;
                    firstChar[0] = previous;
                    firstChar[1] = previousDefault ? span[0] : default;
                    offset = previousDefault ? 1 : 0;
                    strBuild.Append(firstChar.AsValue<Char>());
                }
                else
                    first = false;

                //if (pending)
                //{
                //    Byte[] firstChar = new Byte[sizeOfChar];
                //    firstChar[0] = previous;
                //    firstChar[1] = span[0];
                //    offset = 1;
                //    strBuild.Append(firstChar.AsValue<Char>());
                //}
#pragma warning disable CS8602
                Int32 desiredSize = cstr.Length - offset;
#pragma warning restore CS8602
                if (desiredSize > 0)
                {
                    Int32 charSpanLength = desiredSize / sizeOfChar;
                    ReadOnlySpan<Char> chars = (span.AsIntPtr() + offset).AsReadOnlySpan<Char>(charSpanLength);
                    foreach (Char c in chars)
                        strBuild.Append(c);

                    if (charSpanLength * sizeOfChar != desiredSize)
                        previous = cstr[^1];
                    else
                        previous = default;

                    //pending = charSpanLength * sizeOfChar != desiredSize;
                    //previous = default;
                    //if (!pending)
                    //{
                    //    Byte[] lastChar = new Byte[sizeOfChar];
                    //    lastChar[0] = span[^1];
                    //    lastChar[1] = default;
                    //    offset = 1;
                    //    strBuild.Append(lastChar.AsValue<Char>());
                    //}
                }
            }
        }
    }
}
