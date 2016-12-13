using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    /// <summary>
    /// Extension methods for <see cref="BaseMethodDeclarationSyntax"/> 
    /// </summary>
    static class BaseMethodDeclarationSyntaxExtensions {

        public static TypeSyntax ReturnType(this BaseMethodDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            if (node is MethodDeclarationSyntax)
                return ((MethodDeclarationSyntax)node).ReturnType;

            if (node is OperatorDeclarationSyntax)
                return ((OperatorDeclarationSyntax)node).ReturnType;

            if (node is ConversionOperatorDeclarationSyntax)
                return ((ConversionOperatorDeclarationSyntax)node).Type;

            if (node is ConstructorDeclarationSyntax 
             || node is DestructorDeclarationSyntax)
                return null;

            throw new InvalidOperationException("Invalid method declaration type.");
        }

        public static TNode WithBody<TNode>(this TNode node, BlockSyntax body)
            where TNode : BaseMethodDeclarationSyntax {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (body == null) throw new ArgumentNullException(nameof(body));

            return (node as dynamic)
                .WithBody(body);
        }       
    }
}
