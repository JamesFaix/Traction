using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using StackExchange.Precompilation;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Traction.DiagnosticsTests {

    [TestFixture]
    public class NonNull_CompilationDiagnosticsTest {

        [Test, TestCaseSource(nameof(NonNullAttribute_CreatesCorrectDiagnostics_Cases))]
        public DiagnosticSummary NonNullAttribute_CreatesCorrectDiagnostics(Func<CSharpCompilation> compilationGenerator) {
            //Arrange
            var context = new BeforeCompileContext {
                Compilation = compilationGenerator(),
                Diagnostics = new List<Diagnostic>()
            };
            var module = new ContractModule();

            //Act
            module.BeforeCompile(context);

            var result = new DiagnosticSummary {
                Count = context.Diagnostics.Count(),
                FirstMessage = context.Diagnostics.FirstOrDefault()
                                    ?.Descriptor.Title.ToString()
            };

            //Assert
            return result;
        }

        private static IEnumerable<TestCaseData> NonNullAttribute_CreatesCorrectDiagnostics_Cases =>
            new TestCaseData[] {
                new TestCaseData(
                    (Func<CSharpCompilation>)(() => TestCompilation_NonNullOnParameter("string"))
                ).Returns(new DiagnosticSummary {
                    Count = 0,
                    FirstMessage = null
                }).SetName(nameof(NonNullAttribute_CreatesCorrectDiagnostics) + "_StringPasses"),

                new TestCaseData(
                    (Func<CSharpCompilation>)(() => TestCompilation_NonNullOnParameter("IDisposable"))
                ).Returns(new DiagnosticSummary {
                    Count = 0,
                    FirstMessage = null
                }).SetName(nameof(NonNullAttribute_CreatesCorrectDiagnostics) + "_IDisposablePasses"),

                new TestCaseData(
                    (Func<CSharpCompilation>)(() => TestCompilation_NonNullOnParameter("int"))
                ).Returns(new DiagnosticSummary {
                    Count = 1,
                    FirstMessage = "Traction: Invalid contract attribute usage"
                }).SetName(nameof(NonNullAttribute_CreatesCorrectDiagnostics) + "_Int32Fails"),
                
                new TestCaseData(
                    (Func<CSharpCompilation>)(() => TestCompilation_NonNullOnParameter("DateTime"))
                ).Returns(new DiagnosticSummary {
                    Count = 1,
                    FirstMessage = "Traction: Invalid contract attribute usage"
                }).SetName(nameof(NonNullAttribute_CreatesCorrectDiagnostics) + "_DateTimeFails"),
                
                new TestCaseData(
                    (Func<CSharpCompilation>)(() => TestCompilation_NonNullOnParameter("int?"))
                ).Returns(new DiagnosticSummary {
                    Count = 1,
                    FirstMessage = "Traction: Invalid contract attribute usage"
                }).SetName(nameof(NonNullAttribute_CreatesCorrectDiagnostics) + "_NullableInt32Fails"),

            };

        //Make sure there are spaces before the member name, parameter names, and parameter types.
        private static CSharpCompilation TestCompilation_NonNullOnParameter(string parameterType) {
            return CreateCompilation(
                MethodDeclaration(ParseTypeName("void"), Identifier(" TestMethod"))
                .AddParameterListParameters(
                    Parameter(Identifier(" param1"))
                        .WithType(ParseTypeName(" " + parameterType))
                        .AddAttributeLists(AttributeList()
                                            .AddAttributes(Attribute(ParseName("NonNull")))))
                .WithBody(Block()));
        }

        #region Basic compilation

        //Make sure to include Using directives
        private static CSharpCompilation CreateCompilation(params MemberDeclarationSyntax[] members) {

            return CSharpCompilation.Create("TestAssembly")
               .AddReferences(References)
               .AddSyntaxTrees(CSharpSyntaxTree.Create(CompilationUnit()
                    .AddUsings(
                       UsingDirective(ParseName(" System")),
                       UsingDirective(ParseName(" Traction")))
                    .AddMembers(ClassDeclaration(Identifier(" TestClass"))
                                .AddMembers(members))));
        }

        private static string runtimePath = @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\";

        private static MetadataReference[] References =>
            new[] {
                MetadataReference.CreateFromFile(runtimePath + "mscorlib.dll"),
                MetadataReference.CreateFromFile(runtimePath + "System.dll"),
                MetadataReference.CreateFromFile(runtimePath + "System.Core.dll"),
                MetadataReference.CreateFromFile(typeof(NonNullAttribute).Assembly.Location)
            };

        #endregion
    }
}
