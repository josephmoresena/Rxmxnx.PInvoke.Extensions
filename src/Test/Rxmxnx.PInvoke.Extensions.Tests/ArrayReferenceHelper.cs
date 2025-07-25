namespace Rxmxnx.PInvoke.Tests;

internal static class ArrayReferenceHelper
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference<T>(Array? array)
	{
		switch (array?.Rank)
		{
			case 1: return ref new Span<T>(array as T[])[0];
			case 2: return ref (array as T[,])![0, 0];
			case 3: return ref (array as T[,,])![0, 0, 0];
			case 4: return ref (array as T[,,,])![0, 0, 0, 0];
			case 5: return ref (array as T[,,,,])![0, 0, 0, 0, 0];
			case 6: return ref (array as T[,,,,,])![0, 0, 0, 0, 0, 0];
			case 7: return ref (array as T[,,,,,,])![0, 0, 0, 0, 0, 0, 0];
			case 8: return ref (array as T[,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0];
			case 9: return ref (array as T[,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0];
			case 10: return ref (array as T[,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
			case 11: return ref (array as T[,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
			case 12: return ref (array as T[,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
			case 13: return ref (array as T[,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
			case 14: return ref (array as T[,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
			case 15: return ref (array as T[,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
			case 16: return ref (array as T[,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
			case 17: return ref (array as T[,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
			case 18: return ref (array as T[,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
			case 19:
				return ref (array as T[,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
			case 20:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				                                              0];
			case 21:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				                                               0, 0];
			case 22:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				                                                0, 0, 0];
			case 23:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				                                                 0, 0, 0, 0, 0];
			case 24:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				                                                  0, 0, 0, 0, 0, 0];
			case 25:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				                                                   0, 0, 0, 0, 0, 0, 0];
			case 26:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				                                                    0, 0, 0, 0, 0, 0, 0, 0, 0];
			case 27:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				                                                     0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
			case 28:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				                                                      0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
			case 29:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				                                                       0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
			case 30:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				                                                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
			case 31:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
			case 32:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
			default:
				return ref Unsafe.NullRef<T>();
		}
	}
}