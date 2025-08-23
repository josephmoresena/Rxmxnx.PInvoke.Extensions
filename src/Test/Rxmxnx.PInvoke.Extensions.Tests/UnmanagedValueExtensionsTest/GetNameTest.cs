#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.UnmanagedValueExtensionsTest;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class GetNameTest
{
	[Fact]
	public void MethodImplOptionsTest() => GetNameTest.Test<MethodImplOptions>();
	[Fact]
	public void DayOfWeekTest() => GetNameTest.Test<DayOfWeek>();
	[Fact]
	public void AttributeTargetsTest() => GetNameTest.Test<AttributeTargets>();
	[Fact]
	public void ConsoleColorTest() => GetNameTest.Test<ConsoleColor>();
	[Fact]
	public void FileAccessTest() => GetNameTest.Test<FileAccess>();
	[Fact]
	public void FileShareTest() => GetNameTest.Test<FileShare>();
	[Fact]
	public void ThreadStateTest() => GetNameTest.Test<ThreadState>();
	[Fact]
	public void BindingFlagsTest() => GetNameTest.Test<BindingFlags>();
	[Fact]
	public void CallingConventionTest() => GetNameTest.Test<CallingConvention>();

	private static void Test<T>() where T : struct, Enum
	{
#if NET5_0_OR_GREATER
		IReadOnlyList<T> values = Enum.GetValues<T>();
		IReadOnlyList<String> names = Enum.GetNames<T>();
#else
		Type typeofT = typeof(T);
		IReadOnlyList<T> values = (T[])Enum.GetValues(typeofT);
		IReadOnlyList<String> names = Enum.GetNames(typeofT);
#endif
		using IEnumerator<T> valuesEnumerator = values.GetEnumerator();
		using IEnumerator<String> namesEnumerator = names.GetEnumerator();
		while (valuesEnumerator.MoveNext() && namesEnumerator.MoveNext())
		{
			String? valueName = valuesEnumerator.Current.GetName();
			PInvokeAssert.Equal(namesEnumerator.Current, valueName);
#if !NET5_0_OR_GREATER
			PInvokeAssert.Equal(Enum.GetName(typeofT, valuesEnumerator.Current), valueName);
#else
			PInvokeAssert.Equal(Enum.GetName(valuesEnumerator.Current), valueName);
#endif
		}
	}
}