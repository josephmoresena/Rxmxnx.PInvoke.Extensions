namespace Rxmxnx.PInvoke.Tests;

[StructLayout(LayoutKind.Sequential)]
public readonly struct TextContainer
{
	public static readonly TextContainer Slash =
		new("Windows uses backslashes (\\) to separate directories in file paths");
	public static readonly TextContainer NewLine =
		new(
			"In Windows new lines are represented by the combination of a carriage return and a line feed (\r\n) characters.");
	public static readonly TextContainer Tab = new("Tabs are represented by the \t character in strings.");
	public static readonly TextContainer Quotes = new("Quotes are represented by the \" character in strings.");

	public readonly TextContainer<String> Utf16;
	public readonly TextContainer<CString> Utf8;

	public TextContainer(ReadOnlySpanFunc<Byte> func)
	{
		this.Utf8 = new() { Value = new(func), };
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		this.Utf16 = new() { Value = Encoding.UTF8.GetString(func()), };
#elif NET46_OR_GREATER
		this.Utf16 = new() { Value = func().ToUtf16(), };
#else
		this.Utf16 = new() { Value = Encoding.UTF8.GetString(func().ToArray()), };
#endif
	}

	private TextContainer(String str)
	{
		this.Utf16 = new() { Value = str, };
		this.Utf8 = new() { Value = Encoding.UTF8.GetBytes(str), };
	}
}

[StructLayout(LayoutKind.Sequential)]
public sealed class TextContainer<TString> where TString : IEquatable<TString>, IEquatable<String>
{
	public TString? Value { get; set; }
}