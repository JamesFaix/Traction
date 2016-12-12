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

        public static IEnumerable<INamedTypeSymbol> Ancestors(this ITypeSymbol symbol) {
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));

            var ancestor = symbol as INamedTypeSymbol;

            while (ancestor != null) {
                ancestor = ancestor.BaseType;
                yield return ancestor;
            }
        }

        public static string FullName(this ITypeSymbol symbol) {
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));
            
            return symbol.ToDisplayString(
                new SymbolDisplayFormat(
                    SymbolDisplayGlobalNamespaceStyle.Included,
                    SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                    SymbolDisplayGenericsOptions.IncludeTypeParameters));

//            return result;
        }

        private static string FullNamespace(this ISymbol symbol) {
            string result = "";

            var namespaceSymbol = symbol.ContainingNamespace;
            while (namespaceSymbol != null) {
                if (namespaceSymbol.Name != "") {
                    result = $"{namespaceSymbol.Name}.{result}";
                }
                namespaceSymbol = namespaceSymbol.ContainingNamespace;
            }

            return "global::" + result;
        }

        public static bool CanBeNull(this ITypeSymbol type) =>
            !type.IsValueType || type.IsNullable();

        public static bool IsNullable(this ITypeSymbol type) =>
            type.FullName().EndsWith("?");

        public static bool IsEquatable(this ITypeSymbol type) =>
            type.AllInterfaces
                .Any(i => i.FullName().StartsWith("global::System.IEquatable<"));

        public static bool IsComparable(this ITypeSymbol type) =>
            type.AllInterfaces
                .Any(i => i.FullName().StartsWith("global::System.IComparable<"));

        public static bool IsEnumerable(this ITypeSymbol type) =>
            type.AllInterfaces
                .Any(i => i.FullName().StartsWith("global::System.Collections.Generic.IEnumerable"));

        private static string ToDelimitedString<T>(this IEnumerable<T> sequence, string delimiter) {
            var sb = new StringBuilder();

            bool skipDelimiter = true;

            foreach (var item in sequence) {
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
