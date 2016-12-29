using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Traction.RoslynExtensions;

namespace Traction.Contracts {

    internal static class IMethodSymbolExtensions {

        public static bool HasAnyContract(this IMethodSymbol @this, SemanticModel model) =>
            @this.HasAnyPrecondition(model) ||
            @this.HasAnyPostcondition(model);

        public static bool HasAnyPostcondition(this IMethodSymbol @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return @this.DeclaredAndInheritedAttributes()
                        .HasAnyContractImpl(model);
        }
        
        public static bool HasAnyPrecondition(this IMethodSymbol @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return @this
               .Parameters
               .SelectMany(p => p.DeclaredAndInheritedAttributes())
               .HasAnyContractImpl(model);
        }
        
        public static bool HasContract(this IMethodSymbol @this, SemanticModel model, Contract contract) =>
            @this.HasPrecondition(model, contract) ||
            @this.HasPostcondition(model, contract);
        
        public static bool HasPostcondition(this IMethodSymbol @this, SemanticModel model, Contract contract) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contract == null) throw new ArgumentNullException(nameof(contract));

            return @this.DeclaredAndInheritedAttributes()
                        .HasContractImpl(contract, model);
        }

        public static bool HasPostconditionDeclaration(this IMethodSymbol @this, SemanticModel model, Contract contract) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contract == null) throw new ArgumentNullException(nameof(contract));

            return @this.DeclaredAndInheritedAttributes()
                        .HasContractImpl(contract, model);
        }

        public static bool HasPrecondition(this IMethodSymbol @this, SemanticModel model, Contract contract) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contract == null) throw new ArgumentNullException(nameof(contract));

            return @this
                .Parameters
                .SelectMany(p => p.DeclaredAndInheritedAttributes())
                .HasContractImpl(contract, model);
        }

        public static bool HasPreconditionDeclaration(this IMethodSymbol @this, SemanticModel model, Contract contract) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contract == null) throw new ArgumentNullException(nameof(contract));

            return @this
                .Parameters
                .SelectMany(p => p.GetAttributes())
                .HasContractImpl(contract, model);
        }
    }
}
