using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator

namespace LegacyAppTest.Tasks;

/// <summary>
/// Renames the Objective-C class <c>NSURLSessionDelegate</c> emitted by the Xamarin.Mac registrar.
/// </summary>
/// <remarks>
/// Debug links that class into the app, and NetworkServiceProxy defines another class with the same name.
/// The Foundation protocol keeps the original name. An executable that does not contain the class is left as it is.
/// </remarks>
public sealed class RenameUrlSessionClass : Task
{
	private const String classSymbol = "_OBJC_CLASS_$_NSURLSessionDelegate";
	private const String metaclassSymbol = "_OBJC_METACLASS_$_NSURLSessionDelegate";
	private const UInt32 fatMagic = 0xCAFEBABE;
	private const UInt32 machHeader64 = 0xFEEDFACF;
	private const UInt32 segmentCommand64 = 0x19;
	private const UInt32 symtabCommand = 0x2;
	private static readonly Byte[] originalName = Encoding.ASCII.GetBytes("NSURLSessionDelegate");
	private static readonly Byte[] renamedName = Encoding.ASCII.GetBytes("XamUrlSessionDelegate");

	/// <summary>
	/// Native executable produced by <c>mmp</c>.
	/// </summary>
	[Required]
	public String Executable { get; set; }

	/// <inheritdoc/>
	public override Boolean Execute()
	{
		try
		{
			if (String.IsNullOrEmpty(this.Executable) || !File.Exists(this.Executable))
			{
				this.Log.LogError("Executable was not found: " + this.Executable);
				return false;
			}

			Byte[] data = File.ReadAllBytes(this.Executable);
			if (!RenameUrlSessionClass.Patch(data))
				return true;

			File.WriteAllBytes(this.Executable, data);
			this.Log.LogMessage(MessageImportance.High, "Renamed NSURLSessionDelegate to XamUrlSessionDelegate.");
			return true;
		}
		catch (Exception ex)
		{
			this.Log.LogError(ex.Message);
			return false;
		}
	}

	private static Boolean Patch(Byte[] data)
	{
		Boolean changed = false;
		foreach (Int32 slice in RenameUrlSessionClass.Slices(data))
		{
			if (RenameUrlSessionClass.PatchSlice(data, slice))
				changed = true;
		}

		return changed;
	}

	private static List<Int32> Slices(Byte[] data)
	{
		List<Int32> slices = new();
		if (data.Length < 8 || RenameUrlSessionClass.ReadU32Be(data, 0) != RenameUrlSessionClass.fatMagic)
		{
			slices.Add(0);
			return slices;
		}

		UInt32 count = RenameUrlSessionClass.ReadU32Be(data, 4);
		Int32 cursor = 8;
		for (UInt32 index = 0; index < count; index++)
		{
			slices.Add(checked((Int32)RenameUrlSessionClass.ReadU32Be(data, cursor + 8)));
			cursor += 20;
		}

		return slices;
	}

	private static Boolean PatchSlice(Byte[] data, Int32 slice)
	{
		if (slice < 0 || slice > data.Length - 32 ||
		    RenameUrlSessionClass.ReadU32(data, slice) != RenameUrlSessionClass.machHeader64)
			return false;

		List<Segment> segments = new();
		Symtab symtab = RenameUrlSessionClass.ReadLoadCommands(data, slice, segments);
		Dictionary<String, UInt64> symbols = RenameUrlSessionClass.DefinedSymbols(data, slice, symtab);
		if (!symbols.TryGetValue(RenameUrlSessionClass.classSymbol, out UInt64 classVm) ||
		    !symbols.TryGetValue(RenameUrlSessionClass.metaclassSymbol, out UInt64 metaVm))
			return false;

		Int32 classSlot = RenameUrlSessionClass.ClassNameSlot(data, slice, segments, classVm);
		Int32 metaSlot = RenameUrlSessionClass.ClassNameSlot(data, slice, segments, metaVm);
		if (classSlot < 0 && metaSlot < 0)
			return false;
		if (classSlot < 0 || metaSlot < 0)
			throw new InvalidDataException("NSURLSessionDelegate is only partly renamed.");

		Int32 hole = RenameUrlSessionClass.TextPadding(data, slice, segments);
		UInt64 renamedVm = RenameUrlSessionClass.HoleVm(slice, segments, hole);
		Byte[] packed = new Byte[8];
		RenameUrlSessionClass.WriteU64(packed, 0, renamedVm);
		Buffer.BlockCopy(packed, 0, data, classSlot, 8);
		Buffer.BlockCopy(packed, 0, data, metaSlot, 8);
		return true;
	}

