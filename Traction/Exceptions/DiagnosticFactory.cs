using System;
using Microsoft.CodeAnalysis;

namespace Traction {

    /// <summary>
    /// Factory for contract rewriting diagnostics.
    /// </summary>
    static class DiagnosticFactory {

        public static Diagnostic RewriteConfirmation(Location location, string message) => Diagnostic.Create(
            descriptor: new DiagnosticDescriptor(
                id: $"TR{((int)DiagnosticCode.RewriteConfirmed):D4}",
                title: "Traction: Rewrite confirmation",
                messageFormat: message,
                category: "Traction",
                defaultSeverity: DiagnosticSeverity.Info,
                isEnabledByDefault: true),
            location: location);

        public static Diagnostic RewriteFailed(Location location, Exception exception) => Create(
            code: DiagnosticCode.RewriteFailed,
            title: "Syntax rewrite failed",
            message: $"Syntax rewrite failed. Exception: {exception.GetType()}; {exception.Message}; {exception.StackTrace}",
            location: location);

        public static Diagnostic PostconditionsCannotBeAppliedToMethodReturningVoid(Location location) => Create(
            code: DiagnosticCode.NoPostconditionsOnVoid,
            title: $"Invalid contract attribute usage",
            message: $"Postcondition contracts cannot be applied to methods returning void.",
            location: location);

        public static Diagnostic ContractAttributeCannotBeAppliedToPartialMembers(Location location) => Create(
            code: DiagnosticCode.NoContractsOnPartialMembers,
            title: $"Invalid contract attribute usage",
            message: $"Contracts cannot be applied to partial members.",
            location: location);

        public static Diagnostic PreconditionsCannotBeAppliedToInheritedMembers(Location location) => Create(
            code: DiagnosticCode.NoPreconditionsOnInheritedMembers,
            title: $"Invalid contract attribute usage",
            message: $"Precondition contracts cannot be applied to inherited members (overridden virtual members or interface implementations).",
            location: location);

        public static Diagnostic PostconditionsCannotBeAppliedToIteratorBlocks(Location location) => Create(
            code: DiagnosticCode.NoPostconditionsOnIteratorBlocks,
            title: $"Invalid contract attribute usage",
            message: $"Postcondition contracts cannot be applied to iterator blocks (members with 'yield' statements).",
            location: location);

        public static Diagnostic MembersCannotInheritPreconditionsFromMultipleSources(Location location) => Create(
           code: DiagnosticCode.NoPostconditionsOnIteratorBlocks,
           title: $"Invalid contract attribute usage",
           message: $"Members cannot inherit preconditions from multiple sources.",
           location: location);

        public static Diagnostic Create(DiagnosticCode code, string title, string message, Location location) =>
            Diagnostic.Create(
                descriptor: new DiagnosticDescriptor(
                    id: $"TR{((int)code):D4}",
                    title: "Traction: " + title,
                    messageFormat: message,
                    category: "Traction",
                    defaultSeverity: DiagnosticSeverity.Error,
                    isEnabledByDefault: true),
                location: location);
    }
}
