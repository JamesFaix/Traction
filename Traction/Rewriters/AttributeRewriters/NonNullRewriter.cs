using System;
using System.Linq;
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

        private const string preconditionTemplate = //0 = value
            @"if (global::System.Object.Equals({0}, null)) 
                  throw new global::System.ArgumentNullException(nameof({0}));";

        private const string postconditionTemplate = //0 = type name, 1 = var identifier, 2 = return expression
            @"{{
                  {0} {1} = {2};
                  if (global::System.Object.Equals({1}, null))
                      throw new global::Traction.ReturnValueException(""Return value cannot be null."");
                  return {1};
              }}";

        protected override StatementSyntax CreatePrecondition(TypeInfo type, string parameterName, Location location) {
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));
            if (location == null) throw new ArgumentNullException(nameof(location));

            if (!type.Type.IsReferenceType) {
                context.Diagnostics.Add(DiagnosticProvider.NonNullAttributeCanOnlyBeAppliedToReferenceTypes(location));
                return SyntaxFactory.Block();
            }

            var text = string.Format(preconditionTemplate, parameterName);
            var statement = SyntaxFactory.ParseStatement(text);

            return SyntaxFactory.Block(statement);
        }

        protected override StatementSyntax CreatePostcondition(TypeInfo returnType, ReturnStatementSyntax node, Location location) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (location == null) throw new ArgumentNullException(nameof(location));

            if (!returnType.Type.IsReferenceType) {
                context.Diagnostics.Add(DiagnosticProvider.NonNullAttributeCanOnlyBeAppliedToReferenceTypes(location));
                return node;
            }

            var returnedExpression = node.ChildNodes().FirstOrDefault();

            if (returnedExpression == null) {
                context.Diagnostics.Add(DiagnosticProvider.NonNullAttributeCannotBeAppliedToMethodWithNoReturnType(location));
                return node;
            }

            var typeName = returnType.FullName();
            var tempVariableName = IdentifierFactory.CreatePostconditionLocal(node, model);
            var text = string.Format(postconditionTemplate, typeName, tempVariableName, returnedExpression);
            return SyntaxFactory.ParseStatement(text);
        }
    }
}