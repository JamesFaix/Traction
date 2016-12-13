using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    static class ParameterSyntaxExtensions {

        public static TypeInfo GetTypeInfo(this ParameterSyntax node, SemanticModel model) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return model.GetTypeInfo(node.Type);
        }
    }
}
