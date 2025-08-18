#if !NETCOREAPP
using InlineData = NUnit.Framework.TestCaseAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.CStringTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class StringJoinEmptyTest
{
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void LocalEmptyTest(Boolean emptyData)
	{
		String? separator = !emptyData ? String.Empty : default;
		String?[] values = !emptyData ? Enumerable.Repeat(String.Empty, 3).ToArray() : [];
		StringJoinEmptyTest.Test(separator, values);
		StringJoinEmptyTest.Test(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void NullEmptyTest(Boolean emptyData)
	{
		String? separator = !emptyData ? String.Empty : default;
		String?[] values = !emptyData ? Enumerable.Repeat<String?>(default, 3).ToArray() : [];
		StringJoinEmptyTest.Test(separator, values);
		StringJoinEmptyTest.Test(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public async Task LocalEmptyTestAsync(Boolean emptyData)
	{
		String? separator = !emptyData ? String.Empty : default;
		String?[] values = !emptyData ? Enumerable.Repeat(String.Empty, 3).ToArray() : [];
		await StringJoinEmptyTest.TestAsync(separator, values);
		await StringJoinEmptyTest.TestAsync(separator, values.ToList());
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public async Task NullEmptyTestAsync(Boolean emptyData)
	{
		String? separator = !emptyData ? String.Empty : default;
		String?[] values = !emptyData ? Enumerable.Repeat<String?>(default, 3).ToArray() : [];
		await StringJoinEmptyTest.TestAsync(separator, values);
		await StringJoinEmptyTest.TestAsync(separator, values.ToList());
	}

	private static void Test(String? separator, String?[] values)
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
	private static void Test(String? separator, IEnumerable<String?> values)
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
	private static async Task TestAsync(String? separator, String?[] values)
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
	private static async Task TestAsync(String? separator, IEnumerable<String?> values)
	{
		using MemoryHandle _ = CString.Empty.TryPin(out Boolean pinned);
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