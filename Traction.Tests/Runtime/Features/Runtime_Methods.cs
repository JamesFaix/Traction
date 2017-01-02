using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using static Traction.Tests.Constants;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_Methods {

        private const string FIXTURE = nameof(Runtime_Methods)+"_";

        [Test, TestCaseSource(nameof(Cases))]
        public void Test(string sourceCode, object[] arguments, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("TestMethod");

            CustomAssert.Throws(exceptionType, method, instance, arguments);
        }

        private static string GetSnippet(ContractTypes contractTypes) {
            var sb = new StringBuilder();
            sb.AppendLine("class TestClass {");

            sb.AppendLineIf(contractTypes.HasFlag(ContractTypes.Post), "[return: NonNull]");
            sb.Append("public string TestMethod(");
            sb.AppendIf(contractTypes.HasFlag(ContractTypes.Pre), "[NonNull]");
            sb.AppendLine("string text, string output) {");
            sb.AppendLine("return output; } }");
            return sb.ToString();
        }

        private static IEnumerable<TestCaseData> Cases {
            get {
                yield return new TestCaseData(
                    GetSnippet(ContractTypes.None),
                    new object[] { null, null },
                    null)
                .SetName($"{FIXTURE}Normal_{PASSES}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.Pre),
                    new object[] { "test", null },
                    null)
                .SetName($"{FIXTURE}Precondition_{PASSES_IF}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.Pre),
                    new object[] { null, null },
                    typeof(PreconditionException))
                .SetName($"{FIXTURE}Precondition_{FAILS_IF}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.Post),
                    new object[] { null, "test" },
                    null)
                .SetName($"{FIXTURE}Postcondition_{PASSES_IF}");

                yield return new TestCaseData(
                   GetSnippet(ContractTypes.Post),
                   new object[] { null, null },
                   typeof(PostconditionException))
               .SetName($"{FIXTURE}Postcondition_{FAILS_IF}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.PreAndPost),
                    new object[] { "test", "test" },
                    null)
                .SetName($"{FIXTURE}PreAndPost_Pre{PASSES_IF}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.PreAndPost),
                    new object[] { null, null },
                    typeof(PreconditionException))
                .SetName($"{FIXTURE}PreAndPost_Pre{FAILS_IF}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.PreAndPost),
                    new object[] { "test", "test" },
                    null)
                .SetName($"{FIXTURE}PreAndPost_Post{PASSES_IF}");

                yield return new TestCaseData(
                   GetSnippet(ContractTypes.PreAndPost),
                   new object[] { "test", null },
                   typeof(PostconditionException))
               .SetName($"{FIXTURE}PreAndPost_Post{FAILS_IF}");
            }
        }
    }
}
