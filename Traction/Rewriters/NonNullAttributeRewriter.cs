using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Precompiler {

    class NonNullAttributeReWriter : ContractRewriterBase<NonNullAttribute> {

        public NonNullAttributeReWriter(SemanticModel model) : base(model) {
        }

        private const string preconditionTemplate =
            @"if (Object.Equals({0}, null)) 
                  throw new ArgumentNullException(nameof({0}));";

        private const string postconditionTemplate =
            @"{
                  var {0} = {1};
                  if (Object.Equals({0}, null))
                      throw new ReturnValueException(""Return value cannot be null."");
                  return {0};
              }";

        protected override SyntaxList<StatementSyntax> CreatePrecondition(TypeInfo type, string parameterName) {
            if (parameterName == null) throw new ArgumentNullException(nameof(parameterName));

           // System.Diagnostics.Debugger.Launch();
            if (!type.Type.IsReferenceType) throw new NotSupportedException(
                $"{nameof(NonNullAttribute)} can only be applied to reference types.");

            var text = string.Format(preconditionTemplate, parameterName);
            var statement = SyntaxFactory.ParseStatement(text);

            return new SyntaxList<StatementSyntax>().Add(statement);
        }

        protected override SyntaxList<StatementSyntax> CreatePostcondition(TypeInfo returnType, ReturnStatementSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            System.Diagnostics.Debugger.Launch();

            if (!returnType.Type.IsReferenceType) throw new NotSupportedException(
                $"{nameof(NonNullAttribute)} can only be applied to reference types.");

            var returnedExpression = node.ChildNodes().FirstOrDefault();

            if (returnedExpression == null) throw new NotSupportedException(
                $"{nameof(NonNullAttribute)} cannot be applied to the return value of methods with no return type.");

            //Find all symbol names accessible from the defining type (excessive but thorough)
            var typeDeclaration = node.FirstAncestorOrSelf<TypeDeclarationSyntax>();
            var typeInfo = model.GetTypeInfo(typeDeclaration);
            var typeSymbol = typeInfo.Type;
            var illegalNames = model.LookupSymbols(0, typeSymbol).Select(symbol => symbol.Name);

            //Call the temporary var "result", but prepend underscores until there is no name conflict
            var tempVariableName = "result";
            while (illegalNames.Contains(tempVariableName)) {
                tempVariableName = "_" + tempVariableName;
            }

            var text = string.Format(postconditionTemplate, tempVariableName, returnedExpression);
            var statement = SyntaxFactory.ParseStatement(text);

            return new SyntaxList<StatementSyntax>().Add(statement);
        }
        
    }
}