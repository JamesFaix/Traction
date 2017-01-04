using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Traction.Contracts.Semantics;
using Traction.Contracts.Syntax;
using Traction.Roslyn.Semantics;
using Traction.Roslyn.Syntax;
using Traction.SEPrecompilation;

namespace Traction.Contracts.Analysis {

    internal sealed class Analyzer : SyntaxVisitorBase {

        public Analyzer(SemanticModel model, ICompileContext context, IContractProvider contractProvider)
            : base(model, context, contractProvider) { }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) => VisitBaseMethodDeclaration(node);
        public override SyntaxNode VisitOperatorDeclaration(OperatorDeclarationSyntax node) => VisitBaseMethodDeclaration(node);
        public override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node) => VisitBaseMethodDeclaration(node);
        public override SyntaxNode VisitIndexerDeclaration(IndexerDeclarationSyntax node) => VisitBasePropertyDeclaration(node);
        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) => VisitBasePropertyDeclaration(node);

        private SyntaxNode VisitBaseMethodDeclaration(BaseMethodDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            var symbol = model.GetDeclaredSymbol(node) as IMethodSymbol;

            var hasPrecondDeclaration = node.HasAnyPreconditionAttribute(model);
            var hasPostcondDeclaration = node.HasAnyPostconditionAttribute(model);
            var hasContractDeclaration = hasPrecondDeclaration || hasPostcondDeclaration;

            var hasPrecond = symbol.HasAnyPrecondition(model);
            var hasPostcond = symbol.HasAnyPostcondition(model);
            var hasContract = hasPrecond || hasPostcond;

            var loc = node.GetLocation();

            if (hasContractDeclaration) {
                //No contracts on partial members
                if (node.IsPartial()) {
                    context.Diagnostics.Add(DiagnosticFactory.NoContractsOnPartialMembers(loc));
                }

                //No contracts on extern members
                if (node.IsExtern()) {
                    context.Diagnostics.Add(DiagnosticFactory.NoContractsOnExternMembers(loc));
                }
            }

            if (hasPrecondDeclaration) {
                //No preconditions on inherited members
                if (symbol.IsOverrideOrInterfaceImplementation()) {
                    context.Diagnostics.Add(DiagnosticFactory.NoPreconditiosnOnInheritedMembers(loc));
                }

                //No preconditions on invalid parameter types
                foreach (var p in node.ParameterList.Parameters) {
                    var paramType = model.GetTypeInfo(p.Type).Type;

                    foreach (var c in p.GetPreconditions(model, contractProvider)) {
                        if (!c.IsValidType(paramType)) {
                            context.Diagnostics.Add(DiagnosticFactory.InvalidTypeForContract(c, loc));
                        }
                    }
                }
            }

            if (hasPostcondDeclaration) {
                //No postconditions on void methods
                if (symbol.ReturnType.Name == "Void") {
                    context.Diagnostics.Add(DiagnosticFactory.NoPostconditionsOnVoid(loc));
                }

                //No postconditions on invalid return types
                foreach (var c in symbol.GetPostconditions(model, contractProvider)) {
                    if (!c.IsValidType(symbol.ReturnType)) {
                        context.Diagnostics.Add(DiagnosticFactory.InvalidTypeForContract(c, loc));
                    }
                }
            }

            if (hasPostcond) {
                //No postconditions on iterator blocks (must be re-checks on inherited members)
                if (node.IsIteratorBlock()) {
                    context.Diagnostics.Add(DiagnosticFactory.NoPostconditionsOnIteratorBlocks(loc));
                }
            }

            if (hasPrecond) {
                //Members cannot have preconditions from multiple sources
                var precondSourceCount = symbol
                    .OverriddenAndImplementedInterfaceMembers()
                    .Count(m => m.GetDeclaredPreconditions(model, contractProvider)
                                 .Any());

                if (precondSourceCount > 1) {
                    context.Diagnostics.Add(DiagnosticFactory.MembersCannotInheritPreconditionsFromMultipleSources(loc));
                }
            }

            return node;
        }
        
        private SyntaxNode VisitBasePropertyDeclaration(BasePropertyDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            var symbol = model.GetDeclaredSymbol(node) as IPropertySymbol;

            var hasPrecondDeclaration = node.HasAnyPreconditionAttribute(model);
            var hasPostcondDeclaration = node.HasAnyPostconditionAttribute(model);
            var hasContractDeclaration = hasPrecondDeclaration || hasPostcondDeclaration;

            var hasPrecond = symbol.HasAnyPrecondition(model);
            var hasPostcond = symbol.HasAnyPostcondition(model);
            var hasContract = hasPrecond || hasPostcond;

            var loc = node.GetLocation();

            if (hasContractDeclaration) {
                //No contracts on partial members
                if (node.IsPartial()) {
                    context.Diagnostics.Add(DiagnosticFactory.NoContractsOnPartialMembers(loc));
                }

                //No contracts on extern members
                if (node.IsExtern()) {
                    context.Diagnostics.Add(DiagnosticFactory.NoContractsOnExternMembers(loc));
                }
            }

            if (hasPrecondDeclaration) {
                //No preconditions on inherited members
                if (symbol.IsOverrideOrInterfaceImplementation()) {
                    context.Diagnostics.Add(DiagnosticFactory.NoPreconditiosnOnInheritedMembers(loc));
                }

                //No preconditions on invalid types
                foreach (var c in symbol.GetPreconditions(model, contractProvider)) {
                    if (!c.IsValidType(symbol.Type)) {
                        context.Diagnostics.Add(DiagnosticFactory.InvalidTypeForContract(c, loc));
                    }
                }
            }

            if (hasPostcondDeclaration) {
                //No postconditions on invalid return types
                foreach (var c in symbol.GetPostconditions(model, contractProvider)) {
                    if (!c.IsValidType(symbol.Type)) {
                        context.Diagnostics.Add(DiagnosticFactory.InvalidTypeForContract(c, loc));
                    }
                }
            }

            if (hasPostcond) {
                //No postconditions on iterator blocks (must be re-checks on inherited members)
                if (node.IsIteratorBlock()) {
                    context.Diagnostics.Add(DiagnosticFactory.NoPostconditionsOnIteratorBlocks(loc));
                }
            }

            if (hasPrecond) {
                //Members cannot have preconditions from multiple sources
                var precondSourceCount = symbol
                    .OverriddenAndImplementedInterfaceMembers()
                    .Count(m => m.GetDeclaredPreconditions(model, contractProvider)
                                 .Any());

                if (precondSourceCount > 1) {
                    context.Diagnostics.Add(DiagnosticFactory.MembersCannotInheritPreconditionsFromMultipleSources(loc));
                }
            }

            return node;
        }
    }
}
