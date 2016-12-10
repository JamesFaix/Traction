using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    /// <summary>
    /// Produces unique identifiers for specific contexts.
    /// </summary>
    static class IdentifierFactory {

        private static Regex validIdentifierRegex = new Regex(@"[A-Za-z_][A-Za-z0-9_]*", RegexOptions.Compiled);

        public static string CreateUnique(IEnumerable<string> identifiers, string identifierBase = null) {
            if (identifiers == null) throw new ArgumentNullException(nameof(identifiers));
            if (identifierBase != null && !validIdentifierRegex.IsMatch(identifierBase))
                throw new ArgumentException("Identifier base must be a valid identifier.", nameof(identifierBase));
            identifierBase = identifierBase ?? "identifier";

            var numberedIdentifierRegex = new Regex($"{identifierBase}(?<number>[0-9]*)");

            var existingNumbers = identifiers
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

        public static string CreateUnique(CSharpSyntaxNode scope, SemanticModel model, string identifierBase = null) {
            if (scope == null) throw new ArgumentNullException(nameof(scope));
            if (model == null) throw new ArgumentNullException(nameof(model));
        
            return CreateUnique(GetUsedIdentifiers(scope, model), identifierBase);
        }

        public static IEnumerable<string> GetUsedIdentifiers(CSharpSyntaxNode scope, SemanticModel model) {
            if (scope == null) throw new ArgumentNullException(nameof(scope));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var typeDeclaration = scope.FirstAncestorOrSelf<TypeDeclarationSyntax>();
            var typeSymbol = model.GetDeclaredSymbol(typeDeclaration);

            var memberAndTypeNames = model
                .LookupSymbols(scope.GetLocation().SourceSpan.Start, typeSymbol)
                .Select(symbol => symbol.Name);

            var locals = scope.DescendantNodes()
                .OfType<VariableDeclarationSyntax>()
                .SelectMany(declaration => declaration.Variables)
                .Select(v => model.GetDeclaredSymbol(v).Name);

            return memberAndTypeNames.Concat(locals).ToArray();
        }

        public static string CreatePostconditionLocal(ReturnStatementSyntax returnStatement, SemanticModel model) {
            if (returnStatement == null) throw new ArgumentNullException(nameof(returnStatement));
            if (model == null) throw new ArgumentNullException(nameof(model));

            CSharpSyntaxNode declaration = returnStatement.FirstAncestorOrSelf<BaseMethodDeclarationSyntax>();
            declaration = declaration ?? returnStatement.FirstAncestorOrSelf<AccessorDeclarationSyntax>();

            return CreateUnique(declaration, model, "result");
        }
    }
}
