using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Traction.Contracts.Semantics;
using Traction.Roslyn.Rewriting;
using Traction.Roslyn.Semantics;
using Traction.Roslyn.Syntax;
using Traction.SEPrecompilation;

namespace Traction.Contracts.Injection {

    /// <summary>
    /// Base class for contract attribute syntax rewriters.
    /// </summary>
    internal sealed class Injector : SyntaxVisitorBase {

        public Injector(SemanticModel model, ICompileContext context, IContractProvider contractProvider)
            : base(model, context, contractProvider) { }

        public sealed override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) => VisitPropertyImpl(node);
        public sealed override SyntaxNode VisitIndexerDeclaration(IndexerDeclarationSyntax node) => VisitPropertyImpl(node);
        public sealed override SyntaxNode VisitOperatorDeclaration(OperatorDeclarationSyntax node) => VisitMethodImpl(node);
        public sealed override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node) => VisitMethodImpl(node);
        public sealed override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) => VisitMethodImpl(node);

        private TNode VisitMethodImpl<TNode>(TNode node)
            where TNode : BaseMethodDeclarationSyntax {

            var symbol = model.GetMethodSymbol(node);

            if (!symbol.HasAnyContract(model)
            || context.Diagnostics.Any(d => d.Id != DiagnosticCodes.RewriteConfirmed)
            || node.IsNonImplementedMember()) {
                return node;
            }

            return nodeRewriter
                .Try(node, n => n.WithContracts(model, contractProvider), "Applied contract.")
                .Result;
        }

        private TNode VisitPropertyImpl<TNode>(TNode node)
            where TNode: BasePropertyDeclarationSyntax {

            var symbol = model.GetPropertySymbol(node);

            if (!symbol.HasAnyContract(model)
            || context.Diagnostics.Any(d => d.Id != DiagnosticCodes.RewriteConfirmed)
            || node.IsNonImplementedMember()) {
                return node;
            }

            return nodeRewriter
                .Try(node, n => n.WithContracts(model, contractProvider), "Applied contract.")
                .Result;
        }
    }
}
