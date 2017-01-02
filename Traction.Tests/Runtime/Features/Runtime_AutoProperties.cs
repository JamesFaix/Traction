using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using static Traction.Tests.Constants;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_AutoProperties {

        private const string FIXTURE = nameof(Runtime_AutoProperties)+ "_";

        [Test, TestCaseSource(nameof(Cases))]
        public void Test(string sourceCode, string methodName, object[] arguments, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod(methodName);

            CustomAssert.Throws(exceptionType, method, instance, arguments);
        }

        private static string GetSnippet(bool hasSet, bool hasContract, string initialValue = null) {
            var sb = new StringBuilder();
            sb.AppendLine("class TestClass {");

            sb.AppendLineIf(hasContract, "[NonNull]");
            sb.Append("public string TestProperty { get; ");
            sb.AppendIf(hasSet, "set;");
            sb.Append("}");
            sb.AppendIf(initialValue != null, $" = \"{initialValue}\";");

            sb.Append("}");
            return sb.ToString();
        }

        private static IEnumerable<TestCaseData> Cases {
            get {

                var getter = "get_TestProperty";
                var setter = "set_TestProperty";

                yield return new TestCaseData(
                    GetSnippet(false, false),
                    getter,
                    NO_ARGS,
                    null)
                .SetName($"{FIXTURE}NormalReadonly_Get{PASSES}");

                yield return new TestCaseData(
                    GetSnippet(false, true, "test"),
                    getter,
                    NO_ARGS,
                    null)
                .SetName($"{FIXTURE}ContractReadonly_Get{PASSES_IF}");

                yield return new TestCaseData(
                    GetSnippet(false, true),
                    getter,
                    NO_ARGS,
                    typeof(PostconditionException))
                .SetName($"{FIXTURE}ContractReadonly_Get{FAILS_IF}");

                yield return new TestCaseData(
                    GetSnippet(true, false),
                    getter,
                    NO_ARGS,
                    null)
                .SetName($"{FIXTURE}NormalReadWrite_Get{PASSES}");

                yield return new TestCaseData(
                    GetSnippet(true, false),
                    setter,
                    new object[] { null },
                    null)
                .SetName($"{FIXTURE}NormalReadWrite_Set{PASSES}");

                yield return new TestCaseData(
                    GetSnippet(true, true, "test"),
                    getter,
                    NO_ARGS,
                    null)
                .SetName($"{FIXTURE}ContractReadWrite_Get{PASSES_IF}");

                yield return new TestCaseData(
                    GetSnippet(true, true),
                    getter,
                    NO_ARGS,
                    typeof(PostconditionException))
                .SetName($"{FIXTURE}ContractReadWrite_Get{FAILS_IF}");

                yield return new TestCaseData(
                    GetSnippet(true, true),
                    setter,
                    new object[] { "test" },
                    null)
                .SetName($"{FIXTURE}ContractReadWrite_Set{PASSES_IF}");

                yield return new TestCaseData(
                    GetSnippet(true, true),
                    setter,
                    new object[] { null },
                    typeof(PreconditionException))
                .SetName($"{FIXTURE}ContractReadWrite_Set{FAILS_IF}");
            }

        }
    }
}
