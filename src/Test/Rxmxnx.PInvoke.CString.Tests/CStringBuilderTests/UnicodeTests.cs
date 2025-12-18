#if !NETCOREAPP
using InlineData = NUnit.Framework.TestCaseAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.CStringBuilderTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class UnicodeTests : CStringBuilderTestsBase
{
	[Theory]
	[InlineData(null)]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void AppendTest(Int32? length)
	{
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices(length);
		StringBuilder strBuild = new();
		CStringBuilder cstrBuild = new();
		foreach (String? newString in indices.Select(i => TestSet.GetString(i, true)))
		{
			strBuild.Append(newString);
			PInvokeAssert.True(Object.ReferenceEquals(cstrBuild, cstrBuild.Append(newString)));
		}
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ToString());
	}
	[Theory]
	[InlineData(null)]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void InsertTest(Int32? length)
	{
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices(length);
		StringBuilder strBuild = new();
		CStringBuilder cstrBuild = new();
		String? seed = TestSet.GetString(indices.OrderBy(_ => Guid.NewGuid()).FirstOrDefault(), true);

		strBuild.Append(seed);
		cstrBuild.Append(seed);

		foreach (String? newString in indices.Select(i => TestSet.GetString(i, true)))
		{
			(Int32 utf16Index, Int32 utf8Index) = CStringBuilderTestsBase.GetIndex(strBuild.ToString(), out _);
			strBuild.Insert(utf16Index, newString);
			PInvokeAssert.True(Object.ReferenceEquals(cstrBuild, cstrBuild.Insert(utf8Index, newString)));
		}
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ToString());
	}
}