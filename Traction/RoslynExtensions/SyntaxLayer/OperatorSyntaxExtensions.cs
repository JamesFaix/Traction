using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction.RoslynExtensions {

    static class OperatorSyntaxExtensions {

        public static string UnderlyingMethodName(this OperatorDeclarationSyntax @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            var opSymbol = @this.OperatorToken.ToString();
            var argCount = @this.ParameterList.Parameters.Count();

            switch (argCount) {
                case 1: return GetUnaryOpMethodName(opSymbol);
                case 2: return GetBinaryOpMethodName(opSymbol);
                default: throw new NotSupportedException($"Unsupported operator argument count. ({argCount})");
            }
        }

        private static string GetUnaryOpMethodName(string opSymbol) {
            switch (opSymbol) {
                case "+": return "op_UnaryPlus";
                case "-": return "op_UnaryMinus";
                case "!": return "op_LogicalNot";
                case "~": return "op_OnesCompliment";
                case "++": return "op_Increment";
                case "--": return "op_Decrement";
                case "true": return "op_True";
                case "false": return "op_False";

                default: throw new NotSupportedException($"Unsupported operator. ({opSymbol}, 1)");
            }
        }

        private static string GetBinaryOpMethodName(string opSymbol) {
            switch (opSymbol) {
                case "+": return "op_Addition";
                case "-": return "op_Subtraction";
                case "*": return "op_Multiply";
                case "/": return "op_Division";
                case "%": return "op_Modulus";
                case "&": return "op_BitwiseAnd";
                case "|": return "op_BitwiseOr";
                case "^": return "op_ExclusiveOr";
                case "<<": return "op_LeftShift";
                case ">>": return "op_RightShift";
                case "<": return "op_LessThan";
                case ">": return "op_GreaterThan";
                case "==": return "op_Equality";
                case "!=": return "op_Inequality";

                default: throw new NotSupportedException($"Unsupported operator. ({opSymbol}, 2)");
            }
        }

        public static string UnderlyingMethodName(this ConversionOperatorDeclarationSyntax @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            var explicitness = @this.ImplicitOrExplicitKeyword.ToString();

            //Capitalize first letter
            explicitness = explicitness[0].ToString().ToUpper() + 
                           explicitness.Substring(1, explicitness.Length - 1);

            return "op_" + explicitness;
        }
    }
}
