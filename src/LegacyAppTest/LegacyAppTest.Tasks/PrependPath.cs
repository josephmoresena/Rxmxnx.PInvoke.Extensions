using System;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace LegacyAppTest.Tasks;

/// <summary>
/// Prepends a directory to <c>PATH</c> in the MSBuild process, so child tools inherit it.
/// </summary>
public sealed class PrependPath : Task
{
	/// <summary>
	/// Directory to place at the front of <c>PATH</c>.
	/// </summary>
	[Required]
	// ReSharper disable once UnusedAutoPropertyAccessor.Global
	public String Dir { get; set; }

	/// <inheritdoc/>
	public override Boolean Execute()
	{
		String path = Environment.GetEnvironmentVariable("PATH") ?? "";
		String prefix = this.Dir + ":";
		if (!path.StartsWith(prefix, StringComparison.Ordinal))
			Environment.SetEnvironmentVariable("PATH", prefix + path);
		return true;
	}
}