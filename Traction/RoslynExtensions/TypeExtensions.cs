using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Traction.RoslynExtensions {

    /// <summary>
    /// Extension methods for <see cref="Type"/> for working with Roslyn API. 
    /// </summary>
    static class TypeExtensions {
        
        public static INamedTypeSymbol GetTypeSymbol(this Type @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            if (!@this.IsConstructedGenericType) {
                return model.Compilation.GetTypeByMetadataName(@this.FullName);
            }
            else {
                var typeParams = @this.GenericTypeArguments
                    .Select(t => GetTypeSymbol(t, model))
                    .ToArray();

                var openType = @this.GetGenericTypeDefinition();
                var symbol = model.Compilation.GetTypeByMetadataName(openType.FullName);
                return symbol.Construct(typeParams);
            }
        }

        //public static bool MatchesTypeSymbol(this Type @this, ITypeSymbol symbol, SemanticModel model) {
        //    if (@this == null) throw new ArgumentNullException(nameof(@this));
        //    if (symbol == null) throw new ArgumentNullException(nameof(symbol));
        //    if (model == null) throw new ArgumentNullException(nameof(model));

        //    return @this.GetTypeSymbol(model).Equals(symbol);
        //}
    }
}
