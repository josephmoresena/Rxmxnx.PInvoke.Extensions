namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[ExcludeFromCodeCoverage]
public sealed class WithSafeTransformTest
{
	[Fact]
	internal unsafe void NullFixedTest()
	{
		CStringSequence? input = null;
		fixed (void* ptr = input)
			Assert.Equal(IntPtr.Zero, (IntPtr)ptr);
	}

	[Fact]
	internal unsafe void EmptyFixedTest()
	{
		CStringSequence input = new(Array.Empty<CString>());
		fixed (void* ptr = input)
			Assert.NotEqual(IntPtr.Zero, (IntPtr)ptr);
	}

	[Fact]
	internal unsafe void FixedTest()
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		CStringSequence seq = WithSafeTransformTest.CreateSequence(handle, indices, out CString?[] values);
		fixed (void* ptrSeq = seq)
		{
			IntPtr ptr = (IntPtr)ptrSeq;
			for (Int32 i = 0; i < indices.Count; i++)
			{
				if (!CString.IsNullOrEmpty(values[i]))
				{
					Assert.Equal(values[i], seq[i]);
					Assert.True(Unsafe.AreSame(ref Unsafe.AsRef<Byte>(ptr.ToPointer()),
					                           ref MemoryMarshal.GetReference(seq[i].AsSpan())));
					ptr += seq[i].Length + 1;
				}
				else
				{
					Assert.True(CString.IsNullOrEmpty(seq[i]));
				}
			}
		}
	}

	[Fact]
	internal void EmptyTest()
	{
		FixedCStringSequence fseq = default;
		ReadOnlyFixedMemoryList fml = fseq;
		Assert.Empty(fseq.Values);
		Assert.Empty(fseq.ToArray());
		Assert.Null(fseq.ToString());

		Assert.Equal(0, fml.Count);
		Assert.True(fml.IsEmpty);
		Assert.Empty(fml.ToArray());

		fseq.Unload();
		Assert.Empty(fseq.Values);
		Assert.Empty(fseq.ToArray());
		Assert.Null(fseq.ToString());
	}

	[Fact]
	internal void BasicTest()
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		CStringSequence seq = WithSafeTransformTest.CreateSequence(handle, indices, out _);
		seq.WithSafeTransform(WithSafeTransformTest.AssertReference);
		seq.WithSafeFixed(WithSafeTransformTest.AssertReference);
		Assert.Equal(seq, seq.WithSafeTransform(WithSafeTransformTest.CreateCopy));
		Assert.Equal(seq.Where(c => !CString.IsNullOrEmpty(c)),
		             seq.WithSafeFixed(WithSafeTransformTest.GetNonEmptyValues));
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal void Test(Boolean fixedIndices)
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = !fixedIndices ? TestSet.GetIndices() : WithSafeTransformTest.GetIndices();
		CStringSequence seq = WithSafeTransformTest.CreateSequence(handle, indices, out CString?[] values);
		seq.WithSafeTransform(values, WithSafeTransformTest.AssertSequence);
		seq.WithSafeFixed(seq, WithSafeTransformTest.AssertSequence);
		Assert.Equal(seq, seq.WithSafeTransform(seq, WithSafeTransformTest.CreateCopy));
		Assert.Equal(seq.ToString(), seq.WithSafeFixed(seq, WithSafeTransformTest.CreateCopy).ToString());
	}

	private static void AssertReference(FixedCStringSequence fseq)
	{
		IReadOnlyList<CString> values = fseq.Values;
		IReadOnlyFixedMemory[] fValues = fseq.ToArray();

		for (Int32 i = 0; i < values.Count; i++)
			values[i].WithSafeFixed(fValues[i], WithSafeTransformTest.AssertReference);

		WithSafeTransformTest.AssertReference((ReadOnlyFixedMemoryList)fseq);
	}
	private static void AssertReference(ReadOnlyFixedMemoryList fml)
	{
		Int32 offset = 0;
		IntPtr ptr = IntPtr.Zero;

		for (Int32 i = 0; i < fml.Count; i++)
		{
			if (!fml[i].Bytes.IsEmpty)
			{
				if (ptr == IntPtr.Zero)
					ptr = fml[i].Pointer;

				Assert.Equal(ptr + offset, fml[i].Pointer);
				offset += fml[i].Bytes.Length + 1;
			}
		}
	}
	private static void AssertSequence(FixedCStringSequence fseq, IReadOnlyList<CString?> values)
	{
		for (Int32 i = 0; i < values.Count; i++)
			WithSafeTransformTest.AssertValue(fseq, values[i], i);

		try
		{
			_ = fseq[-1];
		}
		catch (Exception ex)
		{
			Assert.IsType<ArgumentOutOfRangeException>(ex);
		}

		try
		{
			_ = fseq[values.Count];
		}
		catch (Exception ex)
		{
			Assert.IsType<ArgumentOutOfRangeException>(ex);
		}
	}
	private static void AssertValue(FixedCStringSequence fseq, CString? value, Int32 i)
	{
		if (value is not null)
		{
			Assert.Equal(value, fseq.Values[i]);
			Assert.Equal(!fseq.Values[i].IsReference, Object.ReferenceEquals(value, fseq.Values[i]));
		}
		else
		{
			Assert.Equal(0, fseq.Values[i].Length);
		}
	}
	private static unsafe void AssertSequence(ReadOnlyFixedMemoryList fml, CStringSequence seq)
	{
		Int32 offset = 0;
		IntPtr ptr = IntPtr.Zero;

		for (Int32 i = 0; i < fml.Count; i++)
		{
			if (!fml[i].Bytes.IsEmpty)
			{
				if (ptr == IntPtr.Zero)
					ptr = fml[i].Pointer;

				Assert.Equal(seq[i].Length, fml[i].Bytes.Length);
				fixed (void* ptr2 = &MemoryMarshal.GetReference(seq[i].AsSpan()))
					Assert.Equal(new(ptr2), ptr + offset);
				offset += fml[i].Bytes.Length + 1;
			}
			else
			{
				Assert.True(CString.IsNullOrEmpty(seq[i]));
			}
		}

		try
		{
			_ = fml[-1];
		}
		catch (Exception ex)
		{
			Assert.IsType<ArgumentOutOfRangeException>(ex);
		}

		try
		{
			_ = fml[seq.Count];
		}
		catch (Exception ex)
		{
			Assert.IsType<ArgumentOutOfRangeException>(ex);
		}
	}
	private static void AssertReference(in IReadOnlyFixedMemory fcstr, IReadOnlyFixedMemory fValue)
		=> Assert.Equal(fcstr, fValue);
	private static CStringSequence CreateCopy(FixedCStringSequence fseq)
	{
		CStringSequence seq = new(fseq.Values);
		IReadOnlyFixedMemory[] fmems = fseq.ToArray();

		for (Int32 i = 0; i < seq.Count; i++)
			Assert.Equal(seq[i], fmems[i].Bytes.ToArray());

		fseq.Unload();
		foreach (IReadOnlyFixedMemory mem in fmems)
			Assert.Throws<InvalidOperationException>(() => mem.Bytes.ToArray());

		return seq;
	}
	private static CStringSequence CreateCopy(FixedCStringSequence fseq, CStringSequence seq)
	{
		for (Int32 i = 0; i < seq.Count; i++)
			seq[i].WithSafeFixed(fseq[i], WithSafeTransformTest.AssertReference);
		Assert.Equal(new CString(() => MemoryMarshal.AsBytes<Char>(seq.ToString())).ToString(), fseq.ToString());
		return new(fseq.Values);
	}
	private static unsafe CStringSequence CreateCopy(ReadOnlyFixedMemoryList fml, CStringSequence seq)
	{
		Int32 offset = 0;
		IntPtr ptr = IntPtr.Zero;

		List<CString> cstr = new();
		for (Int32 i = 0; i < fml.Count; i++)
		{
			if (!fml[i].Bytes.IsEmpty)
			{
				if (ptr == IntPtr.Zero)
					ptr = fml[i].Pointer;

				Assert.Equal(seq[i].Length, fml[i].Bytes.Length);
				fixed (void* ptr2 = &MemoryMarshal.GetReference(seq[i].AsSpan()))
					Assert.Equal(new(ptr2), ptr + offset);
				offset += fml[i].Bytes.Length + 1;
				cstr.Add(new(fml[i].Bytes));
			}
			else
			{
				Assert.True(CString.IsNullOrEmpty(seq[i]));
			}
		}

		return new(cstr);
	}
	private static CStringSequence CreateSequence(TestMemoryHandle handle, IReadOnlyList<Int32> indices,
		out CString?[] values)
	{
		values = new CString[indices.Count];
		for (Int32 i = 0; i < values.Length; i++)
			values[i] = TestSet.GetCString(indices[i], handle);
		CStringSequence seq = new(values);
		return seq;
	}
	private static IEnumerable<CString> GetNonEmptyValues(ReadOnlyFixedMemoryList fml)
	{
		List<CString> result = new();
		foreach (IReadOnlyFixedMemory fmem in fml)
		{
			if (!fmem.Bytes.IsEmpty)
				result.Add(new(fmem.Bytes));
		}
		return result;
	}
	private static IReadOnlyList<Int32> GetIndices()
	{
		Queue<Int32> queue = new(TestSet.GetIndices());
		List<Int32> result = new();
		Byte space = 0;
		while (queue.TryDequeue(out Int32 value))
		{
			result.Add(value);
			if (space == 2)
				result.Add(-3);
			else if (space == 13)
				result.Add(-2);
			else if (space == 17)
				result.Add(-1);
			else if (space > 23)
				space = 0;
			else
				space++;
		}
		return result.ToArray();
	}
}