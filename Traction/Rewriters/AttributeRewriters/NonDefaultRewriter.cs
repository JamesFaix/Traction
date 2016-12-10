using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    /// <summary>
    /// Syntax rewriter for <see cref="NonDefaultAttribute"/>.
    /// </summary>
    class NonDefaultReWriter : ContractRewriterBase<NonDefaultAttribute> {

        public NonDefaultReWriter(SemanticModel model, ICompileContext context)
            : base(model, context) { }

        private const string preconditionTemplate = //0 = value, 1 = type name
            @"if (global::System.Object.Equals({0}, default({1}))) 
                  throw new global::System.ArgumentException(""Value cannot be default({1})."", nameof({0}));";

        private const string postconditionTemplate = //0 = type name, 1 = identifier, 2 = return expression
            @"{{
                  {0} {1} = {2};
                  if (global::System.Object.Equals({1}, default({0})))
                      throw new global::Traction.ReturnValueException(""Return value cannot be default({0})."");
                  return {1};
              }}";

        protected override StatementSyntax CreatePrecondition(TypeInfo type, string parameterName, Location location) {
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));
            if (location == null) throw new ArgumentNullException(nameof(location));

            var text = string.Format(preconditionTemplate, parameterName, type.FullName());
            var statement = SyntaxFactory.ParseStatement(text);

            return SyntaxFactory.Block(statement);
        }

        protected override StatementSyntax CreatePostcondition(TypeInfo returnType, ReturnStatementSyntax node, Location location) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (location == null) throw new ArgumentNullException(nameof(location));

            var returnedExpression = node.ChildNodes().FirstOrDefault();
            var tempVariableName = IdentifierFactory.CreatePostconditionLocal(node, model);
            var text = string.Format(postconditionTemplate, returnType.FullName(), tempVariableName, returnedExpression);
            return SyntaxFactory.ParseStatement(text);
        }
    }
}