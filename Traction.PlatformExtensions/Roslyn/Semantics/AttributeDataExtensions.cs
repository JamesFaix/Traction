using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Traction.Roslyn.Semantics {

    public static class AttributeDataExtensions {

        public static bool IsSubclassOf(this AttributeData @this, Type attributeType, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (!attributeType.IsSubclassOf(typeof(Attribute))) throw new ArgumentException("Attribute type must extend System.Attribute.", nameof(attributeType));

            return @this
                .AttributeClass
                .BaseClasses()
                .Contains(attributeType.GetTypeSymbol(model));
        }

        public static bool IsExactType(this AttributeData @this, Type attributeType, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (!attributeType.IsSubclassOf(typeof(Attribute))) throw new ArgumentException("Attribute type must extend System.Attribute.", nameof(attributeType));

            return @this.AttributeClass
                .Equals(attributeType.GetTypeSymbol(model));
        }
    }
}
