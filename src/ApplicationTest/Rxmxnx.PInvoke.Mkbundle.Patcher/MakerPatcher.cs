namespace Rxmxnx.PInvoke.Mkbundle.Patcher;

public static class MakerPatcher
{
	[Flags]
	public enum Result : SByte
	{
		MonoLibNotFound = -128,
		MakeBundleNotFound = -64,
		MsCoreLibNotFound = -32,
		MonoCecilError = -16,
		MkBundleTypeNotFound = -8,
		AotCompileMethodNotFound = -4,
		ExecuteMethodNotFound = -2,
		FileError = -1,
		Done = 0,
		Unmodified = 1,
	}

	[UnmanagedCallersOnly(EntryPoint = "PatchAssemblyForWindows")]
	internal static unsafe Result PatchAssembly(Char* monoLibPath, Int32 monoLibPathLength, Char* outputPath,
		Int32 outputPathLength)
		=> MakerPatcher.PatchAssembly(new(monoLibPath, monoLibPathLength), new(outputPath, outputPathLength));

	private static Result PatchAssembly(ReadOnlySpan<Char> monoLibPath, ReadOnlySpan<Char> outputPath)
	{
		try
		{
			DirectoryInfo monoLib = new(monoLibPath.ToString());
			if (!monoLib.Exists) return Result.MonoLibNotFound;

			FileInfo? mkbundleAssembly = monoLib.GetFiles("mkbundle.exe").FirstOrDefault();
			if (mkbundleAssembly is null) return Result.MakeBundleNotFound;
			if (!File.Exists(Path.Combine(monoLib.FullName, "mscorlib.dll"))) return Result.MsCoreLibNotFound;

			DirectoryInfo outputDir = new(outputPath.ToString());
			String patchedAssemblyPath = Path.Combine(outputDir.FullName, mkbundleAssembly.Name);
			Boolean withSymbols = File.Exists(Path.Combine(monoLib.FullName, "mkbundle.pdb"));

			outputDir.Create();
			return !File.Exists(patchedAssemblyPath) ?
				MakerPatcher.PatchAssembly(mkbundleAssembly.FullName, patchedAssemblyPath, withSymbols) :
				Result.Unmodified;
		}
		catch (Exception)
		{
			return Result.FileError;
		}
	}
	private static Result PatchAssembly(String inputPath, String outputPath, Boolean withSymbols)
	{
		ReaderParameters readerParameters = new() { ReadWrite = false, ReadSymbols = withSymbols, };
		WriterParameters writerParameters = new() { WriteSymbols = withSymbols, };

		Boolean modified = false;
		try
		{
			using AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(inputPath, readerParameters);
			using ModuleDefinition? module = assembly.MainModule;

			if (module.Types.FirstOrDefault(t => t.Name == "MakeBundle") is not { } mkbundleType)
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

			MakerPatcher.PatchAotCompileMethod(aotCompileMethod, ref modified);
			MakerPatcher.PatchExecuteMethod(executeMethod, ref modified);

			assembly.Write(outputPath, writerParameters);
		}
		catch (Exception)
		{
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
}