using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using static Traction.Contracts.Analysis.DiagnosticCodes;
using static Traction.Roslyn.Rewriting.DiagnosticCodes;

namespace Traction.Tests.Compilation {

    [TestFixture]
    public class PartialMemberTests {

        private const string fixture = "Compilation_PartialMembers_";

        [Test, TestCaseSource(nameof(AllCases))]
        public void Test(CSharpCompilation compilation, bool isValid) {
            //Arrange/Act
            var diagnostics = TestHelper.GetDiagnostics(compilation);

            //Assert
            if (isValid) {
                Assert.IsFalse(diagnostics.ContainsOnlyCode(RewriteConfirmed));
            }
            else {
                Assert.IsTrue(diagnostics.ContainsCode(NoContractsOnPartialMembers));
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
                            $"[return: {attributeName}] partial int TestMethod();")),
                    isValid)
                 .SetName($"{fixture}NoContracts_{attributeName}_Postcondition");
        }

        private static TestCaseData PreconditionTestCase(string attributeName, bool isValid) {
            return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        SourceCodeFactory.ClassWithMembers(new[] { "partial" },
                            $"partial void TestMethod([{attributeName}] int param1);")),
                    isValid)
                 .SetName($"{fixture}NoContracts_{attributeName}_Precondition");
        }
    }
}
