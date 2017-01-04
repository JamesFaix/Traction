using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction.Roslyn.Syntax {

    /// <summary>
    /// Extension methods for <see cref="BaseMethodDeclarationSyntax"/> 
    /// </summary>
    public static class BaseMethodDeclarationSyntaxExtensions {

        public static TypeSyntax ReturnType(this BaseMethodDeclarationSyntax @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            if (@this is MethodDeclarationSyntax)
                return ((MethodDeclarationSyntax)@this).ReturnType;

            if (@this is OperatorDeclarationSyntax)
                return ((OperatorDeclarationSyntax)@this).ReturnType;

            if (@this is ConversionOperatorDeclarationSyntax)
                return ((ConversionOperatorDeclarationSyntax)@this).Type;

            if (@this is ConstructorDeclarationSyntax
             || @this is DestructorDeclarationSyntax)
                return null;

            throw new InvalidOperationException("Invalid method declaration type.");
        }

        public static TNode WithBody<TNode>(this TNode @this, BlockSyntax body)
            where TNode : BaseMethodDeclarationSyntax {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (body == null) throw new ArgumentNullException(nameof(body));

            return (@this as dynamic)
                .WithBody(body);
        }       
    }
}
