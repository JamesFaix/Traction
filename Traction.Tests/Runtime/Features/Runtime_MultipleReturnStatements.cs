using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using static Traction.Tests.Constants;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_MultipleReturnStatements {

        private const string FIXTURE = nameof(Runtime_MultipleReturnStatements) + "_";

        [Test, TestCaseSource(nameof(Cases))]
        public void Tests(string sourceCode, object[] arguments, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("TestMethod");

            CustomAssert.Throws(exceptionType, method, instance, arguments);
        }

        private static string GetSnippet(bool hasPost) {
            var sb = new StringBuilder();
            sb.AppendLine("class TestClass {");

            sb.AppendLineIf(hasPost, "[return: NonNull]");
            sb.Append("public string TestMethod(");
            sb.AppendLine("string arg1, string arg2, bool useArg1) {");
            sb.AppendLine("if (useArg1) { return arg1; }");
            sb.AppendLine("else { return arg2; }");
            sb.AppendLine("}");

            sb.AppendLine("}");
            return sb.ToString();
        }

        private static IEnumerable<TestCaseData> Cases {
            get {
                yield return new TestCaseData(
                    GetSnippet(false),
                    new object[] { null, null, true },
                    null)
                .SetName($"{FIXTURE}Normal_1stReturn{PASSES}");

                yield return new TestCaseData(
                    GetSnippet(false),
                    new object[] { null, null, false },
                    null)
                .SetName($"{FIXTURE}Normal_2ndReturn{PASSES}");

                yield return new TestCaseData(
                    GetSnippet(true),
                    new object[] { "test", null, true },
                    null)
                .SetName($"{FIXTURE}Postcondition_1stReturn{PASSES_IF}");

                yield return new TestCaseData(
                    GetSnippet(true),
                    new object[] { null, "test", false },
                    null)
                .SetName($"{FIXTURE}Postcondition_2ndReturn{PASSES_IF}");

                yield return new TestCaseData(
                    GetSnippet(true),
                    new object[] { null, "test", true },
                    typeof(PostconditionException))
                .SetName($"{FIXTURE}Postcondition_1stReturn{FAILS_IF}");

                yield return new TestCaseData(
                    GetSnippet(true),
                    new object[] { "test", null, false },
                    typeof(PostconditionException))
                .SetName($"{FIXTURE}Postcondition_2ndReturn{FAILS_IF}");
            }
        }
    }
}
