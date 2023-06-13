namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Fixed method class, used to hold a fixed pointer to a method delegate.
/// </summary>
/// <typeparam name="TDelegate">Type of the method delegate which is being fixed.</typeparam>
internal unsafe sealed class FixedDelegate<TDelegate> : FixedPointer, IFixedMethod<TDelegate> where TDelegate : Delegate
{
    /// <summary>
    /// Internal instance of <see cref="GCHandle"/>.
    /// </summary>
    /// <remarks>It prevents the delegate from being collected by the garbage collector.</remarks>
    private readonly GCHandle? _handle = default;

    /// <inheritdoc/>
    public override Type? Type => typeof(TDelegate);
    /// <inheritdoc/>
    public override Int32 BinaryOffset => default;
    /// <inheritdoc/>
    public override Boolean IsFunction => true;

    TDelegate IFixedMethod<TDelegate>.Method => base.CreateDelegate<TDelegate>();

    /// <summary>
    /// Constructor that takes a method delegate and stores a pointer to it.
    /// </summary>
    /// <param name="method">Delegate of the method to be fixed.</param>
    public FixedDelegate(TDelegate method) : this(GetMethodPointer(method, out GCHandle handle))
    {
        this._handle = handle;
    }
    /// <summary>
    /// Constructor that takes a pointer to a method.
    /// </summary>
    /// <param name="ptr">Pointer to the method to be fixed.</param>
    public FixedDelegate(void* ptr) : base(ptr, sizeof(IntPtr), true)
    {
    }

    /// <inheritdoc/>
    public override void Unload()
    {
        base.Unload();
        if (this._handle.HasValue)
            this._handle.Value.Free();
    }

    /// <summary>
    /// Gets the pointer to the method delegate provided, while creating a <see cref="GCHandle"/> to
    /// prevent the delegate from being collected.
    /// </summary>
    /// <param name="method">Delegate of the method to be fixed.</param>
    /// <param name="handle">
    /// Output. A <see cref="GCHandle"/> to prevent the delegate from being collected by the garbage collector.
    /// </param>
    /// <returns>Pointer to the provided delegate of the method.</returns>
    private static unsafe void* GetMethodPointer(TDelegate method, out GCHandle handle)
    {
        handle = GCHandle.Alloc(method, GCHandleType.Normal);
        return Marshal.GetFunctionPointerForDelegate(method).ToPointer();
    }
}
