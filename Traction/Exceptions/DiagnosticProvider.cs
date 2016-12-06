using Microsoft.CodeAnalysis;
using System;

namespace Traction {

    static class DiagnosticProvider {

        public static Diagnostic NonNullAttributeCanOnlyBeAppliedToReferenceTypes(Location location) {
            return Diagnostic.Create(
                descriptor: new DiagnosticDescriptor(
                    id: "TC0001",
                    title: nameof(NonNullAttribute) + " can only be applied to reference-typed members.",
                    messageFormat: nameof(NonNullAttribute) + " can only be applied to reference-typed members.",
                    category: "Traction.CorrectUsage",
                    defaultSeverity: DiagnosticSeverity.Error,
                    isEnabledByDefault: true),
                location: location);
        }

        public static Diagnostic NonNullAttributeCannotBeAppliedToMethodWithNoReturnType(Location location) {
            return Diagnostic.Create(
                descriptor: new DiagnosticDescriptor(
                    id: "TC0002",
                    title: nameof(NonNullAttribute) + " cannot be applied to methods with no return type.",
                    messageFormat: nameof(NonNullAttribute) + " cannot be applied to methods with no return type.",
                    category: "Traction.CorrectUsage",
                    defaultSeverity: DiagnosticSeverity.Error,
                    isEnabledByDefault: true),
                location: location);
        }

        public static Diagnostic ContractInjectionFailed(Location location, Exception e) {
            return Diagnostic.Create(
                descriptor: new DiagnosticDescriptor(
                    id: "TC0003",
                    title: "Contract injection failed",
                    messageFormat: $"Contract injection failed. {e.GetType()}: {e.Message}",
                    category: "Traction",
                    defaultSeverity: DiagnosticSeverity.Error,
                    isEnabledByDefault: true),
                location: location);
        }        
    }
}
