namespace Rxmxnx.PInvoke.ApplicationTest;

public sealed class ConsoleNotifier : IExecutionNotifier, IPlatformNotifier
{
	public static readonly CancellationToken CancellationToken = ConsoleNotifier.CreateCancellationToken();
	public static readonly ConsoleNotifier Notifier = new();

	private static readonly Lock consoleLock = new();
	public static IPlatformNotifier PlatformNotifier => ConsoleNotifier.Notifier;
	private ConsoleNotifier() { }

	public Int32 RefreshTime => 250;

	public void Begin(ProcessStartInfo info)
	{
		String args = String.Join(' ', info.ArgumentList);
		ConsoleNotifier.WriteColoredLine(ConsoleColor.Blue,
		                                 $"Starting... [{info.WorkingDirectory}] {info.FileName} {args}");
	}

	public void End(ProcessStartInfo info)
	{
		String args = String.Join(' ', info.ArgumentList);
		ConsoleNotifier.WriteColoredLine(ConsoleColor.Green,
		                                 $"Finished. [{info.WorkingDirectory}] {info.FileName} {args}");
	}

	public void Result(Int32 result, String executionName)
	{
		ConsoleColor color = result == 0 ? ConsoleColor.Green : ConsoleColor.Red;
		ConsoleNotifier.WriteColoredLine(color, $"{executionName}: 0x{result:x8}");
	}

	void IPlatformNotifier.BeginDetection()
		=> ConsoleNotifier.WriteColoredLine(ConsoleColor.Blue, "Detecting platform...");
	void IPlatformNotifier.EndDetection(OSPlatform platform, Architecture arch)
		=> ConsoleNotifier.WriteColoredLine(ConsoleColor.Green, $"{platform} {arch} detected.");
	void IPlatformNotifier.Initialization(OSPlatform platform, Architecture arch)
		=> ConsoleNotifier.WriteColoredLine(ConsoleColor.Green, $"{platform} {arch} initialized.");

	public void Begin(String url, Int64? total)
	{
		if (!total.HasValue)
		{
			ConsoleNotifier.WriteColoredLine(ConsoleColor.Blue, $"Starting download... {url}");
			return;
		}
		Double value = ConsoleNotifier.GetValue(total.Value, out String unitName);
		ConsoleNotifier.WriteColoredLine(ConsoleColor.Blue, $"Downloading... {url} [{value:0.##} {unitName}]");
	}
	public void Progress(String url, Int64? total, Int64 progress, ref Int32 cursorTop, ref Int32 textLength)
	{
		Double value;
		String text;
		if (!total.HasValue)
		{
			value = ConsoleNotifier.GetValue(progress, out String unitName);
			text = $"Downloading... {url} [{value:0.##} {unitName}]";
		}
		else
		{
			value = progress / (Double)total.Value;
			text = $"Downloading... {url} [{value:P}]";
		}

		using Lock.Scope scope = ConsoleNotifier.consoleLock.EnterScope();
		try
		{
			if (cursorTop == -1)
				cursorTop = Console.CursorTop;
			else
				Console.SetCursorPosition(0, cursorTop);
		}
		catch (Exception)
		{
			// Ignore
		}

		Console.Write(text.PadRight(textLength));
		Console.WriteLine();
		textLength = text.Length;
	}

	public void End(String url, Int64 total, String destinationPath)
		=> ConsoleNotifier.WriteColoredLine(ConsoleColor.Green,
		                                    $"Downloaded. {url} -> {destinationPath} [{ConsoleNotifier.GetValue(total, out String unitName):0.##} {unitName}]");

	public static void Results(Dictionary<String, Int32> results)
	{
		Int32 maxKeyLength = results.Keys.Max(k => k.Length);
		using Lock.Scope scope = ConsoleNotifier.consoleLock.EnterScope();
		try
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine($"{"Test".PadRight(maxKeyLength)} | Exit code ");
			Console.WriteLine(new String('-', maxKeyLength + 20));

			foreach (KeyValuePair<String, Int32> kvp in results.OrderBy(kvp => (Math.Abs(kvp.Value), kvp.Key)))
			{
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write($"{kvp.Key.PadRight(maxKeyLength)} | ");

				Console.ForegroundColor = kvp.Value != 0 ? ConsoleColor.Red : ConsoleColor.Green;
				Console.WriteLine($"0x{kvp.Value:x8}");
			}
		}
		finally
		{
			Console.ResetColor();
		}
	}
	public static CancellationTokenRegistration RegisterCancellation(CancellationTokenSource cts)
		=> ConsoleNotifier.CancellationToken.Register(ConsoleNotifier.CancelSource, cts);

	private static Double GetValue(Int64 total, out String unitName)
	{
		const Int32 threshold = 1024;
		ReadOnlySpan<String> unitNames = ["B", "KiB", "MiB", "GiB",];
		Double value = total;
		Int32 unit = 0;
		while (unit < unitNames.Length && value >= threshold)
		{
			value /= 1024;
			unit++;
		}
		unitName = unitNames[unit];
		return value;
	}
	private static void WriteColoredLine(ConsoleColor color, String message)
	{
		using Lock.Scope scope = ConsoleNotifier.consoleLock.EnterScope();
		Console.ForegroundColor = color;
		Console.WriteLine(message);
		Console.ResetColor();
	}
	private static CancellationToken CreateCancellationToken()
	{
		CancellationTokenSource cts = new();
		Console.CancelKeyPress += (_, e) =>
		{
			e.Cancel = true;
			ConsoleNotifier.CancelAndDispose(cts);
		};
		AppDomain.CurrentDomain.ProcessExit += (_, _) => { ConsoleNotifier.CancelAndDispose(cts); };
		return cts.Token;
	}
	private static void CancelAndDispose(CancellationTokenSource cts)
	{
		try
		{
			cts.Cancel();
			cts.Dispose();
		}
		catch (Exception)
		{
			// Ignore
		}
	}
	private static void CancelSource(Object? objSource)
	{
		if (objSource is not CancellationTokenSource cts) return;
		cts.Cancel();
	}
}