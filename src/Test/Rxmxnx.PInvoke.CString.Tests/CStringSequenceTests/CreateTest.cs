namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[ExcludeFromCodeCoverage]
public sealed class CreateTest
{
	[Fact]
	internal void Test()
	{
		List<GCHandle> handles = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		try
		{
			CString?[] values = new CString[indices.Count];
			for (Int32 i = 0; i < values.Length; i++)
				values[i] = TestSet.GetCString(indices[i], handles);

			CStringSequence seq =
				CStringSequence.Create(values, CreateTest.CreationMethod, values.Select(x => x?.Length).ToArray());
			CStringSequence seq2 = new(values);

			for (Int32 i = 0; i < values.Length; i++)
			{
				if (seq[i].Length != 0 || !seq[i].IsReference)
					Assert.Equal(values[i], seq[i]);
				else
					Assert.Null(values[i]);
			}
		}
		finally
		{
			foreach (GCHandle handle in handles)
				handle.Free();
		}
	}

	private static void CreationMethod(Span<Byte> span, Int32 index, IReadOnlyList<CString?> values)
	{
		CString? value = values[index];
		Assert.False(CString.IsNullOrEmpty(value));
		Assert.Equal(span.Length, value.Length);

		value.AsSpan().CopyTo(span);
	}
}