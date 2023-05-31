using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;

namespace Rxmxnx.PInvoke.Tests.SourceGenerator
{
    [Generator]
    [ExcludeFromCodeCoverage]
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
            CreateUtf16Text(strBuild);
            CreateUtf8Text(strBuild);
            CreateUtf8Bytes(strBuild);
            CreateUtf8NullTerminatedBytes(strBuild);

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

        private static void CreateUtf16Text(StringBuilder strBuild)
        {
            strBuild.AppendLine("\t\tutf16Text = new String[] {");
            foreach (String value in StringSet.Set)
                strBuild.AppendLine($"\t\t\t\"{value}\",");
            strBuild.AppendLine("\t\t};");

            strBuild.AppendLine("\t\tutf16TextLower = new String[] {");
            foreach (String value in StringSet.Set)
                strBuild.AppendLine($"\t\t\t\"{value.ToLowerInvariant()}\",");
            strBuild.AppendLine("\t\t};");

            strBuild.AppendLine("\t\tutf16TextUpper = new String[] {");
            foreach (String value in StringSet.Set)
                strBuild.AppendLine($"\t\t\t\"{value.ToUpperInvariant()}\",");
            strBuild.AppendLine("\t\t};");
        }
        private static void CreateUtf8Text(StringBuilder strBuild)
        {
            strBuild.AppendLine("\t\tutf8Text = new ReadOnlySpanFunc<Byte>[] {");
            foreach (String value in StringSet.Set)
                strBuild.AppendLine($"\t\t\t() => \"{value}\"u8,");
            strBuild.AppendLine("\t\t};");

            strBuild.AppendLine("\t\tutf8TextLower = new ReadOnlySpanFunc<Byte>[] {");
            foreach (String value in StringSet.Set)
                strBuild.AppendLine($"\t\t\t() => \"{value.ToLowerInvariant()}\"u8,");
            strBuild.AppendLine("\t\t};");

            strBuild.AppendLine("\t\tutf8TextUpper = new ReadOnlySpanFunc<Byte>[] {");
            foreach (String value in StringSet.Set)
                strBuild.AppendLine($"\t\t\t() => \"{value.ToUpperInvariant()}\"u8,");
            strBuild.AppendLine("\t\t};");
        }
        private static void CreateUtf8Bytes(StringBuilder strBuild)
        {
            strBuild.AppendLine("\t\tutf8Bytes = new Byte[][] {");
            foreach (String value in StringSet.Set)
                strBuild.AppendLine($"\t\t\tnew Byte[] {{ {String.Join(", ", Encoding.UTF8.GetBytes(value).Select(x => $"{x}"))} }},");
            strBuild.AppendLine("\t\t};");
        }
        private static void CreateUtf8NullTerminatedBytes(StringBuilder strBuild)
        {
            strBuild.AppendLine("\t\tutf8NullTerminatedBytes = new Byte[][] {");
            foreach (String value in StringSet.Set)
                strBuild.AppendLine($"\t\t\tnew Byte[] {{ {String.Join(", ", Encoding.UTF8.GetBytes(value).Select(x => $"{x}"))}, 0 }},");
            strBuild.AppendLine("\t\t};");
        }
    }
}

