using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Traction.DiagnosticsTests {

    static class CompilationFactory {
        
        public static CSharpCompilation SingleClassCompilation(params MemberDeclarationSyntax[] members) {
            if (members == null) throw new ArgumentNullException(nameof(members));

            return CSharpCompilation.Create("TestAssembly")
               .AddReferences(References)
               .AddSyntaxTrees(CSharpSyntaxTree.Create(CompilationUnit()
                    .AddUsings(Usings)
                    .AddMembers(ClassDeclaration(Identifier(" TestClass"))
                                .AddMembers(members))));
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

        private static UsingDirectiveSyntax[] Usings =>
            new[] {
                UsingDirective(ParseName(" System")),
                UsingDirective(ParseName(" System.Collections.Generic")),
                UsingDirective(ParseName(" System.Linq")),
                UsingDirective(ParseName(" Traction"))
            };
    }
}
