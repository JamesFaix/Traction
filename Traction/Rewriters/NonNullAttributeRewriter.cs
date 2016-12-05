using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    class NonNullAttributeReWriter : ContractRewriterBase<NonNullAttribute> {

        public NonNullAttributeReWriter(SemanticModel model, ICompileContext context) : base(model, context) {
        }

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

        protected override SyntaxList<StatementSyntax> CreatePrecondition(TypeInfo type, string parameterName, Location location) {
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));
            if (location == null) throw new ArgumentNullException(nameof(location));

            //Debugger.Launch();
            if (!type.Type.IsReferenceType) {
                context.Diagnostics.Add(DiagnosticProvider.NonNullAttributeCanOnlyBeAppliedToReferenceTypes(location));
                return new SyntaxList<StatementSyntax>();
            }

            var text = string.Format(preconditionTemplate, parameterName);
            var statement = SyntaxFactory.ParseStatement(text);

            return new SyntaxList<StatementSyntax>().Add(statement);
        }

        protected override SyntaxList<StatementSyntax> CreatePostcondition(TypeInfo returnType, ReturnStatementSyntax node, Location location) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (location == null) throw new ArgumentNullException(nameof(location));

            //Debugger.Launch();

            if (!returnType.Type.IsReferenceType) {
                context.Diagnostics.Add(DiagnosticProvider.NonNullAttributeCanOnlyBeAppliedToReferenceTypes(location));
                return new SyntaxList<StatementSyntax>();
            }

            var returnedExpression = node.ChildNodes().FirstOrDefault();

            if (returnedExpression == null) {
                context.Diagnostics.Add(DiagnosticProvider.NonNullAttributeCannotBeAppliedToMethodWithNoReturnType(location));
                return new SyntaxList<StatementSyntax>();
            }

            var tempVariableName = "result";
            var text = string.Format(postconditionTemplate, tempVariableName, returnedExpression);
            var statement = SyntaxFactory.ParseStatement(text);

            return new SyntaxList<StatementSyntax>().Add(statement);
        }
    }
}