namespace Rxmxnx.PInvoke.Tests.Internal;

[ExcludeFromCodeCoverage]
internal sealed class ReferenceableWrapper<T>(IReadOnlyReferenceable<T> referenceable) : IReferenceable<T>
{
	ref readonly T IReadOnlyReferenceable<T>.Reference => ref referenceable.Reference;

	ref T IReferenceable<T>.Reference => ref UnsafeLegacy.AsRef(in referenceable.Reference);
}