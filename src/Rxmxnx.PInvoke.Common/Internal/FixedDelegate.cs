namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Fixed method class.
/// </summary>
/// <typeparam name="T">Type of the fixed method.</typeparam>
internal unsafe sealed class FixedDelegate<TDelegate> : FixedMemory, IFixedMethod<TDelegate> where TDelegate : Delegate
{
    /// <summary>
    /// Pointer to delegate.
    /// </summary>
    private readonly IntPtr _ptr;

    /// <inheritdoc/>
    public override Type? Type => typeof(TDelegate);
    /// <inheritdoc/>
    public override Int32 BinaryOffset => default;

    TDelegate IFixedMethod<TDelegate>.Method => base.CreateDelegate<TDelegate>();
    IntPtr IFixedMethod<TDelegate>.Pointer => this._ptr;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ptr">Pointer to fixed method.</param>
    public FixedDelegate(IntPtr ptr) : base(ptr.ToPointer(), sizeof(IntPtr), true)
	{
        this._ptr = ptr;
    }
}
