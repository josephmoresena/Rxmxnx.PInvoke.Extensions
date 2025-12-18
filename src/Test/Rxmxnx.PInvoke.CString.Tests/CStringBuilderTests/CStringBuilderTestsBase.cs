namespace Rxmxnx.PInvoke.Tests.CStringBuilderTests;

public abstract class CStringBuilderTestsBase
{
	protected static (Int32 utf16Index, Int32 utf8Index) GetIndex(ReadOnlySpan<Char> value, out Int32 nRune)
	{
		List<Int32> safeUtf16Indices = [];
		Int32 currentUtf16Index = 0;

		nRune = 0;
		while (currentUtf16Index < value.Length)
		{
			if (RuneCompat.DecodeFromUtf16(value[currentUtf16Index..], out _, out Int32 charsConsumed) ==
			    OperationStatus.Done)
			{
				if (currentUtf16Index <= value.Length)
					safeUtf16Indices.Add(currentUtf16Index);
				currentUtf16Index += charsConsumed;
				nRune++;
			}
			else
			{
				break;
			}
		}
		if (safeUtf16Indices.Count == 0) return (0, 0);

		Int32 randomIndexInList = Random.Shared.Next(0, safeUtf16Indices.Count);
		Int32 utf16Index = safeUtf16Indices[randomIndexInList];
		Int32 utf8Index = Encoding.UTF8.GetByteCount(value[..utf16Index]);

		return (utf16Index, utf8Index);
	}

#if !NET6_0_OR_GREATER
	protected static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}