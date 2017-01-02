using System.Text;

namespace Traction.Tests.Runtime {

    internal static class AttributeTestHelper {

        public static string GetSnippet(string attributeName, string typeName, ContractTypes contractTypes) {

            var sb = new StringBuilder();
            sb.AppendLine("class TestClass {");

            sb.AppendLineIf(contractTypes.HasFlag(ContractTypes.Post), $"[return: {attributeName}]");

            sb.Append($"public {typeName} TestMethod(");

            sb.AppendIf(contractTypes.HasFlag(ContractTypes.Pre), $"[{attributeName}]");

            sb.AppendLine($"{typeName} value) {{");
            sb.AppendLine("return value; } }");
            return sb.ToString();
        }
    }
}