	private static Symtab ReadLoadCommands(Byte[] data, Int32 slice, List<Segment> segments)
	{
		UInt32 commands = RenameUrlSessionClass.ReadU32(data, slice + 16);
		Int32 pos = slice + 32;
		Symtab symtab = new();
		for (UInt32 index = 0; index < commands; index++)
		{
			UInt32 command = RenameUrlSessionClass.ReadU32(data, pos);
			UInt32 commandSize = RenameUrlSessionClass.ReadU32(data, pos + 4);
			if (command == RenameUrlSessionClass.segmentCommand64)
				segments.Add(RenameUrlSessionClass.ReadSegment(data, pos));
			else if (command == RenameUrlSessionClass.symtabCommand)
				symtab = RenameUrlSessionClass.ReadSymtab(data, pos);
			pos += checked((Int32)commandSize);
		}

		return symtab;
	}

	private static Segment ReadSegment(Byte[] data, Int32 pos)
	{
		Segment segment = new()
		{
			Name = RenameUrlSessionClass.CString(data, pos + 8, 16),
			VmAddr = RenameUrlSessionClass.ReadU64(data, pos + 24),
			FileOff = RenameUrlSessionClass.ReadU64(data, pos + 40),
			FileSize = RenameUrlSessionClass.ReadU64(data, pos + 48),
		};
		UInt32 sections = RenameUrlSessionClass.ReadU32(data, pos + 64);
		Int32 sectionPos = pos + 72;
		for (UInt32 index = 0; index < sections; index++)
		{
			UInt64 size = RenameUrlSessionClass.ReadU64(data, sectionPos + 40);
			UInt32 offset = RenameUrlSessionClass.ReadU32(data, sectionPos + 48);
			UInt64 end = offset + size;
			if (end > segment.SectionEnd)
				segment.SectionEnd = end;
			segment.HasSections = true;
			sectionPos += 80;
		}

		return segment;
	}

	private static Symtab ReadSymtab(Byte[] data, Int32 pos)
	{
		Symtab symtab = new()
		{
			SymOff = RenameUrlSessionClass.ReadU32(data, pos + 8),
			SymCount = RenameUrlSessionClass.ReadU32(data, pos + 12),
			StrOff = RenameUrlSessionClass.ReadU32(data, pos + 16),
			Present = true,
		};
		return symtab;
	}

	private static Dictionary<String, UInt64> DefinedSymbols(Byte[] data, Int32 slice, Symtab symtab)
	{
		Dictionary<String, UInt64> found = new();
		if (!symtab.Present)
			return found;

		for (UInt32 index = 0; index < symtab.SymCount; index++)
		{
			Int32 entry = slice + checked((Int32)symtab.SymOff) + checked((Int32)index) * 16;
			UInt32 stringIndex = RenameUrlSessionClass.ReadU32(data, entry);
			UInt64 value = RenameUrlSessionClass.ReadU64(data, entry + 8);
			if (value == 0 || stringIndex == 0)
				continue;

			Int32 nameAt = slice + checked((Int32)symtab.StrOff) + checked((Int32)stringIndex);
			String name = RenameUrlSessionClass.CString(data, nameAt, 80);
			if (name == RenameUrlSessionClass.classSymbol || name == RenameUrlSessionClass.metaclassSymbol)
				found[name] = value;
		}

		return found;
	}

	private static Int32 ClassNameSlot(Byte[] data, Int32 slice, List<Segment> segments, UInt64 classVm)
	{
		Int32 classOff = RenameUrlSessionClass.VmToFile(data, slice, segments, classVm);
		UInt64 dataVm = RenameUrlSessionClass.ReadU64(data, classOff + 32) & ~7UL;
		Int32 nameOff = RenameUrlSessionClass.VmToFile(data, slice, segments, dataVm + 24);
		UInt64 current = RenameUrlSessionClass.ReadU64(data, nameOff);
		if (!RenameUrlSessionClass.NameEquals(data, RenameUrlSessionClass.VmToFile(data, slice, segments, current),
		                                      RenameUrlSessionClass.originalName))
			return -1;
		return nameOff;
	}

