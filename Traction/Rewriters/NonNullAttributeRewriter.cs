using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    class NonNullAttributeReWriter : ContractRewriterBase<NonNullAttribute> {

        public NonNullAttributeReWriter(SemanticModel model, ICompileContext context)
            : base(model, context) { }

        private const string preconditionTemplate =
            @"if (global::System.Object.Equals({0}, null)) 
                  throw new global::System.ArgumentNullException(nameof({0}));";

        private const string postconditionTemplate =
            @"{{
                  var {0} = {1};
                  if (global::System.Object.Equals({0}, null))
                      throw new global::Traction.ReturnValueException(""Return value cannot be null."");
                  return {0};
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

            var tempVariableName = GenerateValidLocalVariableName(node);
            var text = string.Format(postconditionTemplate, tempVariableName, returnedExpression);
            return SyntaxFactory.ParseStatement(text);
        }
    }
}