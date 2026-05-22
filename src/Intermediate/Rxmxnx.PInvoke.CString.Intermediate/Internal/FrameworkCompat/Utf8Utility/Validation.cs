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
// (System.text.Unicode.Utf8Utility)

#if !NETCOREAPP
namespace System.Text.Unicode;

// ReSharper disable BuiltInTypeReferenceStyle
using UIntPtr = nuint;
using IntPtr = nint;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3776)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1199)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS907)]
#endif
internal static unsafe partial class Utf8Utility
{
	// Returns &inputBuffer[inputLength] if the input buffer is valid.
	/// <summary>
	/// Given an input buffer <paramref name="pInputBuffer"/> of byte length <paramref name="inputLength"/>,
	/// returns a pointer to where the first invalid data appears in <paramref name="pInputBuffer"/>.
	/// </summary>
	/// <remarks>
	/// Returns a pointer to the end of <paramref name="pInputBuffer"/> if the buffer is well-formed.
	/// </remarks>
	public static Byte* GetPointerToFirstInvalidByte(Byte* pInputBuffer, Int32 inputLength,
		out Int32 utf16CodeUnitCountAdjustment, out Int32 scalarCountAdjustment)
	{
		UIntPtr numAsciiBytesCounted = Ascii.GetIndexOfFirstNonAsciiByte(pInputBuffer, (UInt32)inputLength);
		pInputBuffer += numAsciiBytesCounted;
		inputLength -= (Int32)numAsciiBytesCounted;
		if (inputLength == 0)
		{
			utf16CodeUnitCountAdjustment = 0;
			scalarCountAdjustment = 0;
			return pInputBuffer;
		}
		Int32 tempUtf16CodeUnitCountAdjustment = 0;
		Int32 tempScalarCountAdjustment = 0;

		if (inputLength < sizeof(UInt32)) goto ProcessInputOfLessThanDWordSize;
		Byte* pFinalPosWhereCanReadDWordFromInputBuffer = pInputBuffer + (UInt32)inputLength - sizeof(UInt32);
		while (pInputBuffer <= pFinalPosWhereCanReadDWordFromInputBuffer)
		{
			UInt32 thisDWord = Unsafe.ReadUnaligned<uint>(pInputBuffer);

			AfterReadDWord:

			if (Ascii.AllBytesInUInt32AreAscii(thisDWord))
			{
				pInputBuffer += sizeof(UInt32);
				if ((IntPtr)(void*)Unsafe.ByteOffset(ref *pInputBuffer,
				                                     ref *pFinalPosWhereCanReadDWordFromInputBuffer) >=
				    4 * sizeof(UInt32))
				{
					thisDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer);
					if (!Ascii.AllBytesInUInt32AreAscii(thisDWord)) goto AfterReadDWordSkipAllBytesAsciiCheck;

					pInputBuffer = (Byte*)((UIntPtr)(pInputBuffer + 4) & ~(UIntPtr)3);

					Byte* pInputBufferFinalPosAtWhichCanSafelyLoop =
						pFinalPosWhereCanReadDWordFromInputBuffer - 3 * sizeof(UInt32);
					do
					{
						if (!Ascii.AllBytesInUInt32AreAscii(((UInt32*)pInputBuffer)[0] | ((UInt32*)pInputBuffer)[1]))
							goto LoopTerminatedEarlyDueToNonAsciiDataInFirstPair;

						if (!Ascii.AllBytesInUInt32AreAscii(((UInt32*)pInputBuffer)[2] | ((UInt32*)pInputBuffer)[3]))
							goto LoopTerminatedEarlyDueToNonAsciiDataInSecondPair;

						pInputBuffer += 4 * sizeof(UInt32);
					} while (pInputBuffer <= pInputBufferFinalPosAtWhichCanSafelyLoop);

					continue;

					LoopTerminatedEarlyDueToNonAsciiDataInSecondPair:

					pInputBuffer += 2 * sizeof(UInt32);

					LoopTerminatedEarlyDueToNonAsciiDataInFirstPair:
					thisDWord = *(UInt32*)pInputBuffer;
					if (Ascii.AllBytesInUInt32AreAscii(thisDWord))
					{
						pInputBuffer += sizeof(UInt32);
						thisDWord = *(UInt32*)pInputBuffer;
					}

					goto AfterReadDWordSkipAllBytesAsciiCheck;
				}

				continue;
			}

			AfterReadDWordSkipAllBytesAsciiCheck:
			UInt32 numLeadingAsciiBytes = Ascii.CountNumberOfLeadingAsciiBytesFromUInt32WithSomeNonAsciiData(thisDWord);
			pInputBuffer += numLeadingAsciiBytes;

			if (pFinalPosWhereCanReadDWordFromInputBuffer < pInputBuffer)
				goto ProcessRemainingBytesSlow;
			thisDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer);

