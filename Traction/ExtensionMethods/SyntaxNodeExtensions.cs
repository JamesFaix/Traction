using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    /// <summary>
    /// Extension methods for <see cref="SyntaxNode"/> 
    /// </summary>
    static class SyntaxNodeExtensions {

        public static IEnumerable<ReturnStatementSyntax> GetAllReturnStatements(this SyntaxNode node) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            foreach (var child in node.ChildNodes()) {
                if (child is ReturnStatementSyntax) {
                    yield return (ReturnStatementSyntax)child;
                }
                else if (child is AnonymousFunctionExpressionSyntax) { //Don't drill into anonymous functions
                    continue;
                }
                else {
                    //Drill down into other statements
                    foreach (var ret in child.GetAllReturnStatements()) {
                        yield return ret;
                    }
                }
            }
        }

        public static bool IsIteratorBlock(this SyntaxNode node) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            return node
                .GetYields()
                .Any();
        }

        private static IEnumerable<YieldStatementSyntax> GetYields(this SyntaxNode node) {
            var children = node.ChildNodes();
            var yields = children.OfType<YieldStatementSyntax>();
            var nonYields = children.Where(c =>
                !(c is YieldStatementSyntax) && 
                !(c is AnonymousFunctionExpressionSyntax)); //Don't drill into anonymous functions

            return yields.Concat(nonYields.SelectMany(c => GetYields(c)));
        }
    }
}
