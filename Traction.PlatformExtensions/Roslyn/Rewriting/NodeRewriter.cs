using System;
using Microsoft.CodeAnalysis;
using Traction.SEPrecompilation;

namespace Traction.Roslyn.Rewriting {

    public class NodeRewriter {

        public NodeRewriter(SemanticModel model, ICompileContext context, string confirmationMessage) {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (context == null) throw new ArgumentNullException(nameof(context));
         
            this.model = model;
            this.context = context;
            this.confirmationMessage = confirmationMessage ?? "Rewrite confirmed.";
        }

        private readonly ICompileContext context;
        private readonly SemanticModel model;
        private readonly string confirmationMessage;

        public NodeRewriteAttempt<TNode> Try<TNode>(TNode oldNode, Func<TNode, TNode> rewrite)
            where TNode : SyntaxNode {
            if (oldNode == null) throw new ArgumentNullException(nameof(oldNode));
            if (rewrite == null) throw new ArgumentNullException(nameof(rewrite));

            try {
                var newNode = rewrite(oldNode);

                if (ReferenceEquals(oldNode, newNode)) {
                    return NodeRewriteAttempt.Skip(oldNode);
                }
                else {
#if DEBUG
                    var diagnostic = RewriteConfirmed(
                        location: oldNode.GetLocation(),
                        message: this.confirmationMessage);
                    this.context.Diagnostics.Add(diagnostic);
#endif
                    return NodeRewriteAttempt.Success(oldNode, newNode);
                }
            }
            catch (Exception e) {
                var diagnostic = RewriteFailed(
                    location: oldNode.GetLocation(),
                    exception: e);
                this.context.Diagnostics.Add(diagnostic);

                return NodeRewriteAttempt.Failure(oldNode);
            }
        }

        private static Diagnostic RewriteConfirmed(Location location, string message) => Diagnostic.Create(
            descriptor: new DiagnosticDescriptor(
                id: $"TR{DiagnosticCodes.RewriteConfirmed:D4}",
                title: "Rewrite confirmation",
                messageFormat: message,
                category: "Traction",
                defaultSeverity: DiagnosticSeverity.Info,
                isEnabledByDefault: true),
            location: location);

        private static Diagnostic RewriteFailed(Location location, Exception exception) => Diagnostic.Create(
            descriptor: new DiagnosticDescriptor(
                id: $"TR{DiagnosticCodes.RewriteFailed:D4}",
                title: "Rewrite failed",
                messageFormat: "An error occurred while editing the syntax tree. " +
                    $"{exception.GetType()}; {exception.Message}; {exception.StackTrace}",
                category: "Traction",
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true),
            location: location);
    }
}