using System;
using Microsoft.CodeAnalysis;
using Traction.Roslyn;

namespace Traction.Roslyn.Semantics {

    static class IPropertySymbolExtensions {

        public static bool HasAnyContract(this IPropertySymbol @this, SemanticModel model) =>
            @this.HasAnyPrecondition(model) ||
            @this.HasAnyPostcondition(model);

        public static bool HasAnyPostcondition(this IPropertySymbol @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return !@this.IsWriteOnly
                && @this.DeclaredAndInheritedAttributes()
                        .HasAnyContractImpl(model);
        }

        public static bool HasAnyPrecondition(this IPropertySymbol @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return !@this.IsReadOnly
                 && @this.DeclaredAndInheritedAttributes()
                         .HasAnyContractImpl(model);

        }

        public static bool HasContract(this IPropertySymbol @this, SemanticModel model, Contract contract) =>
            @this.HasPrecondition(model, contract) ||
            @this.HasPostcondition(model, contract);

        public static bool HasPostcondition(this IPropertySymbol @this, SemanticModel model, Contract contract) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contract == null) throw new ArgumentNullException(nameof(contract));

            var attributeSymbol = contract.AttributeType.GetTypeSymbol(model);

            return !@this.IsWriteOnly
                && @this.DeclaredAndInheritedAttributes()
                        .HasContractImpl(contract, model);

        }

        public static bool HasPostconditionDeclaration(this IPropertySymbol @this, SemanticModel model, Contract contract) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contract == null) throw new ArgumentNullException(nameof(contract));

            var attributeSymbol = contract.AttributeType.GetTypeSymbol(model);

            return !@this.IsWriteOnly
                && @this.GetAttributes()
                        .HasContractImpl(contract, model);

        }

        public static bool HasPrecondition(this IPropertySymbol @this, SemanticModel model, Contract contract) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contract == null) throw new ArgumentNullException(nameof(contract));

            return !@this.IsReadOnly
                && @this.DeclaredAndInheritedAttributes()
                        .HasContractImpl(contract, model);
        }

        public static bool HasPreconditionDeclaration(this IPropertySymbol @this, SemanticModel model, Contract contract) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contract == null) throw new ArgumentNullException(nameof(contract));

            return !@this.IsReadOnly
                && @this.GetAttributes()
                        .HasContractImpl(contract, model);
        }
    }
}
