namespace Rxmxnx.PInvoke.ApplicationTest;

public enum NetVersion : Byte
{
	Net60 = 6,
	Net70 = 7,
	Net80 = 8,
	Net90 = 9,
}

public static class NetVersionExtensions
{
	public static String GetTargetFramework(this NetVersion version)
		=> version switch
		{
			NetVersion.Net60 => "net6.0",
			NetVersion.Net70 => "net7.0",
			NetVersion.Net80 => "net8.0",
			NetVersion.Net90 => "net9.0",
			_ => "net10.0",
		};
}