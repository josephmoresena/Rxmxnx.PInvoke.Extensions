namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class WithSafeTransformTest
{
	[Fact]
	public unsafe void NullFixedTest()
	{
		CStringSequence? input = null;
		fixed (void* ptr = input)
			PInvokeAssert.Equal(IntPtr.Zero, (IntPtr)ptr);
	}

	[Fact]
	public unsafe void EmptyFixedTest()
	{
		CStringSequence input = new(Array.Empty<CString>());
		fixed (void* ptr = input)
			PInvokeAssert.NotEqual(IntPtr.Zero, (IntPtr)ptr);
	}

	[Fact]
	public unsafe void FixedTest()
	{
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices();
		CStringSequence seq = WithSafeTransformTest.CreateSequence(handle, indices, out CString?[] values);
		fixed (void* ptrSeq = seq)
		{
			IntPtr ptr = (IntPtr)ptrSeq;
			for (Int32 i = 0; i < indices.Count; i++)
			{
				if (!CString.IsNullOrEmpty(values[i]))
				{
					PInvokeAssert.Equal(values[i], seq[i]);
					PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef<Byte>(ptr.ToPointer()),
					                                  ref MemoryMarshal.GetReference(seq[i].AsSpan())));
					ptr += seq[i].Length + 1;
				}
				else
				{
					PInvokeAssert.True(CString.IsNullOrEmpty(seq[i]));
				}
			}
		}
	}

	[Fact]
	public unsafe void FixedPointerTest()
	{
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices();
		CStringSequence seq = WithSafeTransformTest.CreateSequence(handle, indices, out CString?[] values);
		using IFixedPointer.IDisposable fPtr = seq.GetFixedPointer();
		IntPtr ptr = fPtr.Pointer;
		for (Int32 i = 0; i < indices.Count; i++)
		{
			if (!CString.IsNullOrEmpty(values[i]))
			{
				PInvokeAssert.Equal(values[i], seq[i]);
				PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef<Byte>(ptr.ToPointer()),
				                                  ref MemoryMarshal.GetReference(seq[i].AsSpan())));
				ptr += seq[i].Length + 1;
			}
			else
			{
				PInvokeAssert.True(CString.IsNullOrEmpty(seq[i]));
			}
		}
	}

	[Fact]
	public void EmptyTest()
	{
		FixedCStringSequence fseq = default;
		ReadOnlyFixedMemoryList fml = fseq;
		PInvokeAssert.Empty(fseq.Values);
		PInvokeAssert.Empty(fseq.ToArray());
		PInvokeAssert.Null(fseq.ToString());

		PInvokeAssert.Equal(0, fml.Count);
		PInvokeAssert.True(fml.IsEmpty);
		PInvokeAssert.Empty(fml.ToArray());

		fseq.Unload();
		PInvokeAssert.Empty(fseq.Values);
		PInvokeAssert.Empty(fseq.ToArray());
		PInvokeAssert.Null(fseq.ToString());
	}

	[Fact]
	public void BasicTest()
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		CStringSequence seq = WithSafeTransformTest.CreateSequence(handle, indices, out _);
		seq.WithSafeTransform(WithSafeTransformTest.AssertReference);
		seq.WithSafeFixed(WithSafeTransformTest.AssertReference);
		PInvokeAssert.Equal(seq, seq.WithSafeTransform(WithSafeTransformTest.CreateCopy));
		PInvokeAssert.Equal(seq.Where(c => !CString.IsNullOrEmpty(c)),
		                    seq.WithSafeFixed(WithSafeTransformTest.GetNonEmptyValues));
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void Test(Boolean fixedIndices)
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = !fixedIndices ? TestSet.GetIndices() : WithSafeTransformTest.GetIndices();
		CStringSequence seq = WithSafeTransformTest.CreateSequence(handle, indices, out CString?[] values);
		seq.WithSafeTransform(values, WithSafeTransformTest.AssertSequence);
		seq.WithSafeFixed(seq, WithSafeTransformTest.AssertSequence);
		PInvokeAssert.Equal(seq, seq.WithSafeTransform(seq, WithSafeTransformTest.CreateCopy));
		PInvokeAssert.Equal(seq.ToString(), seq.WithSafeFixed(seq, WithSafeTransformTest.CreateCopy).ToString());
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

		foreach (IReadOnlyFixedMemory fmem in fml)
		{
			if (fmem.Bytes.IsEmpty) continue;
			if (ptr == IntPtr.Zero)
				ptr = fmem.Pointer;

			PInvokeAssert.Equal(ptr + offset, fmem.Pointer);
			offset += fmem.Bytes.Length + 1;
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
			PInvokeAssert.IsType<ArgumentOutOfRangeException>(ex);
		}

		try
		{
			_ = fseq[values.Count];
		}
		catch (Exception ex)
		{
			PInvokeAssert.IsType<ArgumentOutOfRangeException>(ex);
		}
	}
	private static void AssertValue(FixedCStringSequence fseq, CString? value, Int32 i)
	{
		if (value is not null)
		{
			PInvokeAssert.Equal(value, fseq.Values[i]);
			PInvokeAssert.Equal(!fseq.Values[i].IsReference, Object.ReferenceEquals(value, fseq.Values[i]));
		}
		else
		{
			PInvokeAssert.Equal(0, fseq.Values[i].Length);
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

				PInvokeAssert.Equal(seq[i].Length, fml[i].Bytes.Length);
				fixed (void* ptr2 = &MemoryMarshal.GetReference(seq[i].AsSpan()))
					PInvokeAssert.Equal(new(ptr2), ptr + offset);
				offset += fml[i].Bytes.Length + 1;
			}
			else
			{
				PInvokeAssert.True(CString.IsNullOrEmpty(seq[i]));
			}
		}

		try
		{
			_ = fml[-1];
		}
		catch (Exception ex)
		{
			PInvokeAssert.IsType<ArgumentOutOfRangeException>(ex);
		}

		try
		{
			_ = fml[seq.Count];
		}
		catch (Exception ex)
		{
			PInvokeAssert.IsType<ArgumentOutOfRangeException>(ex);
		}
	}
	private static void AssertReference(in IReadOnlyFixedMemory fcstr, IReadOnlyFixedMemory fValue)
	{
		if (!fcstr.IsNullOrEmpty)
		{
			PInvokeAssert.Equal(fcstr, fValue);
			return;
		}
		PInvokeAssert.Equal(fcstr.Bytes.Length, fValue.Bytes.Length);
		PInvokeAssert.True(fcstr.Bytes.SequenceEqual(fValue.Bytes));
		PInvokeAssert.Equal(fcstr.Objects.Length, fValue.Objects.Length);
#if NET6_0_OR_GREATER
		Assert.True(fcstr.Objects.SequenceEqual(fValue.Objects));
#else
		PInvokeAssert.Equal(fcstr.Objects.ToArray(), fValue.Objects.ToArray());
#endif
		PInvokeAssert.Equal(fcstr.IsNullOrEmpty, fValue.IsNullOrEmpty);
	}
	private static CStringSequence CreateCopy(FixedCStringSequence fseq)
	{
		CStringSequence seq = new(fseq.Values);
		IReadOnlyFixedMemory[] fmems = fseq.ToArray();

		for (Int32 i = 0; i < seq.Count; i++)
#if NET6_0_OR_GREATER
			Assert.Equal(seq[i].AsSpan(), fmems[i].Bytes);
#else
			PInvokeAssert.Equal(seq[i].AsSpan().ToArray(), fmems[i].Bytes.ToArray());
#endif

		fseq.Unload();
		foreach (IReadOnlyFixedMemory mem in fmems)
			PInvokeAssert.Throws<InvalidOperationException>(() => mem.Bytes.ToArray());

		return seq;
	}
	private static CStringSequence CreateCopy(FixedCStringSequence fseq, CStringSequence seq)
	{
		for (Int32 i = 0; i < seq.Count; i++)
			seq[i].WithSafeFixed(fseq[i], WithSafeTransformTest.AssertReference);
		PInvokeAssert.Equal(new CString(() => MemoryMarshal.AsBytes<Char>(seq.ToString())).ToString(), fseq.ToString());
		return new(fseq.Values);
	}
	private static unsafe CStringSequence CreateCopy(ReadOnlyFixedMemoryList fml, CStringSequence seq)
	{
		Int32 offset = 0;
		IntPtr ptr = IntPtr.Zero;

		List<CString> cstr = [];
		for (Int32 i = 0; i < fml.Count; i++)
		{
			if (!fml[i].Bytes.IsEmpty)
			{
				if (ptr == IntPtr.Zero)
					ptr = fml[i].Pointer;

				PInvokeAssert.Equal(seq[i].Length, fml[i].Bytes.Length);
				fixed (void* ptr2 = &MemoryMarshal.GetReference(seq[i].AsSpan()))
					PInvokeAssert.Equal(new(ptr2), ptr + offset);
				offset += fml[i].Bytes.Length + 1;
				cstr.Add(new(fml[i].Bytes));
			}
			else
			{
				PInvokeAssert.True(CString.IsNullOrEmpty(seq[i]));
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
		List<CString> result = [];
		foreach (IReadOnlyFixedMemory fmem in fml)
		{
			if (!fmem.Bytes.IsEmpty)
				result.Add(new(fmem.Bytes));
		}
		return result;
	}
	private static Int32[] GetIndices()
	{
		Queue<Int32> queue = new(TestSet.GetIndices());
		List<Int32> result = [];
		Byte space = 0;
		while (queue.TryDequeue(out Int32 value))
		{
			result.Add(value);
			switch (space)
			{
				case 2:
					result.Add(-3);
					break;
				case 13:
					result.Add(-2);
					break;
				case 17:
					result.Add(-1);
					break;
				case > 23:
					space = 0;
					break;
				default:
					space++;
					break;
			}
		}
		return result.ToArray();
	}
}