#if NET5_0_OR_GREATER
using ThreadState = System.Threading.ThreadState;

namespace Rxmxnx.PInvoke.Tests.Internal;

[ExcludeFromCodeCoverage]
public sealed class GetNameTest
{
	[Fact]
	internal void JsonTypeTokenTest() => GetNameTest.Test<JsonTokenType>();
	[Fact]
	internal void MethodImplOptionsTest() => GetNameTest.Test<MethodImplOptions>();
	[Fact]
	internal void DayOfWeekTest() => GetNameTest.Test<DayOfWeek>();
	[Fact]
	internal void AttributeTargetsTest() => GetNameTest.Test<AttributeTargets>();
	[Fact]
	internal void ConsoleColorTest() => GetNameTest.Test<ConsoleColor>();
	[Fact]
	internal void FileAccessTest() => GetNameTest.Test<FileAccess>();
	[Fact]
	internal void FileShareTest() => GetNameTest.Test<FileShare>();
	[Fact]
	internal void HttpStatusCodeTest() => GetNameTest.Test<HttpStatusCode>();
	[Fact]
	internal void ThreadStateTest() => GetNameTest.Test<ThreadState>();
	[Fact]
	internal void BindingFlagsTest() => GetNameTest.Test<BindingFlags>();
	[Fact]
	internal void CallingConventionTest() => GetNameTest.Test<CallingConvention>();

	private static void Test<T>() where T : struct, Enum
	{
		foreach (T value in Enum.GetValues<T>())
			Assert.Equal(Enum.GetName(value), EnumCompat.GetName(value));
	}
}
#endif