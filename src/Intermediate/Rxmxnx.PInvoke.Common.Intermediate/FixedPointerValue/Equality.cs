namespace Rxmxnx.PInvoke;

#if !NET9_0_OR_GREATER
public readonly ref partial struct FixedPointerValue
#else
public readonly ref partial struct FixedPointerValue : IFixedPointer, IEquatable<FixedPointerValue>
#endif
{
	/// <inheritdoc cref="Object.Equals(Object)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public Boolean Equals(FixedPointerValue other) => this == other;
	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public override Boolean Equals(Object? obj)
		=> obj switch
		{
			FixedPointer fp when FixedPointerValue.TryCreateFixedValue(fp, out FixedPointerValue p) => this == p,
			FixedPointerInfo info => this == info.GetValue(this.IsReadOnly, this.Handle),
			_ => false,
		};
	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public override Int32 GetHashCode()
	{
		HashCode result = new();
		result.Add(this.Pointer);
		result.Add(this.Size);
		result.Add(this.IsReadOnly);
		result.Add(this.Type ?? typeof(Byte));
		return result.ToHashCode();
	}
}