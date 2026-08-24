namespace Rxmxnx.PInvoke.Internal;

internal partial class ReadOnlyFixedContext<T>
{
#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
	[Obsolete]
#endif
	IReadOnlyFixedContext<TDestination> IReadOnlyFixedContext<T>.Transformation<TDestination>(
		out IReadOnlyFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		IReadOnlyFixedContext<TDestination> result =
			this.GetTransformation<TDestination>(
				out Unsafe.As<IReadOnlyFixedMemory, ReadOnlyFixedOffset>(ref residual));
		return result;
	}
	/// <inheritdoc cref="IReadOnlyFixedMemory.AsBinaryContext()"/>
#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
	[Obsolete]
#endif
	public override IReadOnlyFixedContext<Byte> AsBinaryContext() => this.GetTransformation<Byte>(out _);
}