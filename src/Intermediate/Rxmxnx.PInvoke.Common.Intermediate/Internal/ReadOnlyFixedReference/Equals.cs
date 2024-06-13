namespace Rxmxnx.PInvoke.Internal;

internal partial class ReadOnlyFixedReference<T> : IEquatable<ReadOnlyFixedReference<T>>
{
	/// <inheritdoc/>
	public Boolean Equals(ReadOnlyFixedReference<T>? other) => this.Equals(other as ReadOnlyFixedMemory);
	/// <inheritdoc/>
	public override Boolean Equals(ReadOnlyFixedMemory? other) => base.Equals(other as ReadOnlyFixedReference<T>);
	/// <inheritdoc/>
	public override Boolean Equals(Object? obj) => base.Equals(obj as ReadOnlyFixedReference<T>);

	/// <inheritdoc/>
	public override Int32 GetHashCode() => base.GetHashCode();
}