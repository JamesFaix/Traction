using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    abstract class ConcreteTypeRewriter : RewriterBase {

        protected ConcreteTypeRewriter(SemanticModel model, ICompileContext context, string confirmationMessage)
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

        private TNode VisitMembers<TNode>(TNode node)
            where TNode : CSharpSyntaxNode {

            UsedIdentifiers = IdentifierFactory.GetUsedIdentifiers(node, model).ToList();
            return nodeRewriter
                .Try(node, ExpandTypeMembers)
                .Result;
        }

        protected abstract TNode ExpandTypeMembers<TNode>(TNode typeDeclaration)
            where TNode : CSharpSyntaxNode;
    }
}
