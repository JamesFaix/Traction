﻿using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Traction.Contracts.Semantics;
using Traction.Contracts.Syntax;
using Traction.Linq;
using Traction.Roslyn.Semantics;
using Traction.Roslyn.Syntax;
using Traction.SEPrecompilation;

namespace Traction.Contracts.Expansion {

    /// <summary>
    /// Rewrites iterator blocks with preconditions so that preconditions are executed immediately.
    /// </summary>
    internal sealed class IteratorBlockExpander : ConcreteTypeMemberExpander<BaseMethodDeclarationSyntax> {

        public IteratorBlockExpander(SemanticModel model, ICompileContext context, IContractProvider contractProvider)
            : base(model, context, contractProvider) { }
        
        protected override bool MemberFilter(BaseMethodDeclarationSyntax member) =>
            member.IsIteratorBlock() &&
            member.ParameterList.Parameters
                .Select(p => model.GetParameterSymbol(p))
                .Any(p => p.HasAnyPrecondition(model));

        protected override SyntaxList<SyntaxNode> ExpandMember(BaseMethodDeclarationSyntax node, ISymbol symbol) {

            var innerMethodName = GetMethodName(node);
          
            var innerMethod = SyntaxFactory.MethodDeclaration(node.ReturnType(), innerMethodName)
                .WithModifiers(GetModifiers(node))
                .WithParameterList(GetParameters(node))
                .WithBody(node.Body);

            var outerMethod = node.WithBody(SyntaxFactory.Block(
                GetReturnStatement(innerMethodName, node.ParameterList)
            ));

            return new SyntaxList<SyntaxNode>()
                .Add(outerMethod)
                .Add(innerMethod);
        }

        private string GetMethodName(BaseMethodDeclarationSyntax node) {

            string result;

            var type = node.GetType();
            if (type == typeof(MethodDeclarationSyntax)) {
                var method = node as MethodDeclarationSyntax;
                result = method.Identifier.ToString();
            }
            else if (type == typeof(OperatorDeclarationSyntax)) {
                var op = node as OperatorDeclarationSyntax;
                result = op.UnderlyingMethodName();
            }
            else if (type == typeof(ConversionOperatorDeclarationSyntax)) {
                var conv = node as ConversionOperatorDeclarationSyntax;
                result = conv.UnderlyingMethodName();
            }
            else {
                throw new NotSupportedException($"Unsupported node type. ({node.GetType()})");
            }

            result = result + "_InnerIterator";
            result = IdentifierFactory.CreateUnique(UsedIdentifiers, result);
            UsedIdentifiers.Add(result);
            return result;
        }

        private SyntaxTokenList GetModifiers(BaseMethodDeclarationSyntax node) {
            var result = SyntaxFactory.TokenList(
                SyntaxFactory.ParseToken("private "));

            result = result.AddRange(node.Modifiers
                .Where(m => !m.IsAccessModifier()
                         && !m.IsInheritanceModifier()));

            return result;
        }

        private ParameterListSyntax GetParameters(BaseMethodDeclarationSyntax node) {
            return SyntaxFactory.ParameterList()
                .AddParameters(node.ParameterList.Parameters
                                    .Select(p => GetParameter(p))
                                    .ToArray());
        }

        private ParameterSyntax GetParameter(ParameterSyntax node) {
            //Remove contract attributes
            var attributes = node.AllAttributes()
                .Where(a => !a.IsContractAttribute(model))
                .ToArray();

            return node.WithAttributeLists(
                    SyntaxFactory.List<AttributeListSyntax>()
                    .Add(SyntaxFactory.AttributeList().AddAttributes(attributes)));
        }

        private StatementSyntax GetReturnStatement(string innerMethodName, ParameterListSyntax parameters) {
            var paramList = parameters.Parameters
                .Select(p => p.Identifier.ToString())
                .ToDelimitedString(", ");

            return SyntaxFactory.ParseStatement(
                $"return {innerMethodName}({paramList});");
        }
    }
}
