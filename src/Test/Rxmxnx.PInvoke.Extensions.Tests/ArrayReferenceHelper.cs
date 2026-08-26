#if !NET6_0_OR_GREATER
namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
internal static class ArrayReferenceHelper
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T GetArrayDataReference<T>(Array? array)
	{
		ReadOnlySpan<Int32> lb = array is not null ?
			ArrayCompat.GetLowerBounds(stackalloc Int32[array.Rank], array) :
			ReadOnlySpan<Int32>.Empty;
		// ReSharper disable once ConvertSwitchStatementToSwitchExpression
		switch (array?.Rank)
		{
			case 1: return ref new Span<T>(array as T[])[lb[0]];
			case 2: return ref (array as T[,])![lb[0], lb[1]];
			case 3: return ref (array as T[,,])![lb[0], lb[1], lb[2]];
			case 4: return ref (array as T[,,,])![lb[0], lb[1], lb[2], lb[3]];
			case 5: return ref (array as T[,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4]];
			case 6: return ref (array as T[,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5]];
			case 7: return ref (array as T[,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6]];
			case 8: return ref (array as T[,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7]];
			case 9: return ref (array as T[,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8]];
			case 10:
				return ref (array as T[,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8],
				                                    lb[9]];
			case 11:
				return ref (array as T[,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8],
				                                     lb[9], lb[10]];
			case 12:
				return ref (array as T[,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8],
				                                      lb[9], lb[10], lb[11]];
			case 13:
				return ref (array as T[,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8],
				                                       lb[9], lb[10], lb[11], lb[12]];
			case 14:
				return ref (array as T[,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8],
				                                        lb[9], lb[10], lb[11], lb[12], lb[13]];
			case 15:
				return ref (array as T[,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8],
				                                         lb[9], lb[10], lb[11], lb[12], lb[13], lb[14]];
			case 16:
				return ref (array as T[,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8],
				                                          lb[9], lb[10], lb[11], lb[12], lb[13], lb[14], lb[15]];
			case 17:
				return ref (array as T[,,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7],
				                                           lb[8], lb[9], lb[10], lb[11], lb[12], lb[13], lb[14], lb[15],
				                                           lb[16]];
			case 18:
				return ref (array as T[,,,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7],
				                                            lb[8], lb[9], lb[10], lb[11], lb[12], lb[13], lb[14],
				                                            lb[15], lb[16], lb[17]];
			case 19:
				return ref (array as T[,,,,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7],
				                                             lb[8], lb[9], lb[10], lb[11], lb[12], lb[13], lb[14],
				                                             lb[15], lb[16], lb[17], lb[18]];
			case 20:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7],
				                                              lb[8], lb[9], lb[10], lb[11], lb[12], lb[13], lb[14],
				                                              lb[15], lb[16], lb[17], lb[18], lb[19]];
			case 21:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7],
				                                               lb[8], lb[9], lb[10], lb[11], lb[12], lb[13], lb[14],
				                                               lb[15], lb[16], lb[17], lb[18], lb[19], lb[20]];
			case 22:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7],
				                                                lb[8], lb[9], lb[10], lb[11], lb[12], lb[13], lb[14],
				                                                lb[15], lb[16], lb[17], lb[18], lb[19], lb[20], lb[21]];
			case 23:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7],
				                                                 lb[8], lb[9], lb[10], lb[11], lb[12], lb[13], lb[14],
				                                                 lb[15], lb[16], lb[17], lb[18], lb[19], lb[20], lb[21],
				                                                 lb[22]];
			case 24:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6],
				                                                  lb[7], lb[8], lb[9], lb[10], lb[11], lb[12], lb[13],
				                                                  lb[14], lb[15], lb[16], lb[17], lb[18], lb[19],
				                                                  lb[20], lb[21], lb[22], lb[23]];
			case 25:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6],
				                                                   lb[7], lb[8], lb[9], lb[10], lb[11], lb[12], lb[13],
				                                                   lb[14], lb[15], lb[16], lb[17], lb[18], lb[19],
				                                                   lb[20], lb[21], lb[22], lb[23], lb[24]];
			case 26:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6],
				                                                    lb[7], lb[8], lb[9], lb[10], lb[11], lb[12], lb[13],
				                                                    lb[14], lb[15], lb[16], lb[17], lb[18], lb[19],
				                                                    lb[20], lb[21], lb[22], lb[23], lb[24], lb[25]];
			case 27:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6],
				                                                     lb[7], lb[8], lb[9], lb[10], lb[11], lb[12],
				                                                     lb[13], lb[14], lb[15], lb[16], lb[17], lb[18],
				                                                     lb[19], lb[20], lb[21], lb[22], lb[23], lb[24],
				                                                     lb[25], lb[26]];
			case 28:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6],
				                                                      lb[7], lb[8], lb[9], lb[10], lb[11], lb[12],
				                                                      lb[13], lb[14], lb[15], lb[16], lb[17], lb[18],
				                                                      lb[19], lb[20], lb[21], lb[22], lb[23], lb[24],
				                                                      lb[25], lb[26], lb[27]];
			case 29:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6],
				                                                       lb[7], lb[8], lb[9], lb[10], lb[11], lb[12],
				                                                       lb[13], lb[14], lb[15], lb[16], lb[17], lb[18],
				                                                       lb[19], lb[20], lb[21], lb[22], lb[23], lb[24],
				                                                       lb[25], lb[26], lb[27], lb[28]];
			case 30:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6],
				                                                        lb[7], lb[8], lb[9], lb[10], lb[11], lb[12],
				                                                        lb[13], lb[14], lb[15], lb[16], lb[17], lb[18],
				                                                        lb[19], lb[20], lb[21], lb[22], lb[23], lb[24],
				                                                        lb[25], lb[26], lb[27], lb[28], lb[29]];
			case 31:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
					lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8], lb[9], lb[10], lb[11], lb[12],
					lb[13], lb[14], lb[15], lb[16], lb[17], lb[18], lb[19], lb[20], lb[21], lb[22], lb[23], lb[24],
					lb[25], lb[26], lb[27], lb[28], lb[29], lb[30]];
			case 32:
				return ref (array as T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])![
					lb[0], lb[1], lb[2], lb[3], lb[4], lb[5], lb[6], lb[7], lb[8], lb[9], lb[10], lb[11], lb[12],
					lb[13], lb[14], lb[15], lb[16], lb[17], lb[18], lb[19], lb[20], lb[21], lb[22], lb[23], lb[24],
					lb[25], lb[26], lb[27], lb[28], lb[29], lb[30], lb[31]];
			default:
				return ref Unsafe.NullRef<T>();
		}
	}
}
#endif