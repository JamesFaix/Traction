using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    /// <summary>
    /// Base class for contract attribute syntax rewriters.
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    abstract class ContractRewriterBase<TAttribute> : RewriterBase
        where TAttribute : Attribute {

        protected ContractRewriterBase(SemanticModel model, ICompileContext context)
            : base(model, context) { }

        public sealed override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) => VisitPropertyImpl(node);
        public sealed override SyntaxNode VisitOperatorDeclaration(OperatorDeclarationSyntax node) => VisitMethodImpl(node);
        public sealed override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node) => VisitMethodImpl(node);

        public sealed override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) {
            if (node.ReturnType.GetText().ToString() == "void") {
                context.Diagnostics.Add(DiagnosticProvider.ContractAttributeCannotBeAppliedToMethodReturningVoid(node.GetLocation()));
                VisitMethodImpl(node); //Visit to check for other errors, but ignore returned value
                return node; //Return original node
            }
            else {
                return VisitMethodImpl(node);
            }
        }

        private TNode VisitMethodImpl<TNode>(TNode node)
            where TNode : BaseMethodDeclarationSyntax =>
            node.HasAnyAttribute<TAttribute>(model)
                ? TryRewrite(node, VisitMethodImplInner)
                : node;

        private PropertyDeclarationSyntax VisitPropertyImpl(PropertyDeclarationSyntax node) =>
            node.HasAttribute<TAttribute>(model)
                ? TryRewrite(node, VisitPropertyImplInner)
                : node;

        private TNode VisitMethodImplInner<TNode>(TNode node)
            where TNode : BaseMethodDeclarationSyntax {

            var preconditionStatements = GetMethodPreconditions(node);

            var result = InsertMethodPostconditions(node);

            return result
                .WithBody(result.Body
                    .WithStatements(new SyntaxList<StatementSyntax>()
                        .AddRange(preconditionStatements)
                        .AddRange(result.Body.Statements)));
        }

        private PropertyDeclarationSyntax VisitPropertyImplInner(PropertyDeclarationSyntax node) {
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

            var returnStatements = node.GetAllReturnStatements();

            return node.ReplaceNodes(returnStatements,
                (oldNode, newNode) => CreatePostcondition(returnType, newNode, location));
        }

        private AccessorDeclarationSyntax InsertPropertyPrecondition(TypeInfo type, AccessorDeclarationSyntax node, Location location) {
            if (node == null) return null;

            return node.WithBody(
                SyntaxFactory.Block(CreatePrecondition(type, "value", location))
                    .AddStatements(node.Body.Statements.ToArray()));
        }

        private AccessorDeclarationSyntax InsertPropertyPostcondition(TypeInfo type, AccessorDeclarationSyntax node, Location location) {
            if (node == null) return null;

            var returnStatements = node.GetAllReturnStatements();

            return node.ReplaceNodes(returnStatements,
                (oldNode, newNode) => CreatePostcondition(type, newNode, location));
        }

        protected abstract StatementSyntax CreatePrecondition(TypeInfo parameterType, string identifier, Location location);

        protected abstract StatementSyntax CreatePostcondition(TypeInfo returnType, ReturnStatementSyntax node, Location location);

        protected abstract bool IsValidType(TypeInfo type);
    }
}
