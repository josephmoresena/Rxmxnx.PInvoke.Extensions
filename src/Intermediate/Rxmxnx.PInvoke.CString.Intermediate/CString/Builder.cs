namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Represents a mutable <see cref="CString"/> instance.
	/// </summary>
	public sealed partial class Builder
	{
		/// <summary>
		/// The default capacity of a <see cref="Builder"/>.
		/// </summary>
		internal const Int32 DefaultCapacity = 32;

		/// <summary>
		/// Lock object.
		/// </summary>
#if NET9_0_OR_GREATER
		private readonly Lock _lock = new();
#else
		private readonly Object _lock = new();
#endif
		/// <summary>
		/// Current string chunk.
		/// </summary>
		private Chunk _chunk;
	}
}