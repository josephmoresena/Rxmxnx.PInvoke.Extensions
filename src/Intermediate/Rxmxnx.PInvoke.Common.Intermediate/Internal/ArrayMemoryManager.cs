namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// A <see cref="ManagedMemoryManager{T}"/> implementation for an abstract array.
/// </summary>
/// <typeparam name="T">The type of the array.</typeparam>
#if NETFRAMEWORK || NETSTANDARD2_0
[SecurityCritical]
#endif
internal sealed partial class ArrayMemoryManager<T> : ManagedMemoryManager<T>
{
	/// <summary>
	/// Internal array.
	/// </summary>
	private readonly Array _array;

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="array">An <see cref="Array"/> instance.</param>
	private ArrayMemoryManager(Array? array) : base(array?.Length)
#if NETSTANDARD1_3_OR_GREATER || NETCOREAPP || NET46_OR_GREATER || UAP10_0
		=> this._array = array ?? Array.Empty<T>();
#else
		=> this._array = array ?? ArrayCompat.Empty<T>();
#endif

	/// <inheritdoc/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecurityCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected override GCHandle AllocPinned() => GCHandle.Alloc(this._array, GCHandleType.Pinned);
}