	private static Int32 TextPadding(Byte[] data, Int32 slice, List<Segment> segments)
	{
		foreach (Segment segment in segments)
		{
			if (segment.Name != "__TEXT" || !segment.HasSections)
				continue;

			UInt64 room = segment.FileOff + segment.FileSize - segment.SectionEnd;
			if (room < (UInt64)(RenameUrlSessionClass.renamedName.Length + 1))
				break;

			Int32 hole = slice + checked((Int32)segment.SectionEnd);
			for (Int32 index = 0; index < RenameUrlSessionClass.renamedName.Length + 1; index++)
			{
				if (data[hole + index] != 0)
					throw new InvalidDataException("__TEXT padding is not empty.");
			}

			Buffer.BlockCopy(RenameUrlSessionClass.renamedName, 0, data, hole,
			                 RenameUrlSessionClass.renamedName.Length);
			data[hole + RenameUrlSessionClass.renamedName.Length] = 0;
			return hole;
		}

		throw new InvalidDataException("No room in __TEXT to store the renamed class.");
	}

	private static UInt64 HoleVm(Int32 slice, List<Segment> segments, Int32 hole)
	{
		foreach (Segment segment in segments)
		{
			if (segment.Name != "__TEXT")
				continue;

			Int64 absolute = slice + (Int64)segment.FileOff;
			if (absolute <= hole && hole < absolute + (Int64)segment.FileSize)
				return segment.VmAddr + (UInt64)(hole - absolute);
		}

		throw new InvalidDataException("Renamed class string is outside __TEXT.");
	}

	private static Int32 VmToFile(Byte[] data, Int32 slice, List<Segment> segments, UInt64 vm)
	{
		foreach (Segment segment in segments)
		{
			if (segment.FileSize == 0 || vm < segment.VmAddr || vm >= segment.VmAddr + segment.FileSize)
				continue;

			Int64 file = slice + (Int64)segment.FileOff + (Int64)(vm - segment.VmAddr);
			if (file < 0 || file > data.Length - 8)
				throw new InvalidDataException("Mach-O address is outside the file.");
			return (Int32)file;
		}

		throw new InvalidDataException("Unmapped Mach-O address.");
	}

	private static Boolean NameEquals(Byte[] data, Int32 offset, Byte[] name)
	{
		if (offset < 0 || offset > data.Length - name.Length - 1)
			return false;
		for (Int32 index = 0; index < name.Length; index++)
		{
			if (data[offset + index] != name[index])
				return false;
		}

		return data[offset + name.Length] == 0;
	}

	private static String CString(Byte[] data, Int32 offset, Int32 max)
	{
		Int32 end = offset;
		Int32 limit = Math.Min(data.Length, offset + max);
		while (end < limit && data[end] != 0)
			end++;
		return Encoding.ASCII.GetString(data, offset, end - offset);
	}

	private static UInt32 ReadU32(Byte[] data, Int32 offset)
		=> data[offset] | ((UInt32)data[offset + 1] << 8) | ((UInt32)data[offset + 2] << 16) |
			((UInt32)data[offset + 3] << 24);

	private static UInt32 ReadU32Be(Byte[] data, Int32 offset)
		=> ((UInt32)data[offset] << 24) | ((UInt32)data[offset + 1] << 16) | ((UInt32)data[offset + 2] << 8) |
			data[offset + 3];

	private static UInt64 ReadU64(Byte[] data, Int32 offset)
		=> RenameUrlSessionClass.ReadU32(data, offset) |
			((UInt64)RenameUrlSessionClass.ReadU32(data, offset + 4) << 32);

	private static void WriteU64(Byte[] data, Int32 offset, UInt64 value)
	{
		for (Int32 index = 0; index < 8; index++)
			data[offset + index] = (Byte)(value >> (8 * index));
	}

	private struct Symtab
	{
		public Boolean Present;
		public UInt32 SymOff;
		public UInt32 SymCount;
		public UInt32 StrOff;
	}

	private sealed class Segment
	{
		public UInt64 FileOff;
		public UInt64 FileSize;
		public Boolean HasSections;
		public String Name;
		public UInt64 SectionEnd;
		public UInt64 VmAddr;
	}
}