using ThreadState = System.Threading.ThreadState;

namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class EnumUtilities
{
	[Fact]
	public void JsonTypeTokenTest() => EnumUtilities.Test<JsonTokenType>();
	[Fact]
	public void MethodImplOptionsTest() => EnumUtilities.Test<MethodImplOptions>();
	[Fact]
	public void DayOfWeekTest() => EnumUtilities.Test<DayOfWeek>();
	[Fact]
	public void AttributeTargetsTest() => EnumUtilities.Test<AttributeTargets>();
	[Fact]
	public void ConsoleColorTest() => EnumUtilities.Test<ConsoleColor>();
	[Fact]
	public void FileAccessTest() => EnumUtilities.Test<FileAccess>();
	[Fact]
	public void FileShareTest() => EnumUtilities.Test<FileShare>();
	[Fact]
	public void HttpStatusCodeTest() => EnumUtilities.Test<HttpStatusCode>();
	[Fact]
	public void ThreadStateTest() => EnumUtilities.Test<ThreadState>();
	[Fact]
	public void BindingFlagsTest() => EnumUtilities.Test<BindingFlags>();
	[Fact]
	public void CallingConventionTest() => EnumUtilities.Test<CallingConvention>();

	private static void Test<T>() where T : struct, Enum
	{
#if NET5_0_OR_GREATER
		T[] values = Enum.GetValues<T>();
		String[] names = Enum.GetNames<T>();
#else
		Type typeofT = typeof(T);
		T[] values = (T[])Enum.GetValues(typeofT);
		String[] names = Enum.GetNames(typeofT);
#endif
		PInvokeAssert.Equal(values, NativeUtilities.GetEnumValuesSpan<T>().ToArray());
		PInvokeAssert.Equal(names, NativeUtilities.GetEnumNamesSpan<T>().ToArray());
	}
}