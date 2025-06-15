#if !PACKAGE || !NET6_0_OR_GREATER

namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

internal static partial class MemoryMarshalCompat
{
	/// <summary>
	/// Delegate for .NET 5.0+ of GetArrayDataReference method.
	/// </summary>
	private delegate ref Byte GetArrayDataReferenceDelegate(Array array);

	/// <summary>
	/// Minimal representation of CoreCLR runtime MethodTable struct.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	private struct MMethodTable
	{
		[FieldOffset(4)]
		public UInt32 BaseSize;
	}

#pragma warning disable CS0649
	/// <summary>
	/// CoreCLR Object data representation.
	/// </summary>
	private sealed class CoreClrRawData
	{
		/// <summary>
		/// Object data. The value of this field should not be used, only the reference to it.
		/// </summary>
		public Byte Data;
	}

	/// <summary>
	/// Mono Array data representation.
	/// </summary>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3260)]
#endif
	[StructLayout(LayoutKind.Sequential)]
	private class MonoRawData
	{
		/// <summary>
		/// Pointer to bounds array.
		/// </summary>
		public IntPtr Bounds;
		/// <summary>
		/// Total number of items.
		/// </summary>
		public IntPtr Count;
		/// <summary>
		/// Object data. The value of this field should not be used, only the reference to it.
		/// </summary>
		public Byte Data;
	}
#pragma warning restore CS0649
}
#endif