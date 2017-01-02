using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_AutoProperties {

        private const string fixture = nameof(Runtime_AutoProperties)+ "_";

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
                    Constants.EmptyArgs,
                    null)
                .SetName($"{fixture}NormalReadonly_Get{Constants.Normal}");

                yield return new TestCaseData(
                    GetSnippet(false, true, "test"),
                    getter,
                    Constants.EmptyArgs,
                    null)
                .SetName($"{fixture}ContractReadonly_Get{Constants.Passes}");

                yield return new TestCaseData(
                    GetSnippet(false, true),
                    getter,
                    Constants.EmptyArgs,
                    typeof(PostconditionException))
                .SetName($"{fixture}ContractReadonly_Get{Constants.Fails}");

                yield return new TestCaseData(
                    GetSnippet(true, false),
                    getter,
                    Constants.EmptyArgs,
                    null)
                .SetName($"{fixture}NormalReadWrite_Get{Constants.Normal}");

                yield return new TestCaseData(
                    GetSnippet(true, false),
                    setter,
                    new object[] { null },
                    null)
                .SetName($"{fixture}NormalReadWrite_Set{Constants.Normal}");

                yield return new TestCaseData(
                    GetSnippet(true, true, "test"),
                    getter,
                    Constants.EmptyArgs,
                    null)
                .SetName($"{fixture}ContractReadWrite_Get{Constants.Passes}");

                yield return new TestCaseData(
                    GetSnippet(true, true),
                    getter,
                    Constants.EmptyArgs,
                    typeof(PostconditionException))
                .SetName($"{fixture}ContractReadWrite_Get{Constants.Fails}");

                yield return new TestCaseData(
                    GetSnippet(true, true),
                    setter,
                    new object[] { "test" },
                    null)
                .SetName($"{fixture}ContractReadWrite_Set{Constants.Passes}");

                yield return new TestCaseData(
                    GetSnippet(true, true),
                    setter,
                    new object[] { null },
                    typeof(PreconditionException))
                .SetName($"{fixture}ContractReadWrite_Set{Constants.Fails}");
            }

        }
    }
}
