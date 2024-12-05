namespace Rxmxnx.PInvoke.Tests;

[StructLayout(LayoutKind.Sequential)]
internal readonly struct ManagedStruct
{
	public String Text { get; init; }
	public DateTime Date { get; init; }

	public static TFixture Register<TFixture>(TFixture fixture) where TFixture : IFixture
	{
		fixture.Register<ManagedStruct>(() => new()
		{
			Text = fixture.Create<String>(), Date = fixture.Create<DateTime>(),
		});
		return fixture;
	}
}