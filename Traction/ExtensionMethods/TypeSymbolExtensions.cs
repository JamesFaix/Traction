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

        public static string FullName(this INamedTypeSymbol symbol) {
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));

            var result = symbol.FullNamespace() + symbol.Name;
            
            if (symbol.TypeArguments.Any()) {
                var args = symbol.TypeArguments
                    .Select(a => a.FullNamespace() + a.Name)
                    .ToDelimitedString(", ");

                result += $"<{args}>";
            }

            return result;
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
