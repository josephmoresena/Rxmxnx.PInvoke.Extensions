#if NET9_0_OR_GREATER
namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public readonly unsafe ref partial struct FixedContextValue<T> : IFixedPointerOperators<FixedContextValue<T>>,
#if !OBSOLETE_FIXED_INTERFACES
	IFixedContext<T>
#else
#pragma warning disable CS0612
	IObsoleteFixedContext<T>
#pragma warning restore CS0612
#endif
{
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => this.Bytes;
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	ReadOnlySpan<Object> IReadOnlyFixedMemory.Objects => this.Objects;
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	ReadOnlySpan<T> IReadOnlyFixedMemory<T>.Values => this.Values;
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	ReadOnlyValPtr<T> IReadOnlyFixedMemory<T>.ValuePointer => this.ValuePointer;

#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IFixedContext<Byte> IFixedMemory.AsBinaryContext()
	{
		this._value.ValidateOperation();
		this._value.ValidateTransformation(typeof(Byte), true);
		if (this.IsNullOrEmpty) return FixedContext<Byte>.Empty;
		FixedValueHandle handle = FixedPointerValue.GetValidationObject(this);
		return new FixedContext<Byte>(this._value.Pointer.ToPointer(), this._value.Size, handle);
	}
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext()
		=> FixedPointerValue.AsBinaryContext<T, FixedContextValue<T>>(this);
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IFixedContext<Object> IFixedMemory.AsObjectContext()
	{
		this._value.ValidateOperation();
		this._value.ValidateTransformation(typeof(Object), true);
		if (this.IsNullOrEmpty) return FixedContext<Object>.Empty;
		FixedValueHandle handle = FixedPointerValue.GetValidationObject(this);
		Int32 count = this._value.Size / IntPtr.Size;
		return new FixedContext<Object>(this._value.Pointer.ToPointer(), count, handle);
	}
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IReadOnlyFixedContext<Object> IReadOnlyFixedMemory.AsObjectContext()
		=> FixedPointerValue.AsObjectContext<T, FixedContextValue<T>>(this);
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IFixedMemory residual)
	{
		this._value.ValidateOperation();
		this._value.ValidateTransformation(typeof(TDestination),
		                                   !RuntimeHelpers.IsReferenceOrContainsReferences<TDestination>());
		if (this.IsNullOrEmpty)
		{
			residual = FixedContext<Byte>.Empty;
			return FixedContext<TDestination>.Empty;
		}
		FixedValueHandle handle = FixedPointerValue.GetValidationObject(this);
#pragma warning disable CS8500
		Int32 sizeOf = sizeof(TDestination);
		Int32 count = this._value.Size / sizeof(T);
#pragma warning restore CS8500
		Int32 offset = count * sizeOf;
		residual = offset == 0 ?
			FixedContext<Byte>.Empty :
			new((this._value.Pointer + offset).ToPointer(), this._value.Size - offset, handle);
		return new FixedContext<TDestination>(this._value.Pointer.ToPointer(), count, handle);
	}
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		ref IFixedMemory refResidual = ref Unsafe.As<IReadOnlyFixedMemory, IFixedMemory>(ref residual);
		return FixedPointerValue.Transformation<T, FixedContextValue<T>, TDestination>(this, out refResidual);
	}
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IReadOnlyFixedContext<TDestination> IReadOnlyFixedContext<T>.Transformation<TDestination>(
		out IReadOnlyFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		ref IFixedMemory refResidual = ref Unsafe.As<IReadOnlyFixedMemory, IFixedMemory>(ref residual);
		return FixedPointerValue.Transformation<T, FixedContextValue<T>, TDestination>(this, out refResidual);
	}
}
#endif