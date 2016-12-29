using Microsoft.CodeAnalysis;
using Traction.Roslyn.Semantics;

namespace Traction.Contracts.Semantics {

    internal static class AttributeDataExtensions {
        
        public static bool IsContractAttribute(this AttributeData @this, SemanticModel model) =>
            @this.IsSubclassOf(typeof(ContractAttribute), model);        
    }
}
