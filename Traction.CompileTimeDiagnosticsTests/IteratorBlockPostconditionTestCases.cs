using System.Collections.Generic;
using NUnit.Framework;

namespace Traction.DiagnosticsTests {

    internal static class IteratorBlockPostconditionTestCases {

        public static IEnumerable<TestCaseData> AllCases {
            get {
                yield return IteratorBlockCase("NonNull", 1);
                yield return IteratorBlockCase("Positive", 2); //2nd is for non-comparable type
                yield return IteratorBlockCase("NonDefault", 1);
            }
        }

        private static TestCaseData IteratorBlockCase(string attributeName, int diagnosticsCount) {
            return new TestCaseData(CompilationFactory.CompileClassFromText(
                    SourceCodeFactory.ClassWithMethods(
                        $"[return: {attributeName}] IEnumerable<int> TestMethod() {{ yield return 1; }}")))
                 .Returns(new DiagnosticSummary {
                     Count = diagnosticsCount,
                     FirstTitle = "Traction: Invalid contract attribute usage"
                 })
                 .SetName($"PostconditionsCannotBePlacedOnIteratorBlocks_{attributeName}");
        }
    }
}
