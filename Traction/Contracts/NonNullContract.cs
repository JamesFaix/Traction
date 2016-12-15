using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    /// <summary>
    /// Contract for <see cref="NonNullAttribute"/>.
    /// </summary>
    public class NonNullContract : Contract<NonNullAttribute> {
                
        public override string ExceptionMessage => "Value cannot be null.";

        public override ExpressionSyntax Condition(string expression, TypeInfo expressionType) {
            return SyntaxFactory.ParseExpression(
                $"!global::System.Object.Equals({expression}, null)");
        }

        //Applies to reference and Nullable types
        public override bool IsValidType(TypeInfo type) => type.Type.IsReferenceType;

        protected override string InvalidTypeDiagnosticMessage =>
            $"{nameof(NonNullAttribute)} can only be applied to members with reference types.";
    }
}