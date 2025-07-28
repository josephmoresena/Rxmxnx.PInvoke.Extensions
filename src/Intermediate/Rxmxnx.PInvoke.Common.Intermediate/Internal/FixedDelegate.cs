namespace Rxmxnx.PInvoke.Internal;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal abstract unsafe class FixedDelegate : FixedPointer
{
	/// <summary>
	/// Internal instance of <see cref="GCHandle"/>.
	/// </summary>
	/// <remarks>It prevents the delegate from being collected by the garbage collector.</remarks>
	private GCHandle _handle;

	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public override Boolean IsUnmanaged => false;
	/// <inheritdoc/>
	public override Int32 BinaryOffset => default;
	/// <inheritdoc/>
	public override Boolean IsFunction => true;

	/// <summary>
	/// Constructor that takes a method delegate and stores a pointer to it.
	/// </summary>
	/// <param name="methodPointer">Pointer to the method.</param>
	/// <param name="handle"><see cref="GCHandle"/> of the method.</param>
	protected FixedDelegate(void* methodPointer, GCHandle handle) : base(methodPointer, sizeof(IntPtr), true)
		=> this._handle = handle;

	/// <inheritdoc/>
	public override void Unload()
	{
		base.Unload();
		this._handle.Free();
	}
}

/// <summary>
/// Fixed method class, used to hold a fixed pointer to a method delegate.
/// </summary>
/// <typeparam name="TDelegate">Type of the method delegate which is being fixed.</typeparam>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal sealed unsafe class FixedDelegate<TDelegate> : FixedDelegate, IFixedMethod<TDelegate>
	where TDelegate : Delegate
{
	/// <inheritdoc/>
	public override Type Type => typeof(TDelegate);

	/// <summary>
	/// Constructor that takes a method delegate and stores a pointer to it.
	/// </summary>
	/// <param name="method">Delegate of the method to be fixed.</param>
	public FixedDelegate(TDelegate method) : base(
		FixedDelegate<TDelegate>.GetMethodPointer(method, out GCHandle handle), handle) { }

	TDelegate IFixedMethod<TDelegate>.Method => this.CreateDelegate<TDelegate>();

	/// <summary>
	/// Gets the pointer to the method delegate provided, while creating a <see cref="GCHandle"/> to
	/// prevent the delegate from being collected.
	/// </summary>
	/// <param name="method">Delegate of the method to be fixed.</param>
	/// <param name="handle">
	/// Output. A <see cref="GCHandle"/> to prevent the delegate from being collected by the garbage collector.
	/// </param>
	/// <returns>Pointer to the provided delegate of the method.</returns>
	private static void* GetMethodPointer(TDelegate method, out GCHandle handle)
	{
		handle = GCHandle.Alloc(method, GCHandleType.Normal);
		return Marshal.GetFunctionPointerForDelegate(method).ToPointer();
	}
}