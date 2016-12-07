using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    static class SyntaxNodeExtensions {

        //public static IEnumerable<ReturnStatementSyntax> GetAllReturnStatements(this SyntaxNode node) {
        //    if (node == null) throw new ArgumentNullException(nameof(node));

        //    foreach (var child in node.ChildNodes()) {
        //        if (child is ReturnStatementSyntax) {
        //            yield return (ReturnStatementSyntax)child;
        //        }
        //        else if (child is LocalDeclarationStatementSyntax) {
        //            //Skip nested or anonymous function bodies
        //            continue;
        //        }
        //        else {
        //            //Drill down into other statements
        //            foreach (var ret in child.GetAllReturnStatements()) {
        //                yield return ret;
        //            }
        //        }
        //    }
        //}
        
        public static TNode ReplaceReturnStatements<TNode>(this TNode node, Func<ReturnStatementSyntax, StatementSyntax> replace) 
            where TNode: SyntaxNode {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (replace == null) throw new ArgumentNullException(nameof(replace));

            var result = node;

            foreach (var child in node.ChildNodes()) {
                if (child is LocalDeclarationStatementSyntax) {
                    //Skip nested or anonymous functions
                    continue;
                }                
                else if (child is ReturnStatementSyntax) {
                    //Replace return statements
                    var ret = child as ReturnStatementSyntax;
                    result = result.ReplaceNode(ret, replace(ret));
                }
                else {
                    //Recurse through other children
                    var newChild = child.ReplaceReturnStatements(replace);
                    result = result.ReplaceNode(child, newChild);
                }
            }

            return result;
        }
    }
}
