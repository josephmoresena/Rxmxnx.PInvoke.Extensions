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
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3011)]
#endif
public static class AotInfo
{
	/// <summary>
	/// Indicates whether the current runtime is ahead-of-time.
	/// </summary>
	private static readonly Boolean isAotRuntime = !AotInfo.IsJitEnabled();

	/// <summary>
	/// Indicates whether runtime reflection is disabled.
	/// </summary>
	private static Boolean? reflectionDisabled;

	/// <summary>
	/// Indicates whether runtime reflection is disabled.
	/// </summary>
	public static Boolean IsReflectionDisabled
		=> AotInfo.reflectionDisabled ??= !AotInfo.StringTypeNameContainsString();
	/// <summary>
	/// Indicates whether the current runtime is NativeAOT.
	/// </summary>
	public static Boolean IsNativeAot => AotInfo.isAotRuntime;

	/// <summary>
	/// Indicates whether JIT is enabled in the current runtime.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if Jit is enabled; otherwise, <see langword="false"/>.
	/// </returns>
	private static Boolean IsJitEnabled()
	{
#if NET6_0_OR_GREATER
		Int64 ilBytes = JitInfo.GetCompiledILBytes();
		Int64 methodCount = JitInfo.GetCompiledMethodCount();
		TimeSpan compilationTime = JitInfo.GetCompilationTime();
		return ilBytes > 0L || methodCount > 0L || compilationTime > TimeSpan.Zero;
#else
		try
		{
			if (!AotInfo.StringTypeNameContainsString())
				// If reflection disabled, is AOT.
				return false;

			if (Type.GetType("System.Runtime.JitInfo") is { } typeJitInfo)
			{
				// Tries to retrieve JIT information using reflection.
				Boolean? isJitEnabled = AotInfo.IsJitEnabled(typeJitInfo);
				if (isJitEnabled.HasValue)
					return isJitEnabled.Value;
			}

			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().AsSpan())
			{
				if (String.IsNullOrEmpty(assembly.FullName)) continue;
				if (assembly.FullName.StartsWith("System.Reflection.Emit")) break;
				if (!assembly.FullName.Contains("Il2Cpp", StringComparison.OrdinalIgnoreCase)) continue;

				// IL2CPP is a AOT mode.
				return false;
			}

			// System.Reflection.Emit is not allowed in NativeAOT.
			return AotInfo.IsEmitSupported();
		}
		// If exception, might be AOT.
		catch (Exception)
		{
			return false;
		}
#endif
	}
#if !NET6_0_OR_GREATER
	/// <summary>
	/// Indicates whether JIT is enabled in the current runtime using reflection.
	/// </summary>
	/// <param name="typeJitInfo">CLR type of System.Runtime.JitInfo class.</param>
	/// <returns>
	/// <see langword="true"/> if Jit is enabled, <see langword="false"/> if Jit is disabled;
	/// otherwise, <see langword="null"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean? IsJitEnabled(Type typeJitInfo)
	{
		const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

		Type typeofAction = typeof(Func<Boolean, Int64>);
		Func<Boolean, Int64>? getCompiledIlBytes = typeJitInfo
		                                           .GetMethod("GetCompiledILBytes", bindingFlags)
		                                           ?.CreateDelegate(typeofAction) as Func<Boolean, Int64>;
		Func<Boolean, Int64>? getCompiledMethodCount = typeJitInfo
		                                               .GetMethod("GetCompiledMethodCount", bindingFlags)
		                                               ?.CreateDelegate(typeofAction) as Func<Boolean, Int64>;
		Func<Boolean, Int64>? getCompilationTimeInTicks = typeJitInfo
		                                                  .GetMethod("GetCompilationTimeInTicks", bindingFlags)
		                                                  ?.CreateDelegate(typeofAction) as Func<Boolean, Int64>;
		Int64? reflectionBytes = getCompiledIlBytes?.Invoke(false);
		Int64? methodCount = getCompiledMethodCount?.Invoke(false);
		Int64? compilationTimeInTicks = getCompilationTimeInTicks?.Invoke(false);

		if (reflectionBytes.HasValue || methodCount.HasValue || compilationTimeInTicks.HasValue)
			return reflectionBytes.GetValueOrDefault() > 0L || methodCount.GetValueOrDefault() > 0L ||
				compilationTimeInTicks.GetValueOrDefault() > 0L;

		return default; // Unabled to retrieve JIT information.
	}
	/// <summary>
	/// Indicates whether <see cref="System.Reflection.Emit"/> namespace is supported in the current runtime.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if a dynamic type was successfully emitted in the current runtime; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsEmitSupported()
	{
		Type? type = AssemblyBuilder
		             .DefineDynamicAssembly(new($"MyDynamicAssembly_{Guid.NewGuid():N}"), AssemblyBuilderAccess.Run)
		             .DefineDynamicModule($"MyDynamicModule_{Guid.NewGuid():N}")
		             .DefineType($"MyDynamicModule_{Guid.NewGuid():N}", TypeAttributes.NotPublic).CreateType();
		return type is not null && Activator.CreateInstance(type) is not null && type.Assembly.IsDynamic;
	}
#endif
	/// <summary>
	/// Indicates whether <see cref="String"/> type name contains the <c>String</c> word.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if <see cref="String"/> type name contains the <c>String</c> word;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	private static Boolean StringTypeNameContainsString()
	{
		ReadOnlySpan<Char> fullTypeName = typeof(String).ToString().AsSpan();
		ReadOnlySpan<Char> stringName = nameof(String).AsSpan();
		return stringName.Length <= fullTypeName.Length && fullTypeName[^stringName.Length..].SequenceEqual(stringName);
	}
}