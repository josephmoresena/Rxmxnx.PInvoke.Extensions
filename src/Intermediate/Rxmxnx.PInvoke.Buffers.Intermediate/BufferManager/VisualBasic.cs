namespace Rxmxnx.PInvoke;

public static partial class BufferManager
{
	/// <summary>
	/// Allocation methods for Visual Basic .NET language.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	// ReSharper disable once UnusedType.Global
	public static partial class VisualBasic
	{
		/// <summary>
		/// Allocates a buffer with <paramref name="count"/> elements and executes <paramref name="action"/>.
		/// </summary>
		/// <typeparam name="T">Type of items in allocated buffer.</typeparam>
		/// <param name="count">Number of elements in allocated buffer.</param>
		/// <param name="action">Action to perform with allocated buffer.</param>
		/// <param name="isMinimumCount">
		/// Indicates whether <paramref name="count"/> is just the minimum limit.
		/// </param>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		public static void Alloc<T>(UInt16 count, VbScopedBufferAction<T> action, Boolean isMinimumCount = false)
			=> BufferManager<T>.Alloc(new VbActionValue<T>(action) { Count = count, IsMinimalCount = isMinimumCount, });
		/// <summary>
		/// Allocates a buffer with <paramref name="count"/> elements and executes <paramref name="action"/>.
		/// </summary>
		/// <typeparam name="T">Type of items in allocated buffer.</typeparam>
		/// <typeparam name="TState">Type of state object.</typeparam>
		/// <param name="count">Number of elements in allocated buffer.</param>
		/// <param name="state">State object.</param>
		/// <param name="action">Action to perform with allocated buffer.</param>
		/// <param name="isMinimumCount">
		/// Indicates whether <paramref name="count"/> is just the minimum limit.
		/// </param>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		public static void Alloc<T, TState>(UInt16 count, TState state, VbScopedBufferAction<T, TState> action,
			Boolean isMinimumCount = false)
			=> BufferManager<T>.Alloc(
				new VbActionValue<T, TState>(action, state) { Count = count, IsMinimalCount = isMinimumCount, });
		/// <summary>
		/// Allocates a buffer with <paramref name="count"/> elements and executes <paramref name="func"/>.
		/// </summary>
		/// <typeparam name="T">Type of items in allocated buffer.</typeparam>
		/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
		/// <param name="count">Number of elements in allocated buffer.</param>
		/// <param name="func">Function to execute with allocated buffer.</param>
		/// <param name="isMinimumCount">
		/// Indicates whether <paramref name="count"/> is just the minimum limit.
		/// </param>
		/// <returns><paramref name="func"/> result.</returns>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		public static TResult Alloc<T, TResult>(UInt16 count, VbScopedBufferFunc<T, TResult> func,
			Boolean isMinimumCount = false)
		{
			BufferManager<T>.Alloc(
				new VbFunctionValue<T, TResult>(func) { Count = count, IsMinimalCount = isMinimumCount, },
				out TResult result);
			return result;
		}
		/// <summary>
		/// Allocates a buffer with <paramref name="count"/> elements and executes <paramref name="func"/>.
		/// </summary>
		/// <typeparam name="T">Type of items in allocated buffer.</typeparam>
		/// <typeparam name="TState">Type of state object.</typeparam>
		/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
		/// <param name="count">Number of elements in allocated buffer.</param>
		/// <param name="state">State object.</param>
		/// <param name="func">Function to execute with allocated buffer.</param>
		/// <param name="isMinimumCount">
		/// Indicates whether <paramref name="count"/> is just the minimum limit.
		/// </param>
		/// <returns><paramref name="func"/> result.</returns>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		public static TResult Alloc<T, TState, TResult>(UInt16 count, TState state,
			VbScopedBufferFunc<T, TState, TResult> func, Boolean isMinimumCount = false)
		{
			BufferManager<T>.Alloc(
				new VbFunctionValue<T, TState, TResult>(func, state)
				{
					Count = count, IsMinimalCount = isMinimumCount,
				}, out TResult result);
			return result;
		}
	}
}