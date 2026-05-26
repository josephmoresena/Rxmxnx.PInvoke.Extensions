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
	public static OperationStatus TranscodeToUtf16(Byte* pInputBuffer, Int32 inputLength, Char* pOutputBuffer,
		Int32 outputCharsRemaining, out Byte* pInputBufferRemaining, out Char* pOutputBufferRemaining)
	{
		UIntPtr numElementsConverted =
			Ascii.WidenAsciiToUtf16(pInputBuffer, pOutputBuffer, (UInt32)Math.Min(inputLength, outputCharsRemaining));

		pInputBuffer += numElementsConverted;
		pOutputBuffer += numElementsConverted;
		if ((Int32)numElementsConverted == inputLength)
		{
			pInputBufferRemaining = pInputBuffer;
			pOutputBufferRemaining = pOutputBuffer;
			return OperationStatus.Done;
		}

		inputLength -= (Int32)numElementsConverted;
		outputCharsRemaining -= (Int32)numElementsConverted;

		if (inputLength < sizeof(UInt32)) goto ProcessInputOfLessThanDWordSize;

		Byte* pFinalPosWhereCanReadDWordFromInputBuffer = pInputBuffer + (UInt32)inputLength - 4;
		do
		{
			UInt32 thisDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer);

			AfterReadDWord:
			if (Ascii.AllBytesInUInt32AreAscii(thisDWord))
			{
				if (outputCharsRemaining < sizeof(UInt32))
					goto ProcessRemainingBytesSlow;
				Ascii.WidenFourAsciiBytesToUtf16AndWriteToBuffer(ref *pOutputBuffer, thisDWord);
				pInputBuffer += 4;
				pOutputBuffer += 4;
				outputCharsRemaining -= 4;

				UInt32 remainingInputBytes =
					(UInt32)(void*)Unsafe.ByteOffset(ref *pInputBuffer,
					                                 ref *pFinalPosWhereCanReadDWordFromInputBuffer) + 4;
				UInt32 maxIters = Math.Min(remainingInputBytes, (UInt32)outputCharsRemaining) / (2 * sizeof(UInt32));
				UInt32 secondDWord;
				Int32 i;
				for (i = 0; (UInt32)i < maxIters; i++)
				{
					thisDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer);
					secondDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer + sizeof(UInt32));

					if (!Ascii.AllBytesInUInt32AreAscii(thisDWord | secondDWord))
						goto LoopTerminatedEarlyDueToNonAsciiData;

					pInputBuffer += 8;

					Ascii.WidenFourAsciiBytesToUtf16AndWriteToBuffer(ref pOutputBuffer[0], thisDWord);
					Ascii.WidenFourAsciiBytesToUtf16AndWriteToBuffer(ref pOutputBuffer[4], secondDWord);

					pOutputBuffer += 8;
				}

				outputCharsRemaining -= 8 * i;

				continue;
				LoopTerminatedEarlyDueToNonAsciiData:

				if (Ascii.AllBytesInUInt32AreAscii(thisDWord))
				{
					Ascii.WidenFourAsciiBytesToUtf16AndWriteToBuffer(ref *pOutputBuffer, thisDWord);
					thisDWord = secondDWord;

					pInputBuffer += 4;
					pOutputBuffer += 4;
					outputCharsRemaining -= 4;
				}

				outputCharsRemaining -= 8 * i;
			}

			if (Utf8Utility.UInt32FirstByteIsAscii(thisDWord))
			{
				if (outputCharsRemaining >= 3)
				{
					UInt32 thisDWordLittleEndian = Utf8Utility.ToLittleEndian(thisDWord);
					UIntPtr adjustment = 1;
					pOutputBuffer[0] = (Char)(Byte)thisDWordLittleEndian;

					if (Utf8Utility.UInt32SecondByteIsAscii(thisDWord))
					{
						adjustment++;
						thisDWordLittleEndian >>= 8;
						pOutputBuffer[1] = (Char)(Byte)thisDWordLittleEndian;

						if (Utf8Utility.UInt32ThirdByteIsAscii(thisDWord))
						{
							adjustment++;
							thisDWordLittleEndian >>= 8;
							pOutputBuffer[2] = (Char)(Byte)thisDWordLittleEndian;
						}
					}

					pInputBuffer += adjustment;
					pOutputBuffer += adjustment;
					outputCharsRemaining -= (int)adjustment;
				}
				else
				{
					if (outputCharsRemaining == 0) goto OutputBufferTooSmall;

					UInt32 thisDWordLittleEndian = Utf8Utility.ToLittleEndian(thisDWord);

					pInputBuffer++;
					*pOutputBuffer++ = (Char)(Byte)thisDWordLittleEndian;
					outputCharsRemaining--;

					if (Utf8Utility.UInt32SecondByteIsAscii(thisDWord))
					{
						if (outputCharsRemaining == 0) goto OutputBufferTooSmall;

						pInputBuffer++;
						thisDWordLittleEndian >>= 8;
						*pOutputBuffer++ = (Char)(Byte)thisDWordLittleEndian;

						if (Utf8Utility.UInt32ThirdByteIsAscii(thisDWord)) goto OutputBufferTooSmall;
						outputCharsRemaining = 0;
					}
				}

				if (pInputBuffer > pFinalPosWhereCanReadDWordFromInputBuffer)
					goto ProcessRemainingBytesSlow;
				thisDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer);
			}

			BeforeProcessTwoByteSequence:
			if (Utf8Utility.UInt32BeginsWithUtf8TwoByteMask(thisDWord))
			{
				if (Utf8Utility.UInt32BeginsWithOverlongUtf8TwoByteSequence(thisDWord)) goto Error;

				ProcessTwoByteSequenceSkipOverlongFormCheck:
				if ((BitConverter.IsLittleEndian &&
					    Utf8Utility.UInt32EndsWithValidUtf8TwoByteSequenceLittleEndian(thisDWord)) ||
				    (!BitConverter.IsLittleEndian && Utf8Utility.UInt32EndsWithUtf8TwoByteMask(thisDWord) &&
					    !Utf8Utility.UInt32EndsWithOverlongUtf8TwoByteSequence(thisDWord)))
				{
					if (outputCharsRemaining < 2) goto ProcessRemainingBytesSlow;

					Unsafe.WriteUnaligned(pOutputBuffer,
					                      Utf8Utility.ExtractTwoCharsPackedFromTwoAdjacentTwoByteSequences(thisDWord));

					pInputBuffer += 4;
					pOutputBuffer += 2;
					outputCharsRemaining -= 2;

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

				UInt32 charToWrite = Utf8Utility.ExtractCharFromFirstTwoByteSequence(thisDWord);
				if (Utf8Utility.UInt32ThirdByteIsAscii(thisDWord))
				{
					if (Utf8Utility.UInt32FourthByteIsAscii(thisDWord))
					{
						if (outputCharsRemaining < 3) goto ProcessRemainingBytesSlow;
						pOutputBuffer[0] = (Char)charToWrite;
						if (BitConverter.IsLittleEndian)
						{
							thisDWord >>= 16;
							pOutputBuffer[1] = (Char)(Byte)thisDWord;
							thisDWord >>= 8;
							pOutputBuffer[2] = (Char)thisDWord;
						}
						else
						{
							pOutputBuffer[2] = (Char)(Byte)thisDWord;
							pOutputBuffer[1] = (Char)(Byte)(thisDWord >> 8);
						}
						pInputBuffer += 4;
						pOutputBuffer += 3;
						outputCharsRemaining -= 3;

						continue;
					}
					if (outputCharsRemaining < 2) goto ProcessRemainingBytesSlow;

					pOutputBuffer[0] = (Char)charToWrite;
					pOutputBuffer[1] = (Char)(Byte)(thisDWord >> (BitConverter.IsLittleEndian ? 16 : 8));
					pInputBuffer += 3;
					pOutputBuffer += 2;
					outputCharsRemaining -= 2;

					if (pFinalPosWhereCanReadDWordFromInputBuffer < pInputBuffer)
						goto ProcessRemainingBytesSlow;
					thisDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer);
					goto BeforeProcessTwoByteSequence;
				}
				if (outputCharsRemaining == 0) goto ProcessRemainingBytesSlow;

				pOutputBuffer[0] = (Char)charToWrite;
				pInputBuffer += 2;
				pOutputBuffer++;
				outputCharsRemaining--;

				if (pFinalPosWhereCanReadDWordFromInputBuffer < pInputBuffer)
					goto ProcessRemainingBytesSlow;
				thisDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer);
			}

			if (Utf8Utility.UInt32BeginsWithUtf8ThreeByteMask(thisDWord))
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
				if (outputCharsRemaining == 0)
					goto OutputBufferTooSmall;
				if (BitConverter.IsLittleEndian && ((thisDWord - 0xE000_0000u) & 0xF000_0000u) == 0 &&
				    outputCharsRemaining > 1 &&
				    (IntPtr)(void*)Unsafe.ByteOffset(ref *pInputBuffer,
				                                     ref *pFinalPosWhereCanReadDWordFromInputBuffer) >= 3)
				{
					UInt32 secondDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer + 3);

					if (Utf8Utility.UInt32BeginsWithUtf8ThreeByteMask(secondDWord) &&
					    (secondDWord & 0x0000_200Fu) != 0 && ((secondDWord - 0x0000_200Du) & 0x0000_200Fu) != 0)
					{
						pOutputBuffer[0] = (Char)Utf8Utility.ExtractCharFromFirstThreeByteSequence(thisDWord);
						pOutputBuffer[1] = (Char)Utf8Utility.ExtractCharFromFirstThreeByteSequence(secondDWord);
						pInputBuffer += 6;
						pOutputBuffer += 2;
						outputCharsRemaining -= 2;

						goto CheckForAsciiByteAfterThreeByteSequence;
					}
				}

				*pOutputBuffer = (Char)Utf8Utility.ExtractCharFromFirstThreeByteSequence(thisDWord);
				pInputBuffer += 3;
				pOutputBuffer++;
				outputCharsRemaining--;

				CheckForAsciiByteAfterThreeByteSequence:
				if (Utf8Utility.UInt32FourthByteIsAscii(thisDWord))
				{
					if (outputCharsRemaining == 0) goto OutputBufferTooSmall;

					if (BitConverter.IsLittleEndian)
						*pOutputBuffer = (Char)(thisDWord >> 24);
					else
						*pOutputBuffer = (Char)(Byte)thisDWord;

					pInputBuffer++;
					pOutputBuffer++;
					outputCharsRemaining--;
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

			if (!Utf8Utility.UInt32BeginsWithUtf8FourByteMask(thisDWord)) goto Error;
			if (BitConverter.IsLittleEndian)
			{
				UInt32 toCheck = thisDWord & 0x0000_FFFFu;
				toCheck = BitOperations.RotateRight(toCheck, 8);
				if (!UnicodeUtility.IsInRangeInclusive(toCheck, 0xF000_0090u, 0xF400_008Fu)) goto Error;
			}
			else
			{
				if (!UnicodeUtility.IsInRangeInclusive(thisDWord, 0xF090_0000u, 0xF48F_FFFFu)) goto Error;
			}
			if (outputCharsRemaining < 2)
				goto OutputBufferTooSmall;

			Unsafe.WriteUnaligned(pOutputBuffer, Utf8Utility.ExtractCharsFromFourByteSequence(thisDWord));

			pInputBuffer += 4;
			pOutputBuffer += 2;
			outputCharsRemaining -= 2;
		} while (pInputBuffer <= pFinalPosWhereCanReadDWordFromInputBuffer);

		ProcessRemainingBytesSlow:
		inputLength =
			(Int32)(void*)Unsafe.ByteOffset(ref *pInputBuffer, ref *pFinalPosWhereCanReadDWordFromInputBuffer) + 4;

		ProcessInputOfLessThanDWordSize:
		while (inputLength > 0)
		{
			UInt32 firstByte = pInputBuffer[0];
			if (firstByte <= 0x7Fu)
			{
				if (outputCharsRemaining == 0)
					goto OutputBufferTooSmall;

				*pOutputBuffer = (Char)firstByte;

				pInputBuffer++;
				pOutputBuffer++;
				inputLength--;
				outputCharsRemaining--;
				continue;
			}
			firstByte -= 0xC2u;
			if ((Byte)firstByte <= 0xDFu - 0xC2u)
			{
				if (inputLength < 2) goto InputBufferTooSmall;
				UInt32 secondByte = pInputBuffer[1];
				if (!Utf8Utility.IsLowByteUtf8ContinuationByte(secondByte))
					goto Error;
				if (outputCharsRemaining == 0)
					goto OutputBufferTooSmall;
				UInt32 asChar = (firstByte << 6) + secondByte + ((0xC2u - 0xC0u) << 6) - 0x80u;
				*pOutputBuffer = (Char)asChar;

				pInputBuffer += 2;
				pOutputBuffer++;
				inputLength -= 2;
				outputCharsRemaining--;
			}
			else if ((Byte)firstByte <= 0xEFu - 0xC2u)
			{
				switch (inputLength)
				{
					case >= 3:
					{
						UInt32 secondByte = pInputBuffer[1];
						UInt32 thirdByte = pInputBuffer[2];
						if (!Utf8Utility.IsLowByteUtf8ContinuationByte(secondByte) ||
						    !Utf8Utility.IsLowByteUtf8ContinuationByte(thirdByte))
							goto Error;

						UInt32 partialChar = (firstByte << 12) + (secondByte << 6);
						if (partialChar < ((0xE0u - 0xC2u) << 12) + (0xA0u << 6))
							goto Error;

						partialChar -= ((0xEDu - 0xC2u) << 12) + (0xA0u << 6);
						if (partialChar < 0x0800u) goto Error;

						if (outputCharsRemaining == 0)
							goto OutputBufferTooSmall;
						partialChar += thirdByte;
						partialChar += 0xD800;
						partialChar -= 0x80u;

						*pOutputBuffer = (Char)partialChar;

						pInputBuffer += 3;
						pOutputBuffer++;
						inputLength -= 3;
						outputCharsRemaining--;
						continue;
					}
					case >= 2:
					{
						UInt32 secondByte = pInputBuffer[1];
						if (!Utf8Utility.IsLowByteUtf8ContinuationByte(secondByte))
							goto Error;
						UInt32 partialChar = (firstByte << 6) + secondByte;
						if (partialChar < ((0xE0u - 0xC2u) << 6) + 0xA0u) goto Error;
						if (UnicodeUtility.IsInRangeInclusive(partialChar, ((0xEDu - 0xC2u) << 6) + 0xA0u,
						                                      ((0xEEu - 0xC2u) << 6) + 0x7Fu))
							goto Error;
						break;
					}
				}

				goto InputBufferTooSmall;
			}
			else if ((Byte)firstByte <= 0xF4u - 0xC2u)
			{
				// Potentially a 4-byte sequence?

				if (inputLength < 2) goto InputBufferTooSmall;

				UInt32 nextByte = pInputBuffer[1];
				if (!Utf8Utility.IsLowByteUtf8ContinuationByte(nextByte))
					goto Error;

				UInt32 asPartialChar = (firstByte << 6) + nextByte;
				if (!UnicodeUtility.IsInRangeInclusive(asPartialChar, ((0xF0u - 0xC2u) << 6) + 0x90u,
				                                       ((0xF4u - 0xC2u) << 6) + 0x8Fu))
					goto Error;

				if (inputLength < 3) goto InputBufferTooSmall;

				if (!Utf8Utility.IsLowByteUtf8ContinuationByte(pInputBuffer[2]))
					goto Error;

				if (inputLength < 4) goto InputBufferTooSmall;

				if (!Utf8Utility.IsLowByteUtf8ContinuationByte(pInputBuffer[3]))
					goto Error;
				goto OutputBufferTooSmall;
			}
			else
			{
				goto Error;
			}
		}

		OperationStatus retVal = OperationStatus.Done;
		goto ReturnCommon;

		InputBufferTooSmall:
		retVal = OperationStatus.NeedMoreData;
		goto ReturnCommon;

		OutputBufferTooSmall:
		retVal = OperationStatus.DestinationTooSmall;
		goto ReturnCommon;

		Error:
		retVal = OperationStatus.InvalidData;

		ReturnCommon:
		pInputBufferRemaining = pInputBuffer;
		pOutputBufferRemaining = pOutputBuffer;
		return retVal;
	}

	// On method return, pInputBufferRemaining and pOutputBufferRemaining will both point to where
	// the next char would have been consumed from / the next byte would have been written to.
	// inputLength in chars, outputBytesRemaining in bytes.
	public static OperationStatus TranscodeToUtf8(Char* pInputBuffer, Int32 inputLength, Byte* pOutputBuffer,
		Int32 outputBytesRemaining, out Char* pInputBufferRemaining, out Byte* pOutputBufferRemaining)
	{
		const Int32 charsPerDWord = sizeof(UInt32) / sizeof(Char);
		UIntPtr numElementsConverted =
			Ascii.NarrowUtf16ToAscii(pInputBuffer, pOutputBuffer, (UInt32)Math.Min(inputLength, outputBytesRemaining));

		pInputBuffer += numElementsConverted;
		pOutputBuffer += numElementsConverted;

		if ((Int32)numElementsConverted == inputLength)
		{
			pInputBufferRemaining = pInputBuffer;
			pOutputBufferRemaining = pOutputBuffer;
			return OperationStatus.Done;
		}

		inputLength -= (Int32)numElementsConverted;
		outputBytesRemaining -= (Int32)numElementsConverted;

		if (inputLength < charsPerDWord) goto ProcessInputOfLessThanDWordSize;

		Char* pFinalPosWhereCanReadDWordFromInputBuffer = pInputBuffer + (UInt32)inputLength - charsPerDWord;
		UInt32 thisDWord;
		do
		{
			thisDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer);
			AfterReadDWord:
			if (Utf16Utility.AllCharsInUInt32AreAscii(thisDWord))
			{
				if (outputBytesRemaining < 2)
					goto ProcessOneCharFromCurrentDWordAndFinish;
				UInt32 valueToWrite = thisDWord | (thisDWord >> 8);

				Unsafe.WriteUnaligned(pOutputBuffer, (UInt16)valueToWrite);

				pInputBuffer += 2;
				pOutputBuffer += 2;
				outputBytesRemaining -= 2;
				UInt32 inputCharsRemaining = (UInt32)(pFinalPosWhereCanReadDWordFromInputBuffer - pInputBuffer) + 2;
				UInt32 minElementsRemaining = (UInt32)Math.Min(inputCharsRemaining, outputBytesRemaining);

				UInt32 maxIters = minElementsRemaining / 4;
				UInt32 secondDWord;
				Int32 i;
				for (i = 0; (UInt32)i < maxIters; i++)
				{
					thisDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer);
					secondDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer + 2);

					if (!Utf16Utility.AllCharsInUInt32AreAscii(thisDWord | secondDWord))
						goto LoopTerminatedDueToNonAsciiData;
					Unsafe.WriteUnaligned(pOutputBuffer, (UInt16)(thisDWord | (thisDWord >> 8)));
					Unsafe.WriteUnaligned(pOutputBuffer + 2, (UInt16)(secondDWord | (secondDWord >> 8)));

					pInputBuffer += 4;
					pOutputBuffer += 4;
				}

				outputBytesRemaining -= 4 * i;

				continue;

				LoopTerminatedDueToNonAsciiData:

				outputBytesRemaining -= 4 * i;
				if (Utf16Utility.AllCharsInUInt32AreAscii(thisDWord))
				{
					Unsafe.WriteUnaligned(pOutputBuffer, (UInt16)(thisDWord | (thisDWord >> 8)));
					pInputBuffer += 2;
					pOutputBuffer += 2;
					outputBytesRemaining -= 2;
					thisDWord = secondDWord;
				}
			}

			AfterReadDWordSkipAllCharsAsciiCheck:
			if (Utf8Utility.IsFirstCharAscii(thisDWord))
			{
				if (outputBytesRemaining == 0) goto OutputBufferTooSmall;

				if (BitConverter.IsLittleEndian)
					pOutputBuffer[0] = (Byte)thisDWord;
				else
					pOutputBuffer[0] = (Byte)(thisDWord >> 16);

				pInputBuffer++;
				pOutputBuffer++;
				outputBytesRemaining--;

				if (pInputBuffer > pFinalPosWhereCanReadDWordFromInputBuffer)
					goto ProcessNextCharAndFinish;
				thisDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer);
			}
			if (!Utf8Utility.IsFirstCharAtLeastThreeUtf8Bytes(thisDWord))
			{
				TryConsumeMultipleTwoByteSequences:

				if (Utf8Utility.IsSecondCharTwoUtf8Bytes(thisDWord))
				{
					if (outputBytesRemaining < 4)
						goto ProcessOneCharFromCurrentDWordAndFinish;
					Unsafe.WriteUnaligned(pOutputBuffer,
					                      Utf8Utility.ExtractTwoUtf8TwoByteSequencesFromTwoPackedUtf16Chars(thisDWord));

					pInputBuffer += 2;
					pOutputBuffer += 4;
					outputBytesRemaining -= 4;

					if (pInputBuffer > pFinalPosWhereCanReadDWordFromInputBuffer)
						goto ProcessNextCharAndFinish;
					thisDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer);

					if (Utf8Utility.IsFirstCharTwoUtf8Bytes(thisDWord))
						goto TryConsumeMultipleTwoByteSequences;
					goto AfterReadDWord;
				}

				if (outputBytesRemaining < 2) goto OutputBufferTooSmall;

				Unsafe.WriteUnaligned(pOutputBuffer,
				                      (UInt16)Utf8Utility.ExtractUtf8TwoByteSequenceFromFirstUtf16Char(thisDWord));

				if (Utf8Utility.IsSecondCharAscii(thisDWord))
				{
					if (outputBytesRemaining >= 3)
					{
						if (BitConverter.IsLittleEndian) thisDWord >>= 16;
						pOutputBuffer[2] = (Byte)thisDWord;

						pInputBuffer += 2;
						pOutputBuffer += 3;
						outputBytesRemaining -= 3;

						continue;
					}
					pInputBuffer++;
					pOutputBuffer += 2;
					goto OutputBufferTooSmall;
				}
				pInputBuffer++;
				pOutputBuffer += 2;
				outputBytesRemaining -= 2;

				if (pInputBuffer > pFinalPosWhereCanReadDWordFromInputBuffer)
					goto ProcessNextCharAndFinish;
				thisDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer);
			}
			BeforeProcessThreeByteSequence:

			if (!Utf8Utility.IsFirstCharSurrogate(thisDWord))
			{
				if (Utf8Utility.IsSecondCharAtLeastThreeUtf8Bytes(thisDWord) &&
				    !Utf8Utility.IsSecondCharSurrogate(thisDWord))
				{
					if (outputBytesRemaining < 6)
						goto ConsumeSingleThreeByteRun;

					Utf8Utility.WriteTwoUtf16CharsAsTwoUtf8ThreeByteSequences(ref *pOutputBuffer, thisDWord);

					pInputBuffer += 2;
					pOutputBuffer += 6;
					outputBytesRemaining -= 6;
					if (pInputBuffer > pFinalPosWhereCanReadDWordFromInputBuffer)
						goto ProcessNextCharAndFinish;
					thisDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer);

					if (Utf8Utility.IsFirstCharAtLeastThreeUtf8Bytes(thisDWord))
						goto BeforeProcessThreeByteSequence;
					goto AfterReadDWord;
				}

				ConsumeSingleThreeByteRun:

				if (outputBytesRemaining < 3) goto OutputBufferTooSmall;

				Utf8Utility.WriteFirstUtf16CharAsUtf8ThreeByteSequence(ref *pOutputBuffer, thisDWord);

				pInputBuffer++;
				pOutputBuffer += 3;
				outputBytesRemaining -= 3;

				if (Utf8Utility.IsSecondCharAscii(thisDWord))
				{
					if (outputBytesRemaining == 0) goto OutputBufferTooSmall;

					if (BitConverter.IsLittleEndian)
						*pOutputBuffer = (Byte)(thisDWord >> 16);
					else
						*pOutputBuffer = (Byte)thisDWord;

					pInputBuffer++;
					pOutputBuffer++;
					outputBytesRemaining--;

					if (pInputBuffer > pFinalPosWhereCanReadDWordFromInputBuffer)
						goto ProcessNextCharAndFinish;
					thisDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer);

					if (Utf8Utility.IsFirstCharAtLeastThreeUtf8Bytes(thisDWord)) goto BeforeProcessThreeByteSequence;
					goto AfterReadDWord;
				}

				if (pInputBuffer > pFinalPosWhereCanReadDWordFromInputBuffer)
					goto ProcessNextCharAndFinish;
				thisDWord = Unsafe.ReadUnaligned<UInt32>(pInputBuffer);
				goto AfterReadDWordSkipAllCharsAsciiCheck;
			}
			if (Utf8Utility.IsWellFormedUtf16SurrogatePair(thisDWord))
			{
				if (outputBytesRemaining < 4) goto OutputBufferTooSmall;

				Unsafe.WriteUnaligned(pOutputBuffer, Utf8Utility.ExtractFourUtf8BytesFromSurrogatePair(thisDWord));

				pInputBuffer += 2;
				pOutputBuffer += 4;
				outputBytesRemaining -= 4;

				continue;
			}

			goto Error;
		} while (pInputBuffer <= pFinalPosWhereCanReadDWordFromInputBuffer);

		ProcessNextCharAndFinish:
		inputLength = (Int32)(pFinalPosWhereCanReadDWordFromInputBuffer - pInputBuffer) + charsPerDWord;

		ProcessInputOfLessThanDWordSize:
		if (inputLength == 0) goto InputBufferFullyConsumed;

		UInt32 thisChar = *pInputBuffer;
		goto ProcessFinalChar;

		ProcessOneCharFromCurrentDWordAndFinish:
		if (BitConverter.IsLittleEndian)
			thisChar = thisDWord & 0xFFFFu;
		else
			thisChar = thisDWord >> 16;

		ProcessFinalChar:
		{
			switch (thisChar)
			{
				case <= 0x7Fu:
				{
					if (outputBytesRemaining == 0)
						goto OutputBufferTooSmall;

					*pOutputBuffer = (Byte)thisChar;

					pInputBuffer++;
					pOutputBuffer++;
					break;
				}
				case < 0x0800u:
				{
					if (outputBytesRemaining < 2)
						goto OutputBufferTooSmall;
					pOutputBuffer[1] = (Byte)((thisChar & 0x3Fu) | unchecked((UInt32)(SByte)0x80));
					pOutputBuffer[0] = (Byte)((thisChar >> 6) | unchecked((UInt32)(SByte)0xC0));

					pInputBuffer++;
					pOutputBuffer += 2;
					break;
				}
				default:
				{
					if (!UnicodeUtility.IsSurrogateCodePoint(thisChar))
					{
						if (outputBytesRemaining < 3)
							goto OutputBufferTooSmall;

						pOutputBuffer[2] = (Byte)((thisChar & 0x3Fu) | unchecked((UInt32)(SByte)0x80));
						pOutputBuffer[1] = (Byte)(((thisChar >> 6) & 0x3Fu) | unchecked((UInt32)(SByte)0x80));
						pOutputBuffer[0] = (Byte)((thisChar >> 12) | unchecked((UInt32)(SByte)0xE0));

						pInputBuffer++;
						pOutputBuffer += 3;
					}
					else if (thisChar <= 0xDBFFu)
					{
						goto InputBufferTooSmall;
					}
					else
					{
						goto Error;
					}
					break;
				}
			}
		}
		if (inputLength > 1) goto OutputBufferTooSmall;

		InputBufferFullyConsumed:
		OperationStatus retVal = OperationStatus.Done;
		goto ReturnCommon;

		InputBufferTooSmall:
		retVal = OperationStatus.NeedMoreData;
		goto ReturnCommon;

		OutputBufferTooSmall:
		retVal = OperationStatus.DestinationTooSmall;
		goto ReturnCommon;

		Error:
		retVal = OperationStatus.InvalidData;

		ReturnCommon:
		pInputBufferRemaining = pInputBuffer;
		pOutputBufferRemaining = pOutputBuffer;
		return retVal;
	}
}
#endif