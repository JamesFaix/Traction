using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Linq;

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

        public static TypeInfo ReturnTypeInfo(this BaseMethodDeclarationSyntax node, SemanticModel model) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return model.GetTypeInfo(node.ReturnType());
        }

        public static TNode WithBody<TNode>(this TNode node, BlockSyntax body)
            where TNode : BaseMethodDeclarationSyntax {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (body == null) throw new ArgumentNullException(nameof(body));

            return (node as dynamic)
                .WithBody(body);
        }       

        public static bool IsInterfaceImplementation(this BaseMethodDeclarationSyntax node, SemanticModel model) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var methodSymbol = model.GetDeclaredSymbol(node) as IMethodSymbol;

            var interfaceMethods = methodSymbol.ContainingType
                .AllInterfaces
                .SelectMany(i => i.GetMembers().OfType<IMethodSymbol>());

            return interfaceMethods
                .Any(method => methodSymbol.Equals(
                                methodSymbol
                                    .ContainingType
                                    .FindImplementationForInterfaceMember(method)));
        }

        public static bool IsOverrideOrInterface(this BaseMethodDeclarationSyntax node, SemanticModel model) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return node.IsOverride() || node.IsInterfaceImplementation(model);
        }
    }
}
