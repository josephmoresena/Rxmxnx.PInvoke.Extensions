#if PACKAGE && !NET5_0_OR_GREATER
using B6 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
			Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>;
#elif !NET5_0_OR_GREATER
using B6 = Rxmxnx.PInvoke.NativeUtilities.B6;
#endif

namespace Rxmxnx.PInvoke;

public static partial class FixedPointerListValueExtensions
{
	/// <summary>
	/// Pins 6 spans and executes an <see cref="IFixedPointerListAction"/> or
	/// <see cref="IFixedPointerListFunction{TResult}"/>.
	/// </summary>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS107)]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2436)]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	private static unsafe class FixedPointerList6
	{
#pragma warning disable CS8500
		/// <summary>
		/// Pins the given spans and executes <paramref name="action"/>.
		/// </summary>
		/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
		/// <typeparam name="T0">Type of the items in the first span.</typeparam>
		/// <typeparam name="T1">Type of the items in the second span.</typeparam>
		/// <typeparam name="T2">Type of the items in the third span.</typeparam>
		/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
		/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
		/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
		/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
		/// <param name="span0">The first span.</param>
		/// <param name="span1">The second span.</param>
		/// <param name="span2">The third span.</param>
		/// <param name="span3">The fourth span.</param>
		/// <param name="span4">The fifth span.</param>
		/// <param name="span5">The sixth span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		internal static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5>(ref TAction action, Span<T0> span0,
			Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5)
#if !NET9_0_OR_GREATER
			where TAction : IFixedPointerListAction
#else
			where TAction : IFixedPointerListAction, allows ref struct
#endif
		{
			fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
			fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
			fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
			fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
			fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
			fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
			{
#if !NET5_0_OR_GREATER
				B6 bufferType = new();
				Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
				Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[types.Length];
#else
				void* infoPtr = stackalloc Byte[sizeof(FixedPointerInfo) * types.Length];
				Span<FixedPointerInfo> info =
					MemoryMarshalCompat.CreateUnsafeSpan<FixedPointerInfo>(infoPtr, types.Length);
#endif
				info[0] = span0.CreateFixedPointerInfo(ptr0, out types[0]);
				info[1] = span1.CreateFixedPointerInfo(ptr1, out types[1]);
				info[2] = span2.CreateFixedPointerInfo(ptr2, out types[2]);
				info[3] = span3.CreateFixedPointerInfo(ptr3, out types[3]);
				info[4] = span4.CreateFixedPointerInfo(ptr4, out types[4]);
				info[5] = span5.CreateFixedPointerInfo(ptr5, out types[5]);
#endif
				action.Accept(new()
				{
					IsReadOnly = false,
					Instances = [],
#if !NET5_0_OR_GREATER
					Information = info,
#else
					Information =
					[
						span0.CreateFixedPointerInfo(ptr0),
						span1.CreateFixedPointerInfo(ptr1),
						span2.CreateFixedPointerInfo(ptr2),
						span3.CreateFixedPointerInfo(ptr3),
						span4.CreateFixedPointerInfo(ptr4),
						span5.CreateFixedPointerInfo(ptr5),
					],
#endif
				});
			}
		}

		/// <summary>
		/// Pins the given read-only spans and executes <paramref name="action"/>.
		/// </summary>
		/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
		/// <typeparam name="T0">Type of the items in the first span.</typeparam>
		/// <typeparam name="T1">Type of the items in the second span.</typeparam>
		/// <typeparam name="T2">Type of the items in the third span.</typeparam>
		/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
		/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
		/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
		/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
		/// <param name="span0">The first read-only span.</param>
		/// <param name="span1">The second read-only span.</param>
		/// <param name="span2">The third read-only span.</param>
		/// <param name="span3">The fourth read-only span.</param>
		/// <param name="span4">The fifth read-only span.</param>
		/// <param name="span5">The sixth read-only span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		internal static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5>(ref TAction action, ReadOnlySpan<T0> span0,
			ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, ReadOnlySpan<T4> span4,
			ReadOnlySpan<T5> span5)
#if !NET9_0_OR_GREATER
			where TAction : IFixedPointerListAction
#else
			where TAction : IFixedPointerListAction, allows ref struct
