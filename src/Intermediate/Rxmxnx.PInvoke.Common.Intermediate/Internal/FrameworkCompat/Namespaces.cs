#if !PACKAGE
[assembly: SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3261)]
#endif
#if NETSTANDARD2_1
namespace System.Runtime.Intrinsics.X86 { }
#endif
#if !NET5_0_OR_GREATER
namespace System.Runtime.Intrinsics.Arm { }
#endif
#if !NET7_0_OR_GREATER
namespace System.Runtime.InteropServices.Marshalling { }
#endif