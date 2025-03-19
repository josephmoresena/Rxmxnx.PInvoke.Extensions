using System.Diagnostics.CodeAnalysis;
using System.Xml;

using Mono.Cecil;

using MsBuildTask = Microsoft.Build.Utilities.Task;

namespace Rxmxnx.PInvoke.Extensions.Il.Patcher;

/// <summary>
/// TaskItem for MSBuild patching.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed partial class PatchAssemblyTask : MsBuildTask
{
	/// <summary>
	/// Reader parameters for assembly.
	/// </summary>
	private static readonly ReaderParameters readParameters = new() { ReadWrite = true, ReadSymbols = true, };

	/// <summary>
	/// MSBuild Output path.
	/// </summary>
	public String? OutputPath { get; set; }
	/// <summary>
	/// Target framework.
	/// </summary>
	public String? TargetFramework { get; set; }
	/// <summary>
	/// Strong-name Key path.
	/// </summary>
	public String? StrongNameKeyPath { get; set; }

	/// <inheritdoc/>
	public override Boolean Execute()
	{
		if (String.IsNullOrEmpty(this.OutputPath) || String.IsNullOrEmpty(this.TargetFramework)) return true;

		// Runs only if MSBuild sends both parameters.
		DirectoryInfo outputPath = new(this.OutputPath);
		FileInfo[] assemblyFiles = outputPath.GetFiles(PatchAssemblyTask.AssemblyName, SearchOption.AllDirectories);

		String? assemblyPath = default;
		String? snkPath = File.Exists(this.StrongNameKeyPath) ? new FileInfo(this.StrongNameKeyPath).FullName : default;
		try
		{
			assemblyPath = assemblyFiles
			               .First(f => f.FullName.Contains(this.TargetFramework,
			                                               StringComparison.InvariantCultureIgnoreCase)).FullName;
			String? documentationPath = outputPath
			                            .GetFiles(PatchAssemblyTask.AssemblyDocumentationName,
			                                      SearchOption.AllDirectories).FirstOrDefault()?.FullName;
			PatchAssemblyTask.PatchAssembly(assemblyPath, snkPath, documentationPath);
			return true;
		}
		catch (Exception ex)
		{
			this.Log.LogError(
				$"Error: {ex.Message} Output Path: {outputPath.FullName} Framework: {this.TargetFramework} SN-Key: {this.StrongNameKeyPath}  Count: {assemblyFiles.Length} Assembly: {assemblyPath} Snk:{snkPath}");
			this.Log.LogError(ex.ToString());
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

		PatchAssemblyTask.PatchAssembly(args[0], args.Length > 1 ? args[1] : default,
		                                args.Length > 2 ? args[2] : default);
	}

	/// <summary>
	/// Patches the <c>Rxmxnx.PInvoke.Extensions</c> assembly.
	/// </summary>
	/// <param name="assemblyPath">Path to <c>Rxmxnx.PInvoke.Extensions</c> assembly.</param>
	/// <param name="snkPath">Path to <c>Rxmxnx.PInvoke.Extensions</c> strong-name key.</param>
	/// <param name="documentationPath">Path to <c>Rxmxnx.PInvoke.Extensions</c> documentation.</param>
	private static void PatchAssembly(String assemblyPath, String? snkPath, String? documentationPath = default)
	{
		using AssemblyDefinition? assembly =
			AssemblyDefinition.ReadAssembly(assemblyPath, PatchAssemblyTask.readParameters);
		using ModuleDefinition? module = assembly.MainModule;
		WriterParameters writerParameters = !File.Exists(snkPath) ?
			new() { WriteSymbols = true, } :
			new() { WriteSymbols = true, StrongNameKeyBlob = File.ReadAllBytes(snkPath), };

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
		assembly.Write(writerParameters);

		if (String.IsNullOrEmpty(documentationPath)) return;

		XmlDocument xmlDocument = new() { PreserveWhitespace = true, };
		xmlDocument.Load(documentationPath);

		XmlNodeList membersNode = xmlDocument.GetElementsByTagName("members");

		if (membersNode is not [XmlElement membersElement,]) return;

		PatchAssemblyTask.DocumentGetUnsafeFixedContextMethod(membersElement);
		xmlDocument.Save(documentationPath);
	}
}