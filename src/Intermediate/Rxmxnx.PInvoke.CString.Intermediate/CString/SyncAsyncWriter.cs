namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Helper class to perform sync writing in async context.
	/// </summary>
	/// <param name="instance">The current <see cref="CString"/> instance.</param>
	/// <param name="stream">
	/// The <see cref="Stream"/> where the contents of the current <see cref="CString"/> will be written.
	/// </param>
	private readonly struct SyncAsyncWriter(CString instance, Stream stream)
	{
		/// <summary>
		/// The index in the current instance where writing begins.
		/// </summary>
		public Int32 StartIndex { get; init; }
		/// <summary>
		/// The number of bytes to write from the current instance.
		/// </summary>
		public Int32 Count { get; init; }

		/// <summary>
		/// Performs write operation.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Write() => instance.Write(stream, this.StartIndex, this.Count);
	}
}