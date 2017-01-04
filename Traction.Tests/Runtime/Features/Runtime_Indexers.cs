using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using static Traction.Tests.Constants;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_Indexers {

        private const string FIXTURE = nameof(Runtime_Indexers) + "_";

        [Test, TestCaseSource(nameof(AllCases))]
        public void Test(string sourceCode, string methodName, object[] arguments, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod(methodName);

            CustomAssert.Throws(exceptionType, method, instance, arguments);
        }

        private static string GetValueSnippet(bool hasGet, bool hasSet, bool hasContract, string initialItem = "test") {
            return new StringBuilder()
                .AppendLine("class TestClass {")

                .AppendLine("private string[] arr = new string[] {")
                .AppendLine(initialItem == null ? "null" : $"\"{initialItem}\"")
                .AppendLine("};")

                .AppendLineIf(hasContract, "[NonNull]")
                .AppendLine("public string this [int index] { ")
                .AppendLineIf(hasGet, "get { return arr[index]; }")
                .AppendLineIf(hasSet, "set { arr[index] = value; }")
                .AppendLine("}")

                .AppendLine("}")
                .ToString();
        }

        private static string GetParameterSnippet(bool hasGet, bool hasSet, bool hasPre) {
            return new StringBuilder()
                .AppendLine("class TestClass {")

                .AppendLine("private Dictionary<string, int> dict = new Dictionary<string, int> {")
                .AppendLine("{ \"\", 0 },")
                .AppendLine("{ \"test\", 1 },")
                .AppendLine("};")

                .Append("public int this [")
                .AppendIf(hasPre, "[NonEmpty]")
                .AppendLine("string name] { ")

                .AppendLineIf(hasGet, "get { return dict[name]; }")
                .AppendLineIf(hasSet, "set { dict[name] = value; }")
                .AppendLine("}")

                .AppendLine("}")
                .ToString();
        }

        private static IEnumerable<TestCaseData> AllCases =>
            ValueCases.Concat(ParameterCases);

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

        private static IEnumerable<TestCaseData> ParameterCases {
            get {

                var getter = "get_Item";
                var setter = "set_Item";
                var head = $"{FIXTURE}Parameter_";

                yield return new TestCaseData(
                    GetParameterSnippet(true, false, false),
                    getter,
                    new object[] { "" },
                    null)
                .SetName($"{head}NormalReadonly_Get{PASSES}");

                yield return new TestCaseData(
                    GetParameterSnippet(true, false, true),
                    getter,
                    new object[] { "test" },
                    null)
                .SetName($"{head}ContractReadonly_Get{PASSES_IF}");

                yield return new TestCaseData(
                    GetParameterSnippet(true, false, true),
                    getter,
                     new object[] { "" },
                    typeof(PreconditionException))
                .SetName($"{head}ContractReadonly_Get{FAILS_IF}");

                yield return new TestCaseData(
                    GetParameterSnippet(false, true, false),
                    setter,
                    new object[] { "test", 0 },
                    null)
                .SetName($"{head}NormalWriteonly_Set{PASSES}");

                yield return new TestCaseData(
                    GetParameterSnippet(false, true, true),
                    setter,
                    new object[] { "test", 0 },
                    null)
                .SetName($"{head}ContractWriteonly_Set{PASSES_IF}");

                yield return new TestCaseData(
                    GetParameterSnippet(false, true, true),
                    setter,
                    new object[] { "", 0 },
                    typeof(PreconditionException))
                .SetName($"{head}ContractWriteonly_Set{FAILS_IF}");

                yield return new TestCaseData(
                    GetParameterSnippet(true, true, false),
                    getter,
                    new object[] { "test" },
                    null)
                .SetName($"{head}NormalReadWrite_Get{PASSES}");

                yield return new TestCaseData(
                    GetParameterSnippet(true, true, false),
                    setter,
                    new object[] { "test", 0 },
                    null)
                .SetName($"{head}NormalReadWrite_Set{PASSES}");

                yield return new TestCaseData(
                    GetParameterSnippet(true, true, true),
                    getter,
                    new object[] { "test" },
                    null)
                .SetName($"{head}ContractReadWrite_Get{PASSES_IF}");

                yield return new TestCaseData(
                    GetParameterSnippet(true, true, true),
                    getter,
                    new object[] { "" },
                    typeof(PreconditionException))
                .SetName($"{head}ContractReadWrite_Get{FAILS_IF}");

                yield return new TestCaseData(
                    GetParameterSnippet(true, true, true),
                    setter,
                    new object[] { "test", 0 },
                    null)
                .SetName($"{head}ContractReadWrite_Set{PASSES_IF}");

                yield return new TestCaseData(
                    GetParameterSnippet(true, true, true),
                    setter,
                    new object[] { "", 0 },
                    typeof(PreconditionException))
                .SetName($"{head}ContractReadWrite_Set{FAILS_IF}");
            }
        }
    }
}
