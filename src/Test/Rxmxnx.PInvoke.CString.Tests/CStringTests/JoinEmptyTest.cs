#if !NETCOREAPP
using InlineData = NUnit.Framework.TestCaseAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.CStringTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class JoinEmptyTest
{
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void LocalEmptySpanTest(Boolean emptyData)
	{
		ReadOnlySpan<Byte> separator = default;
		CString?[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : [];
		JoinEmptyTest.Test(separator, values);
		JoinEmptyTest.Test(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReferenceEmptySpanTest(Boolean emptyData)
	{
		ReadOnlySpan<Byte> separator = default;
		CString?[] values = !emptyData ? Enumerable.Repeat<CString>(new(IntPtr.Zero, default), 3).ToArray() : [];
		JoinEmptyTest.Test(separator, values);
		JoinEmptyTest.Test(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void NullEmptySpanTest(Boolean emptyData)
	{
		ReadOnlySpan<Byte> separator = default;
		CString?[] values = !emptyData ? Enumerable.Repeat<CString?>(default, 3).ToArray() : [];
		JoinEmptyTest.Test(separator, values);
		JoinEmptyTest.Test(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void LocalEmptyTest(Boolean emptyData)
	{
		CString? separator = !emptyData ? CString.Empty : default;
		CString?[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : [];
		JoinEmptyTest.Test(separator, values);
		JoinEmptyTest.Test(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReferenceEmptyTest(Boolean emptyData)
	{
		CString? separator = !emptyData ? CString.Empty : default;
		CString?[] values = !emptyData ? Enumerable.Repeat<CString>(new(IntPtr.Zero, default), 3).ToArray() : [];
		JoinEmptyTest.Test(separator, values);
		JoinEmptyTest.Test(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void NullEmptyTest(Boolean emptyData)
	{
		CString? separator = !emptyData ? CString.Empty : default;
		CString?[] values = !emptyData ? Enumerable.Repeat<CString?>(default, 3).ToArray() : [];
		JoinEmptyTest.Test(separator, values);
		JoinEmptyTest.Test(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public async Task LocalEmptyTestAsync(Boolean emptyData)
	{
		CString? separator = !emptyData ? CString.Empty : default;
		CString?[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : [];
		await JoinEmptyTest.TestAsync(separator, values);
		await JoinEmptyTest.TestAsync(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public async Task ReferenceEmptyTestAsync(Boolean emptyData)
	{
		CString? separator = !emptyData ? CString.Empty : default;
		CString?[] values = !emptyData ? Enumerable.Repeat<CString>(new(IntPtr.Zero, default), 3).ToArray() : [];
		await JoinEmptyTest.TestAsync(separator, values);
		await JoinEmptyTest.TestAsync(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public async Task NullEmptyTestAsync(Boolean emptyData)
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
		PInvokeAssert.NotNull(resultCString);
		PInvokeAssert.Equal(CString.Empty, resultCString);
		PInvokeAssert.Same(CString.Empty, resultCString);
		PInvokeAssert.True(resultCString.IsNullTerminated);
		PInvokeAssert.False(resultCString.IsReference);
		PInvokeAssert.False(resultCString.IsSegmented);
		PInvokeAssert.Equal(resultCString.Length == 0 && CString.Empty.IsFunction, resultCString.IsFunction);

		PInvokeAssert.NotNull(resultCString2);
		PInvokeAssert.Equal(CString.Empty, resultCString2);
		PInvokeAssert.Same(CString.Empty, resultCString2);
		PInvokeAssert.True(resultCString2.IsNullTerminated);
		PInvokeAssert.False(resultCString2.IsReference);
		PInvokeAssert.False(resultCString2.IsSegmented);
		PInvokeAssert.Equal(resultCString2.Length == 0 && CString.Empty.IsFunction, resultCString2.IsFunction);
	}
	private static void Test(ReadOnlySpan<Byte> separator, IEnumerable<CString?> values)
	{
		CString resultCString = CString.Join(separator, values);
		PInvokeAssert.NotNull(resultCString);
		PInvokeAssert.Equal(CString.Empty, resultCString);
		PInvokeAssert.Same(CString.Empty, resultCString);
		PInvokeAssert.True(resultCString.IsNullTerminated);
		PInvokeAssert.False(resultCString.IsReference);
		PInvokeAssert.False(resultCString.IsSegmented);
		PInvokeAssert.Equal(resultCString.Length == 0 && CString.Empty.IsFunction, resultCString.IsFunction);
	}
	private static void Test(CString? separator, CString?[] values)
	{
		Int32 count = values.Length > 0 ? 1 : 0;
		CString resultCString = CString.Join(separator, values);
		CString resultCString2 = CString.Join(separator, values, 0, count);
		PInvokeAssert.NotNull(resultCString);
		PInvokeAssert.Equal(CString.Empty, resultCString);
		PInvokeAssert.Same(CString.Empty, resultCString);
		PInvokeAssert.True(resultCString.IsNullTerminated);
		PInvokeAssert.False(resultCString.IsReference);
		PInvokeAssert.False(resultCString.IsSegmented);
		PInvokeAssert.Equal(resultCString.Length == 0 && CString.Empty.IsFunction, resultCString.IsFunction);

		PInvokeAssert.NotNull(resultCString2);
		PInvokeAssert.Equal(CString.Empty, resultCString2);
		PInvokeAssert.Same(CString.Empty, resultCString2);
		PInvokeAssert.True(resultCString2.IsNullTerminated);
		PInvokeAssert.False(resultCString2.IsReference);
		PInvokeAssert.False(resultCString2.IsSegmented);
		PInvokeAssert.Equal(resultCString2.Length == 0 && CString.Empty.IsFunction, resultCString2.IsFunction);
	}
	private static void Test(CString? separator, IEnumerable<CString?> values)
	{
		CString resultCString = CString.Join(separator, values);
		PInvokeAssert.NotNull(resultCString);
		PInvokeAssert.Equal(CString.Empty, resultCString);
		PInvokeAssert.Same(CString.Empty, resultCString);
		PInvokeAssert.True(resultCString.IsNullTerminated);
		PInvokeAssert.False(resultCString.IsReference);
		PInvokeAssert.False(resultCString.IsSegmented);
		PInvokeAssert.Equal(resultCString.Length == 0 && CString.Empty.IsFunction, resultCString.IsFunction);
	}
	private static async Task TestAsync(CString? separator, CString?[] values)
	{
		Int32 count = values.Length > 0 ? 1 : 0;
		CString resultCString = await CString.JoinAsync(separator, values);
		CString resultCString2 = await CString.JoinAsync(separator, values, 0, count);
		PInvokeAssert.NotNull(resultCString);
		PInvokeAssert.Equal(CString.Empty, resultCString);
		PInvokeAssert.Same(CString.Empty, resultCString);
		PInvokeAssert.True(resultCString.IsNullTerminated);
		PInvokeAssert.False(resultCString.IsReference);
		PInvokeAssert.False(resultCString.IsSegmented);
		PInvokeAssert.Equal(resultCString.Length == 0 && CString.Empty.IsFunction, resultCString.IsFunction);

		PInvokeAssert.NotNull(resultCString2);
		PInvokeAssert.Equal(CString.Empty, resultCString2);
		PInvokeAssert.Same(CString.Empty, resultCString2);
		PInvokeAssert.True(resultCString2.IsNullTerminated);
		PInvokeAssert.False(resultCString2.IsReference);
		PInvokeAssert.False(resultCString2.IsSegmented);
		PInvokeAssert.Equal(resultCString2.Length == 0 && CString.Empty.IsFunction, resultCString2.IsFunction);
	}
	private static async Task TestAsync(CString? separator, IEnumerable<CString?> values)
	{
		CString resultCString = await CString.JoinAsync(separator, values);
		PInvokeAssert.NotNull(resultCString);
		PInvokeAssert.Equal(CString.Empty, resultCString);
		PInvokeAssert.Same(CString.Empty, resultCString);
		PInvokeAssert.True(resultCString.IsNullTerminated);
		PInvokeAssert.False(resultCString.IsReference);
		PInvokeAssert.False(resultCString.IsSegmented);
		PInvokeAssert.Equal(resultCString.Length == 0 && CString.Empty.IsFunction, resultCString.IsFunction);
	}
}