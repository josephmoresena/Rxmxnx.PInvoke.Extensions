#if NET7_0_OR_GREATER
namespace Rxmxnx.PInvoke;

/// <summary>
/// Defines a provider that maps identifiers to UTF-8 literal byte sequences.
/// </summary>
/// <typeparam name="TIdentifier">The type used to identify a UTF-8 literal.</typeparam>
public interface IUtf8LiteralSource<in TIdentifier>
{
	/// <summary>
	/// Gets the UTF-8 byte sequence associated with the specified identifier.
	/// </summary>
	/// <param name="identifier">The identifier of the UTF-8 literal to retrieve.</param>
	/// <returns>
	/// A read-only span containing the UTF-8 encoded bytes associated with <paramref name="identifier"/>.
	/// </returns>
	static abstract ReadOnlySpan<Byte> GetLiteral(TIdentifier identifier);
}
#endif