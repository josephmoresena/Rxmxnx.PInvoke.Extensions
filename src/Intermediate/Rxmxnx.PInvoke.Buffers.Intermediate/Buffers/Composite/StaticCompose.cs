namespace Rxmxnx.PInvoke.Buffers;

#pragma warning disable CA2252
public partial struct Composite<TBufferA, TBufferB, T>
{
#if NET6_0_OR_GREATER && BINARY_SPACES
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	static void IManagedBinaryBuffer<Composite<TBufferA, TBufferB, T>, T>.StaticCompose<T0>(UInt16 s0,
		StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
		if (typeof(TBufferB) != typeof(Atomic<T>))
			TBufferB.StaticCompose<T0>(s0, helper);

		BufferTypeMetadata<T> m = Composite<TBufferB, T0, T>.TypeMetadata;

		if (!helper.Add(m)) return;
		if (typeof(TBufferB) == typeof(Atomic<T>)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		TBufferB.StaticCompose<T0, TBufferB>(s0, bSize, helper);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	static void IManagedBinaryBuffer<Composite<TBufferA, TBufferB, T>, T>.StaticCompose<T0, T1>(UInt16 s0, UInt16 s1,
		StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
		if (typeof(TBufferB) != typeof(Atomic<T>))
			TBufferB.StaticCompose<T0, T1>(s0, s1, helper);

		BufferTypeMetadata<T> m = IManagedBuffer<T>.GetMetadata<Composite<Composite<TBufferB, T1, T>, T0, T>>();

		if (!helper.Add(m)) return;
		if (typeof(TBufferB) == typeof(Atomic<T>)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		TBufferB.StaticCompose<T0, T1, TBufferB>(s0, s1, bSize, helper);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	static void IManagedBinaryBuffer<Composite<TBufferA, TBufferB, T>, T>.StaticCompose<T0, T1, T2>(UInt16 s0,
		UInt16 s1, UInt16 s2, StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
		if (typeof(TBufferB) != typeof(Atomic<T>))
			TBufferB.StaticCompose<T0, T1, T2>(s0, s1, s2, helper);

		BufferTypeMetadata<T> m = IManagedBuffer<T>
			.GetMetadata<Composite<Composite<Composite<TBufferB, T2, T>, T1, T>, T0, T>>();

		if (!helper.Add(m)) return;
		if (typeof(TBufferB) == typeof(Atomic<T>)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		TBufferB.StaticCompose<T0, T1, T2, TBufferB>(s0, s1, s2, bSize, helper);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	static void IManagedBinaryBuffer<Composite<TBufferA, TBufferB, T>, T>.StaticCompose<T0, T1, T2, T3>(UInt16 s0,
		UInt16 s1, UInt16 s2, UInt16 s3, StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
		if (typeof(TBufferB) != typeof(Atomic<T>))
			TBufferB.StaticCompose<T0, T1, T2, T3>(s0, s1, s2, s3, helper);

		BufferTypeMetadata<T> m = IManagedBuffer<T>
			.GetMetadata<Composite<Composite<Composite<Composite<TBufferB, T3, T>, T2, T>, T1, T>, T0, T>>();

		if (!helper.Add(m) || typeof(TBufferB) == typeof(Atomic<T>)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		TBufferB.StaticCompose<T0, T1, T2, T3, TBufferB>(s0, s1, s2, s3, bSize, helper);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	static void IManagedBinaryBuffer<Composite<TBufferA, TBufferB, T>, T>.StaticCompose<T0, T1, T2, T3, T4>(UInt16 s0,
		UInt16 s1, UInt16 s2, UInt16 s3, UInt16 s4, StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
		if (typeof(TBufferB) != typeof(Atomic<T>))
			TBufferB.StaticCompose<T0, T1, T2, T3, T4>(s0, s1, s2, s3, s4, helper);

		BufferTypeMetadata<T> m = IManagedBuffer<T>
			.GetMetadata<Composite<Composite<Composite<Composite<Composite<TBufferB, T4, T>, T3, T>, T2, T>, T1, T>, T0,
				T>>();

		if (!helper.Add(m)) return;
		if (typeof(TBufferB) == typeof(Atomic<T>)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		TBufferB.StaticCompose<T0, T1, T2, T3, T4, TBufferB>(s0, s1, s2, s3, s4, bSize, helper);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	static void IManagedBinaryBuffer<Composite<TBufferA, TBufferB, T>, T>.StaticCompose<T0, T1, T2, T3, T4, T5>(
		UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3, UInt16 s4, UInt16 s5, StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
		if (typeof(TBufferB) != typeof(Atomic<T>))
			TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5>(s0, s1, s2, s3, s4, s5, helper);

		BufferTypeMetadata<T> m = IManagedBuffer<T>
			.GetMetadata<
				Composite<Composite<Composite<Composite<Composite<Composite<TBufferB, T5, T>, T4, T>, T3, T>, T2, T>, T1
					, T>, T0, T>>();

		if (!helper.Add(m)) return;
		if (typeof(TBufferB) == typeof(Atomic<T>)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, TBufferB>(s0, s1, s2, s3, s4, s5, bSize, helper);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	static void IManagedBinaryBuffer<Composite<TBufferA, TBufferB, T>, T>.StaticCompose<T0, T1, T2, T3, T4, T5, T6>(
		UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3, UInt16 s4, UInt16 s5, UInt16 s6, StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
		if (typeof(TBufferB) != typeof(Atomic<T>))
			TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6>(s0, s1, s2, s3, s4, s5, s6, helper);

		BufferTypeMetadata<T> m = IManagedBuffer<T>
			.GetMetadata<
				Composite<Composite<
					Composite<Composite<Composite<Composite<Composite<TBufferB, T6, T>, T5, T>, T4, T>, T3, T>, T2, T>,
					T1, T>, T0, T>>();

		if (!helper.Add(m)) return;
		if (typeof(TBufferB) == typeof(Atomic<T>)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, TBufferB>(s0, s1, s2, s3, s4, s5, s6, bSize, helper);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	static void IManagedBinaryBuffer<Composite<TBufferA, TBufferB, T>, T>.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7>(
		UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3, UInt16 s4, UInt16 s5, UInt16 s6, UInt16 s7,
		StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
		if (typeof(TBufferB) != typeof(Atomic<T>))
			TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7>(s0, s1, s2, s3, s4, s5, s6, s7, helper);

		BufferTypeMetadata<T> m = IManagedBuffer<T>
			.GetMetadata<
				Composite<Composite<
					Composite<Composite<Composite<Composite<Composite<Composite<TBufferB, T7, T>, T6, T>, T5, T>, T4, T>
						, T3, T>, T2, T>, T1, T>, T0, T>>();

		if (!helper.Add(m)) return;
		if (typeof(TBufferB) == typeof(Atomic<T>)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, TBufferB>(s0, s1, s2, s3, s4, s5, s6, s7, bSize, helper);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	static void IManagedBinaryBuffer<Composite<TBufferA, TBufferB, T>, T>.
		StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8>(UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3, UInt16 s4,
			UInt16 s5,
			UInt16 s6, UInt16 s7, UInt16 s8, StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
		if (typeof(TBufferB) != typeof(Atomic<T>))
			TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8>(s0, s1, s2, s3, s4, s5, s6, s7, s8, helper);

		BufferTypeMetadata<T> m = IManagedBuffer<T>
			.GetMetadata<
				Composite<Composite<
					Composite<Composite<
						Composite<Composite<Composite<Composite<Composite<TBufferB, T8, T>, T7, T>, T6, T>, T5, T>, T4,
							T>, T3, T>, T2, T>, T1, T>, T0, T>>();

		if (!helper.Add(m)) return;
		if (typeof(TBufferB) == typeof(Atomic<T>)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, TBufferB>(
			s0, s1, s2, s3, s4, s5, s6, s7, s8, bSize, helper);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	static void IManagedBinaryBuffer<Composite<TBufferA, TBufferB, T>, T>.
		StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3, UInt16 s4,
			UInt16 s5, UInt16 s6, UInt16 s7, UInt16 s8, UInt16 s9, StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
		if (typeof(TBufferB) != typeof(Atomic<T>))
			TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
				s0, s1, s2, s3, s4, s5, s6, s7, s8, s9, helper);

		BufferTypeMetadata<T> m = IManagedBuffer<T>
			.GetMetadata<
				Composite<Composite<
					Composite<Composite<
						Composite<Composite<
								Composite<Composite<Composite<Composite<TBufferB, T9, T>, T8, T>, T7, T>, T6, T>, T5, T>
							, T4
							, T>, T3, T>, T2, T>, T1, T>, T0, T>>();

		if (!helper.Add(m)) return;
		if (typeof(TBufferB) == typeof(Atomic<T>)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TBufferB>(
			s0, s1, s2, s3, s4, s5, s6, s7, s8, s9, bSize, helper);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	static void IManagedBinaryBuffer<Composite<TBufferA, TBufferB, T>, T>.
		StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3,
			UInt16 s4,
			UInt16 s5, UInt16 s6, UInt16 s7, UInt16 s8, UInt16 s9, UInt16 s10, StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
		if (typeof(TBufferB) != typeof(Atomic<T>))
			TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
				s0, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, helper);

		BufferTypeMetadata<T> m = IManagedBuffer<T>
			.GetMetadata<
				Composite<Composite<
					Composite<Composite<
						Composite<Composite<
							Composite<Composite<Composite<Composite<Composite<TBufferB, T10, T>, T9, T>, T8, T>, T7, T>,
								T6, T>, T5, T>, T4, T>, T3, T>, T2, T>, T1, T>, T0, T>>();

		if (!helper.Add(m)) return;
		if (typeof(TBufferB) == typeof(Atomic<T>)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TBufferB>(
			s0, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, bSize, helper);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	static void IManagedBinaryBuffer<Composite<TBufferA, TBufferB, T>, T>.
		StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3,
			UInt16 s4, UInt16 s5, UInt16 s6, UInt16 s7, UInt16 s8, UInt16 s9, UInt16 s10, UInt16 s11,
			StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
		if (typeof(TBufferB) != typeof(Atomic<T>))
			TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
				s0, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, helper);

		BufferTypeMetadata<T> m = IManagedBuffer<T>
			.GetMetadata<Composite<
				Composite<Composite<
					Composite<Composite<
						Composite<Composite<
							Composite<Composite<Composite<Composite<Composite<TBufferB, T11, T>, T10, T>, T9, T>, T8, T>
								, T7, T>, T6, T>, T5, T>, T4, T>, T3, T>, T2, T>, T1, T>, T0, T>>();

		if (!helper.Add(m)) return;
		if (typeof(TBufferB) == typeof(Atomic<T>)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TBufferB>(
			s0, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, bSize, helper);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	static void IManagedBinaryBuffer<Composite<TBufferA, TBufferB, T>, T>.
		StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3,
			UInt16 s4, UInt16 s5, UInt16 s6, UInt16 s7, UInt16 s8, UInt16 s9, UInt16 s10, UInt16 s11, UInt16 s12,
			StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
		if (typeof(TBufferB) != typeof(Atomic<T>))
			TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
				s0, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, helper);

		BufferTypeMetadata<T> m = IManagedBuffer<T>
			.GetMetadata<Composite<Composite<
				Composite<Composite<
					Composite<Composite<
						Composite<Composite<
							Composite<Composite<Composite<Composite<Composite<TBufferB, T12, T>, T11, T>, T10, T>, T9,
								T>, T8, T>, T7, T>, T6, T>, T5, T>, T4, T>, T3, T>, T2, T>, T1, T>, T0, T>>();

		if (!helper.Add(m)) return;
		if (typeof(TBufferB) == typeof(Atomic<T>)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TBufferB>(
			s0, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, bSize, helper);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	static void IManagedBinaryBuffer<Composite<TBufferA, TBufferB, T>, T>.
		StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(UInt16 s0, UInt16 s1, UInt16 s2,
			UInt16 s3, UInt16 s4, UInt16 s5, UInt16 s6, UInt16 s7, UInt16 s8, UInt16 s9, UInt16 s10, UInt16 s11,
			UInt16 s12,
			UInt16 s13, StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
		if (typeof(TBufferB) != typeof(Atomic<T>))
			TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
				s0, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, s13, helper);

		BufferTypeMetadata<T> m = IManagedBuffer<T>
			.GetMetadata<Composite<Composite<Composite<
				Composite<Composite<
					Composite<Composite<
						Composite<Composite<
							Composite<Composite<Composite<Composite<Composite<TBufferB, T13, T>, T12, T>, T11, T>, T10,
								T>, T9, T>, T8, T>, T7, T>, T6, T>, T5, T>, T4, T>, T3, T>, T2, T>, T1, T>, T0, T>>();

		helper.Add(m);
		// Max space 2^15 - 1
	}
#endif
}
#pragma warning restore CA2252