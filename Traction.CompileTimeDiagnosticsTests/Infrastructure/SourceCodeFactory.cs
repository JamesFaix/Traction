using System.Text;

namespace Traction.DiagnosticsTests {

    static class SourceCodeFactory {

        public static string BasicUsingDirectives =>
            @"using System;
                using System.Collections.Generic;
                using Traction;
            ";

        public static string ClassWithMethods(params string[] members) {
            var sb = new StringBuilder();
            sb.AppendLine("class TestClass {");

            foreach (var m in members) {
                sb.AppendLine(m);
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        public static string PreconditionMethod(string typeName, string attributeName) =>
            $"void TestMethod([{attributeName}] {typeName} param1) {{ }}";

        public static string PostconditionMethod(string typeName, string attributeName) =>
            $"[return: {attributeName}] {typeName} TestMethod() {{ }}";

    }
}
