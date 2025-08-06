namespace Rxmxnx.PInvoke;

public static partial class AotInfo
{
	/// <summary>
	/// Provides information about the Mono runtime.
	/// </summary>
	private static class MonoInfo
	{
		/// <summary>
		/// Indicates whether internal UTF-8 empty text is loaded in a read-only memory section.
		/// </summary>
		public static readonly Boolean IsEmptyNonLiteral;

		/// <summary>
		/// Static constructor.
		/// </summary>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3963)]
#endif
		static unsafe MonoInfo()
		{
#if !PACKAGE
			fixed (Byte* ptr = &MemoryMarshal.GetReference(AotInfo.EmptyUt8Text()))
#else
			fixed (Byte* ptr = CString.Empty)
#endif
				MonoInfo.IsEmptyNonLiteral = MemoryInspector.MayBeNonLiteral<Byte>(new(ptr, 1));
		}
	}
}