using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Traction.Contracts;
using Traction.Roslyn;
using Traction.SEPrecompilation;

namespace Traction {

    /// <summary>
    /// Base class for contract attribute syntax rewriters.
    /// </summary>
    internal sealed class ContractRewriter : RewriterBase {

        private ContractRewriter(SemanticModel model, ICompileContext context, Contract contract)
            : base(model, context, $"Applied {contract.GetType()}.") {

            if (contract == null) throw new ArgumentNullException(nameof(contract));
            this.contract = contract;
            this.diagnosticChecker = new DiagnosticChecker(model, contract);
        }

        public static ContractRewriter Create(SemanticModel model, ICompileContext context, Contract contract) =>
            new ContractRewriter(model, context, contract);

        //Allows partial function application on the factory method
        public static RewriterFactoryMethod Create(Contract contract) =>
            (model, context) => Create(model, context, contract);

        private readonly Contract contract;
        private readonly DiagnosticChecker diagnosticChecker;

        public sealed override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) => VisitPropertyImpl(node);
        public sealed override SyntaxNode VisitOperatorDeclaration(OperatorDeclarationSyntax node) => VisitMethodImpl(node);
        public sealed override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node) => VisitMethodImpl(node);
        public sealed override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) => VisitMethodImpl(node);

        private TNode VisitMethodImpl<TNode>(TNode node)
            where TNode : BaseMethodDeclarationSyntax {

            var symbol = model.GetMethodSymbol(node);
            if (!symbol.HasContract(model, contract)) {
                return node;
            }

            var diagnostics = this.diagnosticChecker.GetDiagnostics(node);
            foreach (var d in diagnostics) {
                this.context.Diagnostics.Add(d);
            }

            //If any diagnostics, or if member is non-implemented, do not rewrite
            if (diagnostics.Any() || node.IsNonImplementedMember()) {
                return node;
            }
            else {
                return nodeRewriter
                    .Try(node, VisitMethodImplInner)
                    .Result;
            }
        }

        private PropertyDeclarationSyntax VisitPropertyImpl(PropertyDeclarationSyntax node) {
            var symbol = model.GetPropertySymbol(node);
            if (!symbol.HasContract(model, contract)) {
                return node;
            }

            var diagnostics = this.diagnosticChecker.GetDiagnostics(node);
            foreach (var d in diagnostics) {
                this.context.Diagnostics.Add(d);
            }

            //If any diagnostics, or if member is non-implemented, do not rewrite
            if (diagnostics.Any() || node.IsNonImplementedMember()) {
                return node;
            }
            else {
                return nodeRewriter
                    .Try(node, VisitPropertyImplInner)
                    .Result;
            }
        }

        private TNode VisitMethodImplInner<TNode>(TNode node)
            where TNode : BaseMethodDeclarationSyntax {

            var preconditionStatements = GetMethodPreconditions(node);

            var result = InsertMethodPostconditions(node);

            return result
                .WithBody(result.Body
                    .WithStatements(new SyntaxList<StatementSyntax>()
                        .AddRange(preconditionStatements)
                        .AddRange(result.Body.Statements)));
        }

        private PropertyDeclarationSyntax VisitPropertyImplInner(PropertyDeclarationSyntax node) {
            var propertyType = model.GetTypeInfo(node.Type);

            var location = node.GetLocation();
            var setter = InsertPropertyPrecondition(propertyType, node.Setter(), location);
            var getter = InsertPropertyPostcondition(propertyType, node.Getter(), location);

            var accessors = SyntaxFactory.AccessorList(
                SyntaxFactory.List(
                    new[] { getter, setter }
                    .Where(a => a != null)));

            return node.WithAccessorList(accessors);
        }

        private IEnumerable<StatementSyntax> GetMethodPreconditions<TNode>(TNode node)
            where TNode : BaseMethodDeclarationSyntax {
            var preconditionParameters = node.ParameterList.Parameters
                .Where(p => model
                    .GetParameterSymbol(p)
                    .HasPrecondition(model, contract))
                .ToArray();

            if (!preconditionParameters.Any()) {
                return new StatementSyntax[0];
            }

            var location = node.GetLocation();

            return preconditionParameters
                .Select(p => CreatePrecondition(model.GetTypeInfo(p.Type), p.Identifier.ValueText, location));
        }

        private TNode InsertMethodPostconditions<TNode>(TNode node)
            where TNode : BaseMethodDeclarationSyntax {

            var symbol = model.GetMethodSymbol(node);
            if (!symbol.HasPostcondition(model, contract)) {
                return node;
            }

            var result = node;
            var returnType = model.GetTypeInfo(node.ReturnType());
            var location = node.GetLocation();

            var returnStatements = node.GetReturnStatements();

            return node.ReplaceNodes(returnStatements,
                (oldNode, newNode) => CreatePostcondition(returnType, newNode, location));
        }

        private AccessorDeclarationSyntax InsertPropertyPrecondition(TypeInfo type, AccessorDeclarationSyntax node, Location location) {
            if (node == null) return null;
            return node.WithBody(
                SyntaxFactory.Block(CreatePrecondition(type, "value", location))
                    .AddStatements(node.Body.Statements.ToArray()));
        }

        private AccessorDeclarationSyntax InsertPropertyPostcondition(TypeInfo type, AccessorDeclarationSyntax node, Location location) {
            if (node == null) return null;
            var returnStatements = node.GetReturnStatements();
            return node.ReplaceNodes(returnStatements,
                (oldNode, newNode) => CreatePostcondition(type, newNode, location));
        }

        private StatementSyntax CreatePrecondition(TypeInfo parameterType, string parameterName, Location location) {
            var text = GetPreconditionText(parameterName, parameterType);
            var statement = SyntaxFactory.ParseStatement(text);
            return SyntaxFactory.Block(statement);
        }

        private StatementSyntax CreatePostcondition(TypeInfo returnType, ReturnStatementSyntax node, Location location) {
            var returnedExpression = node.ChildNodes().FirstOrDefault();
            var tempVariableName = IdentifierFactory.CreatePostconditionLocal(node, model);
            var text = GetPostconditionText(returnType, returnedExpression.ToString(), tempVariableName);
            var statement = SyntaxFactory.ParseStatement(text);
            return statement;
        }

        private string GetPreconditionText(string parameterName, TypeInfo parameterType) {
            var sb = new StringBuilder();
            sb.AppendLine($"if (!({this.contract.GetCondition(parameterName, parameterType)}))");
            sb.AppendLine($"    throw new global::Traction.PreconditionException(\"{this.contract.ExceptionMessage}\", nameof({parameterName}));");
            return sb.ToString();
        }

        private string GetPostconditionText(TypeInfo returnType, string returnedExpression, string tempVarName) {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"    {returnType.Type.FullName()} {tempVarName} = {returnedExpression};");
            sb.AppendLine($"    if (!({this.contract.GetCondition(tempVarName, returnType)}))");
            sb.AppendLine($"        throw new global::Traction.PostconditionException(\"{this.contract.ExceptionMessage}\");");
            sb.AppendLine($"    return {tempVarName};");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
