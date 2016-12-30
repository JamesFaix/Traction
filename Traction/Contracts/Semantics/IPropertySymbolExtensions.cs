using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Traction.Roslyn.Reflection;
using Traction.Roslyn.Semantics;

namespace Traction.Contracts.Semantics {

    static class IPropertySymbolExtensions {

        public static bool HasAnyContract(this IPropertySymbol @this, SemanticModel model) =>
            @this.HasAnyPrecondition(model) ||
            @this.HasAnyPostcondition(model);

        public static bool HasAnyPostcondition(this IPropertySymbol @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return !@this.IsWriteOnly
                && @this.DeclaredAndInheritedAttributes()
                        .Any(a => a.IsContractAttribute(model));
        }

        public static bool HasAnyPrecondition(this IPropertySymbol @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return !@this.IsReadOnly
                 && @this.DeclaredAndInheritedAttributes()
                         .Any(a => a.IsContractAttribute(model));

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
                        .Any(a => a.IsExactType(contract.AttributeType, model));

        }

        public static bool HasPrecondition(this IPropertySymbol @this, SemanticModel model, Contract contract) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contract == null) throw new ArgumentNullException(nameof(contract));

            return !@this.IsReadOnly
                && @this.DeclaredAndInheritedAttributes()
                        .Any(a => a.IsExactType(contract.AttributeType, model));
        }


        public static IEnumerable<Contract> GetContracts(this IPropertySymbol @this, SemanticModel model, IContractProvider contractProvider) =>
            @this.GetPreconditions(model, contractProvider).Concat(
            @this.GetPostconditions(model, contractProvider));

        public static IEnumerable<Contract> GetPostconditions(this IPropertySymbol @this, SemanticModel model, IContractProvider contractProvider) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            return @this.IsWriteOnly
                ? Enumerable.Empty<Contract>()
                : @this
                    .DeclaredAndInheritedAttributes()
                    .Where(a => a.IsContractAttribute(model))
                    .Select(a => contractProvider[a.AttributeClass.Name.ToString().Trim()]);
        }

        public static IEnumerable<Contract> GetPreconditions(this IPropertySymbol @this, SemanticModel model, IContractProvider contractProvider) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            return @this.IsReadOnly
                ? Enumerable.Empty<Contract>()
                : @this
                   .Parameters
                   .SelectMany(p => p.DeclaredAndInheritedAttributes())
                   .Where(a => a.IsContractAttribute(model))
                   .Select(a => contractProvider[a.AttributeClass.Name.ToString().Trim()]);
        }

        public static IEnumerable<Contract> GetDeclaredContracts(this IPropertySymbol @this, SemanticModel model, IContractProvider contractProvider) =>
            @this.GetDeclaredPreconditions(model, contractProvider).Concat(
            @this.GetDeclaredPostconditions(model, contractProvider));

        public static IEnumerable<Contract> GetDeclaredPostconditions(this IPropertySymbol @this, SemanticModel model, IContractProvider contractProvider) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            return @this.IsWriteOnly
                ? Enumerable.Empty<Contract>()
                : @this
                    .GetAttributes()
                    .Where(a => a.IsContractAttribute(model))
                    .Select(a => contractProvider[a.AttributeClass.Name.ToString().Trim()]);
        }

        public static IEnumerable<Contract> GetDeclaredPreconditions(this IPropertySymbol @this, SemanticModel model, IContractProvider contractProvider) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            return @this.IsReadOnly
                ? Enumerable.Empty<Contract>()
                : @this
                   .Parameters
                   .SelectMany(p => p.GetAttributes())
                   .Where(a => a.IsContractAttribute(model))
                   .Select(a => contractProvider[a.AttributeClass.Name.ToString().Trim()]);
        }
    }
}
