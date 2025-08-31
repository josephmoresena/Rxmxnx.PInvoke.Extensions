namespace Rxmxnx.PInvoke.ApplicationTest;

public sealed record MonoLauncher
{
	public Architecture Architecture { get; init; }
	public String NativeRuntimeName { get; init; } = default!;
	public String MsbuildPath { get; init; } = default!;
	public String LinkerPath { get; init; } = default!;
	public String MakerPath { get; init; } = default!;
	public String ExecutablePath { get; init; } = default!;
	public String MonoCilStripAssemblyPath { get; init; } = default!;
	public String NativeRuntimePath { get; init; } = default!;
}