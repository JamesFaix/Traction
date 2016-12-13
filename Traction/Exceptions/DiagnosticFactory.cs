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
            title: $"Invalid contract attribute usage",
            message: $"Attributes inheriting {nameof(ContractAttribute)} cannot be applied to methods returning void.",
            location: location);

        public static Diagnostic ContractAttributeCannotBeAppliedToPartialMembers(Location location) => Create(
            title: $"Invalid contract attribute usage",
            message: $"Attributes inheriting {nameof(ContractAttribute)} cannot be applied to partial members.",
            location: location);

        public static Diagnostic PreconditionsCannotBeAddedToInheritedMembers(Location location) => Create(
          title: $"Invalid contract attribute usage",
          message: $"Precondition contracts cannot be added to inherited members (overridden virtual members or interface implementations).",
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
