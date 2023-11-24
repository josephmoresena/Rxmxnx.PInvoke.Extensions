namespace Rxmxnx.PInvoke.Tests.Internal;

[ExcludeFromCodeCoverage]
internal sealed class ReferenceableWrapper<T> : IReadOnlyReferenceable<T>, IReferenceable<T>
{
	private readonly IReadOnlyReferenceable<T> _referenceable;

	public ReferenceableWrapper(IReadOnlyReferenceable<T> referenceable) => this._referenceable = referenceable;

	ref readonly T IReadOnlyReferenceable<T>.Reference => ref this._referenceable.Reference;

	ref T IReferenceable<T>.Reference => ref Unsafe.AsRef(in this._referenceable.Reference);
}