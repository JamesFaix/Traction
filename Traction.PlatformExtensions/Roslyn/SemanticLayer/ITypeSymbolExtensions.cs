using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Traction.Roslyn {

    public static class ITypeSymbolExtensions {

        public static IEnumerable<INamedTypeSymbol> BaseClasses(this ITypeSymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            var ancestor = @this as INamedTypeSymbol;

            while (ancestor != null) {
                ancestor = ancestor.BaseType;
                yield return ancestor;
            }
        }

        public static bool InheritsFrom(this ITypeSymbol @this, ITypeSymbol other) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (other == null) throw new ArgumentNullException(nameof(other));

            return @this
                .BaseClasses()
                .Contains(other);
        }

        public static string FullName(this ITypeSymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            return @this.ToDisplayString(
                new SymbolDisplayFormat(
                    SymbolDisplayGlobalNamespaceStyle.Included,
                    SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                    SymbolDisplayGenericsOptions.IncludeTypeParameters));

            //            return result;
        }

        private static string FullNamespace(this ISymbol @this) {
            string result = "";

            var namespaceSymbol = @this.ContainingNamespace;
            while (namespaceSymbol != null) {
                if (namespaceSymbol.Name != "") {
                    result = $"{namespaceSymbol.Name}.{result}";
                }
                namespaceSymbol = namespaceSymbol.ContainingNamespace;
            }

            return "global::" + result;
        }
    }
}
