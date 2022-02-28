using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Represents a sequence of null-terminated UTF-8 texts.
    /// </summary>
    public sealed record CStringSequence
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
        /// Returns a <see cref="String"/> that represents the current object.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current object.</returns>
        public override String ToString() => this._value;

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
            Boolean pending = false;
            Byte previous = default;
            for (Int32 i = 0; i < lengths.Length; i++)
                CopyText(sb, values[i], ref pending, ref previous);
            return sb.ToString();
        }

        /// <summary>
        /// Copy the UTF-8 into the buffer builder.
        /// </summary>
        /// <param name="strBuild">Buffer builder.</param>
        /// <param name="cstr">UTF-8 text.</param>
        /// <param name="pending">
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
        private static void CopyText(StringBuilder strBuild, CString? cstr, ref Boolean pending, ref Byte previous)
        {
            if (!CString.IsNullOrEmpty(cstr))
            {
                Int32 offset = 0;
                ReadOnlySpan<Byte> span = cstr;
                if (pending)
                {
                    Byte[] firstChar = new Byte[sizeOfChar];
                    firstChar[0] = previous;
                    firstChar[1] = span[0];
                    offset = 1;
                    strBuild.Append(firstChar.AsValue<Char>());
                }
#pragma warning disable CS8602
                Int32 desiredSize = cstr.Length + 1 - offset;
#pragma warning restore CS8602
                if (span.Length - offset > 0)
                {
                    Int32 charCount = span.Length / sizeOfChar;
                    ReadOnlySpan<Char> chars = (span.AsIntPtr() + offset).AsReadOnlySpan<Char>(charCount);
                    foreach (Char c in chars)
                        strBuild.Append(c);

                    pending = chars.Length != desiredSize / sizeOfChar || desiredSize % sizeOfChar != 0;
                    previous = pending && cstr.Length < span.Length ? span[^1] : default;
                }
            }
        }
    }
}
