// ReSharper disable EmptyNamespace

#if !PACKAGE
[assembly: SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3261)]
#endif
#if !NETCOREAPP3_0_OR_GREATER
namespace System.Runtime.Intrinsics.X86 { }

#if !NETCOREAPP || NETFRAMEWORK
namespace System.Text.Json { }
#if PACKAGE
namespace System.Text.Json.Serialization { }
#endif
#endif
#endif
#if !NET5_0_OR_GREATER
namespace System.Runtime.Intrinsics.Arm { }
#endif
#if !NET7_0_OR_GREATER
namespace System.Runtime.InteropServices.Marshalling { }
#endif