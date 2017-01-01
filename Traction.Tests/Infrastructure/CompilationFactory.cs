using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Traction.Tests {

    static class CompilationFactory {
        
        public static CSharpCompilation CompileClassFromText(string classText) {
            return CSharpCompilation.Create("TestAssembly")
               .AddReferences(References)
               .AddSyntaxTrees(CSharpSyntaxTree.ParseText(
                   SourceCodeFactory.BasicUsingDirectives + classText));
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
