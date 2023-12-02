namespace Rxmxnx.PInvoke.Tests.Internal;

[ExcludeFromCodeCoverage]
internal sealed record FixedDelegateStatus<TDelegate> where TDelegate : Delegate
{
	public FixedDelegate<TDelegate> Fixed { get; }
	public Boolean IsFunction { get; }

	public TDelegate Delegate { get; init; }
	public FixedDelegateStatus(Boolean isFunction, FixedDelegate<TDelegate> fd)
	{
		this.IsFunction = isFunction;
		this.Fixed = fd;
		this.Delegate = default!;
	}
}