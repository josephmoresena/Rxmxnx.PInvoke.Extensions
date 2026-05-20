// ReSharper disable EmptyNamespace

#if !PACKAGE
[assembly: SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3261)]
#endif
#if !NETCOREAPP
namespace System.Runtime.Intrinsics.X86 { }

namespace System.Text.Json { }
#if PACKAGE
namespace System.Text.Json.Serialization { }

namespace System.Text.Unicode { }
#endif
#endif
#if !NET5_0_OR_GREATER
namespace System.Runtime.Intrinsics.Arm { }
#endif
#if !NET7_0_OR_GREATER
namespace System.Runtime.InteropServices.Marshalling { }
#endif