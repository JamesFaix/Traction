using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_IteratorBlocks {

        private const string fixture = nameof(Runtime_IteratorBlocks) + "_";

        [Test, TestCaseSource(nameof(Cases))]
        public void Test(string sourceCode, object[] arguments, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("TestMethod");

            CustomAssert.Throws(exceptionType, method, instance, arguments);
        }

        private static string GetSnippet(bool hasContract, bool hasLegacyContract) {
            var sb = new StringBuilder();
            sb.AppendLine("class TestClass {");

            sb.Append("public IEnumerable<int> TestMethod(");
            sb.AppendIf(hasContract, "[NonNull]");
            sb.AppendLine("string text) {");
            sb.AppendLineIf(hasLegacyContract, "if (text == null) throw new ArgumentNullException(nameof(text));");
            sb.AppendLine("yield return 1; } }");
            return sb.ToString();
        }

        private static IEnumerable<TestCaseData> Cases {
            get {
                yield return new TestCaseData(
                    GetSnippet(false, false),
                    new object[] { null },
                    null)
                .SetName($"{fixture}NormalMethod_{Constants.Normal}");

                yield return new TestCaseData(
                    GetSnippet(true, false),
                    new object[] { "test" },
                    null)
                .SetName($"{fixture}PreconditionMethod_{Constants.Passes}");

                yield return new TestCaseData(
                    GetSnippet(true, false),
                    new object[] { null },
                    typeof(PreconditionException))
                .SetName($"{fixture}PreconditionMethod_{Constants.Fails}Immediately");

                yield return new TestCaseData(
                    GetSnippet(false, true),
                    new object[] { null },
                    null)
                .SetName($"{fixture}LegacyPreconditionMethod_DoesNotImmediatelyFailIfContractBroken");
            }
        }
    }
}