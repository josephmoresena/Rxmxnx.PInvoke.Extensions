#if !NETSTANDARD2_1 && !NETCOREAPP2_0_OR_GREATER
using RuntimeHelpers = Rxmxnx.PInvoke.Internal.FrameworkCompat.RuntimeHelpersCompat;
#endif
#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
using MemoryMarshalCompat = Rxmxnx.PInvoke.Internal.FrameworkCompat.MemoryMarshalCompat;
#endif

namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static partial class BufferManager<T>
{
	/// <summary>
	/// Allocates a stack buffer with the required size for execution.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IScopedBufferAction{T}"/>.</typeparam>
	/// <param name="action">Method to execute.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void AllocObject<TAction>(ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : IScopedBufferAction<T>
#else
		where TAction : IScopedBufferAction<T>, allows ref struct
#endif
	{
		BufferTypeMetadata<Object>? metadata = BufferManager.Storage.GetMetadata<Object>(action.Count);
		Boolean stackAlloc = metadata is not null &&
			(action.IsMinimalCount || metadata.Size == action.Count || action.Count == 0);
#if !PACKAGE && (NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299)
		BufferManager.Storage.PrintMetadata<Object>(!stackAlloc);
#endif
		if (stackAlloc)
		{
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
			Debug.Assert(metadata is not null);
			metadata.Execute<T, TAction>(ref action, action.Count);
#else
			metadata!.Execute<T, TAction>(ref action, action.Count);
#endif
			return;
		}

		BufferManager<T>.AllocHeap(ref action);
	}
	/// <summary>
	/// Allocates a stack buffer with the required size for execution.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IScopedBufferFunction{T, TFunction}"/>.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="func">Function to execute.</param>
	/// <param name="result">Output. Function result.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void AllocObject<TFunction, TResult>(ref TFunction func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IScopedBufferFunction<T, TResult>
#else
		where TFunction : IScopedBufferFunction<T, TResult>, allows ref struct
#endif
	{
		BufferTypeMetadata<Object>? metadata = BufferManager.Storage.GetMetadata<Object>(func.Count);
		Boolean stackAlloc = metadata is not null &&
			(func.IsMinimalCount || metadata.Size == func.Count || func.Count == 0);
#if !PACKAGE && (NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299)
		BufferManager.Storage.PrintMetadata<Object>(!stackAlloc);
#endif
		if (!stackAlloc)
		{
			BufferManager<T>.AllocHeap(ref func, out result);
			return;
		}
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
		Debug.Assert(metadata is not null);
		result = metadata.Execute<T, TFunction, TResult>(ref func, func.Count);
#else
		result = metadata!.Execute<T, TFunction, TResult>(ref func, func.Count);
#endif
	}
	/// <summary>
	/// Allocates a stack buffer with the required size for execution.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IScopedBufferAction{T}"/>.</typeparam>
	/// <param name="action">Method to execute.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void AllocValue<TAction>(ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : IScopedBufferAction<T>
#else
		where TAction : IScopedBufferAction<T>, allows ref struct
#endif
	{
		if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
		{
			BufferManager<T>.StackAlloc(ref action);
			return;
		}

		BufferTypeMetadata<T>? metadata = BufferManager.Storage.GetMetadata<T>(action.Count);
		Boolean stackAlloc = metadata is not null && metadata.SizeOf <= BufferManager.StackAllocationByteLimit &&
			(action.IsMinimalCount || metadata.Size == action.Count || action.Count == 0);
#if !PACKAGE && (NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299)
		BufferManager.Storage.PrintMetadata<T>(!stackAlloc);
#endif
		if (stackAlloc)
		{
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
			Debug.Assert(metadata is not null);
			metadata.Execute(ref action, action.Count);
#else
			metadata!.Execute(ref action, action.Count);
#endif
			return;
		}
		BufferManager<T>.AllocHeap(ref action);
	}
	/// <summary>
	/// Allocates a stack buffer with the required size for execution.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IScopedBufferFunction{T, TFunction}"/>.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="func">Function to execute.</param>
	/// <param name="result">Output. Function result.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void AllocValue<TFunction, TResult>(ref TFunction func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IScopedBufferFunction<T, TResult>
#else
		where TFunction : IScopedBufferFunction<T, TResult>, allows ref struct
#endif
	{
		if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
		{
			BufferManager<T>.StackAlloc(ref func, out result);
			return;
		}

		BufferTypeMetadata<T>? metadata = BufferManager.Storage.GetMetadata<T>(func.Count);
		Boolean stackAlloc = metadata is not null && metadata.SizeOf <= BufferManager.StackAllocationByteLimit &&
			(func.IsMinimalCount || metadata.Size == func.Count || func.Count == 0);
#if !PACKAGE && (NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299)
		BufferManager.Storage.PrintMetadata<T>(!stackAlloc);
#endif
		if (stackAlloc)
		{
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
			Debug.Assert(metadata is not null);
			result = metadata.Execute<TFunction, TResult>(ref func, func.Count);
#else
			result = metadata!.Execute<TFunction, TResult>(ref func, func.Count);
#endif
			return;
		}
		BufferManager<T>.AllocHeap(ref func, out result);
	}
	/// <summary>
	/// Allocates a heap buffer with the required size for execution.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IScopedBufferAction{T}"/>.</typeparam>
	/// <param name="action">Method to execute.</param>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void AllocHeap<TAction>(ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : IScopedBufferAction<T>
#else
		where TAction : IScopedBufferAction<T>, allows ref struct
#endif
	{
		T[] arr = ArrayPool<T>.Shared.Rent(action.Count);
		try
		{
			Span<T> span = arr.AsSpan()[..action.Count];
			ScopedBuffer<T> buffer = new(span, true, arr.Length);

			span.Clear();
			action.Accept(buffer);
		}
		finally
		{
			ArrayPool<T>.Shared.Return(arr, true);
		}
	}
	/// <summary>
	/// Allocates a heap buffer with the required size for execution.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IScopedBufferFunction{T, TFunction}"/>.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="func">Function to execute.</param>
	/// <param name="result">Output. Function result.</param>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void AllocHeap<TFunction, TResult>(ref TFunction func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IScopedBufferFunction<T, TResult>
#else
		where TFunction : IScopedBufferFunction<T, TResult>, allows ref struct
#endif
	{
		T[] arr = ArrayPool<T>.Shared.Rent(func.Count);
		try
		{
			Span<T> span = arr.AsSpan()[..func.Count];
			ScopedBuffer<T> buffer = new(span, true, arr.Length);

			span.Clear();
			result = func.Apply(buffer);
		}
		finally
		{
			ArrayPool<T>.Shared.Return(arr, true);
		}
	}
	/// <summary>
	/// Allocates a stack buffer with the required size for execution.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IScopedBufferAction{T}"/>.</typeparam>
	/// <param name="action">Method to execute.</param>
#if NET7_0_OR_GREATER
	[SkipLocalsInit]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable CS8500
	private static unsafe void StackAlloc<TAction>(ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : IScopedBufferAction<T>
#else
		where TAction : IScopedBufferAction<T>, allows ref struct
#endif
	{
		Int32 sizeOfT = sizeof(T);
		Int32 totalBytes = action.Count * sizeOfT;
		if (totalBytes > BufferManager.StackAllocationByteLimit)
		{
			BufferManager<T>.AllocHeap(ref action);
			return;
		}
		Span<Byte> bytes = stackalloc Byte[totalBytes];
#if NET7_0_OR_GREATER
		bytes.Clear();
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		ref T refT = ref Unsafe.As<Byte, T>(ref MemoryMarshal.GetReference(bytes));
		Span<T> span = MemoryMarshal.CreateSpan(ref refT, action.Count);
#else
		void* ptr = Unsafe.AsPointer(ref MemoryMarshal.GetReference(bytes));
		Span<T> span = MemoryMarshalCompat.CreateUnsafeSpan<T>(ptr, action.Count);
#endif
		ScopedBuffer<T> buffer = new(span, false, span.Length);
		action.Accept(buffer);
	}
	/// <summary>
	/// Allocates a stack buffer with the required size for execution.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IScopedBufferFunction{T, TFunction}"/>.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="func">Function to execute.</param>
	/// <param name="result">Output. Function result.</param>
#if NET7_0_OR_GREATER
	[SkipLocalsInit]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe void StackAlloc<TFunction, TResult>(ref TFunction func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IScopedBufferFunction<T, TResult>
#else
		where TFunction : IScopedBufferFunction<T, TResult>, allows ref struct
#endif
	{
		Int32 sizeOfT = sizeof(T);
		Int32 totalBytes = func.Count * sizeOfT;
		if (totalBytes > BufferManager.StackAllocationByteLimit)
		{
			BufferManager<T>.AllocHeap(ref func, out result);
			return;
		}
		Span<Byte> bytes = stackalloc Byte[totalBytes];
#if NET7_0_OR_GREATER
		bytes.Clear();
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		ref T refT = ref Unsafe.As<Byte, T>(ref MemoryMarshal.GetReference(bytes));
		Span<T> span = MemoryMarshal.CreateSpan(ref refT, func.Count);
#else
		void* ptr = Unsafe.AsPointer(ref MemoryMarshal.GetReference(bytes));
		Span<T> span = MemoryMarshalCompat.CreateUnsafeSpan<T>(ptr, func.Count);
#endif
		ScopedBuffer<T> buffer = new(span, false, span.Length);
		result = func.Apply(buffer);
	}
}