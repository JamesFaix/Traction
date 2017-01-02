using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_Methods {

        private const string fixture = nameof(Runtime_Methods)+"_";

        [Test, TestCaseSource(nameof(Cases))]
        public void Test(string sourceCode, object[] arguments, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("TestMethod");

            CustomAssert.Throws(exceptionType, method, instance, arguments);
        }

        private static string GetSnippet(bool hasPre, bool hasPost) {
            var sb = new StringBuilder();
            sb.AppendLine("class TestClass {");

            sb.AppendLineIf(hasPost, "[return: NonNull]");
            sb.Append("public string TestMethod(");
            sb.AppendIf(hasPre, "[NonNull]");
            sb.AppendLine("string text, string output) {");
            sb.AppendLine("return output; } }");
            return sb.ToString();
        }

        private static IEnumerable<TestCaseData> Cases {
            get {
                yield return new TestCaseData(
                    GetSnippet(false, false),
                    new object[] { null, null },
                    null)
                .SetName($"{fixture}Normal_{Constants.Normal}");

                yield return new TestCaseData(
                    GetSnippet(true, false),
                    new object[] { "test", null },
                    null)
                .SetName($"{fixture}Precondition_{Constants.Passes}");

                yield return new TestCaseData(
                    GetSnippet(true, false),
                    new object[] { null, null },
                    typeof(PreconditionException))
                .SetName($"{fixture}Precondition_{Constants.Fails}");

                yield return new TestCaseData(
                    GetSnippet(false, true),
                    new object[] { null, "test" },
                    null)
                .SetName($"{fixture}Postcondition_{Constants.Passes}");

                yield return new TestCaseData(
                   GetSnippet(false, true),
                   new object[] { null, null },
                   typeof(PostconditionException))
               .SetName($"{fixture}Postcondition_{Constants.Fails}");

                yield return new TestCaseData(
                    GetSnippet(true, true),
                    new object[] { "test", "test" },
                    null)
                .SetName($"{fixture}PreAndPost_Pre{Constants.Passes}");

                yield return new TestCaseData(
                    GetSnippet(true, true),
                    new object[] { null, null },
                    typeof(PreconditionException))
                .SetName($"{fixture}PreAndPost_Pre{Constants.Fails}");

                yield return new TestCaseData(
                    GetSnippet(true, true),
                    new object[] { "test", "test" },
                    null)
                .SetName($"{fixture}PreAndPost_Post{Constants.Passes}");

                yield return new TestCaseData(
                   GetSnippet(true, true),
                   new object[] { "test", null },
                   typeof(PostconditionException))
               .SetName($"{fixture}PreAndPost_Post{Constants.Fails}");
            }
        }
    }
}
