using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Traction {

    /// <summary>
    /// Base class for syntax rewriters.
    /// </summary>
    abstract class RewriterBase : CSharpSyntaxRewriter {

        protected RewriterBase(SemanticModel model, ICompileContext context) {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (context == null) throw new ArgumentNullException(nameof(context));
     
            this.model = model;
            this.context = context;
        }

        protected readonly ICompileContext context;
        protected readonly SemanticModel model;

        /// <summary>
        /// Attempts to rewrite the given node using the given function, and return the rewritten copy.
        /// Creates diagnostic and returns original node if rewrite failed.
        /// </summary>
        protected TNode TryRewrite<TNode>(TNode node, Func<TNode, TNode> rewrite)
            where TNode : SyntaxNode {

            try {
                var result = rewrite(node);

#if CONFIRM_REWRITES
                context.Diagnostics.Add(
                    DiagnosticFactory.RewriteConfirmation(
                        node.GetLocation(), 
                        RewriteConfirmationMessage));
#endif

                return result;
            }
            catch (Exception e) {
                context.Diagnostics.Add(
                    DiagnosticFactory.SyntaxRewriteFailed(
                        location: node.GetLocation(), 
                        exception: e));
                return node;
            }
        }

        protected abstract string RewriteConfirmationMessage { get; }
    }
}
