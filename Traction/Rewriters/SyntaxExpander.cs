﻿using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Traction {

    /// <summary>
    /// Expands syntactic sugar in syntax trees (auto-properties, expression-bodied members, etc.)
    /// </summary>
    sealed class SyntaxExpander : RewriterBase {

        public SyntaxExpander(SemanticModel model, ICompileContext context)
            : base(model, context) { }

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
            where TNode : SyntaxNode {
            var descendants = node.DescendantNodes();

            //var markedMethods = descendants.OfType<BaseMethodDeclarationSyntax>()
            //    .Where(m => m.HasAnyAttributeExtending<ContractAttribute>(model));

            var markedProperties = descendants.OfType<PropertyDeclarationSyntax>()
                .Where(m => m.HasAttributeExtending<ContractAttribute>(model)
                    && m.IsAutoImplentedProperty());

            //foreach (var m in markedMethods) {
            //    var expanded = ExpandMethod(m);
            //    node = node.ReplaceNode(m, expanded);
            //}

            foreach (var m in markedProperties) {
                SyntaxList<SyntaxNode> expanded = ExpandProperty(m);
                node = node.ReplaceNode(m, expanded);
                var x = node;
            }

            return node;
        }

        private SyntaxList<SyntaxNode> ExpandMethod(BaseMethodDeclarationSyntax node) {
            return new SyntaxList<SyntaxNode>().Add(node);
        }

        private SyntaxList<SyntaxNode> ExpandProperty(PropertyDeclarationSyntax node) {
            if (node.IsAutoImplentedProperty()) {
                return ExpandAutoProperty(node);
            }
            else if (false) {
                //Expression-bodied member
            }
            else {
                return new SyntaxList<SyntaxNode>().Add(node);
            }
        }

        public SyntaxList<SyntaxNode> ExpandAutoProperty(PropertyDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            var getter = node.Getter();
            var setter = node.Setter();

            var propertyName = node.Identifier.ToString();
            var fieldName = node.GenerateUniqueMemberName(model, $"_{propertyName}");
            var fieldType = model.GetTypeInfo(node.Type).FullName();

            var modifiers = SyntaxFactory.TokenList(
                SyntaxFactory.ParseToken("private")
                    .WithTrailingTrivia(SyntaxFactory.Space));

            if (node.Modifiers.Any(m => m.ValueText == "static")) {
                modifiers = modifiers.Add(
                    SyntaxFactory.ParseToken("static")
                    .WithTrailingTrivia(SyntaxFactory.Space));
            }

            if (setter == null) {
                modifiers = modifiers.Add(
                    SyntaxFactory.ParseToken("readonly")
                    .WithTrailingTrivia(SyntaxFactory.Space));
            }

            var field = SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.ParseTypeName(fieldType)
                            .WithTrailingTrivia(SyntaxFactory.Space),
                        SyntaxFactory.SeparatedList<VariableDeclaratorSyntax>()
                            .Add(SyntaxFactory.VariableDeclarator(fieldName))))
                .WithModifiers(modifiers);

            getter = getter.WithBody(SyntaxFactory.Block(
                SyntaxFactory.ParseStatement($"return {fieldName};")));

            if (setter != null) {
                setter = setter.WithBody(SyntaxFactory.Block(
                    SyntaxFactory.ParseStatement($"{fieldName} = value;")));
            }

            var accessors = new[] { getter, setter }
                .Where(x => x != null);

            var result = new SyntaxList<CSharpSyntaxNode>()
                .Add(field)
                .Add(node
                    .WithAccessorList(node.AccessorList
                        .WithAccessors(new SyntaxList<AccessorDeclarationSyntax>()
                            .AddRange(accessors))));

            return result;
        }
    }
}
