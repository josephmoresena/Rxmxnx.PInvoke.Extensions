namespace Rxmxnx.PInvoke.ApplicationTest;

public interface ICppCompiler
{
	IEnumerable<String> LibraryPaths { get; }
	String CompilerExecutable { get; }
	String DynamicRuntime { get; }
	String IncludeFlag { get; }
	String OutputFlag { get; }
	String StaticLibPathFlag { get; }
	IEnumerable<String> DefaultLink { get; }
	String EnableAllWarnings { get; }
	String RemovePointerWarnings { get; }
	String BeginWholeLink { get; }
	String EndWholeLink { get; }
	String ExportDynamicSymbols { get; }
	String RuntimePath { get; }

	IEnumerable<String> BeginLink(Boolean windowApp);
	async Task<(String name, String value)[]> GetEnv()
	{
		await Task.Yield();
		return [];
	}
}