			BeforeProcessTwoByteSequence:
			thisDWord -= BitConverter.IsLittleEndian ? 0x0000_80C0u : 0xC080_0000u;
			if ((thisDWord & (BitConverter.IsLittleEndian ? 0x0000_C0E0u : 0xE0C0_0000u)) == 0)
			{
				if ((BitConverter.IsLittleEndian && (Byte)thisDWord < 0x02u) ||
				    (!BitConverter.IsLittleEndian && thisDWord < 0x0200_0000u))
					goto Error;
				ProcessTwoByteSequenceSkipOverlongFormCheck:

				if ((BitConverter.IsLittleEndian &&
					    Utf8Utility.UInt32EndsWithValidUtf8TwoByteSequenceLittleEndian(thisDWord)) ||
				    (!BitConverter.IsLittleEndian && Utf8Utility.UInt32EndsWithUtf8TwoByteMask(thisDWord) &&
					    !Utf8Utility.UInt32EndsWithOverlongUtf8TwoByteSequence(thisDWord)))
				{
					pInputBuffer += 4;
					tempUtf16CodeUnitCountAdjustment -= 2;
					if (pInputBuffer <= pFinalPosWhereCanReadDWordFromInputBuffer)
					{
						thisDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer);

						if (BitConverter.IsLittleEndian)
						{
							if (Utf8Utility.UInt32BeginsWithValidUtf8TwoByteSequenceLittleEndian(thisDWord))
								goto ProcessTwoByteSequenceSkipOverlongFormCheck;
						}
						else
						{
							if (Utf8Utility.UInt32BeginsWithUtf8TwoByteMask(thisDWord))
							{
								if (Utf8Utility.UInt32BeginsWithOverlongUtf8TwoByteSequence(thisDWord))
									goto Error;
								goto ProcessTwoByteSequenceSkipOverlongFormCheck;
							}
						}
						goto AfterReadDWord;
					}
					goto ProcessRemainingBytesSlow;
				}

				tempUtf16CodeUnitCountAdjustment--;
				if (Utf8Utility.UInt32ThirdByteIsAscii(thisDWord))
				{
					if (Utf8Utility.UInt32FourthByteIsAscii(thisDWord))
					{
						pInputBuffer += 4;
					}
					else
					{
						pInputBuffer += 3;
						if (pInputBuffer <= pFinalPosWhereCanReadDWordFromInputBuffer)
						{
							thisDWord = Unsafe.ReadUnaligned<uint>(pInputBuffer);
							goto BeforeProcessTwoByteSequence;
						}
					}
				}
				else
				{
					pInputBuffer += 2;
				}

