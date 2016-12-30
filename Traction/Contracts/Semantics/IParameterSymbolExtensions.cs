using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Traction.Roslyn.Semantics;

namespace Traction.Contracts.Semantics {

    internal static class IParameterSymbolExtensions {

        public static bool HasAnyPrecondition(this IParameterSymbol @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return @this
               .DeclaredAndInheritedAttributes()
               .Any(a => a.IsContractAttribute(model));
        }

        public static bool HasPrecondition(this IParameterSymbol @this, SemanticModel model, Contract contract) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contract == null) throw new ArgumentNullException(nameof(contract));

            return @this
                .DeclaredAndInheritedAttributes()
                .Any(a => a.IsExactType(contract.AttributeType, model));
        }

        public static IEnumerable<Contract> GetPreconditions(this IParameterSymbol @this, SemanticModel model, IContractProvider contractProvider) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            return @this
               .DeclaredAndInheritedAttributes()
               .Where(a => a.IsContractAttribute(model))
               .Select(a => contractProvider[a.AttributeClass.Name.ToString().Trim()]);
        }

        public static IEnumerable<Contract> GetDeclaredPreconditions(this IParameterSymbol @this, SemanticModel model, IContractProvider contractProvider) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            return @this
               .GetAttributes()
               .Where(a => a.IsContractAttribute(model))
               .Select(a => contractProvider[a.AttributeClass.Name.ToString().Trim()]);
        }
    }
}
