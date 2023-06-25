namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class StringJoinEmptyTest
{
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal void LocalEmptyTest(Boolean emptyData)
	{
		String? separator = !emptyData ? String.Empty : default;
		String?[] values = !emptyData ? Enumerable.Repeat(String.Empty, 3).ToArray() : Array.Empty<String>();
		StringJoinEmptyTest.Test(separator, values);
		StringJoinEmptyTest.Test(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal void NullEmptyTest(Boolean emptyData)
	{
		String? separator = !emptyData ? String.Empty : default;
		String?[] values = !emptyData ? Enumerable.Repeat<String?>(default, 3).ToArray() : Array.Empty<String>();
		StringJoinEmptyTest.Test(separator, values);
		StringJoinEmptyTest.Test(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal async Task LocalEmptyTestAsync(Boolean emptyData)
	{
		String? separator = !emptyData ? String.Empty : default;
		String?[] values = !emptyData ? Enumerable.Repeat(String.Empty, 3).ToArray() : Array.Empty<String>();
		await StringJoinEmptyTest.TestAsync(separator, values);
		await StringJoinEmptyTest.TestAsync(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal async Task NullEmptyTestAsync(Boolean emptyData)
	{
		String? separator = !emptyData ? String.Empty : default;
		String?[] values = !emptyData ? Enumerable.Repeat<String?>(default, 3).ToArray() : Array.Empty<String>();
		await StringJoinEmptyTest.TestAsync(separator, values);
		await StringJoinEmptyTest.TestAsync(separator, values.ToList());
	}

	private static void Test(String? separator, String?[] values)
	{
		CString resultCString = CString.Join(separator, values);
		CString resultCString2 = CString.Join(separator, values, 0, 1);
		Assert.NotNull(resultCString);
		Assert.Equal(CString.Empty, resultCString);
		Assert.Same(CString.Empty, resultCString);
		Assert.True(resultCString.IsNullTerminated);
		Assert.False(resultCString.IsReference);
		Assert.False(resultCString.IsSegmented);
		Assert.False(resultCString.IsFunction);

		Assert.NotNull(resultCString2);
		Assert.Equal(CString.Empty, resultCString2);
		Assert.Same(CString.Empty, resultCString2);
		Assert.True(resultCString2.IsNullTerminated);
		Assert.False(resultCString2.IsReference);
		Assert.False(resultCString2.IsSegmented);
		Assert.False(resultCString2.IsFunction);
	}
	private static void Test(String? separator, IEnumerable<String?> values)
	{
		CString resultCString = CString.Join(separator, values);
		Assert.NotNull(resultCString);
		Assert.Equal(CString.Empty, resultCString);
		Assert.Same(CString.Empty, resultCString);
		Assert.True(resultCString.IsNullTerminated);
		Assert.False(resultCString.IsReference);
		Assert.False(resultCString.IsSegmented);
		Assert.False(resultCString.IsFunction);
	}
	private static async Task TestAsync(String? separator, String?[] values)
	{
		CString resultCString = await CString.JoinAsync(separator, values);
		CString resultCString2 = await CString.JoinAsync(separator, values, 0, 1);
		Assert.NotNull(resultCString);
		Assert.Equal(CString.Empty, resultCString);
		Assert.Same(CString.Empty, resultCString);
		Assert.True(resultCString.IsNullTerminated);
		Assert.False(resultCString.IsReference);
		Assert.False(resultCString.IsSegmented);
		Assert.False(resultCString.IsFunction);

		Assert.NotNull(resultCString2);
		Assert.Equal(CString.Empty, resultCString2);
		Assert.Same(CString.Empty, resultCString2);
		Assert.True(resultCString2.IsNullTerminated);
		Assert.False(resultCString2.IsReference);
		Assert.False(resultCString2.IsSegmented);
		Assert.False(resultCString2.IsFunction);
	}
	private static async Task TestAsync(String? separator, IEnumerable<String?> values)
	{
		CString resultCString = await CString.JoinAsync(separator, values);
		Assert.NotNull(resultCString);
		Assert.Equal(CString.Empty, resultCString);
		Assert.Same(CString.Empty, resultCString);
		Assert.True(resultCString.IsNullTerminated);
		Assert.False(resultCString.IsReference);
		Assert.False(resultCString.IsSegmented);
		Assert.False(resultCString.IsFunction);
	}
}