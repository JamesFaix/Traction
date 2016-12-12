using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    /// <summary>
    /// Syntax rewriter for <see cref="NonEmptyAttribute"/>.
    /// </summary>
    class NonEmptyRewriter : ContractRewriterBase<NonEmptyAttribute> {

        public NonEmptyRewriter(SemanticModel model, ICompileContext context)
            : base(model, context) { }

        protected override string ExceptionMessage => "Sequence cannot be null or empty.";

        protected override ExpressionSyntax GetConditionExpression(string expression, TypeInfo expressionType) {
            var text = "";
            if (!expressionType.Type.IsValueType) { //Check for null if reference type.
                text += $"!global::System.Object.Equals({expression}, null) && ";
            }
            text += $"global::System.Linq.Enumerable.Any({expression})";
            return SyntaxFactory.ParseExpression(text);
        }

        //Applies to types implementing IEnumerable<T>
        protected override bool IsValidType(TypeInfo type) => type.IsEnumerable();

        protected override Diagnostic InvalidTypeDiagnostic(Location location) => DiagnosticFactory.Create(
            title: $"Incorrect attribute usage",
            message: $"{nameof(NonEmptyAttribute)} can only be applied to members with types implementing IEnumerable<T>.",
            location: location);
    }
}