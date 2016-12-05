using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    abstract class ContractRewriterBase<TAttribute> : CSharpSyntaxRewriter
        where TAttribute : Attribute {
        
        protected ContractRewriterBase(SemanticModel model, ICompileContext context) {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (context == null) throw new ArgumentNullException(nameof(context));

            this.model = model;
            this.context = context;
        }

        protected readonly ICompileContext context;
        protected readonly SemanticModel model;

        public sealed override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) {
           //Debugger.Launch();
            return base.VisitMethodDeclaration(
                InsertMethodPostconditions(
                   InsertMethodPreconditions(node)));
        }

        private MethodDeclarationSyntax InsertMethodPreconditions(MethodDeclarationSyntax node) {
            var preconditionParameters = node.ParameterList.Parameters
               .Where(p => p.HasAttribute<TAttribute>(model))
               .ToArray();

            if (preconditionParameters.Any()) {
                var preconditions = preconditionParameters
                    .SelectMany(p => CreatePrecondition(model.GetTypeInfo(p.Type), p.Identifier.ValueText));

                var statements = new SyntaxList<StatementSyntax>()
                    .AddRange(preconditions)
                    .AddRange(node.Body.Statements);

                return node.WithBody(node.Body
                    .WithStatements(statements));
            }
            else {
                return node;
            }
        }

        private MethodDeclarationSyntax InsertMethodPostconditions(MethodDeclarationSyntax node) {

            Debugger.Launch();

            if (node.HasReturnValueAttribute<TAttribute>(model)) {
                var returnStatements = node.Body.Statements.OfType<ReturnStatementSyntax>();

                var result = node;
                var returnType = model.GetTypeInfo(node.ReturnType);

                foreach (var ret in returnStatements) {
                    var postcondition = CreatePostcondition(returnType, ret);
                    result = result.ReplaceNode(ret, postcondition);
                }

                return result;
            }
            else {
                return node;
            }
        }

        public sealed override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) {
            //Debugger.Launch();

            var result = node;

            if (node.HasAttribute<TAttribute>(model)) {
                var propertyType = model.GetTypeInfo(node.Type);

                var setter = InsertPropertyPrecondition(propertyType, node.Setter());
                var getter = InsertPropertyPostcondition(propertyType, node.Getter());

                var accessors = SyntaxFactory.AccessorList(
                    SyntaxFactory.List(
                        new[] { getter, setter }
                        .Where(a => a != null)));

                result = node.WithAccessorList(accessors);
            }

            return base.VisitPropertyDeclaration(result);
        }

        private AccessorDeclarationSyntax InsertPropertyPrecondition(TypeInfo type, AccessorDeclarationSyntax node) {
            if (node == null) return null;

            var statements = new SyntaxList<StatementSyntax>()
                .AddRange(CreatePrecondition(type, "value"))
                .AddRange(node.Body.Statements);

            return node.WithBody(node.Body
                .WithStatements(statements));
        }

        private AccessorDeclarationSyntax InsertPropertyPostcondition(TypeInfo type, AccessorDeclarationSyntax node) {
            if (node == null) return null;

            var returnStatements = node.DescendantNodes()
                .OfType<ReturnStatementSyntax>();

            BlockSyntax body = node.Body;

            foreach (var ret in returnStatements) {
                body = body.ReplaceNode(ret, CreatePostcondition(type, ret));
            }

            return node.WithBody(body);
        }

        protected abstract SyntaxList<StatementSyntax> CreatePrecondition(TypeInfo parameterType, string identifier);

        protected abstract SyntaxList<StatementSyntax> CreatePostcondition(TypeInfo returnType, ReturnStatementSyntax node);

    }
}
