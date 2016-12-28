using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    /// <summary>
    /// Extension methods for <see cref="BaseMethodDeclarationSyntax"/> 
    /// </summary>
    static class BaseMethodDeclarationSyntaxExtensions {

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

        public static TypeInfo ReturnTypeInfo(this BaseMethodDeclarationSyntax @this, SemanticModel model) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return model.GetTypeInfo(@this.ReturnType());
        }

        public static TNode WithBody<TNode>(this TNode @this, BlockSyntax body)
            where TNode : BaseMethodDeclarationSyntax {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (body == null) throw new ArgumentNullException(nameof(body));

            return (@this as dynamic)
                .WithBody(body);
        }

        //public static bool IsInterfaceImplementation(this BaseMethodDeclarationSyntax @this, SemanticModel model) {
        //    if (@this == null) throw new ArgumentNullException(nameof(@this));
        //    if (model == null) throw new ArgumentNullException(nameof(model));

        //    return @this.InterfaceImplementations(model).Any();
        //}

        //public static IEnumerable<IMethodSymbol> InterfaceImplementations(this BaseMethodDeclarationSyntax @this, SemanticModel model) {
        //    if (@this == null) throw new ArgumentNullException(nameof(@this));
        //    if (model == null) throw new ArgumentNullException(nameof(model));

        //    var methodSymbol = model.GetDeclaredSymbol(@this) as IMethodSymbol;

        //    var interfaceMethods = methodSymbol.ContainingType
        //        .AllInterfaces
        //        .SelectMany(i => i.GetMembers().OfType<IMethodSymbol>());

        //    return interfaceMethods
        //        .Where(method => methodSymbol.Equals(
        //                        methodSymbol
        //                            .ContainingType
        //                            .FindImplementationForInterfaceMember(method)));
        //}

        //public static bool IsOverrideOrInterface(this BaseMethodDeclarationSyntax @this, SemanticModel model) {
        //    if (@this == null) throw new ArgumentNullException(nameof(@this));
        //    if (model == null) throw new ArgumentNullException(nameof(model));

        //    return @this.IsOverride() || @this.IsInterfaceImplementation(model);
        //}
    }
}
