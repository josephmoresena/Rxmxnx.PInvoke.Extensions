namespace Rxmxnx.PInvoke.Internal;

internal abstract partial class FixedMemory
{
	/// <inheritdoc/>
	public virtual Boolean Equals(FixedMemory? other) => this.Equals(other as FixedPointer);

	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public override Boolean Equals(Object? obj) => base.Equals(obj as FixedMemory);
	/// <inheritdoc/>
	public override Int32 GetHashCode() => base.GetHashCode();
}