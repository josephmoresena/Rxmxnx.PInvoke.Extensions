namespace Rxmxnx.PInvoke;

public static partial class BufferManager
{
	/// <summary>
	/// Allocation methods for Visual Basic .NET language.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static partial class VisualBasic
	{
		/// <inheritdoc cref="BufferManager.Alloc{T}(UInt16, ScopedBufferAction{T}, Boolean)"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		public static void Alloc<T>(UInt16 count, VbScopedBufferAction<T> action, Boolean isMinimumCount = false)
		{
			if (typeof(T).IsValueType)
				VisualBasic.AllocValue(count, action, isMinimumCount);
			else
				VisualBasic.AllocObject(count, action, isMinimumCount);
		}
		/// <inheritdoc cref="BufferManager.Alloc{T, TState}(UInt16, TState, ScopedBufferAction{T, TState}, Boolean)"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		public static void Alloc<T, TState>(UInt16 count, TState state, VbScopedBufferAction<T, TState> action,
			Boolean isMinimumCount = false)
		{
			if (typeof(T).IsValueType)
				VisualBasic.AllocValue(count, state, action, isMinimumCount);
			else
				VisualBasic.AllocObject(count, state, action, isMinimumCount);
		}
		/// <inheritdoc cref="BufferManager.Alloc{T, TResult}(UInt16, ScopedBufferFunc{T, TResult}, Boolean)"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		public static TResult Alloc<T, TResult>(UInt16 count, VbScopedBufferFunc<T, TResult> func,
			Boolean isMinimumCount = false)
			=> typeof(T).IsValueType ?
				VisualBasic.AllocValue(count, func, isMinimumCount) :
				VisualBasic.AllocObject(count, func, isMinimumCount);
		/// <inheritdoc
		///     cref="BufferManager.Alloc{T, TState, TResult}(UInt16, TState, ScopedBufferFunc{T, TState, TResult}, Boolean)"/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		public static TResult Alloc<T, TState, TResult>(UInt16 count, TState state,
			VbScopedBufferFunc<T, TState, TResult> func, Boolean isMinimumCount = false)
			=> typeof(T).IsValueType ?
				VisualBasic.AllocValue(count, state, func, isMinimumCount) :
				VisualBasic.AllocObject(count, state, func, isMinimumCount);
	}
}