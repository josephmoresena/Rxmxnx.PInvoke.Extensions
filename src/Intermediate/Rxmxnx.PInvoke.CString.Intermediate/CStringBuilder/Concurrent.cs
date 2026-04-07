#if !NET9_0_OR_GREATER
using Lock = System.Object;
#endif

namespace Rxmxnx.PInvoke;

public partial class CStringBuilder
{
	/// <summary>
	/// Represents a concurrent <see cref="CStringBuilder"/> instance.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private readonly partial struct Concurrent(Lock lockObj, CStringBuilder builder)
	{
		/// <summary>
		/// Gets the length of the current instance object.
		/// </summary>
		public Int32 Length
		{
			get
			{
#if NET9_0_OR_GREATER
				using (lockObj.EnterScope())
#else
				lock (lockObj)
#endif
					return builder.Length;
			}
		}

		/// <summary>
		/// Removes all units from the current <see cref="CStringBuilder"/> instance.
		/// </summary>
		/// <returns>A reference to this instance after the clear operation has completed.</returns>
		public CStringBuilder Clear()
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
				return builder.Clear();
		}
		/// <summary>
		/// Removes the specified range of UTF-8 from this instance.
		/// </summary>
		/// <param name="startIndex">The zero-based position in this instance where removal begins.</param>
		/// <param name="length">The number of UTF-8 units to remove.</param>
		/// <returns>A reference to this instance after the remove operation has completed.</returns>
		public CStringBuilder Remove(Int32 startIndex, Int32 length)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
				return builder.Remove(startIndex, length);
		}
		/// <summary>
		/// Copies UTF-8 units starting at the specified index of this instance into the provided destination
		/// <see cref="Byte"/> span and returns the total number of units actually copied.
		/// </summary>
		/// <param name="index">
		/// The zero-based starting position in this instance from which UTF-8 units will be copied.
		/// </param>
		/// <param name="destination">
		/// The destination span that receives the copied UTF-8 units. The number of units copied is limited
		/// by the length of this span.
		/// </param>
		/// <returns>
		/// The number of UTF-8 units copied to <paramref name="destination"/>. This value is the lesser of
		/// the available UTF-8 units starting at <paramref name="index"/> and the length of the destination span.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		public Int32 CopyTo(Int32 index, Span<Byte> destination)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
				return builder.CopyTo(index, destination);
		}
		/// <summary>
		/// Converts the value of builder instance to a <see cref="CString"/>.
		/// </summary>
		/// <param name="nullTerminated">
		/// Indicates whether the resulting <see cref="CString"/> is null-terminated.
		/// </param>
		/// <returns>A <see cref="CString"/> whose value is the same as builder instance.</returns>
		public CString ToCString(Boolean nullTerminated)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
				return builder.ToCString(nullTerminated);
		}
	}
}