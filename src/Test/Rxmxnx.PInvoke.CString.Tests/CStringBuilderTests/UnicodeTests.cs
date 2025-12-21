using ChunkInfo = Rxmxnx.PInvoke.Internal.DebugView.CStringBuilderDebugView.ChunkInfo;
#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
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
	public void AppendArrayTest(Int32? length)
	{
		List<Int32> indices = TestSet.GetIndices(length);
		StringBuilder strBuild = new();
		CStringBuilder cstrBuild = new();
		foreach (String? newString in indices.Select(i => TestSet.GetString(i, true)))
		{
			Char[]? chars = newString?.ToCharArray();
			strBuild.Append(chars);
			PInvokeAssert.True(Object.ReferenceEquals(cstrBuild, cstrBuild.Append(chars)));
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
	public void AppendLineTest(Int32? length)
	{
		List<Int32> indices = TestSet.GetIndices(length);
		StringBuilder strBuild = new();
		CStringBuilder cstrBuild = new();
		foreach (String? newString in indices.Select(i => TestSet.GetString(i, true)))
		{
			strBuild.AppendLine(newString);
			PInvokeAssert.True(Object.ReferenceEquals(cstrBuild, cstrBuild.AppendLine(newString)));
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
	public void AppendLineArrayTest(Int32? length)
	{
		List<Int32> indices = TestSet.GetIndices(length);
		StringBuilder strBuild = new();
		CStringBuilder cstrBuild = new();
		foreach (String? newString in indices.Select(i => TestSet.GetString(i, true)))
		{
			strBuild.AppendLine(newString);
			PInvokeAssert.True(Object.ReferenceEquals(cstrBuild, cstrBuild.AppendLine(newString?.ToCharArray())));
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
	[Fact]
	public void AppendRemoveAppendTest()
	{
		Int32 indexSeed = CStringBuilderTestsBase.GetSeedIndex();
		String? seed = TestSet.GetString(indexSeed);
		Int32 seedLength = Encoding.UTF8.GetByteCount((ReadOnlySpan<Char>)seed);
		if (seedLength < CStringBuilder.DefaultCapacity) return;

		CStringBuilder cstrBuild = new((UInt16)(seedLength * 2));
		cstrBuild.Append(seed).Append(seed).Append(seed);

		PInvokeAssert.Equal(seed + seed + seed, cstrBuild.GetDebugInfo(out Int32 length, out ChunkInfo[] chunks));
		PInvokeAssert.Equal(3 * seedLength, length);
		PInvokeAssert.Equal(2, chunks.Length);
		PInvokeAssert.Equal(2 * seedLength, chunks[0].Size);
		PInvokeAssert.Equal(seedLength, chunks[1].Used);
		PInvokeAssert.Equal(chunks[0].Size, chunks[0].Used);
		PInvokeAssert.True(chunks[1].Size > chunks[1].Used);

		cstrBuild.Remove(seedLength, seedLength * 2);
		PInvokeAssert.Equal(seed, cstrBuild.GetDebugInfo(out length, out chunks));
		PInvokeAssert.Equal(seedLength, length);
		PInvokeAssert.Equal(2, chunks.Length);
		PInvokeAssert.Equal(seedLength, chunks[0].Used);
		PInvokeAssert.Equal(0, chunks[1].Used);
		PInvokeAssert.True(chunks[0].Size > chunks[0].Used);
		PInvokeAssert.True(chunks[1].Size > chunks[1].Used);

		cstrBuild.Append(seed);
		PInvokeAssert.Equal(seed + seed, cstrBuild.GetDebugInfo(out length, out chunks));
		PInvokeAssert.Equal(2 * seedLength, length);
		PInvokeAssert.Equal(2, chunks.Length);
		PInvokeAssert.Equal(2 * seedLength, chunks[0].Used);
		PInvokeAssert.Equal(0, chunks[1].Used);
		PInvokeAssert.Equal(chunks[0].Size, chunks[0].Used);

		cstrBuild.Append(seed);
		PInvokeAssert.Equal(seed + seed + seed, cstrBuild.GetDebugInfo(out length, out chunks));
		PInvokeAssert.Equal(3 * seedLength, length);
		PInvokeAssert.Equal(2, chunks.Length);
		PInvokeAssert.Equal(2 * seedLength, chunks[0].Used);
		PInvokeAssert.Equal(seedLength, chunks[1].Used);
	}
}