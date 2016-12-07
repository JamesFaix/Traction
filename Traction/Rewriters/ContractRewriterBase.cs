using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    abstract class ContractRewriterBase<TAttribute> : RewriterBase
        where TAttribute : Attribute {

        protected ContractRewriterBase(SemanticModel model, ICompileContext context)
            : base(model, context) { }

        public sealed override SyntaxNode VisitOperatorDeclaration(OperatorDeclarationSyntax node) {
            return base.VisitOperatorDeclaration(VisitMethodImpl(node));
        }

        public sealed override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node) {
            return base.VisitConversionOperatorDeclaration(VisitMethodImpl(node));
        }

        public sealed override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) {
            return base.VisitMethodDeclaration(VisitMethodImpl(node));
        }

        public sealed override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) {
            return base.VisitPropertyDeclaration(VisitPropertyImpl(node));
        }

        private TNode VisitMethodImpl<TNode>(TNode node)
            where TNode : BaseMethodDeclarationSyntax {

            if (!node.HasAnyAttribute<TAttribute>(model)) {
                return node;
            }

            try {
                var preconditionStatements = GetMethodPreconditions(node);

                var result = InsertMethodPostconditions(node);

                return result
                    .WithBody(result.Body
                        .WithStatements(new SyntaxList<StatementSyntax>()
                            .AddRange(preconditionStatements)
                            .AddRange(result.Body.Statements)));
            }
            catch (Exception e) {
                context.Diagnostics.Add(DiagnosticProvider.ContractInjectionFailed(node.GetLocation(), e));
                return node;
            }
        }

        private PropertyDeclarationSyntax VisitPropertyImpl(PropertyDeclarationSyntax node) {

            if (!node.HasAttribute<TAttribute>(model)) {
                return node;
            }

            try {
                var propertyType = model.GetTypeInfo(node.Type);

                var location = node.GetLocation();
                var setter = InsertPropertyPrecondition(propertyType, node.Setter(), location);
                var getter = InsertPropertyPostcondition(propertyType, node.Getter(), location);

                var accessors = SyntaxFactory.AccessorList(
                    SyntaxFactory.List(
                        new[] { getter, setter }
                        .Where(a => a != null)));

                return node.WithAccessorList(accessors);
            }
            catch (Exception e) {
                context.Diagnostics.Add(DiagnosticProvider.ContractInjectionFailed(node.GetLocation(), e));
                return node;
            }
        }

        private IEnumerable<StatementSyntax> GetMethodPreconditions<TNode>(TNode node)
            where TNode : BaseMethodDeclarationSyntax {
            var preconditionParameters = node.ParameterList.Parameters
               .Where(p => p.HasAttribute<TAttribute>(model))
               .ToArray();

            if (!preconditionParameters.Any()) {
                return new StatementSyntax[0];
            }

            var location = node.GetLocation();

            return preconditionParameters
                .Select(p => CreatePrecondition(model.GetTypeInfo(p.Type), p.Identifier.ValueText, location));
        }

        private TNode InsertMethodPostconditions<TNode>(TNode node)
            where TNode : BaseMethodDeclarationSyntax {
            if (!node.HasAttribute<TAttribute>(model)) {
                return node;
            }
            
            var result = node;
            var returnType = model.GetTypeInfo(node.ReturnType());
            var location = node.GetLocation();

            return node.ReplaceReturnStatements(ret => 
                CreatePostcondition(returnType, ret, location));

            //var returnStatements = node.Body.GetAllReturnStatements().ToArray();

            //foreach (var ret in returnStatements) {
            //    var postcondition = CreatePostcondition(returnType, ret, location);
            //    var statements = result.Body.Statements.Replace(ret, postcondition);

            //    result = result.WithBody( 
            //        result.Body.WithStatements(statements));
            //}
            
            //return result;
        }

        private AccessorDeclarationSyntax InsertPropertyPrecondition(TypeInfo type, AccessorDeclarationSyntax node, Location location) {
            if (node == null) return null;

            return node.WithBody(
                SyntaxFactory.Block(CreatePrecondition(type, "value", location))
                    .AddStatements(node.Body.Statements.ToArray()));
        }

        private AccessorDeclarationSyntax InsertPropertyPostcondition(TypeInfo type, AccessorDeclarationSyntax node, Location location) {
            if (node == null) return null;

            var returnStatements = node.DescendantNodes()
                .OfType<ReturnStatementSyntax>();

            BlockSyntax body = node.Body;

            foreach (var ret in returnStatements) {
                var postcondition = CreatePostcondition(type, ret, location);
                body = body.ReplaceNode(ret, postcondition);
            }

            return node.WithBody(body);
        }

        protected abstract StatementSyntax CreatePrecondition(TypeInfo parameterType, string identifier, Location location);

        protected abstract StatementSyntax CreatePostcondition(TypeInfo returnType, ReturnStatementSyntax node, Location location);

        protected string GeneratePostconditionVariableName(ReturnStatementSyntax node) {

            CSharpSyntaxNode declaration = node.FirstAncestorOrSelf<BaseMethodDeclarationSyntax>();
            declaration = declaration ?? node.FirstAncestorOrSelf<AccessorDeclarationSyntax>();

            return declaration.GenerateUniqueMemberName(model, "result");
        }
    }
}
