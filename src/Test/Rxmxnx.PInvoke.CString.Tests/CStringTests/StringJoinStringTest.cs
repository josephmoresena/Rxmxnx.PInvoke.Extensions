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
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		PInvokeAssert.Equal(expectedCString, resultCStringCString);
		PInvokeAssert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
	private static void EnumerableTest(String? separator, IEnumerable<String?> strings)
	{
		String? strSeparator = separator;
		String expectedCString = String.Join(strSeparator, strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = CString.Join(separator, strings);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		PInvokeAssert.Equal(expectedCString, resultCStringCString);
		PInvokeAssert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
	private static void ArrayRangeTest(String? separator, String?[] strings)
	{
		Int32 startIndex = Random.Shared.Next(0, strings.Length);
		Int32 count = Random.Shared.Next(startIndex, strings.Length) - startIndex;
		String? strSeparator = separator;
		String expectedCString = String.Join(strSeparator, strings, startIndex, count);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = CString.Join(separator, strings, startIndex, count);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		PInvokeAssert.Equal(expectedCString, resultCStringCString);
		PInvokeAssert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
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
		Int32 startIndex = Random.Shared.Next(0, strings.Length);
		Int32 count = Random.Shared.Next(startIndex, strings.Length) - startIndex;
		String? strSeparator = separator;
		String expectedCString = String.Join(strSeparator, strings, startIndex, count);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = await CString.JoinAsync(separator, strings, startIndex, count);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
#endif
	private static String? GetCStringSeparator()
	{
		Int32 result = Random.Shared.Next(-3, TestSet.Utf16Text.Count);
		return TestSet.GetString(result);
	}
#if !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}