using Microsoft.CodeAnalysis;

namespace Traction.Roslyn.Semantics {

    internal static class AttributeDataExtensions {
        
        public static bool IsContractAttribute(this AttributeData @this, SemanticModel model) =>
            @this.IsSubclassOf(typeof(ContractAttribute), model);        
    }
}
