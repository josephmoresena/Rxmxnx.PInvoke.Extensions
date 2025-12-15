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

// Adopted and adapted by Joseph Moreno in 2025 based on code from .NET 10.0 CoreCLR
// (System.Runtime.InteropServices.MemoryMarshal / System.String /  System.SpanHelpers )

#if NETCOREAPP && !NET7_0_OR_GREATER
// ReSharper disable BuiltInTypeReferenceStyle

using UIntPtr = nuint;
using IntPtr = nint;

namespace Rxmxnx.PInvoke.Internal;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3776)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1199)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1066)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS907)]
#endif
internal unsafe partial class Utf8Comparator
{
#pragma warning disable CA2020
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private static Int32 SequenceCompareTo(ref Char first, Int32 firstLength, ref Char second, Int32 secondLength)
	{
		Int32 lengthDelta = firstLength - secondLength;

		if (Unsafe.AreSame(ref first, ref second))
			goto Equal;

		UIntPtr minLength = (UInt32)firstLength < (UInt32)secondLength ? (UInt32)firstLength : (UInt32)secondLength;
		UIntPtr i = 0;

		if (minLength >= (UIntPtr)(sizeof(UIntPtr) / sizeof(Char)))
		{
			if (Vector.IsHardwareAccelerated && minLength >= (UIntPtr)Vector<UInt16>.Count)
			{
				UIntPtr nLength = minLength - (UIntPtr)Vector<UInt16>.Count;
				do
				{
					if (Unsafe.ReadUnaligned<Vector<UInt16>>(
						    ref Unsafe.As<Char, Byte>(ref Unsafe.Add(ref first, (IntPtr)i))) !=
					    Unsafe.ReadUnaligned<Vector<UInt16>>(
						    ref Unsafe.As<Char, Byte>(ref Unsafe.Add(ref second, (IntPtr)i))))
						break;
					i += (UIntPtr)Vector<UInt16>.Count;
				} while (nLength >= i);
			}

			while (minLength >= i + (UIntPtr)(sizeof(UIntPtr) / sizeof(Char)))
			{
				if (Unsafe.ReadUnaligned<UIntPtr>(ref Unsafe.As<Char, Byte>(ref Unsafe.Add(ref first, (IntPtr)i))) !=
				    Unsafe.ReadUnaligned<UIntPtr>(ref Unsafe.As<Char, Byte>(ref Unsafe.Add(ref second, (IntPtr)i))))
					break;
				i += (UIntPtr)(sizeof(UIntPtr) / sizeof(Char));
			}
		}

		if (Environment.Is64BitProcess && minLength >= i + sizeof(Int32) / sizeof(Char))
			if (Unsafe.ReadUnaligned<Int32>(ref Unsafe.As<Char, Byte>(ref Unsafe.Add(ref first, (IntPtr)i))) ==
			    Unsafe.ReadUnaligned<Int32>(ref Unsafe.As<Char, Byte>(ref Unsafe.Add(ref second, (IntPtr)i))))
				i += sizeof(Int32) / sizeof(Char);

		while (i < minLength)
		{
			Int32 result = Unsafe.Add(ref first, (IntPtr)i).CompareTo(Unsafe.Add(ref second, (IntPtr)i));
			if (result != 0)
				return result;
			i += 1;
		}

		Equal:
		return lengthDelta;
	}
#pragma warning restore CA2020
}
#endif