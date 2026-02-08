namespace Rxmxnx.PInvoke.Mkbundle.Patcher;

public static class MakerPatcher
{
	[Flags]
	public enum Result
	{
		MonoLibNotFound = -1024,
		MakeBundleNotFound = -512,
		MsCoreLibNotFound = -256,
		MonoCecilError = -128,
		Vc14ClangTypeNotFound = -64,
		MkBundleTypeNotFound = -32,
		FindVcToolchainProgramMethodNotFound = -16,
		GenerateBundlesMethodNotFound = -8,
		AotCompileMethodNotFound = -4,
		ExecuteMethodNotFound = -2,
		FileError = -1,
		Done = 0,
		Unmodified = 1,
	}

	[UnmanagedCallersOnly(EntryPoint = "PatchAssemblyForWindows")]
	internal static unsafe Result PatchAssembly(Char* monoRuntimePath, Int32 monoRuntimePathLength, Char* outputPath,
		Int32 outputPathLength)
		=> MakerPatcher.PatchAssembly(new(monoRuntimePath, monoRuntimePathLength), new(outputPath, outputPathLength));

	private static Result PatchAssembly(ReadOnlySpan<Char> monoRuntimePath, ReadOnlySpan<Char> outputPath)
	{
		DirectoryInfo monoLibDirectory = new(monoRuntimePath.ToString());
		if (!monoLibDirectory.Exists) return Result.MonoLibNotFound;

		FileInfo mkbundleAssembly = new(Path.Combine(monoLibDirectory.FullName, "mkbundle.exe"));
		if (!mkbundleAssembly.Exists) return Result.MakeBundleNotFound;

		FileInfo mscorlibFile = new(Path.Combine(monoLibDirectory.FullName, "mscorlib.dll"));
		if (!mscorlibFile.Exists) return Result.MsCoreLibNotFound;

		DirectoryInfo outputDir = new(outputPath.ToString());
		FileInfo patchedAssembly = new(Path.Combine(outputDir.FullName, mkbundleAssembly.Name));
		try
		{
			if (!patchedAssembly.Exists || patchedAssembly.Length <= 0)
				mkbundleAssembly.CopyTo(patchedAssembly.FullName, true);
			return MakerPatcher.PatchIlCode(mkbundleAssembly, patchedAssembly.FullName);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			return Result.FileError;
		}
	}
	private static Result PatchIlCode(FileInfo sourceAssembly, String outputPath)
	{
		DefaultAssemblyResolver resolver = new();
		resolver.AddSearchDirectory(sourceAssembly.DirectoryName);

		ReaderParameters readerParameters =
			new() { ReadWrite = true, ReadSymbols = false, AssemblyResolver = resolver, };
		WriterParameters writerParameters = new() { WriteSymbols = false, };

		Boolean modified = false;
		try
		{
			Console.WriteLine($"Patching {sourceAssembly.FullName}...");
			using AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(outputPath, readerParameters);
			using ModuleDefinition? module = assembly.MainModule;
			TypeDefinition[] typesToPatch = module.Types.Where(t => t.Name is "MakeBundle" or "VC14Clang").ToArray();

			if (typesToPatch.FirstOrDefault(t => t.Name == "MakeBundle") is not { } mkbundleType)
				return Result.MkBundleTypeNotFound;

			MethodDefinition? aotCompileMethod = default;
			MethodDefinition? executeMethod = default;
			foreach (MethodDefinition? method in mkbundleType.Methods)
			{
				switch (method.Name)
				{
					case "AotCompile":
						aotCompileMethod = method;
						break;
					case "Execute":
						executeMethod = method;
						break;
				}
				if (aotCompileMethod is not null && executeMethod is not null)
					break;
			}

			switch (aotCompileMethod)
			{
				case null when executeMethod is null:
					return Result.AotCompileMethodNotFound | Result.ExecuteMethodNotFound;
				case null:
					return Result.AotCompileMethodNotFound;
				default:
					if (executeMethod is null)
						return Result.ExecuteMethodNotFound;
					break;
			}

			MethodDefinition? generateBundlesMethod = mkbundleType.NestedTypes
			                                                      .Where(nt => nt.Name.Contains("DisplayClass"))
			                                                      .SelectMany(nt => nt.Methods.Where(m => m.Name
				                                                                  .Contains("<GenerateBundles>")))
			                                                      .FirstOrDefault();

			if (generateBundlesMethod is null)
				return Result.GenerateBundlesMethodNotFound;

			if (typesToPatch.FirstOrDefault(t => t.Name == "VC14Clang") is not { } vc14ClangType)
				return Result.Vc14ClangTypeNotFound;

			MethodDefinition? findVcToolchainProgramMethod =
				vc14ClangType.Methods.FirstOrDefault(m => m.Name is "FindVCToolchainProgram");

			if (findVcToolchainProgramMethod is null)
				return Result.FindVcToolchainProgramMethodNotFound;

			MakerPatcher.PatchAotCompileMethod(aotCompileMethod, ref modified);
			MakerPatcher.PatchExecuteMethod(executeMethod, ref modified);
			MakerPatcher.PatchGenerateBundlesMethod(generateBundlesMethod, ref modified);
			MakerPatcher.PatchFindVcToolchainProgramMethod(findVcToolchainProgramMethod, ref modified);

			Console.WriteLine($"{sourceAssembly.FullName} -> {outputPath}");
			if (modified)
				assembly.Write(writerParameters);
			Console.WriteLine($"{outputPath}...Done.");
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			return modified ? Result.FileError : Result.MonoCecilError;
		}
		return modified ? Result.Done : Result.Unmodified;
	}
	private static void PatchAotCompileMethod(MethodDefinition aotCompileMethod, ref Boolean modified)
	{
		Span<Boolean> done = [false, false, false, false,];
		foreach (Instruction instr in aotCompileMethod.Body.Instructions.Where(i => i.OpCode == OpCodes.Ldstr))
		{
			String replace;
			switch (instr.Operand)
			{
				case "MONO_PATH={0} {1} --aot={2},outfile={3}{4}{5} {6}":
					replace = "SET MONO_PATH={0}&& \"{1}\" \"--aot={2},outfile={3}{4}{5}\" \"{6}\"";
					done[0] = true;
					break;
				case "MONO_PATH={0} {1} --aot={2},outfile={3}{4} {5}":
					replace = "SET MONO_PATH={0}&& \"{1}\" \"--aot={2},outfile={3}{4}\" \"{5}\"";
					done[1] = true;
					break;
				case "MONO_PATH={7} {0} --aot={1},outfile={2}{3}{4} {5} {6}":
					replace = "SET MONO_PATH={7}&& \"{0}\" \"--aot={1},outfile={2}{3}{4}\" \"{5}\" \"{6}\"";
					done[2] = true;
					break;
				case "{0} {1} {2}":
					replace = "\"{0}\" \"{1}\" \"{2}\"";
					done[3] = true;
					break;
				default:
					continue;
			}
			Console.WriteLine($"{instr.Operand} -> {replace}");
			instr.Operand = replace;
			modified = true;
			if (done.SequenceEqual([true, true, true, true,])) return;
		}
	}
	private static void PatchExecuteMethod(MethodDefinition executeMethod, ref Boolean modified)
	{
		foreach (Instruction instr in executeMethod.Body.Instructions.Where(i => i.OpCode == OpCodes.Ldstr))
		{
			switch (instr.Operand)
			{
				case "/c \"{0}\"":
				{
					instr.Operand = "/v /c \"{0}\"";
					modified = true;
					return;
				}
			}
		}
	}
	private static void PatchGenerateBundlesMethod(MethodDefinition generateBundlesMethod, ref Boolean modified)
	{
		MethodBody methodBody = generateBundlesMethod.Body;
		foreach (Instruction instr in generateBundlesMethod.Body.Instructions.Where(i => i.OpCode == OpCodes.Call))
		{
			if (instr.Operand is not MethodReference { Name: "LocateFile", })
				continue;

			ILProcessor processor = methodBody.GetILProcessor();
			processor.Remove(instr);
			modified = true;
			return;
		}
	}
	private static void PatchFindVcToolchainProgramMethod(MethodDefinition findVcToolchainProgramMethod,
		ref Boolean modified)
	{
		Span<Boolean> done = [false, false, false, false,];
		foreach (Instruction instr in
		         findVcToolchainProgramMethod.Body.Instructions.Where(i => i.OpCode == OpCodes.Ldstr))
		{
			String replace;
			switch (instr.Operand)
			{
				case "ClangC2":
					replace = "Tools";
					done[0] = true;
					break;
				case "bin":
					replace = "llvm";
					done[1] = true;
					break;
				case "amd64":
					replace = "x64\\bin";
					done[2] = true;
					break;
				case "x86":
					replace = "bin";
					done[3] = true;
					break;
				default:
					continue;
			}
			Console.WriteLine($"{instr.Operand} -> {replace}");
			instr.Operand = replace;
			modified = true;
			if (done.SequenceEqual([true, true, true, true,])) return;
		}
	}
}