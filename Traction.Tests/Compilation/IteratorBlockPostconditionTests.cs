using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using static Traction.Contracts.Analysis.DiagnosticCodes;

namespace Traction.Tests.Compilation {

    [TestFixture]
    public class IteratorBlockPostconditionTests {

        private const string fixture = "Compilation_IteratorBlocks_";

        [Test, TestCaseSource(nameof(AllCases))]
        public void Test(CSharpCompilation compilation, bool isValid) {
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

        private static IEnumerable<TestCaseData> AllCases {
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
    }
}
