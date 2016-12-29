using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Traction.SEPrecompilation;

namespace Traction {

    internal abstract class ConcreteTypeMemberExpander<TMemberNode> : RewriterBase
        where TMemberNode : CSharpSyntaxNode {

        protected ConcreteTypeMemberExpander(SemanticModel model, ICompileContext context, string confirmationMessage)
            : base(model, context, confirmationMessage) { }

        //Accumulates used identifiers within type definition as new members are generated
        protected List<string> UsedIdentifiers { get; private set; }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            node = VisitMembers(node);
            return base.VisitClassDeclaration(node);
        }

        public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            node = VisitMembers(node);
            return base.VisitStructDeclaration(node);
        }

        private TTypeNode VisitMembers<TTypeNode>(TTypeNode node)
            where TTypeNode : CSharpSyntaxNode {

            UsedIdentifiers = IdentifierFactory.GetUsedIdentifiers(node, model).ToList();
            return nodeRewriter
                .Try(node, ExpandTypeMembers)
                .Result;
        }

        private TTypeNode ExpandTypeMembers<TTypeNode>(TTypeNode typeDeclaration)
            where TTypeNode : CSharpSyntaxNode {

            var membersToExpand = typeDeclaration
                    .DescendantNodes()
                    .OfType<TMemberNode>()
                    .Where(MemberFilter)
                    .Select(m => new {
                        Node = m,
                        Symbol = model.GetDeclaredSymbol(m)
                    });

            foreach (var mem in membersToExpand) {
                var oldMember = typeDeclaration
                    .DescendantNodes()
                    .OfType<TMemberNode>()
                    .Single(m => m.GetText().ToString() == mem.Node.GetText().ToString());

                var newMembers = ExpandMember(oldMember, mem.Symbol);

                typeDeclaration = typeDeclaration.ReplaceNode(oldMember, newMembers);
            }

            return typeDeclaration;
        }

        protected abstract bool MemberFilter(TMemberNode member);

        protected abstract SyntaxList<SyntaxNode> ExpandMember(TMemberNode member, ISymbol symbol);
    }
}
