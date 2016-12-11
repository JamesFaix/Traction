using System;
using System.Linq;
using System.Text;
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
            sb.AppendLine($"if (!global::System.Linq.Enumerable.Any({parameterName}))");
            sb.AppendLine($"    throw new global::System.ArgumentException(\"Sequence cannot be empty.\", nameof({parameterName}));");
            return sb.ToString();
        }

        private string GetPostconditionText(string returnTypeName, string returnedExpression, string tempVarName) {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"    {returnTypeName} {tempVarName} = {returnedExpression};");
            sb.AppendLine($"    if (!global::System.Linq.Enumerable.Any({tempVarName}))");
            sb.AppendLine($"        throw new global::Traction.ReturnValueException(\"Sequence cannot be empty.\");");
            sb.AppendLine($"    return {tempVarName};");
            sb.AppendLine("}");
            return sb.ToString();
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