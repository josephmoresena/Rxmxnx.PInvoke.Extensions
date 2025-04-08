﻿namespace Rxmxnx.PInvoke;

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
}