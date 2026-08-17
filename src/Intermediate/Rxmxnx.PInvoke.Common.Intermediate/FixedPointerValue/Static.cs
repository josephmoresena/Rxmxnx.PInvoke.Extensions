namespace Rxmxnx.PInvoke;

public readonly ref partial struct FixedPointerValue
{
	/// <summary>
	/// Determines whether two specified instances of <see cref="FixedPointerValue"/> are equal.
	/// </summary>
	/// <param name="value1">The first pointer to compare.</param>
	/// <param name="value2">The second pointer to compare.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="value1"/> equals <paramref name="value2"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean operator ==(FixedPointerValue value1, FixedPointerValue value2)
	{
		if (value1.IsUnmanaged && !value2.IsUnmanaged) return false;
		if (value1.IsReadOnly != value2.IsReadOnly) return false;
		if (!value1.IsUnmanaged || !value2.IsUnmanaged)
			if (value1.Type is { IsValueType: true, } || value2.Type is { IsValueType: true, })
				if (value1.Type != value2.Type)
					return false;
		if (value1.Pointer != value2.Pointer) return false;
		if (value1.Size != value2.Size) return false;
		if (value1.IsNullOrEmpty && !value2.IsNullOrEmpty) return false;
		return Object.Equals(value1.Handle, value2.Handle);
	}
	/// <summary>
	/// Determines whether two specified instances of <see cref="FixedPointerValue"/> are not equal.
	/// </summary>
	/// <param name="value1">The first pointer to compare.</param>
	/// <param name="value2">The second pointer to compare.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="value1"/> does not equal <paramref name="value2"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	/// <inheritdoc cref="IntPtr.op_Inequality(IntPtr, IntPtr)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean operator !=(FixedPointerValue value1, FixedPointerValue value2) => !(value1 == value2);

	/// <summary>
	/// Tries to create a <see cref="FixedPointerValue"/> from <paramref name="instance"/>
	/// </summary>
	/// <param name="instance">A <see cref="IFixedPointer"/> instance.</param>
	/// <param name="value">Output. A <see cref="FixedPointerValue"/> instance.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="instance"/> was successfully converted to
	/// <see cref="FixedPointerValue"/> value; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean TryCreateFixedValue(IFixedPointer instance, out FixedPointerValue value)
	{
		value = default;
		if (instance is not FixedPointer ptr || ptr.IsFunction) return false;
		value = new(instance.Pointer, ptr.BinaryLength)
		{
			Handle = (FixedValueHandle)ptr,
			Type = ptr.Type,
			IsUnmanaged = ptr.IsUnmanaged,
			IsReadOnly = ptr.IsReadOnly,
		};
		return true;
	}

#if NET9_0_OR_GREATER
	/// <summary>
	/// Retrieves the <see cref="IMutableWrapper{Boolean}"/> instance for <see cref="FixedPointer"/> instances.
	/// </summary>
	/// <typeparam name="TPointer">Type of the <see cref="IFixedPointer"/>.</typeparam>
	/// <returns>The <see cref="FixedValueHandle"/> instance for <see cref="FixedPointer"/> instances.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	internal static FixedValueHandle GetValidationObject<TPointer>(TPointer pointer)
		where TPointer : struct, IFixedPointerOperators<TPointer>, allows ref struct
	{
		FixedPointerValue value = pointer;
		if (value.Handle is null)
			ValidationUtilities.ThrowIfNotObject(typeof(TPointer));
		return value.Handle!;
	}
	/// <inheritdoc cref="IFixedContext{T}.Transformation{TDestination}(out IReadOnlyFixedMemory)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete(ObsoleteConstants.ObsoleteFixedInterface, ObsoleteConstants.ErrorFixedInterface)]
#endif
	internal static IFixedContext<TDestination>
		Transformation<T, TFixedContext, TDestination>(TFixedContext ctx, out IFixedMemory residual)
		where TFixedContext : IFixedContext<T>, allows ref struct
		=> ctx.Transformation<TDestination>(out residual);
	/// <inheritdoc cref="IFixedMemory.AsObjectContext()"/>
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	internal static IFixedContext<Object> AsObjectContext<T, TFixedContext>(TFixedContext ctx)
		where TFixedContext : IFixedMemory<T>, allows ref struct
		=> ctx.AsObjectContext();
	/// <inheritdoc cref="IFixedMemory.AsObjectContext()"/>
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	internal static IFixedContext<Byte> AsBinaryContext<T, TFixedContext>(TFixedContext ctx)
		where TFixedContext : IFixedMemory<T>, allows ref struct
		=> ctx.AsBinaryContext();
#endif
}