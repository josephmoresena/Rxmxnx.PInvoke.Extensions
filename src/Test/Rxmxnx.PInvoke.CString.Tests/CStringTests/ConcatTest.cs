namespace Rxmxnx.PInvoke.Tests.CStringTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class ConcatTest
{
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void LocalEmptyTest(Boolean emptyData)
	{
		CString?[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : [];
		ConcatTest.EmptyTest(values);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReferenceEmptyTest(Boolean emptyData)
	{
		CString?[] values = !emptyData ? Enumerable.Repeat<CString>(new(IntPtr.Zero, default), 3).ToArray() : [];
		ConcatTest.EmptyTest(values);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void NullEmptyTest(Boolean emptyData)
	{
		CString?[] values = !emptyData ? Enumerable.Repeat<CString?>(default, 3).ToArray() : [];
		ConcatTest.EmptyTest(values);
	}

	[Fact]
	public void Test()
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
		CString?[] values = TestSet.GetValues(indices, handle);

		ConcatTest.NormalTest(strings, values);
	}

	[Fact]
	public void BinaryTest()
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
		Byte[]?[] values = indices.Select(i => TestSet.GetCString(i, handle)?.ToArray()).ToArray();

		ConcatTest.NormalTest(strings, values);
	}

	[Fact]
	public void SpanTest()
	{
		List<Int32> indices = TestSet.GetIndices(8);
		String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();

		PInvokeAssert.Equal(String.Concat(strings[..2]),
		                    CString.Concat(TestSet.GetSpan(indices[0]), TestSet.GetSpan(indices[1])).ToString());
		PInvokeAssert.Equal(String.Concat(strings[..3]),
		                    CString.Concat(TestSet.GetSpan(indices[0]), TestSet.GetSpan(indices[1]),
		                                   TestSet.GetSpan(indices[2])).ToString());
		PInvokeAssert.Equal(String.Concat(strings[..4]),
		                    CString.Concat(TestSet.GetSpan(indices[0]), TestSet.GetSpan(indices[1]),
		                                   TestSet.GetSpan(indices[2]), TestSet.GetSpan(indices[3])).ToString());
		PInvokeAssert.Equal(String.Concat(strings[..5]),
		                    CString.Concat(TestSet.GetSpan(indices[0]), TestSet.GetSpan(indices[1]),
		                                   TestSet.GetSpan(indices[2]), TestSet.GetSpan(indices[3]),
		                                   TestSet.GetSpan(indices[4])).ToString());
		PInvokeAssert.Equal(String.Concat(strings[..6]),
		                    CString.Concat(TestSet.GetSpan(indices[0]), TestSet.GetSpan(indices[1]),
		                                   TestSet.GetSpan(indices[2]), TestSet.GetSpan(indices[3]),
		                                   TestSet.GetSpan(indices[4]), TestSet.GetSpan(indices[5])).ToString());
		PInvokeAssert.Equal(String.Concat(strings[..7]),
		                    CString.Concat(TestSet.GetSpan(indices[0]), TestSet.GetSpan(indices[1]),
		                                   TestSet.GetSpan(indices[2]), TestSet.GetSpan(indices[3]),
		                                   TestSet.GetSpan(indices[4]), TestSet.GetSpan(indices[5]),
		                                   TestSet.GetSpan(indices[6])).ToString());
		PInvokeAssert.Equal(String.Concat(strings),
		                    CString.Concat(TestSet.GetSpan(indices[0]), TestSet.GetSpan(indices[1]),
		                                   TestSet.GetSpan(indices[2]), TestSet.GetSpan(indices[3]),
		                                   TestSet.GetSpan(indices[4]), TestSet.GetSpan(indices[5]),
		                                   TestSet.GetSpan(indices[6]), TestSet.GetSpan(indices[7])).ToString());

		PInvokeAssert.Same(CString.Empty, CString.Concat(ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty));
		PInvokeAssert.Same(CString.Empty,
		                   CString.Concat(ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
		                                  ReadOnlySpan<Byte>.Empty));
		PInvokeAssert.Same(CString.Empty,
		                   CString.Concat(ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
		                                  ReadOnlySpan<Byte>.Empty));
		PInvokeAssert.Same(CString.Empty,
		                   CString.Concat(ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
		                                  ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty));
		PInvokeAssert.Same(CString.Empty,
		                   CString.Concat(ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
		                                  ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
		                                  ReadOnlySpan<Byte>.Empty));
		PInvokeAssert.Same(CString.Empty,
		                   CString.Concat(ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
		                                  ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
		                                  ReadOnlySpan<Byte>.Empty));
		PInvokeAssert.Same(CString.Empty,
		                   CString.Concat(ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
		                                  ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
		                                  ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty));
	}
	private static void NormalTest(String?[] strings, CString?[] values)
	{
		String expectedCString = String.Concat(strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = CString.Concat(values);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		PInvokeAssert.Equal(expectedCString, resultCStringCString);
		PInvokeAssert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
	private static void NormalTest(String?[] strings, Byte[]?[] values)
	{
		String expectedCString = String.Concat(strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = CString.Concat(values);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		PInvokeAssert.Equal(expectedCString, resultCStringCString);
		PInvokeAssert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
	private static void EmptyTest(CString?[] values)
	{
		using MemoryHandle _ = CString.Empty.TryPin(out Boolean pinned);
		CString resultCString = CString.Concat(values);
		PInvokeAssert.NotNull(resultCString);
		PInvokeAssert.Equal(CString.Empty, resultCString);
		PInvokeAssert.Same(CString.Empty, resultCString);
		PInvokeAssert.True(resultCString.IsNullTerminated);
		PInvokeAssert.False(resultCString.IsReference);
		PInvokeAssert.False(resultCString.IsSegmented);
		PInvokeAssert.Equal(!pinned, resultCString.IsFunction && !CString.IsImagePersistent(resultCString));
	}

#if NET6_0_OR_GREATER
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal async Task LocalEmptyTestAsync(Boolean emptyData)
	{
		CString?[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : [];
		await ConcatTest.EmptyTestAsync(values);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal async Task ReferenceEmptyTestAsync(Boolean emptyData)
	{
		CString?[] values = !emptyData ? Enumerable.Repeat<CString>(new(IntPtr.Zero, default), 3).ToArray() : [];
		await ConcatTest.EmptyTestAsync(values);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal async Task NullEmptyTestAsync(Boolean emptyData)
	{
		CString?[] values = !emptyData ? Enumerable.Repeat<CString?>(default, 3).ToArray() : [];
		await ConcatTest.EmptyTestAsync(values);
	}

	[Fact]
	internal async Task TestAsync()
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
		CString?[] values = TestSet.GetValues(indices, handle);

		await ConcatTest.NormalTestAsync(strings, values);
	}

	[Fact]
	internal async Task BinaryTestAsync()
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
		Byte[]?[] values = indices.Select(i => TestSet.GetCString(i, handle)?.ToArray()).ToArray();

		await ConcatTest.NormalTestAsync(strings, values);
	}
#endif

#if NET6_0_OR_GREATER
	private static async Task NormalTestAsync(String?[] strings, CString?[] values)
	{
		String expectedCString = String.Concat(strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = await CString.ConcatAsync(values);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
	private static async Task NormalTestAsync(String?[] strings, Byte[]?[] values)
	{
		String expectedCString = String.Concat(strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = await CString.ConcatAsync(values);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
	private static async Task EmptyTestAsync(CString?[] values)
	{
		CString resultCString = await CString.ConcatAsync(values);
		Assert.NotNull(resultCString);
		Assert.Equal(CString.Empty, resultCString);
		Assert.Same(CString.Empty, resultCString);
		Assert.True(resultCString.IsNullTerminated);
		Assert.False(resultCString.IsReference);
		Assert.False(resultCString.IsSegmented);
	}
#endif
}