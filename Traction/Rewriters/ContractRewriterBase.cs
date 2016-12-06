using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Traction {

    abstract class ContractRewriterBase<TAttribute> : CSharpSyntaxRewriter
        where TAttribute : Attribute {

        protected ContractRewriterBase(SemanticModel model, ICompileContext context) {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (context == null) throw new ArgumentNullException(nameof(context));

            this.model = model;
            this.context = context;
        }

        protected readonly ICompileContext context;
        protected readonly SemanticModel model;

        public sealed override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) {
            //Debugger.Launch();
            try {
                var preconditionStatements = GetMethodPreconditions(node);

                var result = InsertMethodPostconditions(node);

                result = result
                    .WithBody(result.Body
                        .WithStatements(new SyntaxList<StatementSyntax>()
                            .AddRange(preconditionStatements)
                            .AddRange(result.Body.Statements)));

                return base.VisitMethodDeclaration(result);
            }
            catch (Exception e) {
                context.Diagnostics.Add(DiagnosticProvider.ContractInjectionFailed(node.GetLocation(), e));
                return node;
            }
        }

        private IEnumerable<StatementSyntax> GetMethodPreconditions(MethodDeclarationSyntax node) {
            var preconditionParameters = node.ParameterList.Parameters
               .Where(p => p.HasAttribute<TAttribute>(model))
               .ToArray();

            if (preconditionParameters.Any()) {
                var location = node.GetLocation();

                return preconditionParameters
                    .SelectMany(p => CreatePrecondition(model.GetTypeInfo(p.Type), p.Identifier.ValueText, location));
            }
            else {
                return new StatementSyntax[0];
            }
        }

        private MethodDeclarationSyntax InsertMethodPostconditions(MethodDeclarationSyntax node) {

            //  Debugger.Launch();

            if (node.HasAttribute<TAttribute>(model)) {
                var returnStatements = node.Body.Statements.OfType<ReturnStatementSyntax>();

                var result = node;
                var returnType = model.GetTypeInfo(node.ReturnType);
                var location = node.GetLocation();

                foreach (var ret in returnStatements) {
                    var postcondition = CreatePostcondition(returnType, ret, location);
                    result = result.ReplaceNode(ret, postcondition);
                }

                return result;
            }
            else {
                return node;
            }
        }

        public sealed override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) {
            //Debugger.Launch();
            try {
                var result = node;

                if (node.HasAttribute<TAttribute>(model)) {
                    var propertyType = model.GetTypeInfo(node.Type);

                    var location = node.GetLocation();
                    var setter = InsertPropertyPrecondition(propertyType, node.Setter(), location);
                    var getter = InsertPropertyPostcondition(propertyType, node.Getter(), location);

                    var accessors = SyntaxFactory.AccessorList(
                        SyntaxFactory.List(
                            new[] { getter, setter }
                            .Where(a => a != null)));

                    result = node.WithAccessorList(accessors);
                }

                return base.VisitPropertyDeclaration(result);
            }
            catch (Exception e) {
                context.Diagnostics.Add(DiagnosticProvider.ContractInjectionFailed(node.GetLocation(), e));
                return node;
            }
        }

        private AccessorDeclarationSyntax InsertPropertyPrecondition(TypeInfo type, AccessorDeclarationSyntax node, Location location) {
            if (node == null) return null;

            var statements = new SyntaxList<StatementSyntax>()
                .AddRange(CreatePrecondition(type, "value", location))
                .AddRange(node.Body.Statements);

            return node.WithBody(node.Body
                .WithStatements(statements));
        }

        private AccessorDeclarationSyntax InsertPropertyPostcondition(TypeInfo type, AccessorDeclarationSyntax node, Location location) {
            if (node == null) return null;

            var returnStatements = node.DescendantNodes()
                .OfType<ReturnStatementSyntax>();

            BlockSyntax body = node.Body;

            foreach (var ret in returnStatements) {
                body = body.ReplaceNode(ret, CreatePostcondition(type, ret, location));
            }

            return node.WithBody(body);
        }

        protected abstract SyntaxList<StatementSyntax> CreatePrecondition(TypeInfo parameterType, string identifier, Location location);

        protected abstract SyntaxList<StatementSyntax> CreatePostcondition(TypeInfo returnType, ReturnStatementSyntax node, Location location);

        protected string GenerateValidLocalVariableName(ReturnStatementSyntax node) {
            Debugger.Launch();
            //Find all symbol names accessible from the defining type (excessive but thorough)

            var illegalNames = node
                .FirstAncestorOrSelf<MethodDeclarationSyntax>()
                .IllegalVariableNames(model)
                .ToArray();

            //Call the temporary var "result", but prepend underscores until there is no name conflict
            var tempVariableName = "result";
            while (illegalNames.Contains(tempVariableName)) {
                tempVariableName = "_" + tempVariableName;
            }
            return tempVariableName;
        }
    }
}
