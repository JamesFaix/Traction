using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    static class SyntaxNodeExtensions {

        public static IEnumerable<ReturnStatementSyntax> GetAllReturnStatements(this SyntaxNode node) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            foreach (var child in node.ChildNodes()) {
                if (child is ReturnStatementSyntax) {
                    yield return (ReturnStatementSyntax)child;
                }
                else if (child is LocalDeclarationStatementSyntax) {
                    //Skip nested or anonymous function bodies
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
    }
}
