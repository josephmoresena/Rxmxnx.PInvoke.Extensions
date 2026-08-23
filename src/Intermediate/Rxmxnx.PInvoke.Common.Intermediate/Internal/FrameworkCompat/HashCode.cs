// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/*

The xxHash32 implementation is based on the code published by Yann Collet:
https://raw.githubusercontent.com/Cyan4973/xxHash/5c174cfa4e45a42f94082dc0d4539b39696afea1/xxhash.c

  xxHash - Fast Hash algorithm
  Copyright (C) 2012-2016, Yann Collet

  BSD 2-Clause License (http://www.opensource.org/licenses/bsd-license.php)

  Redistribution and use in source and binary forms, with or without
  modification, are permitted provided that the following conditions are
  met:

  * Redistributions of source code must retain the above copyright
  notice, this list of conditions and the following disclaimer.
  * Redistributions in binary form must reproduce the above
  copyright notice, this list of conditions and the following disclaimer
  in the documentation and/or other materials provided with the
  distribution.

  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
  OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
  SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
  LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
  THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
  (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
  OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

  You can contact the author at :
  - xxHash homepage: http://www.xxhash.com
  - xxHash source repository : https://github.com/Cyan4973/xxHash

*/

// Adopted and adapted by Joseph Moreno in 2026 based on code from Microsoft.Blc.HashCode 1.0.0
// (System.HashCode)

#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER && !NET462_OR_GREATER && !UAP10_0_16299
namespace System;
// xxHash32 is used for the hash code.
// https://github.com/Cyan4973/xxHash

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS107)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2436)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3877)]
#endif
internal struct HashCode
{
	private static readonly UInt32 sSeed = HashCode.GenerateGlobalSeed();

	private const UInt32 prime1 = 2654435761U;
	private const UInt32 prime2 = 2246822519U;
	private const UInt32 prime3 = 3266489917U;
	private const UInt32 prime4 = 668265263U;
	private const UInt32 prime5 = 374761393U;

	private UInt32 _v1, _v2, _v3, _v4;
	private UInt32 _queue1, _queue2, _queue3;
	private UInt32 _length;

	private static UInt32 GenerateGlobalSeed()
	{
		Byte[] bytes = Guid.NewGuid().ToByteArray();
		return BitConverter.ToUInt32(bytes, 0);
	}

	public static Int32 Combine<T1>(T1 value1)
	{
		// Provide a way of diffusing bits from something with a limited
		// input hash space. For example, many enums only have a few
		// possible hashes, only using the bottom few bits of the code. Some
		// collections are built on the assumption that hashes are spread
		// over a larger space, so diffusing the bits may help the
		// collection work more efficiently.

		UInt32 hc1 = (UInt32)(value1?.GetHashCode() ?? 0);

		UInt32 hash = HashCode.MixEmptyState();
		hash += 4;

		hash = HashCode.QueueRound(hash, hc1);

		hash = HashCode.MixFinal(hash);
		return (Int32)hash;
	}

	public static Int32 Combine<T1, T2>(T1 value1, T2 value2)
	{
		UInt32 hc1 = (UInt32)(value1?.GetHashCode() ?? 0);
		UInt32 hc2 = (UInt32)(value2?.GetHashCode() ?? 0);

		UInt32 hash = HashCode.MixEmptyState();
		hash += 8;

		hash = HashCode.QueueRound(hash, hc1);
		hash = HashCode.QueueRound(hash, hc2);

		hash = HashCode.MixFinal(hash);
		return (Int32)hash;
	}

	public static Int32 Combine<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
	{
		UInt32 hc1 = (UInt32)(value1?.GetHashCode() ?? 0);
		UInt32 hc2 = (UInt32)(value2?.GetHashCode() ?? 0);
		UInt32 hc3 = (UInt32)(value3?.GetHashCode() ?? 0);

		UInt32 hash = HashCode.MixEmptyState();
		hash += 12;

		hash = HashCode.QueueRound(hash, hc1);
		hash = HashCode.QueueRound(hash, hc2);
		hash = HashCode.QueueRound(hash, hc3);

		hash = HashCode.MixFinal(hash);
		return (Int32)hash;
	}

