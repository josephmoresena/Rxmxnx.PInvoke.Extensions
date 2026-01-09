using MsBuildTask = Microsoft.Build.Utilities.Task;

namespace Rxmxnx.PInvoke.Patch.Tests;

/// <summary>
/// <c>TaskItem</c> for MSBuild patching.
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class TestAssemblyPatchTask : MsBuildTask
{
	/// <summary>
	/// Reader parameters for assembly.
	/// </summary>
	// ReSharper disable once MemberCanBePrivate.Global
	protected static readonly ReaderParameters ReadParameters = new() { ReadWrite = true, ReadSymbols = true, };

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
			assemblyPath = assemblyFiles
			               // ReSharper disable once HeapView.DelegateAllocation
			               .First(f => f.FullName.ToLowerInvariant().Contains(this.TargetFramework!.ToLowerInvariant()))
			               .FullName;
			this.AssemblyPatch(assemblyPath);
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
	// ReSharper disable once MemberCanBePrivate.Global
	protected void AssemblyPatch(String assemblyPath)
	{
		using AssemblyDefinition? assembly =
			AssemblyDefinition.ReadAssembly(assemblyPath, TestAssemblyPatchTask.ReadParameters);
		using ModuleDefinition? module = assembly.MainModule;
		WriterParameters writerParameters = new() { WriteSymbols = true, };

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