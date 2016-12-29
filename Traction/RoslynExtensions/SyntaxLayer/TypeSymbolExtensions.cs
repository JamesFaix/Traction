using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Traction {

    /// <summary>
    /// Extension methods for <see cref="ITypeSymbol"/> 
    /// </summary>
    static class TypeSymbolExtensions {

        public static IEnumerable<INamedTypeSymbol> Ancestors(this ITypeSymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            var ancestor = @this as INamedTypeSymbol;

            while (ancestor != null) {
                ancestor = ancestor.BaseType;
                yield return ancestor;
            }
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
        
        private static string ToDelimitedString<T>(this IEnumerable<T> @this, string delimiter) {
            var sb = new StringBuilder();

            bool skipDelimiter = true;

            foreach (var item in @this) {
                if (skipDelimiter) {
                    skipDelimiter = false;
                }
                else {
                    sb.Append(delimiter);
                }
                sb.Append(item);
            }

            return sb.ToString();
        }
    }
}
