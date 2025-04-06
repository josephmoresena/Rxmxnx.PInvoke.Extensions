namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
public sealed class AssemblyTest
{
	[Fact]
	internal void CommonTest() => AssemblyTest.InternalTest(typeof(NativeUtilities).Assembly);
	[Fact]
	internal void BuffersTest() => AssemblyTest.InternalTest(typeof(BufferManager).Assembly);
	[Fact]
	internal void CStringTest() => AssemblyTest.InternalTest(typeof(CString).Assembly);
	[Fact]
	internal void ExtensionTest() => AssemblyTest.InternalTest(typeof(MemoryBlockExtensions).Assembly);

	private static void InternalTest(Assembly asm)
	{
		const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
			BindingFlags.Static;
		Type[] types = asm.GetExportedTypes();
		Assert.DoesNotContain(types, t => t.FullName is { } name && name.Contains("Rxmxnx.PInvoke.Internal"));
#if PACKAGE
		HashSet<Type> attributeTypes = [];
		foreach (Type type in asm.GetTypes())
		{
			foreach (Attribute attr in type.GetCustomAttributes())
				attributeTypes.Add(attr.GetType());
			foreach (FieldInfo field in type.GetFields(flags))
			{
				foreach (Attribute attr in field.GetCustomAttributes())
					attributeTypes.Add(attr.GetType());
			}
			foreach (PropertyInfo prop in type.GetProperties(flags))
			{
				foreach (Attribute attr in prop.GetCustomAttributes())
					attributeTypes.Add(attr.GetType());
			}
			foreach (MethodInfo method in type.GetMethods(flags))
			{
				foreach (Attribute attr in method.GetCustomAttributes())
					attributeTypes.Add(attr.GetType());
			}
		}
		Assert.DoesNotContain(typeof(ExcludeFromCodeCoverageAttribute), attributeTypes);
		Assert.DoesNotContain(typeof(SuppressMessageAttribute), attributeTypes);
#endif
	}
}