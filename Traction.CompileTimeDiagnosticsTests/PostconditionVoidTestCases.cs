using System.Collections.Generic;
using NUnit.Framework;

namespace Traction.DiagnosticsTests {

    internal static class PostconditionVoidTestCases {
        
        public static IEnumerable<TestCaseData> AllCases {
            get {
                yield return VoidPostconditionTestCase("NonNull", 2); //2nd is for non-reference type
                yield return VoidPostconditionTestCase("Positive", 2); //2nd is for non-comparable type
                yield return VoidPostconditionTestCase("NonDefault", 1);
            }
        }

        private static TestCaseData VoidPostconditionTestCase(string attributeName, int diagnosticsCount) {
            return new TestCaseData(CompilationFactory.CompileClassFromText(
                    SourceCodeFactory.ClassWithMethods(
                        SourceCodeFactory.PostconditionMethod("void", attributeName))))
                 .Returns(new DiagnosticSummary {
                     Count = diagnosticsCount,
                     FirstTitle = "Traction: Invalid contract attribute usage"
                 })
                 .SetName($"PostconditionsCannotBePlacedOnMethodsReturningVoid_{attributeName}");
        }
    }
}
