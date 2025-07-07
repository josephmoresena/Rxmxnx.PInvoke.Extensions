namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class JoinCStringTest
{
	[Fact]
	internal void Test()
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
		CString?[] values = TestSet.GetValues(indices, handle);

		JoinCStringTest.ArrayTest(JoinCStringTest.GetCStringSeparator(handle), strings, values);
		JoinCStringTest.ArrayRangeTest(JoinCStringTest.GetCStringSeparator(handle), strings, values);
		JoinCStringTest.EnumerableTest(JoinCStringTest.GetCStringSeparator(handle), strings, values);
	}

#if NET6_0_OR_GREATER
	[Fact]
	internal async Task TestAsync()
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
		CString?[] values = TestSet.GetValues(indices, handle);

		await JoinCStringTest.ArrayTestAsync(JoinCStringTest.GetCStringSeparator(handle), strings, values);
		await JoinCStringTest.ArrayRangeTestAsync(JoinCStringTest.GetCStringSeparator(handle), strings, values);
		await JoinCStringTest.EnumerableTestAsync(JoinCStringTest.GetCStringSeparator(handle), strings, values);
	}
#endif

	private static void ArrayTest(CString? separator, String?[] strings, CString?[] values)
	{
		String? strSeparator = separator?.ToString();
		String expectedCString = String.Join(strSeparator, strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = CString.Join(separator, values);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
	private static void EnumerableTest(CString? separator, String?[] strings, IEnumerable<CString?> values)
	{
		String? strSeparator = separator?.ToString();
		String expectedCString = String.Join(strSeparator, strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = CString.Join(separator, values);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
	private static void ArrayRangeTest(CString? separator, String?[] strings, CString?[] values)
	{
		Int32 startIndex = Random.Shared.Next(0, strings.Length);
		Int32 count = Random.Shared.Next(startIndex, strings.Length) - startIndex;
		String? strSeparator = separator?.ToString();
		String expectedCString = String.Join(strSeparator, strings, startIndex, count);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = CString.Join(separator, values, startIndex, count);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}

#if NET6_0_OR_GREATER
	private static async Task ArrayTestAsync(CString? separator, String?[] strings, CString?[] values)
	{
		String? strSeparator = separator?.ToString();
		String expectedCString = String.Join(strSeparator, strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = await CString.JoinAsync(separator, values);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
	private static async Task EnumerableTestAsync(CString? separator, String?[] strings, IEnumerable<CString?> values)
	{
		String? strSeparator = separator?.ToString();
		String expectedCString = String.Join(strSeparator, strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = await CString.JoinAsync(separator, values);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
	private static async Task ArrayRangeTestAsync(CString? separator, String?[] strings, CString?[] values)
	{
		Int32 startIndex = Random.Shared.Next(0, strings.Length);
		Int32 count = Random.Shared.Next(startIndex, strings.Length) - startIndex;
		String? strSeparator = separator?.ToString();
		String expectedCString = String.Join(strSeparator, strings, startIndex, count);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = await CString.JoinAsync(separator, values, startIndex, count);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
#endif
	private static CString? GetCStringSeparator(TestMemoryHandle handle)
	{
		Int32 result = Random.Shared.Next(-3, TestSet.Utf16Text.Count);
		return TestSet.GetCString(result, handle);
	}
#if !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}