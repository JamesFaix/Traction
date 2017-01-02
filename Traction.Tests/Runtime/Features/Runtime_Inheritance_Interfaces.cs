using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_Inheritance_Interfaces {

        private const string fixture = nameof(Runtime_Inheritance_Interfaces) + "_";

        [Test, TestCaseSource(nameof(ImplicitCases))]
        public void ImplicitImplementationTests(string sourceCode, string methodName, object[] arguments, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod(methodName);

            CustomAssert.Throws(exceptionType, method, instance, arguments);
        }

        [Test, TestCaseSource(nameof(ExplicitCases))]
        public void ExplicitImplementationTests(string sourceCode, string methodName, object[] arguments, Type exceptionType) {

            var assembly = TestHelper.GetAssembly(sourceCode);
            var type = assembly.GetType("TestClass");
            var instance = Activator.CreateInstance(type);

            var interfaceType = assembly.GetType("ITest");
            var map = type.GetInterfaceMap(interfaceType);
            var method = map.TargetMethods.First(m => m.Name == "ITest." + methodName);

            CustomAssert.Throws(exceptionType, method, instance, arguments);
        }

        private static string GetMethodSnippet(bool hasPre, bool hasPost, bool explicitImplementation) {
            var sb = new StringBuilder();
            sb.AppendLine("interface ITest {");
            sb.AppendLineIf(hasPost, "[return: NonNull]");
            sb.Append("string TestMethod(");
            sb.AppendIf(hasPre, "[NonNull]");
            sb.AppendLine("string text);");
            sb.AppendLine("}");

            sb.AppendLine("class TestClass : ITest {");
            sb.AppendIf(!explicitImplementation, "public ");
            sb.Append("string ");
            sb.AppendIf(explicitImplementation, "ITest.");
            sb.AppendLine("TestMethod(string text) {");
            sb.AppendLine("return text;");
            sb.AppendLine("}");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string GetPropertySnippet(bool hasContract, bool explicitImplementation, string initialValue = null) {
            var sb = new StringBuilder();
            sb.AppendLine("interface ITest {");
            sb.AppendLineIf(hasContract, "[NonNull]");
            sb.AppendLine("string TestProperty { get; set; }");
            sb.AppendLine("}");

            sb.AppendLine("class TestClass : ITest {");
            sb.Append("public string testField");
            sb.AppendIf(initialValue != null, $" = \"{initialValue}\"");
            sb.AppendLine(";");

            sb.AppendIf(!explicitImplementation, "public ");
            sb.Append("string ");
            sb.AppendIf(explicitImplementation, "ITest.");
            sb.AppendLine("TestProperty { ");
            sb.AppendLine("get { return testField; }");
            sb.AppendLine("set { testField = value; }");
            sb.AppendLine("}");
            sb.AppendLine("}");
            return sb.ToString();
        }
        
        private static IEnumerable<TestCaseData> ImplicitCases =>
            GetCases($"{fixture}_ImplicitImplementation_", false);

        private static IEnumerable<TestCaseData> ExplicitCases =>
           GetCases($"{fixture}_ExplicitImplementation_", true);

        private static IEnumerable<TestCaseData> GetCases(string head, bool isExplicit) {

            yield return new TestCaseData(
                GetMethodSnippet(false, false, isExplicit),
                "TestMethod",
                new object[] { null },
                null)
            .SetName($"{head}NormalMethod_{Constants.Normal}");

            yield return new TestCaseData(
                GetMethodSnippet(true, false, isExplicit),
                "TestMethod",
                new object[] { "test" },
                null)
            .SetName($"{head}PreconditionMethod_{Constants.Passes}");

            yield return new TestCaseData(
                GetMethodSnippet(true, false, isExplicit),
                "TestMethod",
                new object[] { null },
                typeof(PreconditionException))
            .SetName($"{head}PreconditionMethod_{Constants.Fails}");

            yield return new TestCaseData(
                GetMethodSnippet(false, true, isExplicit),
                "TestMethod",
                new object[] { "test" },
                null)
            .SetName($"{head}PostconditionMethod_{Constants.Passes}");

            yield return new TestCaseData(
                GetMethodSnippet(false, true, isExplicit),
                "TestMethod",
                new object[] { null },
                typeof(PostconditionException))
            .SetName($"{head}PostconditionMethod_{Constants.Fails}");


            yield return new TestCaseData(
                GetPropertySnippet(false, isExplicit),
                "get_TestProperty",
                Constants.EmptyArgs,
                null)
            .SetName($"{head}NormalProperty_Get{Constants.Normal}");

            yield return new TestCaseData(
                GetPropertySnippet(false, isExplicit),
                "set_TestProperty",
                new object[] { null },
                null)
            .SetName($"{head}NormalProperty_Set{Constants.Normal}");

            yield return new TestCaseData(
                GetPropertySnippet(true, isExplicit, "test"),
                "get_TestProperty",
                Constants.EmptyArgs,
                null)
            .SetName($"{head}ContractProperty_Get{Constants.Passes}");

            yield return new TestCaseData(
                GetPropertySnippet(true, isExplicit),
                "get_TestProperty",
                Constants.EmptyArgs,
                typeof(PostconditionException))
            .SetName($"{head}ContractProperty_Get{Constants.Fails}");

            yield return new TestCaseData(
                GetPropertySnippet(true, isExplicit),
                "set_TestProperty",
                new object[] { "test" },
                null)
            .SetName($"{head}ContractProperty_Set{Constants.Passes}");

            yield return new TestCaseData(
                GetPropertySnippet(true, isExplicit),
                "set_TestProperty",
                new object[] { null },
                typeof(PreconditionException))
            .SetName($"{head}ContractProperty_Set{Constants.Fails}");
        }
    }
}