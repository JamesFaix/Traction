using System;
using Microsoft.CodeAnalysis;
using Traction.RoslynExtensions;

namespace Traction.Contracts {

    static class IParameterSymbolExtensions {

        public static bool HasAnyPrecondition(this IParameterSymbol @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return @this
               .DeclaredAndInheritedAttributes()
               .HasAnyContractImpl(model);
        }

        public static bool HasPrecondition(this IParameterSymbol @this, SemanticModel model, Contract contract) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contract == null) throw new ArgumentNullException(nameof(contract));

            return @this
                .DeclaredAndInheritedAttributes()
                .HasContractImpl(contract, model);
        }

        public static bool HasPreconditionDeclaration(this IParameterSymbol @this, SemanticModel model, Contract contract) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contract == null) throw new ArgumentNullException(nameof(contract));

            return @this
                .GetAttributes()
                .HasContractImpl(contract, model);
        }
    }
}
