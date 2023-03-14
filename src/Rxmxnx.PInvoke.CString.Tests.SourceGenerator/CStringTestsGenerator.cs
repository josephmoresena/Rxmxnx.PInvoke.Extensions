using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Rxmxnx.PInvoke.Tests.SourceGenerator
{
    [Generator]
    public class CStringTestsGenerator : ISourceGenerator
    {
        void ISourceGenerator.Execute(GeneratorExecutionContext context)
        {
            StringBuilder strBuild = new StringBuilder();
            strBuild.AppendLine(@"namespace Rxmxnx.PInvoke.Tests;

internal static partial class TestSet
{
    static TestSet()
    {");

            strBuild.AppendLine("\t\tutf16Text = new String[] {");
            foreach (String value in StringSet.Set)
                strBuild.AppendLine($"\t\t\t\"{value}\",");
            strBuild.AppendLine("\t\t};");

            strBuild.AppendLine("\t\tutf8Text = new ReadOnlySpanFunc<Byte>[] {");
            foreach (String value in StringSet.Set)
                strBuild.AppendLine($"\t\t\t() => \"{value}\"u8,");
            strBuild.AppendLine("\t\t};");

            strBuild.AppendLine("\t\tutf8Bytes = new Byte[][] {");
            foreach (String value in StringSet.Set)
                strBuild.AppendLine($"\t\t\tnew Byte[] {{ {String.Join(", ", Encoding.UTF8.GetBytes(value).Select(x => $"{x}"))} }},");
            strBuild.AppendLine("\t\t};");

            strBuild.AppendLine("\t\tutf8NullTerminatedBytes = new Byte[][] {");
            foreach (String value in StringSet.Set)
                strBuild.AppendLine($"\t\t\tnew Byte[] {{ {String.Join(", ", Encoding.UTF8.GetBytes(value).Select(x => $"{x}"))}, 0 }},");
            strBuild.AppendLine("\t\t};");

            strBuild.AppendLine(@"    }
}");
            context.AddSource("TestSet.g.cs", strBuild.ToString());
        }

        void ISourceGenerator.Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            //if (!System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Launch();
#endif
        }
    }
}

