using Microsoft.CodeAnalysis;
using Traction.RoslynExtensions;

namespace Traction.Contracts {

    internal static class AttributeDataExtensions {
        
        public static bool IsContractAttribute(this AttributeData @this, SemanticModel model) =>
            @this.IsSubclassOf(typeof(ContractAttribute), model);        
    }
}
