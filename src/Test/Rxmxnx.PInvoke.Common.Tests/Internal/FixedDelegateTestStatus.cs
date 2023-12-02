namespace Rxmxnx.PInvoke.Tests.Internal;

internal delegate String GetStringDelegate();
internal delegate Span<Byte> GetByteSpanDelegate(Byte[] bytes);
internal delegate Span<Guid> GetGuidSpanDelegate();
internal delegate void VoidDelegate();
internal delegate void VoidObjectDelegate(Object obj);

[ExcludeFromCodeCoverage]
internal sealed record FixedDelegateTestStatus
{
	public FixedDelegateStatus<GetStringDelegate> Status1 { get; init; } = default!;
	public FixedDelegateStatus<GetStringDelegate> Status2 { get; init; } = default!;
	public FixedDelegateStatus<GetByteSpanDelegate> Status3 { get; init; } = default!;
	public FixedDelegateStatus<VoidDelegate> Status4 { get; init; } = default!;
	public FixedDelegateStatus<VoidObjectDelegate> Status5 { get; init; } = default!;
	public FixedDelegateStatus<GetGuidSpanDelegate> Status6 { get; init; } = default!;
}