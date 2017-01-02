using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using static Traction.Tests.Constants;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_Inheritance_Classes {

        private const string FIXTURE = nameof(Runtime_Inheritance_Classes) + "_";

        [Test, TestCaseSource(nameof(Cases))]
        public void Test(string sourceCode, string methodName, object[] arguments, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod(methodName);

            CustomAssert.Throws(exceptionType, method, instance, arguments);
        }

        private static string GetMethodSnippet(ContractTypes contractTypes) {
            var sb = new StringBuilder();
            sb.AppendLine("abstract class BaseClass {");
            sb.AppendLineIf(contractTypes.HasFlag(ContractTypes.Post), "[return: NonNull]");
            sb.Append("public abstract string TestMethod(");
            sb.AppendIf(contractTypes.HasFlag(ContractTypes.Pre), "[NonNull]");
            sb.AppendLine("string text);");
            sb.AppendLine("}");

            sb.AppendLine("class TestClass : BaseClass {");
            sb.AppendLine("public override string TestMethod(string text) {");
            sb.AppendLine("return text;");
            sb.AppendLine("}");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string GetPropertySnippet(bool hasContract, string initialValue = null) {
            var sb = new StringBuilder();
            sb.AppendLine("abstract class BaseClass {");
            sb.AppendLineIf(hasContract, "[NonNull]");
            sb.AppendLine("public abstract string TestProperty { get; set; }");
            sb.AppendLine("}");

            sb.AppendLine("class TestClass : BaseClass {");
            sb.Append("public string testField");
            sb.AppendIf(initialValue != null, $" = \"{initialValue}\"");
            sb.AppendLine(";");

            sb.AppendLine("public override string TestProperty { ");
            sb.AppendLine("get { return testField; }");
            sb.AppendLine("set { testField = value; }");
            sb.AppendLine("}");
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
                    GetMethodSnippet(ContractTypes.Post),
                    "TestMethod",
                    new object[] { "test" },
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
                .SetName($"{FIXTURE}NormalProperty_Get{PASSES}");

                yield return new TestCaseData(
                    GetPropertySnippet(false),
                    "set_TestProperty",
                    new object[] { null },
                    null)
                .SetName($"{FIXTURE}NormalProperty_Set{PASSES}");

                yield return new TestCaseData(
                    GetPropertySnippet(true),
                    "set_TestProperty",
                    new object[] { "test" },
                    null)
                .SetName($"{FIXTURE}PreconditionProperty_Set{PASSES_IF}");

                yield return new TestCaseData(
                     GetPropertySnippet(true),
                     "set_TestProperty",
                     new object[] { null },
                     typeof(PreconditionException))
                 .SetName($"{FIXTURE}PreconditionProperty_Set{FAILS_IF}");

                yield return new TestCaseData(
                     GetPropertySnippet(true, "test"),
                     "get_TestProperty",
                     NO_ARGS,
                     null)
                 .SetName($"{FIXTURE}PostconditionProperty_Get{PASSES_IF}");

                yield return new TestCaseData(
                     GetPropertySnippet(true),
                     "get_TestProperty",
                     NO_ARGS,
                     typeof(PostconditionException))
                 .SetName($"{FIXTURE}PostconditionProperty_Get{FAILS_IF}");
            }
        }
    }
}