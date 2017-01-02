using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using static Traction.Tests.Constants;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_Attributes_NonDefault {

        private const string NAME = "NonDefault";
        private const string FIXTURE = nameof(Runtime_Attributes_NonDefault) + "_";

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
            public object DefaultValue;
            public object NonDefaultValue;
        }

        private static IEnumerable<TestCaseData> Cases {
            get {
                var typeSettings = new[] {
                    new TypeSetting {
                        TypeName = "String",
                        DefaultValue = null,
                        NonDefaultValue = "test"
                    },
                    new TypeSetting {
                        TypeName = "Int32",
                        DefaultValue = 0,
                        NonDefaultValue = 1
                    },
                    new TypeSetting {
                        TypeName = "Int32?",
                        DefaultValue = 0,
                        NonDefaultValue = null
                    },
                    new TypeSetting {
                        TypeName = "Action",
                        DefaultValue = null,
                        NonDefaultValue = (Action)(() => { return; })
                    },
                    new TypeSetting {
                        TypeName = "List<Double>",
                        DefaultValue = null,
                        NonDefaultValue = new List<double>() { 1.2, 3.4 }
                    }
                };

                foreach (var t in typeSettings) {
                    var head = $"{FIXTURE}{t.TypeName}_";

                    yield return new TestCaseData(
                        AttributeTestHelper.GetSnippet(NAME, t.TypeName, ContractTypes.None),
                        new object[] { t.DefaultValue },
                        null)
                    .SetName($"{head}NormalMethod_{PASSES}");

                    yield return new TestCaseData(
                        AttributeTestHelper.GetSnippet(NAME, t.TypeName, ContractTypes.Pre),
                        new object[] { t.NonDefaultValue },
                        null)
                    .SetName($"{head}PreconditionMethod_{PASSES}IfArgNonDefault");

                    yield return new TestCaseData(
                        AttributeTestHelper.GetSnippet(NAME, t.TypeName, ContractTypes.Pre),
                        new object[] { t.DefaultValue },
                        typeof(PreconditionException))
                    .SetName($"{head}PreconditionMethod_{FAILS}IfArgDefault");

                    yield return new TestCaseData(
                        AttributeTestHelper.GetSnippet(NAME, t.TypeName, ContractTypes.Post),
                        new object[] { t.NonDefaultValue },
                        null)
                    .SetName($"{head}PostconditionMethod_{PASSES}IfResultNonefault");

                    yield return new TestCaseData(
                        AttributeTestHelper.GetSnippet(NAME, t.TypeName, ContractTypes.Post),
                        new object[] { t.DefaultValue },
                        typeof(PostconditionException))
                    .SetName($"{head}PostconditionMethod_{FAILS}IfResultDefault");
                }
            }
        }
    }
}
