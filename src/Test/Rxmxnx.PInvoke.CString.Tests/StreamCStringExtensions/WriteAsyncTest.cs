namespace Rxmxnx.PInvoke.Tests.StreamCStringExtensions;

[ExcludeFromCodeCoverage]
public sealed class WriteAsyncTest
{
	[Fact]
	internal async Task BasicTestAsync()
	{
		List<GCHandle> handles = new();
		try
		{
			List<WritedCString> values = new();
			using MemoryStream strm = new();
			foreach (Int32 index in TestSet.GetIndices(10))
			{
				if (WritedCString.Create(TestSet.GetCString(index, handles)) is WritedCString writed)
				{
					values.Add(writed);
					await strm.WriteAsync(writed.Value);
				}
			}

			WritedCString.AssertWrite(strm, values, false);
		}
		finally
		{
			foreach (GCHandle handle in handles)
				handle.Free();
		}
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal async Task TestAsync(Boolean writeNullTermination)
	{
		List<GCHandle> handles = new();
		try
		{
			List<WritedCString> values = new();
			using MemoryStream strm = new();
			foreach (Int32 index in TestSet.GetIndices(10))
			{
				if (WritedCString.Create(TestSet.GetCString(index, handles)) is WritedCString writed)
				{
					values.Add(writed);
					await strm.WriteAsync(writed.Value, writeNullTermination);
				}
			}

			WritedCString.AssertWrite(strm, values, writeNullTermination);
		}
		finally
		{
			foreach (GCHandle handle in handles)
				handle.Free();
		}
	}

	[Fact]
	internal async Task RangeTestAsync()
	{
		List<GCHandle> handles = new();
		try
		{
			List<WritedCString> values = new();
			using MemoryStream strm = new();
			foreach (Int32 index in TestSet.GetIndices(10))
			{
				if (WritedCString.Create(TestSet.GetCString(index, handles), false) is WritedCString writed)
				{
					values.Add(writed);
					await strm.WriteAsync(writed.Value, writed.Start, writed.Count);
				}
			}

			WritedCString.AssertWrite(strm, values, false);
		}
		finally
		{
			foreach (GCHandle handle in handles)
				handle.Free();
		}
	}
}