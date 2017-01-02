using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using static Traction.Tests.Constants;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_ExpressionBodiedMembers {

        private const string FIXTURE = nameof(Runtime_ExpressionBodiedMembers) + "_";

        [Test, TestCaseSource(nameof(Cases))]
        public void Test(string sourceCode, string methodName, object[] arguments, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod(methodName);

            CustomAssert.Throws(exceptionType, method, instance, arguments);
        }

        private static string GetMethodSnippet(ContractTypes contractTypes, string result = null) {
            var sb = new StringBuilder();
            sb.AppendLine("class TestClass {");

            sb.AppendLineIf(contractTypes.HasFlag(ContractTypes.Post), "[return: NonNull]");
            sb.Append("public string TestMethod(");
            sb.AppendIf(contractTypes.HasFlag(ContractTypes.Pre), "[NonNull]");
            sb.AppendLine("string text) =>");
            sb.Append($"{(result == null ? "null" : $"\"{result}\"")};");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string GetPropertySnippet(bool hasContract, string result = null) {
            var sb = new StringBuilder();
            sb.AppendLine("class TestClass {");

            sb.AppendLineIf(hasContract, "[NonNull]");
            sb.Append("public string TestProperty => ");
            sb.Append($"{(result == null ? "null" : $"\"{result}\"")};");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static IEnumerable<TestCaseData> Cases {
            get {
                yield return new TestCaseData(
                    GetMethodSnippet(ContractTypes.None),
                    "TestMethod",
                    new object[] { null },
                    null)
                .SetName($"{FIXTURE}NormalMethod_{PASSES}");

                yield return new TestCaseData(
                    GetMethodSnippet(ContractTypes.Pre),
                    "TestMethod",
                    new object[] { "test" },
                    null)
                .SetName($"{FIXTURE}PreconditionMethod_{PASSES_IF}");

                yield return new TestCaseData(
                    GetMethodSnippet(ContractTypes.Pre),
                    "TestMethod",
                    new object[] { null },
                    typeof(PreconditionException))
                .SetName($"{FIXTURE}PreconditionMethod_{FAILS_IF}");

                yield return new TestCaseData(
                    GetMethodSnippet(ContractTypes.Post, "test"),
                    "TestMethod",
                    new object[] { null },
                    null)
                .SetName($"{FIXTURE}PostconditionMethod_{PASSES_IF}");

                yield return new TestCaseData(
                    GetMethodSnippet(ContractTypes.Post),
                    "TestMethod",
                    new object[] { null },
                    typeof(PostconditionException))
                .SetName($"{FIXTURE}PostconditionMethod_{FAILS_IF}");

                yield return new TestCaseData(
                   GetPropertySnippet(false),
                   "get_TestProperty",
                   NO_ARGS,
                   null)
               .SetName($"{FIXTURE}NormalProperty_{PASSES}");

                yield return new TestCaseData(
                   GetPropertySnippet(true, "test"),
                   "get_TestProperty",
                   NO_ARGS,
                   null)
               .SetName($"{FIXTURE}PostconditionProperty_{PASSES_IF}");

                yield return new TestCaseData(
                   GetPropertySnippet(true),
                   "get_TestProperty",
                   NO_ARGS,
                   typeof(PostconditionException))
               .SetName($"{FIXTURE}PostconditionProperty_{FAILS_IF}");
            }
        }
    }
}
