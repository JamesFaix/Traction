using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Traction.Roslyn;

namespace Traction.Roslyn.Semantics {

    internal static class SymbolExtensionMethodsImpl {

        internal static bool HasAnyContractImpl(this IEnumerable<AttributeData> @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return @this.Any(a => a.IsContractAttribute(model));
        }

        internal static bool HasContractImpl(this IEnumerable<AttributeData> @this, Contract contract, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return @this.Any(a => a.IsExactType(contract.AttributeType, model));
        }
    }
}
