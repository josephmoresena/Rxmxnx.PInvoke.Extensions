namespace Rxmxnx.PInvoke.Internal;

internal abstract partial class ReadOnlyFixedMemory : IEquatable<ReadOnlyFixedMemory>
{
	/// <inheritdoc/>
	public virtual Boolean Equals(ReadOnlyFixedMemory? other) => this.Equals(other as FixedPointer);

	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public override Boolean Equals(Object? obj) => base.Equals(obj as FixedMemory);

	/// <inheritdoc/>
	public override Int32 GetHashCode() => base.GetHashCode();
}