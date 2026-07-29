#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
#if PACKAGE
using B2 =
	Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
		Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>;
using B3 =
	Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
		System.Object>;
using B4 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
			Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
		System.Object>;
using B5 = Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, Rxmxnx.PInvoke.Buffers.
	Composite<
		Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
			Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
		Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
			Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>;
using B6 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
			Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>;
using B7 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				System.Object>,
			System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>;
using B8 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, Rxmxnx.PInvoke.Buffers.
		Composite<
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>;
#endif

#if !NET6_0_OR_GREATER
using ArgumentNullException = Rxmxnx.PInvoke.Internal.FrameworkCompat.ArgumentNullExceptionCompat;
#endif

namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS107)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2436)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
#pragma warning disable CS8500
public static unsafe partial class NativeUtilities
{
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedListAction"/> delegate.</param>
#if OBSOLTE_DELEGATES
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteDelegateExtensions, ObsoleteConstants.ErrorDelegate)]
#endif
	public static void WithSafeReadOnlyFixed<T0, T1>(Span<T0> span0, Span<T1> span1, ReadOnlyFixedListAction action)
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		{
#if !NET5_0_OR_GREATER
			B2 buffer = new(), bufferType = new(), bufferConstructor = new();
			Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
			Span<Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>> constructors =
				NativeUtilities.CreateConstructorSpan(ref bufferConstructor);
			Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[constructors.Length];
			info[0] = NativeUtilities.CreateFixedPointerInfo(ptr0, span0, out types[0], out constructors[0]);
			info[1] = NativeUtilities.CreateFixedPointerInfo(ptr1, span1, out types[1], out constructors[1]);
#else
			B2 buffer = new();
#endif
			ReadOnlyFixedMemoryList lst = new(new()
			{
				Handle = new(),
				IsReadOnly = false,
				Instances = NativeUtilities.CreateReadOnlyFixedMemorySpan(ref buffer),
#if !NET5_0_OR_GREATER
				Information = info,
#else
				Information =
				[
					NativeUtilities.CreateFixedPointerInfo(ptr0, span0),
					NativeUtilities.CreateFixedPointerInfo(ptr1, span1),
				],
#endif
			});
			try
			{
				action(lst);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedListAction{TArg}"/> delegate.</param>
#if OBSOLTE_DELEGATES
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteDelegateExtensions, ObsoleteConstants.ErrorDelegate)]
#endif
	public static void WithSafeReadOnlyFixed<T0, T1, TArg>(Span<T0> span0, Span<T1> span1, TArg arg,
		ReadOnlyFixedListAction<TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		{
#if !NET5_0_OR_GREATER
			B2 buffer = new(), bufferType = new(), bufferConstructor = new();
			Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
			Span<Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>> constructors =
				NativeUtilities.CreateConstructorSpan(ref bufferConstructor);
			Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[constructors.Length];
			info[0] = NativeUtilities.CreateFixedPointerInfo(ptr0, span0, out types[0], out constructors[0]);
			info[1] = NativeUtilities.CreateFixedPointerInfo(ptr1, span1, out types[1], out constructors[1]);
#else
			B2 buffer = new();
#endif
			ReadOnlyFixedMemoryList lst = new(new()
			{
				Handle = new(),
				IsReadOnly = false,
				Instances = NativeUtilities.CreateReadOnlyFixedMemorySpan(ref buffer),
#if !NET5_0_OR_GREATER
				Information = info,
#else
				Information =
				[
					NativeUtilities.CreateFixedPointerInfo(ptr0, span0),
					NativeUtilities.CreateFixedPointerInfo(ptr1, span1),
				],
#endif
			});
			try
			{
				action(lst, arg);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedListAction"/> delegate.</param>
#if OBSOLTE_DELEGATES
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteDelegateExtensions, ObsoleteConstants.ErrorDelegate)]
#endif
	public static void WithSafeReadOnlyFixed<T0, T1, T2>(Span<T0> span0, Span<T1> span1, Span<T2> span2,
		ReadOnlyFixedListAction action)
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		{
#if !NET5_0_OR_GREATER
			B3 buffer = new(), bufferType = new(), bufferConstructor = new();
			Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
			Span<Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>> constructors =
				NativeUtilities.CreateConstructorSpan(ref bufferConstructor);
			Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[constructors.Length];
			info[0] = NativeUtilities.CreateFixedPointerInfo(ptr0, span0, out types[0], out constructors[0]);
			info[1] = NativeUtilities.CreateFixedPointerInfo(ptr1, span1, out types[1], out constructors[1]);
			info[2] = NativeUtilities.CreateFixedPointerInfo(ptr2, span2, out types[2], out constructors[2]);
#else
			B3 buffer = new();
#endif
			ReadOnlyFixedMemoryList lst = new(new()
			{
				Handle = new(),
				IsReadOnly = false,
				Instances = NativeUtilities.CreateReadOnlyFixedMemorySpan(ref buffer),
#if !NET5_0_OR_GREATER
				Information = info,
#else
				Information =
				[
					NativeUtilities.CreateFixedPointerInfo(ptr0, span0),
					NativeUtilities.CreateFixedPointerInfo(ptr1, span1),
					NativeUtilities.CreateFixedPointerInfo(ptr2, span2),
				],
#endif
			});
			try
			{
				action(lst);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedListAction{TArg}"/> delegate.</param>
#if OBSOLTE_DELEGATES
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteDelegateExtensions, ObsoleteConstants.ErrorDelegate)]
#endif
	public static void WithSafeReadOnlyFixed<T0, T1, T2, TArg>(Span<T0> span0, Span<T1> span1, Span<T2> span2, TArg arg,
		ReadOnlyFixedListAction<TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		{
#if !NET5_0_OR_GREATER
			B3 buffer = new(), bufferType = new(), bufferConstructor = new();
			Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
			Span<Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>> constructors =
				NativeUtilities.CreateConstructorSpan(ref bufferConstructor);
			Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[constructors.Length];
			info[0] = NativeUtilities.CreateFixedPointerInfo(ptr0, span0, out types[0], out constructors[0]);
			info[1] = NativeUtilities.CreateFixedPointerInfo(ptr1, span1, out types[1], out constructors[1]);
			info[2] = NativeUtilities.CreateFixedPointerInfo(ptr2, span2, out types[2], out constructors[2]);
#else
			B3 buffer = new();
#endif
			ReadOnlyFixedMemoryList lst = new(new()
			{
				Handle = new(),
				IsReadOnly = false,
				Instances = NativeUtilities.CreateReadOnlyFixedMemorySpan(ref buffer),
#if !NET5_0_OR_GREATER
				Information = info,
#else
				Information =
				[
					NativeUtilities.CreateFixedPointerInfo(ptr0, span0),
					NativeUtilities.CreateFixedPointerInfo(ptr1, span1),
					NativeUtilities.CreateFixedPointerInfo(ptr2, span2),
				],
#endif
			});
			try
			{
				action(lst, arg);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedListAction"/> delegate.</param>
#if OBSOLTE_DELEGATES
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteDelegateExtensions, ObsoleteConstants.ErrorDelegate)]
#endif
	public static void WithSafeReadOnlyFixed<T0, T1, T2, T3>(Span<T0> span0, Span<T1> span1, Span<T2> span2,
		Span<T3> span3, ReadOnlyFixedListAction action)
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		{
#if !NET5_0_OR_GREATER
			B4 buffer = new(), bufferType = new(), bufferConstructor = new();
			Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
			Span<Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>> constructors =
				NativeUtilities.CreateConstructorSpan(ref bufferConstructor);
			Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[constructors.Length];
			info[0] = NativeUtilities.CreateFixedPointerInfo(ptr0, span0, out types[0], out constructors[0]);
			info[1] = NativeUtilities.CreateFixedPointerInfo(ptr1, span1, out types[1], out constructors[1]);
			info[2] = NativeUtilities.CreateFixedPointerInfo(ptr2, span2, out types[2], out constructors[2]);
			info[3] = NativeUtilities.CreateFixedPointerInfo(ptr3, span3, out types[3], out constructors[3]);
#else
			B4 buffer = new();
#endif
			ReadOnlyFixedMemoryList lst = new(new()
			{
				Handle = new(),
				IsReadOnly = false,
				Instances = NativeUtilities.CreateReadOnlyFixedMemorySpan(ref buffer),
#if !NET5_0_OR_GREATER
				Information = info,
#else
				Information =
				[
					NativeUtilities.CreateFixedPointerInfo(ptr0, span0),
					NativeUtilities.CreateFixedPointerInfo(ptr1, span1),
					NativeUtilities.CreateFixedPointerInfo(ptr2, span2),
					NativeUtilities.CreateFixedPointerInfo(ptr3, span3),
				],
#endif
			});
			try
			{
				action(lst);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedListAction{TArg}"/> delegate.</param>
#if OBSOLTE_DELEGATES
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteDelegateExtensions, ObsoleteConstants.ErrorDelegate)]
#endif
	public static void WithSafeReadOnlyFixed<T0, T1, T2, T3, TArg>(Span<T0> span0, Span<T1> span1, Span<T2> span2,
		Span<T3> span3, TArg arg, ReadOnlyFixedListAction<TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		{
#if !NET5_0_OR_GREATER
			B4 buffer = new(), bufferType = new(), bufferConstructor = new();
			Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
			Span<Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>> constructors =
				NativeUtilities.CreateConstructorSpan(ref bufferConstructor);
			Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[constructors.Length];
			info[0] = NativeUtilities.CreateFixedPointerInfo(ptr0, span0, out types[0], out constructors[0]);
			info[1] = NativeUtilities.CreateFixedPointerInfo(ptr1, span1, out types[1], out constructors[1]);
			info[2] = NativeUtilities.CreateFixedPointerInfo(ptr2, span2, out types[2], out constructors[2]);
			info[3] = NativeUtilities.CreateFixedPointerInfo(ptr3, span3, out types[3], out constructors[3]);
#else
			B4 buffer = new();
#endif
			ReadOnlyFixedMemoryList lst = new(new()
			{
				Handle = new(),
				IsReadOnly = false,
				Instances = NativeUtilities.CreateReadOnlyFixedMemorySpan(ref buffer),
#if !NET5_0_OR_GREATER
				Information = info,
#else
				Information =
				[
					NativeUtilities.CreateFixedPointerInfo(ptr0, span0),
					NativeUtilities.CreateFixedPointerInfo(ptr1, span1),
					NativeUtilities.CreateFixedPointerInfo(ptr2, span2),
					NativeUtilities.CreateFixedPointerInfo(ptr3, span3),
				],
#endif
			});
			try
			{
				action(lst, arg);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedListAction"/> delegate.</param>
#if OBSOLTE_DELEGATES
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteDelegateExtensions, ObsoleteConstants.ErrorDelegate)]
#endif
	public static void WithSafeReadOnlyFixed<T0, T1, T2, T3, T4>(Span<T0> span0, Span<T1> span1, Span<T2> span2,
		Span<T3> span3, Span<T4> span4, ReadOnlyFixedListAction action)
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		{
#if !NET5_0_OR_GREATER
			B5 buffer = new(), bufferType = new(), bufferConstructor = new();
			Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
			Span<Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>> constructors =
				NativeUtilities.CreateConstructorSpan(ref bufferConstructor);
			Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[constructors.Length];
			info[0] = NativeUtilities.CreateFixedPointerInfo(ptr0, span0, out types[0], out constructors[0]);
			info[1] = NativeUtilities.CreateFixedPointerInfo(ptr1, span1, out types[1], out constructors[1]);
			info[2] = NativeUtilities.CreateFixedPointerInfo(ptr2, span2, out types[2], out constructors[2]);
			info[3] = NativeUtilities.CreateFixedPointerInfo(ptr3, span3, out types[3], out constructors[3]);
			info[4] = NativeUtilities.CreateFixedPointerInfo(ptr4, span4, out types[4], out constructors[4]);
#else
			B5 buffer = new();
#endif
			ReadOnlyFixedMemoryList lst = new(new()
			{
				Handle = new(),
				IsReadOnly = false,
				Instances = NativeUtilities.CreateReadOnlyFixedMemorySpan(ref buffer),
#if !NET5_0_OR_GREATER
				Information = info,
#else
				Information =
				[
					NativeUtilities.CreateFixedPointerInfo(ptr0, span0),
					NativeUtilities.CreateFixedPointerInfo(ptr1, span1),
					NativeUtilities.CreateFixedPointerInfo(ptr2, span2),
					NativeUtilities.CreateFixedPointerInfo(ptr3, span3),
					NativeUtilities.CreateFixedPointerInfo(ptr4, span4),
				],
#endif
			});
			try
			{
				action(lst);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedListAction{TArg}"/> delegate.</param>
#if OBSOLTE_DELEGATES
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteDelegateExtensions, ObsoleteConstants.ErrorDelegate)]
#endif
	public static void WithSafeReadOnlyFixed<T0, T1, T2, T3, T4, TArg>(Span<T0> span0, Span<T1> span1, Span<T2> span2,
		Span<T3> span3, Span<T4> span4, TArg arg, ReadOnlyFixedListAction<TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		{
#if !NET5_0_OR_GREATER
			B5 buffer = new(), bufferType = new(), bufferConstructor = new();
			Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
			Span<Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>> constructors =
				NativeUtilities.CreateConstructorSpan(ref bufferConstructor);
			Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[constructors.Length];
			info[0] = NativeUtilities.CreateFixedPointerInfo(ptr0, span0, out types[0], out constructors[0]);
			info[1] = NativeUtilities.CreateFixedPointerInfo(ptr1, span1, out types[1], out constructors[1]);
			info[2] = NativeUtilities.CreateFixedPointerInfo(ptr2, span2, out types[2], out constructors[2]);
			info[3] = NativeUtilities.CreateFixedPointerInfo(ptr3, span3, out types[3], out constructors[3]);
			info[4] = NativeUtilities.CreateFixedPointerInfo(ptr4, span4, out types[4], out constructors[4]);
#else
			B5 buffer = new();
#endif
			ReadOnlyFixedMemoryList lst = new(new()
			{
				Handle = new(),
				IsReadOnly = false,
				Instances = NativeUtilities.CreateReadOnlyFixedMemorySpan(ref buffer),
#if !NET5_0_OR_GREATER
				Information = info,
#else
				Information =
				[
					NativeUtilities.CreateFixedPointerInfo(ptr0, span0),
					NativeUtilities.CreateFixedPointerInfo(ptr1, span1),
					NativeUtilities.CreateFixedPointerInfo(ptr2, span2),
					NativeUtilities.CreateFixedPointerInfo(ptr3, span3),
					NativeUtilities.CreateFixedPointerInfo(ptr4, span4),
				],
#endif
			});
			try
			{
				action(lst, arg);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedListAction"/> delegate.</param>
#if OBSOLTE_DELEGATES
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteDelegateExtensions, ObsoleteConstants.ErrorDelegate)]
#endif
	public static void WithSafeReadOnlyFixed<T0, T1, T2, T3, T4, T5>(Span<T0> span0, Span<T1> span1, Span<T2> span2,
		Span<T3> span3, Span<T4> span4, Span<T5> span5, ReadOnlyFixedListAction action)
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		{
#if !NET5_0_OR_GREATER
			B6 buffer = new(), bufferType = new(), bufferConstructor = new();
			Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
			Span<Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>> constructors =
				NativeUtilities.CreateConstructorSpan(ref bufferConstructor);
			Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[constructors.Length];
			info[0] = NativeUtilities.CreateFixedPointerInfo(ptr0, span0, out types[0], out constructors[0]);
			info[1] = NativeUtilities.CreateFixedPointerInfo(ptr1, span1, out types[1], out constructors[1]);
			info[2] = NativeUtilities.CreateFixedPointerInfo(ptr2, span2, out types[2], out constructors[2]);
			info[3] = NativeUtilities.CreateFixedPointerInfo(ptr3, span3, out types[3], out constructors[3]);
			info[4] = NativeUtilities.CreateFixedPointerInfo(ptr4, span4, out types[4], out constructors[4]);
			info[5] = NativeUtilities.CreateFixedPointerInfo(ptr5, span5, out types[5], out constructors[5]);
#else
			B6 buffer = new();
#endif
			ReadOnlyFixedMemoryList lst = new(new()
			{
				Handle = new(),
				IsReadOnly = false,
				Instances = NativeUtilities.CreateReadOnlyFixedMemorySpan(ref buffer),
#if !NET5_0_OR_GREATER
				Information = info,
#else
				Information =
				[
					NativeUtilities.CreateFixedPointerInfo(ptr0, span0),
					NativeUtilities.CreateFixedPointerInfo(ptr1, span1),
					NativeUtilities.CreateFixedPointerInfo(ptr2, span2),
					NativeUtilities.CreateFixedPointerInfo(ptr3, span3),
					NativeUtilities.CreateFixedPointerInfo(ptr4, span4),
					NativeUtilities.CreateFixedPointerInfo(ptr5, span5),
				],
#endif
			});
			try
			{
				action(lst);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedListAction{TArg}"/> delegate.</param>
#if OBSOLTE_DELEGATES
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteDelegateExtensions, ObsoleteConstants.ErrorDelegate)]
#endif
	public static void WithSafeReadOnlyFixed<T0, T1, T2, T3, T4, T5, TArg>(Span<T0> span0, Span<T1> span1,
		Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, TArg arg, ReadOnlyFixedListAction<TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		{
#if !NET5_0_OR_GREATER
			B6 buffer = new(), bufferType = new(), bufferConstructor = new();
			Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
			Span<Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>> constructors =
				NativeUtilities.CreateConstructorSpan(ref bufferConstructor);
			Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[constructors.Length];
			info[0] = NativeUtilities.CreateFixedPointerInfo(ptr0, span0, out types[0], out constructors[0]);
			info[1] = NativeUtilities.CreateFixedPointerInfo(ptr1, span1, out types[1], out constructors[1]);
			info[2] = NativeUtilities.CreateFixedPointerInfo(ptr2, span2, out types[2], out constructors[2]);
			info[3] = NativeUtilities.CreateFixedPointerInfo(ptr3, span3, out types[3], out constructors[3]);
			info[4] = NativeUtilities.CreateFixedPointerInfo(ptr4, span4, out types[4], out constructors[4]);
			info[5] = NativeUtilities.CreateFixedPointerInfo(ptr5, span5, out types[5], out constructors[5]);
#else
			B6 buffer = new();
#endif
			ReadOnlyFixedMemoryList lst = new(new()
			{
				Handle = new(),
				IsReadOnly = false,
				Instances = NativeUtilities.CreateReadOnlyFixedMemorySpan(ref buffer),
#if !NET5_0_OR_GREATER
				Information = info,
#else
				Information =
				[
					NativeUtilities.CreateFixedPointerInfo(ptr0, span0),
					NativeUtilities.CreateFixedPointerInfo(ptr1, span1),
					NativeUtilities.CreateFixedPointerInfo(ptr2, span2),
					NativeUtilities.CreateFixedPointerInfo(ptr3, span3),
					NativeUtilities.CreateFixedPointerInfo(ptr4, span4),
					NativeUtilities.CreateFixedPointerInfo(ptr5, span5),
				],
#endif
			});
			try
			{
				action(lst, arg);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="span6">7th span.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedListAction"/> delegate.</param>
#if OBSOLTE_DELEGATES
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteDelegateExtensions, ObsoleteConstants.ErrorDelegate)]
#endif
	public static void WithSafeReadOnlyFixed<T0, T1, T2, T3, T4, T5, T6>(Span<T0> span0, Span<T1> span1, Span<T2> span2,
		Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6, ReadOnlyFixedListAction action)
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		{
#if !NET5_0_OR_GREATER
			B7 buffer = new(), bufferType = new(), bufferConstructor = new();
			Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
			Span<Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>> constructors =
				NativeUtilities.CreateConstructorSpan(ref bufferConstructor);
			Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[constructors.Length];
			info[0] = NativeUtilities.CreateFixedPointerInfo(ptr0, span0, out types[0], out constructors[0]);
			info[1] = NativeUtilities.CreateFixedPointerInfo(ptr1, span1, out types[1], out constructors[1]);
			info[2] = NativeUtilities.CreateFixedPointerInfo(ptr2, span2, out types[2], out constructors[2]);
			info[3] = NativeUtilities.CreateFixedPointerInfo(ptr3, span3, out types[3], out constructors[3]);
			info[4] = NativeUtilities.CreateFixedPointerInfo(ptr4, span4, out types[4], out constructors[4]);
			info[5] = NativeUtilities.CreateFixedPointerInfo(ptr5, span5, out types[5], out constructors[5]);
			info[6] = NativeUtilities.CreateFixedPointerInfo(ptr6, span6, out types[6], out constructors[6]);
#else
			B7 buffer = new();
#endif
			ReadOnlyFixedMemoryList lst = new(new()
			{
				Handle = new(),
				IsReadOnly = false,
				Instances = NativeUtilities.CreateReadOnlyFixedMemorySpan(ref buffer),
#if !NET5_0_OR_GREATER
				Information = info,
#else
				Information =
				[
					NativeUtilities.CreateFixedPointerInfo(ptr0, span0),
					NativeUtilities.CreateFixedPointerInfo(ptr1, span1),
					NativeUtilities.CreateFixedPointerInfo(ptr2, span2),
					NativeUtilities.CreateFixedPointerInfo(ptr3, span3),
					NativeUtilities.CreateFixedPointerInfo(ptr4, span4),
					NativeUtilities.CreateFixedPointerInfo(ptr5, span5),
					NativeUtilities.CreateFixedPointerInfo(ptr6, span6),
				],
#endif
			});
			try
			{
				action(lst);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="span6">7th span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedListAction{TArg}"/> delegate.</param>
#if OBSOLTE_DELEGATES
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteDelegateExtensions, ObsoleteConstants.ErrorDelegate)]
#endif
	public static void WithSafeReadOnlyFixed<T0, T1, T2, T3, T4, T5, T6, TArg>(Span<T0> span0, Span<T1> span1,
		Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6, TArg arg,
		ReadOnlyFixedListAction<TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		{
#if !NET5_0_OR_GREATER
			B7 buffer = new(), bufferType = new(), bufferConstructor = new();
			Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
			Span<Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>> constructors =
				NativeUtilities.CreateConstructorSpan(ref bufferConstructor);
			Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[constructors.Length];
			info[0] = NativeUtilities.CreateFixedPointerInfo(ptr0, span0, out types[0], out constructors[0]);
			info[1] = NativeUtilities.CreateFixedPointerInfo(ptr1, span1, out types[1], out constructors[1]);
			info[2] = NativeUtilities.CreateFixedPointerInfo(ptr2, span2, out types[2], out constructors[2]);
			info[3] = NativeUtilities.CreateFixedPointerInfo(ptr3, span3, out types[3], out constructors[3]);
			info[4] = NativeUtilities.CreateFixedPointerInfo(ptr4, span4, out types[4], out constructors[4]);
			info[5] = NativeUtilities.CreateFixedPointerInfo(ptr5, span5, out types[5], out constructors[5]);
			info[6] = NativeUtilities.CreateFixedPointerInfo(ptr6, span6, out types[6], out constructors[6]);
#else
			B7 buffer = new();
#endif
			ReadOnlyFixedMemoryList lst = new(new()
			{
				Handle = new(),
				IsReadOnly = false,
				Instances = NativeUtilities.CreateReadOnlyFixedMemorySpan(ref buffer),
#if !NET5_0_OR_GREATER
				Information = info,
#else
				Information =
				[
					NativeUtilities.CreateFixedPointerInfo(ptr0, span0),
					NativeUtilities.CreateFixedPointerInfo(ptr1, span1),
					NativeUtilities.CreateFixedPointerInfo(ptr2, span2),
					NativeUtilities.CreateFixedPointerInfo(ptr3, span3),
					NativeUtilities.CreateFixedPointerInfo(ptr4, span4),
					NativeUtilities.CreateFixedPointerInfo(ptr5, span5),
					NativeUtilities.CreateFixedPointerInfo(ptr6, span6),
				],
#endif
			});
			try
			{
				action(lst, arg);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="T7">Type of the items in 8th span.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="span6">7th span.</param>
	/// <param name="span7">8th span.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedListAction"/> delegate.</param>
#if OBSOLTE_DELEGATES
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteDelegateExtensions, ObsoleteConstants.ErrorDelegate)]
#endif
	public static void WithSafeReadOnlyFixed<T0, T1, T2, T3, T4, T5, T6, T7>(Span<T0> span0, Span<T1> span1,
		Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6, Span<T7> span7,
		ReadOnlyFixedListAction action)
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		fixed (void* ptr7 = &MemoryMarshal.GetReference(span7))
		{
#if !NET5_0_OR_GREATER
			B8 buffer = new(), bufferType = new(), bufferConstructor = new();
			Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
			Span<Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>> constructors =
				NativeUtilities.CreateConstructorSpan(ref bufferConstructor);
			Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[constructors.Length];
			info[0] = NativeUtilities.CreateFixedPointerInfo(ptr0, span0, out types[0], out constructors[0]);
			info[1] = NativeUtilities.CreateFixedPointerInfo(ptr1, span1, out types[1], out constructors[1]);
			info[2] = NativeUtilities.CreateFixedPointerInfo(ptr2, span2, out types[2], out constructors[2]);
			info[3] = NativeUtilities.CreateFixedPointerInfo(ptr3, span3, out types[3], out constructors[3]);
			info[4] = NativeUtilities.CreateFixedPointerInfo(ptr4, span4, out types[4], out constructors[4]);
			info[5] = NativeUtilities.CreateFixedPointerInfo(ptr5, span5, out types[5], out constructors[5]);
			info[6] = NativeUtilities.CreateFixedPointerInfo(ptr6, span6, out types[6], out constructors[6]);
			info[7] = NativeUtilities.CreateFixedPointerInfo(ptr7, span7, out types[7], out constructors[7]);
#else
			B8 buffer = new();
#endif
			ReadOnlyFixedMemoryList lst = new(new()
			{
				Handle = new(),
				IsReadOnly = false,
				Instances = NativeUtilities.CreateReadOnlyFixedMemorySpan(ref buffer),
#if !NET5_0_OR_GREATER
				Information = info,
#else
				Information =
				[
					NativeUtilities.CreateFixedPointerInfo(ptr0, span0),
					NativeUtilities.CreateFixedPointerInfo(ptr1, span1),
					NativeUtilities.CreateFixedPointerInfo(ptr2, span2),
					NativeUtilities.CreateFixedPointerInfo(ptr3, span3),
					NativeUtilities.CreateFixedPointerInfo(ptr4, span4),
					NativeUtilities.CreateFixedPointerInfo(ptr5, span5),
					NativeUtilities.CreateFixedPointerInfo(ptr6, span6),
					NativeUtilities.CreateFixedPointerInfo(ptr7, span7),
				],
#endif
			});
			try
			{
				action(lst);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="T7">Type of the items in 8th span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="span6">7th span.</param>
	/// <param name="span7">8th span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedListAction{TArg}"/> delegate.</param>
#if OBSOLTE_DELEGATES
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteDelegateExtensions, ObsoleteConstants.ErrorDelegate)]
#endif
	public static void WithSafeReadOnlyFixed<T0, T1, T2, T3, T4, T5, T6, T7, TArg>(Span<T0> span0, Span<T1> span1,
		Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6, Span<T7> span7, TArg arg,
		ReadOnlyFixedListAction<TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		fixed (void* ptr7 = &MemoryMarshal.GetReference(span7))
		{
#if !NET5_0_OR_GREATER
			B8 buffer = new(), bufferType = new(), bufferConstructor = new();
			Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
			Span<Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>> constructors =
				NativeUtilities.CreateConstructorSpan(ref bufferConstructor);
			Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[constructors.Length];
			info[0] = NativeUtilities.CreateFixedPointerInfo(ptr0, span0, out types[0], out constructors[0]);
			info[1] = NativeUtilities.CreateFixedPointerInfo(ptr1, span1, out types[1], out constructors[1]);
			info[2] = NativeUtilities.CreateFixedPointerInfo(ptr2, span2, out types[2], out constructors[2]);
			info[3] = NativeUtilities.CreateFixedPointerInfo(ptr3, span3, out types[3], out constructors[3]);
			info[4] = NativeUtilities.CreateFixedPointerInfo(ptr4, span4, out types[4], out constructors[4]);
			info[5] = NativeUtilities.CreateFixedPointerInfo(ptr5, span5, out types[5], out constructors[5]);
			info[6] = NativeUtilities.CreateFixedPointerInfo(ptr6, span6, out types[6], out constructors[6]);
			info[7] = NativeUtilities.CreateFixedPointerInfo(ptr7, span7, out types[7], out constructors[7]);
#else
			B8 buffer = new();
#endif
			ReadOnlyFixedMemoryList lst = new(new()
			{
				Handle = new(),
				IsReadOnly = false,
				Instances = NativeUtilities.CreateReadOnlyFixedMemorySpan(ref buffer),
#if !NET5_0_OR_GREATER
				Information = info,
#else
				Information =
				[
					NativeUtilities.CreateFixedPointerInfo(ptr0, span0),
					NativeUtilities.CreateFixedPointerInfo(ptr1, span1),
					NativeUtilities.CreateFixedPointerInfo(ptr2, span2),
					NativeUtilities.CreateFixedPointerInfo(ptr3, span3),
					NativeUtilities.CreateFixedPointerInfo(ptr4, span4),
					NativeUtilities.CreateFixedPointerInfo(ptr5, span5),
					NativeUtilities.CreateFixedPointerInfo(ptr6, span6),
					NativeUtilities.CreateFixedPointerInfo(ptr7, span7),
				],
#endif
			});
			try
			{
				action(lst, arg);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
}
#pragma warning restore CS8500
#endif