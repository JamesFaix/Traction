using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    static class MethodSyntaxExtensionMethods {

        public static IEnumerable<string> IllegalVariableNames(this MethodDeclarationSyntax node, SemanticModel model) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var typeDeclaration = node.FirstAncestorOrSelf<TypeDeclarationSyntax>();
            var typeSymbol = model.GetDeclaredSymbol(typeDeclaration);

            var memberAndTypeNames = model
                .LookupSymbols(node.GetLocation().SourceSpan.Start, typeSymbol)
                .Select(symbol => symbol.Name);

            var locals = node.DescendantNodes()
                .OfType<VariableDeclarationSyntax>()
                .SelectMany(declaration => declaration.Variables)
                .Select(v => model.GetDeclaredSymbol(v).Name);

            return memberAndTypeNames.Concat(locals).ToArray();
        }
    }
}
