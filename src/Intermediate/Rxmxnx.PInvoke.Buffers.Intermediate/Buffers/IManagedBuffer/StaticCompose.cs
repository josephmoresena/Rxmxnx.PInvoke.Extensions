namespace Rxmxnx.PInvoke.Buffers;

public partial interface IManagedBuffer<T>
{
	/// <summary>
	/// Compose statically all buffers in current space.
	/// </summary>
	/// <typeparam name="T0">Buffer space type.</typeparam>
	/// <param name="helper">A <see cref="StaticCompositionHelper{T}"/> instance.</param>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	internal static abstract void StaticCompose<T0>(StaticCompositionHelper<T> helper)
		where T0 : struct, IManagedBuffer<T>;
	/// <summary>
	/// Compose statically all buffers in current space.
	/// </summary>
	/// <typeparam name="T0">1st buffer space type.</typeparam>
	/// <typeparam name="T1">2nd buffer space type.</typeparam>
	/// <param name="s0">Size of 1st buffer space.</param>
	/// <param name="s1">Size of 2nd buffer space.</param>
	/// <param name="helper">A <see cref="StaticCompositionHelper{T}"/> instance.</param>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	private protected static abstract void StaticCompose<T0, T1>(UInt16 s0, UInt16 s1,
		StaticCompositionHelper<T> helper) where T0 : struct, IManagedBuffer<T> where T1 : struct, IManagedBuffer<T>;
	/// <summary>
	/// Compose statically all buffers in current space.
	/// </summary>
	/// <typeparam name="T0">1st buffer space type.</typeparam>
	/// <typeparam name="T1">2nd buffer space type.</typeparam>
	/// <typeparam name="T2">3rd buffer space type.</typeparam>
	/// <param name="s0">Size of 1st buffer space.</param>
	/// <param name="s1">Size of 2nd buffer space.</param>
	/// <param name="s2">Size of 3rd buffer space.</param>
	/// <param name="helper">A <see cref="StaticCompositionHelper{T}"/> instance.</param>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	private protected static abstract void StaticCompose<T0, T1, T2>(UInt16 s0, UInt16 s1, UInt16 s2,
		StaticCompositionHelper<T> helper) where T0 : struct, IManagedBuffer<T>
		where T1 : struct, IManagedBuffer<T>
		where T2 : struct, IManagedBuffer<T>;
	/// <summary>
	/// Compose statically all buffers in current space.
	/// </summary>
	/// <typeparam name="T0">1st buffer space type.</typeparam>
	/// <typeparam name="T1">2nd buffer space type.</typeparam>
	/// <typeparam name="T2">3rd buffer space type.</typeparam>
	/// <typeparam name="T3">4th buffer space type.</typeparam>
	/// <param name="s0">Size of 1st buffer space.</param>
	/// <param name="s1">Size of 2nd buffer space.</param>
	/// <param name="s2">Size of 3rd buffer space.</param>
	/// <param name="s3">Size of 4th buffer space.</param>
	/// <param name="helper">A <see cref="StaticCompositionHelper{T}"/> instance.</param>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	private protected static abstract void StaticCompose<T0, T1, T2, T3>(UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3,
		StaticCompositionHelper<T> helper) where T0 : struct, IManagedBuffer<T>
		where T1 : struct, IManagedBuffer<T>
		where T2 : struct, IManagedBuffer<T>
		where T3 : struct, IManagedBuffer<T>;
	/// <summary>
	/// Compose statically all buffers in current space.
	/// </summary>
	/// <typeparam name="T0">1st buffer space type.</typeparam>
	/// <typeparam name="T1">2nd buffer space type.</typeparam>
	/// <typeparam name="T2">3rd buffer space type.</typeparam>
	/// <typeparam name="T3">4th buffer space type.</typeparam>
	/// <typeparam name="T4">5th buffer space type.</typeparam>
	/// <param name="s0">Size of 1st buffer space.</param>
	/// <param name="s1">Size of 2nd buffer space.</param>
	/// <param name="s2">Size of 3rd buffer space.</param>
	/// <param name="s3">Size of 4th buffer space.</param>
	/// <param name="s4">Size of 5th buffer space.</param>
	/// <param name="helper">A <see cref="StaticCompositionHelper{T}"/> instance.</param>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	private protected static abstract void StaticCompose<T0, T1, T2, T3, T4>(UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3,
		UInt16 s4, StaticCompositionHelper<T> helper) where T0 : struct, IManagedBuffer<T>
		where T1 : struct, IManagedBuffer<T>
		where T2 : struct, IManagedBuffer<T>
		where T3 : struct, IManagedBuffer<T>
		where T4 : struct, IManagedBuffer<T>;
	/// <summary>
	/// Compose statically all buffers in current space.
	/// </summary>
	/// <typeparam name="T0">1st buffer space type.</typeparam>
	/// <typeparam name="T1">2nd buffer space type.</typeparam>
	/// <typeparam name="T2">3rd buffer space type.</typeparam>
	/// <typeparam name="T3">4th buffer space type.</typeparam>
	/// <typeparam name="T4">5th buffer space type.</typeparam>
	/// <typeparam name="T5">6th buffer space type.</typeparam>
	/// <param name="s0">Size of 1st buffer space.</param>
	/// <param name="s1">Size of 2nd buffer space.</param>
	/// <param name="s2">Size of 3rd buffer space.</param>
	/// <param name="s3">Size of 4th buffer space.</param>
	/// <param name="s4">Size of 5th buffer space.</param>
	/// <param name="s5">Size of 6th buffer space.</param>
	/// <param name="helper">A <see cref="StaticCompositionHelper{T}"/> instance.</param>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	private protected static abstract void StaticCompose<T0, T1, T2, T3, T4, T5>(UInt16 s0, UInt16 s1, UInt16 s2,
		UInt16 s3, UInt16 s4, UInt16 s5, StaticCompositionHelper<T> helper) where T0 : struct, IManagedBuffer<T>
		where T1 : struct, IManagedBuffer<T>
		where T2 : struct, IManagedBuffer<T>
		where T3 : struct, IManagedBuffer<T>
		where T4 : struct, IManagedBuffer<T>
		where T5 : struct, IManagedBuffer<T>;
}