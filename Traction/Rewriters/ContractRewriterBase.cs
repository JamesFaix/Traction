using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace Traction {

    /// <summary>
    /// Base class for contract attribute syntax rewriters.
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    abstract class ContractRewriterBase<TAttribute> : RewriterBase
        where TAttribute : Attribute {

        protected ContractRewriterBase(SemanticModel model, ICompileContext context)
            : base(model, context) { }

        public sealed override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) => VisitPropertyImpl(node);
        public sealed override SyntaxNode VisitOperatorDeclaration(OperatorDeclarationSyntax node) => VisitMethodImpl(node);
        public sealed override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node) => VisitMethodImpl(node);

        public sealed override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) {
            if (node.ReturnType.GetText().ToString() == "void") {
                context.Diagnostics.Add(DiagnosticFactory.ContractAttributeCannotBeAppliedToMethodReturningVoid(node.GetLocation()));
                VisitMethodImpl(node); //Visit to check for other errors, but ignore returned value
                return node; //Return original node
            }
            else {
                return VisitMethodImpl(node);
            }
        }

        private TNode VisitMethodImpl<TNode>(TNode node)
            where TNode : BaseMethodDeclarationSyntax =>
            node.HasAnyAttribute<TAttribute>(model)
                ? TryRewrite(node, VisitMethodImplInner)
                : node;

        private PropertyDeclarationSyntax VisitPropertyImpl(PropertyDeclarationSyntax node) =>
            node.HasAttribute<TAttribute>(model)
                ? TryRewrite(node, VisitPropertyImplInner)
                : node;

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
               .Where(p => p.HasAttribute<TAttribute>(model))
               .ToArray();

            if (!preconditionParameters.Any()) {
                return new StatementSyntax[0];
            }

            var location = node.GetLocation();

            return preconditionParameters
                .Select(p => CreatePreconditionIfValidType(model.GetTypeInfo(p.Type), p.Identifier.ValueText, location));
        }

        private TNode InsertMethodPostconditions<TNode>(TNode node)
            where TNode : BaseMethodDeclarationSyntax {
            if (!node.HasAttribute<TAttribute>(model)) {
                return node;
            }

            var result = node;
            var returnType = model.GetTypeInfo(node.ReturnType());
            var location = node.GetLocation();

            var returnStatements = node.GetAllReturnStatements();

            return node.ReplaceNodes(returnStatements,
                (oldNode, newNode) => CreatePostconditionIfValidType(returnType, newNode, location));
        }

        private AccessorDeclarationSyntax InsertPropertyPrecondition(TypeInfo type, AccessorDeclarationSyntax node, Location location) {
            if (node == null) return null;

            return node.WithBody(
                SyntaxFactory.Block(CreatePreconditionIfValidType(type, "value", location))
                    .AddStatements(node.Body.Statements.ToArray()));
        }

        private AccessorDeclarationSyntax InsertPropertyPostcondition(TypeInfo type, AccessorDeclarationSyntax node, Location location) {
            if (node == null) return null;

            var returnStatements = node.GetAllReturnStatements();

            return node.ReplaceNodes(returnStatements,
                (oldNode, newNode) => CreatePostconditionIfValidType(type, newNode, location));
        }

        private StatementSyntax CreatePreconditionIfValidType(TypeInfo parameterType, string identifier, Location location) {
            if (IsValidType(parameterType)) {
                return CreatePrecondition(parameterType, identifier, location);
            }
            else {
                context.Diagnostics.Add(InvalidTypeDiagnostic(location));
                return SyntaxFactory.Block();
            }
        }

        private StatementSyntax CreatePostconditionIfValidType(TypeInfo returnType, ReturnStatementSyntax node, Location location) {
            if (IsValidType(returnType)) {
                return CreatePostcondition(returnType, node, location);
            }
            else {
                context.Diagnostics.Add(InvalidTypeDiagnostic(location));
                return SyntaxFactory.Block();
            }
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
            sb.AppendLine($"if (!({GetConditionExpression(parameterName, parameterType)}))");
            sb.AppendLine($"    throw new global::Traction.PreconditionException(\"{ExceptionMessage}\", nameof({parameterName}));");
            return sb.ToString();
        }

        private string GetPostconditionText(TypeInfo returnType, string returnedExpression, string tempVarName) {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"    {returnType.Type.FullName()} {tempVarName} = {returnedExpression};");
            sb.AppendLine($"    if (!({GetConditionExpression(tempVarName, returnType)}))");
            sb.AppendLine($"        throw new global::Traction.PostconditionException(\"{ExceptionMessage}\");");
            sb.AppendLine($"    return {tempVarName};");
            sb.AppendLine("}");
            return sb.ToString();
        }

        /// <summary>
        /// Gets boolean expression to evaluate.  Should return false if contract is broken.
        /// </summary>
        protected abstract ExpressionSyntax GetConditionExpression(string expression, TypeInfo expressionType);

        protected abstract string ExceptionMessage { get; }

        protected abstract bool IsValidType(TypeInfo type);

        protected abstract Diagnostic InvalidTypeDiagnostic(Location location);
    }
}
