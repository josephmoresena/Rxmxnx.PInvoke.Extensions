#if NET9_0_OR_GREATER
using IEnumerator = System.Collections.IEnumerator;

namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	public readonly ref partial struct Utf8View : IEquatable<Utf8View>
	{
		/// <inheritdoc/>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		public Boolean Equals(Utf8View other) => this == other;

		public ref partial struct Enumerator : IEnumerator<ReadOnlySpan<Byte>>
		{
			Object? IEnumerator.Current => null;
			void IDisposable.Dispose() { }
		}
	}
}
#endif