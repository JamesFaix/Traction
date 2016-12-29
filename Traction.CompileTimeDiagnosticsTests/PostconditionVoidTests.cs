using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace Traction.DiagnosticsTests {

    [TestFixture]
    public class PostconditionVoidTests {

        [Test, TestCaseSource(nameof(AllCases))]
        public void Test(CSharpCompilation compilation, bool isValid) {
            //Arrange/Act
            var diagnostics = TestHelper.GetDiagnostics(compilation);

            //Assert
            if (isValid) {
                Assert.IsFalse(diagnostics.Any());
            }
            else {
                Assert.IsTrue(diagnostics.ContainsCode(DiagnosticCode.NoPostconditionsOnVoid));
            }
        }

        private static IEnumerable<TestCaseData> AllCases {
            get {
                yield return VoidPostconditionTestCase("NonNull", false);
                yield return VoidPostconditionTestCase("Positive", false);
                yield return VoidPostconditionTestCase("NonDefault", false);
            }
        }

        private static TestCaseData VoidPostconditionTestCase(string attributeName, bool isValid) {
            return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        SourceCodeFactory.ClassWithMembers(
                            SourceCodeFactory.PostconditionMethod("void", attributeName))),
                    isValid)
                 .SetName($"PostconditionsCannotBePlacedOnMethodsReturningVoid_{attributeName}");
        }
    }
}
