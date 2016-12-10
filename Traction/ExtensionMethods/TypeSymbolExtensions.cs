using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

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
    }
}
