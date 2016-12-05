using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Traction {

    static class TypeSymbolExtensions {

        public static INamedTypeSymbol GetTypeSymbol(this Type type, SemanticModel model) {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (model == null) throw new ArgumentNullException(nameof(model));

            if (!type.IsConstructedGenericType) {
                return model.Compilation.GetTypeByMetadataName(type.FullName);
            }
            else {
                var typeParams = type.GenericTypeArguments
                    .Select(t => GetTypeSymbol(t, model))
                    .ToArray();

                var openType = type.GetGenericTypeDefinition();
                var symbol = model.Compilation.GetTypeByMetadataName(openType.FullName);
                return symbol.Construct(typeParams);
            }
        }

        public static bool MatchesTypeSymbol(this Type type, ITypeSymbol symbol, SemanticModel model) {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return type.GetTypeSymbol(model).Equals(symbol);
        }
    }
}
