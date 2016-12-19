using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    /// <summary>
    /// Rewrites automatically implemented properties as normal properties with backing fields.
    /// </summary>
    sealed class AutoPropertyExpander : ConcreteTypeMemberExpander<PropertyDeclarationSyntax> {

        private AutoPropertyExpander(SemanticModel model, ICompileContext context)
            : base(model, context, "Expanded automatically implemented property.") { }

        public static AutoPropertyExpander Create(SemanticModel model, ICompileContext context) =>
            new AutoPropertyExpander(model, context);

        protected override bool MemberFilter(PropertyDeclarationSyntax member) =>
            member.IsAutoImplentedProperty() &&
            member.HasAttributeExtending<PropertyDeclarationSyntax, ContractAttribute>(model);

        protected override SyntaxList<SyntaxNode> ExpandMember(PropertyDeclarationSyntax node, ISymbol symbol) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            var getter = node.Getter();
            var setter = node.Setter();

            var propertyName = node.Identifier.ToString();

            var fieldName = IdentifierFactory.CreateUnique(UsedIdentifiers, $"_{propertyName}");
            UsedIdentifiers.Add(fieldName);

            var fieldType = (symbol as IPropertySymbol).Type.FullName();

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
