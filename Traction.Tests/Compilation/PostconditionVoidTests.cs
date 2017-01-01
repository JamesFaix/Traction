using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using static Traction.Contracts.Analysis.DiagnosticCodes;

namespace Traction.Tests.Compilation {

    [TestFixture]
    public class PostconditionVoidTests {

        private const string fixture = "Compilation_Void_";

        [Test, TestCaseSource(nameof(AllCases))]
        public void Test(CSharpCompilation compilation, bool isValid) {
            //Arrange/Act
            var diagnostics = TestHelper.GetDiagnostics(compilation);

            //Assert
            if (isValid) {
                Assert.IsFalse(diagnostics.Any());
            }
            else {
                Assert.IsTrue(diagnostics.ContainsCode(NoPostconditionsOnVoid));
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
                 .SetName($"{fixture}NoPostconditions_{attributeName}");
        }
    }
}
