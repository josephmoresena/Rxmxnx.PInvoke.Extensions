namespace Rxmxnx.PInvoke.Tests.Internal;

internal delegate String GetStringDelegate();
internal delegate Span<Byte> GetByteSpanDelegate(Byte[] bytes);
internal delegate Span<Guid> GetGuidSpanDelegate();
internal delegate void VoidDelegate();
internal delegate void VoidObjectDelegate(Object obj);

[ExcludeFromCodeCoverage]
internal sealed record FixedDelegateTestStatus
{
	public FixedDelegateTestStatus()
	{
		this.Status1 = default!;
		this.Status2 = default!;
		this.Status3 = default!;
		this.Status4 = default!;
		this.Status5 = default!;
		this.Status6 = default!;
	}
	public FixedDelegateStatus<GetStringDelegate> Status1 { get; init; }
	public FixedDelegateStatus<GetStringDelegate> Status2 { get; init; }
	public FixedDelegateStatus<GetByteSpanDelegate> Status3 { get; init; }
	public FixedDelegateStatus<VoidDelegate> Status4 { get; init; }
	public FixedDelegateStatus<VoidObjectDelegate> Status5 { get; init; }
	public FixedDelegateStatus<GetGuidSpanDelegate> Status6 { get; init; }
}