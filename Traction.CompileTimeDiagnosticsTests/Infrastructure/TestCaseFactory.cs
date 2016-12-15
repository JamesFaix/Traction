using NUnit.Framework;

namespace Traction.DiagnosticsTests {

    static class TestCaseFactory {

        public static TestCaseData CreatePreconditionInvalidTypeTestCase(string typeName, string attributeName, bool shouldHaveDiagnostic) =>
            new TestCaseData((CompilationGenerator)(() =>
                    CompilationFactory.SingleClassCompilation(
                        MemberFactory.MethodWithPrecondition(typeName, attributeName))))
                .Returns(new DiagnosticSummary {
                    Count = shouldHaveDiagnostic ? 1 : 0,
                    FirstMessage = shouldHaveDiagnostic ? "Traction: Invalid contract attribute usage" : null
                })
                .SetName($"{attributeName}_CanOnlyBePlacedOnParametersWithValidTypes" +
                    $"_{typeName}{(shouldHaveDiagnostic ? "Fails" : "Passes")}");

        public static TestCaseData CreatePostconditionInvalidTypeTestCase(string typeName, string attributeName, bool shouldHaveDiagnostic) =>
            new TestCaseData((CompilationGenerator)(() =>
                    CompilationFactory.SingleClassCompilation(
                        MemberFactory.MethodWithPostcondition(typeName, attributeName))))
                .Returns(new DiagnosticSummary {
                    Count = shouldHaveDiagnostic ? 1 : 0,
                    FirstMessage = shouldHaveDiagnostic ? "Traction: Invalid contract attribute usage" : null
                })
                .SetName($"{attributeName}_CanOnlyBePlacedOnReturnValuesWithValidTypes" +
                    $"_{typeName}{(shouldHaveDiagnostic ? "Fails" : "Passes")}");
    }
}