﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    /// <summary>
    /// Syntax rewriter for <see cref="NonDefaultAttribute"/>.
    /// </summary>
    class NonDefaultRewriter : ContractRewriterBase<NonDefaultAttribute> {

        public NonDefaultRewriter(SemanticModel model, ICompileContext context)
            : base(model, context) { }
        
        protected override string ExceptionMessage => "Value cannot be default(T).";

        protected override ExpressionSyntax GetConditionExpression(string expression, TypeInfo expressionType) {
            var typeName = expressionType.FullName();
            if (typeName.EndsWith("?")) { //If type is nullable (T?), compare to default(T) instead of default(T?)
                typeName = typeName.Substring(0, typeName.Length - 1);
            }
            return SyntaxFactory.ParseExpression(
                $"!global::System.Object.Equals({expression}, default({typeName}))");
        }

        //Applies to all types
        protected override bool IsValidType(TypeInfo type) => true;

        protected override Diagnostic InvalidTypeDiagnostic(Location location) {
            throw new InvalidOperationException($"{nameof(NonDefaultRewriter)} should never throw an invalid type diagnostic.");
        }
    }
}