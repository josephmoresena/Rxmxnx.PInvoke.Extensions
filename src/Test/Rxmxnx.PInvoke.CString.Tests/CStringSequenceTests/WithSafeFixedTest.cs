namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class WithSafeFixedTest
{
	[Fact]
	public void Test()
	{
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices();
		CStringSequence seq = WithSafeFixedTest.CreateSequence(handle, indices, out CString?[] values);
		seq.WithSafeFixed(new FixedAction(values));
		seq.WithSafeFixed(new FixedFunction(values), out CStringSequence seq2);
		PInvokeAssert.Equal(seq, seq2);
	}

	internal static CStringSequence CreateSequence(TestMemoryHandle handle, IReadOnlyList<Int32> indices,
		out CString?[] values)
	{
		values = new CString[indices.Count];
		for (Int32 i = 0; i < values.Length; i++)
			values[i] = TestSet.GetCString(indices[i], handle);
		CStringSequence seq = new(values);
		return seq;
	}

	private readonly struct FixedAction(CString?[] values) : IFixedPointerListAction
	{
		public void Accept(scoped FixedPointerValueList list)
		{
			PInvokeAssert.True(list.IsReadOnly);
			for (Int32 i = 0; i < list.Count; i++)
			{
				ReadOnlyFixedContextValue<Byte> ctx = (ReadOnlyFixedContextValue<Byte>)list[i].Value;
				if (values[i] is { } value)
					PInvokeAssert.True(ctx.Values.SequenceEqual(value.AsSpan()));
				else
					PInvokeAssert.Equal(0, ctx.Values.Length);
			}
		}
	}

	private readonly struct FixedFunction(CString?[] values) : IFixedPointerListFunction<CStringSequence>
	{
		public CStringSequence Apply(scoped FixedPointerValueList list)
		{
			new FixedAction(values).Accept(list);
			CStringSequence.Builder builder = CStringSequence.CreateBuilder();
			foreach (FixedPointerValueList.ItemValue item in list)
			{
				ReadOnlyFixedContextValue<Byte> ctx = (ReadOnlyFixedContextValue<Byte>)item.Value;
				if (ctx.Pointer == IntPtr.Zero)
				{
					builder.Append(default(CString?));
					continue;
				}
				builder.Append(ctx.Values);
			}
			return builder.Build();
		}
	}
}