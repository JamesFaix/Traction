using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Traction.Contracts;
using Traction.Roslyn;
using Traction.SEPrecompilation;

namespace Traction {

    /// <summary>
    /// Rewrites automatically implemented properties as normal properties with backing fields.
    /// </summary>
    internal sealed class AutoPropertyExpander : ConcreteTypeMemberExpander<PropertyDeclarationSyntax> {

        private AutoPropertyExpander(SemanticModel model, ICompileContext context)
            : base(model, context, "Expanded automatically implemented property.") { }

        public static AutoPropertyExpander Create(SemanticModel model, ICompileContext context) =>
            new AutoPropertyExpander(model, context);

        protected override bool MemberFilter(PropertyDeclarationSyntax member) =>
            member.IsAutoImplentedProperty() &&
            model.GetPropertySymbol(member)
                .HasAnyContract(model);

        protected override SyntaxList<SyntaxNode> ExpandMember(PropertyDeclarationSyntax node, ISymbol symbol) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            var getter = node.Getter();
            var setter = node.Setter();

            var fieldName = GetFieldName(node);
            var fieldType = (symbol as IPropertySymbol).Type.FullName();
            var fieldModifiers = GetFieldModifiers(node, setter != null);
            var field = GetField(fieldName, fieldType, fieldModifiers);
            var accessors = GetAccessors(getter, setter, fieldName);

            return new SyntaxList<CSharpSyntaxNode>()
                .Add(field)
                .Add(node.WithAccessorList(accessors));
        }

        private string GetFieldName(PropertyDeclarationSyntax node) {
            var propertyName = node.Identifier.ToString();
            var result = IdentifierFactory.CreateUnique(UsedIdentifiers, $"_{propertyName}");
            UsedIdentifiers.Add(result);
            return result;
        }

        private SyntaxTokenList GetFieldModifiers(PropertyDeclarationSyntax node, bool hasSetter) {
            var result = SyntaxFactory.TokenList(
                SyntaxFactory.ParseToken("private "));

            if (node.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword))) {
                result = result.Add(
                    SyntaxFactory.ParseToken("static "));
            }

            if (!hasSetter) {
                result = result.Add(
                    SyntaxFactory.ParseToken("readonly "));
            }

            return result;
        }

        private FieldDeclarationSyntax GetField(string name, string typeName, SyntaxTokenList modifiers) {
            return SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.ParseTypeName(typeName)
                            .WithTrailingTrivia(SyntaxFactory.Space),
                        SyntaxFactory.SeparatedList<VariableDeclaratorSyntax>()
                            .Add(SyntaxFactory.VariableDeclarator(name))))
                .WithModifiers(modifiers);
        }

        private AccessorListSyntax GetAccessors(AccessorDeclarationSyntax getter,
            AccessorDeclarationSyntax setter, string fieldName) {

            getter = getter.WithBody(SyntaxFactory.Block(
                SyntaxFactory.ParseStatement($"return {fieldName};")));

            if (setter != null) {
                setter = setter.WithBody(SyntaxFactory.Block(
                    SyntaxFactory.ParseStatement($"{fieldName} = value;")));
            }

            var accessors = new[] { getter, setter }
                .Where(x => x != null)
                .ToArray();

            return SyntaxFactory.AccessorList()
                .AddAccessors(accessors);
        }
    }
}
