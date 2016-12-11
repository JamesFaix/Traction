﻿using System;
using System.Linq;
using System.Text;
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

        protected override StatementSyntax CreatePrecondition(TypeInfo type, string parameterName, Location location) {
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));
            if (location == null) throw new ArgumentNullException(nameof(location));

            var text = GetPreconditionText(parameterName, type.FullName());
            var statement = SyntaxFactory.ParseStatement(text);

            return SyntaxFactory.Block(statement);
        }

        protected override StatementSyntax CreatePostcondition(TypeInfo returnType, ReturnStatementSyntax node, Location location) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (location == null) throw new ArgumentNullException(nameof(location));

            var returnedExpression = node.ChildNodes().FirstOrDefault();
            var tempVariableName = IdentifierFactory.CreatePostconditionLocal(node, model);
            var text = GetPostconditionText(returnType.FullName(), returnedExpression.ToString(), tempVariableName);
            var statement = SyntaxFactory.ParseStatement(text);
            return statement;
        }

        private string GetPreconditionText(string parameterName, string parameterTypeName) {
            var sb = new StringBuilder();
            sb.AppendLine($"if (global::System.Object.Equals({parameterName}, default({parameterTypeName})))");
            sb.AppendLine($"    throw new global::System.ArgumentException(\"Value cannot be default(T).\", nameof({parameterName}));");
            return sb.ToString();
        }

        private string GetPostconditionText(string returnTypeName, string returnedExpression, string tempVarName) {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"    {returnTypeName} {tempVarName} = {returnedExpression};");
            sb.AppendLine($"    if (global::System.Object.Equals({tempVarName}, default({returnTypeName})))");
            sb.AppendLine($"        throw new global::Traction.ReturnValueException(\"Value cannot be default(T).\");");
            sb.AppendLine($"    return {tempVarName};");
            sb.AppendLine("}");
            return sb.ToString();
        }

        //Applies to all types
        protected override bool IsValidType(TypeInfo type) => true;

        protected override Diagnostic InvalidTypeDiagnostic(Location location) {
            throw new InvalidOperationException($"{nameof(NonDefaultRewriter)} should never throw an invalid type diagnostic.");
        }
    }
}