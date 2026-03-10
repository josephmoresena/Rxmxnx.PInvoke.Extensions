namespace Rxmxnx.PInvoke.Tests.Internal;

[ExcludeFromCodeCoverage]
internal sealed record FixedDelegateTestStatus
{
	public FixedDelegateStatus<GetStringDelegate> Status1 { get; init; } = default!;
	public FixedDelegateStatus<GetStringDelegate> Status2 { get; init; } = default!;
#if NETCOREAPP
	public FixedDelegateStatus<GetByteSpanDelegate> Status3 { get; init; } = default!;
#endif
	public FixedDelegateStatus<VoidDelegate> Status4 { get; init; } = default!;
	public FixedDelegateStatus<VoidObjectDelegate> Status5 { get; init; } = default!;
#if NETCOREAPP
	public FixedDelegateStatus<GetGuidSpanDelegate> Status6 { get; init; } = default!;
#endif
}