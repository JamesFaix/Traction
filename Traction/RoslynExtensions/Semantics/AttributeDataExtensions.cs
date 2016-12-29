using Microsoft.CodeAnalysis;

namespace Traction.Roslyn {

    internal static class AttributeDataExtensions {
        
        public static bool IsContractAttribute(this AttributeData @this, SemanticModel model) =>
            @this.IsSubclassOf(typeof(ContractAttribute), model);        
    }
}
