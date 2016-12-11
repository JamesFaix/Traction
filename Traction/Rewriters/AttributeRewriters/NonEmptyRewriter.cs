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

        protected override string ExceptionMessage => "Sequence cannot be empty.";

        protected override ExpressionSyntax GetConditionExpression(string expression, string expressionType) {
            return SyntaxFactory.ParseExpression(
                $"!global::System.Linq.Enumerable.Any({expression})");
        }

        //Applies to types implementing IEnumerable<T>
        protected override bool IsValidType(TypeInfo type) {
            var interfaceNames = type.Type.AllInterfaces
                .Select(i => i.FullName())
                .ToArray();

            return interfaceNames.Any(i => i.StartsWith("global::System.Collections.Generic.IEnumerable"));
        }

        protected override Diagnostic InvalidTypeDiagnostic(Location location) => DiagnosticFactory.Create(
            title: $"Incorrect attribute usage",
            message: $"{nameof(NonEmptyAttribute)} can only be applied to members with types implementing IEnumerable<T>.",
            location: location);
    }
}