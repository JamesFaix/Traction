using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Traction.Contracts.Semantics;
using Traction.Linq;
using Traction.Roslyn.Semantics;
using Traction.Roslyn.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Traction.Contracts.Injection {

    internal static class InjectionExtensionMethods {

        public static TMethodSyntax WithContracts<TMethodSyntax>(this TMethodSyntax @this, SemanticModel model, IContractProvider contractProvider)
            where TMethodSyntax : BaseMethodDeclarationSyntax {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            var symbol = model.GetMethodSymbol(@this);

            var preconditionStatements = symbol
                .Parameters
                .SelectMany(parameter => parameter
                    .GetPreconditions(model, contractProvider)
                    .Select(contract => new { P = parameter, C = contract }))
                .Select(x => PreconditionStatement(x.C, x.P.Type, x.P.Name));

            var returnType = model.GetTypeInfo(@this.ReturnType()).Type;
            var returnStatements = @this.GetReturnStatements();
            var postconditions = symbol
                .GetPostconditions(model, contractProvider);

            var result = @this.ReplaceNodes(returnStatements,
                (oldNode, newNode) => PostconditionStatement(postconditions, returnType, oldNode, model));

            return result
                .WithBody(result.Body
                    .WithStatements(new SyntaxList<StatementSyntax>()
                        .AddRange(preconditionStatements)
                        .AddRange(result.Body.Statements)));
        }

        public static PropertyDeclarationSyntax WithContracts(this PropertyDeclarationSyntax @this, SemanticModel model, IContractProvider contractProvider) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            var symbol = model.GetPropertySymbol(@this);

            var setter = @this.Setter();
            if (setter != null) {
                var preconditions = symbol
                    .GetPreconditions(model, contractProvider);

                var statements = preconditions
                    .Select(c => PreconditionStatement(c, symbol.Type, "value"))
                    .Concat(setter.Body.Statements)
                    .ToArray();

                setter = setter.WithBody(Block(statements));
            }

            var getter = @this.Getter();
            if (getter != null) {
                var returnStatements = getter.GetReturnStatements();
                var postconditions = symbol
                    .GetPostconditions(model, contractProvider);

                getter = getter.ReplaceNodes(returnStatements,
                    (oldNode, newNode) => PostconditionStatement(postconditions, symbol.Type, oldNode, model));
            }

            var accessors = AccessorList(List(
                new[] {
                    getter,
                    setter
                }
                .Where(a => a != null)));

            return @this.WithAccessorList(accessors);
        }

        private static StatementSyntax PreconditionStatement(Contract contract, ITypeSymbol type, string parameterName) {
            return Block(ParseStatement(
                GetPreconditionText(contract, type, parameterName)));
        }

        private static StatementSyntax PostconditionStatement(IEnumerable<Contract> contracts, ITypeSymbol type, ReturnStatementSyntax node, SemanticModel model) {
            var returnedExpression = node.ChildNodes().FirstOrDefault().ToString();
            var tempVariableName = IdentifierFactory.CreatePostconditionLocal(node, model);
            return Block(ParseStatement(
                GetPostconditionText(contracts, type, returnedExpression, tempVariableName)));
        }

        private static string GetPreconditionText(Contract contract, ITypeSymbol type, string parameterName) {
            var sb = new StringBuilder();
            sb.AppendLine($"if (!({contract.GetCondition(parameterName, type)}))");
            sb.AppendLine($"    throw new global::Traction.PreconditionException(\"{contract.ExceptionMessage}\", nameof({parameterName}));");
            return sb.ToString();
        }

        private static string GetPostconditionText(IEnumerable<Contract> contracts, ITypeSymbol type, string returnedExpression, string tempVarName) {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"    {type.FullName()} {tempVarName} = {returnedExpression};");
            foreach (var c in contracts) {
                sb.AppendLine($"    if (!({c.GetCondition(tempVarName, type)}))");
                sb.AppendLine($"        throw new global::Traction.PostconditionException(\"{c.ExceptionMessage}\");");
            }
            sb.AppendLine($"    return {tempVarName};");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
