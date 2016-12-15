using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace Traction.DiagnosticsTests {

    [TestFixture]
    public class PartialMemberTests {

        [Test, TestCaseSource(nameof(AllCases))]
        public void DiagnosticSummaryTest(CSharpCompilation compilation, bool isValid) {
            //Arrange/Act
            var diagnostics = TestHelper.GetDiagnostics(compilation);

            //Assert
            if (isValid) {
                Assert.IsFalse(diagnostics.Any());
            }
            else {
                Assert.AreEqual(1, diagnostics.Count());
                Assert.IsTrue(diagnostics.Any(d => d.Id == "TR0003"));
            }
        }

        private static IEnumerable<TestCaseData> AllCases {
            get {
                yield return PreconditionTestCase("NonNull", false);
                yield return PreconditionTestCase("Positive", false);
                yield return PreconditionTestCase("NonDefault", false);
                yield return PostconditionTestCase("NonNull", false);
                yield return PostconditionTestCase("Positive", false);
                yield return PostconditionTestCase("NonDefault", false);
            }
        }

        private static TestCaseData PostconditionTestCase(string attributeName, bool isValid) {
            return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        SourceCodeFactory.ClassWithMembers(new[] { "partial" },
                            $"[return: {attributeName}] partial void TestMethod();")),
                    isValid)
                 .SetName($"ContractCannotBePlacedOnPartialMembers_{attributeName}_Postcondition");
        }

        private static TestCaseData PreconditionTestCase(string attributeName, bool isValid) {
            return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        SourceCodeFactory.ClassWithMembers(new[] { "partial" },
                            $"partial void TestMethod([{attributeName}] int param1);")),
                    isValid)
                 .SetName($"ContractCannotBePlacedOnPartialMembers_{attributeName}_Precondition");
        }
    }
}
