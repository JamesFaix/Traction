using Microsoft.CodeAnalysis;

namespace Traction {

    class NodeRewriteAttempt<TNode> where TNode : SyntaxNode {

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
}