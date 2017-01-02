using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_Conversions {

        private const string fixture = nameof(Runtime_Conversions) + "_";

        [Test, TestCaseSource(nameof(Cases))]
        public void Test(string sourceCode, bool nullArg, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("op_Explicit");

            CustomAssert.Throws(exceptionType, method, null,
                new object[] { nullArg ? null : instance });
        }

        private static string GetSnippet(bool hasPre, bool hasPost, string result = null) {
            var sb = new StringBuilder();
            sb.AppendLine("class TestClass {");

            sb.AppendLineIf(hasPost, "[return: NonNull]");
            sb.Append("public static explicit operator string(");
            sb.AppendIf(hasPre, "[NonNull]");
            sb.AppendLine("TestClass obj) {");
            sb.AppendLine($"return {(result == null ? "null" : $"\"{result}\"")};");
            sb.AppendLine("} }");
            return sb.ToString();
        }

        private static IEnumerable<TestCaseData> Cases {
            get {
                yield return new TestCaseData(
                    GetSnippet(false, false),
                    true,
                    null)
                .SetName($"{fixture}Normal_{Constants.Normal}");

                yield return new TestCaseData(
                    GetSnippet(true, false),
                    false,
                    null)
                .SetName($"{fixture}Precondition_{Constants.Passes}");

                yield return new TestCaseData(
                    GetSnippet(true, false),
                    true,
                    typeof(PreconditionException))
                .SetName($"{fixture}Precondition_{Constants.Fails}");

                yield return new TestCaseData(
                    GetSnippet(false, true, "test"),
                    true,
                    null)
                .SetName($"{fixture}Postcondition_{Constants.Passes}");

                yield return new TestCaseData(
                   GetSnippet(false, true),
                   true,
                   typeof(PostconditionException))
               .SetName($"{fixture}Postcondition_{Constants.Fails}");

                yield return new TestCaseData(
                    GetSnippet(true, true, "test"),
                    false,
                    null)
                .SetName($"{fixture}PreAndPost_Pre{Constants.Passes}");

                yield return new TestCaseData(
                    GetSnippet(true, true, "test"),
                    true,
                    typeof(PreconditionException))
                .SetName($"{fixture}PreAndPost_Pre{Constants.Fails}");

                yield return new TestCaseData(
                    GetSnippet(true, true, "test"),
                    false,
                    null)
                .SetName($"{fixture}PreAndPost_Post{Constants.Passes}");

                yield return new TestCaseData(
                   GetSnippet(true, true),
                   false,
                   typeof(PostconditionException))
               .SetName($"{fixture}PreAndPost_Post{Constants.Fails}");
            }
        }
    }
}
