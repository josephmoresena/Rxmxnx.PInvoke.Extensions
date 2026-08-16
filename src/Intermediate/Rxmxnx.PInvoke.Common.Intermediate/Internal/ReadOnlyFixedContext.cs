#if !NETSTANDARD2_1 && !NETCOREAPP2_0_OR_GREATER
using RuntimeHelpers = Rxmxnx.PInvoke.Internal.FrameworkCompat.RuntimeHelpersCompat;
#endif

namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents a fixed read-only memory block for a specific type.
/// </summary>
/// <typeparam name="T">The type of the items in the fixed memory block.</typeparam>
#if !OBSOLETE_FIXED_INTERFACES
internal sealed partial class ReadOnlyFixedContext<T> : ReadOnlyFixedMemory, IReadOnlyFixedContext<T>
#else
#pragma warning disable CS0612
internal sealed partial class ReadOnlyFixedContext<T> : ReadOnlyFixedMemory, IObsoleteReadOnlyFixedContext<T>
#pragma warning restore CS0612
#endif
{
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	/// <summary>
	/// An empty instance of <see cref="ReadOnlyFixedContext{T}"/>.
	/// </summary>
	public static readonly ReadOnlyFixedContext<T> Empty = new();
	/// <summary>
	/// An empty instance of <see cref="IReadOnlyFixedContext{T}.IDisposable"/>.
	/// </summary>
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	public static readonly IReadOnlyFixedContext<T>.IDisposable EmptyDisposable = Disposable.Default;
#endif

	/// <inheritdoc/>
	public override Boolean IsUnmanaged => !RuntimeHelpers.IsReferenceOrContainsReferences<T>();

	/// <summary>
	/// Gets the number of items of type <typeparamref name="T"/> in the memory block.
	/// </summary>
	public Int32 Count { get; }

	/// <inheritdoc/>
	public override Int32 BinaryOffset => default;
	/// <inheritdoc/>
	public override Type Type => typeof(T);
	/// <inheritdoc/>
	public override Boolean IsFunction => false;

	ReadOnlySpan<T> IReadOnlyFixedMemory<T>.Values => this.CreateReadOnlySpan<T>(this.Count);

#if !NETSTANDARD2_1 && !NETCOREAPP3_0_OR_GREATER
	ReadOnlyValPtr<T> IReadOnlyFixedMemory<T>.ValuePointer => (ReadOnlyValPtr<T>)(this as IFixedPointer).Pointer;
#endif
}