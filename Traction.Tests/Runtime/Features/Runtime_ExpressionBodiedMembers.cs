using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_ExpressionBodiedMembers {

        private const string fixture = nameof(Runtime_ExpressionBodiedMembers) + "_";

        [Test, TestCaseSource(nameof(Cases))]
        public void Test(string sourceCode, string methodName, object[] arguments, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod(methodName);

            CustomAssert.Throws(exceptionType, method, instance, arguments);
        }

        private static string GetMethodSnippet(bool hasPre, bool hasPost, string result = null) {
            var sb = new StringBuilder();
            sb.AppendLine("class TestClass {");

            sb.AppendLineIf(hasPost, "[return: NonNull]");
            sb.Append("public string TestMethod(");
            sb.AppendIf(hasPre, "[NonNull]");
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
                    GetMethodSnippet(false, false),
                    "TestMethod",
                    new object[] { null },
                    null)
                .SetName($"{fixture}NormalMethod_{Constants.Normal}");

                yield return new TestCaseData(
                    GetMethodSnippet(true, false),
                    "TestMethod",
                    new object[] { "test" },
                    null)
                .SetName($"{fixture}PreconditionMethod_{Constants.Passes}");

                yield return new TestCaseData(
                    GetMethodSnippet(true, false),
                    "TestMethod",
                    new object[] { null },
                    typeof(PreconditionException))
                .SetName($"{fixture}PreconditionMethod_{Constants.Fails}");

                yield return new TestCaseData(
                    GetMethodSnippet(false, true, "test"),
                    "TestMethod",
                    new object[] { null },
                    null)
                .SetName($"{fixture}PostconditionMethod_{Constants.Passes}");

                yield return new TestCaseData(
                    GetMethodSnippet(false, true),
                    "TestMethod",
                    new object[] { null },
                    typeof(PostconditionException))
                .SetName($"{fixture}PostconditionMethod_{Constants.Fails}");

                yield return new TestCaseData(
                   GetPropertySnippet(false),
                   "get_TestProperty",
                   Constants.EmptyArgs,
                   null)
               .SetName($"{fixture}NormalProperty_{Constants.Normal}");

                yield return new TestCaseData(
                   GetPropertySnippet(true, "test"),
                   "get_TestProperty",
                   Constants.EmptyArgs,
                   null)
               .SetName($"{fixture}PostconditionProperty_{Constants.Passes}");

                yield return new TestCaseData(
                   GetPropertySnippet(true),
                   "get_TestProperty",
                   Constants.EmptyArgs,
                   typeof(PostconditionException))
               .SetName($"{fixture}PostconditionProperty_{Constants.Fails}");
            }
        }
    }
}
