using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace Traction.DiagnosticsTests {

    [TestFixture]
    public class AbstractMemberTests {

        [Test, TestCaseSource(nameof(AllCases))]
        public void DiagnosticSummaryTest(CSharpCompilation compilation) {
            //Arrange/Act
            var diagnostics = TestHelper.GetDiagnostics(compilation);

            //Assert
            Assert.IsFalse(diagnostics.Any());
        }

        private static IEnumerable<TestCaseData> AllCases {
            get {
                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        "abstract class TestClass {\n" +
                            "[return: NonNull] abstract string TestMethod();\n" +
                        "}"))
                    .SetName($"AbstractMethodsCompileWithoutError");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        "abstract class TestClass {\n" +
                            "[NonNull] abstract string TestProperty { get; }\n" +
                        "}"))
                    .SetName($"AbstractPropertiesCompileWithoutError");
                
                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        "interface ITest {\n" +
                            "[return: NonNull] string TestMethod();\n" +
                        "}"))
                    .SetName($"InterfaceMethodsCompileWithoutError");

                yield return new TestCaseData(
                    CompilationFactory.CompileClassFromText(
                        "interface ITest {\n" +
                            "[return: NonNull] string TestProperty { get; }\n" +
                        "}"))
                    .SetName($"InterfacePropertiesCompileWithoutError");
            }
        }
    }
}
