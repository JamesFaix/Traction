using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Traction.Roslyn.Reflection;
using Traction.Roslyn.Semantics;
using Traction.Roslyn.Syntax;

namespace Traction.Contracts.Syntax {

    static class ParameterDeclarationExtensions {

        public static bool HasPreconditionAttribute(this ParameterSyntax @this, Contract contract, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (contract == null) throw new ArgumentNullException(nameof(contract));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var attributeTypeSymbol = contract.AttributeType.GetTypeSymbol(model);

            return @this.AllAttributes()
                .Any(a => model.GetTypeInfo(a).Type.Equals(attributeTypeSymbol));
        }

        public static bool HasAnyPreconditionAttribute(this ParameterSyntax @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var attributeTypeSymbol = typeof(ContractAttribute).GetTypeSymbol(model);

            return @this.AllAttributes()
                .Any(a => model.GetTypeInfo(a).Type.InheritsFrom(attributeTypeSymbol));
        }

        public static IEnumerable<Contract> GetPreconditions(this ParameterSyntax @this, SemanticModel model, IContractProvider contractProvider) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            var attributeTypeSymbol = typeof(ContractAttribute).GetTypeSymbol(model);

            return @this.AllAttributes()
                .Where(a => model.GetTypeInfo(a).Type.InheritsFrom(attributeTypeSymbol))
                .Select(a => contractProvider[a.GetText().ToString().Trim()]);
        }
    }
}
