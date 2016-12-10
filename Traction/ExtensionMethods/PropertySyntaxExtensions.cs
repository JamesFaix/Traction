using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

            var getter = node.Getter();
            return getter != null  //Auto-properties must have getter
                && getter.Body == null;
        }
        
    }
}
