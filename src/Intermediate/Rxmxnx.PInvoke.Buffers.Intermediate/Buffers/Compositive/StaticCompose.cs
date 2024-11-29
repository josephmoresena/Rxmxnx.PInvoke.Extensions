namespace Rxmxnx.PInvoke.Buffers;

#pragma warning disable CA2252
public partial struct Composite<TBufferA, TBufferB, T>
{
	static void IManagedBuffer<T>.StaticCompose<T0>(StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;

		BufferTypeMetadata<T> btMetadata = Composite<TBufferB, T0, T>.typeMetadata;
		BufferTypeMetadata<T> tMetadata = T0.TypeMetadata;
		BufferTypeMetadata<T> bMetadata = TBufferB.TypeMetadata;

		if (typeof(TBufferB) == typeof(Atomic<T>))
		{
			Boolean t = helper.Add(tMetadata);
		}
		else
		{
			if (helper.IsCompositionRequired(bMetadata.Size, bMetadata.Size))
				TBufferB.StaticCompose<TBufferB>(helper);
			if (helper.IsCompositionRequired(bMetadata.Size, tMetadata.Size) &&
			    tMetadata.IsBinaryAppendableTo(bMetadata))
				TBufferB.StaticCompose<T0>(helper);
			if (btMetadata.IsBinaryAppendableTo(bMetadata))
				TBufferB.StaticCompose<Composite<TBufferB, T0, T>>(helper);

			if (tMetadata.IsPure && tMetadata.Size > bMetadata.Size + bMetadata.Size / 2)
				TBufferB.StaticCompose<T0, TBufferB>(tMetadata.Size, bMetadata.Size, helper);
		}
		Boolean bt = helper.Add(btMetadata);
	}
	static void IManagedBuffer<T>.StaticCompose<T0, T1>(UInt16 s0, UInt16 s1, StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		BufferTypeMetadata<T> _10Metadata =
			IManagedBuffer<T>.GetMetadata<Composite<Composite<TBufferB, T1, T>, T0, T>>();
		Boolean next = helper.Add(_10Metadata);

		if (!next || typeof(TBufferB) == typeof(Atomic<T>)) return;

		if (helper.IsCompositionRequired(s0, bSize))
			TBufferB.StaticCompose<T0>(helper);

		if (s0 <= s1 + bSize + bSize / 2) return;

		TBufferB.StaticCompose<T0, T1>(s0, s1, helper);
		TBufferB.StaticCompose<T0, T1, TBufferB>(s0, s1, bSize, helper);
	}
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2>(UInt16 s0, UInt16 s1, UInt16 s2,
		StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		BufferTypeMetadata<T> _210Metadata = IManagedBuffer<T>
			.GetMetadata<Composite<Composite<Composite<TBufferB, T2, T>, T1, T>, T0, T>>();
		Boolean next = helper.Add(_210Metadata);

		if (!next || typeof(TBufferB) == typeof(Atomic<T>) || s0 <= s1 + s2 + bSize + bSize / 2) return;

		TBufferB.StaticCompose<T0, T1>(s0, s1, helper);
		TBufferB.StaticCompose<T0, T1, T2>(s0, s1, s2, helper);
		TBufferB.StaticCompose<T0, T1, T2, TBufferB>(s0, s1, s2, bSize, helper);
	}
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2, T3>(UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3,
		StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		BufferTypeMetadata<T> _3210Metadata = IManagedBuffer<T>
			.GetMetadata<Composite<Composite<Composite<Composite<TBufferB, T3, T>, T2, T>, T1, T>, T0, T>>();
		Boolean next = helper.Add(_3210Metadata);

		if (!next || typeof(TBufferB) == typeof(Atomic<T>) || s0 <= s1 + s2 + s3 + bSize + bSize / 2) return;

		TBufferB.StaticCompose<T0, T1, T2>(s0, s1, s2, helper);
		TBufferB.StaticCompose<T0, T1, T2, T3>(s0, s1, s2, s3, helper);
		TBufferB.StaticCompose<T0, T1, T2, T3, TBufferB>(s0, s1, s2, s3, bSize, helper);
	}
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2, T3, T4>(UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3,
		UInt16 s4, StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		BufferTypeMetadata<T> _43210Metadata = IManagedBuffer<T>
			.GetMetadata<Composite<Composite<Composite<Composite<Composite<TBufferB, T4, T>, T3, T>, T2, T>, T1, T>, T0,
				T>>();
		Boolean next = helper.Add(_43210Metadata);

		if (!next || typeof(TBufferB) == typeof(Atomic<T>) || s0 <= s1 + s2 + s3 + s4 + bSize + bSize / 2) return;

		TBufferB.StaticCompose<T0, T1, T2, T3>(s0, s1, s2, s3, helper);
		TBufferB.StaticCompose<T0, T1, T2, T3, T4>(s0, s1, s2, s3, s4, helper);
		TBufferB.StaticCompose<T0, T1, T2, T3, T4, TBufferB>(s0, s1, s2, s3, s4, bSize, helper);
	}
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2, T3, T4, T5>(UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3,
		UInt16 s4, UInt16 s5, StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		BufferTypeMetadata<T> _543210Metadata = IManagedBuffer<T>
			.GetMetadata<
				Composite<Composite<Composite<Composite<Composite<Composite<TBufferB, T5, T>, T4, T>, T3, T>, T2, T>, T1
					, T>, T0, T>>();
		Boolean next = helper.Add(_543210Metadata);

		if (!next || typeof(TBufferB) == typeof(Atomic<T>) || s0 <= s1 + s2 + s3 + s4 + s5 + bSize + bSize / 2) return;

		TBufferB.StaticCompose<T0, T1, T2, T3, T4>(s0, s1, s2, s3, s4, helper);
		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5>(s0, s1, s2, s3, s4, s5, helper);
		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, TBufferB>(s0, s1, s2, s3, s4, s5, bSize, helper);
	}
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2, T3, T4, T5, T6>(UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3,
		UInt16 s4, UInt16 s5, UInt16 s6, StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		BufferTypeMetadata<T> _6543210Metadata = IManagedBuffer<T>
			.GetMetadata<
				Composite<Composite<
					Composite<Composite<Composite<Composite<Composite<TBufferB, T6, T>, T5, T>, T4, T>, T3, T>, T2, T>,
					T1, T>, T0, T>>();
		Boolean next = helper.Add(_6543210Metadata);

		if (!next || typeof(TBufferB) == typeof(Atomic<T>) ||
		    s0 <= s1 + s2 + s3 + s4 + s5 + s6 + bSize + bSize / 2) return;

		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5>(s0, s1, s2, s3, s4, s5, helper);
		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6>(s0, s1, s2, s3, s4, s5, s6, helper);
		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, TBufferB>(s0, s1, s2, s3, s4, s5, s6, bSize, helper);
	}
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7>(UInt16 s0, UInt16 s1, UInt16 s2,
		UInt16 s3, UInt16 s4, UInt16 s5, UInt16 s6, UInt16 s7, StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		BufferTypeMetadata<T> _76543210Metadata = IManagedBuffer<T>
			.GetMetadata<
				Composite<Composite<
					Composite<Composite<Composite<Composite<Composite<Composite<TBufferB, T7, T>, T6, T>, T5, T>, T4, T>
						, T3, T>, T2, T>, T1, T>, T0, T>>();
		Boolean next = helper.Add(_76543210Metadata);

		if (!next || typeof(TBufferB) == typeof(Atomic<T>) ||
		    s0 <= s1 + s2 + s3 + s4 + s5 + s6 + s7 + bSize + bSize / 2) return;

		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6>(s0, s1, s2, s3, s4, s5, s6, helper);
		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7>(s0, s1, s2, s3, s4, s5, s6, s7, helper);
		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, TBufferB>(s0, s1, s2, s3, s4, s5, s6, s7, bSize, helper);
	}
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8>(UInt16 s0, UInt16 s1, UInt16 s2,
		UInt16 s3, UInt16 s4, UInt16 s5, UInt16 s6, UInt16 s7, UInt16 s8, StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;

		UInt16 bSize = TBufferB.TypeMetadata.Size;
		BufferTypeMetadata<T> _876543210Metadata = IManagedBuffer<T>
			.GetMetadata<
				Composite<Composite<
					Composite<Composite<
						Composite<Composite<Composite<Composite<Composite<TBufferB, T8, T>, T7, T>, T6, T>, T5, T>, T4,
							T>, T3, T>, T2, T>, T1, T>, T0, T>>();
		Boolean next = helper.Add(_876543210Metadata);

		if (!next || typeof(TBufferB) == typeof(Atomic<T>) ||
		    s0 <= s1 + s2 + s3 + s4 + s5 + s6 + s7 + s8 + bSize + bSize / 2) return;

		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7>(s0, s1, s2, s3, s4, s5, s6, s7, helper);
		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8>(s0, s1, s2, s3, s4, s5, s6, s7, s8, helper);
		TBufferB.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, TBufferB>(
			s0, s1, s2, s3, s4, s5, s6, s7, s8, bSize, helper);
	}
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(UInt16 s0, UInt16 s1, UInt16 s2,
		UInt16 s3, UInt16 s4, UInt16 s5, UInt16 s6, UInt16 s7, UInt16 s8, UInt16 s9, StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
	}
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(UInt16 s0, UInt16 s1,
		UInt16 s2, UInt16 s3, UInt16 s4, UInt16 s5, UInt16 s6, UInt16 s7, UInt16 s8, UInt16 s9, UInt16 s10,
		StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
	}
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(UInt16 s0, UInt16 s1,
		UInt16 s2, UInt16 s3, UInt16 s4, UInt16 s5, UInt16 s6, UInt16 s7, UInt16 s8, UInt16 s9, UInt16 s10, UInt16 s11,
		StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
	}
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(UInt16 s0,
		UInt16 s1, UInt16 s2, UInt16 s3, UInt16 s4, UInt16 s5, UInt16 s6, UInt16 s7, UInt16 s8, UInt16 s9, UInt16 s10,
		UInt16 s11, UInt16 s12, StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
	}
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(UInt16 s0,
		UInt16 s1, UInt16 s2, UInt16 s3, UInt16 s4, UInt16 s5, UInt16 s6, UInt16 s7, UInt16 s8, UInt16 s9, UInt16 s10,
		UInt16 s11, UInt16 s12, UInt16 s13, StaticCompositionHelper<T> helper)
	{
		if (typeof(TBufferB) != typeof(TBufferA)) return;
	}
}
#pragma warning restore CA2252