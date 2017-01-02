using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using static Traction.Contracts.Analysis.DiagnosticCodes;
using static Traction.Roslyn.Rewriting.DiagnosticCodes;

namespace Traction.Tests.Compilation {

    [TestFixture]
    public class Compilation_IteratorBlocks {

        private const string fixture = nameof(Compilation_IteratorBlocks) + "_";

        [Test, TestCaseSource(nameof(PostconditionCases))]
        public void PostconditionTest(CSharpCompilation compilation, bool isValid) {
            //Arrange/Act
            var diagnostics = TestHelper.GetDiagnostics(compilation);

            //Assert
            if (isValid) {
                Assert.IsFalse(diagnostics.Any());
            }
            else {
                Assert.IsTrue(diagnostics.ContainsCode(NoPostconditionsOnIteratorBlocks));
            }
        }

        private static IEnumerable<TestCaseData> PostconditionCases {
            get {
                yield return IteratorBlockCase("NonNull", false);
                yield return IteratorBlockCase("Positive", false);
                yield return IteratorBlockCase("NonDefault", false);
            }
        }

        private static TestCaseData IteratorBlockCase(string attributeName, bool isValid) {
            return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        SourceCodeFactory.ClassWithMembers(
                            $"[return: {attributeName}] IEnumerable<int> TestMethod() {{ yield return 1; }}")),
                    isValid)
                 .SetName($"{fixture}NoPostconditions_{attributeName}");
        }

        [Test, TestCaseSource(nameof(PreconditionCases))]
        public void PreconditionTest(string code) {
            //Arrange
            var compilation = CompilationFactory.CompileClassFromText(
                SourceCodeFactory.ClassWithMembers(code));

            //Act
            var diagnostics = TestHelper.GetDiagnostics(compilation);

            Assert.IsTrue(diagnostics.Any(d => d.GetMessage().StartsWith("Expanded")));
            Assert.IsTrue(diagnostics.ContainsOnlyCode(RewriteConfirmed));
        }

        private static IEnumerable<TestCaseData> PreconditionCases {
            get {
                yield return new TestCaseData(
                    "IEnumerable<int> GetNumbers([NonNull] string path) " +
                    "{ yield return 1; }")
                    .SetName($"{fixture}_RewritePerformed");
            }
        }
    }
}
