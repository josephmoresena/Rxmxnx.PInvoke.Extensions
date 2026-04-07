using ChunkInfo = Rxmxnx.PInvoke.Internal.DebugView.CStringBuilderDebugView.ChunkInfo;

namespace Rxmxnx.PInvoke.Tests.CStringBuilderTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class BasicConcurrentTests : CStringBuilderTestsBase
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
			Assert.True(Object.ReferenceEquals(cstrBuild, cstrBuild.Append(newCString)));
			builderLength += newCString?.Length ?? 0;
			PInvokeAssert.Equal(builderLength, cstrBuild.ConcurrentLength());
			PInvokeAssert.True(cstrBuild.ConcurrentToCString().AsSpan()
			                            .SequenceEqual(Encoding.UTF8.GetBytes(strBuild.ToString())));
		}
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ConcurrentToString());
	}
	[Theory]
	[InlineData(null)]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void AppendByteSequenceTest(Int32? length)
	{
		List<Int32> indices = TestSet.GetIndices(length);
		StringBuilder strBuild = new();
		CStringBuilder cstrBuild = new();
		Int32 builderLength = 0;
		foreach (Int32 i in indices)
		{
			String? newString = TestSet.GetString(i, true);
			ReadOnlySequence<Byte> newByteSequence = !String.IsNullOrEmpty(newString) ?
				new(Encoding.UTF8.GetBytes(newString)) :
				default;

			strBuild.Append(newString);
			Assert.True(Object.ReferenceEquals(cstrBuild, cstrBuild.Append(newByteSequence)));
			builderLength += (Int32)newByteSequence.Length;
			PInvokeAssert.Equal(builderLength, cstrBuild.ConcurrentLength());
			PInvokeAssert.True(cstrBuild.ConcurrentToCString().AsSpan()
			                            .SequenceEqual(Encoding.UTF8.GetBytes(strBuild.ToString())));
		}
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ConcurrentToString());
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
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices(length);
		StringBuilder strBuild = new();
		CStringBuilder cstrBuild = new();
		foreach (Int32 i in indices)
		{
			Char[]? chars = TestSet.GetString(i, true)?.ToCharArray();
			Byte[]? bytes = TestSet.GetCString(i, handle)?.ToArray();

			strBuild.Append(chars);
			Assert.True(Object.ReferenceEquals(cstrBuild, cstrBuild.Append(bytes)));
		}
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ConcurrentToString());
	}
	[Theory]
	[InlineData(null)]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void AppendSequenceTest(Int32? length)
	{
		CStringSequence seq = new(TestSet.GetIndices(length).Select(i => TestSet.GetString(i)));
		CString value0 = seq.ToCString(false);
		CString value1 = new CStringBuilder().ConcurrentAppend(seq).ToCString(false);
		Assert.True(value0.AsSpan().SequenceEqual(value1.AsSpan()));
	}
	[Theory]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	public void AppendLineParameterlessTest(Int32 length)
	{
		StringBuilder strBuild = new();
		CStringBuilder cstrBuild = new();
		while (length > 0)
		{
			strBuild.AppendLine();
			Assert.True(Object.ReferenceEquals(cstrBuild, cstrBuild.ConcurrentAppendLine()));
			length--;
		}
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ConcurrentToString());
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
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices(length);
		StringBuilder strBuild = new();
		CStringBuilder cstrBuild = new();
		foreach (Int32 i in indices)
		{
			String? newString = TestSet.GetString(i, true);
			CString? newCString = TestSet.GetCString(i, handle);

			strBuild.AppendLine(newString);
			Assert.True(Object.ReferenceEquals(cstrBuild, cstrBuild.ConcurrentAppendLine(newCString)));
		}
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ConcurrentToString());
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
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices(length);
		StringBuilder strBuild = new();
		CStringBuilder cstrBuild = new();
		foreach (Int32 i in indices)
		{
			String? newString = TestSet.GetString(i, true);
			Byte[]? bytes = TestSet.GetCString(i, handle)?.ToArray();

			strBuild.AppendLine(newString);
			Assert.True(Object.ReferenceEquals(cstrBuild, cstrBuild.ConcurrentAppendLine(bytes)));
		}
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ConcurrentToString());
	}
	[Theory]
	[InlineData(null)]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	[InlineData(null, "|")]
	[InlineData(8, "|")]
	[InlineData(32, "|")]
	[InlineData(256, "|")]
	[InlineData(300, "|")]
	[InlineData(1000, "|")]
	[InlineData(null, ", ")]
	[InlineData(8, ", ")]
	[InlineData(32, ", ")]
	[InlineData(256, ", ")]
	[InlineData(300, ", ")]
	[InlineData(1000, ", ")]
	public void AppendJoinTest(Int32? length, String? separator = default)
	{
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices(length);
		StringBuilder strBuild = new();
		CStringBuilder cstrBuild = new();
		strBuild.AppendJoin(separator, indices.Select(i => TestSet.GetString(i, true)));
		PInvokeAssert.True(Object.ReferenceEquals(cstrBuild,
		                                          // ReSharper disable once AccessToDisposedClosure
		                                          cstrBuild.ConcurrentAppendJoin((CString?)separator,
			                                          indices.Select(i => TestSet.GetCString(i, handle)))));
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ConcurrentToString());
	}
	[Theory]
	[InlineData(null)]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	[InlineData(null, "|")]
	[InlineData(8, "|")]
	[InlineData(32, "|")]
	[InlineData(256, "|")]
	[InlineData(300, "|")]
	[InlineData(1000, "|")]
	[InlineData(null, ", ")]
	[InlineData(8, ", ")]
	[InlineData(32, ", ")]
	[InlineData(256, ", ")]
	[InlineData(300, ", ")]
	[InlineData(1000, ", ")]
	public void AppendJoinSequenceTest(Int32? length, String? separator = default)
	{
		List<Int32> indices = TestSet.GetIndices(length);
		String?[] values = indices.Select(i => TestSet.GetString(i, true)).ToArray();
		CStringSequence seq = new(values);
		StringBuilder strBuild = new();
		CStringBuilder cstrBuild = new();
		strBuild.AppendJoin(separator, values);
		PInvokeAssert.True(Object.ReferenceEquals(cstrBuild, cstrBuild.ConcurrentAppendJoin((CString?)separator, seq)));
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ConcurrentToString());
	}
	[Theory]
	[InlineData(null)]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	[InlineData(null, "|")]
	[InlineData(8, "|")]
	[InlineData(32, "|")]
	[InlineData(256, "|")]
	[InlineData(300, "|")]
	[InlineData(1000, "|")]
	[InlineData(null, ", ")]
	[InlineData(8, ", ")]
	[InlineData(32, ", ")]
	[InlineData(256, ", ")]
	[InlineData(300, ", ")]
	[InlineData(1000, ", ")]
	public void AppendJoinArrayTest(Int32? length, String? separator = default)
	{
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices(length);
		String?[] values = indices.Select(i => TestSet.GetString(i, true)).ToArray();
		CString?[] cValues = // ReSharper disable once AccessToDisposedClosure
			indices.Select(i => TestSet.GetCString(i, handle)).ToArray();
		StringBuilder strBuild = new();
		CStringBuilder cstrBuild = new();
		strBuild.AppendJoin(separator, values);
		PInvokeAssert.True(
			Object.ReferenceEquals(cstrBuild, cstrBuild.ConcurrentAppendJoin((CString?)separator, cValues)));
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ConcurrentToString());
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
		Int32 seed = indices.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
		StringBuilder strBuild = new(TestSet.GetString(seed, true));
		CStringBuilder cstrBuild = new(TestSet.GetCString(seed, handle));
		Int32 rune = 0;
		String strBuildValue;

		Int32 builderLength = cstrBuild.ConcurrentLength();
		foreach (Int32 i in indices)
		{
			String? newString = TestSet.GetString(i, true);
			CString? newCString = TestSet.GetCString(i, handle);
			(Int32 utf16Index, Int32 utf8Index) = CStringBuilderTestsBase.GetIndex(strBuild.ToString(), out rune);
			strBuild.Insert(utf16Index, newString);
			PInvokeAssert.True(Object.ReferenceEquals(cstrBuild, cstrBuild.ConcurrentInsert(utf8Index, newCString)));
			builderLength += newCString?.Length ?? 0;
			PInvokeAssert.Equal(builderLength, cstrBuild.ConcurrentLength());
		}
		PInvokeAssert.Equal(strBuildValue = strBuild.ToString(), cstrBuild.ConcurrentToCString().ToString());
		if (rune < 100) return;

		Int32 minRune = (Int32)(0.5 * rune);
		while (rune > minRune && strBuild.Length > 0)
		{
			(Int32 utf16Start, Int32 utf8Start) = CStringBuilderTestsBase.GetIndex(strBuildValue, out rune);
			(Int32 utf16End, Int32 utf8End) =
				CStringBuilderTestsBase.GetIndex(strBuildValue.AsSpan()[utf16Start..], out _);

			strBuild.Remove(utf16Start, utf16End);
			PInvokeAssert.True(Object.ReferenceEquals(cstrBuild, cstrBuild.ConcurrentRemove(utf8Start, utf8End)));
			strBuildValue = strBuild.ToString();
			PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ConcurrentToCString().ToString());
		}
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ConcurrentToCString().ToString());
		PInvokeAssert.Equal("", cstrBuild.ConcurrentClear().ToString());
		PInvokeAssert.Equal(0, cstrBuild.ConcurrentLength());
	}
	[Theory]
	[InlineData(8)]
	[InlineData(32)]
	public void AppendU8CharTest(Int32? length)
	{
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices(length);
		StringBuilder strBuild = new();
		CStringBuilder cstrBuild = new();
		foreach (Int32 i in indices)
		{
			strBuild.Append(TestSet.GetString(i, true));

			if (TestSet.GetCString(i, handle) is not { } cstr)
			{
				Assert.True(Object.ReferenceEquals(cstrBuild, cstrBuild.Append(default(Byte?))));
				continue;
			}

			foreach (Byte u8 in cstr.AsSpan())
			{
				CStringBuilder result = PInvokeRandom.Shared.Next(0, 3) < 2 ?
					cstrBuild.Append(u8) :
					cstrBuild.Append((Byte?)u8);
				Assert.True(Object.ReferenceEquals(cstrBuild, result));
			}
		}
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ConcurrentToString());
	}
	[Theory]
	[InlineData(8)]
	[InlineData(32)]
	public void InsertU8Test(Int32? length)
	{
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices(length);
		StringBuilder strBuild = new();
		CStringBuilder cstrBuild = new();
		String? seed = TestSet.GetString(indices.OrderBy(_ => Guid.NewGuid()).FirstOrDefault(), true);

		strBuild.Append(seed);
		cstrBuild.Append(seed);

		foreach (Int32 i in indices)
		{
			(Int32 utf16Index, Int32 utf8Index) = CStringBuilderTestsBase.GetIndex(strBuild.ToString(), out _);
			strBuild.Insert(utf16Index, TestSet.GetString(i, true));
			if (TestSet.GetCString(i, handle) is not { } cstr)
			{
				Assert.True(Object.ReferenceEquals(cstrBuild, cstrBuild.ConcurrentInsert(utf8Index, default(Byte?))));
				continue;
			}

			ReadOnlySpan<Byte> bytes = cstr;
			for (Int32 j = 0; j < bytes.Length; j++)
			{
				CStringBuilder result = PInvokeRandom.Shared.Next(0, 3) < 2 ?
					cstrBuild.ConcurrentInsert(utf8Index + j, bytes[j]) :
					cstrBuild.ConcurrentInsert(utf8Index + j, (Byte?)bytes[j]);
				Assert.True(Object.ReferenceEquals(cstrBuild, result));
			}
		}
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ConcurrentToString());
	}
	[Fact]
	public void AppendRemoveAppendTest()
	{
		using TestMemoryHandle handle = new();
		Int32 indexSeed = CStringBuilderTestsBase.GetSeedIndex();
		CString? seed = TestSet.GetCString(indexSeed, handle);
		Int32 seedLength = seed?.Length ?? 0;
		if (seed is null || seedLength < CStringBuilder.DefaultCapacity) return;

		CStringBuilder cstrBuild = new((UInt16)(seedLength * 2));
		cstrBuild.Append(seed).ConcurrentAppend(seed).ConcurrentAppend(seed);

		PInvokeAssert.Equal((seed + seed + seed).ToString(),
		                    cstrBuild.GetDebugInfo(out Int32 length, out ChunkInfo[] chunks));
		PInvokeAssert.Equal(3 * seedLength, length);
		PInvokeAssert.Equal(2, chunks.Length);
		PInvokeAssert.Equal(2 * seedLength, chunks[0].Size);
		PInvokeAssert.Equal(seedLength, chunks[1].Used);
		PInvokeAssert.Equal(chunks[0].Size, chunks[0].Used);
		PInvokeAssert.True(chunks[1].Size > chunks[1].Used);

		cstrBuild.ConcurrentRemove(seedLength, seedLength * 2);
		PInvokeAssert.Equal(seed.ToString(), cstrBuild.GetDebugInfo(out length, out chunks));
		PInvokeAssert.Equal(seedLength, length);
		PInvokeAssert.Equal(2, chunks.Length);
		PInvokeAssert.Equal(seedLength, chunks[0].Used);
		PInvokeAssert.Equal(0, chunks[1].Used);
		PInvokeAssert.True(chunks[0].Size > chunks[0].Used);
		PInvokeAssert.True(chunks[1].Size > chunks[1].Used);

		cstrBuild.Append(seed);
		PInvokeAssert.Equal((seed + seed).ToString(), cstrBuild.GetDebugInfo(out length, out chunks));
		PInvokeAssert.Equal(2 * seedLength, length);
		PInvokeAssert.Equal(2, chunks.Length);
		PInvokeAssert.Equal(2 * seedLength, chunks[0].Used);
		PInvokeAssert.Equal(0, chunks[1].Used);
		PInvokeAssert.Equal(chunks[0].Size, chunks[0].Used);

		cstrBuild.Append(seed);
		PInvokeAssert.Equal((seed + seed + seed).ToString(), cstrBuild.GetDebugInfo(out length, out chunks));
		PInvokeAssert.Equal(3 * seedLength, length);
		PInvokeAssert.Equal(2, chunks.Length);
		PInvokeAssert.Equal(2 * seedLength, chunks[0].Used);
		PInvokeAssert.Equal(seedLength, chunks[1].Used);
	}
}