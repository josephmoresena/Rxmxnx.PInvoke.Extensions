// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

/*
MIT License

Copyright (c) .NET Foundation and contributors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

// Adopted and adapted by Joseph Moreno in 2026 based on code from Microsoft.Blc.Memory 10.0.8
// (System.Rune)

#if !NETCOREAPP
namespace System;

/// <summary>
/// Represents a Unicode scalar value ([ U+0000..U+D7FF ], inclusive; or [ U+E000..U+10FFFF ], inclusive).
/// </summary>
/// <remarks>
/// This type's constructors and conversion operators validate the input, so consumers can call the APIs
/// assuming that the underlying <see cref="Rune"/> instance is well-formed.
/// </remarks>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
#if !PACKAGE
[ExcludeFromCodeCoverage]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1764)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3776)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS907)]
#endif
internal readonly struct Rune : IComparable, IComparable<Rune>, IEquatable<Rune>
{
	internal const Int32 MaxUtf16CharsPerRune = 2;
	internal const Int32 MaxUtf8BytesPerRune = 4;

	private const Char highSurrogateStart = '\ud800';
	private const Char lowSurrogateStart = '\udc00';
	private const Int32 highSurrogateRange = 0x3FF;

	private const Byte isWhiteSpaceFlag = 0x80;
	private const Byte isLetterOrDigitFlag = 0x40;
	private const Byte unicodeCategoryMask = 0x1F;

	private static ReadOnlySpan<Byte> AsciiCharInfo
		=>
		[
			0x0E, 0x0E, 0x0E, 0x0E, 0x0E, 0x0E, 0x0E, 0x0E, 0x0E, 0x8E, 0x8E, 0x8E, 0x8E, 0x8E, 0x0E,
			0x0E,
			0x0E, 0x0E, 0x0E, 0x0E, 0x0E, 0x0E, 0x0E, 0x0E, 0x0E, 0x0E, 0x0E, 0x0E, 0x0E, 0x0E, 0x0E,
			0x0E,
			0x8B, 0x18, 0x18, 0x18, 0x1A, 0x18, 0x18, 0x18, 0x14, 0x15, 0x18, 0x19, 0x18, 0x13, 0x18,
			0x18,
			0x48, 0x48, 0x48, 0x48, 0x48, 0x48, 0x48, 0x48, 0x48, 0x48, 0x18, 0x18, 0x19, 0x19, 0x19,
			0x18,
			0x18, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40,
			0x40,
			0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x14, 0x18, 0x15, 0x1B,
			0x12,
			0x1B, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41,
			0x41,
			0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x14, 0x19, 0x15, 0x19,
			0x0E,
		];

	private readonly UInt32 _value;

	/// <summary>
	/// Creates a <see cref="Rune"/> from the provided UTF-16 code unit.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException">
	/// If <paramref name="ch"/> represents a UTF-16 surrogate code point
	/// U+D800..U+DFFF, inclusive.
	/// </exception>
	public Rune(Char ch)
	{
		UInt32 expanded = ch;
		if (UnicodeUtility.IsSurrogateCodePoint(expanded))
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.ch);
		this._value = expanded;
	}

	/// <summary>
	/// Creates a <see cref="Rune"/> from the provided UTF-16 surrogate pair.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException">
	/// If <paramref name="highSurrogate"/> does not represent a UTF-16 high surrogate code point
	/// or <paramref name="lowSurrogate"/> does not represent a UTF-16 low surrogate code point.
	/// </exception>
	public Rune(Char highSurrogate, Char lowSurrogate) : this((UInt32)Char.ConvertToUtf32(highSurrogate, lowSurrogate),
	                                                          false) { }

	/// <summary>
	/// Creates a <see cref="Rune"/> from the provided Unicode scalar value.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException">
	/// If <paramref name="value"/> does not represent a value Unicode scalar value.
	/// </exception>
	public Rune(Int32 value) : this((UInt32)value) { }

	/// <summary>
	/// Creates a <see cref="Rune"/> from the provided Unicode scalar value.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException">
	/// If <paramref name="value"/> does not represent a value Unicode scalar value.
	/// </exception>
	public Rune(UInt32 value)
	{
		if (!UnicodeUtility.IsValidUnicodeScalar(value))
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value);
		this._value = value;
	}

	// ReSharper disable once UnusedParameter.Local
	private Rune(UInt32 scalarValue, Boolean _) => this._value = scalarValue;

	public static Boolean operator ==(Rune left, Rune right) => left._value == right._value;

	public static Boolean operator !=(Rune left, Rune right) => left._value != right._value;

	public static Boolean operator <(Rune left, Rune right) => left._value < right._value;

	public static Boolean operator <=(Rune left, Rune right) => left._value <= right._value;

	public static Boolean operator >(Rune left, Rune right) => left._value > right._value;

	public static Boolean operator >=(Rune left, Rune right) => left._value >= right._value;

	public static explicit operator Rune(Char ch) => new(ch);

	public static explicit operator Rune(UInt32 value) => new(value);

	public static explicit operator Rune(Int32 value) => new(value);

	// Displayed as "'<char>' (U+XXXX)"; e.g., "'e' (U+0065)"
	private String DebuggerDisplay
		=> FormattableString.Invariant(
			$"U+{this._value:X4} '{(Rune.IsValid(this._value) ? this.ToString() : "\uFFFD")}'");

	/// <summary>
	/// Returns true if and only if this scalar value is ASCII ([ U+0000..U+007F ])
	/// and therefore representable by a single UTF-8 code unit.
	/// </summary>
	public Boolean IsAscii => UnicodeUtility.IsAsciiCodePoint(this._value);

	/// <summary>
	/// Returns true if and only if this scalar value is within the BMP ([ U+0000..U+FFFF ])
	/// and therefore representable by a single UTF-16 code unit.
	/// </summary>
	public Boolean IsBmp => UnicodeUtility.IsBmpCodePoint(this._value);

	/// <summary>
	/// Returns the Unicode plane (0 to 16, inclusive) which contains this scalar.
	/// </summary>
	public Int32 Plane => UnicodeUtility.GetPlane(this._value);

	/// <summary>
	/// A <see cref="Rune"/> instance that represents the Unicode replacement character U+FFFD.
	/// </summary>
	public static Rune ReplacementChar => Rune.UnsafeCreate(UnicodeUtility.ReplacementChar);

	/// <summary>
	/// Returns the length in code units (<see cref="char"/>) of the
	/// UTF-16 sequence required to represent this scalar value.
	/// </summary>
	/// <remarks>
	/// The return value will be 1 or 2.
	/// </remarks>
	public Int32 Utf16SequenceLength
	{
		get
		{
			Int32 codeUnitCount = UnicodeUtility.GetUtf16SequenceLength(this._value);
			return codeUnitCount;
		}
	}

	/// <summary>
	/// Returns the length in code units of the
	/// UTF-8 sequence required to represent this scalar value.
	/// </summary>
	/// <remarks>
	/// The return value will be 1 through 4, inclusive.
	/// </remarks>
	public Int32 Utf8SequenceLength
	{
		get
		{
			Int32 codeUnitCount = UnicodeUtility.GetUtf8SequenceLength(this._value);
			return codeUnitCount;
		}
	}

	/// <summary>
	/// Returns the Unicode scalar value as an integer.
	/// </summary>
	public Int32 Value => (Int32)this._value;

	private static Rune ChangeCaseCultureAware(Rune rune, CultureInfo culture, Boolean toUpper)
	{
		Span<Char> original = stackalloc Char[Rune.MaxUtf16CharsPerRune];
		Span<Char> modified = stackalloc Char[Rune.MaxUtf16CharsPerRune];
		Int32 charCount = rune.EncodeToUtf16(original);

		original = original[..charCount];
		modified = modified[..charCount];

		if (toUpper)
			original.ToUpper(modified, culture);
		else
			original.ToLower(modified, culture);

		return rune.IsBmp ?
			Rune.UnsafeCreate(modified[0]) :
			Rune.UnsafeCreate(UnicodeUtility.GetScalarFromUtf16SurrogatePair(modified[0], modified[1]));
	}

	public Int32 CompareTo(Rune other) => this.Value - other.Value;

	/// <summary>
	/// Decodes the <see cref="Rune"/> at the beginning of the provided UTF-16 source buffer.
	/// </summary>
	/// <returns>
	///     <para>
	///     If the source buffer begins with a valid UTF-16 encoded scalar value, returns <see cref="OperationStatus.Done"/>,
	///     and outs via <paramref name="result"/> the decoded <see cref="Rune"/> and via <paramref name="charsConsumed"/> the
	///     number of <see langword="char"/>s used in the input buffer to encode the <see cref="Rune"/>.
	///     </para>
	///     <para>
	///     If the source buffer is empty or contains only a standalone UTF-16 high surrogate character, returns
	///     <see cref="OperationStatus.NeedMoreData"/>,
	///     and outs via <paramref name="result"/> <see cref="ReplacementChar"/> and via <paramref name="charsConsumed"/> the
	///     length of the input buffer.
	///     </para>
	///     <para>
	///     If the source buffer begins with an ill-formed UTF-16 encoded scalar value, returns
	///     <see cref="OperationStatus.InvalidData"/>,
	///     and outs via <paramref name="result"/> <see cref="ReplacementChar"/> and via <paramref name="charsConsumed"/> the
	///     number of
	///     <see langword="char"/>s used in the input buffer to encode the ill-formed sequence.
	///     </para>
	/// </returns>
	/// <remarks>
	/// The general calling convention is to call this method in a loop, slicing the <paramref name="source"/> buffer by
	/// <paramref name="charsConsumed"/> elements on each iteration of the loop. On each iteration of the loop
	/// <paramref name="result"/>
	/// will contain the real scalar value if successfully decoded, or it will contain <see cref="ReplacementChar"/> if
	/// the data could not be successfully decoded. This pattern provides convenient automatic U+FFFD substitution of
	/// invalid sequences while iterating through the loop.
	/// </remarks>
	public static OperationStatus DecodeFromUtf16(ReadOnlySpan<Char> source, out Rune result, out Int32 charsConsumed)
	{
		if (!source.IsEmpty)
		{
			Char firstChar = source[0];
			if (Rune.TryCreate(firstChar, out result))
			{
				charsConsumed = 1;
				return OperationStatus.Done;
			}

			if (source.Length > 1)
			{
				Char secondChar = source[1];
				if (Rune.TryCreate(firstChar, secondChar, out result))
				{
					charsConsumed = 2;
					return OperationStatus.Done;
				}
				goto InvalidData;
			}
			if (!Char.IsHighSurrogate(firstChar))
				goto InvalidData;
		}

		charsConsumed = source.Length;
		result = Rune.ReplacementChar;
		return OperationStatus.NeedMoreData;

		InvalidData:

		charsConsumed = 1;
		result = Rune.ReplacementChar;
		return OperationStatus.InvalidData;
	}

	/// <summary>
	/// Decodes the <see cref="Rune"/> at the beginning of the provided UTF-8 source buffer.
	/// </summary>
	/// <returns>
	///     <para>
	///     If the source buffer begins with a valid UTF-8 encoded scalar value, returns <see cref="OperationStatus.Done"/>,
	///     and outs via <paramref name="result"/> the decoded <see cref="Rune"/> and via <paramref name="bytesConsumed"/> the
	///     number of <see langword="byte"/>s used in the input buffer to encode the <see cref="Rune"/>.
	///     </para>
	///     <para>
	///     If the source buffer is empty or contains only a partial UTF-8 subsequence, returns
	///     <see cref="OperationStatus.NeedMoreData"/>,
	///     and outs via <paramref name="result"/> <see cref="ReplacementChar"/> and via <paramref name="bytesConsumed"/> the
	///     length of the input buffer.
	///     </para>
	///     <para>
	///     If the source buffer begins with an ill-formed UTF-8 encoded scalar value, returns
	///     <see cref="OperationStatus.InvalidData"/>,
	///     and outs via <paramref name="result"/> <see cref="ReplacementChar"/> and via <paramref name="bytesConsumed"/> the
	///     number of
	///     <see langword="char"/>s used in the input buffer to encode the ill-formed sequence.
	///     </para>
	/// </returns>
	/// <remarks>
	/// The general calling convention is to call this method in a loop, slicing the <paramref name="source"/> buffer by
	/// <paramref name="bytesConsumed"/> elements on each iteration of the loop. On each iteration of the loop
	/// <paramref name="result"/>
	/// will contain the real scalar value if successfully decoded, or it will contain <see cref="ReplacementChar"/> if
	/// the data could not be successfully decoded. This pattern provides convenient automatic U+FFFD substitution of
	/// invalid sequences while iterating through the loop.
	/// </remarks>
	public static OperationStatus DecodeFromUtf8(ReadOnlySpan<Byte> source, out Rune result, out Int32 bytesConsumed)
	{
		Int32 index = 0;
		if (source.IsEmpty)
			goto NeedsMoreData;

		UInt32 tempValue = source[0];
		if (UnicodeUtility.IsAsciiCodePoint(tempValue))
		{
			bytesConsumed = 1;
			result = Rune.UnsafeCreate(tempValue);
			return OperationStatus.Done;
		}
		index = 1;
		if (!UnicodeUtility.IsInRangeInclusive(tempValue, 0xC2, 0xF4))
			goto Invalid;

		tempValue = (tempValue - 0xC2) << 6;

		if (source.Length <= 1)
			goto NeedsMoreData;

		Int32 thisByteSignExtended = (SByte)source[1];
		if (thisByteSignExtended >= -64)
			goto Invalid;

		unchecked
		{
			tempValue += (UInt32)thisByteSignExtended;
			tempValue += 0x80;
			tempValue += (0xC2 - 0xC0) << 6;
		}

		if (tempValue < 0x0800)
			goto Finish;

		if (!UnicodeUtility.IsInRangeInclusive(tempValue, ((0xE0 - 0xC0) << 6) + (0xA0 - 0x80),
		                                       ((0xF4 - 0xC0) << 6) + (0x8F - 0x80)))
			goto Invalid;

		if (UnicodeUtility.IsInRangeInclusive(tempValue, ((0xED - 0xC0) << 6) + (0xA0 - 0x80),
		                                      ((0xED - 0xC0) << 6) + (0xBF - 0x80)))
			goto Invalid;

		if (UnicodeUtility.IsInRangeInclusive(tempValue, ((0xF0 - 0xC0) << 6) + (0x80 - 0x80),
		                                      ((0xF0 - 0xC0) << 6) + (0x8F - 0x80)))
			goto Invalid;

		index = 2;
		if (source.Length <= 2)
			goto NeedsMoreData;

		thisByteSignExtended = (SByte)source[2];
		if (thisByteSignExtended >= -64)
			goto Invalid;

		unchecked
		{
			tempValue <<= 6;
			tempValue += (UInt32)thisByteSignExtended;
			tempValue += 0x80;
			tempValue -= (0xE0 - 0xC0) << 12;
		}

		if (tempValue <= 0xFFFF)
			goto Finish;

		index = 3;
		if (source.Length <= 3) goto NeedsMoreData;

		thisByteSignExtended = (SByte)source[3];
		if (thisByteSignExtended >= -64) goto Invalid;

		unchecked
		{
			tempValue <<= 6;
			tempValue += (UInt32)thisByteSignExtended;
			tempValue += 0x80;
			tempValue -= (0xF0 - 0xE0) << 18;
		}

		Finish:

		bytesConsumed = index + 1;
		result = Rune.UnsafeCreate(tempValue);
		return OperationStatus.Done;

		NeedsMoreData:
		bytesConsumed = index;
		result = Rune.ReplacementChar;
		return OperationStatus.NeedMoreData;

		Invalid:
		bytesConsumed = index;
		result = Rune.ReplacementChar;
		return OperationStatus.InvalidData;
	}

	/// <summary>
	/// Decodes the <see cref="Rune"/> at the end of the provided UTF-16 source buffer.
	/// </summary>
	/// <remarks>
	/// This method is very similar to <see cref="DecodeFromUtf16(ReadOnlySpan{char}, out Rune, out int)"/>, but it allows
	/// the caller to loop backward instead of forward. The typical calling convention is that on each iteration
	/// of the loop, the caller should slice off the final <paramref name="charsConsumed"/> elements of
	/// the <paramref name="source"/> buffer.
	/// </remarks>
	public static OperationStatus DecodeLastFromUtf16(ReadOnlySpan<Char> source, out Rune result,
		out Int32 charsConsumed)
	{
		Int32 index = source.Length - 1;
		if ((UInt32)index < (UInt32)source.Length)
		{
			Char finalChar = source[index];
			if (Rune.TryCreate(finalChar, out result))
			{
				charsConsumed = 1;
				return OperationStatus.Done;
			}

			if (Char.IsLowSurrogate(finalChar))
			{
				index--;
				if ((UInt32)index < (UInt32)source.Length)
				{
					Char penultimateChar = source[index];
					if (Rune.TryCreate(penultimateChar, finalChar, out result))
					{
						charsConsumed = 2;
						return OperationStatus.Done;
					}
				}
				charsConsumed = 1;
				result = Rune.ReplacementChar;
				return OperationStatus.InvalidData;
			}
		}

		// If we got this far, the source buffer was empty, or the source buffer ended
		// with a UTF-16 high surrogate code point. These aren't errors since they could
		// be valid given more input data.

		charsConsumed = -source.Length >>> 31; // 0 -> 0, all other lengths -> 1
		result = Rune.ReplacementChar;
		return OperationStatus.NeedMoreData;
	}

	/// <summary>
	/// Decodes the <see cref="Rune"/> at the end of the provided UTF-8 source buffer.
	/// </summary>
	/// <remarks>
	/// This method is very similar to <see cref="DecodeFromUtf8(ReadOnlySpan{byte}, out Rune, out int)"/>, but it allows
	/// the caller to loop backward instead of forward. The typical calling convention is that on each iteration
	/// of the loop, the caller should slice off the final <paramref name="bytesConsumed"/> elements of
	/// the <paramref name="source"/> buffer.
	/// </remarks>
	public static OperationStatus DecodeLastFromUtf8(ReadOnlySpan<Byte> source, out Rune value, out Int32 bytesConsumed)
	{
		Int32 index = source.Length - 1;
		if ((UInt32)index < (UInt32)source.Length)
		{
			UInt32 tempValue = source[index];
			if (UnicodeUtility.IsAsciiCodePoint(tempValue))
			{
				bytesConsumed = 1;
				value = Rune.UnsafeCreate(tempValue);
				return OperationStatus.Done;
			}
			if (((Byte)tempValue & 0x40) != 0)
				return Rune.DecodeFromUtf8(source[index..], out value, out bytesConsumed);

			for (Int32 i = 3; i > 0; i--)
			{
				index--;
				if ((UInt32)index >= (UInt32)source.Length) goto Invalid;
				if ((SByte)source[index] >= -64) goto ForwardDecode;
			}

			Invalid:
			value = Rune.ReplacementChar;
			bytesConsumed = 1;
			return OperationStatus.InvalidData;

			ForwardDecode:

			source = source[index..];

			OperationStatus operationStatus =
				Rune.DecodeFromUtf8(source, out Rune tempRune, out Int32 tempBytesConsumed);
			if (tempBytesConsumed == source.Length)
			{
				bytesConsumed = tempBytesConsumed;
				value = tempRune;
				return operationStatus;
			}

			goto Invalid;
		}
		value = Rune.ReplacementChar;
		bytesConsumed = 0;
		return OperationStatus.NeedMoreData;
	}

	/// <summary>
	/// Encodes this <see cref="Rune"/> to a UTF-16 destination buffer.
	/// </summary>
	/// <param name="destination">The buffer to which to write this value as UTF-16.</param>
	/// <returns>The number of <see cref="char"/>s written to <paramref name="destination"/>.</returns>
	/// <exception cref="ArgumentException">
	/// If <paramref name="destination"/> is not large enough to hold the output.
	/// </exception>
	public Int32 EncodeToUtf16(Span<Char> destination)
	{
		if (!this.TryEncodeToUtf16(destination, out Int32 charsWritten))
			ThrowHelper.ThrowArgumentException_DestinationTooShort();

		return charsWritten;
	}

	/// <summary>
	/// Encodes this <see cref="Rune"/> to a UTF-8 destination buffer.
	/// </summary>
	/// <param name="destination">The buffer to which to write this value as UTF-8.</param>
	/// <returns>The number of <see cref="byte"/>s written to <paramref name="destination"/>.</returns>
	/// <exception cref="ArgumentException">
	/// If <paramref name="destination"/> is not large enough to hold the output.
	/// </exception>
	public Int32 EncodeToUtf8(Span<Byte> destination)
	{
		if (!this.TryEncodeToUtf8(destination, out Int32 bytesWritten))
			ThrowHelper.ThrowArgumentException_DestinationTooShort();

		return bytesWritten;
	}

	public override Boolean Equals([NotNullWhen(true)] Object? obj) => obj is Rune other && this.Equals(other);

	public Boolean Equals(Rune other) => this == other;

	public override Int32 GetHashCode() => this.Value;

	/// <summary>
	/// Returns <see langword="true"/> iff <paramref name="value"/> is a valid Unicode scalar
	/// value, i.e., is in [ U+0000..U+D7FF ], inclusive; or [ U+E000..U+10FFFF ], inclusive.
	/// </summary>
	public static Boolean IsValid(Int32 value) => Rune.IsValid((UInt32)value);

	/// <summary>
	/// Returns <see langword="true"/> iff <paramref name="value"/> is a valid Unicode scalar
	/// value, i.e., is in [ U+0000..U+D7FF ], inclusive; or [ U+E000..U+10FFFF ], inclusive.
	/// </summary>
	public static Boolean IsValid(UInt32 value) => UnicodeUtility.IsValidUnicodeScalar(value);

	internal static Int32 ReadFirstRuneFromUtf16Buffer(ReadOnlySpan<Char> input)
	{
		if (input.IsEmpty) return -1;

		UInt32 returnValue = input[0];
		if (!UnicodeUtility.IsSurrogateCodePoint(returnValue)) return (Int32)returnValue;
		if (!UnicodeUtility.IsHighSurrogateCodePoint(returnValue) || input.Length <= 1) return -1;

		UInt32 potentialLowSurrogate = input[1];
		if (!UnicodeUtility.IsLowSurrogateCodePoint(potentialLowSurrogate)) return -1;
		returnValue = UnicodeUtility.GetScalarFromUtf16SurrogatePair(returnValue, potentialLowSurrogate);
		return (Int32)returnValue;
	}

	/// <summary>
	/// Returns a <see cref="string"/> representation of this <see cref="Rune"/> instance.
	/// </summary>
	public override String ToString()
	{
		if (this.IsBmp) return ((Char)this._value).ToString();
		Span<Char> buffer = stackalloc Char[Rune.MaxUtf16CharsPerRune];
		UnicodeUtility.GetUtf16SurrogatesFromSupplementaryPlaneScalar(this._value, out buffer[0], out buffer[1]);
		return buffer.ToString();
	}

	/// <summary>
	/// Attempts to create a <see cref="Rune"/> from the provided input value.
	/// </summary>
	public static Boolean TryCreate(Char ch, out Rune result)
	{
		UInt32 extendedValue = ch;
		if (!UnicodeUtility.IsSurrogateCodePoint(extendedValue))
		{
			result = Rune.UnsafeCreate(extendedValue);
			return true;
		}
		result = default;
		return false;
	}

	/// <summary>
	/// Attempts to create a <see cref="Rune"/> from the provided UTF-16 surrogate pair.
	/// Returns <see langword="false"/> if the input values don't represent a well-formed UTF-16surrogate pair.
	/// </summary>
	public static Boolean TryCreate(Char highSurrogate, Char lowSurrogate, out Rune result)
	{
		UInt32 highSurrogateOffset = (UInt32)highSurrogate - Rune.highSurrogateStart;
		UInt32 lowSurrogateOffset = (UInt32)lowSurrogate - Rune.lowSurrogateStart;
		if ((highSurrogateOffset | lowSurrogateOffset) <= Rune.highSurrogateRange)
		{
			result = Rune.UnsafeCreate((highSurrogateOffset << 10) + ((UInt32)lowSurrogate - Rune.lowSurrogateStart) +
			                           (0x40u << 10));
			return true;
		}
		result = default;
		return false;
	}

	/// <summary>
	/// Attempts to create a <see cref="Rune"/> from the provided input value.
	/// </summary>
	public static Boolean TryCreate(Int32 value, out Rune result) => Rune.TryCreate((UInt32)value, out result);

	/// <summary>
	/// Attempts to create a <see cref="Rune"/> from the provided input value.
	/// </summary>
	public static Boolean TryCreate(UInt32 value, out Rune result)
	{
		if (UnicodeUtility.IsValidUnicodeScalar(value))
		{
			result = Rune.UnsafeCreate(value);
			return true;
		}
		result = default;
		return false;
	}

	/// <summary>
	/// Encodes this <see cref="Rune"/> to a UTF-16 destination buffer.
	/// </summary>
	/// <param name="destination">The buffer to which to write this value as UTF-16.</param>
	/// <param name="charsWritten">
	/// The number of <see cref="char"/>s written to <paramref name="destination"/>,
	/// or 0 if the destination buffer is not large enough to contain the output.
	/// </param>
	/// <returns>True if the value was written to the buffer; otherwise, false.</returns>
	/// <remarks>
	/// The <see cref="Utf16SequenceLength"/> property can be queried ahead of time to determine
	/// the required size of the <paramref name="destination"/> buffer.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Boolean TryEncodeToUtf16(Span<Char> destination, out Int32 charsWritten)
		=> Rune.TryEncodeToUtf16(this, destination, out charsWritten);

	private static Boolean TryEncodeToUtf16(Rune value, Span<Char> destination, out Int32 charsWritten)
	{
		if (!destination.IsEmpty)
		{
			if (value.IsBmp)
			{
				destination[0] = (Char)value._value;
				charsWritten = 1;
				return true;
			}
			if (destination.Length > 1)
			{
				UnicodeUtility.GetUtf16SurrogatesFromSupplementaryPlaneScalar(
					value._value, out destination[0], out destination[1]);
				charsWritten = 2;
				return true;
			}
		}
		charsWritten = default;
		return false;
	}

	/// <summary>
	/// Encodes this <see cref="Rune"/> to a destination buffer as UTF-8 bytes.
	/// </summary>
	/// <param name="destination">The buffer to which to write this value as UTF-8.</param>
	/// <param name="bytesWritten">
	/// The number of <see cref="byte"/>s written to <paramref name="destination"/>,
	/// or 0 if the destination buffer is not large enough to contain the output.
	/// </param>
	/// <returns>True if the value was written to the buffer; otherwise, false.</returns>
	/// <remarks>
	/// The <see cref="Utf8SequenceLength"/> property can be queried ahead of time to determine
	/// the required size of the <paramref name="destination"/> buffer.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Boolean TryEncodeToUtf8(Span<Byte> destination, out Int32 bytesWritten)
		=> Rune.TryEncodeToUtf8(this, destination, out bytesWritten);

	private static Boolean TryEncodeToUtf8(Rune value, Span<Byte> destination, out Int32 bytesWritten)
	{
		if (!destination.IsEmpty)
		{
			if (value.IsAscii)
			{
				destination[0] = (Byte)value._value;
				bytesWritten = 1;
				return true;
			}

			if (destination.Length > 1)
			{
				if (value.Value <= 0x7FFu)
				{
					destination[0] = (Byte)((value._value + (0b110u << 11)) >> 6);
					destination[1] = (Byte)((value._value & 0x3Fu) + 0x80u);
					bytesWritten = 2;
					return true;
				}

				if (destination.Length > 2)
				{
					if (value.Value <= 0xFFFFu)
					{
						destination[0] = (Byte)((value._value + (0b1110 << 16)) >> 12);
						destination[1] = (Byte)(((value._value & (0x3Fu << 6)) >> 6) + 0x80u);
						destination[2] = (Byte)((value._value & 0x3Fu) + 0x80u);
						bytesWritten = 3;
						return true;
					}

					if (destination.Length > 3)
					{
						destination[0] = (Byte)((value._value + (0b11110 << 21)) >> 18);
						destination[1] = (Byte)(((value._value & (0x3Fu << 12)) >> 12) + 0x80u);
						destination[2] = (Byte)(((value._value & (0x3Fu << 6)) >> 6) + 0x80u);
						destination[3] = (Byte)((value._value & 0x3Fu) + 0x80u);
						bytesWritten = 4;
						return true;
					}
				}
			}
		}
		bytesWritten = default;
		return false;
	}

	/// <summary>
	/// Creates a <see cref="Rune"/> without performing validation on the input.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Rune UnsafeCreate(UInt32 scalarValue) => new(scalarValue, false);

	public static Double GetNumericValue(Rune value)
	{
		if (!value.IsAscii)
			if (value.IsBmp)
				return CharUnicodeInfo.GetNumericValue((Char)value._value);
			else
				return CharUnicodeInfo.GetNumericValue(value.ToString(), 0);
		UInt32 baseNum = value._value - '0';
		return baseNum <= 9 ? baseNum : -1;
	}

	public static UnicodeCategory GetUnicodeCategory(Rune value)
	{
		if (value.IsAscii) return (UnicodeCategory)(Rune.AsciiCharInfo[value.Value] & Rune.unicodeCategoryMask);
		return Rune.GetUnicodeCategoryNonAscii(value);
	}

	private static UnicodeCategory GetUnicodeCategoryNonAscii(Rune value)
		=> CharUnicodeInfo.GetUnicodeCategory(value.Value);

	// Returns true iff this Unicode category represents a letter
	private static Boolean IsCategoryLetter(UnicodeCategory category)
		=> UnicodeUtility.IsInRangeInclusive((UInt32)category, (UInt32)UnicodeCategory.UppercaseLetter,
		                                     (UInt32)UnicodeCategory.OtherLetter);

	// Returns true iff this Unicode category represents a letter or a decimal digit
	private static Boolean IsCategoryLetterOrDecimalDigit(UnicodeCategory category)
		=> UnicodeUtility.IsInRangeInclusive((UInt32)category, (UInt32)UnicodeCategory.UppercaseLetter,
		                                     (UInt32)UnicodeCategory.OtherLetter) ||
			category == UnicodeCategory.DecimalDigitNumber;

	// Returns true iff this Unicode category represents a number
	private static Boolean IsCategoryNumber(UnicodeCategory category)
		=> UnicodeUtility.IsInRangeInclusive((UInt32)category, (UInt32)UnicodeCategory.DecimalDigitNumber,
		                                     (UInt32)UnicodeCategory.OtherNumber);

	// Returns true iff this Unicode category represents a punctuation mark
	private static Boolean IsCategoryPunctuation(UnicodeCategory category)
		=> UnicodeUtility.IsInRangeInclusive((UInt32)category, (UInt32)UnicodeCategory.ConnectorPunctuation,
		                                     (UInt32)UnicodeCategory.OtherPunctuation);

	// Returns true iff this Unicode category represents a separator
	private static Boolean IsCategorySeparator(UnicodeCategory category)
		=> UnicodeUtility.IsInRangeInclusive((UInt32)category, (UInt32)UnicodeCategory.SpaceSeparator,
		                                     (UInt32)UnicodeCategory.ParagraphSeparator);

	// Returns true iff this Unicode category represents a symbol
	private static Boolean IsCategorySymbol(UnicodeCategory category)
		=> UnicodeUtility.IsInRangeInclusive((UInt32)category, (UInt32)UnicodeCategory.MathSymbol,
		                                     (UInt32)UnicodeCategory.OtherSymbol);

	public static Boolean IsControl(Rune value) => ((value._value + 1) & ~0x80u) <= 0x20u;

	public static Boolean IsDigit(Rune value)
	{
		if (value.IsAscii) return UnicodeUtility.IsInRangeInclusive(value._value, '0', '9');
		return Rune.GetUnicodeCategoryNonAscii(value) == UnicodeCategory.DecimalDigitNumber;
	}

	public static Boolean IsLetter(Rune value)
	{
		if (value.IsAscii) return ((value._value - 'A') & ~0x20u) <= 'Z' - 'A'; // [A-Za-z]
		return Rune.IsCategoryLetter(Rune.GetUnicodeCategoryNonAscii(value));
	}

	public static Boolean IsLetterOrDigit(Rune value)
	{
		if (value.IsAscii) return (Rune.AsciiCharInfo[value.Value] & Rune.isLetterOrDigitFlag) != 0;
		return Rune.IsCategoryLetterOrDecimalDigit(Rune.GetUnicodeCategoryNonAscii(value));
	}

	public static Boolean IsLower(Rune value)
	{
		if (value.IsAscii) return UnicodeUtility.IsInRangeInclusive(value._value, 'a', 'z');
		return Rune.GetUnicodeCategoryNonAscii(value) == UnicodeCategory.LowercaseLetter;
	}

	public static Boolean IsNumber(Rune value)
		=> value.IsAscii ?
			UnicodeUtility.IsInRangeInclusive(value._value, '0', '9') :
			Rune.IsCategoryNumber(Rune.GetUnicodeCategoryNonAscii(value));

	public static Boolean IsPunctuation(Rune value) => Rune.IsCategoryPunctuation(Rune.GetUnicodeCategory(value));

	public static Boolean IsSeparator(Rune value) => Rune.IsCategorySeparator(Rune.GetUnicodeCategory(value));

	public static Boolean IsSymbol(Rune value) => Rune.IsCategorySymbol(Rune.GetUnicodeCategory(value));

	public static Boolean IsUpper(Rune value)
	{
		if (value.IsAscii) return UnicodeUtility.IsInRangeInclusive(value._value, 'A', 'Z');
		return Rune.GetUnicodeCategoryNonAscii(value) == UnicodeCategory.UppercaseLetter;
	}

	public static Boolean IsWhiteSpace(Rune value)
	{
		if (value.IsAscii) return (Rune.AsciiCharInfo[value.Value] & Rune.isWhiteSpaceFlag) != 0;
		return value.IsBmp && Char.IsWhiteSpace((Char)value._value);
	}

	public static Rune ToLower(Rune value, CultureInfo? culture)
	{
		if (culture is null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.culture);
		return Rune.ChangeCaseCultureAware(value, culture, false);
	}

	public static Rune ToLowerInvariant(Rune value)
		=> value.IsAscii ?
			Rune.UnsafeCreate(Utf16Utility.ConvertAllAsciiCharsInUInt32ToLowercase(value._value)) :
			Rune.ChangeCaseCultureAware(value, CultureInfo.InvariantCulture, false);

	public static Rune ToUpper(Rune value, CultureInfo? culture)
	{
		if (culture is null) ThrowHelper.ThrowArgumentNullException(ExceptionArgument.culture);
		return Rune.ChangeCaseCultureAware(value, culture, true);
	}

	public static Rune ToUpperInvariant(Rune value)
		=> value.IsAscii ?
			Rune.UnsafeCreate(Utf16Utility.ConvertAllAsciiCharsInUInt32ToUppercase(value._value)) :
			Rune.ChangeCaseCultureAware(value, CultureInfo.InvariantCulture, true);

	/// <inheritdoc cref="IComparable.CompareTo"/>
	Int32 IComparable.CompareTo(Object? obj)
		=> obj switch
		{
			null => 1,
			Rune other => this.CompareTo(other),
			_ => throw new ArgumentException(),
		};
}
#endif