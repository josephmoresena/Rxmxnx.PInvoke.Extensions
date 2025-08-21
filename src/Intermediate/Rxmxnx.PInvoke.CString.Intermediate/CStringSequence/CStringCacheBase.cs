using INonGenericEnumerator = System.Collections.IEnumerator;
using INonGenericEnumerable = System.Collections.IEnumerable;

namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// Internal <see cref="CString"/> list.
	/// </summary>
	private abstract class CStringCacheBase : IList<CString?>
	{
		/// <inheritdoc/>
		public virtual Boolean IsReadOnly => true;
		/// <inheritdoc/>
		public abstract Int32 Count { get; }

		/// <inheritdoc/>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		public virtual void Clear() { }

		/// <inheritdoc/>
		public abstract void Insert(Int32 index, CString? item);
		/// <inheritdoc/>
		public abstract void RemoveAt(Int32 index);
		/// <inheritdoc/>
		public abstract CString? this[Int32 index] { get; set; }

#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		void ICollection<CString?>.Add(CString? item) => throw new NotImplementedException();
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		Boolean ICollection<CString?>.Contains(CString? item) => throw new NotImplementedException();
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		void ICollection<CString?>.CopyTo(CString?[] array, Int32 arrayIndex) => throw new NotImplementedException();
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		Boolean ICollection<CString?>.Remove(CString? item) => throw new NotImplementedException();
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		Int32 IList<CString?>.IndexOf(CString? item) => throw new NotImplementedException();
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		IEnumerator<CString?> IEnumerable<CString?>.GetEnumerator() => throw new NotImplementedException();
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		INonGenericEnumerator INonGenericEnumerable.GetEnumerator() => throw new NotImplementedException();
	}
}