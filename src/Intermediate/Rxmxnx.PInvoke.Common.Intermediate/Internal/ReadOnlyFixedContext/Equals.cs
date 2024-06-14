namespace Rxmxnx.PInvoke.Internal;

internal partial class ReadOnlyFixedContext<T> : IEquatable<ReadOnlyFixedContext<T>>
{
	/// <inheritdoc/>
	public Boolean Equals(ReadOnlyFixedContext<T>? other) => this.Equals(other as ReadOnlyFixedMemory);
	/// <inheritdoc/>
	public override Boolean Equals(ReadOnlyFixedMemory? other) => base.Equals(other as ReadOnlyFixedContext<T>);
	/// <inheritdoc/>
	public override Boolean Equals(Object? obj) => base.Equals(obj as ReadOnlyFixedContext<T>);
	/// <inheritdoc/>
	public override Int32 GetHashCode() => base.GetHashCode();
}