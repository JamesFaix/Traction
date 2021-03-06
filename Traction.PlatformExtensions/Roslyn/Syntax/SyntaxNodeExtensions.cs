﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction.Roslyn.Syntax {

    /// <summary>
    /// Extension methods for <see cref="SyntaxNode"/> 
    /// </summary>
    public static class SyntaxNodeExtensions {

        public static IEnumerable<ReturnStatementSyntax> GetReturnStatements(this SyntaxNode @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            foreach (var child in @this.ChildNodes()) {
                if (child is ReturnStatementSyntax) {
                    yield return (ReturnStatementSyntax)child;
                }
                else if (child is AnonymousFunctionExpressionSyntax) { //Don't drill into anonymous functions
                    continue;
                }
                else {
                    //Drill down into other statements
                    foreach (var ret in child.GetReturnStatements()) {
                        yield return ret;
                    }
                }
            }
        }

        public static bool IsIteratorBlock(this SyntaxNode @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            return @this
                .GetYieldStatements()
                .Any();
        }

        private static IEnumerable<YieldStatementSyntax> GetYieldStatements(this SyntaxNode @this) {
            var children = @this.ChildNodes();
            var yields = children.OfType<YieldStatementSyntax>();
            var nonYields = children.Where(c =>
                !(c is YieldStatementSyntax) &&
                !(c is AnonymousFunctionExpressionSyntax)); //Don't drill into anonymous functions

            return yields.Concat(nonYields.SelectMany(c => GetYieldStatements(c)));
        }

        public static bool IsNonImplementedMember(this SyntaxNode @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            
            return (@this is BaseMethodDeclarationSyntax || @this is PropertyDeclarationSyntax)
                && (@this.IsInInterface() || @this.IsAbstract() || @this.IsPartial());
        }

        private static bool IsInInterface(this SyntaxNode @this) =>
            @this.Ancestors()
                .OfType<InterfaceDeclarationSyntax>()
                .Any();

        public static bool IsAbstract(this SyntaxNode @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.ChildTokens()
                .Any(t => t.IsKind(SyntaxKind.AbstractKeyword));
        }

        public static bool IsPartial(this SyntaxNode @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.ChildTokens()
                .Any(t => t.IsKind(SyntaxKind.PartialKeyword));
        }

        public static bool IsStatic(this SyntaxNode @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.ChildTokens()
                .Any(t => t.IsKind(SyntaxKind.StaticKeyword));
        }

        public static bool IsOverride(this SyntaxNode @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.ChildTokens()
                .Any(t => t.IsKind(SyntaxKind.OverrideKeyword));
        }

        public static bool IsExtern(this SyntaxNode @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.ChildTokens()
                .Any(t => t.IsKind(SyntaxKind.ExternKeyword));
        }
    }
}
