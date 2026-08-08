namespace Rxmxnx.PInvoke.Tests.CStringTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class StringConcatTest
{
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void LocalEmptyTest(Boolean emptyData)
	{
		String?[] values = !emptyData ? Enumerable.Repeat(String.Empty, 3).ToArray() : [];
		StringConcatTest.EmptyTest(values);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void NullEmptyTest(Boolean emptyData)
	{
		String?[] values = !emptyData ? Enumerable.Repeat<String?>(default, 3).ToArray() : [];
		StringConcatTest.EmptyTest(values);
	}

	[Fact]
	public void Test()
	{
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
		StringConcatTest.NormalTest(strings);
	}

#if NETSTANDARD2_1 && !LEGACY || NETCOREAPP2_1_OR_GREATER
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public async Task LocalEmptyTestAsync(Boolean emptyData)
	{
		String?[] values = !emptyData ? Enumerable.Repeat(String.Empty, 3).ToArray() : [];
		await StringConcatTest.EmptyTestAsync(values);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public async Task NullEmptyTestAsync(Boolean emptyData)
	{
		String?[] values = !emptyData ? Enumerable.Repeat<String?>(default, 3).ToArray() : [];
		await StringConcatTest.EmptyTestAsync(values);
	}

	[Fact]
	public async Task TestAsync()
	{
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
		await StringConcatTest.NormalTestAsync(strings);
	}

	private static async Task NormalTestAsync(String?[] strings)
	{
		String expectedCString = String.Concat(strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = await CString.ConcatAsync(strings);
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);
#elif NETCOREAPP2_0_OR_GREATER || NET46_OR_GREATER
		String resultCStringCString = CString.GetBytes(resultCString).AsSpan()[..^1].ToUtf16();
#else
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString).AsSpan()[..^1].ToArray());
#endif

		PInvokeAssert.Equal(expectedCString, resultCStringCString);
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
		PInvokeAssert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
#else
		PInvokeAssert.True(expectedResultCString.AsSpan()
		                                        .SequenceEqual(CString.GetBytes(resultCString).AsSpan()[..^1]));
#endif
	}
	private static async Task EmptyTestAsync(String?[] values)
	{
		CString resultCString = await CString.ConcatAsync(values);
		PInvokeAssert.NotNull(resultCString);
		PInvokeAssert.Equal(CString.Empty, resultCString);
		PInvokeAssert.Same(CString.Empty, resultCString);
		PInvokeAssert.True(resultCString.IsNullTerminated);
		PInvokeAssert.False(resultCString.IsReference);
		PInvokeAssert.False(resultCString.IsSegmented);
	}
#endif
	private static void NormalTest(String?[] strings)
	{
		String expectedCString = String.Concat(strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = CString.Concat(strings);

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);
#elif NETCOREAPP2_0_OR_GREATER || NET46_OR_GREATER
		String resultCStringCString = CString.GetBytes(resultCString).AsSpan()[..^1].ToUtf16();
#else
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString).AsSpan()[..^1].ToArray());
#endif

		PInvokeAssert.Equal(expectedCString, resultCStringCString);
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
		PInvokeAssert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
#else
		PInvokeAssert.True(expectedResultCString.AsSpan()
		                                        .SequenceEqual(CString.GetBytes(resultCString).AsSpan()[..^1]));
#endif
	}
	private static void EmptyTest(String?[] values)
	{
		CString resultCString = CString.Concat(values);
		PInvokeAssert.NotNull(resultCString);
		PInvokeAssert.Equal(CString.Empty, resultCString);
		PInvokeAssert.Same(CString.Empty, resultCString);
		PInvokeAssert.True(resultCString.IsNullTerminated);
		PInvokeAssert.False(resultCString.IsReference);
		PInvokeAssert.False(resultCString.IsSegmented);
	}
}