#endif
		{
			fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
			fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
			fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
			fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
			fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
			fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
			{
#if !NET5_0_OR_GREATER
				B6 bufferType = new();
				Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
				Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[types.Length];
#else
				void* infoPtr = stackalloc Byte[sizeof(FixedPointerInfo) * types.Length];
				Span<FixedPointerInfo> info =
					MemoryMarshalCompat.CreateUnsafeSpan<FixedPointerInfo>(infoPtr, types.Length);
#endif
				info[0] = span0.CreateFixedPointerInfo(ptr0, out types[0]);
				info[1] = span1.CreateFixedPointerInfo(ptr1, out types[1]);
				info[2] = span2.CreateFixedPointerInfo(ptr2, out types[2]);
				info[3] = span3.CreateFixedPointerInfo(ptr3, out types[3]);
				info[4] = span4.CreateFixedPointerInfo(ptr4, out types[4]);
				info[5] = span5.CreateFixedPointerInfo(ptr5, out types[5]);
#endif
				action.Accept(new()
				{
					IsReadOnly = true,
					Instances = [],
#if !NET5_0_OR_GREATER
					Information = info,
#else
					Information =
					[
						span0.CreateFixedPointerInfo(ptr0),
						span1.CreateFixedPointerInfo(ptr1),
						span2.CreateFixedPointerInfo(ptr2),
						span3.CreateFixedPointerInfo(ptr3),
						span4.CreateFixedPointerInfo(ptr4),
						span5.CreateFixedPointerInfo(ptr5),
					],
#endif
				});
			}
		}

		/// <summary>
		/// Pins the given spans and executes <paramref name="func"/>.
		/// </summary>
		/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
		/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
		/// <typeparam name="T0">Type of the items in the first span.</typeparam>
		/// <typeparam name="T1">Type of the items in the second span.</typeparam>
		/// <typeparam name="T2">Type of the items in the third span.</typeparam>
		/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
		/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
		/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
		/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
		/// <param name="span0">The first span.</param>
		/// <param name="span1">The second span.</param>
		/// <param name="span2">The third span.</param>
		/// <param name="span3">The fourth span.</param>
		/// <param name="span4">The fifth span.</param>
		/// <param name="span5">The sixth span.</param>
		/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		internal static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5>(ref TFunction func,
			Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5,
			out TResult result)
#if !NET9_0_OR_GREATER
			where TFunction : IFixedPointerListFunction<TResult>
#else
			where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
		{
			fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
			fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
			fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
			fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
			fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
			fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
			{
#if !NET5_0_OR_GREATER
				B6 bufferType = new();
				Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
				Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[types.Length];
#else
				void* infoPtr = stackalloc Byte[sizeof(FixedPointerInfo) * types.Length];
				Span<FixedPointerInfo> info =
					MemoryMarshalCompat.CreateUnsafeSpan<FixedPointerInfo>(infoPtr, types.Length);
#endif
				info[0] = span0.CreateFixedPointerInfo(ptr0, out types[0]);
				info[1] = span1.CreateFixedPointerInfo(ptr1, out types[1]);
				info[2] = span2.CreateFixedPointerInfo(ptr2, out types[2]);
				info[3] = span3.CreateFixedPointerInfo(ptr3, out types[3]);
				info[4] = span4.CreateFixedPointerInfo(ptr4, out types[4]);
				info[5] = span5.CreateFixedPointerInfo(ptr5, out types[5]);
#endif
				result = func.Apply(new()
				{
					IsReadOnly = false,
					Instances = [],
#if !NET5_0_OR_GREATER
					Information = info,
#else
					Information =
					[
						span0.CreateFixedPointerInfo(ptr0),
						span1.CreateFixedPointerInfo(ptr1),
						span2.CreateFixedPointerInfo(ptr2),
						span3.CreateFixedPointerInfo(ptr3),
						span4.CreateFixedPointerInfo(ptr4),
						span5.CreateFixedPointerInfo(ptr5),
					],
#endif
				});
			}
		}

		/// <summary>
		/// Pins the given read-only spans and executes <paramref name="func"/>.
		/// </summary>
		/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
		/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
		/// <typeparam name="T0">Type of the items in the first span.</typeparam>
		/// <typeparam name="T1">Type of the items in the second span.</typeparam>
		/// <typeparam name="T2">Type of the items in the third span.</typeparam>
		/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
		/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
		/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
		/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
		/// <param name="span0">The first read-only span.</param>
		/// <param name="span1">The second read-only span.</param>
		/// <param name="span2">The third read-only span.</param>
		/// <param name="span3">The fourth read-only span.</param>
		/// <param name="span4">The fifth read-only span.</param>
		/// <param name="span5">The sixth read-only span.</param>
		/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		internal static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5>(ref TFunction func,
			ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
			ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, out TResult result)
#if !NET9_0_OR_GREATER
			where TFunction : IFixedPointerListFunction<TResult>
