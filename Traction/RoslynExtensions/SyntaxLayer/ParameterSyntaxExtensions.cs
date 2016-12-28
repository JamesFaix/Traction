using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction.RoslynExtensions {

    static class ParameterSyntaxExtensions {

        public static TypeInfo GetTypeInfo(this ParameterSyntax @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return model.GetTypeInfo(@this.Type);
        }
    }
}
