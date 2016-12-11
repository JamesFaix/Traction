using System;
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

        private const string preconditionTemplate = //0 = value
            @"if (!global::System.Linq.Enumerable.Any({0})) 
                  throw new global::System.ArgumentException(""Sequence cannot be empty."", nameof({0}));";

        private const string postconditionTemplate = //0 = type name, 1 = identifier, 2 = return expression
            @"{{
                  {0} {1} = {2};
                  if (!global::System.Linq.Enumerable.Any({1}))
                      throw new global::Traction.ReturnValueException(""Sequence cannot be empty."");
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
            var statement = SyntaxFactory.ParseStatement(text);
            return statement;
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