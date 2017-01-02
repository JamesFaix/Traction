using System;
using System.Collections.Generic;
using NUnit.Framework;
using static Traction.Tests.Constants;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_Attributes_NonNull {

        private const string NAME = "NonNull";
        private const string FIXTURE = nameof(Runtime_Attributes_NonNull) + "_";

        [Test, TestCaseSource(nameof(Cases))]
        public void Test(string sourceCode, object[] arguments, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("TestMethod");

            CustomAssert.Throws(exceptionType, method, instance, arguments);
        }

        private static IEnumerable<TestCaseData> Cases {
            get {
                yield return new TestCaseData(
                    AttributeTestHelper.GetSnippet(NAME, "string", ContractTypes.None),
                    new object[] { null },
                    null)
                .SetName($"{FIXTURE}NormalMethod_{PASSES}");

                yield return new TestCaseData(
                    AttributeTestHelper.GetSnippet(NAME, "string", ContractTypes.Pre),
                    new object[] { "test" },
                    null)
                .SetName($"{FIXTURE}PreconditionMethod_{PASSES}IfArgNonNull");

                yield return new TestCaseData(
                    AttributeTestHelper.GetSnippet(NAME, "string", ContractTypes.Pre),
                    new object[] { null },
                    typeof(PreconditionException))
                .SetName($"{FIXTURE}PreconditionMethod_{FAILS}IfArgNull");

                yield return new TestCaseData(
                    AttributeTestHelper.GetSnippet(NAME, "string", ContractTypes.Post),
                    new object[] { "test" },
                    null)
                .SetName($"{FIXTURE}PostconditionMethod_{PASSES}IfResultNonNull");

                yield return new TestCaseData(
                    AttributeTestHelper.GetSnippet(NAME, "string", ContractTypes.Post),
                    new object[] { null },
                    typeof(PostconditionException))
                .SetName($"{FIXTURE}PostconditionMethod_{FAILS}IfResultNull");
            }
        }
    }
}
