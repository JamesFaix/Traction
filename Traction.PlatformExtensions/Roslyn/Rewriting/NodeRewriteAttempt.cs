using System;
using Microsoft.CodeAnalysis;

namespace Traction.Roslyn.Rewriting {

    public class NodeRewriteAttempt<TNode> where TNode : SyntaxNode {

        internal NodeRewriteAttempt(NodeRewriteStatus status, TNode oldNode, TNode newNode) {
            Status = status;
            OldNode = oldNode;
            NewNode = newNode;
        }

        public NodeRewriteStatus Status { get; }

        public TNode OldNode { get; }

        public TNode NewNode { get; }

        public TNode Result =>
            Status == NodeRewriteStatus.Success
                ? NewNode
                : OldNode;
    }

    internal static class NodeRewriteAttempt {

        public static NodeRewriteAttempt<TNode> Success<TNode>(TNode oldNode, TNode newNode)
           where TNode : SyntaxNode {
            if (oldNode == null) throw new ArgumentNullException(nameof(oldNode));
            return new NodeRewriteAttempt<TNode>(NodeRewriteStatus.Success, oldNode, newNode);
        }

        public static NodeRewriteAttempt<TNode> Failure<TNode>(TNode oldNode)
            where TNode : SyntaxNode {
            if (oldNode == null) throw new ArgumentNullException(nameof(oldNode));
            return new NodeRewriteAttempt<TNode>(NodeRewriteStatus.Failure, oldNode, null);
        }

        public static NodeRewriteAttempt<TNode> Skip<TNode>(TNode oldNode)
            where TNode : SyntaxNode {
            if (oldNode == null) throw new ArgumentNullException(nameof(oldNode));
            return new NodeRewriteAttempt<TNode>(NodeRewriteStatus.Skip, oldNode, null);
        }
    }
}