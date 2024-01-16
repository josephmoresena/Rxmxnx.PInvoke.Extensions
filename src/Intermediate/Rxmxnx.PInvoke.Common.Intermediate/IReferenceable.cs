namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface exposes a reference to an object of type <typeparamref name="T"/>,
/// allowing the object to be used and potentially modified.
/// </summary>
/// <typeparam name="T">The type of the object that the reference points to.</typeparam>
public interface IReferenceable<T> : IReadOnlyReferenceable<T>, IEquatable<IReferenceable<T>>
{
	/// <summary>
	/// Gets the reference to the instance of an object of type <typeparamref name="T"/>.
	/// </summary>
	/// <remarks>This reference can be used to modify the object.</remarks>
	new ref T Reference { get; }

	[ExcludeFromCodeCoverage]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	Boolean IEquatable<IReferenceable<T>>.Equals(IReferenceable<T>? other)
		=> other is not null && Unsafe.AreSame(ref this.Reference, ref other.Reference);
}