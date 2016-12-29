using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Traction.SEPrecompilation;

namespace Traction.Contracts.Expansion {

    /// <summary>
    /// Rewrites expression-bodied members as normal "blocked" members.
    /// </summary>
    internal sealed class ExpressionBodiedMemberExpander : RewriterBase {

        private ExpressionBodiedMemberExpander(SemanticModel model, ICompileContext context)
            : base(model, context, "Expanded expression-bodied member.") { }

        public static ExpressionBodiedMemberExpander Create(SemanticModel model, ICompileContext context) =>
            new ExpressionBodiedMemberExpander(model, context);

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (node.ExpressionBody == null) return node;

            var expr = node.ExpressionBody.Expression;
            var statement = SyntaxFactory.ReturnStatement(expr);

            return nodeRewriter
                .Try(node, n => n.WithExpressionBody(null)
                                 .WithBody(SyntaxFactory.Block(statement)))
                .Result;
        }

        public override SyntaxNode VisitOperatorDeclaration(OperatorDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (node.ExpressionBody == null) return node;

            var expr = node.ExpressionBody.Expression;
            var statement = SyntaxFactory.ReturnStatement(expr);

            return nodeRewriter
                .Try(node, n => n.WithExpressionBody(null)
                                 .WithBody(SyntaxFactory.Block(statement)))
                .Result;
        }

        public override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (node.ExpressionBody == null) return node;

            var expr = node.ExpressionBody.Expression;
            var statement = SyntaxFactory.ReturnStatement(expr);

            return nodeRewriter
                .Try(node, n => n.WithExpressionBody(null)
                                 .WithBody(SyntaxFactory.Block(statement)))
                .Result;
        }

        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (node.ExpressionBody == null) return node;

            var expr = node.ExpressionBody.Expression;
            var statement = SyntaxFactory.ReturnStatement(expr);

            var getter = SyntaxFactory.AccessorDeclaration(
                SyntaxKind.GetAccessorDeclaration,
                SyntaxFactory.Block(statement));

            var accessors = SyntaxFactory.AccessorList(
                SyntaxFactory.List<AccessorDeclarationSyntax>()
                .Add(getter));

            return nodeRewriter
                .Try(node, n => n.WithExpressionBody(null)
                                 .WithAccessorList(accessors))
                .Result;
        }
    }
}
