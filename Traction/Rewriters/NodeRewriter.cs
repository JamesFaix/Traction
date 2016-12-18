using System;
using Microsoft.CodeAnalysis;

namespace Traction {

    class NodeRewriter {

        public NodeRewriter(SemanticModel model, ICompileContext context, string confirmationMessage) {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (confirmationMessage == null) throw new ArgumentNullException(nameof(confirmationMessage));

            this.model = model;
            this.context = context;
            this.confirmationMessage = confirmationMessage;
        }

        private readonly ICompileContext context;
        private readonly SemanticModel model;
        private readonly string confirmationMessage;

        #region Factory methods
        private static NodeRewriteAttempt<TNode> Success<TNode>(TNode oldNode, TNode newNode)
            where TNode : SyntaxNode =>
            new NodeRewriteAttempt<TNode>(NodeRewriteStatus.Success, oldNode, newNode);

        private static NodeRewriteAttempt<TNode> Failure<TNode>(TNode oldNode)
            where TNode : SyntaxNode =>
            new NodeRewriteAttempt<TNode>(NodeRewriteStatus.Failure, oldNode, null);

        private static NodeRewriteAttempt<TNode> Skip<TNode>(TNode oldNode)
            where TNode : SyntaxNode =>
            new NodeRewriteAttempt<TNode>(NodeRewriteStatus.Skip, oldNode, null);
        #endregion

        public NodeRewriteAttempt<TNode> Try<TNode>(TNode oldNode, Func<TNode, TNode> rewrite)
            where TNode : SyntaxNode {
            if (oldNode == null) throw new ArgumentNullException(nameof(oldNode));
            if (rewrite == null) throw new ArgumentNullException(nameof(rewrite));

            try {
                var newNode = rewrite(oldNode);

                if (ReferenceEquals(oldNode, newNode)) {
                    return Skip(oldNode);
                }
                else {
#if CONFIRM_REWRITES
                    var diagnostic = DiagnosticFactory.RewriteConfirmation(
                        location: oldNode.GetLocation(),
                        message: this.confirmationMessage);
                    this.context.Diagnostics.Add(diagnostic);
#endif
                    return Success(oldNode, newNode);
                }
            }
            catch (Exception e) {
                var diagnostic = DiagnosticFactory.SyntaxRewriteFailed(
                    location: oldNode.GetLocation(),
                    exception: e);
                this.context.Diagnostics.Add(diagnostic);

                return Failure(oldNode);
            }
        }
    }
}