#else
			where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
		{
			fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
			fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
			fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
			fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
			fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
			fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
			{
#if !NET5_0_OR_GREATER
				B6 bufferType = new();
				Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
				Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[types.Length];
#else
				void* infoPtr = stackalloc Byte[sizeof(FixedPointerInfo) * types.Length];
				Span<FixedPointerInfo> info =
					MemoryMarshalCompat.CreateUnsafeSpan<FixedPointerInfo>(infoPtr, types.Length);
#endif
				info[0] = span0.CreateFixedPointerInfo(ptr0, out types[0]);
				info[1] = span1.CreateFixedPointerInfo(ptr1, out types[1]);
				info[2] = span2.CreateFixedPointerInfo(ptr2, out types[2]);
				info[3] = span3.CreateFixedPointerInfo(ptr3, out types[3]);
				info[4] = span4.CreateFixedPointerInfo(ptr4, out types[4]);
				info[5] = span5.CreateFixedPointerInfo(ptr5, out types[5]);
#endif
				result = func.Apply(new()
				{
					IsReadOnly = true,
					Instances = [],
#if !NET5_0_OR_GREATER
					Information = info,
#else
					Information =
					[
						span0.CreateFixedPointerInfo(ptr0),
						span1.CreateFixedPointerInfo(ptr1),
						span2.CreateFixedPointerInfo(ptr2),
						span3.CreateFixedPointerInfo(ptr3),
						span4.CreateFixedPointerInfo(ptr4),
						span5.CreateFixedPointerInfo(ptr5),
					],
#endif
				});
			}
		}

		/// <summary>Pins 6 references and executes <paramref name="action"/>.</summary>
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		internal static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5>(ref TAction action, ref T0 value0,
			ref T1 value1, ref T2 value2, ref T3 value3, ref T4 value4, ref T5 value5)
#if !NET9_0_OR_GREATER
			where TAction : IFixedPointerListAction
#else
			where T0 : allows ref struct
			where T1 : allows ref struct
			where T2 : allows ref struct
			where T3 : allows ref struct
			where T4 : allows ref struct
			where T5 : allows ref struct
			where TAction : IFixedPointerListAction, allows ref struct
#endif
		{
			fixed (void* ptr0 = &value0)
			fixed (void* ptr1 = &value1)
			fixed (void* ptr2 = &value2)
			fixed (void* ptr3 = &value3)
			fixed (void* ptr4 = &value4)
			fixed (void* ptr5 = &value5)
			{
#if !NET5_0_OR_GREATER
				B6 bufferType = new();
				Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
				Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[types.Length];
#else
				void* infoPtr = stackalloc Byte[sizeof(FixedPointerInfo) * types.Length];
				Span<FixedPointerInfo> info =
					MemoryMarshalCompat.CreateUnsafeSpan<FixedPointerInfo>(infoPtr, types.Length);
#endif
				info[0] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T0>(ptr0, out types[0]);
				info[1] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T1>(ptr1, out types[1]);
				info[2] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T2>(ptr2, out types[2]);
				info[3] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T3>(ptr3, out types[3]);
				info[4] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T4>(ptr4, out types[4]);
				info[5] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T5>(ptr5, out types[5]);
#endif
				action.Accept(new()
				{
					IsReadOnly = false,
					Instances = [],
#if !NET5_0_OR_GREATER
					Information = info,
#else
					Information =
					[
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T0>(ptr0),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T1>(ptr1),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T2>(ptr2),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T3>(ptr3),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T4>(ptr4),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T5>(ptr5),
					],
#endif
				});
			}
		}

		/// <summary>Pins 6 references and executes <paramref name="func"/>.</summary>
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		internal static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5>(ref TFunction func,
			ref T0 value0, ref T1 value1, ref T2 value2, ref T3 value3, ref T4 value4, ref T5 value5,
			out TResult result)
#if !NET9_0_OR_GREATER
			where TFunction : IFixedPointerListFunction<TResult>
#else
			where T0 : allows ref struct
			where T1 : allows ref struct
			where T2 : allows ref struct
			where T3 : allows ref struct
			where T4 : allows ref struct
			where T5 : allows ref struct
			where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
		{
			fixed (void* ptr0 = &value0)
			fixed (void* ptr1 = &value1)
			fixed (void* ptr2 = &value2)
			fixed (void* ptr3 = &value3)
			fixed (void* ptr4 = &value4)
			fixed (void* ptr5 = &value5)
			{
#if !NET5_0_OR_GREATER
				B6 bufferType = new();
				Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
				Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[types.Length];
#else
				void* infoPtr = stackalloc Byte[sizeof(FixedPointerInfo) * types.Length];
				Span<FixedPointerInfo> info =
					MemoryMarshalCompat.CreateUnsafeSpan<FixedPointerInfo>(infoPtr, types.Length);
#endif
				info[0] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T0>(ptr0, out types[0]);
				info[1] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T1>(ptr1, out types[1]);
				info[2] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T2>(ptr2, out types[2]);
				info[3] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T3>(ptr3, out types[3]);
				info[4] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T4>(ptr4, out types[4]);
				info[5] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T5>(ptr5, out types[5]);
#endif
				result = func.Apply(new()
				{
					IsReadOnly = false,
					Instances = [],
#if !NET5_0_OR_GREATER
					Information = info,
#else
					Information =
					[
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T0>(ptr0),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T1>(ptr1),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T2>(ptr2),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T3>(ptr3),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T4>(ptr4),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T5>(ptr5),
					],
#endif
				});
			}
		}

		/// <summary>Pins 6 read-only references and executes <paramref name="action"/>.</summary>
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		internal static void WithSafeReadOnlyFixed<TAction, T0, T1, T2, T3, T4, T5>(ref TAction action, in T0 value0,
			in T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5)
