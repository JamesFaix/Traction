using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_Operators {

        private const string fixture = nameof(Runtime_Operators) + "_";

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
                .SetName($"{fixture}Normal_{Constants.Normal}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.Pre),
                    false, null,
                    null)
                .SetName($"{fixture}Precondition_{Constants.Passes}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.Pre),
                    true, null,
                    typeof(PreconditionException))
                .SetName($"{fixture}Precondition_{Constants.Fails}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.Post),
                    false, "test",
                    null)
                .SetName($"{fixture}Postcondition_{Constants.Passes}");

                yield return new TestCaseData(
                   GetSnippet(ContractTypes.Post),
                   false, null,
                   typeof(PostconditionException))
               .SetName($"{fixture}Postcondition_{Constants.Fails}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.PreAndPost),
                    false, "test",
                    null)
                .SetName($"{fixture}PreAndPost_Pre{Constants.Passes}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.PreAndPost),
                    true, "test",
                    typeof(PreconditionException))
                .SetName($"{fixture}PreAndPost_Pre{Constants.Fails}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.PreAndPost),
                    false, "test",
                    null)
                .SetName($"{fixture}PreAndPost_Post{Constants.Passes}");

                yield return new TestCaseData(
                   GetSnippet(ContractTypes.PreAndPost),
                   false, null,
                   typeof(PostconditionException))
               .SetName($"{fixture}PreAndPost_Post{Constants.Fails}");
            }
        }
    }
}
