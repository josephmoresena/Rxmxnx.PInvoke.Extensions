#if !NETCOREAPP
using InlineData = NUnit.Framework.TestCaseAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.CStringBuilderTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class BasicTests
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
		Int32 builderLength = 0;
		foreach (Int32 i in indices)
		{
			String? newString = TestSet.GetString(i, true);
			CString? newCString = TestSet.GetCString(i, handle);

			strBuild.Append(newString);
			cstrBuild.Append(newCString);
			builderLength += newCString?.Length ?? 0;
			PInvokeAssert.Equal(builderLength, cstrBuild.Length);
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
	public void InsertRemoveTest(Int32? length)
	{
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices(length);
		StringBuilder strBuild = new();
		CStringBuilder cstrBuild = new();
		CStringBuilder? cstrBuildClone = new();
		Int32 seed = indices.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
		Int32 rune = 0;
		Int32 builderLength;
		String strBuildValue;

		strBuild.Append(TestSet.GetString(seed, true));
		cstrBuild.Append(TestSet.GetCString(seed, handle));
		cstrBuildClone?.Append(TestSet.GetCString(seed, handle));
		builderLength = cstrBuild.Length;
		foreach (Int32 i in indices)
		{
			String? newString = TestSet.GetString(i, true);
			CString? newCString = TestSet.GetCString(i, handle);
			(Int32 utf16Index, Int32 utf8Index) = BasicTests.GetIndex(strBuild.ToString(), out rune);
			strBuild.Insert(utf16Index, newString);
			cstrBuild.Insert(utf8Index, newCString);
			builderLength += newCString?.Length ?? 0;
			PInvokeAssert.Equal(builderLength, cstrBuild.Length);
			cstrBuildClone?.Insert(utf8Index, newCString);
		}
		PInvokeAssert.Equal(strBuildValue = strBuild.ToString(), cstrBuild.ToCString().ToString());
		if (rune < 100) return;

		Int32 minRune = (Int32)(0.5 * rune);
		while (rune > minRune && strBuild.Length > 0)
		{
			(Int32 utf16Start, Int32 utf8Start) = BasicTests.GetIndex(strBuildValue, out rune);
			(Int32 utf16End, Int32 utf8End) = BasicTests.GetIndex(strBuildValue.AsSpan()[utf16Start..], out _);

			strBuild.Remove(utf16Start, utf16End);
			cstrBuild.Remove(utf8Start, utf8End);
			strBuildValue = strBuild.ToString();
			PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ToCString().ToString());
			cstrBuildClone?.Remove(utf8Start, utf8End);
		}
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ToCString().ToString());
	}

	private static (Int32 utf16Index, Int32 utf8Index) GetIndex(ReadOnlySpan<Char> value, out Int32 nRune)
	{
		List<Int32> safeUtf16Indices = [];
		Int32 currentUtf16Index = 0;

		nRune = 0;
		while (currentUtf16Index < value.Length)
		{
			if (RuneCompat.DecodeFromUtf16(value[currentUtf16Index..], out _, out Int32 charsConsumed) ==
			    OperationStatus.Done)
			{
				if (currentUtf16Index <= value.Length)
					safeUtf16Indices.Add(currentUtf16Index);
				currentUtf16Index += charsConsumed;
				nRune++;
			}
			else
			{
				break;
			}
		}
		if (safeUtf16Indices.Count == 0) return (0, 0);

		Int32 randomIndexInList = Random.Shared.Next(0, safeUtf16Indices.Count);
		Int32 utf16Index = safeUtf16Indices[randomIndexInList];
		Int32 utf8Index = Encoding.UTF8.GetByteCount(value[..utf16Index]);

		return (utf16Index, utf8Index);
	}

#if !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}