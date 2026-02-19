namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3011)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3267)]
#endif
public static partial class AotInfo
{
	/// <summary>
	/// Indicates whether the executing frame is AOT.
	/// </summary>
	/// <returns><see langword="true"/> if executing frame is AOT; otherwise, <see langword="false"/>.</returns>
	[UnconditionalSuppressMessage("Trimming", "IL2026")]
	private static Boolean IsAotFrame()
	{
		StackTrace stackTrace = new();
#if !NETCOREAPP
		ReadOnlySpan<StackFrame?> frames = stackTrace.GetFrames() ?? [];
#else
		ReadOnlySpan<StackFrame?> frames = stackTrace.GetFrames();
#endif
		foreach (StackFrame? frame in frames)
		{
			if (frame is null || !frame.HasMethod()) continue;
			MethodBase methodBase = frame.GetMethod()!;
			if (!AotInfo.IsNativeMethod(methodBase.MethodHandle))
				return false;
		}
		return true;
	}
	/// <summary>
	/// Indicates whether the function pointer of <paramref name="methodHandle"/> references to an R/RX memory section.
	/// </summary>
	/// <param name="methodHandle">A <see langword="RuntimeMethodHandle"/> value.</param>
	/// <returns>
	/// <see langword="true"/> if the function pointer references to an R/RX memory section; otherwise,
	/// <see langword="false"/>.
	/// </returns>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	private static unsafe Boolean IsNativeMethod(RuntimeMethodHandle methodHandle)
		=> MemoryInspector.Instance.IsLiteral(methodHandle.GetFunctionPointer().ToPointer());
#if !NET6_0_OR_GREATER
	/// <summary>
	/// Indicates whether JIT is enabled in the current runtime.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if Jit is enabled; otherwise, <see langword="false"/>.
	/// </returns>
	[UnconditionalSuppressMessage("Trimming", "IL2070")]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsJitEnabled()
	{
		Boolean hasReflectionEmit = false;
		try
		{
			if (!TrimInfo.StringTypeNameContainsString())
			{
				// If reflection disabled, is AOT.
				AotInfo.reflectionDisabled = true;
				return false;
			}

			foreach (Assembly assembly in AotInfo.GetAssembliesSpan())
			{
				if (String.IsNullOrEmpty(assembly.FullName)) continue;
				switch (assembly.FullName[..assembly.FullName.IndexOf(',')])
				{
					case "System.Reflection.Emit":
						hasReflectionEmit = true;
						continue;
					case "Microsoft.iOS":
					case "Xamarin.iOS":
						return false;
				}
			}

			if (MonoInfo.MonoAssemblyNameType is not null && MemoryInspector.IsSupported)
				return AotInfo.IsAotFrame();

			if (MonoInfo.MonoAssemblyNameType is null &&
			    typeof(RuntimeInformation).Assembly.GetType("System.Runtime.JitInfo") is { } jitInfoType)
			{
				// Tries to retrieve JIT information using reflection.
				Boolean? isJitEnabled = AotInfo.IsJitEnabled(jitInfoType);
				if (isJitEnabled.HasValue && isJitEnabled.Value)
					return true;
			}
		}
		// If exception, might be AOT.
		catch (Exception)
		{
			return false;
		}

		// System.Reflection.Emit is not allowed in AOT/IL2CPP.
		return hasReflectionEmit && EmitInfo.IsEmitAllowed;
	}
	/// <summary>
	/// Indicates whether JIT is enabled in the current runtime using reflection.
	/// </summary>
	/// <param name="jitInfoType">CLR type of System.Runtime.JitInfo class.</param>
	/// <returns>
	/// <see langword="true"/> if Jit is enabled, <see langword="false"/> if Jit is disabled;
	/// otherwise, <see langword="null"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[UnconditionalSuppressMessage("AOT", "IL3050")]
	[UnconditionalSuppressMessage("Trimming", "IL2070")]
	private static Boolean? IsJitEnabled(Type jitInfoType)
	{
		const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

#if !NET5_0_OR_GREATER
		Type typeofFunc = typeof(Func<Boolean, Int64>);
#endif
		Func<Boolean, Int64>? getCompiledIlBytes = jitInfoType.GetMethod("GetCompiledILBytes", bindingFlags)
#if !NET5_0_OR_GREATER
		                                                      ?.CreateDelegate(typeofFunc) as Func<Boolean, Int64>;
#else
		                                                      ?.CreateDelegate<Func<Boolean, Int64>>();
#endif
		Int64? reflectionBytes = getCompiledIlBytes?.Invoke(false);

		if (reflectionBytes.GetValueOrDefault() != 0L)
			return true;

		Func<Boolean, Int64>? getCompiledMethodCount = jitInfoType.GetMethod("GetCompiledMethodCount", bindingFlags)
#if !NET5_0_OR_GREATER
		                                                          ?.CreateDelegate(typeofFunc) as Func<Boolean, Int64>;
#else
		                                                          ?.CreateDelegate<Func<Boolean, Int64>>();
#endif
		Int64? methodCount = getCompiledMethodCount?.Invoke(false);

		if (methodCount.GetValueOrDefault() != 0L)
			return true;

		if (reflectionBytes.HasValue || methodCount.HasValue)
			return false;

		return default; // Unabled to retrieve JIT information.
	}
	/// <inheritdoc cref="AppDomain.GetAssemblies()"/>
	/// <returns>A read-only span of assemblies in this application domain.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ReadOnlySpan<Assembly> GetAssembliesSpan()
	{
		Assembly[] array = AppDomain.CurrentDomain.GetAssemblies();
		return MemoryMarshal.CreateReadOnlySpan(ref NativeUtilities.GetArrayDataReference(array), array.Length);
	}
#endif
}