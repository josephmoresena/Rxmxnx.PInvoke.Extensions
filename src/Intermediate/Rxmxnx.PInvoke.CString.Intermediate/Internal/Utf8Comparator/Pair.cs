namespace Rxmxnx.PInvoke.Internal;

internal partial class Utf8Comparator
{
	/// <summary>
	/// Represents a fixed-size pair of unmanaged values of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">An unmanaged value type whose instances are stored contiguously.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[StructLayout(LayoutKind.Sequential)]
	private readonly struct Pair<T> where T : unmanaged
	{
		/// <summary>
		/// First <typeparamref name="T"/> item.
		/// </summary>
		public readonly T T0;
		/// <summary>
		/// Second <typeparamref name="T"/> item.
		/// </summary>
		public readonly T T1;
	}
}