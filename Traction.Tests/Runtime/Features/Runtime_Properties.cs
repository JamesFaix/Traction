using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Text;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_Properties {

        private const string fixture = nameof(Runtime_Properties) + "_";

        [Test, TestCaseSource(nameof(Cases))]
        public void Test(string sourceCode, string methodName, object[] arguments, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod(methodName);

            CustomAssert.Throws(exceptionType, method, instance, arguments);
        }

        private static string GetSnippet(bool hasGet, bool hasSet, bool hasContract, string initialFieldValue = null) {
            var sb = new StringBuilder();
            sb.AppendLine("class TestClass {");

            sb.AppendLine($"private string testField{(initialFieldValue == null ? "" : ($" = \"{initialFieldValue}\""))};");

            sb.AppendLineIf(hasContract, "[NonNull]");
            sb.AppendLine("public string TestProperty { ");
            sb.AppendLineIf(hasGet, "get { return testField; }");
            sb.AppendLineIf(hasSet, "set { testField = value; }");
            sb.AppendLine("}");

            sb.AppendLine("}");
            return sb.ToString();
        }

        private static IEnumerable<TestCaseData> Cases {
            get {

                var getter = "get_TestProperty";
                var setter = "set_TestProperty";

                yield return new TestCaseData(
                    GetSnippet(true, false, false),
                    getter,
                    Constants.EmptyArgs,
                    null)
                .SetName($"{fixture}NormalReadonly_Get{Constants.Normal}");

                yield return new TestCaseData(
                    GetSnippet(true, false, true, "test"),
                    getter,
                    Constants.EmptyArgs,
                    null)
                .SetName($"{fixture}ContractReadonly_Get{Constants.Passes}");

                yield return new TestCaseData(
                    GetSnippet(true, false, true),
                    getter,
                    Constants.EmptyArgs,
                    typeof(PostconditionException))
                .SetName($"{fixture}ContractReadonly_Get{Constants.Fails}");

                yield return new TestCaseData(
                    GetSnippet(false, true, false),
                    setter,
                    new object[] { null },
                    null)
                .SetName($"{fixture}NormalWriteonly_Set{Constants.Normal}");

                yield return new TestCaseData(
                    GetSnippet(false, true, true),
                    setter,
                    new object[] { "test" },
                    null)
                .SetName($"{fixture}ContractWriteonly_Set{Constants.Passes}");

                yield return new TestCaseData(
                    GetSnippet(false, true, true),
                    setter,
                    new object[] { null },
                    typeof(PreconditionException))
                .SetName($"{fixture}ContractWriteonly_Set{Constants.Fails}");

                yield return new TestCaseData(
                    GetSnippet(true, true, false),
                    getter,
                    Constants.EmptyArgs,
                    null)
                .SetName($"{fixture}NormalReadWrite_Get{Constants.Normal}");

                yield return new TestCaseData(
                    GetSnippet(true, true, false),
                    setter,
                    new object[] { null },
                    null)
                .SetName($"{fixture}NormalReadWrite_Set{Constants.Normal}");

                yield return new TestCaseData(
                    GetSnippet(true, true, true, "test"),
                    getter,
                    Constants.EmptyArgs,
                    null)
                .SetName($"{fixture}ContractReadWrite_Get{Constants.Passes}");

                yield return new TestCaseData(
                    GetSnippet(true, true, true),
                    getter,
                    Constants.EmptyArgs,
                    typeof(PostconditionException))
                .SetName($"{fixture}ContractReadWrite_Get{Constants.Fails}");

                yield return new TestCaseData(
                    GetSnippet(true, true, true),
                    setter,
                    new object[] { "test" },
                    null)
                .SetName($"{fixture}ContractReadWrite_Set{Constants.Passes}");

                yield return new TestCaseData(
                    GetSnippet(true, true, true),
                    setter,
                    new object[] { null },
                    typeof(PreconditionException))
                .SetName($"{fixture}ContractReadWrite_Set{Constants.Fails}");
            }
        }
    }
}
