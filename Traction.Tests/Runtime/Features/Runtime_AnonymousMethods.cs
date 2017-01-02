using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_AnonymousMethods {

        private const string fixture = nameof(Runtime_AnonymousMethods) + "_";

        [Test, TestCaseSource(nameof(Cases))]
        public void BasicTests(string sourceCode, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("TestMethod");

            CustomAssert.Throws(exceptionType, method, instance, Constants.EmptyArgs);
        }

        private static string GetSnippet(bool hasPost) {
            var sb = new StringBuilder();
            sb.AppendLine("class TestClass {");

                sb.AppendLineIf(hasPost, "[return: NonNull]");
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
                    GetSnippet(false),
                    null)
                .SetName($"{fixture}NormalMethod_AnonMethod{Constants.Normal}");

                yield return new TestCaseData(
                    GetSnippet(true),
                    null)
                .SetName($"{fixture}PostconditionMethod_AnonMethod{Constants.Normal}");
            }
        }
    }
}