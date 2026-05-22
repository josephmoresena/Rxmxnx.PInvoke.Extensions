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
// (System.ThrowHelper)

#if !NETCOREAPP
namespace System;

#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal static class ThrowHelper
{
	[DoesNotReturn]
	internal static void ThrowArgumentException_DestinationTooShort()
		=> throw new ArgumentException("Destination is too short.", "destination");

	[DoesNotReturn]
	internal static void ThrowArgumentNullException(ExceptionArgument argument)
		=> throw new ArgumentNullException(ThrowHelper.GetArgumentName(argument));

	[DoesNotReturn]
	internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
		=> throw new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(argument));

	private static String GetArgumentName(ExceptionArgument argument)
	{
		switch (argument)
		{
			case ExceptionArgument.ch:
				return nameof(ExceptionArgument.ch);
			case ExceptionArgument.culture:
				return nameof(ExceptionArgument.culture);
			case ExceptionArgument.index:
				return nameof(ExceptionArgument.index);
			case ExceptionArgument.input:
				return nameof(ExceptionArgument.input);
			case ExceptionArgument.value:
				return nameof(ExceptionArgument.value);
			default:
				Debug.Fail("The enum value is not defined, please check the ExceptionArgument Enum.");
				return "";
		}
	}
}

//
// The convention for this enum is using the argument name as the enum name
//
// ReSharper disable InconsistentNaming
internal enum ExceptionArgument
{
	ch,
	culture,
	index,
	input,
	value,
}
#endif