using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    /// <summary>
    /// Syntax rewriter for <see cref="NonNullAttribute"/>.
    /// </summary>
    class NonNullReWriter : ContractRewriterBase<NonNullAttribute> {

        public NonNullReWriter(SemanticModel model, ICompileContext context)
            : base(model, context) { }

        protected override StatementSyntax CreatePrecondition(TypeInfo type, string parameterName, Location location) {
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));
            if (location == null) throw new ArgumentNullException(nameof(location));

            var text = GetPreconditionText(parameterName);
            var statement = SyntaxFactory.ParseStatement(text);

            return SyntaxFactory.Block(statement);
        }

        protected override StatementSyntax CreatePostcondition(TypeInfo returnType, ReturnStatementSyntax node, Location location) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (location == null) throw new ArgumentNullException(nameof(location));

            var returnedExpression = node.ChildNodes().FirstOrDefault();
            var tempVariableName = IdentifierFactory.CreatePostconditionLocal(node, model);
            var text = GetPostconditionText(returnType.FullName(), returnedExpression.ToString(), tempVariableName);
            return SyntaxFactory.ParseStatement(text);
        }

        private string GetPreconditionText(string parameterName) {
            var sb = new StringBuilder();
            sb.AppendLine($"if (global::System.Object.Equals({parameterName}, null))");
            sb.AppendLine($"    throw new global::System.ArgumentNullException(nameof({parameterName}));");
            return sb.ToString();
        }

        private string GetPostconditionText(string returnTypeName, string returnedExpression, string tempVarName) {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"    {returnTypeName} {tempVarName} = {returnedExpression};");
            sb.AppendLine($"    if (global::System.Object.Equals({tempVarName}, null))");
            sb.AppendLine($"        throw new global::Traction.ReturnValueException(\"Return value cannot be null\");");
            sb.AppendLine($"    return {tempVarName};");
            sb.AppendLine("}");
            return sb.ToString();
        }

        //Applies to reference and Nullable types
        protected override bool IsValidType(TypeInfo type) {
            return !type.Type.IsValueType
            || type.FullName().EndsWith("?");
        }

        protected override Diagnostic InvalidTypeDiagnostic(Location location) => DiagnosticFactory.Create(
            title: $"Incorrect attribute usage",
            message: $"{nameof(NonNullAttribute)} can only be applied to members with reference or nullable types.",
            location: location);
    }
}