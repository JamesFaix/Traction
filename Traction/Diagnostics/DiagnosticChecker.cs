using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Traction.Contracts;
using Traction.RoslynExtensions;

namespace Traction.Diagnostics {

    internal class DiagnosticChecker {

        public DiagnosticChecker(SemanticModel model, Contract contract) {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contract == null) throw new ArgumentNullException(nameof(contract));

            this.contract = contract;
            this.model = model;
        }

        private readonly Contract contract;
        private readonly SemanticModel model;

        public List<Diagnostic> GetDiagnostics(BaseMethodDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            var symbol = model.GetDeclaredSymbol(node) as IMethodSymbol;

            //Only check declarations for diagnostics, not inheritance.
            //Assume base class declarations were checked.
            var hasPrecondition = symbol.HasPreconditionDeclaration(model, contract);
            var hasPostcondition = symbol.HasPostconditionDeclaration(model, contract);

            var result = new List<Diagnostic>();

            if (hasPrecondition) {
                //No preconditions on inherited methods
                if (symbol.IsOverrideOrInterfaceImplementation()) {
                    result.Add(DiagnosticFactory.PreconditionsCannotBeAppliedToInheritedMembers(node.GetLocation()));
                }

                //No preconditions on invalid parameter types
                foreach (var p in node.ParameterList.Parameters) {
                    var paramSymbol = model.GetDeclaredSymbol(p) as IParameterSymbol;

                    if (paramSymbol.HasPreconditionDeclaration(model, contract)
                    && !contract.IsValidType(p.GetTypeInfo(model))) {
                        result.Add(contract.GetInvalidTypeDiagnostic(p.GetLocation()));
                    }
                }

                //var implementations = symbol.ImplementedInterfaceMembers();
                //var withPres = implementations.Where(m => m.HasAnyPrecondition(model));

                //Methods cannot implement multiple interfaces with contracts
                if (symbol
                    .ImplementedInterfaceMembers()
                    .Where(m => m.HasAnyPrecondition(model))
                    .Count() > 1) {

                    result.Add(DiagnosticFactory.MembersCannotInheritPreconditionsFromMultipleSources(node.GetLocation()));
                }
            }

            if (hasPostcondition) {
                //No postconditions on void methods
                if (node.ReturnType().GetText().ToString().Trim() == "void") {
                    result.Add(DiagnosticFactory.PostconditionsCannotBeAppliedToMethodReturningVoid(node.GetLocation()));
                }

                //No postconditions on invalid return types
                if (!contract.IsValidType(node.ReturnTypeInfo(model))) {
                    result.Add(contract.GetInvalidTypeDiagnostic(node.GetLocation()));
                }

                //No postconditions on iterator blocks
                if (node.IsIteratorBlock()) {
                    result.Add(DiagnosticFactory.PostconditionsCannotBeAppliedToIteratorBlocks(node.GetLocation()));
                }
            }

            if (hasPrecondition || hasPostcondition) {
                //No contracts on partial members
                if (node.IsPartial()) {
                    result.Add(DiagnosticFactory.ContractAttributeCannotBeAppliedToPartialMembers(node.GetLocation()));
                }
            }

            return result;
        }

        public List<Diagnostic> GetDiagnostics(PropertyDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            var symbol = model.GetDeclaredSymbol(node) as IPropertySymbol;

            //Only check declarations for diagnostics, not inheritance.
            //Assume base class declarations were checked.
            var hasPrecondition = node.Getter() != null
                && symbol.HasPreconditionDeclaration(model, contract);

            var hasPostcondition = node.Setter() != null
                && symbol.HasPostconditionDeclaration(model, contract);

            var result = new List<Diagnostic>();

            if (hasPrecondition) {
                //No preconditions on inherited members
                if (symbol.IsOverrideOrInterfaceImplementation()) {
                    result.Add(DiagnosticFactory.PreconditionsCannotBeAppliedToInheritedMembers(node.GetLocation()));
                }
            }

            if (hasPostcondition) {
                //No postconditions on iterator blocks
                if (node.IsIteratorBlock()) {
                    result.Add(DiagnosticFactory.PostconditionsCannotBeAppliedToIteratorBlocks(node.GetLocation()));
                }
            }

            if (hasPostcondition || hasPrecondition) {
                //No postconditions on invalid return types
                if (!contract.IsValidType(node.TypeInfo(model))) {
                    result.Add(contract.GetInvalidTypeDiagnostic(node.GetLocation()));
                }
            }

            return result;
        }
    }
}
