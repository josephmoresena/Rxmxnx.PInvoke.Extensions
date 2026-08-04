using System.Runtime.CompilerServices;

namespace Rxmxnx.PInvoke.ApplicationTest;

public static class RefStructHelper
{
	public static void PointerFeature<T>(Span<T> span, Boolean inStack, TextWriter writer)
	{
		ref Span<T> refSpan = ref span;
		ValPtr<Span<T>> valRefSpan = NativeUtilities.GetUnsafeValPtrFromRef(ref span);

		writer.WriteLine($"Unsafe Span Address: 0x{span.GetUnsafeIntPtr():x8}");
#pragma warning disable CS0612
		NativeUtilities.WithSafeFixed(ref refSpan, (inStack, writer), RefStructHelper.UseFixedRefSpan);
#pragma warning restore CS0612
		writer.WriteLine($"Span Pointer: 0x{valRefSpan.Pointer:x8}");
		writer.WriteLine($"Ref Span vs Span Pointer: {Unsafe.AreSame(ref refSpan, ref valRefSpan.Reference)}");
	}
	public static void PointerFeature<T>(ReadOnlySpan<T> span, Boolean inStack, TextWriter writer)
	{
		ref ReadOnlySpan<T> refSpan = ref span;
		ValPtr<ReadOnlySpan<T>> valRefSpan = NativeUtilities.GetUnsafeValPtrFromRef(ref span);

		writer.WriteLine($"Unsafe Read-only Span Address: 0x{span.GetUnsafeIntPtr():x8}");
#pragma warning disable CS0612
		NativeUtilities.WithSafeFixed(ref refSpan, (inStack, writer), RefStructHelper.UseFixedRefSpan);
#pragma warning restore CS0612
		writer.WriteLine($"Read-only Span Pointer: 0x{valRefSpan.Pointer:x8}");
		writer.WriteLine(
			$"Ref Read-only Span vs Read-only Span Pointer: {Unsafe.AreSame(ref refSpan, ref valRefSpan.Reference)}");
	}

	[Obsolete]
	private static void UseFixedRefSpan<T>(in IFixedReference<Span<T>> frs, (Boolean inStack, TextWriter writer) val)
	{
		ReadOnlyValPtr<Span<T>> spanPtr = NativeUtilities.GetUnsafeValPtr(in frs.Reference);

		val.writer.WriteLine($"Span Reference Length: {frs.Reference.Length}");
		val.writer.WriteLine($"Unsafe Span Read-only Pointer: 0x{spanPtr.Pointer:x8}");
		val.writer.WriteLine(
			$"Span Fixed Reference vs Span Read-only Pointer: {Unsafe.AreSame(ref frs.Reference, in spanPtr.Reference)}");
		if (!val.inStack) return;

		ValPtr<T> valPtr = spanPtr.Reference.GetUnsafeValPtr();
		using IFixedContext<T>.IDisposable f = valPtr.GetUnsafeFixedContext(spanPtr.Reference.Length);
		val.writer.WriteLine($"Unsafe Span Memory Pointer: 0x{f.Pointer:x8}");
	}
	[Obsolete]
	private static void UseFixedRefSpan<T>(in IFixedReference<ReadOnlySpan<T>> frs,
		(Boolean inStack, TextWriter writer) val)
	{
		ReadOnlyValPtr<ReadOnlySpan<T>> spanPtr = NativeUtilities.GetUnsafeValPtr(in frs.Reference);

		val.writer.WriteLine($"Span Reference Length: {frs.Reference.Length}");
		val.writer.WriteLine($"Unsafe Read-only Span Read-only Pointer: 0x{spanPtr.Pointer:x8}");
		val.writer.WriteLine(
			$"Read-only Span Fixed Reference vs Read-only Span Read-only Pointer: {Unsafe.AreSame(ref frs.Reference, in spanPtr.Reference)}");
		if (!val.inStack) return;

		ReadOnlyValPtr<T> valPtr = spanPtr.Reference.GetUnsafeValPtr();
		using IReadOnlyFixedContext<T>.IDisposable f = valPtr.GetUnsafeFixedContext(spanPtr.Reference.Length);
		val.writer.WriteLine($"Unsafe Read-only Span Memory Pointer: 0x{f.Pointer:x8}");
	}
}