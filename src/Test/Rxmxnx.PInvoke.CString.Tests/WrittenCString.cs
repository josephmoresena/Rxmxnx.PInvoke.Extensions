namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
internal sealed record WrittenCString
{
	public CString Value { get; }
	public Int32 Start { get; private init; }
	public Int32 Count { get; private init; }
	private WrittenCString(CString value) => this.Value = value;

	[return: NotNullIfNotNull(nameof(value))]
	public static WrittenCString? Create(CString? value, Boolean writeAllBytes = true)
	{
		if (value is null)
			return default;

		Int32 start = writeAllBytes ? 0 : Random.Shared.Next(0, value.Length);
		Int32 count = writeAllBytes ? value.Length : Random.Shared.Next(0, value.Length - start);

		return new(value) { Start = start, Count = count, };
	}

	public static void AssertWrite(MemoryStream strm, IReadOnlyList<WrittenCString> values,
		Boolean writeNullTermination)
	{
		Byte[] result = strm.ToArray();
		Int32 offset = 0;

		for (Int32 i = 0; i < values.Count; i++)
		{
			Assert.Equal(values[i].Value.Skip(values[i].Start).Take(values[i].Count),
			             result.Skip(offset).Take(values[i].Count));
			if (writeNullTermination)
			{
				Assert.Equal(0, result[offset + values[i].Value.Length]);
				offset += values[i].Value.Length + 1;
			}
			else
			{
				offset += values[i].Count;
			}
		}
	}
}