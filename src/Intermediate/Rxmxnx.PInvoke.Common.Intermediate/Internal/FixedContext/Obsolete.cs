namespace Rxmxnx.PInvoke.Internal;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal sealed partial class FixedContext<T>
{
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		IFixedContext<TDestination> result =
			this.GetTransformation<TDestination>(out Unsafe.As<IFixedMemory, FixedOffset>(ref residual));
		return result;
	}
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	IReadOnlyFixedContext<TDestination> IReadOnlyFixedContext<T>.Transformation<TDestination>(
		out IReadOnlyFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		IReadOnlyFixedContext<TDestination> result =
			this.GetTransformation<TDestination>(out Unsafe.As<IReadOnlyFixedMemory, FixedOffset>(ref residual), true);
		return result;
	}
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		IFixedContext<TDestination> result =
			this.GetTransformation<TDestination>(out Unsafe.As<IReadOnlyFixedMemory, FixedOffset>(ref residual), true);
		return result;
	}
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.GetTransformation<Byte>(out _, true);

	/// <inheritdoc/>
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	public override IFixedContext<Byte> AsBinaryContext() => this.GetTransformation<Byte>(out _);
}