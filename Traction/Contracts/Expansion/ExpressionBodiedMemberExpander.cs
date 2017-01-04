using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Traction.SEPrecompilation;

namespace Traction.Contracts.Expansion {

    /// <summary>
    /// Rewrites expression-bodied members as normal "blocked" members.
    /// </summary>
    internal sealed class ExpressionBodiedMemberExpander : SyntaxVisitorBase {

        public ExpressionBodiedMemberExpander(SemanticModel model, ICompileContext context, IContractProvider contractProvider)
            : base(model, context, contractProvider) { }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) => VisitBaseMethodDeclaration(node);
        public override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node) => VisitBaseMethodDeclaration(node);
        public override SyntaxNode VisitOperatorDeclaration(OperatorDeclarationSyntax node) => VisitBaseMethodDeclaration(node);
        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) => VisitBasePropertyDeclaration(node);
        public override SyntaxNode VisitIndexerDeclaration(IndexerDeclarationSyntax node) => VisitBasePropertyDeclaration(node);

        private SyntaxNode VisitBaseMethodDeclaration(BaseMethodDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            //HACK: Must cast to dynamic because expression body is only defined on subclasses
            if (((dynamic)node).ExpressionBody == null) return node;

            var expr = ((dynamic)node).ExpressionBody.Expression;
            var statement = SyntaxFactory.ReturnStatement(expr);

            return nodeRewriter
                .Try(node, n => ((dynamic)n).WithExpressionBody(null)
                                 .WithBody(SyntaxFactory.Block(statement)))
                .Result;
        }

        private SyntaxNode VisitBasePropertyDeclaration(BasePropertyDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            //HACK: Must cast to dynamic because expression body is only defined on subclasses
            if (((dynamic)node).ExpressionBody == null) return node;

            var expr = ((dynamic)node).ExpressionBody.Expression;
            var statement = SyntaxFactory.ReturnStatement(expr);

            var getter = SyntaxFactory.AccessorDeclaration(
                SyntaxKind.GetAccessorDeclaration,
                SyntaxFactory.Block(statement));

            var accessors = SyntaxFactory.AccessorList(
                SyntaxFactory.List<AccessorDeclarationSyntax>()
                .Add(getter));

            return nodeRewriter
                .Try(node, n => ((dynamic)n).WithExpressionBody(null)
                                 .WithAccessorList(accessors))
                .Result;
        }
    }
}
