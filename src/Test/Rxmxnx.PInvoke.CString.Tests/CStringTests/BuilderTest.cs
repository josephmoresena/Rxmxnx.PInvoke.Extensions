#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
using InlineData = NUnit.Framework.TestCaseAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.CStringTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class BuilderTest
{
	[Theory]
	[InlineData(null)]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void SimpleTest(Int32? length)
	{
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices(length);
		StringBuilder strBuild = new();
		CString.Builder cstrBuild = new();
		foreach (Int32 i in indices)
		{
			strBuild.Append(TestSet.GetString(i, true));
			cstrBuild.Append(TestSet.GetCString(i, handle));
			PInvokeAssert.True(
				cstrBuild.ToCString().AsSpan().SequenceEqual(Encoding.UTF8.GetBytes(strBuild.ToString())));
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
		CString.Builder cstrBuild = new();
		Int32 seed = indices.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();

		strBuild.Append(TestSet.GetString(seed, true));
		cstrBuild.Append(TestSet.GetCString(seed, handle));
		foreach (Int32 i in indices)
		{
			String? newString = TestSet.GetString(i, true);
			CString? newCString = TestSet.GetCString(i, handle);
			(Int32 utf16Index, Int32 utf8Index) = BuilderTest.GetIndex(strBuild.ToString());
			strBuild.Insert(utf16Index, newString);
			cstrBuild.Insert(utf8Index, newCString);
		}
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ToCString().ToString());
	}
	private static (Int32 utf16Index, Int32 utf8Index) GetIndex(String value)
	{
		List<Int32> safeUtf16Indices = [];
		Int32 currentUtf16Index = 0;

		while (currentUtf16Index < value.Length)
		{
			if (RuneCompat.DecodeFromUtf16(value.AsSpan(currentUtf16Index), out _, out Int32 charsConsumed) ==
			    OperationStatus.Done)
			{
				if (currentUtf16Index <= value.Length)
					safeUtf16Indices.Add(currentUtf16Index);
				currentUtf16Index += charsConsumed;
			}
			else
			{
				break;
			}
		}
		if (safeUtf16Indices.Count == 0) return (0, 0);

		Int32 randomIndexInList = Random.Shared.Next(0, safeUtf16Indices.Count);
		Int32 utf16Index = safeUtf16Indices[randomIndexInList];
		Int32 utf8Index = Encoding.UTF8.GetByteCount(value.AsSpan(0, utf16Index));

		return (utf16Index, utf8Index);
	}

#if !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}