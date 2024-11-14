using Rxmxnx.PInvoke.Buffers;

public static class Program
{
	public static void Main(String[] args)
	{
		AllocatedBuffer
			.Register<Composed<Composed<Primordial<Object>, Primordial<Object>, Object>, Composed<
				Composed<Composed<Primordial<Object>, Primordial<Object>, Object>,
					Composed<Primordial<Object>, Primordial<Object>, Object>, Object>,
				Composed<Composed<Primordial<Object>, Primordial<Object>, Object>,
					Composed<Primordial<Object>, Primordial<Object>, Object>, Object>, Object>, Object>>();
		Console.Write("Size: ");
		Int32 count = Int32.Parse(Console.ReadLine()!);
		AllocatedBuffer.Alloc<String?>((UInt16)count, Program.UseStringSpan);
	}

	private static void UseStringSpan(AllocatedBuffer<String?> buff)
	{
		Console.WriteLine("In stack: " + buff.InStack);
		for (Int32 i = 0; i < buff.Span.Length; i++)
			buff.Span[i] = $"Index: {i} Value: {Random.Shared.NextDouble()}";

		Program.PrintStringSpan(buff.Span);

		Console.WriteLine("Begin GC.Collect()");

		GC.Collect();
		GC.WaitForFullGCComplete();

		Console.WriteLine("End GC.Collect()");

		Program.PrintStringSpan(buff.Span);
	}
	private static void PrintStringSpan(ReadOnlySpan<String?> span)
	{
		foreach (ref readonly String? str in span)
			Console.WriteLine(str);
	}
}