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
			if (frame?.GetMethod() is not { } methodBase) continue;
			if (!EmitInfo.IsDynamicMethod(methodBase) && !AotInfo.IsImageMethodUnsafe(methodBase.MethodHandle))
				return false;
		}
		return true;
	}
#if !NET6_0_OR_GREATER
	/// <summary>
	/// Indicates whether JIT is enabled in the current runtime.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if Jit is enabled; otherwise, <see langword="false"/>.
	/// </returns>
	[UnconditionalSuppressMessage("Trimming", "IL2070")]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3776)]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS907)]
#endif
	private static Boolean IsJitEnabled()
	{
#if NET5_0_OR_GREATER
		if (TrimInfo.IsMobileTrimmedXnu())
			return true;
#endif
		try
		{
			if (!TrimInfo.StringTypeNameContainsString())
			{
				// If reflection disabled, is AOT.
				AotInfo.reflectionDisabled = true;
				return false;
			}
#if NET5_0_OR_GREATER
			if (TrimInfo.IsDesktopTrimmedPlatform())
				goto JitInfoCheck; // Skip Mono Runtime checks.
			if (OperatingSystem.IsAndroid())
				goto AotFrameCheck; // Skip XNU checks.
#else
			Boolean isAndroid = false;
#endif
			foreach (Assembly assembly in AotInfo.GetAssembliesSpan())
			{
				if (String.IsNullOrWhiteSpace(assembly.FullName) || assembly.IsDynamic) continue;
				switch (AotInfo.GetAssemblyName(assembly.FullName))
				{
					case "Xamarin.iOS":
					case "Xamarin.MacCatalyst":
					case "Xamarin.TVOS":
					case "Xamarin.WatchOS":
					case "Microsoft.iOS":
					case "Microsoft.MacCatalyst":
					case "Microsoft.tvOS":
					case "Microsoft.watchOS":
						return false;
#if !NET5_0_OR_GREATER
					case "Mono.Android":
						isAndroid = true;
						break;
#endif
				}
			}
#if NET5_0_OR_GREATER
			AotFrameCheck:
#endif

			if (MonoInfo.MonoAssemblyNameType is not null)
			{
				if (MemoryInspector.IsSupported)
					return !AotInfo.IsAotFrame();
#if !NET5_0_OR_GREATER
				if (isAndroid)
#else
				if (OperatingSystem.IsAndroid())
#endif
					goto JitInfoCheck;
				goto EmitCheck; // Avoid CoreCLR checks.
			}

			JitInfoCheck:
			// Tries to retrieve JIT information using reflection.
			Type? jitInfoType = typeof(RuntimeInformation).Assembly.GetType("System.Runtime.JitInfo");
			Boolean? isJitEnabled = AotInfo.IsJitEnabled(jitInfoType);
			if (isJitEnabled.HasValue && isJitEnabled.Value)
				return true;
		}
		// If exception, might be AOT.
		catch (Exception)
		{
			return false;
		}
		EmitCheck:
#if NET5_0_OR_GREATER
		if (TrimInfo.IsDesktopTrimmedPlatform() || OperatingSystem.IsAndroid())
			return false; // Avoid use System.Reflection.Emit on .NET 5.0
#endif
		// System.Reflection.Emit is not allowed in AOT/IL2CPP.
		return EmitInfo.IsEmitAllowed;
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
	private static Boolean? IsJitEnabled(Type? jitInfoType)
	{
		String ilBytesCountName = jitInfoType is not null ? "GetCompiledILBytes" : "GetILBytesJitted";
		Int64? reflectionBytes = AotInfo.GetJitCount<Int64>(jitInfoType ?? typeof(RuntimeHelpers), ilBytesCountName);

		if (reflectionBytes.GetValueOrDefault() != 0L)
			return true;

		Int64? methodCount = jitInfoType is not null ?
			AotInfo.GetJitCount<Int64>(jitInfoType, "GetCompiledMethodCount") :
			AotInfo.GetJitCount<Int32>(typeof(RuntimeHelpers), "GetMethodsJittedCount");

		if (methodCount.GetValueOrDefault() != 0L)
			return true;

		if (reflectionBytes.HasValue || methodCount.HasValue)
			return false;

		return default; // Unabled to retrieve JIT information.
	}
	/// <summary>
	/// Retrieves the result of a JIT method count.
	/// </summary>
	/// <typeparam name="T">Type of JIT method count result.</typeparam>
	/// <param name="declaringType">Declaring method count type.</param>
	/// <param name="methodName">JIT method count name.</param>
	/// <returns>The result of a JIT method count.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[UnconditionalSuppressMessage("AOT", "IL3050")]
	[UnconditionalSuppressMessage("Trimming", "IL2070")]
	private static T? GetJitCount<T>(Type declaringType, String methodName) where T : unmanaged, IEquatable<T>
	{
		const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
#if !NET5_0_OR_GREATER
		Type typeofFunc = typeof(Func<Boolean, T>);
#endif
		Func<Boolean, T>? getCompiledIlBytes = declaringType.GetMethod(methodName, bindingFlags)
#if !NET5_0_OR_GREATER
		                                                    ?.CreateDelegate(typeofFunc) as Func<Boolean, T>;
#else
		                                                    ?.CreateDelegate<Func<Boolean, T>>();
#endif
		return getCompiledIlBytes?.Invoke(false);
	}
	/// <inheritdoc cref="AppDomain.GetAssemblies()"/>
	/// <returns>A read-only span of assemblies in this application domain.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ReadOnlySpan<Assembly> GetAssembliesSpan()
	{
		Assembly[] array = AppDomain.CurrentDomain.GetAssemblies();
		return MemoryMarshal.CreateReadOnlySpan(ref NativeUtilities.GetArrayDataReference(array), array.Length);
	}
	/// <summary>
	/// Retrieves the assembly name from its full name.
	/// </summary>
	/// <param name="assemblyFullName">The full name of an assembly.</param>
	/// <returns>The name of the assembly.</returns>
	private static String GetAssemblyName(String assemblyFullName)
	{
		Int32 assemblyNameLength = assemblyFullName.IndexOf(',');
		return assemblyNameLength < 0 ? assemblyFullName : assemblyFullName[..assemblyNameLength];
	}
#else
	/// <summary>
	/// Indicates whether the current platform is Desktop or Android.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if current platform is Desktop or Android; otherwise, <see langword="false"/>.
	/// </returns>
	private static Boolean IsDesktopOrAndroid() => TrimInfo.IsDesktopTrimmedPlatform() || OperatingSystem.IsAndroid();
	/// <summary>
	/// Indicates whether the current runtime is Mono ahead-of-time.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if Mono AOT is enabled; otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsMonoAot()
	{
		if (MonoInfo.MonoAssemblyNameType is null || !MemoryInspector.IsSupported) return false;
		try
		{
			return AotInfo.IsAotFrame();
		}
		// If exception, might be AOT.
		catch (Exception)
		{
			return true;
		}
	}
#endif
}