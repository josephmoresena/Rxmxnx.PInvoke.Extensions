using MsBuildTask = Microsoft.Build.Utilities.Task;

namespace Rxmxnx.PInvoke.Extensions.IlPatcher;

/// <summary>
/// <c>TaskItem</c> for MSBuild patching.
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class AssemblyPatchTask : MsBuildTask
{
	/// <summary>
	/// Package assembly name.
	/// </summary>
	public const String AssemblyName = "Rxmxnx.PInvoke.Extensions.dll";
	/// <summary>
	/// Package documentation name.
	/// </summary>
	public const String AssemblyDocumentationName = "Rxmxnx.PInvoke.Extensions.xml";

	/// <summary>
	/// Reader parameters for assembly.
	/// </summary>
	protected static readonly ReaderParameters ReadParameters = new() { ReadWrite = true, ReadSymbols = true, };

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
		DirectoryInfo outputPath = new(this.OutputPath!);
		FileInfo[] assemblyFiles = outputPath.GetFiles(AssemblyPatchTask.AssemblyName, SearchOption.AllDirectories);

		String? assemblyPath = default;
		String? snkPath = File.Exists(this.StrongNameKeyPath) ?
			new FileInfo(this.StrongNameKeyPath!).FullName :
			default;
		try
		{
			assemblyPath = assemblyFiles
			               .First(f => f.FullName.ToLowerInvariant().Contains(this.TargetFramework!.ToLowerInvariant()))
			               .FullName;
			String? documentationPath = outputPath
			                            .GetFiles(AssemblyPatchTask.AssemblyDocumentationName,
			                                      SearchOption.AllDirectories).FirstOrDefault()?.FullName;
			this.AssemblyPatch(assemblyPath, snkPath, documentationPath);
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
	/// Patches the <c>Rxmxnx.PInvoke.Extensions</c> assembly.
	/// </summary>
	/// <param name="assemblyPath">Path to <c>Rxmxnx.PInvoke.Extensions</c> assembly.</param>
	/// <param name="snkPath">Path to <c>Rxmxnx.PInvoke.Extensions</c> strong-name key.</param>
	/// <param name="documentationPath">Path to <c>Rxmxnx.PInvoke.Extensions</c> documentation.</param>
	protected void AssemblyPatch(String assemblyPath, String? snkPath, String? documentationPath)
	{
		using AssemblyDefinition? assembly =
			AssemblyDefinition.ReadAssembly(assemblyPath, AssemblyPatchTask.ReadParameters);
		using ModuleDefinition? module = assembly.MainModule;
		WriterParameters writerParameters = !File.Exists(snkPath) ?
			new() { WriteSymbols = true, } :
			new() { WriteSymbols = true, StrongNameKeyBlob = File.ReadAllBytes(snkPath!), };

		if (this.IlPatch(module))
			assembly.Write(writerParameters);

		if (String.IsNullOrEmpty(documentationPath)) return;

		XmlDocument xmlDocument = new() { PreserveWhitespace = true, };
		xmlDocument.Load(documentationPath);

		if (this.DocumentationPatch(xmlDocument))
			xmlDocument.Save(documentationPath);
	}

	/// <summary>
	/// Patches the IL code from loaded assembly.
	/// </summary>
	/// <param name="mainModule">Main module of loaded assembly.</param>
	/// <returns>
	/// <see langword="true"/> if <see cref="mainModule"/> was modified; otherwise; <see langword="false"/>.
	/// </returns>
	protected abstract Boolean IlPatch(ModuleDefinition mainModule);
	/// <summary>
	/// Patches the documentation from loaded assembly.
	/// </summary>
	/// <param name="xmlDocument">Xml documentation of loaded assembly.</param>
	/// <returns>
	/// <see langword="true"/> if <see cref="xmlDocument"/> was modified; otherwise; <see langword="false"/>.
	/// </returns>
	protected virtual Boolean DocumentationPatch(XmlDocument xmlDocument) => false;
}