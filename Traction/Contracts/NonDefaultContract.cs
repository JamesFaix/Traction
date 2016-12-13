using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    /// <summary>
    /// Contract for <see cref="NonDefaultAttribute"/>.
    /// </summary>
   public class NonDefaultContract : Contract<NonDefaultAttribute> {
                
        public override string ExceptionMessage => "Value cannot be default(T).";

        public override ExpressionSyntax Condition(string expression, TypeInfo expressionType) {
            var symbol = expressionType.Type;
            var typeName = symbol.FullName();

            if (symbol.IsNullable()) { //If type is nullable (T?), compare to default(T) instead of default(T?)
                typeName = (symbol as INamedTypeSymbol).TypeArguments[0].FullName();
            }
            return SyntaxFactory.ParseExpression(
                $"!global::System.Object.Equals({expression}, default({typeName}))");
        }

        //Applies to all types
        public override bool IsValidType(TypeInfo type) => true;

        public override Diagnostic InvalidTypeDiagnostic(Location location) {
            throw new InvalidOperationException($"{nameof(NonDefaultContract)} should never throw an invalid type diagnostic.");
        }
    }
}