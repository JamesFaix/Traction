using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

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

            //Only check declarations for diagnostics, not inheritance.
            //Assume base class declarations were checked.
            var hasPrecondition = contract.IsDeclaredOnParameterOf(node, model);
            var hasPostcondition = contract.IsDeclaredOnReturnValueOf(node, model);

            var result = new List<Diagnostic>();

            if (hasPrecondition) {
                //No preconditions on inherited methods
                if (node.IsOverrideOrInterface(model)) { 
                    result.Add(DiagnosticFactory.PreconditionsCannotBeAppliedToInheritedMembers(node.GetLocation()));
                }

                //No preconditions on invalid parameter types
                foreach (var p in node.ParameterList.Parameters) { 
                    if (contract.IsDeclaredOn(p, model)
                    && !contract.IsValidType(p.GetTypeInfo(model))) {
                        result.Add(contract.InvalidTypeDiagnostic(p.GetLocation()));
                    }
                }
            }

            if (hasPostcondition) {
                //No postconditions on void methods
                if (node.ReturnType().GetText().ToString().Trim() == "void") {
                    result.Add(DiagnosticFactory.PostconditionsCannotBeAppliedToMethodReturningVoid(node.GetLocation()));
                }

                //No postconditions on invalid return types
                if (!contract.IsValidType(node.ReturnTypeInfo(model))) {
                    result.Add(contract.InvalidTypeDiagnostic(node.GetLocation()));
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

            //Only check declarations for diagnostics, not inheritance.
            //Assume base class declarations were checked.
            var hasPrecondition = node.Getter() != null
                && contract.IsDeclaredOn(node, model);

            var hasPostcondition = node.Setter() != null
                && contract.IsDeclaredOn(node, model);

            var result = new List<Diagnostic>();

            if (hasPrecondition) {
                //No preconditions on inherited members
                if (node.IsOverrideOrInterface(model)) {
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
                    result.Add(contract.InvalidTypeDiagnostic(node.GetLocation()));
                }
            }

            return result;
        }
    }
}
