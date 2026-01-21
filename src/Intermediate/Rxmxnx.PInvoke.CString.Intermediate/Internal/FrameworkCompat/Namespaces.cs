#if !PACKAGE && !NET7_0_OR_GREATER
// ReSharper disable EmptyNamespace
[assembly: SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3261)]

namespace System.Runtime.InteropServices.Marshalling { }
#if !NETCOREAPP
namespace System.Text.Json { }

namespace System.Text.Json.Serialization { }
#endif
#endif