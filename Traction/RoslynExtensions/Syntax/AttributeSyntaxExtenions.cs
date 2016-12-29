using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Traction.Roslyn;

namespace Traction.Roslyn {

    internal static class AttributeSyntaxExtenions {

        public static bool IsContractAttribute(this AttributeSyntax @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var baseType = typeof(ContractAttribute).GetTypeSymbol(model);
            var attributeType = model.GetTypeInfo(@this).Type;
            return attributeType.InheritsFrom(baseType);
        }
    }
}
