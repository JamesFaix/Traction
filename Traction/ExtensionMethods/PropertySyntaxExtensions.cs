using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace Traction {

    /// <summary>
    /// Extension methods for <see cref="PropertyDeclarationSyntax"/> 
    /// </summary>
    static class PropertySyntaxExtensions {

        public static AccessorDeclarationSyntax Getter(this PropertyDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            if (node.ExpressionBody != null) return null;

            return node.AccessorList.Accessors
                .SingleOrDefault(accessor => accessor.Kind().ToString().StartsWith("Get"));
        }

        public static AccessorDeclarationSyntax Setter(this PropertyDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            if (node.ExpressionBody != null) return null;

            return node.AccessorList.Accessors
                .SingleOrDefault(accessor => accessor.Kind().ToString().StartsWith("Set"));
        }

        public static bool IsAutoImplentedProperty(this PropertyDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            if (node.IsAbstract()) return false;
            var getter = node.Getter();
            return getter != null  //Auto-properties must have getter
                && getter.Body == null;
        }
        
        public static TypeInfo TypeInfo(this PropertyDeclarationSyntax node, SemanticModel model) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return model.GetTypeInfo(node.Type);
        }

        public static bool IsInterfaceImplementation(this PropertyDeclarationSyntax node, SemanticModel model) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var propertySymbol = model.GetDeclaredSymbol(node) as IPropertySymbol;

            return propertySymbol.ContainingType
                .AllInterfaces
                .SelectMany(i => i.GetMembers().OfType<IPropertySymbol>())
                .Any(property => propertySymbol.Equals(
                                propertySymbol
                                    .ContainingType
                                    .FindImplementationForInterfaceMember(property)));
        }

        public static bool IsOverrideOrInterface(this PropertyDeclarationSyntax node, SemanticModel model) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return node.IsOverride() || node.IsInterfaceImplementation(model);
        }
    }
}
