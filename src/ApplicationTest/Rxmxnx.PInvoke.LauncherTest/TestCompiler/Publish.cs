namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class TestCompiler
{
	[Flags]
	private enum Publish : Byte
	{
		SelfContained = 0x0,
		ReadyToRun = 0x1,
		NativeAot = 0x2,
		InvariantGlobalization = 0x4,
		NoReflection = 0x80,
		DisableBufferAutoComposition = 0x10,
	}
}