using Mono.Cecil;

using MsBuildTask = Microsoft.Build.Utilities.Task;

namespace Rxmxnx.PInvoke.Extensions.Il.Patcher;

/// <summary>
/// TaskItem for MSBuild patching.
/// </summary>
public sealed partial class PatchAssemblyTask : MsBuildTask
{
	/// <summary>
	/// Reader parameters for assembly.
	/// </summary>
	private static readonly ReaderParameters readParameters = new() { ReadWrite = true, ReadSymbols = true, };
	/// <summary>
	/// Writer parameters for assembly.
	/// </summary>
	private static readonly WriterParameters writeParameters = new() { WriteSymbols = true, };

	/// <summary>
	/// MSBuild Output path.
	/// </summary>
	public String? OutputPath { get; set; }
	/// <summary>
	/// Target framework.
	/// </summary>
	public String? TargetFramework { get; set; }

	/// <inheritdoc/>
	public override Boolean Execute()
	{
		if (String.IsNullOrEmpty(this.OutputPath) || String.IsNullOrEmpty(this.TargetFramework)) return true;
		// Runs only if MSBuild sends both parameters.

		DirectoryInfo outputPath = new(this.OutputPath);
		FileInfo[] assemblyFiles = outputPath.GetFiles(PatchAssemblyTask.AssemblyName, SearchOption.AllDirectories);

		String? assemblyPath = default;
		try
		{
			assemblyPath = assemblyFiles
			               .First(f => f.FullName.Contains(this.TargetFramework,
			                                               StringComparison.InvariantCultureIgnoreCase)).FullName;
			PatchAssemblyTask.PatchAssembly(assemblyPath);
			return true;
		}
		catch (Exception ex)
		{
			this.Log.LogError(
				$"Error: {ex.Message} Output Path: {outputPath.FullName} Framework: {this.TargetFramework} Count: {assemblyFiles.Length} Assembly: {assemblyPath}");
			this.Log.LogError(ex.StackTrace);
			return false;
		}
	}

	/// <summary>
	/// Entry point.
	/// </summary>
	/// <param name="args">Executable args.</param>
	public static void Main(String[] args)
	{
		if (args.Length < 1)
			Console.Write("Please select the path of Rxmxnx.PInvoke.Extension assembly.");

		PatchAssemblyTask.PatchAssembly(args[0]);
	}

	/// <summary>
	/// Patches the <c>Rxmxnx.PInvoke.Extensions</c> assembly.
	/// </summary>
	/// <param name="assemblyPath">Path to <c>Rxmxnx.PInvoke.Extensions</c> assembly.</param>
	private static void PatchAssembly(String assemblyPath)
	{
		using AssemblyDefinition? assembly =
			AssemblyDefinition.ReadAssembly(assemblyPath, PatchAssemblyTask.readParameters);
		using ModuleDefinition? module = assembly.MainModule;

		TypeDefinition readOnlyValPtrTypeDefinition =
			module.Types.First(t => t.Name == PatchAssemblyTask.ReadOnlyValPtrName);
		TypeDefinition valPtrTypeDefinition = module.Types.First(t => t.Name == PatchAssemblyTask.ValPtrName);

		TypeDefinition? readOnlyFixedContext =
			module.Types.First(t => t.Name == PatchAssemblyTask.ReadOnlyFixedContextName);
		TypeDefinition? fixedContext = module.Types.First(t => t.Name == PatchAssemblyTask.FixedContextName);
		TypeDefinition? iReadOnlyFixedContext =
			module.Types.First(t => t.Name == PatchAssemblyTask.IReadOnlyFixedContextName);
		TypeDefinition? iFixedContext = module.Types.First(t => t.Name == PatchAssemblyTask.IFixedContextName);

		PatchAssemblyTask.ImplementGetUnsafeFixedContextMethod(module, readOnlyValPtrTypeDefinition,
		                                                       iReadOnlyFixedContext, readOnlyFixedContext);
		PatchAssemblyTask.ImplementGetUnsafeFixedContextMethod(module, valPtrTypeDefinition, iFixedContext,
		                                                       fixedContext);
		assembly.Write(PatchAssemblyTask.writeParameters);
	}
}