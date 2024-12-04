namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// This interfaces exposes an allocated buffer.
/// </summary>
/// <typeparam name="TBuffer">The type of buffer.</typeparam>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
public interface IManagedBinaryBuffer<TBuffer, T> : IManagedBuffer<T>
	where TBuffer : struct, IManagedBinaryBuffer<TBuffer, T>
{
#if BINARY_SPACES
	/// <summary>
	/// Compose statically all buffers in current space.
	/// </summary>
	/// <typeparam name="T0">Buffer space type.</typeparam>
	/// <param name="s0"></param>
	/// <param name="helper">A <see cref="StaticCompositionHelper{T}"/> instance.</param>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	internal static abstract void StaticCompose<T0>(UInt16 s0, StaticCompositionHelper<T> helper)
		where T0 : struct, IManagedBinaryBuffer<T0, T>;
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
		StaticCompositionHelper<T> helper) where T0 : struct, IManagedBinaryBuffer<T0, T>
		where T1 : struct, IManagedBinaryBuffer<T1, T>;
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
		StaticCompositionHelper<T> helper) where T0 : struct, IManagedBinaryBuffer<T0, T>
		where T1 : struct, IManagedBinaryBuffer<T1, T>
		where T2 : struct, IManagedBinaryBuffer<T2, T>;
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
		StaticCompositionHelper<T> helper) where T0 : struct, IManagedBinaryBuffer<T0, T>
		where T1 : struct, IManagedBinaryBuffer<T1, T>
		where T2 : struct, IManagedBinaryBuffer<T2, T>
		where T3 : struct, IManagedBinaryBuffer<T3, T>;
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
		UInt16 s4, StaticCompositionHelper<T> helper) where T0 : struct, IManagedBinaryBuffer<T0, T>
		where T1 : struct, IManagedBinaryBuffer<T1, T>
		where T2 : struct, IManagedBinaryBuffer<T2, T>
		where T3 : struct, IManagedBinaryBuffer<T3, T>
		where T4 : struct, IManagedBinaryBuffer<T4, T>;
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
		UInt16 s3, UInt16 s4, UInt16 s5, StaticCompositionHelper<T> helper)
		where T0 : struct, IManagedBinaryBuffer<T0, T>
		where T1 : struct, IManagedBinaryBuffer<T1, T>
		where T2 : struct, IManagedBinaryBuffer<T2, T>
		where T3 : struct, IManagedBinaryBuffer<T3, T>
		where T4 : struct, IManagedBinaryBuffer<T4, T>
		where T5 : struct, IManagedBinaryBuffer<T5, T>;
	/// <summary>
	/// Compose statically all buffers in current space.
	/// </summary>
	/// <typeparam name="T0">1st buffer space type.</typeparam>
	/// <typeparam name="T1">2nd buffer space type.</typeparam>
	/// <typeparam name="T2">3rd buffer space type.</typeparam>
	/// <typeparam name="T3">4th buffer space type.</typeparam>
	/// <typeparam name="T4">5th buffer space type.</typeparam>
	/// <typeparam name="T5">6th buffer space type.</typeparam>
	/// <typeparam name="T6">7th buffer space type.</typeparam>
	/// <param name="s0">Size of 1st buffer space.</param>
	/// <param name="s1">Size of 2nd buffer space.</param>
	/// <param name="s2">Size of 3rd buffer space.</param>
	/// <param name="s3">Size of 4th buffer space.</param>
	/// <param name="s4">Size of 5th buffer space.</param>
	/// <param name="s5">Size of 6th buffer space.</param>
	/// <param name="s6">Size of 7th buffer space.</param>
	/// <param name="helper">A <see cref="StaticCompositionHelper{T}"/> instance.</param>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	private protected static abstract void StaticCompose<T0, T1, T2, T3, T4, T5, T6>(UInt16 s0, UInt16 s1, UInt16 s2,
		UInt16 s3, UInt16 s4, UInt16 s5, UInt16 s6, StaticCompositionHelper<T> helper)
		where T0 : struct, IManagedBinaryBuffer<T0, T>
		where T1 : struct, IManagedBinaryBuffer<T1, T>
		where T2 : struct, IManagedBinaryBuffer<T2, T>
		where T3 : struct, IManagedBinaryBuffer<T3, T>
		where T4 : struct, IManagedBinaryBuffer<T4, T>
		where T5 : struct, IManagedBinaryBuffer<T5, T>
		where T6 : struct, IManagedBinaryBuffer<T6, T>;
	/// <summary>
	/// Compose statically all buffers in current space.
	/// </summary>
	/// <typeparam name="T0">1st buffer space type.</typeparam>
	/// <typeparam name="T1">2nd buffer space type.</typeparam>
	/// <typeparam name="T2">3rd buffer space type.</typeparam>
	/// <typeparam name="T3">4th buffer space type.</typeparam>
	/// <typeparam name="T4">5th buffer space type.</typeparam>
	/// <typeparam name="T5">6th buffer space type.</typeparam>
	/// <typeparam name="T6">7th buffer space type.</typeparam>
	/// <typeparam name="T7">8th buffer space type.</typeparam>
	/// <param name="s0">Size of 1st buffer space.</param>
	/// <param name="s1">Size of 2nd buffer space.</param>
	/// <param name="s2">Size of 3rd buffer space.</param>
	/// <param name="s3">Size of 4th buffer space.</param>
	/// <param name="s4">Size of 5th buffer space.</param>
	/// <param name="s5">Size of 6th buffer space.</param>
	/// <param name="s6">Size of 7th buffer space.</param>
	/// <param name="s7">Size of 8th buffer space.</param>
	/// <param name="helper">A <see cref="StaticCompositionHelper{T}"/> instance.</param>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	private protected static abstract void StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7>(UInt16 s0, UInt16 s1,
		UInt16 s2, UInt16 s3, UInt16 s4, UInt16 s5, UInt16 s6, UInt16 s7, StaticCompositionHelper<T> helper)
		where T0 : struct, IManagedBinaryBuffer<T0, T>
		where T1 : struct, IManagedBinaryBuffer<T1, T>
		where T2 : struct, IManagedBinaryBuffer<T2, T>
		where T3 : struct, IManagedBinaryBuffer<T3, T>
		where T4 : struct, IManagedBinaryBuffer<T4, T>
		where T5 : struct, IManagedBinaryBuffer<T5, T>
		where T6 : struct, IManagedBinaryBuffer<T6, T>
		where T7 : struct, IManagedBinaryBuffer<T7, T>;
	/// <summary>
	/// Compose statically all buffers in current space.
	/// </summary>
	/// <typeparam name="T0">1st buffer space type.</typeparam>
	/// <typeparam name="T1">2nd buffer space type.</typeparam>
	/// <typeparam name="T2">3rd buffer space type.</typeparam>
	/// <typeparam name="T3">4th buffer space type.</typeparam>
	/// <typeparam name="T4">5th buffer space type.</typeparam>
	/// <typeparam name="T5">6th buffer space type.</typeparam>
	/// <typeparam name="T6">7th buffer space type.</typeparam>
	/// <typeparam name="T7">8th buffer space type.</typeparam>
	/// <typeparam name="T8">9th buffer space type.</typeparam>
	/// <param name="s0">Size of 1st buffer space.</param>
	/// <param name="s1">Size of 2nd buffer space.</param>
	/// <param name="s2">Size of 3rd buffer space.</param>
	/// <param name="s3">Size of 4th buffer space.</param>
	/// <param name="s4">Size of 5th buffer space.</param>
	/// <param name="s5">Size of 6th buffer space.</param>
	/// <param name="s6">Size of 7th buffer space.</param>
	/// <param name="s7">Size of 8th buffer space.</param>
	/// <param name="s8">Size of 9th buffer space.</param>
	/// <param name="helper">A <see cref="StaticCompositionHelper{T}"/> instance.</param>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	private protected static abstract void StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8>(UInt16 s0, UInt16 s1,
		UInt16 s2, UInt16 s3, UInt16 s4, UInt16 s5, UInt16 s6, UInt16 s7, UInt16 s8, StaticCompositionHelper<T> helper)
		where T0 : struct, IManagedBinaryBuffer<T0, T>
		where T1 : struct, IManagedBinaryBuffer<T1, T>
		where T2 : struct, IManagedBinaryBuffer<T2, T>
		where T3 : struct, IManagedBinaryBuffer<T3, T>
		where T4 : struct, IManagedBinaryBuffer<T4, T>
		where T5 : struct, IManagedBinaryBuffer<T5, T>
		where T6 : struct, IManagedBinaryBuffer<T6, T>
		where T7 : struct, IManagedBinaryBuffer<T7, T>
		where T8 : struct, IManagedBinaryBuffer<T8, T>;
	/// <summary>
	/// Compose statically all buffers in current space.
	/// </summary>
	/// <typeparam name="T0">1st buffer space type.</typeparam>
	/// <typeparam name="T1">2nd buffer space type.</typeparam>
	/// <typeparam name="T2">3rd buffer space type.</typeparam>
	/// <typeparam name="T3">4th buffer space type.</typeparam>
	/// <typeparam name="T4">5th buffer space type.</typeparam>
	/// <typeparam name="T5">6th buffer space type.</typeparam>
	/// <typeparam name="T6">7th buffer space type.</typeparam>
	/// <typeparam name="T7">8th buffer space type.</typeparam>
	/// <typeparam name="T8">9th buffer space type.</typeparam>
	/// <typeparam name="T9">10th buffer space type.</typeparam>
	/// <param name="s0">Size of 1st buffer space.</param>
	/// <param name="s1">Size of 2nd buffer space.</param>
	/// <param name="s2">Size of 3rd buffer space.</param>
	/// <param name="s3">Size of 4th buffer space.</param>
	/// <param name="s4">Size of 5th buffer space.</param>
	/// <param name="s5">Size of 6th buffer space.</param>
	/// <param name="s6">Size of 7th buffer space.</param>
	/// <param name="s7">Size of 8th buffer space.</param>
	/// <param name="s8">Size of 9th buffer space.</param>
	/// <param name="s9">Size of 10th buffer space.</param>
	/// <param name="helper">A <see cref="StaticCompositionHelper{T}"/> instance.</param>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	private protected static abstract void StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(UInt16 s0, UInt16 s1,
		UInt16 s2, UInt16 s3, UInt16 s4, UInt16 s5, UInt16 s6, UInt16 s7, UInt16 s8, UInt16 s9,
		StaticCompositionHelper<T> helper) where T0 : struct, IManagedBinaryBuffer<T0, T>
		where T1 : struct, IManagedBinaryBuffer<T1, T>
		where T2 : struct, IManagedBinaryBuffer<T2, T>
		where T3 : struct, IManagedBinaryBuffer<T3, T>
		where T4 : struct, IManagedBinaryBuffer<T4, T>
		where T5 : struct, IManagedBinaryBuffer<T5, T>
		where T6 : struct, IManagedBinaryBuffer<T6, T>
		where T7 : struct, IManagedBinaryBuffer<T7, T>
		where T8 : struct, IManagedBinaryBuffer<T8, T>
		where T9 : struct, IManagedBinaryBuffer<T9, T>;
	/// <summary>
	/// Compose statically all buffers in current space.
	/// </summary>
	/// <typeparam name="T0">1st buffer space type.</typeparam>
	/// <typeparam name="T1">2nd buffer space type.</typeparam>
	/// <typeparam name="T2">3rd buffer space type.</typeparam>
	/// <typeparam name="T3">4th buffer space type.</typeparam>
	/// <typeparam name="T4">5th buffer space type.</typeparam>
	/// <typeparam name="T5">6th buffer space type.</typeparam>
	/// <typeparam name="T6">7th buffer space type.</typeparam>
	/// <typeparam name="T7">8th buffer space type.</typeparam>
	/// <typeparam name="T8">9th buffer space type.</typeparam>
	/// <typeparam name="T9">10th buffer space type.</typeparam>
	/// <typeparam name="T10">11th buffer space type.</typeparam>
	/// <param name="s0">Size of 1st buffer space.</param>
	/// <param name="s1">Size of 2nd buffer space.</param>
	/// <param name="s2">Size of 3rd buffer space.</param>
	/// <param name="s3">Size of 4th buffer space.</param>
	/// <param name="s4">Size of 5th buffer space.</param>
	/// <param name="s5">Size of 6th buffer space.</param>
	/// <param name="s6">Size of 7th buffer space.</param>
	/// <param name="s7">Size of 8th buffer space.</param>
	/// <param name="s8">Size of 9th buffer space.</param>
	/// <param name="s9">Size of 10th buffer space.</param>
	/// <param name="s10">Size of 11th buffer space.</param>
	/// <param name="helper">A <see cref="StaticCompositionHelper{T}"/> instance.</param>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	private protected static abstract void StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(UInt16 s0,
		UInt16 s1, UInt16 s2, UInt16 s3, UInt16 s4, UInt16 s5, UInt16 s6, UInt16 s7, UInt16 s8, UInt16 s9, UInt16 s10,
		StaticCompositionHelper<T> helper) where T0 : struct, IManagedBinaryBuffer<T0, T>
		where T1 : struct, IManagedBinaryBuffer<T1, T>
		where T2 : struct, IManagedBinaryBuffer<T2, T>
		where T3 : struct, IManagedBinaryBuffer<T3, T>
		where T4 : struct, IManagedBinaryBuffer<T4, T>
		where T5 : struct, IManagedBinaryBuffer<T5, T>
		where T6 : struct, IManagedBinaryBuffer<T6, T>
		where T7 : struct, IManagedBinaryBuffer<T7, T>
		where T8 : struct, IManagedBinaryBuffer<T8, T>
		where T9 : struct, IManagedBinaryBuffer<T9, T>
		where T10 : struct, IManagedBinaryBuffer<T10, T>;
	/// <summary>
	/// Compose statically all buffers in current space.
	/// </summary>
	/// <typeparam name="T0">1st buffer space type.</typeparam>
	/// <typeparam name="T1">2nd buffer space type.</typeparam>
	/// <typeparam name="T2">3rd buffer space type.</typeparam>
	/// <typeparam name="T3">4th buffer space type.</typeparam>
	/// <typeparam name="T4">5th buffer space type.</typeparam>
	/// <typeparam name="T5">6th buffer space type.</typeparam>
	/// <typeparam name="T6">7th buffer space type.</typeparam>
	/// <typeparam name="T7">8th buffer space type.</typeparam>
	/// <typeparam name="T8">9th buffer space type.</typeparam>
	/// <typeparam name="T9">10th buffer space type.</typeparam>
	/// <typeparam name="T10">11th buffer space type.</typeparam>
	/// <typeparam name="T11">12th buffer space type.</typeparam>
	/// <param name="s0">Size of 1st buffer space.</param>
	/// <param name="s1">Size of 2nd buffer space.</param>
	/// <param name="s2">Size of 3rd buffer space.</param>
	/// <param name="s3">Size of 4th buffer space.</param>
	/// <param name="s4">Size of 5th buffer space.</param>
	/// <param name="s5">Size of 6th buffer space.</param>
	/// <param name="s6">Size of 7th buffer space.</param>
	/// <param name="s7">Size of 8th buffer space.</param>
	/// <param name="s8">Size of 9th buffer space.</param>
	/// <param name="s9">Size of 10th buffer space.</param>
	/// <param name="s10">Size of 11th buffer space.</param>
	/// <param name="s11">Size of 12th buffer space.</param>
	/// <param name="helper">A <see cref="StaticCompositionHelper{T}"/> instance.</param>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	private protected static abstract void StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(UInt16 s0,
		UInt16 s1, UInt16 s2, UInt16 s3, UInt16 s4, UInt16 s5, UInt16 s6, UInt16 s7, UInt16 s8, UInt16 s9, UInt16 s10,
		UInt16 s11, StaticCompositionHelper<T> helper) where T0 : struct, IManagedBinaryBuffer<T0, T>
		where T1 : struct, IManagedBinaryBuffer<T1, T>
		where T2 : struct, IManagedBinaryBuffer<T2, T>
		where T3 : struct, IManagedBinaryBuffer<T3, T>
		where T4 : struct, IManagedBinaryBuffer<T4, T>
		where T5 : struct, IManagedBinaryBuffer<T5, T>
		where T6 : struct, IManagedBinaryBuffer<T6, T>
		where T7 : struct, IManagedBinaryBuffer<T7, T>
		where T8 : struct, IManagedBinaryBuffer<T8, T>
		where T9 : struct, IManagedBinaryBuffer<T9, T>
		where T10 : struct, IManagedBinaryBuffer<T10, T>
		where T11 : struct, IManagedBinaryBuffer<T11, T>;
	/// <summary>
	/// Compose statically all buffers in current space.
	/// </summary>
	/// <typeparam name="T0">1st buffer space type.</typeparam>
	/// <typeparam name="T1">2nd buffer space type.</typeparam>
	/// <typeparam name="T2">3rd buffer space type.</typeparam>
	/// <typeparam name="T3">4th buffer space type.</typeparam>
	/// <typeparam name="T4">5th buffer space type.</typeparam>
	/// <typeparam name="T5">6th buffer space type.</typeparam>
	/// <typeparam name="T6">7th buffer space type.</typeparam>
	/// <typeparam name="T7">8th buffer space type.</typeparam>
	/// <typeparam name="T8">9th buffer space type.</typeparam>
	/// <typeparam name="T9">10th buffer space type.</typeparam>
	/// <typeparam name="T10">11th buffer space type.</typeparam>
	/// <typeparam name="T11">12th buffer space type.</typeparam>
	/// <typeparam name="T12">13th buffer space type.</typeparam>
	/// <param name="s0">Size of 1st buffer space.</param>
	/// <param name="s1">Size of 2nd buffer space.</param>
	/// <param name="s2">Size of 3rd buffer space.</param>
	/// <param name="s3">Size of 4th buffer space.</param>
	/// <param name="s4">Size of 5th buffer space.</param>
	/// <param name="s5">Size of 6th buffer space.</param>
	/// <param name="s6">Size of 7th buffer space.</param>
	/// <param name="s7">Size of 8th buffer space.</param>
	/// <param name="s8">Size of 9th buffer space.</param>
	/// <param name="s9">Size of 10th buffer space.</param>
	/// <param name="s10">Size of 11th buffer space.</param>
	/// <param name="s11">Size of 12th buffer space.</param>
	/// <param name="s12">Size of 13th buffer space.</param>
	/// <param name="helper">A <see cref="StaticCompositionHelper{T}"/> instance.</param>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	private protected static abstract void
		StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3,
			UInt16 s4, UInt16 s5, UInt16 s6, UInt16 s7, UInt16 s8, UInt16 s9, UInt16 s10, UInt16 s11, UInt16 s12,
			StaticCompositionHelper<T> helper) where T0 : struct, IManagedBinaryBuffer<T0, T>
		where T1 : struct, IManagedBinaryBuffer<T1, T>
		where T2 : struct, IManagedBinaryBuffer<T2, T>
		where T3 : struct, IManagedBinaryBuffer<T3, T>
		where T4 : struct, IManagedBinaryBuffer<T4, T>
		where T5 : struct, IManagedBinaryBuffer<T5, T>
		where T6 : struct, IManagedBinaryBuffer<T6, T>
		where T7 : struct, IManagedBinaryBuffer<T7, T>
		where T8 : struct, IManagedBinaryBuffer<T8, T>
		where T9 : struct, IManagedBinaryBuffer<T9, T>
		where T10 : struct, IManagedBinaryBuffer<T10, T>
		where T11 : struct, IManagedBinaryBuffer<T11, T>
		where T12 : struct, IManagedBinaryBuffer<T12, T>;
	/// <summary>
	/// Compose statically all buffers in current space.
	/// </summary>
	/// <typeparam name="T0">1st buffer space type.</typeparam>
	/// <typeparam name="T1">2nd buffer space type.</typeparam>
	/// <typeparam name="T2">3rd buffer space type.</typeparam>
	/// <typeparam name="T3">4th buffer space type.</typeparam>
	/// <typeparam name="T4">5th buffer space type.</typeparam>
	/// <typeparam name="T5">6th buffer space type.</typeparam>
	/// <typeparam name="T6">7th buffer space type.</typeparam>
	/// <typeparam name="T7">8th buffer space type.</typeparam>
	/// <typeparam name="T8">9th buffer space type.</typeparam>
	/// <typeparam name="T9">10th buffer space type.</typeparam>
	/// <typeparam name="T10">11th buffer space type.</typeparam>
	/// <typeparam name="T11">12th buffer space type.</typeparam>
	/// <typeparam name="T12">13th buffer space type.</typeparam>
	/// <typeparam name="T13">14th buffer space type.</typeparam>
	/// <param name="s0">Size of 1st buffer space.</param>
	/// <param name="s1">Size of 2nd buffer space.</param>
	/// <param name="s2">Size of 3rd buffer space.</param>
	/// <param name="s3">Size of 4th buffer space.</param>
	/// <param name="s4">Size of 5th buffer space.</param>
	/// <param name="s5">Size of 6th buffer space.</param>
	/// <param name="s6">Size of 7th buffer space.</param>
	/// <param name="s7">Size of 8th buffer space.</param>
	/// <param name="s8">Size of 9th buffer space.</param>
	/// <param name="s9">Size of 10th buffer space.</param>
	/// <param name="s10">Size of 11th buffer space.</param>
	/// <param name="s11">Size of 12th buffer space.</param>
	/// <param name="s12">Size of 13th buffer space.</param>
	/// <param name="s13">Size of 14th buffer space.</param>
	/// <param name="helper">A <see cref="StaticCompositionHelper{T}"/> instance.</param>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	private protected static abstract void
		StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(UInt16 s0, UInt16 s1, UInt16 s2,
			UInt16 s3, UInt16 s4, UInt16 s5, UInt16 s6, UInt16 s7, UInt16 s8, UInt16 s9, UInt16 s10, UInt16 s11,
			UInt16 s12,
			UInt16 s13, StaticCompositionHelper<T> helper) where T0 : struct, IManagedBinaryBuffer<T0, T>
		where T1 : struct, IManagedBinaryBuffer<T1, T>
		where T2 : struct, IManagedBinaryBuffer<T2, T>
		where T3 : struct, IManagedBinaryBuffer<T3, T>
		where T4 : struct, IManagedBinaryBuffer<T4, T>
		where T5 : struct, IManagedBinaryBuffer<T5, T>
		where T6 : struct, IManagedBinaryBuffer<T6, T>
		where T7 : struct, IManagedBinaryBuffer<T7, T>
		where T8 : struct, IManagedBinaryBuffer<T8, T>
		where T9 : struct, IManagedBinaryBuffer<T9, T>
		where T10 : struct, IManagedBinaryBuffer<T10, T>
		where T11 : struct, IManagedBinaryBuffer<T11, T>
		where T12 : struct, IManagedBinaryBuffer<T12, T>
		where T13 : struct, IManagedBinaryBuffer<T13, T>;
#endif
}