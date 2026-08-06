namespace Rxmxnx.PInvoke.Internal;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal unsafe partial class FixedPointer : IEquatable<FixedPointer>
{
	/// <inheritdoc/>
	public virtual Boolean Equals(FixedPointer? other)
		=> other is not null && this.GetMemoryOffset() == other.GetMemoryOffset() &&
			this.BinaryLength == other.BinaryLength && this.IsReadOnly == other.IsReadOnly;
	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public override Boolean Equals(Object? obj) => this.Equals(obj as FixedPointer);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override Int32 GetHashCode()
	{
		HashCode result = new();
		result.Add(new IntPtr(this._ptr));
		result.Add(this.BinaryOffset);
		result.Add(this._binaryLength);
		result.Add(this.IsReadOnly);
		if (this.Type is not null)
			result.Add(this.Type);
		return result.ToHashCode();
	}
}