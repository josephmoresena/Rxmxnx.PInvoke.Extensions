namespace Rxmxnx.PInvoke.Tests;

public readonly struct TextContainer(String str)
{
	public static readonly TextContainer Slash =
		new("Windows uses backslashes (\\) to separate directories in file paths");
	public static readonly TextContainer NewLine =
		new(
			"In Windows new lines are represented by the combination of a carriage return and a line feed (\r\n) characters.");
	public static readonly TextContainer Tab = new("Tabs are represented by the \t character in strings.");
	public static readonly TextContainer Quotes = new("Quotes are represented by the \" character in strings.");

	public readonly TextContainer<String> Utf16 = new() { Value = str, };
	public readonly TextContainer<CString> Utf8 = new() { Value = Encoding.UTF8.GetBytes(str), };
}

public sealed class TextContainer<TString> where TString : IEquatable<TString>, IEquatable<String>
{
	public TString? Value { get; set; }
}