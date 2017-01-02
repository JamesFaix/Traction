using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_MultipleReturnStatements {

        private const string fixture = nameof(Runtime_MultipleReturnStatements) + "_";

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
                .SetName($"{fixture}Normal_1stReturn{Constants.Normal}");

                yield return new TestCaseData(
                    GetSnippet(false),
                    new object[] { null, null, false },
                    null)
                .SetName($"{fixture}Normal_2ndReturn{Constants.Normal}");

                yield return new TestCaseData(
                    GetSnippet(true),
                    new object[] { "test", null, true },
                    null)
                .SetName($"{fixture}Postcondition_1stReturn{Constants.Passes}");

                yield return new TestCaseData(
                    GetSnippet(true),
                    new object[] { null, "test", false },
                    null)
                .SetName($"{fixture}Postcondition_2ndReturn{Constants.Passes}");

                yield return new TestCaseData(
                    GetSnippet(true),
                    new object[] { null, "test", true },
                    typeof(PostconditionException))
                .SetName($"{fixture}Postcondition_1stReturn{Constants.Fails}");

                yield return new TestCaseData(
                    GetSnippet(true),
                    new object[] { "test", null, false },
                    typeof(PostconditionException))
                .SetName($"{fixture}Postcondition_2ndReturn{Constants.Fails}");
            }
        }
    }
}
