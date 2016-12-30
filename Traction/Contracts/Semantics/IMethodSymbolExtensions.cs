using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Traction.Roslyn.Semantics;

namespace Traction.Contracts.Semantics {

    internal static class IMethodSymbolExtensions {

        public static bool HasContract(this IMethodSymbol @this, SemanticModel model, Contract contract) =>
            @this.HasPrecondition(model, contract) ||
            @this.HasPostcondition(model, contract);

        public static bool HasPostcondition(this IMethodSymbol @this, SemanticModel model, Contract contract) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contract == null) throw new ArgumentNullException(nameof(contract));

            return @this
                .DeclaredAndInheritedAttributes()
                .Any(a => a.IsExactType(contract.AttributeType, model));
        }

        public static bool HasPrecondition(this IMethodSymbol @this, SemanticModel model, Contract contract) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contract == null) throw new ArgumentNullException(nameof(contract));

            return @this
                .Parameters
                .SelectMany(p => p.DeclaredAndInheritedAttributes())
                .Any(a => a.IsExactType(contract.AttributeType, model));
        }

        public static bool HasAnyContract(this IMethodSymbol @this, SemanticModel model) =>
            @this.HasAnyPrecondition(model) ||
            @this.HasAnyPostcondition(model);

        public static bool HasAnyPostcondition(this IMethodSymbol @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return @this
                .DeclaredAndInheritedAttributes()
                .Any(a => a.IsContractAttribute(model));
        }

        public static bool HasAnyPrecondition(this IMethodSymbol @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return @this
               .Parameters
               .SelectMany(p => p.DeclaredAndInheritedAttributes())
               .Any(a => a.IsContractAttribute(model));
        }

        public static IEnumerable<Contract> GetContracts(this IMethodSymbol @this, SemanticModel model, IContractProvider contractProvider) =>
            @this.GetPreconditions(model, contractProvider).Concat(
            @this.GetPostconditions(model, contractProvider));

        public static IEnumerable<Contract> GetPostconditions(this IMethodSymbol @this, SemanticModel model, IContractProvider contractProvider) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            return @this
                .DeclaredAndInheritedAttributes()
                .Where(a => a.IsContractAttribute(model))
                .Select(a => contractProvider[a.AttributeClass.Name.ToString().Trim()]);
        }

        public static IEnumerable<Contract> GetPreconditions(this IMethodSymbol @this, SemanticModel model, IContractProvider contractProvider) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            return @this
               .Parameters
               .SelectMany(p => p.DeclaredAndInheritedAttributes())
               .Where(a => a.IsContractAttribute(model))
               .Select(a => contractProvider[a.AttributeClass.Name.ToString().Trim()]);
        }

        public static IEnumerable<Contract> GetDeclaredContracts(this IMethodSymbol @this, SemanticModel model, IContractProvider contractProvider) =>
            @this.GetDeclaredPreconditions(model, contractProvider).Concat(
            @this.GetDeclaredPostconditions(model, contractProvider));

        public static IEnumerable<Contract> GetDeclaredPostconditions(this IMethodSymbol @this, SemanticModel model, IContractProvider contractProvider) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            return @this
                .GetAttributes()
                .Where(a => a.IsContractAttribute(model))
                .Select(a => contractProvider[a.AttributeClass.Name.ToString().Trim()]);
        }

        public static IEnumerable<Contract> GetDeclaredPreconditions(this IMethodSymbol @this, SemanticModel model, IContractProvider contractProvider) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            return @this
               .Parameters
               .SelectMany(p => p.GetAttributes())
               .Where(a => a.IsContractAttribute(model))
               .Select(a => contractProvider[a.AttributeClass.Name.ToString().Trim()]);
        }
    }
}
