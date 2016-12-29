﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Traction.RoslynExtensions;

namespace Traction.Contracts {

    internal static class SymbolExtensionMethodsImpl {

        internal static bool HasAnyContractImpl(this IEnumerable<AttributeData> @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return @this.Any(a => a.IsContractAttribute(model));
        }

        internal static bool HasContractImpl(this IEnumerable<AttributeData> @this, Contract contract, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return @this.Any(a => a.IsType(contract.AttributeType, model));
        }
    }
}
