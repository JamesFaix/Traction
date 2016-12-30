using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction.Roslyn.Syntax {

    /// <summary>
    /// Extension methods for <see cref="PropertyDeclarationSyntax"/> 
    /// </summary>
    public static class PropertySyntaxExtensions {

        public static AccessorDeclarationSyntax Getter(this PropertyDeclarationSyntax @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            if (@this.ExpressionBody != null) return null;

            return @this.AccessorList.Accessors
                .SingleOrDefault(accessor => accessor.Kind().ToString().StartsWith("Get"));
        }

        public static AccessorDeclarationSyntax Setter(this PropertyDeclarationSyntax @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            if (@this.ExpressionBody != null) return null;

            return @this.AccessorList.Accessors
                .SingleOrDefault(accessor => accessor.Kind().ToString().StartsWith("Set"));
        }

        public static bool IsReadonly(this PropertyDeclarationSyntax @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.Setter() == null;
        }

        public static bool IsWriteonly(this PropertyDeclarationSyntax @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.Getter() == null;
        }

        public static bool IsReadWrite(this PropertyDeclarationSyntax @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.Getter() != null && @this.Setter() != null;
        }

        public static bool IsAutoImplentedProperty(this PropertyDeclarationSyntax @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            if (@this.IsAbstract()) return false;
            var getter = @this.Getter();
            return getter != null  //Auto-properties must have getter
                && getter.Body == null;
        }

        public static TypeInfo TypeInfo(this PropertyDeclarationSyntax @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return model.GetTypeInfo(@this.Type);
        }

    }
}
