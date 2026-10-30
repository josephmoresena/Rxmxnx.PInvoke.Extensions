#if NET9_0_OR_GREATER
namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public readonly unsafe ref partial struct FixedReferenceValue<T> : IFixedPointerOperators<FixedReferenceValue<T>>,
	IFixedReference<T> where T : allows ref struct
{
#pragma warning disable CS8500
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	ref T IReferenceable<T>.Reference => ref this.Reference;
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	ref readonly T IReadOnlyReferenceable<T>.Reference => ref this.Reference;
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
	Span<Byte> IFixedMemory.Bytes => this.Bytes;
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	Span<Object> IFixedMemory.Objects => this.Objects;

#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	Boolean IEquatable<IReferenceable<T>>.Equals(IReferenceable<T>? other)
		=> other is not null && Unsafe.AreSame(ref this.Reference, ref other.Reference);
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	Boolean IEquatable<IReadOnlyReferenceable<T>>.Equals(IReadOnlyReferenceable<T>? other)
		=> other is not null && Unsafe.AreSame(ref this.Reference, ref Unsafe.AsRef(in other.Reference));

#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
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
#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
	[Obsolete]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext()
	{
		this._value.ValidateOperation(true);
		this._value.ValidateTransformation(typeof(Byte), true);
		if (this.IsNullOrEmpty) return ReadOnlyFixedContext<Byte>.Empty;
		FixedValueHandle handle = FixedPointerValue.GetValidationObject(this);
		return new ReadOnlyFixedContext<Byte>(this._value.Pointer.ToPointer(), this._value.Size, handle);
	}
#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
	[Obsolete]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IFixedContext<Object> IFixedMemory.AsObjectContext()
	{
		this._value.ValidateOperation();
		this._value.ValidateTransformation(typeof(Object), false);
		if (this.IsNullOrEmpty) return FixedContext<Object>.Empty;
		FixedValueHandle handle = FixedPointerValue.GetValidationObject(this);
		Int32 count = this._value.Size / IntPtr.Size;
		return new FixedContext<Object>(this._value.Pointer.ToPointer(), count, handle);
	}
#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
	[Obsolete]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IReadOnlyFixedContext<Object> IReadOnlyFixedMemory.AsObjectContext()
	{
		this._value.ValidateOperation(true);
		this._value.ValidateTransformation(typeof(Object), false);
		if (this.IsNullOrEmpty) return ReadOnlyFixedContext<Object>.Empty;
		FixedValueHandle handle = FixedPointerValue.GetValidationObject(this);
		Int32 count = this._value.Size / IntPtr.Size;
		return new ReadOnlyFixedContext<Object>(this._value.Pointer.ToPointer(), count, handle);
	}

#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IFixedReference<TDestination> IFixedReference<T>.Transformation<TDestination>()
	{
		Unsafe.SkipInit(out IFixedMemory residual);
		return FixedPointerValue.TransformationReference<T, FixedReferenceValue<T>, TDestination>(this, out residual);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IFixedReference<TDestination> IFixedReference<T>.Transformation<TDestination>(out IFixedMemory residual)
	{
		this._value.ValidateOperation();
		this._value.ValidateTransformation(typeof(TDestination),
		                                   !RuntimeHelpers.IsReferenceOrContainsReferences<TDestination>());
		Int32 sizeOf = sizeof(TDestination);
		this._value.ValidateReferenceSize(typeof(TDestination), sizeOf);
		if (this.IsNullOrEmpty)
		{
			FixedReference<TDestination> empty = new(null);
			residual = new FixedOffset(empty, 0);
			return empty;
		}
		FixedValueHandle handle = FixedPointerValue.GetValidationObject(this);
		FixedReference<T> source = new(this._value.Pointer.ToPointer(), this._value.Size, handle);
		FixedReference<TDestination> result = source.GetTransformation<TDestination>(out FixedOffset offset);
		residual = offset;
		return result;
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IFixedReference<TDestination> IFixedReference<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		ref IFixedMemory refResidual = ref Unsafe.As<IReadOnlyFixedMemory, IFixedMemory>(ref residual);
		return FixedPointerValue.TransformationReference<T, FixedReferenceValue<T>, TDestination>(
			this, out refResidual);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IReadOnlyFixedReference<TDestination> IReadOnlyFixedReference<T>.Transformation<TDestination>()
	{
		Unsafe.SkipInit(out IFixedMemory residual);
		return FixedPointerValue.TransformationReference<T, FixedReferenceValue<T>, TDestination>(this, out residual);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IReadOnlyFixedReference<TDestination> IReadOnlyFixedReference<T>.Transformation<TDestination>(
		out IReadOnlyFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		ref IFixedMemory refResidual = ref Unsafe.As<IReadOnlyFixedMemory, IFixedMemory>(ref residual);
		return FixedPointerValue.TransformationReference<T, FixedReferenceValue<T>, TDestination>(
			this, out refResidual);
	}
#pragma warning restore CS8500
}
#endif