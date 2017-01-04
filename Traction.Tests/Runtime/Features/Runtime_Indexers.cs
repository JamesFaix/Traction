using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using static Traction.Tests.Constants;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_Indexers {

        private const string FIXTURE = nameof(Runtime_Indexers) + "_";

        [Test, TestCaseSource(nameof(ValueCases))]
        public void ValueTest(string sourceCode, string methodName, object[] arguments, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod(methodName);

            CustomAssert.Throws(exceptionType, method, instance, arguments);
        }

        private static string GetValueSnippet(bool hasGet, bool hasSet, bool hasContract, string initialFieldValue = "test") {
            var sb = new StringBuilder();
            sb.AppendLine("class TestClass {");

            sb.AppendLine("private string[] testField = new string[] {");
            sb.AppendLine(initialFieldValue == null ? "null" : $"\"{initialFieldValue}\"");
            sb.AppendLine("};");

            sb.AppendLineIf(hasContract, "[NonNull]");
            sb.AppendLine("public string this [int index] { ");
            sb.AppendLineIf(hasGet, "get { return testField[index]; }");
            sb.AppendLineIf(hasSet, "set { testField[index] = value; }");
            sb.AppendLine("}");

            sb.AppendLine("}");
            return sb.ToString();
        }

        private static IEnumerable<TestCaseData> ValueCases {
            get {

                var getter = "get_Item";
                var setter = "set_Item";
                var head = $"{FIXTURE}Value_";

                yield return new TestCaseData(
                    GetValueSnippet(true, false, false, null),
                    getter,
                    new object[] { 0 },
                    null)
                .SetName($"{head}NormalReadonly_Get{PASSES}");

                yield return new TestCaseData(
                    GetValueSnippet(true, false, true),
                    getter,
                     new object[] { 0 },
                    null)
                .SetName($"{head}ContractReadonly_Get{PASSES_IF}");

                yield return new TestCaseData(
                    GetValueSnippet(true, false, true, null),
                    getter,
                     new object[] { 0 },
                    typeof(PostconditionException))
                .SetName($"{head}ContractReadonly_Get{FAILS_IF}");

                yield return new TestCaseData(
                    GetValueSnippet(false, true, false),
                    setter,
                    new object[] { 0, null },
                    null)
                .SetName($"{head}NormalWriteonly_Set{PASSES}");

                yield return new TestCaseData(
                    GetValueSnippet(false, true, true),
                    setter,
                    new object[] { 0, "test" },
                    null)
                .SetName($"{head}ContractWriteonly_Set{PASSES_IF}");

                yield return new TestCaseData(
                    GetValueSnippet(false, true, true),
                    setter,
                    new object[] { 0, null },
                    typeof(PreconditionException))
                .SetName($"{head}ContractWriteonly_Set{FAILS_IF}");

                yield return new TestCaseData(
                    GetValueSnippet(true, true, false, null),
                    getter,
                    new object[] { 0 },
                    null)
                .SetName($"{head}NormalReadWrite_Get{PASSES}");

                yield return new TestCaseData(
                    GetValueSnippet(true, true, false),
                    setter,
                    new object[] { 0, null },
                    null)
                .SetName($"{head}NormalReadWrite_Set{PASSES}");

                yield return new TestCaseData(
                    GetValueSnippet(true, true, true),
                    getter,
                    new object[] { 0 },
                    null)
                .SetName($"{head}ContractReadWrite_Get{PASSES_IF}");

                yield return new TestCaseData(
                    GetValueSnippet(true, true, true, null),
                    getter,
                    new object[] { 0 },
                    typeof(PostconditionException))
                .SetName($"{head}ContractReadWrite_Get{FAILS_IF}");

                yield return new TestCaseData(
                    GetValueSnippet(true, true, true),
                    setter,
                    new object[] { 0, "test" },
                    null)
                .SetName($"{head}ContractReadWrite_Set{PASSES_IF}");

                yield return new TestCaseData(
                    GetValueSnippet(true, true, true),
                    setter,
                    new object[] { 0, null },
                    typeof(PreconditionException))
                .SetName($"{head}ContractReadWrite_Set{FAILS_IF}");
            }
        }
    }
}
