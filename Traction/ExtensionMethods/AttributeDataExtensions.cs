using System;
using System.Linq;

using Microsoft.CodeAnalysis;

namespace Traction {

    static class AttributeDataExtensions {

        public static bool Extends<TAttrbute>(this AttributeData @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var symbol1 = typeof(TAttrbute).GetTypeSymbol(model);
            var symbol2 = @this.AttributeClass;
            return symbol2.Ancestors().Contains(symbol1);
        }
    }
}
