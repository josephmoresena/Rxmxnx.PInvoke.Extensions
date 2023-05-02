namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Fixed method class.
/// </summary>
/// <typeparam name="TDelegate">Type of the fixed method.</typeparam>
internal unsafe sealed class FixedDelegate<TDelegate> : FixedMemory, IFixedMethod<TDelegate> where TDelegate : Delegate
{
    /// <summary>
    /// Internal <see cref="GCHandle"/> instance.
    /// </summary>
    private readonly GCHandle? _handle = default;

    /// <inheritdoc/>
    public override Type? Type => typeof(TDelegate);
    /// <inheritdoc/>
    public override Int32 BinaryOffset => default;

    TDelegate IFixedMethod<TDelegate>.Method => base.CreateDelegate<TDelegate>();

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="method">Method delegate.</param>
    public FixedDelegate(TDelegate method) : this(GetMethodPointer(method, out GCHandle handle))
    {
        this._handle = handle;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ptr">Pointer to fixed method.</param>
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
    /// Retrieves the pointer to <paramref name="method"/>.
    /// </summary>
    /// <param name="method">Method delegate.</param>
    /// <param name="handle">Output. <see cref="GCHandle"/> to prevent delegate collection.</param>
    /// <returns>The pointer to <paramref name="method"/>.</returns>
    private static unsafe void* GetMethodPointer(TDelegate method, out GCHandle handle)
    {
        handle = GCHandle.Alloc(method, GCHandleType.Normal);
        return Marshal.GetFunctionPointerForDelegate(method).ToPointer();
    }
}
