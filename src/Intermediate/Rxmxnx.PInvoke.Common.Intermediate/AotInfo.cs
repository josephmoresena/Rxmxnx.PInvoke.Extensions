namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides information about the Ahead-of-Time compilation.
/// </summary>
[ExcludeFromCodeCoverage]
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
			Int64 ilBytes = JitInfo.GetCompiledILBytes();
			Int64 methodCount = JitInfo.GetCompiledMethodCount();
			TimeSpan compilationTime = JitInfo.GetCompilationTime();

			// If JIT info default, is AOT.
			AotInfo.IsNativeAot = ilBytes == default && methodCount == default && compilationTime == default;
		}
		// If exception, might be AOT.
		catch (Exception)
		{
			AotInfo.IsNativeAot = true;
		}
	}
}