#if NET9_0_OR_GREATER
namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public unsafe ref partial struct ReadOnlyFixedContextValue<T> : IFixedPointerOperators<ReadOnlyFixedContextValue<T>>,
#if !OBSOLETE_FIXED_INTERFACES
	IReadOnlyFixedContext<T>
#else
#pragma warning disable CS0612
	IObsoleteReadOnlyFixedContext<T>
#pragma warning restore CS0612
#endif
{
	#if OBSOLETE_FIXED_INTERFACES
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
#if OBSOLETE_FIXED_INTERFACES
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
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IReadOnlyFixedContext<TDestination> IReadOnlyFixedContext<T>.Transformation<TDestination>(
		out IReadOnlyFixedMemory residual)
	{
		this._value.ValidateOperation(true);
		this._value.ValidateTransformation(typeof(TDestination),
		                                   !RuntimeHelpers.IsReferenceOrContainsReferences<TDestination>());
		if (this.IsNullOrEmpty)
		{
			residual = ReadOnlyFixedContext<Byte>.Empty;
			return ReadOnlyFixedContext<TDestination>.Empty;
		}
		FixedValueHandle handle = FixedPointerValue.GetValidationObject(this);
#pragma warning disable CS8500
		Int32 sizeOf = sizeof(TDestination);
		Int32 count = this._value.Size / sizeof(T);
#pragma warning restore CS8500
		Int32 offset = count * sizeOf;
		residual = offset == 0 ?
			ReadOnlyFixedContext<Byte>.Empty :
			new((this._value.Pointer + offset).ToPointer(), this._value.Size - offset, handle);
		return new ReadOnlyFixedContext<TDestination>(this._value.Pointer.ToPointer(), count, handle);
	}
}
#endif