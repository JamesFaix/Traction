using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    /// <summary>
    /// Expands automatically implemented properties.
    /// </summary>
    sealed class AutoPropertyExpander : RewriterBase {

        public AutoPropertyExpander(SemanticModel model, ICompileContext context)
            : base(model, context) { }

        private CSharpSyntaxNode originalTypeDeclarationNode;

        //Accumulates used identifiers within type definition as new members are generated
        private List<string> usedIdentifiers;

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

            this.originalTypeDeclarationNode = node;
            this.usedIdentifiers = IdentifierFactory.GetUsedIdentifiers(node, model).ToList();

            node = ExpandAutoProperties(node);

            return node;
        }

        private TNode ExpandAutoProperties<TNode>(TNode typeDeclaration)
            where TNode : CSharpSyntaxNode {

            var propertiesToExpand = typeDeclaration.DescendantNodes()
                .OfType<PropertyDeclarationSyntax>()
                .Where(prop => prop.HasAttributeExtending<ContractAttribute>(model)
                    && prop.IsAutoImplentedProperty())
                .Select(prop => new {
                    Name = prop.Identifier.ValueText,
                    Type = model.GetTypeInfo(prop.Type)
                });

            foreach (var prop in propertiesToExpand) {
                var oldProperty = typeDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>()
                    .Single(p => p.Identifier.ValueText == prop.Name);

                var newProperty = ExpandAutoProperty(oldProperty, prop.Type);

                typeDeclaration = typeDeclaration.ReplaceNode(oldProperty, newProperty);
            }

            return typeDeclaration;
        }

        private SyntaxList<SyntaxNode> ExpandAutoProperty(PropertyDeclarationSyntax node, TypeInfo propertyType) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            var getter = node.Getter();
            var setter = node.Setter();

            var propertyName = node.Identifier.ToString();

            var fieldName = IdentifierFactory.CreateUnique(this.usedIdentifiers, $"_{propertyName}");
            this.usedIdentifiers.Add(fieldName);

            var fieldType = propertyType.FullName();

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
