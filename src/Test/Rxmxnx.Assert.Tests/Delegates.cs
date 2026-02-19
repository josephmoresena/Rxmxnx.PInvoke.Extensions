namespace Rxmxnx.PInvoke.Tests;

public delegate String GetStringDelegate();
#if NETCOREAPP
public delegate Span<Byte> GetByteSpanDelegate(Byte[] bytes);
public delegate Span<Guid> GetGuidSpanDelegate();
#endif
public delegate void VoidDelegate();
public delegate void VoidObjectDelegate(Object obj);
public delegate ReadOnlySpan<T> GetSpanDelegate<T>();