	public static Int32 Combine<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
	{
		UInt32 hc1 = (UInt32)(value1?.GetHashCode() ?? 0);
		UInt32 hc2 = (UInt32)(value2?.GetHashCode() ?? 0);
		UInt32 hc3 = (UInt32)(value3?.GetHashCode() ?? 0);
		UInt32 hc4 = (UInt32)(value4?.GetHashCode() ?? 0);

		HashCode.Initialize(out UInt32 v1, out UInt32 v2, out UInt32 v3, out UInt32 v4);

		v1 = HashCode.Round(v1, hc1);
		v2 = HashCode.Round(v2, hc2);
		v3 = HashCode.Round(v3, hc3);
		v4 = HashCode.Round(v4, hc4);

		UInt32 hash = HashCode.MixState(v1, v2, v3, v4);
		hash += 16;

		hash = HashCode.MixFinal(hash);
		return (Int32)hash;
	}

	public static Int32 Combine<T1, T2, T3, T4, T5>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
	{
		UInt32 hc1 = (UInt32)(value1?.GetHashCode() ?? 0);
		UInt32 hc2 = (UInt32)(value2?.GetHashCode() ?? 0);
		UInt32 hc3 = (UInt32)(value3?.GetHashCode() ?? 0);
		UInt32 hc4 = (UInt32)(value4?.GetHashCode() ?? 0);
		UInt32 hc5 = (UInt32)(value5?.GetHashCode() ?? 0);

		HashCode.Initialize(out UInt32 v1, out UInt32 v2, out UInt32 v3, out UInt32 v4);

		v1 = HashCode.Round(v1, hc1);
		v2 = HashCode.Round(v2, hc2);
		v3 = HashCode.Round(v3, hc3);
		v4 = HashCode.Round(v4, hc4);

		UInt32 hash = HashCode.MixState(v1, v2, v3, v4);
		hash += 20;

		hash = HashCode.QueueRound(hash, hc5);

		hash = HashCode.MixFinal(hash);
		return (Int32)hash;
	}

