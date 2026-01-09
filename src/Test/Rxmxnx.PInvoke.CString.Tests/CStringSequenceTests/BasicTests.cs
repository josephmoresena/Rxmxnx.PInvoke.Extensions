namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class BasicTests
{
	/// <summary>
	/// <see cref="GCHandle"/> field info.
	/// </summary>
	private static readonly FieldInfo handleFieldInfo = typeof(MemoryHandle)
	                                                    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
	                                                    .First(f => f.FieldType == typeof(GCHandle));

	[Fact]
	public void EmptyTest()
	{
		PInvokeAssert.Empty(CStringSequence.Empty);
		PInvokeAssert.Equal(String.Empty, CStringSequence.Empty.ToString());
	}

	[Theory]
	[InlineData(null)]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	[InlineData(null, true)]
	[InlineData(8, true)]
	[InlineData(32, true)]
	[InlineData(256, true)]
	[InlineData(300, true)]
	[InlineData(1000, true)]
	public void Test(Int32? length, Boolean endWithEmpty = false)
	{
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices(length);
		if (endWithEmpty) indices.AddRange([-1, -2, -3,]);
		String?[] strings = indices.Select(i => TestSet.GetString(i, true)).ToArray();
		CStringSequence seqRef = new(strings);
		CString?[] values = TestSet.GetValues(indices, handle);
		CStringSequence seq = new(values);

		PInvokeAssert.Equal(seqRef, seq);
		PInvokeAssert.Equal(strings.Select(c => (CString?)c ?? CString.Zero), seq);
		PInvokeAssert.Equal(seqRef.Count, seq.Count);
		PInvokeAssert.Equal(seqRef.ToString(), seq.ToString());
		PInvokeAssert.NotSame(seqRef.ToString(), seq.ToString());
		PInvokeAssert.Equal(seqRef.ToString().GetHashCode(), seq.GetHashCode());
		PInvokeAssert.Equal(String.Concat(strings), seq.ToCString().ToString());
		PInvokeAssert.NotSame(seqRef, seq);
		PInvokeAssert.False(seqRef.Equals(default));
		PInvokeAssert.False(seqRef?.Equals(default(Object)));
		PInvokeAssert.True(seqRef?.Equals(seq));
		PInvokeAssert.True(seqRef?.Equals((Object)seq));

		PInvokeAssert.Equal(new(strings.ToList()), seq);
		PInvokeAssert.Equal(new(values.ToList()), seq);

		CString nullTerminated = seq.ToCString(true);
		CString nonNullTerminated = seq.ToCString(false);

		PInvokeAssert.Equal(nullTerminated.Length, nonNullTerminated.Length);
		PInvokeAssert.Equal(nullTerminated.IsFunction, nonNullTerminated.IsFunction);
		PInvokeAssert.Equal(nullTerminated.IsReference, nonNullTerminated.IsReference);
		PInvokeAssert.Equal(nullTerminated.IsSegmented, nonNullTerminated.IsSegmented);
		PInvokeAssert.Equal(nullTerminated.IsReference, nonNullTerminated.IsReference);
		PInvokeAssert.Equal(nullTerminated.IsZero, nonNullTerminated.IsZero);
		if (seq.NonEmptyCount > 0)
		{
			PInvokeAssert.Equal(nullTerminated.IsNullTerminated, !nonNullTerminated.IsNullTerminated);
			PInvokeAssert.True(nullTerminated.Length < CString.GetBytes(nullTerminated).Length);
			PInvokeAssert.True(nonNullTerminated.Length == CString.GetBytes(nonNullTerminated).Length);
			PInvokeAssert.True(nullTerminated.IsNullTerminated);
			PInvokeAssert.False(nullTerminated.IsFunction);
			PInvokeAssert.False(nullTerminated.IsReference);
			PInvokeAssert.False(nullTerminated.IsSegmented);
			PInvokeAssert.False(nullTerminated.IsZero);
		}
		else
		{
			PInvokeAssert.Equal(nullTerminated.IsZero, CString.Empty.IsZero);
			PInvokeAssert.Equal(nullTerminated.IsFunction, CString.Empty.IsFunction);
			PInvokeAssert.Equal(nullTerminated.IsReference, CString.Empty.IsReference);
			PInvokeAssert.Equal(nullTerminated.IsSegmented, CString.Empty.IsSegmented);
			PInvokeAssert.Equal(nullTerminated.IsNullTerminated, nonNullTerminated.IsNullTerminated);
			PInvokeAssert.True(Object.ReferenceEquals(nullTerminated, nonNullTerminated));
			PInvokeAssert.True(nullTerminated.IsNullTerminated);
		}
		PInvokeAssert.True(nullTerminated.AsSpan().SequenceEqual(nonNullTerminated));

		CStringSequence clone = (CStringSequence)seq.Clone();
		PInvokeAssert.Equal(seqRef?.Count, clone.Count);
		PInvokeAssert.Equal(seqRef?.ToString(), clone.ToString());
		PInvokeAssert.NotSame(seqRef?.ToString(), clone.ToString());

		IEnumerator<CString> enumerator = (seq as IEnumerable<CString>).GetEnumerator();
		enumerator.MoveNext();
		BasicTests.AssertSequence(seq, strings, values);
		PInvokeAssert.Equal(seq, values.Select(c => c ?? CString.Zero));
		GC.Collect();
		PInvokeAssert.Equal(seq.Where(c => !CString.IsNullOrEmpty(c)), values.Where(c => !CString.IsNullOrEmpty(c)));
		GC.Collect();
		enumerator.MoveNext();
		enumerator.MoveNext();
		GC.Collect();
		enumerator.Dispose();
	}

	private static unsafe void AssertSequence(CStringSequence seq, String?[] strings, CString?[] values)
	{
		String value = seq.ToString();
		Int32 nonEmptyCount = 0;
		CStringSequence clone = CStringSequence.Parse(value);
		for (Int32 i = 0; i < seq.Count; i++)
		{
			if (!CString.IsNullOrEmpty(seq[i])) nonEmptyCount++;
			if (seq[i].Length != 0 || !seq[i].IsReference)
			{
				PInvokeAssert.Equal(values[i], seq[i]);
				PInvokeAssert.Equal(strings[i], seq[i].ToString());
			}
			else
			{
				PInvokeAssert.Same(seq[i], CString.Zero);
				PInvokeAssert.Equal(String.Empty, seq[i].ToString());
				if (values[i] is not null)
					fixed (void* ptr = values[i]!.AsSpan())
						PInvokeAssert.Equal(IntPtr.Zero, new(ptr));
				PInvokeAssert.Null(strings[i]);
			}
			BasicTests.AssertPin(seq.ToString(), seq[i]);
		}

		PInvokeAssert.Equal(nonEmptyCount, seq.NonEmptyCount);
		PInvokeAssert.Equal(clone.NonEmptyCount, seq.NonEmptyCount);
		PInvokeAssert.Equal(clone.Count, seq.NonEmptyCount);

		Int32 lowerLength = Random.Shared.Next(0, 2 * nonEmptyCount / 3);
		Int32 upperLength = Random.Shared.Next(nonEmptyCount, nonEmptyCount * 2);
		Span<Int32> offsetSpan = stackalloc Int32[nonEmptyCount];
		Span<Int32> offsetSpanClone = stackalloc Int32[nonEmptyCount];

		PInvokeAssert.Equal(lowerLength, seq.GetOffsets(new Int32[lowerLength]));
		PInvokeAssert.Equal(nonEmptyCount, seq.GetOffsets(new Int32[upperLength]));
		PInvokeAssert.Equal(nonEmptyCount, seq.GetOffsets(offsetSpan));
		PInvokeAssert.Equal(nonEmptyCount, clone.GetOffsets(offsetSpanClone));
		PInvokeAssert.True(offsetSpan.SequenceEqual(offsetSpanClone));

		using IFixedPointer.IDisposable fp = clone.GetFixedPointer();
		for (Int32 i = 0; i < clone.Count; i++)
		{
			ReadOnlySpan<Byte> spanValue =
#if NET6_0_OR_GREATER
				MemoryMarshal.CreateReadOnlySpanFromNullTerminated((Byte*)(fp.Pointer + offsetSpan[i]));
#else
				MemoryMarshalCompat.CreateReadOnlySpanFromNullTerminated((Byte*)(fp.Pointer + offsetSpan[i]));
#endif
			PInvokeAssert.True(clone[i].AsSpan().SequenceEqual(spanValue));
#if NET8_0_OR_GREATER
			Assert.True(Unsafe.AreSame(in clone[i].AsSpan()[0], in spanValue[0]));
#else
			PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef(in clone[i].AsSpan()[0]),
			                                  ref Unsafe.AsRef(in spanValue[0])));
#endif
		}
	}
	private static unsafe void AssertPin(String utf8Buffer, CString? cstr)
	{
		if (CString.IsNullOrEmpty(cstr))
			return;

		using MemoryHandle handle = cstr.TryPin(out Boolean pinned);
		PInvokeAssert.True(pinned);
		PInvokeAssert.Equal(PInvokeAssert.IsType<GCHandle>(BasicTests.handleFieldInfo.GetValue(handle)).Target,
		                    utf8Buffer);

		if (cstr.Length <= 3) return;

		using MemoryHandle handle2 = cstr[1..^1].TryPin(out pinned);
		PInvokeAssert.True(pinned);
		PInvokeAssert.NotEqual((IntPtr)handle2.Pointer, IntPtr.Zero);
		PInvokeAssert.Equal((IntPtr)handle2.Pointer, (IntPtr)handle.Pointer + 1);
	}
#if !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}