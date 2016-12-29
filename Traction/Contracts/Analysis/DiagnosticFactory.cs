using System;
using Microsoft.CodeAnalysis;
using Traction.Contracts.Semantics;

namespace Traction.Contracts.Analysis {

    /// <summary>
    /// Factory for contract rewriting diagnostics.
    /// </summary>
    static class DiagnosticFactory {

        public static Diagnostic Create(int code, string title, string message, Location location) =>
            Diagnostic.Create(
                descriptor: new DiagnosticDescriptor(
                    id: $"TR{code:D4}",
                    title: "Traction: " + title,
                    messageFormat: message,
                    category: "Traction",
                    defaultSeverity: DiagnosticSeverity.Error,
                    isEnabledByDefault: true),
                location: location);

        public static Diagnostic NoPostconditionsOnVoid(Location location) => Create(
            code: DiagnosticCodes.NoPostconditionsOnVoid,
            title: $"Invalid contract attribute usage",
            message: $"Postcondition contracts cannot be applied to methods returning void.",
            location: location);

        public static Diagnostic NoContractsOnPartialMembers(Location location) => Create(
            code: DiagnosticCodes.NoContractsOnPartialMembers,
            title: $"Invalid contract attribute usage",
            message: $"Contracts cannot be applied to partial members.",
            location: location);

        public static Diagnostic NoPreconditiosnOnInheritedMembers(Location location) => Create(
            code: DiagnosticCodes.NoPreconditionsOnInheritedMembers,
            title: $"Invalid contract attribute usage",
            message: $"Precondition contracts cannot be applied to inherited members (overridden virtual members or interface implementations).",
            location: location);

        public static Diagnostic NoPostconditionsOnIteratorBlocks(Location location) => Create(
            code: DiagnosticCodes.NoPostconditionsOnIteratorBlocks,
            title: $"Invalid contract attribute usage",
            message: $"Postcondition contracts cannot be applied to iterator blocks (members with 'yield' statements).",
            location: location);

        public static Diagnostic MembersCannotInheritPreconditionsFromMultipleSources(Location location) => Create(
           code: DiagnosticCodes.MembersCannotInheritPreconditionsFromMultipleSources,
           title: $"Invalid contract attribute usage",
           message: $"Members cannot inherit preconditions from multiple sources.",
           location: location);

        public static Diagnostic InvalidTypeForContract(Contract contract, Location location) => Create(
            code: DiagnosticCodes.InvalidTypeForContract,
            title: $"Invalid contract attribute usage",
            message: contract.InvalidTypeDiagnosticMessage,
            location: location);
    }
}
