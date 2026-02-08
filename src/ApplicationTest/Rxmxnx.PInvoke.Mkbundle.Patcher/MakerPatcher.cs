namespace Rxmxnx.PInvoke.Mkbundle.Patcher;

public static class MakerPatcher
{
	[Flags]
	public enum Result
	{
		MonoLibNotFound = -8192,
		MakeBundleNotFound = -4096,
		MsCoreLibNotFound = -2048,
		MonoCecilError = -1024,
		Vc15ClangTypeNotFound = -512,
		Vc14ClangTypeNotFound = -256,
		Vc15ToolchainProgramTypeNotFound = -128,
		MkBundleTypeNotFound = -64,
		FindVcToolchainProgramMethodNotFound = -32,
		IsVisualStudio15NotFound = -16,
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

			if (module.Types.FirstOrDefault(t => t.Name == "MakeBundle") is not { } mkbundleType)
				return Result.MkBundleTypeNotFound;

			Dictionary<String, TypeDefinition> nestedTypes = mkbundleType.NestedTypes
			                                                             .Where(MakerPatcher.IsRequiredNestedType)
			                                                             .ToDictionary(
				                                                             MakerPatcher.GetNestedTypeName, t => t);

			if (nestedTypes.GetValueOrDefault("VC15ToolchainProgram") is not { } vc15ToolchainProgramType)
				return Result.Vc15ToolchainProgramTypeNotFound;
			if (nestedTypes.GetValueOrDefault("VC14Clang") is not { } vc14ClangType)
				return Result.Vc14ClangTypeNotFound;
			if (nestedTypes.GetValueOrDefault("VC15Clang") is not { } vc15ClangType)
				return Result.Vc15ClangTypeNotFound;

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
			MethodDefinition? generateBundlesMethod = MakerPatcher.GetMethod(nestedTypes, "GenerateBundles");
			MethodDefinition? isVisualStudio15Method =
				MakerPatcher.GetMethod(nestedTypes, "VisualStudioSDKHelper", "IsVisualStudio15");

			MethodDefinition? findVcToolchainProgramMethod =
				vc14ClangType.Methods.FirstOrDefault(m => m.Name is "FindVCToolchainProgram");

			Result error = Result.Done;
			if (aotCompileMethod is null)
				error |= Result.AotCompileMethodNotFound;
			if (executeMethod is null)
				error |= Result.ExecuteMethodNotFound;
			if (generateBundlesMethod is null)
				error |= Result.GenerateBundlesMethodNotFound;
			if (isVisualStudio15Method is null)
				error |= Result.IsVisualStudio15NotFound;
			if (findVcToolchainProgramMethod is null)
				error |= Result.FindVcToolchainProgramMethodNotFound;
			if (error is not Result.Done)
				return error;

			MakerPatcher.PatchAotCompileMethod(aotCompileMethod!, ref modified);
			MakerPatcher.PatchExecuteMethod(executeMethod!, ref modified);
			MakerPatcher.PatchGenerateBundlesMethod(generateBundlesMethod!, ref modified);
			MakerPatcher.PatchIsVersion15Method(isVisualStudio15Method!, ref modified);
			MakerPatcher.PatchFindVcToolchainProgramMethod(findVcToolchainProgramMethod!, ref modified);
			MakerPatcher.ChangeBaseType(vc15ClangType, vc15ToolchainProgramType.BaseType, ref modified);
			MakerPatcher.ChangeBaseType(vc14ClangType, vc15ToolchainProgramType, ref modified);

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
	private static Boolean IsRequiredNestedType(TypeDefinition nestedType)
	{
		if (nestedType.Name.StartsWith("VC1")) //VC14 or VC15
			return true;
		if (nestedType.Name.Contains("DisplayClass")) // Class for GenerateBundles action.
			return nestedType.Methods.Any(m => m.Name.StartsWith("<GenerateBundles>"));
		return nestedType.Name is "VCToolChainProgram" or "VisualStudioSDKHelper";
	}
	private static String GetNestedTypeName(TypeDefinition nestedType)
		=> nestedType.Name.Contains("DisplayClass") ? "GenerateBundles" : nestedType.Name;
	private static MethodDefinition? GetMethod(Dictionary<String, TypeDefinition> nestedTypes, String className,
		String? methodName = default)
		=> nestedTypes.GetValueOrDefault(className)?.Methods
		              .FirstOrDefault(m => !String.IsNullOrEmpty(methodName) ?
			                              m.Name == methodName :
			                              m.Name.StartsWith($"<{className}>"));
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
			String? oldOperand = instr.Operand as String;
			switch (oldOperand)
			{
				case "/c \"{0}\"":
				{
					const String replace = "/v /c \"{0}\"";

					Console.WriteLine($"{oldOperand} -> {replace}");
					instr.Operand = replace;
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

			Console.WriteLine($"Remove: {instr}");
			processor.Remove(instr);
			modified = true;
			return;
		}
	}
	private static void PatchIsVersion15Method(MethodDefinition isVersion15Method, ref Boolean modified)
	{
		Instruction? instr = isVersion15Method.Body.Instructions.FirstOrDefault(i => i.OpCode == OpCodes.Beq_S);
		if (instr is null) return;

		instr.OpCode = OpCodes.Bge_S;
		Console.WriteLine($"{OpCodes.Beq_S} -> {OpCodes.Bge_S}");
		modified = true;
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
	private static void ChangeBaseType(TypeDefinition classType, TypeReference newBaseType, ref Boolean modified)
	{
		String className = classType.FullName;
		String oldBaseClassName = classType.BaseType.FullName;
		String newBaseTypeName = newBaseType.FullName;

		if (oldBaseClassName == newBaseTypeName) return;

		Console.WriteLine($"{className}: {oldBaseClassName} -> {newBaseTypeName}");
		classType.BaseType = newBaseType;
		modified = true;
	}
}