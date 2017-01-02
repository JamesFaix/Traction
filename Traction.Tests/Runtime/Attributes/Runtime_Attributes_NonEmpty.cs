using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using static Traction.Tests.Constants;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_Attributes_NonEmpty {

        private const string NAME = "NonEmpty";
        private const string FIXTURE = nameof(Runtime_Attributes_NonEmpty) + "_";

        [Test, TestCaseSource(nameof(Cases))]
        public void Test(string sourceCode, object[] arguments, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("TestMethod");

            CustomAssert.Throws(exceptionType, method, instance, arguments);
        }
        
        private class TypeSetting {
            public string TypeName;
            public object EmptyValue;
            public object NonEmptyValue;
        }

        private static IEnumerable<TestCaseData> Cases {
            get {
                var typeSettings = new[] {
                    new TypeSetting {
                        TypeName = "String",
                        EmptyValue = "",
                        NonEmptyValue = "test"
                    },
                    new TypeSetting {
                        TypeName = "Int32[]",
                        EmptyValue = new int[0],
                        NonEmptyValue = new int[] { 1 }
                    }
                };

                foreach (var t in typeSettings) {
                    var head = $"{FIXTURE}{t.TypeName}_";

                    yield return new TestCaseData(
                        AttributeTestHelper.GetSnippet(NAME, t.TypeName, ContractTypes.None),
                        new object[] { null },
                        null)
                    .SetName($"{head}NormalMethod_{PASSES}");

                    yield return new TestCaseData(
                        AttributeTestHelper.GetSnippet(NAME, t.TypeName, ContractTypes.Pre),
                        new object[] { t.NonEmptyValue },
                        null)
                    .SetName($"{head}PreconditionMethod_{PASSES}IfArgNonEmpty");

                    yield return new TestCaseData(
                        AttributeTestHelper.GetSnippet(NAME, t.TypeName, ContractTypes.Pre),
                        new object[] { null },
                        typeof(PreconditionException))
                    .SetName($"{head}PreconditionMethod_{FAILS}IfArgNull");

                    yield return new TestCaseData(
                        AttributeTestHelper.GetSnippet(NAME, t.TypeName, ContractTypes.Pre),
                        new object[] { t.EmptyValue },
                        typeof(PreconditionException))
                    .SetName($"{head}PreconditionMethod_{FAILS}IfArgEmpty");

                    yield return new TestCaseData(
                        AttributeTestHelper.GetSnippet(NAME, t.TypeName, ContractTypes.Post),
                        new object[] { t.NonEmptyValue },
                        null)
                   .SetName($"{head}PostconditionMethod_{PASSES}IfResultNonNull");

                    yield return new TestCaseData(
                        AttributeTestHelper.GetSnippet(NAME, t.TypeName, ContractTypes.Post),
                        new object[] { null },
                        typeof(PostconditionException))
                    .SetName($"{head}PostconditionMethod_{FAILS}IfResultNull");

                    yield return new TestCaseData(
                        AttributeTestHelper.GetSnippet(NAME, t.TypeName, ContractTypes.Post),
                        new object[] { t.EmptyValue },
                        typeof(PostconditionException))
                    .SetName($"{head}PostconditionMethod_{FAILS}IfResultEmpty");
                }
            }
        }
    }
}
