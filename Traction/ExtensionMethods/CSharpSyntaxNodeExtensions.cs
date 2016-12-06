using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    static class MethodSyntaxExtensionMethods {
        
        public static IEnumerable<string> IllegalMemberNames(this CSharpSyntaxNode node, SemanticModel model) {
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

        public static string GenerateUniqueMemberName(this CSharpSyntaxNode node, SemanticModel model) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var illegalNames = node
                .IllegalMemberNames(model)
                .ToArray();

            //Call the temporary var "x", but prepend underscores until there is no name conflict
            var tempVariableName = "x";
            while (illegalNames.Contains(tempVariableName)) {
                tempVariableName = "_" + tempVariableName;
            }
            return tempVariableName;
        }
    }
}
