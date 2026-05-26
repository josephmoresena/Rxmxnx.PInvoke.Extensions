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
// (System.text.Unicode.Utf8)

#if !NETCOREAPP
// ReSharper disable UnusedMethodReturnValue.Global
namespace System.Text.Unicode;

/// <summary>
/// Provides static methods that convert chunked data between UTF-8 and UTF-16 encodings.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal static unsafe class Utf8
{
	/// <summary>
	/// Transcodes the UTF-16 <paramref name="source"/> buffer to <paramref name="destination"/> as UTF-8.
	/// </summary>
	/// <remarks>
	/// If <paramref name="replaceInvalidSequences"/> is <see langword="true"/>, invalid UTF-16 sequences
	/// in <paramref name="source"/> will be replaced with U+FFFD in <paramref name="destination"/>, and
	/// this method will not return <see cref="OperationStatus.InvalidData"/>.
	/// </remarks>
	public static OperationStatus FromUtf16(ReadOnlySpan<Char> source, Span<Byte> destination, out Int32 charsRead,
		out Int32 bytesWritten, Boolean replaceInvalidSequences = true, Boolean isFinalBlock = true)
	{
		fixed (Char* pOriginalSource = &MemoryMarshal.GetReference(source))
		fixed (Byte* pOriginalDestination = &MemoryMarshal.GetReference(destination))
		{
			OperationStatus operationStatus = OperationStatus.Done;
			Char* pInputBufferRemaining = pOriginalSource;
			Byte* pOutputBufferRemaining = pOriginalDestination;
			while (!source.IsEmpty)
			{
				Char* sourcePtr = (Char*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(source));
				Byte* destinationPtr = (Byte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(destination));
				operationStatus = Utf8Utility.TranscodeToUtf8(sourcePtr, source.Length, destinationPtr,
				                                              destination.Length, out pInputBufferRemaining,
				                                              out pOutputBufferRemaining);

				if (operationStatus <= OperationStatus.DestinationTooSmall ||
				    (operationStatus == OperationStatus.NeedMoreData && !isFinalBlock))
					break;

				if (!replaceInvalidSequences)
				{
					operationStatus = OperationStatus.InvalidData;
					break;
				}

				Int32 destinationOffset = (Int32)(pOutputBufferRemaining - destinationPtr);
				destination = destination[destinationOffset..];

				if (destination.Length <= 2)
				{
					operationStatus = OperationStatus.DestinationTooSmall;
					break;
				}

				destination[0] = 0xEF;
				destination[1] = 0xBF;
				destination[2] = 0xBD;
				destination = destination[3..];

				Int32 sourceOffset = (Int32)(pInputBufferRemaining - sourcePtr) + 1;
				source = source[sourceOffset..];

				operationStatus = OperationStatus.Done;
				pInputBufferRemaining = sourcePtr;
				pOutputBufferRemaining = destinationPtr;
			}
			charsRead = (Int32)(pInputBufferRemaining - pOriginalSource);
			bytesWritten = (Int32)(pOutputBufferRemaining - pOriginalDestination);
			return operationStatus;
		}
	}

	/// <summary>
	/// Transcodes the UTF-8 <paramref name="source"/> buffer to <paramref name="destination"/> as UTF-16.
	/// </summary>
	/// <remarks>
	/// If <paramref name="replaceInvalidSequences"/> is <see langword="true"/>, invalid UTF-8 sequences
	/// in <paramref name="source"/> will be replaced with U+FFFD in <paramref name="destination"/>, and
	/// this method will not return <see cref="OperationStatus.InvalidData"/>.
	/// </remarks>
	public static OperationStatus ToUtf16(ReadOnlySpan<Byte> source, Span<Char> destination, out Int32 bytesRead,
		out Int32 charsWritten, Boolean replaceInvalidSequences = true, Boolean isFinalBlock = true)
	{
		fixed (Byte* pOriginalSource = &MemoryMarshal.GetReference(source))
		fixed (Char* pOriginalDestination = &MemoryMarshal.GetReference(destination))
		{
			OperationStatus operationStatus = OperationStatus.Done;
			Byte* pInputBufferRemaining = pOriginalSource;
			Char* pOutputBufferRemaining = pOriginalDestination;

			while (!source.IsEmpty)
			{
				Byte* sourcePtr = (Byte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(source));
				Char* destinationPtr = (Char*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(destination));
				operationStatus = Utf8Utility.TranscodeToUtf16(sourcePtr, source.Length, destinationPtr,
				                                               destination.Length, out pInputBufferRemaining,
				                                               out pOutputBufferRemaining);

				if (operationStatus <= OperationStatus.DestinationTooSmall ||
				    (operationStatus == OperationStatus.NeedMoreData && !isFinalBlock))
					break;

				if (!replaceInvalidSequences)
				{
					operationStatus = OperationStatus.InvalidData;
					break;
				}

				Int32 destinationOffset = (Int32)(pOutputBufferRemaining - destinationPtr);
				destination = destination[destinationOffset..];

				if (destination.IsEmpty)
				{
					operationStatus = OperationStatus.DestinationTooSmall;
					break;
				}

				destination[0] = (Char)UnicodeUtility.ReplacementChar;
				destination = destination[1..];

				Int32 sourceOffset = (Int32)(pInputBufferRemaining - sourcePtr);
				source = source[sourceOffset..];

				Rune.DecodeFromUtf8(source, out _, out Int32 bytesConsumedJustNow);
				source = source[bytesConsumedJustNow..];

				operationStatus = OperationStatus.Done;
				pInputBufferRemaining = sourcePtr;
				pOutputBufferRemaining = destinationPtr;
			}
			bytesRead = (Int32)(pInputBufferRemaining - pOriginalSource);
			charsWritten = (Int32)(pOutputBufferRemaining - pOriginalDestination);
			return operationStatus;
		}
	}
}
#endif