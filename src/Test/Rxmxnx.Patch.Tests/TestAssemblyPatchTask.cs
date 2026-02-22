using MsBuildTask = Microsoft.Build.Utilities.Task;

namespace Rxmxnx.PInvoke.Patch.Tests;

/// <summary>
/// <c>TaskItem</c> for MSBuild patching.
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class TestAssemblyPatchTask : MsBuildTask
{
	/// <summary>
	/// Test Assembly name.
	/// </summary>
	// ReSharper disable once UnusedAutoPropertyAccessor.Global
	public String? TestAssemblyName { get; set; }
	/// <summary>
	/// MSBuild Output path.
	/// </summary>
	// ReSharper disable once UnusedAutoPropertyAccessor.Global
	public String? OutputPath { get; set; }
	/// <summary>
	/// Target framework.
	/// </summary>
	// ReSharper disable once UnusedAutoPropertyAccessor.Global
	public String? TargetFramework { get; set; }

	/// <inheritdoc/>
	public override Boolean Execute()
	{
		if (String.IsNullOrEmpty(this.OutputPath) || String.IsNullOrEmpty(this.TargetFramework) ||
		    String.IsNullOrEmpty(this.TestAssemblyName)) return true;

		// Runs only if MSBuild sends all parameters.
		DirectoryInfo outputPath = new(this.OutputPath!);
		FileInfo[] assemblyFiles = outputPath.GetFiles(this.TestAssemblyName! + ".dll", SearchOption.AllDirectories);

		String? assemblyPath = default;
		try
		{
			FileInfo assembly = assemblyFiles
				// ReSharper disable once HeapView.DelegateAllocation
				.First(f => f.FullName.ToLowerInvariant().Contains(this.TargetFramework!.ToLowerInvariant()));
			String symbolsFileName = assembly.Name.Replace(assembly.Extension, ".pdb");
			Boolean debugSymbols = File.Exists(assembly.FullName.Replace(assembly.Name, symbolsFileName));
			assemblyPath = assembly.FullName;
			this.AssemblyPatch(assemblyPath, debugSymbols);
			return true;
		}
		catch (Exception ex)
		{
			this.Log.LogError(
				// ReSharper disable once HeapView.BoxingAllocation
				$"Error: {ex.Message} Output Path: {outputPath.FullName} Framework: {this.TargetFramework} Count: {assemblyFiles.Length} Assembly: {assemblyPath}");
			this.Log.LogError(ex.ToString());
			return false;
		}
	}

	/// <summary>
	/// Patches the test assembly.
	/// </summary>
	/// <param name="assemblyPath">Path to test assembly.</param>
	/// <param name="withDebugSymbols">Indicates whether the debug symbols of the test assembly ara available.</param>
	// ReSharper disable once MemberCanBePrivate.Global
	protected void AssemblyPatch(String assemblyPath, Boolean withDebugSymbols)
	{
		ReaderParameters readParameters = new() { ReadWrite = true, ReadSymbols = withDebugSymbols, };
		using AssemblyDefinition? assembly = AssemblyDefinition.ReadAssembly(assemblyPath, readParameters);
		using ModuleDefinition? module = assembly.MainModule;
		WriterParameters writerParameters = new() { WriteSymbols = readParameters.ReadSymbols, };

		if (this.IlPatch(module))
			assembly.Write(writerParameters);
	}

	/// <summary>
	/// Patches the IL code from loaded assembly.
	/// </summary>
	/// <param name="mainModule">Main module of loaded assembly.</param>
	/// <returns>
	/// <see langword="true"/> if <see cref="mainModule"/> was modified; otherwise; <see langword="false"/>.
	/// </returns>
	protected abstract Boolean IlPatch(ModuleDefinition mainModule);
}