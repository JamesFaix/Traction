using System;
using System.Collections.Generic;
using NUnit.Framework;
using static Traction.Tests.Constants;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_Attributes_BasicComparisons {

        private const string FIXTURE = nameof(Runtime_Attributes_BasicComparisons) + "_";

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
            public object ZeroValue;
            public object PositiveValue;
            public object NegativeValue;
        }

        private class AttributeSetting {
            public string AttributeName;
            public bool ZeroPasses;
            public bool PositivePasses;
            public bool NegativePasses;
        }

        private static TypeSetting[] TypeSettings {
            get {
                return new[] {
                    new TypeSetting {
                        TypeName = "Int32",
                        ZeroValue = 0,
                        PositiveValue = 1,
                        NegativeValue = -1
                    },
                    new TypeSetting {
                        TypeName = "Int32?",
                        ZeroValue = 0,
                        PositiveValue = 1,
                        NegativeValue = -1
                    }
                };
            }
        }

        private static AttributeSetting[] AttributeSettings {
            get {
                return new[] {
                    new AttributeSetting {
                        AttributeName = "Positive",
                        ZeroPasses = false,
                        PositivePasses = true,
                        NegativePasses = false
                    },
                    new AttributeSetting {
                        AttributeName = "Negative",
                        ZeroPasses = false,
                        PositivePasses = false,
                        NegativePasses = true
                    },
                    new AttributeSetting {
                        AttributeName = "NonPositive",
                        ZeroPasses = true,
                        PositivePasses = false,
                        NegativePasses = true
                    },
                    new AttributeSetting {
                        AttributeName = "NonNegative",
                        ZeroPasses = true,
                        PositivePasses = true,
                        NegativePasses = false
                    },
                };
            }
        }

        private static IEnumerable<TestCaseData> Cases {
            get {
                foreach (var a in AttributeSettings) {
                    foreach (var t in TypeSettings) {
                        var head = $"{FIXTURE}{a.AttributeName}_{t.TypeName}";

                        yield return new TestCaseData(
                            AttributeTestHelper.GetSnippet(a.AttributeName, t.TypeName, ContractTypes.None),
                            new object[] { null },
                            null)
                        .SetName($"{head}NormalMethod_{PASSES}");

                        yield return new TestCaseData(
                            AttributeTestHelper.GetSnippet(a.AttributeName, t.TypeName, ContractTypes.Pre),
                            new object[] { t.ZeroValue },
                            a.ZeroPasses ? null : typeof(PreconditionException))
                        .SetName($"{head}PreconditionMethod_ZeroValue{(a.ZeroPasses ? PASSES : FAILS)}");

                        yield return new TestCaseData(
                            AttributeTestHelper.GetSnippet(a.AttributeName, t.TypeName, ContractTypes.Pre),
                            new object[] { t.PositiveValue },
                            a.PositivePasses ? null : typeof(PreconditionException))
                        .SetName($"{head}PreconditionMethod_PositiveValue{(a.PositivePasses ? PASSES : FAILS)}");

                        yield return new TestCaseData(
                            AttributeTestHelper.GetSnippet(a.AttributeName, t.TypeName, ContractTypes.Pre),
                            new object[] { t.NegativeValue },
                            a.NegativePasses ? null : typeof(PreconditionException))
                        .SetName($"{head}PreconditionMethod_NegativeValue{(a.NegativePasses ? PASSES : FAILS)}");

                        yield return new TestCaseData(
                           AttributeTestHelper.GetSnippet(a.AttributeName, t.TypeName, ContractTypes.Post),
                           new object[] { t.ZeroValue },
                           a.ZeroPasses ? null : typeof(PostconditionException))
                       .SetName($"{head}PostconditionMethod_ZeroValue{(a.ZeroPasses ? PASSES : FAILS)}");

                        yield return new TestCaseData(
                            AttributeTestHelper.GetSnippet(a.AttributeName, t.TypeName, ContractTypes.Post),
                            new object[] { t.PositiveValue },
                            a.PositivePasses ? null : typeof(PostconditionException))
                        .SetName($"{head}PostconditionMethod_PositiveValue{(a.PositivePasses ? PASSES : FAILS)}");

                        yield return new TestCaseData(
                            AttributeTestHelper.GetSnippet(a.AttributeName, t.TypeName, ContractTypes.Post),
                            new object[] { t.NegativeValue },
                            a.NegativePasses ? null : typeof(PostconditionException))
                        .SetName($"{head}PostconditionMethod_NegativeValue{(a.NegativePasses ? PASSES : FAILS)}");

                    }

                    //Null should pass for all comparison contracts on nullable types
                    yield return new TestCaseData(
                        AttributeTestHelper.GetSnippet(a.AttributeName, "Int32?", ContractTypes.Pre),
                        new object[] { null },
                        null)
                    .SetName($"{FIXTURE}{a.AttributeName}_Int32?PeeconditionMethod_Null{PASSES}");

                    yield return new TestCaseData(
                        AttributeTestHelper.GetSnippet(a.AttributeName, "Int32?", ContractTypes.Post),
                        new object[] { null },
                        null)
                    .SetName($"{FIXTURE}{a.AttributeName}_Int32?PostconditionMethod_Null{PASSES}");
                }
            }
        }
    }
}