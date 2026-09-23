namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// A stack-only view over the UTF-8 items on a <see cref="CStringSequence"/>, with control over empty entries.
	/// </summary>
	[Preserve(AllMembers = true)]
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(CStringSequenceDebugView))]
	[StructLayout(LayoutKind.Sequential)]
#if NET7_0_OR_GREATER
	[NativeMarshalling(typeof(InputMarshaller))]
#endif
	public readonly ref partial struct Utf8View
	{
		/// <summary>
		/// Internal <see cref="CStringSequence"/> instance.
		/// </summary>
		private readonly CStringSequence? _instance;
		/// <summary>
		/// Indicates whether the current enumeration is only for non-empty items.
		/// </summary>
		private readonly Boolean _excludeEmptyItems;

		/// <summary>
		/// Enumeration source sequence.
		/// </summary>
		// ReSharper disable once ConvertToAutoPropertyWhenPossible
		public CStringSequence? Source => this._instance;
		/// <summary>
		/// Indicates whether the current enumeration includes empty items from the source sequence.
		/// </summary>
		public Boolean EmptyItemsIncluded => !this._excludeEmptyItems;
		/// <summary>
		/// Gets the number of elements in the current enumeration.
		/// </summary>
		public Int32 Count
			=> (!this._excludeEmptyItems ? this._instance?.Count : this._instance?.NonEmptyCount).GetValueOrDefault();

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="instance">A <see cref="CStringSequence"/> instance.</param>
		/// <param name="includeEmptyItems">Specifies whether empty items should be included in the enumeration.</param>
		internal Utf8View(CStringSequence? instance, Boolean includeEmptyItems)
		{
			this._instance = instance;
			this._excludeEmptyItems = !includeEmptyItems;
		}

		/// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
		public Enumerator GetEnumerator()
			=> this._instance is not null ? new(this._instance, this._excludeEmptyItems) : default;

		/// <summary>
		/// Creates an array of <see cref="CString"/> from current instance.
		/// </summary>
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		internal CString[] ToArray()
		{
			if (this._instance is null or { Count: 0, }) return [];
			if (!this._excludeEmptyItems) return [.. this._instance,];

			CString[] result = new CString[this._instance.NonEmptyCount];
			ref CString item = ref MemoryMarshal.GetReference(result.AsSpan());
			for (Int32 index = 0; index < this._instance._lengths.Length; index++)
			{
				Int32 length = this._instance._lengths[index];
				if (length <= 0) continue;
				item = this._instance[index];
				item = ref Unsafe.Add(ref item, 1);
			}
			return result;
		}
	}
}