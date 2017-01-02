using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using static Traction.Tests.Constants;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_Properties {

        private const string FIXTURE = nameof(Runtime_Properties) + "_";

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
                    NO_ARGS,
                    null)
                .SetName($"{FIXTURE}NormalReadonly_Get{PASSES}");

                yield return new TestCaseData(
                    GetSnippet(true, false, true, "test"),
                    getter,
                    NO_ARGS,
                    null)
                .SetName($"{FIXTURE}ContractReadonly_Get{PASSES_IF}");

                yield return new TestCaseData(
                    GetSnippet(true, false, true),
                    getter,
                    NO_ARGS,
                    typeof(PostconditionException))
                .SetName($"{FIXTURE}ContractReadonly_Get{FAILS_IF}");

                yield return new TestCaseData(
                    GetSnippet(false, true, false),
                    setter,
                    new object[] { null },
                    null)
                .SetName($"{FIXTURE}NormalWriteonly_Set{PASSES}");

                yield return new TestCaseData(
                    GetSnippet(false, true, true),
                    setter,
                    new object[] { "test" },
                    null)
                .SetName($"{FIXTURE}ContractWriteonly_Set{PASSES_IF}");

                yield return new TestCaseData(
                    GetSnippet(false, true, true),
                    setter,
                    new object[] { null },
                    typeof(PreconditionException))
                .SetName($"{FIXTURE}ContractWriteonly_Set{FAILS_IF}");

                yield return new TestCaseData(
                    GetSnippet(true, true, false),
                    getter,
                    NO_ARGS,
                    null)
                .SetName($"{FIXTURE}NormalReadWrite_Get{PASSES}");

                yield return new TestCaseData(
                    GetSnippet(true, true, false),
                    setter,
                    new object[] { null },
                    null)
                .SetName($"{FIXTURE}NormalReadWrite_Set{PASSES}");

                yield return new TestCaseData(
                    GetSnippet(true, true, true, "test"),
                    getter,
                    NO_ARGS,
                    null)
                .SetName($"{FIXTURE}ContractReadWrite_Get{PASSES_IF}");

                yield return new TestCaseData(
                    GetSnippet(true, true, true),
                    getter,
                    NO_ARGS,
                    typeof(PostconditionException))
                .SetName($"{FIXTURE}ContractReadWrite_Get{FAILS_IF}");

                yield return new TestCaseData(
                    GetSnippet(true, true, true),
                    setter,
                    new object[] { "test" },
                    null)
                .SetName($"{FIXTURE}ContractReadWrite_Set{PASSES_IF}");

                yield return new TestCaseData(
                    GetSnippet(true, true, true),
                    setter,
                    new object[] { null },
                    typeof(PreconditionException))
                .SetName($"{FIXTURE}ContractReadWrite_Set{FAILS_IF}");
            }
        }
    }
}
