namespace Rxmxnx.PInvoke.Tests.CStringTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class StringJoinStringTest
{
	[Fact]
	public void Test()
	{
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
		StringJoinStringTest.ArrayTest(StringJoinStringTest.GetCStringSeparator(), strings);
		StringJoinStringTest.ArrayRangeTest(StringJoinStringTest.GetCStringSeparator(), strings);
		StringJoinStringTest.EnumerableTest(StringJoinStringTest.GetCStringSeparator(), strings);
	}

#if NET6_0_OR_GREATER
	[Fact]
	internal async Task TestAsync()
	{
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
		await StringJoinStringTest.ArrayTestAsync(StringJoinStringTest.GetCStringSeparator(), strings);
		await StringJoinStringTest.ArrayRangeTestAsync(StringJoinStringTest.GetCStringSeparator(), strings);
		await StringJoinStringTest.EnumerableTestAsync(StringJoinStringTest.GetCStringSeparator(), strings);
	}
#endif

	private static void ArrayTest(String? separator, String?[] strings)
	{
		String? strSeparator = separator;
		String expectedCString = String.Join(strSeparator, strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = CString.Join(separator, strings);
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
	private static void EnumerableTest(String? separator, IEnumerable<String?> strings)
	{
		String? strSeparator = separator;
		// ReSharper disable once PossibleMultipleEnumeration
		String expectedCString = String.Join(strSeparator, strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		// ReSharper disable once PossibleMultipleEnumeration
		CString resultCString = CString.Join(separator, strings);
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
	private static void ArrayRangeTest(String? separator, String?[] strings)
	{
		Int32 startIndex = PInvokeRandom.Shared.Next(0, strings.Length);
		Int32 count = PInvokeRandom.Shared.Next(startIndex, strings.Length) - startIndex;
		String? strSeparator = separator;
		String expectedCString = String.Join(strSeparator, strings, startIndex, count);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = CString.Join(separator, strings, startIndex, count);
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
	private static String? GetCStringSeparator()
	{
		Int32 result = PInvokeRandom.Shared.Next(-3, TestSet.Utf16Text.Count);
		return TestSet.GetString(result);
	}

#if NET6_0_OR_GREATER
	private static async Task ArrayTestAsync(String? separator, String?[] strings)
	{
		String? strSeparator = separator;
		String expectedCString = String.Join(strSeparator, strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = await CString.JoinAsync(separator, strings);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
	private static async Task EnumerableTestAsync(String? separator, IEnumerable<String?> strings)
	{
		String? strSeparator = separator;
		String expectedCString = String.Join(strSeparator, strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = await CString.JoinAsync(separator, strings);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
	private static async Task ArrayRangeTestAsync(String? separator, String?[] strings)
	{
		Int32 startIndex = PInvokeRandom.Shared.Next(0, strings.Length);
		Int32 count = PInvokeRandom.Shared.Next(startIndex, strings.Length) - startIndex;
		String? strSeparator = separator;
		String expectedCString = String.Join(strSeparator, strings, startIndex, count);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = await CString.JoinAsync(separator, strings, startIndex, count);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
#endif
}