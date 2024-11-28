namespace Rxmxnx.PInvoke.Buffers;

#pragma warning disable CA2252
public partial struct Composite<TBufferA, TBufferB, T>
{
	static void IManagedBuffer<T>.StaticCompose<T0>(StaticCompositionHelper<T> helper)
	{
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
		BufferTypeMetadata<T> b10Metadata =
			IManagedBuffer<T>.GetMetadata<Composite<Composite<TBufferB, T1, T>, T0, T>>();
		Boolean next = helper.Add(b10Metadata);
		if (!next || typeof(TBufferB) == typeof(Atomic<T>)) return;
		UInt16 bSize = TBufferB.TypeMetadata.Size;
		if (helper.IsCompositionRequired(s0, bSize))
			TBufferB.StaticCompose<T0>(helper);
		if (s0 <= s1 + bSize + bSize / 2) return;
		TBufferB.StaticCompose<T0, T1>(s0, s1, helper);
		TBufferB.StaticCompose<T0, T1, TBufferB>(s0, s1, bSize, helper);
	}
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2>(UInt16 s0, UInt16 s1, UInt16 s2,
		StaticCompositionHelper<T> helper)
	{
		BufferTypeMetadata<T> b210Metadata = IManagedBuffer<T>
			.GetMetadata<Composite<Composite<Composite<TBufferB, T2, T>, T1, T>, T0, T>>();
		BufferTypeMetadata<T> b10Metadata =
			IManagedBuffer<T>.GetMetadata<Composite<Composite<TBufferB, T1, T>, T0, T>>();
		Boolean next = helper.Add(b210Metadata) || helper.Add(b10Metadata);
		if (!next || typeof(TBufferB) == typeof(Atomic<T>)) return;
		UInt16 bSize = TBufferB.TypeMetadata.Size;
		if (s0 <= s1 + s2 + bSize + bSize / 2) return;
		TBufferB.StaticCompose<T0, T1>(s0, s1, helper);
		TBufferB.StaticCompose<T0, T1, T2>(s0, s1, s2, helper);
		TBufferB.StaticCompose<T0, T1, T2, TBufferB>(s0, s1, s2, bSize, helper);
	}
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2, T3>(UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3,
		StaticCompositionHelper<T> helper)
	{
		BufferTypeMetadata<T> b3210Metadata = IManagedBuffer<T>
			.GetMetadata<Composite<Composite<Composite<Composite<TBufferB, T3, T>, T2, T>, T1, T>, T0, T>>();
		BufferTypeMetadata<T> b210Metadata = IManagedBuffer<T>
			.GetMetadata<Composite<Composite<Composite<TBufferB, T2, T>, T1, T>, T0, T>>();
		BufferTypeMetadata<T> b10Metadata =
			IManagedBuffer<T>.GetMetadata<Composite<Composite<TBufferB, T1, T>, T0, T>>();
		BufferTypeMetadata<T> _210Metadata = IManagedBuffer<T>.GetMetadata<Composite<Composite<T2, T1, T>, T0, T>>();
		Boolean next = helper.Add(b3210Metadata) || helper.Add(b210Metadata) || helper.Add(b10Metadata) ||
			helper.Add(_210Metadata);
		if (!next && typeof(TBufferB) == typeof(Atomic<T>)) return;
		//TODO
	}
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2, T3, T4>(UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3,
		UInt16 s4, StaticCompositionHelper<T> helper) { }
	static void IManagedBuffer<T>.StaticCompose<T0, T1, T2, T3, T4, T5>(UInt16 s0, UInt16 s1, UInt16 s2, UInt16 s3,
		UInt16 s4, UInt16 s5, StaticCompositionHelper<T> helper) { }
}
#pragma warning restore CA2252