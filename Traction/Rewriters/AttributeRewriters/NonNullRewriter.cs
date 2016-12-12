﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    /// <summary>
    /// Syntax rewriter for <see cref="NonNullAttribute"/>.
    /// </summary>
    class NonNullReWriter : ContractRewriterBase<NonNullAttribute> {

        public NonNullReWriter(SemanticModel model, ICompileContext context)
            : base(model, context) { }
        
        protected override string ExceptionMessage => "Value cannot be null.";

        protected override ExpressionSyntax GetConditionExpression(string expression, TypeInfo expressionType) {
            return SyntaxFactory.ParseExpression(
                $"!global::System.Object.Equals({expression}, null)");
        }

        //Applies to reference and Nullable types
        protected override bool IsValidType(TypeInfo type) => type.Type.CanBeNull();

        protected override Diagnostic InvalidTypeDiagnostic(Location location) => DiagnosticFactory.Create(
            title: $"Incorrect attribute usage",
            message: $"{nameof(NonNullAttribute)} can only be applied to members with reference or nullable types.",
            location: location);
    }
}