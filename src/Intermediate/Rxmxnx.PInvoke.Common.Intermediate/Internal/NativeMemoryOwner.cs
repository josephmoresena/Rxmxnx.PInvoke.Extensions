namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Owns a native memory allocation.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal sealed unsafe class NativeMemoryOwner : IDisposable
{
	/// <summary>
	/// Pointer to the native memory allocation.
	/// </summary>
	private IntPtr _pointer;

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="byteLength">Allocation size in bytes.</param>
	private NativeMemoryOwner(Int32 byteLength)
	{
#if !NET6_0_OR_GREATER
		this._pointer = Marshal.AllocHGlobal(byteLength);
#else
		this._pointer = (IntPtr)NativeMemory.Alloc((UIntPtr)byteLength);
#endif
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		this.Release();
		GC.SuppressFinalize(this);
	}

#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	~NativeMemoryOwner() { this.Release(); }

	/// <summary>
	/// Releases the native memory allocation.
	/// </summary>
	private void Release()
	{
		if (this._pointer == IntPtr.Zero) return;
		this._pointer = IntPtr.Zero;
#if !NET6_0_OR_GREATER
		Marshal.FreeHGlobal(this._pointer);
#else
		NativeMemory.Free(this._pointer.ToPointer());
#endif
	}

	/// <summary>
	/// Allocates a native memory block for <paramref name="count"/> values of type <typeparamref name="T"/> and exposes
	/// it through an <see cref="IFixedContext{T}.IDisposable"/> instance.
	/// </summary>
	/// <typeparam name="T">The unmanaged value type stored in the allocated memory block.</typeparam>
	/// <param name="count">The number of values of type <typeparamref name="T"/> to allocate.</param>
	/// <returns>An <see cref="IFixedContext{T}.IDisposable"/> instance over the allocated native memory block.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IFixedContext<T>.IDisposable CreateContext<T>(Int32 count) where T : unmanaged
	{
		if (count == 0) return FixedContext<T>.EmptyDisposable;
		Int32 byteLength = checked(count * sizeof(T));
		NativeMemoryOwner owner = new(byteLength);
		return new FixedContext<T>(owner._pointer.ToPointer(), count).ToDisposable(owner);
	}
}