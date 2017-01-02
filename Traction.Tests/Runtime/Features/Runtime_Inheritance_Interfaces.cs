using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using static Traction.Tests.Constants;

namespace Traction.Tests.Runtime {

    [TestFixture]
    public class Runtime_Inheritance_Interfaces {

        private const string FIXTURE = nameof(Runtime_Inheritance_Interfaces) + "_";

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

        private static string GetMethodSnippet(ContractTypes contractTypes, bool explicitImplementation) {
            var sb = new StringBuilder();
            sb.AppendLine("interface ITest {");
            sb.AppendLineIf(contractTypes.HasFlag(ContractTypes.Post), "[return: NonNull]");
            sb.Append("string TestMethod(");
            sb.AppendIf(contractTypes.HasFlag(ContractTypes.Pre), "[NonNull]");
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
            GetCases($"{FIXTURE}_ImplicitImplementation_", false);

        private static IEnumerable<TestCaseData> ExplicitCases =>
           GetCases($"{FIXTURE}_ExplicitImplementation_", true);

        private static IEnumerable<TestCaseData> GetCases(string head, bool isExplicit) {

            yield return new TestCaseData(
                GetMethodSnippet(ContractTypes.None, isExplicit),
                "TestMethod",
                new object[] { null },
                null)
            .SetName($"{head}NormalMethod_{PASSES}");

            yield return new TestCaseData(
                GetMethodSnippet(ContractTypes.Pre, isExplicit),
                "TestMethod",
                new object[] { "test" },
                null)
            .SetName($"{head}PreconditionMethod_{PASSES_IF}");

            yield return new TestCaseData(
                GetMethodSnippet(ContractTypes.Pre, isExplicit),
                "TestMethod",
                new object[] { null },
                typeof(PreconditionException))
            .SetName($"{head}PreconditionMethod_{FAILS_IF}");

            yield return new TestCaseData(
                GetMethodSnippet(ContractTypes.Post, isExplicit),
                "TestMethod",
                new object[] { "test" },
                null)
            .SetName($"{head}PostconditionMethod_{PASSES_IF}");

            yield return new TestCaseData(
                GetMethodSnippet(ContractTypes.Post, isExplicit),
                "TestMethod",
                new object[] { null },
                typeof(PostconditionException))
            .SetName($"{head}PostconditionMethod_{FAILS_IF}");


            yield return new TestCaseData(
                GetPropertySnippet(false, isExplicit),
                "get_TestProperty",
                NO_ARGS,
                null)
            .SetName($"{head}NormalProperty_Get{PASSES}");

            yield return new TestCaseData(
                GetPropertySnippet(false, isExplicit),
                "set_TestProperty",
                new object[] { null },
                null)
            .SetName($"{head}NormalProperty_Set{PASSES}");

            yield return new TestCaseData(
                GetPropertySnippet(true, isExplicit, "test"),
                "get_TestProperty",
                NO_ARGS,
                null)
            .SetName($"{head}ContractProperty_Get{PASSES_IF}");

            yield return new TestCaseData(
                GetPropertySnippet(true, isExplicit),
                "get_TestProperty",
                NO_ARGS,
                typeof(PostconditionException))
            .SetName($"{head}ContractProperty_Get{FAILS_IF}");

            yield return new TestCaseData(
                GetPropertySnippet(true, isExplicit),
                "set_TestProperty",
                new object[] { "test" },
                null)
            .SetName($"{head}ContractProperty_Set{PASSES_IF}");

            yield return new TestCaseData(
                GetPropertySnippet(true, isExplicit),
                "set_TestProperty",
                new object[] { null },
                typeof(PreconditionException))
            .SetName($"{head}ContractProperty_Set{FAILS_IF}");
        }
    }
}