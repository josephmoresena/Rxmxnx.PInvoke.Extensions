namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

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
	internal void EmptyTest()
	{
		Assert.Empty(CStringSequence.Empty);
		Assert.Equal(String.Empty, CStringSequence.Empty.ToString());
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
	internal void Test(Int32? length, Boolean endWithEmpty = false)
	{
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices(length);
		if (endWithEmpty) indices.AddRange([-1, -2, -3,]);
		String?[] strings = indices.Select(i => TestSet.GetString(i, true)).ToArray();
		CStringSequence seqRef = new(strings);
		CString?[] values = TestSet.GetValues(indices, handle);
		CStringSequence seq = new(values);

		Assert.Equal(seqRef, seq);
		Assert.Equal(strings.Select(c => (CString?)c ?? CString.Zero), seq);
		Assert.Equal(seqRef.Count, seq.Count);
		Assert.Equal(seqRef.ToString(), seq.ToString());
		Assert.NotSame(seqRef.ToString(), seq.ToString());
		Assert.Equal(seqRef.ToString().GetHashCode(), seq.GetHashCode());
		Assert.Equal(String.Concat(strings), seq.ToCString().ToString());
		Assert.NotSame(seqRef, seq);
		Assert.False(seqRef.Equals(default));
		Assert.False(seqRef.Equals(default(Object)));
		Assert.True(seqRef.Equals(seq));
		Assert.True(seqRef.Equals((Object)seq));

		Assert.Equal(new(strings.ToList()), seq);
		Assert.Equal(new(values.ToList()), seq);

		CStringSequence clone = (CStringSequence)seq.Clone();
		Assert.Equal(seqRef.Count, clone.Count);
		Assert.Equal(seqRef.ToString(), clone.ToString());
		Assert.NotSame(seqRef.ToString(), clone.ToString());

		IEnumerator<CString> enumerator = (seq as IEnumerable<CString>).GetEnumerator();
		enumerator.MoveNext();
		BasicTests.AssertSequence(seq, strings, values);
		Assert.Equal(seq, values.Select(c => c ?? CString.Zero));
		GC.Collect();
		Assert.Equal(seq.Where(c => !CString.IsNullOrEmpty(c)), values.Where(c => !CString.IsNullOrEmpty(c)));
		GC.Collect();
		enumerator.MoveNext();
		enumerator.MoveNext();
		GC.Collect();
		enumerator.Dispose();
	}

	private static unsafe void AssertSequence(CStringSequence seq, String?[] strings, CString?[] values)
	{
		String value = seq.ToString();
		for (Int32 i = 0; i < seq.Count; i++)
		{
			if (seq[i].Length != 0 || !seq[i].IsReference)
			{
				Assert.Equal(values[i], seq[i]);
				Assert.Equal(strings[i], seq[i].ToString());
			}
			else
			{
				Assert.Same(seq[i], CString.Zero);
				Assert.Equal(String.Empty, seq[i].ToString());
				if (values[i] is not null)
					fixed (void* ptr = values[i]!.AsSpan())
						Assert.Equal(IntPtr.Zero, new(ptr));
				Assert.Null(strings[i]);
			}
			BasicTests.AssertPin(seq.ToString(), seq[i]);
		}
	}
	private static unsafe void AssertPin(String utf8Buffer, CString? cstr)
	{
		if (CString.IsNullOrEmpty(cstr))
			return;

		using MemoryHandle handle = cstr.TryPin(out Boolean pinned);
		Assert.True(pinned);
		Assert.Equal(Assert.IsType<GCHandle>(BasicTests.handleFieldInfo.GetValue(handle)).Target, utf8Buffer);

		if (cstr.Length <= 3) return;

		using MemoryHandle handle2 = cstr[1..^1].TryPin(out pinned);
		Assert.True(pinned);
		Assert.NotEqual((IntPtr)handle.Pointer, IntPtr.Zero);
		Assert.Equal((IntPtr)handle.Pointer, (IntPtr)handle.Pointer + 1);
	}
}