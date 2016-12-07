using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.RegularExpressions;

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

        //public static string GenerateUniqueMemberName(this CSharpSyntaxNode node, SemanticModel model) {
        //    if (node == null) throw new ArgumentNullException(nameof(node));
        //    if (model == null) throw new ArgumentNullException(nameof(model));

        //    var illegalNames = node
        //        .IllegalMemberNames(model)
        //        .ToArray();

        //    //Call the temporary var "x", but prepend underscores until there is no name conflict
        //    var tempVariableName = "x";
        //    while (illegalNames.Contains(tempVariableName)) {
        //        tempVariableName = "_" + tempVariableName;
        //    }
        //    return tempVariableName;
        //}

        public static string GenerateUniqueMemberName(this CSharpSyntaxNode node, SemanticModel model, string identifierBase = null) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (identifierBase != null && !validIdentifierRegex.IsMatch(identifierBase))
                throw new ArgumentException("Identifier base must be a valid identifier.", nameof(identifierBase));
            identifierBase = identifierBase ?? "identifier";

            var numberedIdentifierRegex = new Regex($"{identifierBase}(?<number>[0-9]*)");

            var existingNumbers = node
                .IllegalMemberNames(model)
                .Select(name => numberedIdentifierRegex.Match(name))
                .Where(match => match.Success)
                .Select(match => {
                    var text = match.Groups["number"].Value;
                    return (text == "") ? 0 : int.Parse(text);
                })
                .ToArray();

            var nextNumber = existingNumbers.Any()
                ? (existingNumbers.Max() + 1).ToString()
                : ""; //equivalent to 0

            return identifierBase + nextNumber;
        }

        private static Regex validIdentifierRegex = new Regex(@"[A-Za-z_][A-Za-z0-9_]*", RegexOptions.Compiled);
    }
}
