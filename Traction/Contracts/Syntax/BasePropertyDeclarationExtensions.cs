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

    internal static class BasePropertyDeclarationExtensions {

        public static bool HasContractAttribute(this BasePropertyDeclarationSyntax @this, Contract contract, SemanticModel model) =>
            @this.HasPreconditionAttribute(contract, model) ||
            @this.HasPostconditionAttribute(contract, model);

        public static bool HasPreconditionAttribute(this BasePropertyDeclarationSyntax @this, Contract contract, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (contract == null) throw new ArgumentNullException(nameof(contract));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var attributeTypeSymbol = contract.AttributeType.GetTypeSymbol(model);

            return !@this.IsReadonly() &&
                @this.AllAttributes()
                .Any(a => model.GetTypeInfo(a).Type.Equals(attributeTypeSymbol));
        }

        public static bool HasPostconditionAttribute(this BasePropertyDeclarationSyntax @this, Contract contract, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (contract == null) throw new ArgumentNullException(nameof(contract));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var attributeTypeSymbol = contract.AttributeType.GetTypeSymbol(model);

            return !@this.IsWriteonly() &&
                @this.AllAttributes()
                .Any(a => model.GetTypeInfo(a).Type.Equals(attributeTypeSymbol));
        }

        public static bool HasAnyContractAttribute(this BasePropertyDeclarationSyntax @this, SemanticModel model) =>
            @this.HasAnyPreconditionAttribute(model) ||
            @this.HasAnyPostconditionAttribute(model);

        public static bool HasAnyPreconditionAttribute(this BasePropertyDeclarationSyntax @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var attributeTypeSymbol = typeof(ContractAttribute).GetTypeSymbol(model);

            return !@this.IsWriteonly() &&
                @this.AllAttributes()
                     .Any(a => model.GetTypeInfo(a).Type.InheritsFrom(attributeTypeSymbol));
        }

        public static bool HasAnyPostconditionAttribute(this BasePropertyDeclarationSyntax @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var attributeTypeSymbol = typeof(ContractAttribute).GetTypeSymbol(model);

            return !@this.IsWriteonly() &&
                @this.AllAttributes()
                     .Any(a => model.GetTypeInfo(a).Type.InheritsFrom(attributeTypeSymbol));
        }

        public static IEnumerable<Contract> GetContracts(this BasePropertyDeclarationSyntax @this, SemanticModel model, IContractProvider contractProvider) =>
            @this.GetPreconditions(model, contractProvider).Concat(
            @this.GetPostconditions(model, contractProvider));

        public static IEnumerable<Contract> GetPreconditions(this BasePropertyDeclarationSyntax @this, SemanticModel model, IContractProvider contractProvider) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            var attributeTypeSymbol = typeof(ContractAttribute).GetTypeSymbol(model);

            return @this.IsReadonly()
                ? Enumerable.Empty<Contract>()
                : @this.AllAttributes()
                       .Where(a => model.GetTypeInfo(a).Type.InheritsFrom(attributeTypeSymbol))
                       .Select(a => contractProvider[a.GetText().ToString().Trim()]);
        }

        public static IEnumerable<Contract> GetPostconditions(this BasePropertyDeclarationSyntax @this, SemanticModel model, IContractProvider contractProvider) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            var attributeTypeSymbol = typeof(ContractAttribute).GetTypeSymbol(model);

            return @this.IsWriteonly()
                ? Enumerable.Empty<Contract>()
                : @this.AllAttributes()
                       .Where(a => model.GetTypeInfo(a).Type.InheritsFrom(attributeTypeSymbol))
                       .Select(a => contractProvider[a.GetText().ToString().Trim()]);
        }
    }
}
