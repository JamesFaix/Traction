using Microsoft.CodeAnalysis;
using System;

namespace Traction {

    /// <summary>
    /// Factory for contract rewriting diagnostics.
    /// </summary>
    static class DiagnosticFactory {

        public static Diagnostic SyntaxRewriteFailed(Location location, Exception exception) => Create(
            title: "Syntax rewrite failed",
            message: $"Syntax rewrite failed. Exception: {exception.GetType()}; {exception.Message}; {exception.StackTrace}",
            location: location);

        public static Diagnostic ContractAttributeCannotBeAppliedToMethodReturningVoid(Location location) => Create(
            title: $"Incorrect attribute usage",
            message: $"Attributes inheriting {nameof(ContractAttribute)} cannot be applied to methods returning void.",
            location: location);
        
        public static Diagnostic Create(string title, string message, Location location) => Diagnostic.Create(
            descriptor: new DiagnosticDescriptor(
                id: "TRACTION",
                title: "Traction: " + title,
                messageFormat: message,
                category: "Traction",
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true),
            location: location);
    }
}
