using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace Traction.DiagnosticsTests {

    [TestFixture]
    public class IteratorBlockPostconditionTests {
        
        [Test, TestCaseSource(nameof(IteratorBlockCases))]
        public DiagnosticSummary IteratorBlockPostconditionsTest(CompilationGenerator generator) {
            //Arrange
            var context = ContextFactory.Before(generator);
            var module = new ContractModule();

            //Act
            module.BeforeCompile(context);

            //Assert
            return new DiagnosticSummary {
                Count = context.Diagnostics.Count(),
                FirstTitle = context.Diagnostics.FirstOrDefault()
                                    ?.Descriptor.Title.ToString()
            };
        }

        private static IEnumerable<TestCaseData> IteratorBlockCases {
            get {
                yield return IteratorBlockCase("NonNull", 1);
                yield return IteratorBlockCase("Positive", 2); //2nd is for non-comparable type
                yield return IteratorBlockCase("NonDefault", 1);
            }
        }

        private static TestCaseData IteratorBlockCase(string attributeName, int diagnosticsCount) {
            return new TestCaseData((CompilationGenerator)(() =>
                     CompilationFactory.SingleClassCompilation(
                         MemberFactory.MethodWithPostcondition("IEnumerable<int>", attributeName)
                             .AddBodyStatements(SyntaxFactory.ParseStatement("yield return 1;")))))
                 .Returns(new DiagnosticSummary {
                     Count = diagnosticsCount,
                     FirstTitle = "Traction: Invalid contract attribute usage"
                 })
                 .SetName($"PostconditionsCannotBePlacedOnIteratorBlocks_{attributeName}");
        }
    }
}
