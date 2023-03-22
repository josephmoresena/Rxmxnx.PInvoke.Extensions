namespace Rxmxnx.PInvoke.Tests.Internal;

public sealed class ReferenceableWrapper<T> : IReadOnlyReferenceable<T>
{
    private readonly IReadOnlyReferenceable<T> _referenceable;

    public ReferenceableWrapper(IReadOnlyReferenceable<T> referenceable)
    {
        this._referenceable = referenceable;
    }

    ref readonly T? IReadOnlyReferenceable<T>.Reference => ref this._referenceable.Reference;
}