using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using static Traction.Roslyn.Rewriting.DiagnosticCodes;

namespace Traction.Tests.Compilation {

    [TestFixture]
    public class IteratorBlockPreconditionTests {

        private const string fixture = "Compilation_IteratorBlocks_";

        [Test, TestCaseSource(nameof(Cases))]
        public void Test(string code) {
            //Arrange
            var compilation = CompilationFactory.CompileClassFromText(
                SourceCodeFactory.ClassWithMembers(code));

            //Act
            var diagnostics = TestHelper.GetDiagnostics(compilation);

            Assert.IsTrue(diagnostics.Any(d => d.GetMessage().StartsWith("Expanded")));
            Assert.IsTrue(diagnostics.ContainsOnlyCode(RewriteConfirmed));
        }

        private static IEnumerable<TestCaseData> Cases {
            get {
                yield return new TestCaseData(
                    "IEnumerable<int> GetNumbers([NonNull] string path) " +
                    "{ yield return 1; }")
                    .SetName($"{fixture}_RewritePerformed");
            }
        }
    }
}
