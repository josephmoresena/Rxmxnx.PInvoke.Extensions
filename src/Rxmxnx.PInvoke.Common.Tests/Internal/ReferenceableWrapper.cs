namespace Rxmxnx.PInvoke.Tests.Internal;

public class ReferenceableWrapper<T> : IReferenceable<T>
{
    private readonly IReferenceable<T> _referenceable;

    public ReferenceableWrapper(IReferenceable<T> referenceable)
    {
        this._referenceable = referenceable;
    }

    ref readonly T? IReferenceable<T>.Reference => ref this._referenceable.Reference;
}