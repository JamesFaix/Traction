using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Precompiler {

    static class RoslynExtensions {

        #region All Attributes

        public static IEnumerable<AttributeSyntax> AllAttributes(this BaseMethodDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            return node.AttributeLists.SelectMany(list => list.Attributes);
        }

        public static IEnumerable<AttributeSyntax> AllAttributes(this ParameterSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            return node.AttributeLists.SelectMany(list => list.Attributes);
        }

        public static IEnumerable<AttributeSyntax> AllAttributes(this TypeDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            return node.AttributeLists.SelectMany(list => list.Attributes);
        }

        public static IEnumerable<AttributeSyntax> AllAttributes(this PropertyDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            return node.AttributeLists.SelectMany(list => list.Attributes);
        }

        #endregion

        public static IEnumerable<AttributeSyntax> ReturnValueAttributes(this MethodDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            return node.AttributeLists
                .Where(IsAttributeListTargetingReturnValue)
                .SelectMany(list => list.Attributes);
        }

        private static bool IsAttributeListTargetingReturnValue(AttributeListSyntax attributes) {
            var specifier = attributes.ChildNodes()
                .OfType<AttributeTargetSpecifierSyntax>()
                .FirstOrDefault();

            if (specifier == null) return false;

            var target = specifier.ChildTokens().First();
            return target.Text == "return";
        }

        #region Has Attribute

        public static bool HasAttribute<TAttribute>(this BaseMethodDeclarationSyntax node, SemanticModel model) where TAttribute : Attribute {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));
            return HasAttributeImpl<TAttribute>(node.AllAttributes(), model);
        }

        public static bool HasAttribute<TAttribute>(this ParameterSyntax node, SemanticModel model) where TAttribute : Attribute {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));
            return HasAttributeImpl<TAttribute>(node.AllAttributes(), model);
        }

        public static bool HasAttribute<TAttribute>(this TypeDeclarationSyntax node, SemanticModel model) where TAttribute : Attribute {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));
            return HasAttributeImpl<TAttribute>(node.AllAttributes(), model);
        }

        public static bool HasAttribute<TAttribute>(this PropertyDeclarationSyntax node, SemanticModel model) where TAttribute : Attribute {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));
            return HasAttributeImpl<TAttribute>(node.AllAttributes(), model);
        }

        #endregion

        public static bool ContainsAttribute<TAttribute>(this IEnumerable<AttributeSyntax> attributes, SemanticModel model) where TAttribute : Attribute {
            if (attributes == null) throw new ArgumentNullException(nameof(attributes));
            if (model == null) throw new ArgumentNullException(nameof(model));
            return HasAttributeImpl<TAttribute>(attributes, model);
        }

        private static bool HasAttributeImpl<TAttribute>(IEnumerable<AttributeSyntax> attributes, SemanticModel model) where TAttribute : Attribute {

            var symbol = typeof(TAttribute).GetTypeSymbol(model);
            return attributes.Any(a => symbol.Equals(model.GetTypeInfo(a).Type));
        }
        
        public static INamedTypeSymbol GetTypeSymbol(this Type type, SemanticModel model) {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (model == null) throw new ArgumentNullException(nameof(model));

            if (!type.IsConstructedGenericType) {
                return model.Compilation.GetTypeByMetadataName(type.FullName);
            }
            else  {
                var typeParams = type.GenericTypeArguments
                    .Select(t => GetTypeSymbol(t, model))
                    .ToArray();

                var openType = type.GetGenericTypeDefinition();
                var symbol = model.Compilation.GetTypeByMetadataName(openType.FullName);
                return symbol.Construct(typeParams);
            }
        }

        public static bool MatchesTypeSymbol(this Type type, ITypeSymbol symbol, SemanticModel model) {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));
            if (model == null) throw new ArgumentNullException(nameof(model));

            return type.GetTypeSymbol(model).Equals(symbol);
        }

        #region Property Accessors

        public static AccessorDeclarationSyntax Getter(this PropertyDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            
            return node.AccessorList.Accessors
                .SingleOrDefault(accessor => accessor.Kind().ToString().StartsWith("Get"));
        }

        public static AccessorDeclarationSyntax Setter(this PropertyDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
          //  System.Diagnostics.Debugger.Launch();
            return node.AccessorList.Accessors
                .SingleOrDefault(accessor => accessor.Kind().ToString().StartsWith("Set"));
        }

        #endregion
    }
}
