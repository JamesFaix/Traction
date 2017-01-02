using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using static Traction.Tests.Constants;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_Conversions {

        private const string FIXTURE = nameof(Runtime_Conversions) + "_";

        [Test, TestCaseSource(nameof(Cases))]
        public void Test(string sourceCode, bool nullArg, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("op_Explicit");

            CustomAssert.Throws(exceptionType, method, null,
                new object[] { nullArg ? null : instance });
        }

        private static string GetSnippet(ContractTypes contractTypes, string result = null) {
            var sb = new StringBuilder();
            sb.AppendLine("class TestClass {");

            sb.AppendLineIf(contractTypes.HasFlag(ContractTypes.Post), "[return: NonNull]");
            sb.Append("public static explicit operator string(");
            sb.AppendIf(contractTypes.HasFlag(ContractTypes.Pre), "[NonNull]");
            sb.AppendLine("TestClass obj) {");
            sb.AppendLine($"return {(result == null ? "null" : $"\"{result}\"")};");
            sb.AppendLine("} }");
            return sb.ToString();
        }

        private static IEnumerable<TestCaseData> Cases {
            get {
                yield return new TestCaseData(
                    GetSnippet(ContractTypes.None),
                    true,
                    null)
                .SetName($"{FIXTURE}Normal_{PASSES}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.Pre),
                    false,
                    null)
                .SetName($"{FIXTURE}Precondition_{PASSES_IF}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.Pre),
                    true,
                    typeof(PreconditionException))
                .SetName($"{FIXTURE}Precondition_{FAILS_IF}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.Post, "test"),
                    true,
                    null)
                .SetName($"{FIXTURE}Postcondition_{PASSES_IF}");

                yield return new TestCaseData(
                   GetSnippet(ContractTypes.Post),
                   true,
                   typeof(PostconditionException))
               .SetName($"{FIXTURE}Postcondition_{FAILS_IF}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.PreAndPost, "test"),
                    false,
                    null)
                .SetName($"{FIXTURE}PreAndPost_Pre{PASSES_IF}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.PreAndPost, "test"),
                    true,
                    typeof(PreconditionException))
                .SetName($"{FIXTURE}PreAndPost_Pre{FAILS_IF}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.PreAndPost, "test"),
                    false,
                    null)
                .SetName($"{FIXTURE}PreAndPost_Post{PASSES_IF}");

                yield return new TestCaseData(
                   GetSnippet(ContractTypes.PreAndPost),
                   false,
                   typeof(PostconditionException))
               .SetName($"{FIXTURE}PreAndPost_Post{FAILS_IF}");
            }
        }
    }
}
