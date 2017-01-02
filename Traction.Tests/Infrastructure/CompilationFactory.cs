using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Traction.Tests {

    static class CompilationFactory {

        public static CSharpCompilation CompileClassFromText(string classText) {
            if (classText == null) throw new ArgumentNullException(nameof(classText));

            return CSharpCompilation.Create(
                assemblyName: "TestAssembly",
                references: References,
                syntaxTrees: new[] { CSharpSyntaxTree.ParseText(
                   SourceCodeFactory.BasicUsingDirectives + classText)},
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }

        private static string runtimePath =
            @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\";

        private static MetadataReference[] References =>
            new[] {
                MetadataReference.CreateFromFile(runtimePath + "mscorlib.dll"),
                MetadataReference.CreateFromFile(runtimePath + "System.dll"),
                MetadataReference.CreateFromFile(runtimePath + "System.Core.dll"),
                MetadataReference.CreateFromFile(typeof(NonNullAttribute).Assembly.Location)
            };
    }
}
