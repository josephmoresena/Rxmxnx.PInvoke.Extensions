namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public unsafe partial class CStringSequence
{
	/// <summary>
	/// State for pinned <see cref="CString"/> span.
	/// </summary>
#if NET9_0_OR_GREATER
	private readonly ref struct CStringSpanState
#else
	private readonly struct CStringSpanState
#endif
	{
#pragma warning disable CS8500
		/// <summary>
		/// <see cref="CString"/> pointer.
		/// </summary>
		public CString?* Ptr
		{
#if NETFRAMEWORK || NETSTANDARD2_0
			[SecurityCritical]
#endif
			get;
#if NETFRAMEWORK || NETSTANDARD2_0
			[SecurityCritical]
#endif
			init;
		}
		/// <summary>
		/// Span length.
		/// </summary>
		public Int32 Length
		{
#if NETFRAMEWORK || NETSTANDARD2_0
			[SecurityCritical]
#endif
			get;
#if NETFRAMEWORK || NETSTANDARD2_0
			[SecurityCritical]
#endif
			init;
		}
#pragma warning restore CS8500
	}
}