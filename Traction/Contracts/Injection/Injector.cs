using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Traction.Contracts.Semantics;
using Traction.Roslyn.Semantics;
using Traction.Roslyn.Syntax;
using Traction.SEPrecompilation;
using Traction.Roslyn.Rewriting;

namespace Traction.Contracts.Injection {

    /// <summary>
    /// Base class for contract attribute syntax rewriters.
    /// </summary>
    internal sealed class Injector : SyntaxVisitorBase {

        public Injector(SemanticModel model, ICompileContext context, IContractProvider contractProvider)
            : base(model, context, contractProvider) { }
        
        public sealed override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) => VisitPropertyImpl(node);
        public sealed override SyntaxNode VisitOperatorDeclaration(OperatorDeclarationSyntax node) => VisitMethodImpl(node);
        public sealed override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node) => VisitMethodImpl(node);
        public sealed override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) => VisitMethodImpl(node);

        private TNode VisitMethodImpl<TNode>(TNode node)
            where TNode : BaseMethodDeclarationSyntax {

            var symbol = model.GetMethodSymbol(node);

            if (!symbol.HasAnyContract(model)
            || context.Diagnostics.Any(d => d.Id != DiagnosticCodes.RewriteConfirmed)
            || node.IsNonImplementedMember()) {
                return node;
            }

            //inject each precond on each param
            //inject each postcond

            return nodeRewriter
                .Try(node, n => n.WithContracts(model, contractProvider), "Applied contract.")
                .Result;
        }

        private PropertyDeclarationSyntax VisitPropertyImpl(PropertyDeclarationSyntax node) {
            var symbol = model.GetPropertySymbol(node);

            //If any diagnostics, or if member is non-implemented, do not rewrite
            if (!symbol.HasAnyContract(model)
            || context.Diagnostics.Any(d => d.Id != DiagnosticCodes.RewriteConfirmed)
            || node.IsNonImplementedMember()) {
                return node;
            }
            
            return nodeRewriter
                .Try(node, n => n.WithContracts(model, contractProvider), "Applied contract.")
                .Result;
        }

        //private TNode VisitMethodImplInner<TNode>(TNode node)
        //    where TNode : BaseMethodDeclarationSyntax {

        //    var result = InsertMethodPostconditions(node);

        //    return result
        //        .WithBody(result.Body
        //            .WithStatements(new SyntaxList<StatementSyntax>()
        //                .AddRange(GetMethodPreconditions(node))
        //                .AddRange(result.Body.Statements)));
        //}

        //private PropertyDeclarationSyntax VisitPropertyImplInner(PropertyDeclarationSyntax node) {
        //    var propertyType = model.GetTypeInfo(node.Type).Type;

        //    var location = node.GetLocation();
        //    var setter = InsertPropertyPrecondition(propertyType, node.Setter(), location);
        //    var getter = InsertPropertyPostcondition(propertyType, node.Getter(), location);

        //    var accessors = SyntaxFactory.AccessorList(
        //        SyntaxFactory.List(
        //            new[] { getter, setter }
        //            .Where(a => a != null)));

        //    return node.WithAccessorList(accessors);
        //}

        //private IEnumerable<StatementSyntax> GetMethodPreconditions<TNode>(TNode node)
        //    where TNode : BaseMethodDeclarationSyntax {
        //    var preconditionParameters = node.ParameterList.Parameters
        //        .Where(p => model
        //            .GetParameterSymbol(p)
        //            .HasPrecondition(model, contract))
        //        .ToArray();

        //    if (!preconditionParameters.Any()) {
        //        return new StatementSyntax[0];
        //    }

        //    var location = node.GetLocation();

        //    return preconditionParameters
        //        .Select(p => CreatePrecondition(model.GetTypeInfo(p.Type).Type, p.Identifier.ValueText, location));
        //}

        //private TNode InsertMethodPostconditions<TNode>(TNode node)
        //    where TNode : BaseMethodDeclarationSyntax {

        //    var symbol = model.GetMethodSymbol(node);
        //    if (!symbol.HasPostcondition(model, contract)) {
        //        return node;
        //    }

        //    var result = node;
        //    var returnType = model.GetTypeInfo(node.ReturnType()).Type;
        //    var location = node.GetLocation();

        //    var returnStatements = node.GetReturnStatements();

        //    return node.ReplaceNodes(returnStatements,
        //        (oldNode, newNode) => CreatePostcondition(returnType, newNode, location));
        //}

        //private AccessorDeclarationSyntax InsertPropertyPrecondition(ITypeSymbol type, AccessorDeclarationSyntax node, Location location) {
        //    if (node == null) return null;
        //    return node.WithBody(
        //        SyntaxFactory.Block(CreatePrecondition(type, "value", location))
        //            .AddStatements(node.Body.Statements.ToArray()));
        //}

        //private AccessorDeclarationSyntax InsertPropertyPostcondition(ITypeSymbol type, AccessorDeclarationSyntax node, Location location) {
        //    if (node == null) return null;
        //    var returnStatements = node.GetReturnStatements();
        //    return node.ReplaceNodes(returnStatements,
        //        (oldNode, newNode) => CreatePostcondition(type, newNode, location));
        //}
    }
}
