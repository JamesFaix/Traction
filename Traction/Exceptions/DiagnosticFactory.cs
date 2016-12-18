using System;
using Microsoft.CodeAnalysis;

namespace Traction {

    /// <summary>
    /// Factory for contract rewriting diagnostics.
    /// </summary>
    static class DiagnosticFactory {

        public static Diagnostic RewriteConfirmation(Location location, string message) => Diagnostic.Create(
            descriptor: new DiagnosticDescriptor(
                id: "TR0000",
                title: "Traction: Rewrite confirmation",
                messageFormat: message,
                category: "Traction",
                defaultSeverity: DiagnosticSeverity.Info,
                isEnabledByDefault: true),
            location: location);

        public static Diagnostic SyntaxRewriteFailed(Location location, Exception exception) => Create(
            id: "TR0001",
            title: "Syntax rewrite failed",
            message: $"Syntax rewrite failed. Exception: {exception.GetType()}; {exception.Message}; {exception.StackTrace}",
            location: location);

        public static Diagnostic PostconditionsCannotBeAppliedToMethodReturningVoid(Location location) => Create(
            id: "TR0002",
            title: $"Invalid contract attribute usage",
            message: $"Postcondition contracts cannot be applied to methods returning void.",
            location: location);

        public static Diagnostic ContractAttributeCannotBeAppliedToPartialMembers(Location location) => Create(
            id: "TR0003",
            title: $"Invalid contract attribute usage",
            message: $"Contracts cannot be applied to partial members.",
            location: location);

        public static Diagnostic PreconditionsCannotBeAppliedToInheritedMembers(Location location) => Create(
            id: "TR0004",
            title: $"Invalid contract attribute usage",
            message: $"Precondition contracts cannot be applied to inherited members (overridden virtual members or interface implementations).",
            location: location);

        public static Diagnostic PostconditionsCannotBeAppliedToIteratorBlocks(Location location) => Create(
            id: "TR0005",
            title: $"Invalid contract attribute usage",
            message: $"Postcondition contracts cannot be applied to iterator blocks (members with 'yield' statements).",
            location: location);

        public static Diagnostic Create(string id, string title, string message, Location location) => Diagnostic.Create(
            descriptor: new DiagnosticDescriptor(
                id: id,
                title: "Traction: " + title,
                messageFormat: message,
                category: "Traction",
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true),
            location: location);
    }
}
