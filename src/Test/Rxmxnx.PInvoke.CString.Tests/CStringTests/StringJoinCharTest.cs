namespace Rxmxnx.PInvoke.Tests.CStringTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class StringJoinCharTest
{
	private readonly IFixture _fixture = new Fixture();

	[Fact]
	public void Test()
	{
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
		StringJoinCharTest.ArrayTest(this.GetByteSeparator(), strings);
		StringJoinCharTest.ArrayRangeTest(this.GetByteSeparator(), strings);
		StringJoinCharTest.EnumerableTest(this.GetByteSeparator(), strings);
	}

#if NET6_0_OR_GREATER
	[Fact]
	internal async Task TestAsync()
	{
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
		await StringJoinCharTest.ArrayTestAsync(this.GetByteSeparator(), strings);
		await StringJoinCharTest.ArrayRangeTestAsync(this.GetByteSeparator(), strings);
		await StringJoinCharTest.EnumerableTestAsync(this.GetByteSeparator(), strings);
	}
#endif

	private static void ArrayTest(Char separator, String?[] strings)
	{
		String strSeparator = new(new[] { separator, });
		String expectedCString = String.Join(strSeparator, strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = CString.Join(separator, strings);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		PInvokeAssert.Equal(expectedCString, resultCStringCString);
		PInvokeAssert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
		PInvokeAssert.Same(CString.Empty, CString.Join(separator));
	}
	private static void EnumerableTest(Char separator, IEnumerable<String?> strings)
	{
		String strSeparator = new(new[] { separator, });
		String expectedCString = String.Join(strSeparator, strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = CString.Join(separator, strings);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		PInvokeAssert.Equal(expectedCString, resultCStringCString);
		PInvokeAssert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
		PInvokeAssert.Same(CString.Empty, CString.Join(separator, Array.Empty<String?>().ToList()));
	}
	private static void ArrayRangeTest(Char separator, String?[] strings)
	{
		Int32 startIndex = PInvokeRandom.Shared.Next(0, strings.Length);
		Int32 count = PInvokeRandom.Shared.Next(startIndex, strings.Length) - startIndex;
		String strSeparator = new(new[] { separator, });
		String expectedCString = String.Join(strSeparator, strings, startIndex, count);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = CString.Join(separator, strings, startIndex, count);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		PInvokeAssert.Equal(expectedCString, resultCStringCString);
		PInvokeAssert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
		PInvokeAssert.Same(CString.Empty, CString.Join(separator, strings, 0, 0));
	}
	private Char GetByteSeparator() => this._fixture.Create<Char>();

#if NET6_0_OR_GREATER
	private static async Task ArrayTestAsync(Char separator, String?[] strings)
	{
		String strSeparator = new(new[] { separator, });
		String expectedCString = String.Join(strSeparator, strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = await CString.JoinAsync(separator, strings);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
	private static async Task EnumerableTestAsync(Char separator, IEnumerable<String?> strings)
	{
		String strSeparator = new(new[] { separator, });
		String expectedCString = String.Join(strSeparator, strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = await CString.JoinAsync(separator, strings);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
	private static async Task ArrayRangeTestAsync(Char separator, String?[] strings)
	{
		Int32 startIndex = PInvokeRandom.Shared.Next(0, strings.Length);
		Int32 count = PInvokeRandom.Shared.Next(startIndex, strings.Length) - startIndex;
		String strSeparator = new(new[] { separator, });
		String expectedCString = String.Join(strSeparator, strings, startIndex, count);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = await CString.JoinAsync(separator, strings, startIndex, count);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
#endif
}