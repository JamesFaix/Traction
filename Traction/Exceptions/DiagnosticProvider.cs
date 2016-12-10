﻿using Microsoft.CodeAnalysis;
using System;

namespace Traction {

    /// <summary>
    /// Factory for contract rewriting diagnostics.
    /// </summary>
    static class DiagnosticProvider {

        public static Diagnostic SyntaxRewriteFailed(Location location, Exception exception) => Create(
            title: "Syntax rewrite failed",
            message: $"Syntax rewrite failed. Exception: {exception.GetType()}; {exception.Message}; {exception.StackTrace}",
            location: location);

        public static Diagnostic ContractAttributeCannotBeAppliedToMethodReturningVoid(Location location) => Create(
            title: $"Incorrect attribute usage",
            message: $"Attributes inheriting {nameof(ContractAttribute)} cannot be applied to methods returning void.",
            location: location);

        public static Diagnostic NonNullAttributeCanOnlyBeAppliedToReferenceTypes(Location location) => Create(
            title: $"Incorrect attribute usage",
            message: $"{nameof(NonNullAttribute)} can only be applied to reference-typed members.",
            location: location);

        private static Diagnostic Create(string title, string message, Location location) => Diagnostic.Create(
            descriptor: new DiagnosticDescriptor(
                id: "",
                title: "Traction: " + title,
                messageFormat: message,
                category: "Traction",
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true),
            location: location);
    }
}
