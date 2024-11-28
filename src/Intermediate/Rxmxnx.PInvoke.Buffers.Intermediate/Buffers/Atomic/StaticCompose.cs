namespace Rxmxnx.PInvoke.Buffers;

#pragma warning disable CA2252
public partial struct Atomic<T>
{
	static void IManagedBuffer<T>.StaticCompose<T0>(StaticCompositionHelper<T> helper) { }
	static void IManagedBuffer<T>.StaticCompose<T0, T1>(UInt16 s0, UInt16 s1, StaticCompositionHelper<T> helper) { }
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2>(UInt16 s0, UInt16 s1, UInt16 s2,
		StaticCompositionHelper<T> helper) { }
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2, T3>(UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3,
		StaticCompositionHelper<T> helper) { }
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2, T3, T4>(UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3,
		UInt16 s4, StaticCompositionHelper<T> helper) { }
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2, T3, T4, T5>(UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3,
		UInt16 s4, UInt16 s5, StaticCompositionHelper<T> helper) { }
}
#pragma warning restore CA2252