	public static Int32 Combine<T1, T2, T3, T4, T5, T6>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5,
		T6 value6)
	{
		UInt32 hc1 = (UInt32)(value1?.GetHashCode() ?? 0);
		UInt32 hc2 = (UInt32)(value2?.GetHashCode() ?? 0);
		UInt32 hc3 = (UInt32)(value3?.GetHashCode() ?? 0);
		UInt32 hc4 = (UInt32)(value4?.GetHashCode() ?? 0);
		UInt32 hc5 = (UInt32)(value5?.GetHashCode() ?? 0);
		UInt32 hc6 = (UInt32)(value6?.GetHashCode() ?? 0);

		HashCode.Initialize(out UInt32 v1, out UInt32 v2, out UInt32 v3, out UInt32 v4);

		v1 = HashCode.Round(v1, hc1);
		v2 = HashCode.Round(v2, hc2);
		v3 = HashCode.Round(v3, hc3);
		v4 = HashCode.Round(v4, hc4);

		UInt32 hash = HashCode.MixState(v1, v2, v3, v4);
		hash += 24;

		hash = HashCode.QueueRound(hash, hc5);
		hash = HashCode.QueueRound(hash, hc6);

		hash = HashCode.MixFinal(hash);
		return (Int32)hash;
	}

	public static Int32 Combine<T1, T2, T3, T4, T5, T6, T7>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5,
		T6 value6, T7 value7)
	{
		UInt32 hc1 = (UInt32)(value1?.GetHashCode() ?? 0);
		UInt32 hc2 = (UInt32)(value2?.GetHashCode() ?? 0);
		UInt32 hc3 = (UInt32)(value3?.GetHashCode() ?? 0);
		UInt32 hc4 = (UInt32)(value4?.GetHashCode() ?? 0);
		UInt32 hc5 = (UInt32)(value5?.GetHashCode() ?? 0);
		UInt32 hc6 = (UInt32)(value6?.GetHashCode() ?? 0);
		UInt32 hc7 = (UInt32)(value7?.GetHashCode() ?? 0);

		HashCode.Initialize(out UInt32 v1, out UInt32 v2, out UInt32 v3, out UInt32 v4);

		v1 = HashCode.Round(v1, hc1);
		v2 = HashCode.Round(v2, hc2);
		v3 = HashCode.Round(v3, hc3);
		v4 = HashCode.Round(v4, hc4);

		UInt32 hash = HashCode.MixState(v1, v2, v3, v4);
		hash += 28;

		hash = HashCode.QueueRound(hash, hc5);
		hash = HashCode.QueueRound(hash, hc6);
		hash = HashCode.QueueRound(hash, hc7);

		hash = HashCode.MixFinal(hash);
		return (Int32)hash;
	}

	public static Int32 Combine<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5,
		T6 value6, T7 value7, T8 value8)
	{
		UInt32 hc1 = (UInt32)(value1?.GetHashCode() ?? 0);
		UInt32 hc2 = (UInt32)(value2?.GetHashCode() ?? 0);
		UInt32 hc3 = (UInt32)(value3?.GetHashCode() ?? 0);
		UInt32 hc4 = (UInt32)(value4?.GetHashCode() ?? 0);
		UInt32 hc5 = (UInt32)(value5?.GetHashCode() ?? 0);
		UInt32 hc6 = (UInt32)(value6?.GetHashCode() ?? 0);
		UInt32 hc7 = (UInt32)(value7?.GetHashCode() ?? 0);
		UInt32 hc8 = (UInt32)(value8?.GetHashCode() ?? 0);

		HashCode.Initialize(out UInt32 v1, out UInt32 v2, out UInt32 v3, out UInt32 v4);

		v1 = HashCode.Round(v1, hc1);
		v2 = HashCode.Round(v2, hc2);
		v3 = HashCode.Round(v3, hc3);
		v4 = HashCode.Round(v4, hc4);

		v1 = HashCode.Round(v1, hc5);
		v2 = HashCode.Round(v2, hc6);
		v3 = HashCode.Round(v3, hc7);
		v4 = HashCode.Round(v4, hc8);

		UInt32 hash = HashCode.MixState(v1, v2, v3, v4);
		hash += 32;

		hash = HashCode.MixFinal(hash);
		return (Int32)hash;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void Initialize(out UInt32 v1, out UInt32 v2, out UInt32 v3, out UInt32 v4)
	{
		v1 = HashCode.sSeed + HashCode.prime1 + HashCode.prime2;
		v2 = HashCode.sSeed + HashCode.prime2;
		v3 = HashCode.sSeed;
		v4 = HashCode.sSeed - HashCode.prime1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UInt32 Round(UInt32 hash, UInt32 input)
		=> BitOperations.RotateLeft(hash + input * HashCode.prime2, 13) * HashCode.prime1;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UInt32 QueueRound(UInt32 hash, UInt32 queuedValue)
		=> BitOperations.RotateLeft(hash + queuedValue * HashCode.prime3, 17) * HashCode.prime4;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UInt32 MixState(UInt32 v1, UInt32 v2, UInt32 v3, UInt32 v4)
		=> BitOperations.RotateLeft(v1, 1) + BitOperations.RotateLeft(v2, 7) + BitOperations.RotateLeft(v3, 12) +
			BitOperations.RotateLeft(v4, 18);

	private static UInt32 MixEmptyState() => HashCode.sSeed + HashCode.prime5;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UInt32 MixFinal(UInt32 hash)
	{
		hash ^= hash >> 15;
		hash *= HashCode.prime2;
		hash ^= hash >> 13;
		hash *= HashCode.prime3;
		hash ^= hash >> 16;
		return hash;
	}

	public void Add<T>(T value) { this.Add(value?.GetHashCode() ?? 0); }

	public void Add<T>(T value, IEqualityComparer<T>? comparer)
	{
		this.Add(comparer?.GetHashCode(value) ?? value?.GetHashCode() ?? 0);
	}

	private void Add(Int32 value)
	{
		// The original xxHash works as follows:
		// 0. Initialize immediately. We can't do this in a struct (no
		//    default ctor).
		// 1. Accumulate blocks of length 16 (4 uints) into 4 accumulators.
		// 2. Accumulate remaining blocks of length 4 (1 uint) into the
		//    hash.
		// 3. Accumulate remaining blocks of length 1 into the hash.

		// There is no need for #3 as this type only accepts ints. _queue1,
		// _queue2 and _queue3 are basically a buffer so that when
		// ToHashCode is called we can execute #2 correctly.

		// We need to initialize the xxHash32 state (_v1 to _v4) lazily (see
		// #0) nd the last place that can be done if you look at the
		// original code is just before the first block of 16 bytes is mixed
		// in. The xxHash32 state is never used for streams containing fewer
		// than 16 bytes.

		// To see what's really going on here, have a look at the Combine
		// methods.

		UInt32 val = (UInt32)value;

		// Storing the value of _length locally shaves of quite a few bytes
		// in the resulting machine code.
		UInt32 previousLength = this._length++;
		UInt32 position = previousLength % 4;

		// Switch can't be inlined.

		if (position == 0)
		{
			this._queue1 = val;
		}
		else if (position == 1)
		{
			this._queue2 = val;
		}
		else if (position == 2)
		{
			this._queue3 = val;
		}
		else // position == 3
		{
			if (previousLength == 3)
				HashCode.Initialize(out this._v1, out this._v2, out this._v3, out this._v4);

			this._v1 = HashCode.Round(this._v1, this._queue1);
			this._v2 = HashCode.Round(this._v2, this._queue2);
			this._v3 = HashCode.Round(this._v3, this._queue3);
			this._v4 = HashCode.Round(this._v4, val);
		}
	}

	public Int32 ToHashCode()
	{
		// Storing the value of _length locally shaves of quite a few bytes
		// in the resulting machine code.
		UInt32 length = this._length;

		// position refers to the *next* queue position in this method, so
		// position == 1 means that _queue1 is populated; _queue2 would have
		// been populated on the next call to Add.
		UInt32 position = length % 4;

		// If the length is less than 4, _v1 to _v4 don't contain anything
		// yet. xxHash32 treats this differently.

		UInt32 hash = length < 4 ? HashCode.MixEmptyState() : HashCode.MixState(this._v1, this._v2, this._v3, this._v4);

		// _length is incremented once per Add(Int32) and is therefore 4
		// times too small (xxHash length is in bytes, not ints).

		hash += length * 4;

		// Mix what remains in the queue

		// Switch can't be inlined right now, so use as few branches as
		// possible by manually excluding impossible scenarios (position > 1
		// is always false if position is not > 0).
		if (position > 0)
		{
			hash = HashCode.QueueRound(hash, this._queue1);
			if (position > 1)
			{
				hash = HashCode.QueueRound(hash, this._queue2);
				if (position > 2)
					hash = HashCode.QueueRound(hash, this._queue3);
			}
		}

		hash = HashCode.MixFinal(hash);
		return (Int32)hash;
	}

#pragma warning disable 0809
	// Obsolete member 'memberA' overrides non-obsolete member 'memberB'. 
	// Disallowing GetHashCode and Equals is by design

	// * We decided to not override GetHashCode() to produce the hash code 
	//   as this would be weird, both naming-wise as well as from a
	//   behavioral standpoint (GetHashCode() should return the object's
	//   hash code, not the one being computed).

	// * Even though ToHashCode() can be called safely multiple times on
	//   this implementation, it is not part of the contract. If the
	//   implementation has to change in the future we don't want to worry
	//   about people who might have incorrectly used this type.

#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1133)]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3877)]
#endif
	[Obsolete(
		"HashCode is a mutable struct and should not be compared with other HashCodes. Use ToHashCode to retrieve the computed hash code.",
		true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Int32 GetHashCode()
		=> throw new NotSupportedException(
			"HashCode is a mutable struct and should not be compared with other HashCodes. Use ToHashCode to retrieve the computed hash code.");

#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1133)]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3877)]
#endif
	[Obsolete("HashCode is a mutable struct and should not be compared with other HashCodes.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Boolean Equals(Object? obj)
		=> throw new NotSupportedException(
			"HashCode is a mutable struct and should not be compared with other HashCodes.");
#pragma warning restore 0809
}
#endif