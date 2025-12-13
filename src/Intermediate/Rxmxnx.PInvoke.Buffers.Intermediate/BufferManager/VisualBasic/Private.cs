namespace Rxmxnx.PInvoke;

public static partial class BufferManager
{
	// ReSharper disable once UnusedType.Global
	public static partial class VisualBasic
	{
		/// <inheritdoc cref="BufferManager.AllocHeap{T}(UInt16, ScopedBufferAction{T})"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		private static void AllocHeap<T>(UInt16 count, VbScopedBufferAction<T> action)
		{
			T[] arr = ArrayPool<T>.Shared.Rent(count);
			VbScopedBuffer<T> buffer = new(arr, count);
			try
			{
				arr.AsSpan()[..count].Clear();
				action(buffer);
			}
			finally
			{
				buffer.Unload();
				ArrayPool<T>.Shared.Return(arr, true);
			}
		}
		/// <inheritdoc cref="BufferManager.AllocHeap{T, TState}(UInt16, TState, ScopedBufferAction{T, TState})"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		private static void AllocHeap<T, TState>(UInt16 count, TState state, VbScopedBufferAction<T, TState> action)
		{
			T[] arr = ArrayPool<T>.Shared.Rent(count);
			VbScopedBuffer<T> buffer = new(arr, count);
			try
			{
				arr.AsSpan()[..count].Clear();
				action(buffer, state);
			}
			finally
			{
				buffer.Unload();
				ArrayPool<T>.Shared.Return(arr, true);
			}
		}
		/// <inheritdoc cref="BufferManager.AllocHeap{T, TResult}(UInt16, ScopedBufferFunc{T, TResult})"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		private static TResult AllocHeap<T, TResult>(UInt16 count, VbScopedBufferFunc<T, TResult> func)
		{
			T[] arr = ArrayPool<T>.Shared.Rent(count);
			VbScopedBuffer<T> buffer = new(arr, count);
			try
			{
				arr.AsSpan()[..count].Clear();
				return func(buffer);
			}
			finally
			{
				buffer.Unload();
				ArrayPool<T>.Shared.Return(arr, true);
			}
		}
		/// <inheritdoc cref="BufferManager.AllocHeap{T, TState, TResult}(UInt16, TState, ScopedBufferFunc{T, TState, TResult})"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		private static TResult AllocHeap<T, TState, TResult>(UInt16 count, TState state,
			VbScopedBufferFunc<T, TState, TResult> func)
		{
			T[] arr = ArrayPool<T>.Shared.Rent(count);
			VbScopedBuffer<T> buffer = new(arr, count);
			try
			{
				arr.AsSpan()[..count].Clear();
				return func(buffer, state);
			}
			finally
			{
				buffer.Unload();
				ArrayPool<T>.Shared.Return(arr, true);
			}
		}
		/// <inheritdoc cref="BufferManager.AllocObject{T}(UInt16, ScopedBufferAction{T}, Boolean)"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		private static void AllocObject<T>(UInt16 count, VbScopedBufferAction<T> action, Boolean isMinimumCount)
		{
			BufferTypeMetadata<Object>? metadata = MetadataManager<Object>.GetMetadata(count);
			Boolean stackAlloc = metadata is not null && (isMinimumCount || metadata.Size == count);
#if !PACKAGE
			MetadataManager<Object>.PrintMetadata(!stackAlloc);
#endif
			if (stackAlloc)
				VisualBasic.StackAllocObject(metadata!, count, action);
			else
				VisualBasic.AllocHeap(count, action);
		}
		/// <inheritdoc cref="BufferManager.AllocObject{T, TState}(UInt16, TState, ScopedBufferAction{T, TState}, Boolean)"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		private static void AllocObject<T, TState>(UInt16 count, TState state, VbScopedBufferAction<T, TState> action,
			Boolean isMinimumCount)
		{
			BufferTypeMetadata<Object>? metadata = MetadataManager<Object>.GetMetadata(count);
			Boolean stackAlloc = metadata is not null && (isMinimumCount || metadata.Size == count);
#if !PACKAGE
			MetadataManager<Object>.PrintMetadata(!stackAlloc);
#endif
			if (stackAlloc)
				metadata!.Execute(state, action, count);
			else
				VisualBasic.AllocHeap(count, state, action);
		}
		/// <inheritdoc cref="BufferManager.AllocObject{T, TResult}(UInt16, ScopedBufferFunc{T, TResult}, Boolean)"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		private static TResult AllocObject<T, TResult>(UInt16 count, VbScopedBufferFunc<T, TResult> func,
			Boolean isMinimumCount)
		{
			BufferTypeMetadata<Object>? metadata = MetadataManager<Object>.GetMetadata(count);
			Boolean stackAlloc = metadata is not null && (isMinimumCount || metadata.Size == count);
#if !PACKAGE
			MetadataManager<Object>.PrintMetadata(!stackAlloc);
#endif
			return stackAlloc ?
				VisualBasic.StackAllocObject(metadata!, count, func) :
				VisualBasic.AllocHeap(count, func);
		}
		/// <inheritdoc
		///     cref="BufferManager.AllocObject{T, TState, TResult}(UInt16, TState, ScopedBufferFunc{T, TState, TResult}, Boolean)"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		private static TResult AllocObject<T, TState, TResult>(UInt16 count, TState state,
			VbScopedBufferFunc<T, TState, TResult> func, Boolean isMinimumCount)
		{
			BufferTypeMetadata<Object>? metadata = MetadataManager<Object>.GetMetadata(count);
			Boolean stackAlloc = metadata is not null && (isMinimumCount || metadata.Size == count);
#if !PACKAGE
			MetadataManager<Object>.PrintMetadata(!stackAlloc);
#endif
			return stackAlloc ? metadata!.Execute(state, func, count) : VisualBasic.AllocHeap(count, state, func);
		}
		/// <inheritdoc cref="BufferManager.AllocValue{T}(UInt16, ScopedBufferAction{T}, Boolean)"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		private static void AllocValue<T>(UInt16 count, VbScopedBufferAction<T> action, Boolean isMinimumCount)
		{
			if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
			{
				VisualBasic.StackAlloc(count, action);
				return;
			}

			BufferTypeMetadata<T>? metadata = MetadataManager<T>.GetMetadata(count);
			Boolean stackAlloc = metadata is not null && (isMinimumCount || metadata.Size == count);
#if !PACKAGE
			MetadataManager<T>.PrintMetadata(!stackAlloc);
#endif
			if (stackAlloc)
			{
				VbTransformationState<T> state = new(action);
				metadata!.Execute(state, VbTransformationState<T>.Execute, count);
			}
			else
			{
				VisualBasic.AllocHeap(count, action);
			}
		}
		/// <inheritdoc cref="BufferManager.AllocValue{T, TState}(UInt16, TState, ScopedBufferAction{T, TState}, Boolean)"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		private static void AllocValue<T, TState>(UInt16 count, TState state, VbScopedBufferAction<T, TState> action,
			Boolean isMinimumCount)
		{
			if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
			{
				VisualBasic.StackAlloc(count, state, action);
				return;
			}

			BufferTypeMetadata<T>? metadata = MetadataManager<T>.GetMetadata(count);
			Boolean stackAlloc = metadata is not null && (isMinimumCount || metadata.Size == count);
#if !PACKAGE
			MetadataManager<T>.PrintMetadata(!stackAlloc);
#endif
			if (stackAlloc)
				metadata!.Execute<TState>(state, action, count);
			else
				VisualBasic.AllocHeap(count, state, action);
		}
		/// <inheritdoc cref="BufferManager.AllocValue{T, TResult}(UInt16, ScopedBufferFunc{T, TResult}, Boolean)"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		private static TResult AllocValue<T, TResult>(UInt16 count, VbScopedBufferFunc<T, TResult> func,
			Boolean isMinimumCount)
		{
			if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
				return VisualBasic.StackAlloc(count, func);

			BufferTypeMetadata<T>? metadata = MetadataManager<T>.GetMetadata(count);
			Boolean stackAlloc = metadata is not null && (isMinimumCount || metadata.Size == count);
#if !PACKAGE
			MetadataManager<T>.PrintMetadata(!stackAlloc);
#endif
			if (!stackAlloc) return VisualBasic.AllocHeap(count, func);

			VbTransformationState<T, TResult> state = new(func);
			return metadata!.Execute(state, VbTransformationState<T, TResult>.Execute, count);
		}
		/// <inheritdoc
		///     cref="BufferManager.AllocValue{T, TState, TResult}(UInt16, TState, ScopedBufferFunc{T, TState, TResult}, Boolean)"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		private static TResult AllocValue<T, TState, TResult>(UInt16 count, TState state,
			VbScopedBufferFunc<T, TState, TResult> func, Boolean isMinimumCount)
		{
			if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
				return VisualBasic.StackAlloc(count, state, func);

			BufferTypeMetadata<T>? metadata = MetadataManager<T>.GetMetadata(count);
			Boolean stackAlloc = metadata is not null && (isMinimumCount || metadata.Size == count);
#if !PACKAGE
			MetadataManager<T>.PrintMetadata(!stackAlloc);
#endif
			return stackAlloc ?
				metadata!.Execute<TState, TResult>(state, func, count) :
				VisualBasic.AllocHeap(count, state, func);
		}
		/// <inheritdoc cref="BufferManager.StackAllocObject{T}(BufferTypeMetadata{Object}, UInt16, ScopedBufferAction{T})"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void StackAllocObject<T>(BufferTypeMetadata<Object> metadata, UInt16 count,
			VbScopedBufferAction<T> action)
		{
			VbTransformationState<T> stateT = new(action);
			metadata.Execute(stateT, VbTransformationState<T>.Execute, count);
		}
		/// <inheritdoc
		///     cref="BufferManager.StackAllocObject{T, TResult}(BufferTypeMetadata{Object}, UInt16, ScopedBufferFunc{T, TResult})"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static TResult StackAllocObject<T, TResult>(BufferTypeMetadata<Object> metadata, UInt16 count,
			VbScopedBufferFunc<T, TResult> func)
		{
			VbTransformationState<T, TResult> stateT = new(func);
			return metadata.Execute(stateT, VbTransformationState<T, TResult>.Execute, count);
		}

		/// <inheritdoc cref="BufferManager.StackAlloc{T}(UInt16, ScopedBufferAction{T})"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable CS8500
		private static unsafe void StackAlloc<T>(UInt16 count, VbScopedBufferAction<T> action)
		{
			Int32 sizeOfT = sizeof(T);
			Span<Byte> bytes = stackalloc Byte[count * sizeOfT];
			ref T refT = ref Unsafe.As<Byte, T>(ref MemoryMarshal.GetReference(bytes));
			VbScopedBuffer<T> buffer = new(ref refT, count);
			try
			{
				action(buffer);
			}
			finally
			{
				buffer.Unload();
			}
		}
		/// <inheritdoc cref="BufferManager.StackAlloc{T, TState}(UInt16, TState, ScopedBufferAction{T, TState})"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void StackAlloc<T, TState>(UInt16 count, TState state,
			VbScopedBufferAction<T, TState> action)
		{
			Int32 sizeOfT = sizeof(T);
			Span<Byte> bytes = stackalloc Byte[count * sizeOfT];
			ref T refT = ref Unsafe.As<Byte, T>(ref MemoryMarshal.GetReference(bytes));
			VbScopedBuffer<T> buffer = new(ref refT, count);
			try
			{
				action(buffer, state);
			}
			finally
			{
				buffer.Unload();
			}
		}
		/// <inheritdoc cref="BufferManager.StackAlloc{T, TResult}(UInt16, ScopedBufferFunc{T, TResult})"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe TResult StackAlloc<T, TResult>(UInt16 count, VbScopedBufferFunc<T, TResult> func)
		{
			Int32 sizeOfT = sizeof(T);
			Span<Byte> bytes = stackalloc Byte[count * sizeOfT];
			ref T refT = ref Unsafe.As<Byte, T>(ref MemoryMarshal.GetReference(bytes));
			VbScopedBuffer<T> buffer = new(ref refT, count);
			try
			{
				return func(buffer);
			}
			finally
			{
				buffer.Unload();
			}
		}
		/// <inheritdoc cref="BufferManager.StackAlloc{T, TState, TResult}(UInt16, TState, ScopedBufferFunc{T, TState, TResult})"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe TResult StackAlloc<T, TState, TResult>(UInt16 count, TState state,
			VbScopedBufferFunc<T, TState, TResult> func)
		{
			Int32 sizeOfT = sizeof(T);
			Span<Byte> bytes = stackalloc Byte[count * sizeOfT];
			ref T refT = ref Unsafe.As<Byte, T>(ref MemoryMarshal.GetReference(bytes));
			VbScopedBuffer<T> buffer = new(ref refT, count);
			try
			{
				return func(buffer, state);
			}
			finally
			{
				buffer.Unload();
			}
		}
#pragma warning restore CS8500
	}
}