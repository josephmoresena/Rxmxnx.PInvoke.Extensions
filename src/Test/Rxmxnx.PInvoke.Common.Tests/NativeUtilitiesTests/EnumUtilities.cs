#if NET461_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_1_OR_GREATER || WINDOWS_UWP
using System.Text.Json;
#endif
using ThreadState = System.Threading.ThreadState;
using JsonToken = Newtonsoft.Json.JsonToken;

namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class EnumUtilities
{
#if NET461_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_1_OR_GREATER || WINDOWS_UWP
	[Fact]
	public void JsonTokenTypeTest() => EnumUtilities.Test<JsonTokenType>();
#endif
	[Fact]
	public void JsonTokenTest() => EnumUtilities.Test<JsonToken>();
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