#if !NET9_0_OR_GREATER
			where TAction : IFixedPointerListAction
#else
			where T0 : allows ref struct
			where T1 : allows ref struct
			where T2 : allows ref struct
			where T3 : allows ref struct
			where T4 : allows ref struct
			where T5 : allows ref struct
			where TAction : IFixedPointerListAction, allows ref struct
#endif
		{
			fixed (void* ptr0 = &value0)
			fixed (void* ptr1 = &value1)
			fixed (void* ptr2 = &value2)
			fixed (void* ptr3 = &value3)
			fixed (void* ptr4 = &value4)
			fixed (void* ptr5 = &value5)
			{
#if !NET5_0_OR_GREATER
				B6 bufferType = new();
				Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
				Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[types.Length];
#else
				void* infoPtr = stackalloc Byte[sizeof(FixedPointerInfo) * types.Length];
				Span<FixedPointerInfo> info =
					MemoryMarshalCompat.CreateUnsafeSpan<FixedPointerInfo>(infoPtr, types.Length);
#endif
				info[0] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T0>(ptr0, out types[0]);
				info[1] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T1>(ptr1, out types[1]);
				info[2] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T2>(ptr2, out types[2]);
				info[3] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T3>(ptr3, out types[3]);
				info[4] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T4>(ptr4, out types[4]);
				info[5] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T5>(ptr5, out types[5]);
#endif
				action.Accept(new()
				{
					IsReadOnly = true,
					Instances = [],
#if !NET5_0_OR_GREATER
					Information = info,
#else
					Information =
					[
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T0>(ptr0),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T1>(ptr1),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T2>(ptr2),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T3>(ptr3),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T4>(ptr4),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T5>(ptr5),
					],
#endif
				});
			}
		}

		/// <summary>Pins 6 read-only references and executes <paramref name="func"/>.</summary>
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		internal static void WithSafeReadOnlyFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5>(ref TFunction func,
			in T0 value0, in T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, out TResult result)
#if !NET9_0_OR_GREATER
			where TFunction : IFixedPointerListFunction<TResult>
#else
			where T0 : allows ref struct
			where T1 : allows ref struct
			where T2 : allows ref struct
			where T3 : allows ref struct
			where T4 : allows ref struct
			where T5 : allows ref struct
			where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
		{
			fixed (void* ptr0 = &value0)
			fixed (void* ptr1 = &value1)
			fixed (void* ptr2 = &value2)
			fixed (void* ptr3 = &value3)
			fixed (void* ptr4 = &value4)
			fixed (void* ptr5 = &value5)
			{
#if !NET5_0_OR_GREATER
				B6 bufferType = new();
				Span<Type> types = NativeUtilities.CreateTypeSpan(ref bufferType);
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
				Span<FixedPointerInfo> info = stackalloc FixedPointerInfo[types.Length];
#else
				void* infoPtr = stackalloc Byte[sizeof(FixedPointerInfo) * types.Length];
				Span<FixedPointerInfo> info =
					MemoryMarshalCompat.CreateUnsafeSpan<FixedPointerInfo>(infoPtr, types.Length);
#endif
				info[0] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T0>(ptr0, out types[0]);
				info[1] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T1>(ptr1, out types[1]);
				info[2] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T2>(ptr2, out types[2]);
				info[3] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T3>(ptr3, out types[3]);
				info[4] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T4>(ptr4, out types[4]);
				info[5] = FixedPointerListValueExtensions.CreateFixedPointerInfo<T5>(ptr5, out types[5]);
#endif
				result = func.Apply(new()
				{
					IsReadOnly = true,
					Instances = [],
#if !NET5_0_OR_GREATER
					Information = info,
#else
					Information =
					[
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T0>(ptr0),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T1>(ptr1),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T2>(ptr2),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T3>(ptr3),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T4>(ptr4),
						FixedPointerListValueExtensions.CreateFixedPointerInfo<T5>(ptr5),
					],
#endif
				});
			}
		}

#pragma warning restore CS8500
	}
}