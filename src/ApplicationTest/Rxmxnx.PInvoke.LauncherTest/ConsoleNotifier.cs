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

	public void Print(String message, Boolean done = false)
	{
		ConsoleColor color = done ? ConsoleColor.Green : ConsoleColor.Blue;
		ConsoleNotifier.WriteColoredLine(color, message);
	}
	public void PrintError(String message, Exception? exception)
	{
		ConsoleNotifier.WriteColoredLine(ConsoleColor.Red, message);
		String? exMessage;
		try
		{
			exMessage = exception?.ToString();
		}
		catch (Exception)
		{
			exMessage = exception?.Message;
		}
		if (!String.IsNullOrWhiteSpace(exMessage))
			ConsoleNotifier.WriteColoredLine(ConsoleColor.Magenta, exMessage);
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
	public static void ShowDiskUsage()
	{
		ReadOnlySpan<String> headers = ["Filesystem", "Size", "Used", "Avail", "Use %",];
		ReadOnlySpan<Char> separator = ['\t',];
		Span<Int32> maxLen = [-1, -1, -1, -1, -1,];
		DriveInfo[] drives = DriveInfo.GetDrives();
		Span<ConsoleColor> colors = stackalloc ConsoleColor[drives.Length];
		String[] colValues = ArrayPool<String>.Shared.Rent(drives.Length * headers.Length);

		Span<String> colFs = colValues.AsSpan()[..drives.Length];
		Span<String> colSize = colValues.AsSpan().Slice(drives.Length, drives.Length);
		Span<String> colUsed = colValues.AsSpan().Slice(2 * drives.Length, drives.Length);
		Span<String> colAvail = colValues.AsSpan().Slice(3 * drives.Length, drives.Length);
		Span<String> colUse = colValues.AsSpan().Slice(4 * drives.Length, drives.Length);

		try
		{
			Int32 validRows = 0;
			foreach (DriveInfo drive in drives)
			{
				try
				{
					if (!drive.IsReady || drive.TotalSize == 0)
						continue;

					Int64 total = drive.TotalSize;
					Int64 free = drive.AvailableFreeSpace;
					Int64 used = total - free;
					Double percentUsed = (Double)used / total * 100.0;

					String fs = drive.Name.TrimEnd(Path.DirectorySeparatorChar);
					String size = ConsoleNotifier.FormatBytes(total);
					String usedStr = ConsoleNotifier.FormatBytes(used);
					String avail = ConsoleNotifier.FormatBytes(free);
					String usePct = $"{percentUsed:0.0}%";

					colFs[validRows] = fs;
					colSize[validRows] = size;
					colUsed[validRows] = usedStr;
					colAvail[validRows] = avail;
					colUse[validRows] = usePct;

					colors[validRows] = percentUsed switch
					{
						// Determine color based on usage
						< 70.0 => ConsoleColor.Blue,
						< 90.0 => ConsoleColor.Cyan,
						_ => ConsoleColor.Red,
					};

					// Track max widths
					maxLen[0] = Math.Max(maxLen[0], fs.Length);
					maxLen[1] = Math.Max(maxLen[1], size.Length);
					maxLen[2] = Math.Max(maxLen[2], usedStr.Length);
					maxLen[3] = Math.Max(maxLen[3], avail.Length);
					maxLen[4] = Math.Max(maxLen[4], usePct.Length);

					validRows++;
				}
				catch (Exception ex)
				{
					colFs[validRows] = drive.Name;
					colSize[validRows] = $"[Error: {ex.Message}]";
					colUsed[validRows] = String.Empty;
					colAvail[validRows] = String.Empty;
					colUse[validRows] = String.Empty;
					colors[validRows] = ConsoleColor.Yellow;

					maxLen[0] = Math.Max(maxLen[0], colFs[validRows].Length);
					maxLen[1] = Math.Max(maxLen[1], colSize[validRows].Length);
					validRows++;
				}
			}

			if (validRows == 0)
			{
				Console.WriteLine("No drives found or accessible.");
				return;
			}

			Int32 bufferLength = 0;
			for (Int32 i = 0; i < headers.Length; i++)
			{
				maxLen[i] = Math.Max(maxLen[i], headers[i].Length);
				bufferLength += maxLen[i];
			}
			bufferLength += (headers.Length - 1) * separator.Length;

			Span<Char> buffer = stackalloc Char[bufferLength];
			ConsoleNotifier.FillBufferLine(buffer, headers, maxLen, separator);
			Console.WriteLine(buffer.ToString());

			for (Int32 i = 0; i < validRows; i++)
			{
				ConsoleNotifier.FillBufferLine(buffer, [colFs[i], colSize[i], colUsed[i], colAvail[i], colUse[i],],
				                               maxLen, separator);
				ConsoleNotifier.WriteColoredLine(colors[i], buffer.ToString());
			}
		}
		finally
		{
			ArrayPool<String>.Shared.Return(colValues, true);
		}
	}

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
	private static String FormatBytes(Int64 bytes)
	{
		ReadOnlySpan<Char> sizes = ['B', 'K', 'M', 'G', 'T',];
		Double len = bytes;
		Int32 order = 0;
		while (len >= 1024 && order < sizes.Length - 1)
		{
			order++;
			len /= 1024;
		}
		return $"{len:0.##}{sizes[order]}";
	}
	private static void FillBufferLine(Span<Char> buffer, ReadOnlySpan<String> fields, ReadOnlySpan<Int32> widths,
		ReadOnlySpan<Char> separator)
	{
		buffer.Fill(' ');

		Int32 pos = 0;
		for (Int32 i = 0; i < fields.Length; i++)
		{
			String text = fields[i];

			Int32 copyLen = Math.Min(text.Length, widths[i]);
			text.AsSpan(0, copyLen).CopyTo(buffer[pos..]);

			pos += widths[i];

			if (i >= fields.Length - 1) continue;
			separator.CopyTo(buffer[pos..]);
			pos += separator.Length;
		}
	}
}