namespace Rxmxnx.PInvoke.Tests.Internal;

[ExcludeFromCodeCoverage]
internal sealed record FixedDelegateStatus<TDelegate> where TDelegate : Delegate
{
    private readonly Boolean _isFunction;
    private readonly FixedDelegate<TDelegate> _fixed;

    public FixedDelegate<TDelegate> Fixed => this._fixed;
    public Boolean IsFunction => this._isFunction;

    public TDelegate Delegate { get; init; }

    public FixedDelegateStatus(Boolean isFunction, FixedDelegate<TDelegate> fd)
    {
        this._isFunction = isFunction;
        this._fixed = fd;
        this.Delegate = default!;
    }
}
