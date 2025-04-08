namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	/// <summary>
	/// Supported OS.
	/// </summary>
	private enum OperatingSystem : Byte
	{
		/// <summary>
		/// Linux OS.
		/// </summary>
		Linux,
		/// <summary>
		/// Mac OS.
		/// </summary>
		Mac,
		/// <summary>
		/// Windows OS.
		/// </summary>
		Windows,
	}
}