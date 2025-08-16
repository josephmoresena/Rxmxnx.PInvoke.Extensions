namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	/// <summary>
	/// MacOS implementation of <see cref="MemoryInspector"/> class.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	private sealed unsafe partial class Mac : MemoryInspector
	{
		/// <summary>
		/// Indicates whether memory marked as executable is treated as read-only.
		/// </summary>
		private readonly Boolean _readonlyExecutable = RuntimeInformation.OSArchitecture is Architecture.Arm64 &&
				RuntimeInformation.ProcessArchitecture is Architecture.Arm64 &&
#if !NET5_0_OR_GREATER
				RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
#else
				OperatingSystem.IsMacOS()
#endif
			;

		/// <inheritdoc/>
		public override Boolean IsLiteral(ReadOnlySpan<Byte> span)
		{
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
			{
				UInt32 taskHandle = SystemB.GetTaskHandle();
				UInt32 count = MemoryInfo.Count;
				Int32 result = SystemB.MemoryRegion(taskHandle, &ptr, out _, MemoryInfo.Flavor, out MemoryInfo info,
				                                    ref count, out _);
				SystemB.ValidateResult(result);
				if ((info.Protection & Protection.Read) == Protection.None) return false;
				if (this._readonlyExecutable && (info.Protection & Protection.Execute) == Protection.Execute)
					return true;
				return (info.Protection & Protection.Write) == Protection.None;
			}
		}
	}
}