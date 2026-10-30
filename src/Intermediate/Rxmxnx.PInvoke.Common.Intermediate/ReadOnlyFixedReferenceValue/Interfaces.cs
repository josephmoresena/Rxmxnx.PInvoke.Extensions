#if NET9_0_OR_GREATER
namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public readonly unsafe ref partial struct
	ReadOnlyFixedReferenceValue<T> : IFixedPointerOperators<ReadOnlyFixedReferenceValue<T>>, IReadOnlyFixedReference<T>
	where T : allows ref struct
{
#pragma warning disable CS8500
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
	Boolean IEquatable<IReadOnlyReferenceable<T>>.Equals(IReadOnlyReferenceable<T>? other)
		=> other is not null &&
			Unsafe.AreSame(ref Unsafe.AsRef(in this.Reference), ref Unsafe.AsRef(in other.Reference));

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
	IReadOnlyFixedReference<TDestination> IReadOnlyFixedReference<T>.Transformation<TDestination>()
		=> this.TransformationInterface<TDestination>(out _);
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IReadOnlyFixedReference<TDestination> IReadOnlyFixedReference<T>.Transformation<TDestination>(
		out IReadOnlyFixedMemory residual)
		=> this.TransformationInterface<TDestination>(out residual);

#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private IReadOnlyFixedReference<TDestination> TransformationInterface<TDestination>(
		out IReadOnlyFixedMemory residual)
	{
		this._value.ValidateOperation(true);
		this._value.ValidateTransformation(typeof(TDestination),
		                                   !RuntimeHelpers.IsReferenceOrContainsReferences<TDestination>());
		Int32 sizeOf = sizeof(TDestination);
		this._value.ValidateReferenceSize(typeof(TDestination), sizeOf);
		if (this.IsNullOrEmpty)
		{
			ReadOnlyFixedReference<TDestination> empty = new(null);
			residual = new ReadOnlyFixedOffset(empty, 0);
			return empty;
		}
		FixedValueHandle handle = FixedPointerValue.GetValidationObject(this);
		ReadOnlyFixedReference<T> source = new(this._value.Pointer.ToPointer(), this._value.Size, handle);
		ReadOnlyFixedReference<TDestination> result =
			source.GetTransformation<TDestination>(out ReadOnlyFixedOffset offset);
		residual = offset;
		return result;
	}
#pragma warning restore CS8500
}
#endif