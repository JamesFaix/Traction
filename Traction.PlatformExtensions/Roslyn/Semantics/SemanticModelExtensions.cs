using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction.Roslyn.Semantics {

    public static class SemanticModelExtensions {

        public static IMethodSymbol GetMethodSymbol(this SemanticModel @this, BaseMethodDeclarationSyntax node) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (node == null) throw new ArgumentNullException(nameof(node));

            return @this.GetDeclaredSymbol(node) as IMethodSymbol;
        }

        public static IPropertySymbol GetPropertySymbol(this SemanticModel @this, BasePropertyDeclarationSyntax node) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (node == null) throw new ArgumentNullException(nameof(node));

            return @this.GetDeclaredSymbol(node) as IPropertySymbol;
        }

        public static IParameterSymbol GetParameterSymbol(this SemanticModel @this, ParameterSyntax node) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (node == null) throw new ArgumentNullException(nameof(node));

            return @this.GetDeclaredSymbol(node) as IParameterSymbol;
        }
    }
}
