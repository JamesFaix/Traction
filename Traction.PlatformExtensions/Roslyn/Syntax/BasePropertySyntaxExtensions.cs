using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction.Roslyn.Syntax {

    /// <summary>
    /// Extension methods for <see cref="BasePropertyDeclarationSyntax"/> 
    /// </summary>
    public static class BasePropertySyntaxExtensions {

        public static AccessorDeclarationSyntax Getter(this BasePropertyDeclarationSyntax @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            //HACK: Must cast to dynamic because ExpressionBody is only defined on subclasses
            if (((dynamic)@this).ExpressionBody != null) return null;

            return @this.AccessorList.Accessors
                .SingleOrDefault(accessor => accessor.Kind().ToString().StartsWith("Get"));
        }

        public static AccessorDeclarationSyntax Setter(this BasePropertyDeclarationSyntax @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            //HACK: Must cast to dynamic because ExpressionBody is only defined on subclasses
            if (((dynamic)@this).ExpressionBody != null) return null;

            return @this.AccessorList.Accessors
                .SingleOrDefault(accessor => accessor.Kind().ToString().StartsWith("Set"));
        }

        public static bool IsReadonly(this BasePropertyDeclarationSyntax @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.Setter() == null;
        }

        public static bool IsWriteonly(this BasePropertyDeclarationSyntax @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.Getter() == null;
        }

        public static bool IsReadWrite(this BasePropertyDeclarationSyntax @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.Getter() != null && @this.Setter() != null;
        }

        public static bool IsAutoImplentedProperty(this BasePropertyDeclarationSyntax @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            if (@this.IsAbstract()) return false;
            var getter = @this.Getter();
            return getter != null  //Auto-properties must have getter
                && getter.Body == null;
        }
    }
}
