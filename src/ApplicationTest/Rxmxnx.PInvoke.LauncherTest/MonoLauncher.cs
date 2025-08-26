namespace Rxmxnx.PInvoke.ApplicationTest;

public sealed record MonoLauncher
{
	public String MsbuildPath { get; init; } = default!;
	public String LinkerPath { get; init; } = default!;
	public String MakerPath { get; init; } = default!;
	public String ExecutablePath { get; init; } = default!;
	public Architecture Architecture { get; init; }
}