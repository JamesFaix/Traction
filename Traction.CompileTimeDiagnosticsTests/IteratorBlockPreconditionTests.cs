using System.Linq;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using static Traction.Roslyn.Rewriting.DiagnosticCodes;

namespace Traction.DiagnosticsTests {

    [TestFixture]
    public class IteratorBlockPreconditionTests {

        [Test]
        public void IteratorBlockPreconditions_RewritePerformed() {
            //Arrange
            var compilation = CompilationFactory.CompileClassFromText(
                SourceCodeFactory.ClassWithMembers(
                    "IEnumerable<int> GetNumbers([NonNull] string path) " +
                    "{ yield return 1; }"));

            //Act
            var diagnostics = TestHelper.GetDiagnostics(compilation);

            Assert.IsTrue(diagnostics.Any(d => d.GetMessage().StartsWith("Expanded")));
            Assert.IsTrue(diagnostics.ContainsOnlyCode(RewriteConfirmed));
        }
    }
}
