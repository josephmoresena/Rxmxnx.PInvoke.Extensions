// ReSharper disable UnusedMember.Local
namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class GetValuesFixedContextTest
{
	[Fact]
	public void Test()
	{
		ReadOnlySpan<Probe> expected = NativeUtilities.GetEnumValuesSpan<Probe>();
		IDisposable release = NativeUtilities.GetValuesFixedContext(out ReadOnlyFixedContextValue<Probe> ctx);
		try
		{
			PInvokeAssert.Equal(expected.Length, ctx.Values.Length);
			for (Int32 index = 0; index < expected.Length; index++)
				PInvokeAssert.Equal(expected[index], ctx.Values[index]);
		}
		finally
		{
			release.Dispose();
		}
	}

	private enum Probe : Byte
	{
		None = 0,
		One = 1,
		Two = 2,
	}
}
