using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using static Traction.Tests.Constants;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_AnonymousMethods {

        private const string FIXTURE = nameof(Runtime_AnonymousMethods) + "_";

        [Test, TestCaseSource(nameof(Cases))]
        public void BasicTests(string sourceCode, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("TestMethod");

            CustomAssert.Throws(exceptionType, method, instance, NO_ARGS);
        }

        private static string GetSnippet(ContractTypes contractTypes) {
            var sb = new StringBuilder();
            sb.AppendLine("class TestClass {");

                sb.AppendLineIf(contractTypes.HasFlag(ContractTypes.Post), "[return: NonNull]");
                sb.AppendLine("public string TestMethod() {");

                    //This lambda's return statement should not be affected.
                    sb.AppendLine(@"Func<string> someLambda = () => { return null; };");
                    sb.AppendLine(@"var someResult = someLambda();");

                    sb.AppendLine(@"return ""text"";");
                sb.AppendLine("}");

            sb.AppendLine("}");
            return sb.ToString();
        }

        private static IEnumerable<TestCaseData> Cases {
            get {
                yield return new TestCaseData(
                    GetSnippet(ContractTypes.None),
                    null)
                .SetName($"{FIXTURE}NormalMethod_AnonMethod{PASSES}");

                yield return new TestCaseData(
                    GetSnippet(ContractTypes.Post),
                    null)
                .SetName($"{FIXTURE}PostconditionMethod_AnonMethod{PASSES}");
            }
        }
    }
}