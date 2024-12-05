namespace Rxmxnx.PInvoke.Tests;

[StructLayout(LayoutKind.Sequential)]
internal readonly struct ManagedStruct
{
	public String Text { get; init; }
	public Int32 Value { get; init; }
	public Object Obj { get; init; }
	public DateTime Date { get; init; }

	public static TFixture Register<TFixture>(TFixture fixture) where TFixture : IFixture
	{
		fixture.Register<ManagedStruct>(() => new()
		{
			Text = fixture.Create<String>(),
			Value = fixture.Create<Int32>(),
			Obj = fixture.Create<Object>(),
			Date = fixture.Create<DateTime>(),
		});
		return fixture;
	}
}