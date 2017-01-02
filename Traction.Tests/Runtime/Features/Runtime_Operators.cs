using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using static Traction.Tests.Constants;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_Operators {

        private const string FIXTURE = nameof(Runtime_Operators) + "_";

        [Test, TestCaseSource(nameof(Cases))]
        public void Test(string sourceCode, bool nullFirstArg, string secondArg, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("op_Addition");

            CustomAssert.Throws(exceptionType, method, null,
                new object[] { nullFirstArg ? null : instance, secondArg });
        }

        private static string GetSnippet(ContractTypes contractTypes) {
            var sb = new StringBuilder();
            sb.AppendLine("class TestClass {");

            sb.AppendLineIf(contractTypes.HasFlag(ContractTypes.Post), "[return: NonNull]");
            sb.Append("public static string operator + (");
            sb.AppendIf(contractTypes.HasFlag(ContractTypes.Pre), "[NonNull]");
            sb.AppendLine("TestClass obj, string output) {");
            sb.AppendLine("return output; } }");
            return sb.ToString();
        }

        private static IEnumerable<TestCaseData> Cases {
            get {
                yield return new TestCaseData(
                    GetSnippet(ContractTypes.None),
                    true, null,
                    null)
                .SetName($"{FIXTURE}Normal_{PASSES}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.Pre),
                    false, null,
                    null)
                .SetName($"{FIXTURE}Precondition_{PASSES_IF}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.Pre),
                    true, null,
                    typeof(PreconditionException))
                .SetName($"{FIXTURE}Precondition_{FAILS_IF}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.Post),
                    false, "test",
                    null)
                .SetName($"{FIXTURE}Postcondition_{PASSES_IF}");

                yield return new TestCaseData(
                   GetSnippet(ContractTypes.Post),
                   false, null,
                   typeof(PostconditionException))
               .SetName($"{FIXTURE}Postcondition_{FAILS_IF}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.PreAndPost),
                    false, "test",
                    null)
                .SetName($"{FIXTURE}PreAndPost_Pre{PASSES_IF}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.PreAndPost),
                    true, "test",
                    typeof(PreconditionException))
                .SetName($"{FIXTURE}PreAndPost_Pre{FAILS_IF}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.PreAndPost),
                    false, "test",
                    null)
                .SetName($"{FIXTURE}PreAndPost_Post{PASSES_IF}");

                yield return new TestCaseData(
                   GetSnippet(ContractTypes.PreAndPost),
                   false, null,
                   typeof(PostconditionException))
               .SetName($"{FIXTURE}PreAndPost_Post{FAILS_IF}");
            }
        }
    }
}
