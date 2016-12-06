using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

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
            
            var preconditionStatements = GetMethodPreconditions(node);

            node = InsertMethodPostconditions(node);

            node = node
                .WithBody(node.Body
                    .WithStatements(new SyntaxList<StatementSyntax>()
                        .AddRange(preconditionStatements)
                        .AddRange(node.Body.Statements)));

            return base.VisitMethodDeclaration(node);
        }

        private IEnumerable<StatementSyntax> GetMethodPreconditions(MethodDeclarationSyntax node) {
            var preconditionParameters = node.ParameterList.Parameters
               .Where(p => p.HasAttribute<TAttribute>(model))
               .ToArray();

            if (preconditionParameters.Any()) {
                var location = node.GetLocation();

                return preconditionParameters
                    .SelectMany(p => CreatePrecondition(model.GetTypeInfo(p.Type), p.Identifier.ValueText, location));
            }
            else {
                return new StatementSyntax[0];
            }
        }

        private MethodDeclarationSyntax InsertMethodPostconditions(MethodDeclarationSyntax node) {

            Debugger.Launch();

            if (node.HasAttribute<TAttribute>(model)) {
                var returnStatements = node.Body.Statements.OfType<ReturnStatementSyntax>();

                var result = node;
                var returnType = model.GetTypeInfo(node.ReturnType);
                var location = node.GetLocation();

                foreach (var ret in returnStatements) {
                    var postcondition = CreatePostcondition(returnType, ret, location);
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

                var location = node.GetLocation();
                var setter = InsertPropertyPrecondition(propertyType, node.Setter(), location);
                var getter = InsertPropertyPostcondition(propertyType, node.Getter(), location);

                var accessors = SyntaxFactory.AccessorList(
                    SyntaxFactory.List(
                        new[] { getter, setter }
                        .Where(a => a != null)));

                result = node.WithAccessorList(accessors);
            }

            return base.VisitPropertyDeclaration(result);
        }

        private AccessorDeclarationSyntax InsertPropertyPrecondition(TypeInfo type, AccessorDeclarationSyntax node, Location location) {
            if (node == null) return null;

            var statements = new SyntaxList<StatementSyntax>()
                .AddRange(CreatePrecondition(type, "value", location))
                .AddRange(node.Body.Statements);

            return node.WithBody(node.Body
                .WithStatements(statements));
        }

        private AccessorDeclarationSyntax InsertPropertyPostcondition(TypeInfo type, AccessorDeclarationSyntax node, Location location) {
            if (node == null) return null;

            var returnStatements = node.DescendantNodes()
                .OfType<ReturnStatementSyntax>();

            BlockSyntax body = node.Body;

            foreach (var ret in returnStatements) {
                body = body.ReplaceNode(ret, CreatePostcondition(type, ret, location));
            }

            return node.WithBody(body);
        }

        protected abstract SyntaxList<StatementSyntax> CreatePrecondition(TypeInfo parameterType, string identifier, Location location);

        protected abstract SyntaxList<StatementSyntax> CreatePostcondition(TypeInfo returnType, ReturnStatementSyntax node, Location location);

    }
}
