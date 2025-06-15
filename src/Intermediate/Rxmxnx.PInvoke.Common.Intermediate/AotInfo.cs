#if !NET6_0_OR_GREATER
using AssemblyBuilder = System.Reflection.Emit.AssemblyBuilder;
using AssemblyBuilderAccess = System.Reflection.Emit.AssemblyBuilderAccess;
#endif

namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides information about the Ahead-of-Time compilation.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
public static class AotInfo
{
	/// <summary>
	/// Indicates whether runtime reflection is disabled.
	/// </summary>
	public static Boolean IsReflectionDisabled { get; }
	/// <summary>
	/// Indicates whether the current runtime is NativeAOT.
	/// </summary>
	public static Boolean IsNativeAot { get; }

	/// <summary>
	/// Static constructor.
	/// </summary>
	static AotInfo()
	{
		ReadOnlySpan<Char> fullTypeName = typeof(String).ToString().AsSpan();
		ReadOnlySpan<Char> stringName = nameof(String).AsSpan();

		AotInfo.IsReflectionDisabled = stringName.Length >= fullTypeName.Length ||
			!fullTypeName[^stringName.Length..].SequenceEqual(stringName);

		if (AotInfo.IsReflectionDisabled)
		{
			// If reflection disabled, is AOT.
			AotInfo.IsNativeAot = true;
			return;
		}

		try
		{
#if NET6_0_OR_GREATER
			Int64 ilBytes = JitInfo.GetCompiledILBytes();
			Int64 methodCount = JitInfo.GetCompiledMethodCount();
			TimeSpan compilationTime = JitInfo.GetCompilationTime();

			// If JIT info default, is AOT.
			AotInfo.IsNativeAot = ilBytes == default && methodCount == default && compilationTime == default;
#else
			foreach (Assembly? assembly in AppDomain.CurrentDomain.GetAssemblies().AsSpan())
			{
				if (assembly.FullName?.StartsWith("System.Reflection.Emit") == true)
				{
					// System.Reflection.Emit is not allowed in NativeAOT.
					AotInfo.IsNativeAot = AotInfo.TryToEmit();
					break;
				}
				if (!assembly.FullName?.Contains("Il2Cpp", StringComparison.OrdinalIgnoreCase) == true) continue;

				// IL2CPP is a AOT mode.
				AotInfo.IsNativeAot = true;
				break;
			}
			AotInfo.IsNativeAot = false;
#endif
		}
		// If exception, might be AOT.
		catch (Exception)
		{
			AotInfo.IsNativeAot = true;
		}
	}
#if !NET6_0_OR_GREATER
	/// <summary>
	/// Tries to emit a dynamic type in the current runtime. 
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if the dynamic type was successfully emitted in the current runtime; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	private static Boolean TryToEmit()
	{
		Type? type = AssemblyBuilder
		             .DefineDynamicAssembly(new($"MyDynamicAssembly_{Guid.NewGuid():N}"), AssemblyBuilderAccess.Run)
		             .DefineDynamicModule($"MyDynamicModule_{Guid.NewGuid():N}")
		             .DefineType($"MyDynamicModule_{Guid.NewGuid():N}", TypeAttributes.NotPublic).CreateType();
		Boolean isAot = type is null || Activator.CreateInstance(type) is null || !type.Assembly.IsDynamic;
		return isAot;
	}
#endif
}