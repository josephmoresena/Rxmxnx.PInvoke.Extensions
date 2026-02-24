namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class Mac
	{
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS4487)]
#endif
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		[Preserve(AllMembers = true, Conditional = true)]
		private readonly struct MemoryInfo
		{
			public static readonly UInt32 Flavor = MemoryInfo.SupportsFlavor64() ? 9u : 1u;
			public static readonly UInt32 Count = MemoryInfo.SupportsFlavor64() ? 9u : 8u;

			public readonly Protection Protection;
			public readonly Protection MaxProtection;
			public readonly Inheritance Inheritance;
			public readonly UInt32 Shared;
			public readonly UInt32 Reserved;
			public readonly UInt64 Offset;
			public readonly Behavior Behavior;
			public readonly UInt16 UserWiredCount;

			private readonly UInt16 _padding;

			private static Boolean SupportsFlavor64()
				=> Environment.Is64BitProcess || RuntimeInformation.ProcessArchitecture is Architecture.Arm64;
		}
	}
}