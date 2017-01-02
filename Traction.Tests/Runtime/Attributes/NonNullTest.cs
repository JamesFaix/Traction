using System;
using NUnit.Framework;
using System.Reflection;
using System.Collections.Generic;

namespace Traction.Tests.RuntimeBehaviors {

    [TestFixture]
    public class NonNullTest {

        private const string fixture = "Runtime_NonNull_";

        [Test, TestCaseSource(nameof(Cases))]
        public void Test(string sourceCode, string methodName, 
            object[] arguments, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod(methodName);

            CustomAssert.Throws(exceptionType, method, instance, arguments);
        }

        private static IEnumerable<TestCaseData> Cases {
            get {
                yield return new TestCaseData(
                        @"class TestClass {
                            public string TestMethod([NonNull] string text) {
                                return text;
                            }
                        }",
                        "TestMethod",
                        new object[] { null },
                        typeof(PreconditionException))
                    .SetName($"{fixture}MethodPreconditionThrowsIfContractBroken");
                
                yield return new TestCaseData(
                        @"class TestClass {
                            public string TestMethod([NonNull] string text) {
                                return text;
                            }
                        }",
                        "TestMethod",
                        new object[] { "test" },
                        null)
                    .SetName($"{fixture}MethodPreconditionDoesNotThrowIfContractMet");

                yield return new TestCaseData(
                        @"class TestClass {
                            [return: NonNull]
                            public string TestMethod(string text) {
                                return text;
                            }
                        }",
                        "TestMethod",
                        new object[] { null },
                        typeof(PostconditionException))
                    .SetName($"{fixture}MethodPostconditionThrowsIfContractBroken");

                yield return new TestCaseData(
                        @"class TestClass {
                            [return: NonNull]
                            public string TestMethod(string text) {
                                return text;
                            }
                        }",
                        "TestMethod",
                        new object[] { "test" },
                        null)
                    .SetName($"{fixture}MethodPostconditionDoesNotThrowIfContractMet");

                yield return new TestCaseData(
                        @"class TestClass {
                            private string testField;

                            [NonNull]
                            public string TestProperty {
                                set { testField = value; }
                            }
                        }",
                        "set_TestProperty",
                        new object[] { null },
                        typeof(PreconditionException))
                    .SetName($"{fixture}PropertyPreconditionThrowsIfContractBroken");

                yield return new TestCaseData(
                       @"class TestClass {
                            private string testField;

                            [NonNull]
                            public string TestProperty {
                                set { testField = value; }
                            }
                        }",
                        "set_TestProperty",
                        new object[] { "test" },
                        null)
                    .SetName($"{fixture}PropertyPreconditionDoesNotThrowIfContractMet");

                yield return new TestCaseData(
                        @"class TestClass {
                            private string testField = null;

                            [NonNull]
                            public string TestProperty {
                                get { return testField; }
                            }
                        }",
                        "get_TestProperty",
                        new object[0],
                        typeof(PostconditionException))
                    .SetName($"{fixture}PropertyPostconditionThrowsIfContractBroken");

                yield return new TestCaseData(
                       @"class TestClass {
                            private string testField = ""test"";

                            [NonNull]
                            public string TestProperty {
                                get { return testField; }
                            }
                        }",
                        "get_TestProperty",
                        new object[0],
                        null)
                    .SetName($"{fixture}PropertyPostconditionDoesNotThrowIfContractMet");
            }
        }        
    }
}
