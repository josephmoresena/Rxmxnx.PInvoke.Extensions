namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class BuilderTest
{
	[Fact]
	public void EmptyTest()
	{
		CStringSequence.Builder builder = CStringSequence.CreateBuilder();
		CStringSequence build = builder.Build();

		PInvokeAssert.StrictEqual(CStringSequence.Empty, build);

		PInvokeAssert.Equal(
			builder,
			builder.Append(ReadOnlySpan<Byte>.Empty).Append(default(CString?)).Append(ReadOnlySpan<Char>.Empty)
			       .Append(default(String?)).Append(ReadOnlySequence<Byte>.Empty).Append(CString.Zero)
			       .Append(CString.Empty).Append(String.Empty));
		build = builder.Build();

		PInvokeAssert.Equal(String.Empty, build.ToString());
		PInvokeAssert.Equal(0, build.NonEmptyCount);
		PInvokeAssert.Equal(8, build.Count);
		PInvokeAssert.StrictEqual(CString.Empty, build[0]);
		PInvokeAssert.StrictEqual(CString.Zero, build[1]);
		PInvokeAssert.StrictEqual(CString.Empty, build[2]);
		PInvokeAssert.StrictEqual(CString.Zero, build[3]);
		PInvokeAssert.StrictEqual(CString.Empty, build[4]);
		PInvokeAssert.StrictEqual(CString.Zero, build[5]);
		PInvokeAssert.StrictEqual(CString.Empty, build[6]);
		PInvokeAssert.StrictEqual(CString.Empty, build[7]);

		PInvokeAssert.Equal(
			builder, builder.AppendEscaped(ReadOnlySequence<Byte>.Empty).AppendEscaped(ReadOnlySpan<Byte>.Empty));
		build = builder.Build();

		PInvokeAssert.Equal(String.Empty, build.ToString());
		PInvokeAssert.Equal(0, build.NonEmptyCount);
		PInvokeAssert.Equal(10, build.Count);
		PInvokeAssert.StrictEqual(CString.Empty, build[0]);
		PInvokeAssert.StrictEqual(CString.Zero, build[1]);
		PInvokeAssert.StrictEqual(CString.Empty, build[2]);
		PInvokeAssert.StrictEqual(CString.Zero, build[3]);
		PInvokeAssert.StrictEqual(CString.Empty, build[4]);
		PInvokeAssert.StrictEqual(CString.Zero, build[5]);
		PInvokeAssert.StrictEqual(CString.Empty, build[6]);
		PInvokeAssert.StrictEqual(CString.Empty, build[7]);
		PInvokeAssert.StrictEqual(CString.Empty, build[8]);
		PInvokeAssert.StrictEqual(CString.Empty, build[9]);

		PInvokeAssert.Equal(
			builder,
			builder.Insert(0, default(CString?)).Insert(0, default(String?)).RemoveAt(11).RemoveAt(10).RemoveAt(9)
			       .RemoveAt(8).RemoveAt(6).RemoveAt(4).RemoveAt(2));
		build = builder.Build();

		PInvokeAssert.Equal(String.Empty, build.ToString());
		PInvokeAssert.Equal(0, build.NonEmptyCount);
		PInvokeAssert.Equal(5, build.Count);
		PInvokeAssert.True(build.All(c => c.IsZero));

		PInvokeAssert.Equal(
			builder,
			builder.Insert(4, ReadOnlySpan<Char>.Empty).Insert(3, ReadOnlySpan<Byte>.Empty).RemoveAt(6).RemoveAt(4)
			       .RemoveAt(2).RemoveAt(1).RemoveAt(0).Insert(1, String.Empty).Insert(2, CString.Empty));
		build = builder.Build();

		PInvokeAssert.Equal(String.Empty, build.ToString());
		PInvokeAssert.Equal(0, build.NonEmptyCount);
		PInvokeAssert.Equal(4, build.Count);
		PInvokeAssert.True(build.All(c => Object.Equals(CString.Empty, c)));

		builder.Clear();
		build = builder.Build();
		PInvokeAssert.StrictEqual(CStringSequence.Empty, build);
	}
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
		String?[] strings = indices.Select(i => TestSet.GetString(i, true)).ToArray();
		CStringSequence seqRef = new(strings);
		CStringSequence.Builder builder = CStringSequence.CreateBuilder();

		PInvokeAssert.StrictEqual(CStringSequence.Empty, builder.Build());
		foreach (CString? value in TestSet.GetValues(indices, handle))
			builder.Append(value);
		PInvokeAssert.Equal(seqRef, builder.Build());
	}
	[Theory]
	[InlineData(null)]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void AppendStringTest(Int32? length)
	{
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices(length);
		String?[] strings = indices.Select(i => TestSet.GetString(i, true)).ToArray();
		CStringSequence seqRef = new(strings);
		CStringSequence.Builder builder = CStringSequence.CreateBuilder();

		foreach (CString? value in strings)
			builder.Append(value);
		PInvokeAssert.Equal(seqRef, builder.Build());
	}
	[Fact]
	public void AppendSpanTest()
	{
		CStringSequence seqRef = new(TestSet.Utf16Text);
		CStringSequence.Builder builder = CStringSequence.CreateBuilder();

		foreach (ReadOnlySpanFunc<Byte> value in TestSet.Utf8Text)
			builder.Append(value());
		PInvokeAssert.Equal(seqRef, builder.Build());

		builder.Clear();

		foreach (ReadOnlySpanFunc<Byte> value in TestSet.Utf8Text)
		{
#if NETCOREAPP
			ReadOnlySpan<Byte> encoded = JsonEncodedText.Encode(value()).EncodedUtf8Bytes;
#else
			ReadOnlySpan<Byte> encoded =
				Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Encoding.UTF8.GetString(value()))[1..^1]);
#endif
			builder.AppendEscaped(encoded);
		}
		PInvokeAssert.Equal(seqRef, builder.Build());
	}
	[Fact]
	public void AppendCharSpanTest()
	{
		CStringSequence.Builder builder = CStringSequence.CreateBuilder();
		foreach (String value in TestSet.Utf16Text)
			builder.Append(value.AsSpan());

		PInvokeAssert.Equal(new(TestSet.Utf16Text), builder.Build());
	}
	[Fact]
	public void AppendSequenceTest()
	{
		CStringSequence seqRef = new(TestSet.Utf16Text);
		CStringSequence.Builder builder = CStringSequence.CreateBuilder();

		foreach (Byte[] value in TestSet.Utf8Bytes)
			builder.Append(new ReadOnlySequence<Byte>(value));
		PInvokeAssert.Equal(seqRef, builder.Build());

		builder.Clear();

		foreach (String value in TestSet.Utf16Text)
		{
#if NETCOREAPP
			Byte[] encoded = JsonEncodedText.Encode(value).EncodedUtf8Bytes.ToArray();
#else
			Byte[] encoded = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)[1..^1]);
#endif
			builder.AppendEscaped(new ReadOnlySequence<Byte>(encoded));
		}
		PInvokeAssert.Equal(seqRef, builder.Build());
	}
}