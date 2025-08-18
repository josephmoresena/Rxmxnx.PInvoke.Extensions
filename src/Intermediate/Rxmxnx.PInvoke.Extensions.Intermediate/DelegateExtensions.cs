namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see cref="Delegate"/> instances.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static unsafe partial class DelegateExtensions
{
	/// <summary>
	/// Retrieves a <see cref="FuncPtr{TDelegate}"/> from a memory reference to a <typeparamref name="TDelegate"/> delegate
	/// instance.
	/// </summary>
	/// <typeparam name="TDelegate">Type of the <see cref="Delegate"/> to be referenced by the pointer.</typeparam>
	/// <param name="delegateInstance">Instance of the <typeparamref name="TDelegate"/> delegate.</param>
	/// <returns>An <see cref="FuncPtr{TDelegate}"/> pointer.</returns>
	/// <remarks>
	/// The pointer will point to the address in memory where the delegate instance was located at the moment this method was
	/// called.
	/// To ensure that the pointer remains valid, the delegate instance must be kept alive and not allowed to be collected by
	/// the GC.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FuncPtr<TDelegate> GetUnsafeFuncPtr<TDelegate>(this TDelegate? delegateInstance)
		where TDelegate : Delegate
		=> delegateInstance is not null ?
			(FuncPtr<TDelegate>)Marshal.GetFunctionPointerForDelegate(delegateInstance) :
			FuncPtr<TDelegate>.Zero;
	/// <summary>
	/// Retrieves an <see cref="IntPtr"/> from a memory reference to a <typeparamref name="TDelegate"/> delegate instance.
	/// </summary>
	/// <typeparam name="TDelegate">Type of the <see cref="Delegate"/> to be referenced by the pointer.</typeparam>
	/// <param name="delegateInstance">Instance of the <typeparamref name="TDelegate"/> delegate.</param>
	/// <returns>An <see cref="IntPtr"/> pointer.</returns>
	/// <remarks>
	/// The pointer will point to the address in memory where the delegate instance was located at the moment this method was
	/// called.
	/// To ensure that the pointer remains valid, the delegate instance must be kept alive and not allowed to be collected by
	/// the GC.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntPtr GetUnsafeIntPtr<TDelegate>(this TDelegate? delegateInstance) where TDelegate : Delegate
		=> delegateInstance is not null ? Marshal.GetFunctionPointerForDelegate(delegateInstance) : IntPtr.Zero;
	/// <summary>
	/// Retrieves a <see cref="UIntPtr"/> from a memory reference to a <typeparamref name="TDelegate"/> delegate instance.
	/// </summary>
	/// <typeparam name="TDelegate">Type of the <see cref="Delegate"/> to be referenced by the pointer.</typeparam>
	/// <param name="delegateInstance">Instance of the <typeparamref name="TDelegate"/> delegate.</param>
	/// <returns>A <see cref="UIntPtr"/> pointer.</returns>
	/// <remarks>
	/// The pointer will point to the address in memory where the delegate instance was located at the moment this method was
	/// called.
	/// To ensure that the pointer remains valid, the delegate instance must be kept alive and not allowed to be collected by
	/// the GC.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIntPtr GetUnsafeUIntPtr<TDelegate>(this TDelegate? delegateInstance) where TDelegate : Delegate
	{
		IntPtr ptr = delegateInstance.GetUnsafeIntPtr();
		return (UIntPtr)ptr.ToPointer();
	}
	/// <summary>
	/// Creates an <see cref="IFixedMethod{TDelegate}.IDisposable"/> instance by marshalling the current
	/// <typeparamref name="TDelegate"/> instance, ensuring a safe interop context.
	/// </summary>
	/// <typeparam name="TDelegate">Type of the method delegate which is being fixed.</typeparam>
	/// <param name="method">Delegate of the method to be fixed.</param>
	/// <returns>An <see cref="IFixedMethod{TDelegate}.IDisposable"/> instance representing the marshalled method.</returns>
	/// <remarks>
	/// This method marshalls and protect the managed delegate to prevent the garbage collector from moving it.
	/// Ensure that the <see cref="IDisposable"/> object returned is properly disposed to release the managed delegate
	/// and avoid memory leaks.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IFixedMethod<TDelegate>.IDisposable GetFixedMethod<TDelegate>(this TDelegate? method)
		where TDelegate : Delegate
		=> NativeUtilities.GetFixedMethod(method);
}