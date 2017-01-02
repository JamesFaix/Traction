using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_Inheritance_ClassesMultiLayer {

        private const string fixture = nameof(Runtime_Inheritance_ClassesMultiLayer) + "_";

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

            sb.AppendLine("class IntermediateClass : BaseClass {");
            sb.AppendLine("public override string TestMethod(string text) { return text; }");
            sb.AppendLine("}");

            sb.AppendLine("class TestClass : IntermediateClass {");
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

            sb.AppendLine("class IntermediateClass : BaseClass {");
            sb.AppendLine("public override string TestProperty { get; set; }");
            sb.AppendLine("}");

            sb.AppendLine("class TestClass : IntermediateClass {");
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
                .SetName($"{fixture}NormalMethod_{Constants.Normal}");

                yield return new TestCaseData(
                    GetMethodSnippet(ContractTypes.Pre),
                    "TestMethod",
                    new object[] { "test" },
                    null)
                .SetName($"{fixture}PreconditionMethod_{Constants.Passes}");

                yield return new TestCaseData(
                    GetMethodSnippet(ContractTypes.Pre),
                    "TestMethod",
                    new object[] { null },
                    typeof(PreconditionException))
                .SetName($"{fixture}PreconditionMethod_{Constants.Fails}");

                yield return new TestCaseData(
                    GetMethodSnippet(ContractTypes.Post),
                    "TestMethod",
                    new object[] { "test" },
                    null)
                .SetName($"{fixture}PostconditionMethod_{Constants.Passes}");

                yield return new TestCaseData(
                    GetMethodSnippet(ContractTypes.Post),
                    "TestMethod",
                    new object[] { null },
                    typeof(PostconditionException))
                .SetName($"{fixture}PostconditionMethod_{Constants.Fails}");

                yield return new TestCaseData(
                    GetPropertySnippet(false),
                    "get_TestProperty",
                    Constants.EmptyArgs,
                    null)
                .SetName($"{fixture}NormalProperty_Get{Constants.Normal}");

                yield return new TestCaseData(
                    GetPropertySnippet(false),
                    "set_TestProperty",
                    new object[] { null },
                    null)
                .SetName($"{fixture}NormalProperty_Set{Constants.Normal}");

                yield return new TestCaseData(
                    GetPropertySnippet(true),
                    "set_TestProperty",
                    new object[] { "test" },
                    null)
                .SetName($"{fixture}PreconditionProperty_Set{Constants.Passes}");

                yield return new TestCaseData(
                     GetPropertySnippet(true),
                     "set_TestProperty",
                     new object[] { null },
                     typeof(PreconditionException))
                 .SetName($"{fixture}PreconditionProperty_Set{Constants.Fails}");

                yield return new TestCaseData(
                     GetPropertySnippet(true, "test"),
                     "get_TestProperty",
                     Constants.EmptyArgs,
                     null)
                 .SetName($"{fixture}PostconditionProperty_Get{Constants.Passes}");

                yield return new TestCaseData(
                     GetPropertySnippet(true),
                     "get_TestProperty",
                     Constants.EmptyArgs,
                     typeof(PostconditionException))
                 .SetName($"{fixture}PostconditionProperty_Get{Constants.Fails}");
            }
        }
    }
}