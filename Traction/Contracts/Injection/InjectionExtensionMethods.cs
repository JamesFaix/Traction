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

        public static TNode WithContracts<TNode>(this TNode @this, SemanticModel model, IContractProvider contractProvider)
            where TNode : MemberDeclarationSyntax {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            //HACK: Must use dynamic because of limitation with generic constraints.
            return typeof(TNode).IsSubclassOf(typeof(BaseMethodDeclarationSyntax))
                ? MethodWithContracts((dynamic)@this, model, contractProvider)
                : PropertyWithContracts((dynamic)@this, model, contractProvider);
        }

        private static TMethodSyntax MethodWithContracts<TMethodSyntax>(this TMethodSyntax @this, SemanticModel model, IContractProvider contractProvider)
            where TMethodSyntax : BaseMethodDeclarationSyntax {

            var symbol = model.GetMethodSymbol(@this);

            var preconditionStatements = symbol
                .Parameters
                .SelectMany(parameter => parameter
                    .GetPreconditions(model, contractProvider)
                    .Select(contract => new _PreconditionParameters(contract, parameter)))
                .Select(x => PreconditionStatement(x.Contract, x.ParameterType, x.ParameterName));

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

        private static TPropertySyntax PropertyWithContracts<TPropertySyntax>(this TPropertySyntax @this, SemanticModel model, IContractProvider contractProvider)
            where TPropertySyntax : BasePropertyDeclarationSyntax {
            var symbol = model.GetPropertySymbol(@this);

            var indexer = @this as IndexerDeclarationSyntax;
            var paramPreconditions = (indexer == null)
                ? Enumerable.Empty<_PreconditionParameters>()
                : model.GetPropertySymbol(indexer)
                       .Parameters
                       .SelectMany(p => p.GetPreconditions(model, contractProvider)
                                         .Select(contract => new _PreconditionParameters(contract, p)));

            var setter = @this.Setter();
            if (setter != null) {
                var preconditions = symbol
                    .GetPreconditions(model, contractProvider)
                    .Select(contract => new _PreconditionParameters(contract, symbol.Type, "value"))
                    .Concat(paramPreconditions);

                var statements = preconditions
                    .Select(x => PreconditionStatement(x.Contract, x.ParameterType, x.ParameterName))
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

                var statements = paramPreconditions
                    .Select(x => PreconditionStatement(x.Contract, x.ParameterType, x.ParameterName))
                    .Concat(getter.Body.Statements)
                    .ToArray();

                getter = getter.WithBody(Block(statements));
            }

            var accessors = AccessorList(List(
                new[] {
                    getter,
                    setter
                }
                .Where(a => a != null)));

            //HACK: Must cast to dynamic because WithAccessorList is only defined on subclasses
            return ((dynamic)@this).WithAccessorList(accessors);
        }

        private class _PreconditionParameters {
            public readonly Contract Contract;
            public readonly ITypeSymbol ParameterType;
            public readonly string ParameterName;

            public _PreconditionParameters(Contract contract, IParameterSymbol parameter) {
                Contract = contract;
                ParameterType = parameter.Type;
                ParameterName = parameter.Name;
            }

            public _PreconditionParameters(Contract contract, ITypeSymbol parameterType, string parameterName) {
                Contract = contract;
                ParameterType = parameterType;
                ParameterName = parameterName;
            }
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
