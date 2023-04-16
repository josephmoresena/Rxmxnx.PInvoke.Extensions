namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Fixed method class.
/// </summary>
/// <typeparam name="TDelegate">Type of the fixed method.</typeparam>
internal unsafe sealed class FixedDelegate<TDelegate> : FixedMemory, IFixedMethod<TDelegate> where TDelegate : Delegate
{
    /// <inheritdoc/>
    public override Type? Type => typeof(TDelegate);
    /// <inheritdoc/>
    public override Int32 BinaryOffset => default;

    TDelegate IFixedMethod<TDelegate>.Method => base.CreateDelegate<TDelegate>();

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ptr">Pointer to fixed method.</param>
    public FixedDelegate(IntPtr ptr) : base(ptr.ToPointer(), sizeof(IntPtr), true)
	{
    }
}