				continue;
			}
			thisDWord -= BitConverter.IsLittleEndian ? 0x0080_00E0u - 0x0000_00C0u : 0xE000_8000u - 0xC000_0000u;
			if ((thisDWord & (BitConverter.IsLittleEndian ? 0x00C0_C0F0u : 0xF0C0_C000u)) == 0)
			{
				ProcessThreeByteSequenceWithCheck:
				if (BitConverter.IsLittleEndian)
				{
					if ((thisDWord & 0x0000_200Fu) == 0 || ((thisDWord - 0x0000_200Du) & 0x0000_200Fu) == 0)
						goto Error;
				}
				else
				{
					if ((thisDWord & 0x0F20_0000u) == 0 || ((thisDWord - 0x0D20_0000u) & 0x0F20_0000u) == 0)
						goto Error;
				}

				ProcessSingleThreeByteSequenceSkipOverlongAndSurrogateChecks:

				IntPtr asciiAdjustment;
				if (BitConverter.IsLittleEndian)
					asciiAdjustment = (Int32)thisDWord >> 31;
				else
					asciiAdjustment = (IntPtr)(SByte)thisDWord >> 7;
				pInputBuffer += 4;
				pInputBuffer += asciiAdjustment;
				tempUtf16CodeUnitCountAdjustment -= 2;

				SuccessfullyProcessedThreeByteSequence:

				if (System.IntPtr.Size >= 8 && BitConverter.IsLittleEndian &&
				    (IntPtr)(pFinalPosWhereCanReadDWordFromInputBuffer - pInputBuffer) >= 5)
				{
					UInt64 thisQWord = Unsafe.ReadUnaligned<UInt64>(pInputBuffer);
					thisDWord = (UInt32)thisQWord;
					if ((thisQWord & 0xC0F0_C0C0_F0C0_C0F0ul) == 0x80E0_8080_E080_80E0ul &&
					    Utf8Utility.IsUtf8ContinuationByte(in pInputBuffer[8]))
					{
						if (((UInt32)thisQWord & 0x200Fu) == 0 || (((UInt32)thisQWord - 0x200Du) & 0x200Fu) == 0)
							goto Error;

						thisQWord >>= 24;
						if (((UInt32)thisQWord & 0x200Fu) == 0 || (((UInt32)thisQWord - 0x200Du) & 0x200Fu) == 0)
							goto ProcessSingleThreeByteSequenceSkipOverlongAndSurrogateChecks;

						thisQWord >>= 24;
						if (((UInt32)thisQWord & 0x200Fu) == 0 || (((UInt32)thisQWord - 0x200Du) & 0x200Fu) == 0)
							goto ProcessSingleThreeByteSequenceSkipOverlongAndSurrogateChecks;

						pInputBuffer += 9;
						tempUtf16CodeUnitCountAdjustment -= 6;

						goto SuccessfullyProcessedThreeByteSequence;
					}
					if ((thisQWord & 0xC0C0_F0C0_C0F0ul) == 0x8080_E080_80E0ul)
					{
						if (((UInt32)thisQWord & 0x200Fu) == 0 || (((UInt32)thisQWord - 0x200Du) & 0x200Fu) == 0)
							goto Error;

						thisQWord >>= 24;
						if (((UInt32)thisQWord & 0x200Fu) == 0 || (((UInt32)thisQWord - 0x200Du) & 0x200Fu) == 0)
							goto ProcessSingleThreeByteSequenceSkipOverlongAndSurrogateChecks;

						pInputBuffer += 6;
						tempUtf16CodeUnitCountAdjustment -= 4;

						continue;
					}

					if (Utf8Utility.UInt32BeginsWithUtf8ThreeByteMask(thisDWord))
						goto ProcessThreeByteSequenceWithCheck;
					goto AfterReadDWord;
				}

				if (pInputBuffer <= pFinalPosWhereCanReadDWordFromInputBuffer)
				{
					thisDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer);
					if (Utf8Utility.UInt32BeginsWithUtf8ThreeByteMask(thisDWord))
						goto ProcessThreeByteSequenceWithCheck;
					goto AfterReadDWord;
				}
				goto ProcessRemainingBytesSlow;
			}
			if (BitConverter.IsLittleEndian)
			{
				thisDWord &= 0xC0C0_FFFFu;
				if ((Int32)thisDWord > unchecked((Int32)0x8000_3FFF)) goto Error;
				thisDWord = BitOperations.RotateRight(thisDWord, 8);
				if (!UnicodeUtility.IsInRangeInclusive(thisDWord, 0x1080_0010u, 0x1480_000Fu))
					goto Error;
			}
			else
			{
				thisDWord -= 0x80u;
				if ((thisDWord & 0x00C0_C0C0u) != 0) goto Error;
				if (!UnicodeUtility.IsInRangeInclusive(thisDWord, 0x1010_0000u, 0x140F_FFFFu))
					goto Error;
			}
			pInputBuffer += 4;
			tempUtf16CodeUnitCountAdjustment -= 2;
			tempScalarCountAdjustment--;
		}

		goto ProcessRemainingBytesSlow;

		ProcessInputOfLessThanDWordSize:
		UIntPtr inputBufferRemainingBytes = (UInt32)inputLength;
		goto ProcessSmallBufferCommon;

		ProcessRemainingBytesSlow:

		inputBufferRemainingBytes =
			(UIntPtr)(void*)Unsafe.ByteOffset(ref *pInputBuffer, ref *pFinalPosWhereCanReadDWordFromInputBuffer) + 4;

		ProcessSmallBufferCommon:
		while (inputBufferRemainingBytes > 0)
		{
			UInt32 firstByte = pInputBuffer[0];

			if ((Byte)firstByte < 0x80u)
			{
				pInputBuffer++;
				inputBufferRemainingBytes--;
				continue;
			}
			if (inputBufferRemainingBytes >= 2)
			{
				UInt32 secondByte = pInputBuffer[1];
				if ((Byte)firstByte < 0xE0u)
				{
					if ((Byte)firstByte >= 0xC2u && Utf8Utility.IsLowByteUtf8ContinuationByte(secondByte))
					{
						pInputBuffer += 2;
						tempUtf16CodeUnitCountAdjustment--;
						inputBufferRemainingBytes -= 2;
						continue;
					}
				}
				else if (inputBufferRemainingBytes >= 3 && (Byte)firstByte < 0xF0u)
				{
					if ((Byte)firstByte == 0xE0u)
					{
						if (!UnicodeUtility.IsInRangeInclusive(secondByte, 0xA0u, 0xBFu))
							goto Error;
					}
					else if ((Byte)firstByte == 0xEDu)
					{
						if (!UnicodeUtility.IsInRangeInclusive(secondByte, 0x80u, 0x9Fu))
							goto Error;
					}
					else
					{
						if (!Utf8Utility.IsLowByteUtf8ContinuationByte(secondByte))
							goto Error;
					}

					if (Utf8Utility.IsUtf8ContinuationByte(in pInputBuffer[2]))
					{
						pInputBuffer += 3;
						tempUtf16CodeUnitCountAdjustment -= 2;
						inputBufferRemainingBytes -= 3;
						continue;
					}
				}
			}
			goto Error;
		}
		Error:
		utf16CodeUnitCountAdjustment = tempUtf16CodeUnitCountAdjustment;
		scalarCountAdjustment = tempScalarCountAdjustment;
		return pInputBuffer;
	}
}
#endif