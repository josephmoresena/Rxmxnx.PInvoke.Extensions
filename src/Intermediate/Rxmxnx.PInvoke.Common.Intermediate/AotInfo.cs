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
	[ReadOnly(true)]
	public static Boolean IsReflectionDisabled { get; }
	/// <summary>
	/// Indicates whether the current runtime is NativeAOT.
	/// </summary>
	[ReadOnly(true)]
	public static Boolean IsNativeAot { get; }

	/// <summary>
	/// Static constructor.
	/// </summary>
	static AotInfo()
	{
		Int64 ilBytes = JitInfo.GetCompiledILBytes();
		Int64 methodCount = JitInfo.GetCompiledMethodCount();
		TimeSpan compilationTime = JitInfo.GetCompilationTime();
		Boolean jitDisabled = ilBytes == default && methodCount == default && compilationTime == default;

		AotInfo.IsNativeAot = jitDisabled;
		AotInfo.IsReflectionDisabled = jitDisabled && !AotInfo.StringTypeNameContainsString();
#if !NET6_0_OR_GREATER
		try
		{
			if (!AotInfo.StringTypeNameContainsString())
			{
				// If reflection disabled, is AOT.
				AotInfo.IsReflectionDisabled = true;
				AotInfo.IsNativeAot = true;
				return;
			}

			AotInfo.IsReflectionDisabled = false;
			if (Type.GetType("System.Runtime.JitInfo") is { } typeJitInfo)
			{
				AotInfo.IsNativeAot = !AotInfo.IsJitEnabled(typeJitInfo, out Boolean invoked);
				if (invoked) return;
			}

			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().AsSpan())
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
		}
		// If exception, might be AOT.
		catch (Exception)
		{
			AotInfo.IsNativeAot = true;
		}
#endif
	}
	/// <summary>
	/// Indicates whether <see cref="String"/> type name contains the <c>String</c> word.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if <see cref="String"/> type name contains the <c>String</c> word;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean StringTypeNameContainsString()
	{
		ReadOnlySpan<Char> fullTypeName = typeof(String).ToString().AsSpan();
		ReadOnlySpan<Char> stringName = nameof(String).AsSpan();
		return stringName.Length <= fullTypeName.Length && fullTypeName[^stringName.Length..].SequenceEqual(stringName);
	}
#if !NET6_0_OR_GREATER
	/// <summary>
	/// Use the reflected JitInfo class to inspect the current runtime.
	/// </summary>
	/// <param name="typeJitInfo">CLR type of JitInfo class.</param>
	/// <param name="invoked">
	/// Output. Indicates whether at least one of the methods of the JitInfo class was invoked.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if Jit is enabled; otherwise <see langword="false"/>.
	/// </returns>
	private static Boolean IsJitEnabled(Type typeJitInfo, out Boolean invoked)
	{
		Func<Boolean, Int64>? getCompiledIlBytes = typeJitInfo
		                                           .GetMethod("GetCompiledILBytes",
		                                                      BindingFlags.Public | BindingFlags.Static)
		                                           ?.CreateDelegate<Func<Boolean, Int64>>();
		Func<Boolean, Int64>? getCompiledMethodCount = typeJitInfo
		                                               .GetMethod("GetCompiledMethodCount",
		                                                          BindingFlags.Public | BindingFlags.Static)
		                                               ?.CreateDelegate<Func<Boolean, Int64>>();
		Func<Boolean, Int64>? getCompilationTimeInTicks = typeJitInfo
		                                                  .GetMethod("GetCompilationTimeInTicks",
		                                                             BindingFlags.NonPublic | BindingFlags.Static)
		                                                  ?.CreateDelegate<Func<Boolean, Int64>>();

		Int64? reflectionBytes = getCompiledIlBytes?.Invoke(false);
		Int64? methodCount = getCompiledMethodCount?.Invoke(false);
		Int64? compilationTimeInTicks = getCompilationTimeInTicks?.Invoke(false);

		invoked = reflectionBytes.HasValue || methodCount.HasValue || compilationTimeInTicks.HasValue;
		return reflectionBytes.GetValueOrDefault() > 0L || methodCount.GetValueOrDefault() > 0L ||
			compilationTimeInTicks.GetValueOrDefault() > 0L;
	}
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