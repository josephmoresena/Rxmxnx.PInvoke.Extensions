namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class JoinEmptyTest
{
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal void LocalEmptySpanTest(Boolean emptyData)
	{
		ReadOnlySpan<Byte> separator = default;
		CString?[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : [];
		JoinEmptyTest.Test(separator, values);
		JoinEmptyTest.Test(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal void ReferenceEmptySpanTest(Boolean emptyData)
	{
		ReadOnlySpan<Byte> separator = default;
		CString?[] values = !emptyData ? Enumerable.Repeat<CString>(new(IntPtr.Zero, default), 3).ToArray() : [];
		JoinEmptyTest.Test(separator, values);
		JoinEmptyTest.Test(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal void NullEmptySpanTest(Boolean emptyData)
	{
		ReadOnlySpan<Byte> separator = default;
		CString?[] values = !emptyData ? Enumerable.Repeat<CString?>(default, 3).ToArray() : [];
		JoinEmptyTest.Test(separator, values);
		JoinEmptyTest.Test(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal void LocalEmptyTest(Boolean emptyData)
	{
		CString? separator = !emptyData ? CString.Empty : default;
		CString?[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : [];
		JoinEmptyTest.Test(separator, values);
		JoinEmptyTest.Test(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal void ReferenceEmptyTest(Boolean emptyData)
	{
		CString? separator = !emptyData ? CString.Empty : default;
		CString?[] values = !emptyData ? Enumerable.Repeat<CString>(new(IntPtr.Zero, default), 3).ToArray() : [];
		JoinEmptyTest.Test(separator, values);
		JoinEmptyTest.Test(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal void NullEmptyTest(Boolean emptyData)
	{
		CString? separator = !emptyData ? CString.Empty : default;
		CString?[] values = !emptyData ? Enumerable.Repeat<CString?>(default, 3).ToArray() : [];
		JoinEmptyTest.Test(separator, values);
		JoinEmptyTest.Test(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal async Task LocalEmptyTestAsync(Boolean emptyData)
	{
		CString? separator = !emptyData ? CString.Empty : default;
		CString?[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : [];
		await JoinEmptyTest.TestAsync(separator, values);
		await JoinEmptyTest.TestAsync(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal async Task ReferenceEmptyTestAsync(Boolean emptyData)
	{
		CString? separator = !emptyData ? CString.Empty : default;
		CString?[] values = !emptyData ? Enumerable.Repeat<CString>(new(IntPtr.Zero, default), 3).ToArray() : [];
		await JoinEmptyTest.TestAsync(separator, values);
		await JoinEmptyTest.TestAsync(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal async Task NullEmptyTestAsync(Boolean emptyData)
	{
		CString? separator = !emptyData ? CString.Empty : default;
		CString?[] values = !emptyData ? Enumerable.Repeat<CString?>(default, 3).ToArray() : [];
		await JoinEmptyTest.TestAsync(separator, values);
		await JoinEmptyTest.TestAsync(separator, values.ToList());
	}

	private static void Test(ReadOnlySpan<Byte> separator, CString?[] values)
	{
		Int32 count = values.Length > 0 ? 1 : 0;
		CString resultCString = CString.Join(separator, values);
		CString resultCString2 = CString.Join(separator, values, 0, count);
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
	private static void Test(ReadOnlySpan<Byte> separator, IEnumerable<CString?> values)
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
	private static void Test(CString? separator, CString?[] values)
	{
		Int32 count = values.Length > 0 ? 1 : 0;
		CString resultCString = CString.Join(separator, values);
		CString resultCString2 = CString.Join(separator, values, 0, count);
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
	private static void Test(CString? separator, IEnumerable<CString?> values)
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
	private static async Task TestAsync(CString? separator, CString?[] values)
	{
		Int32 count = values.Length > 0 ? 1 : 0;
		CString resultCString = await CString.JoinAsync(separator, values);
		CString resultCString2 = await CString.JoinAsync(separator, values, 0, count);
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
	private static async Task TestAsync(CString? separator, IEnumerable<CString?> values)
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