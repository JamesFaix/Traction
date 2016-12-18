using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    /// <summary>
    /// Extension methods for syntax nodes to get information about attributes decorating those nodes.
    /// </summary>
    static class AttributeSyntaxExtensions {

        #region All Attributes

        public static IEnumerable<AttributeSyntax> AllAttributes(this BaseMethodDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            return node.AttributeLists.SelectMany(list => list.Attributes);
        }

        public static IEnumerable<AttributeSyntax> AllAttributes(this ParameterSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            return node.AttributeLists.SelectMany(list => list.Attributes);
        }
        
        public static IEnumerable<AttributeSyntax> AllAttributes(this PropertyDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            return node.AttributeLists.SelectMany(list => list.Attributes);
        }

        #endregion

        #region Has Attribute

        public static bool HasAttribute<TAttribute>(this BaseMethodDeclarationSyntax node, SemanticModel model)
            where TAttribute : Attribute {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));
            return HasAttributeImpl<TAttribute>(node.AllAttributes(), model);
        }

        public static bool HasAttribute<TAttribute>(this ParameterSyntax node, SemanticModel model)
            where TAttribute : Attribute {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));
            return HasAttributeImpl<TAttribute>(node.AllAttributes(), model);
        }
        
        public static bool HasAttribute<TAttribute>(this PropertyDeclarationSyntax node, SemanticModel model)
            where TAttribute : Attribute {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));
            return HasAttributeImpl<TAttribute>(node.AllAttributes(), model);
        }

        #endregion

        #region HasOrInheritsAttribute

        public static bool HasOrInheritsAttribute<TAttribute>(this BaseMethodDeclarationSyntax node, SemanticModel model)
           where TAttribute : Attribute {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var symbol = typeof(TAttribute).GetTypeSymbol(model);

            var attributeTypeSymbols = model
                .GetDeclaredSymbol(node)
                .AncestorAttributes()
                .Select(a => a.AttributeClass);

            return attributeTypeSymbols.Any(t => t.Equals(symbol));
        }

        public static bool HasOrInheritsAttribute<TAttribute>(this ParameterSyntax node, SemanticModel model)
            where TAttribute : Attribute {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var symbol = typeof(TAttribute).GetTypeSymbol(model);

            var attributeTypeSymbols = model
                .GetDeclaredSymbol(node)
                .AncestorAttributes()
                .Select(a => a.AttributeClass);

            return attributeTypeSymbols.Any(t => t.Equals(symbol));
        }

        public static bool HasOrInheritsAttribute<TAttribute>(this PropertyDeclarationSyntax node, SemanticModel model)
            where TAttribute : Attribute {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var symbol = typeof(TAttribute).GetTypeSymbol(model);

            var attributeTypeSymbols = model
                .GetDeclaredSymbol(node)
                .AncestorAttributes()
                .Select(a => a.AttributeClass);

            return attributeTypeSymbols.Any(t => t.Equals(symbol));
        }
        #endregion

        #region HasAttributeExtenging

        public static bool HasAttributeExtending<TAttribute>(this BaseMethodDeclarationSyntax node, SemanticModel model)
            where TAttribute : Attribute {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));
            return HasAttributeExtendingImpl<TAttribute>(node.AllAttributes(), model);
        }

        public static bool HasAttributeExtending<TAttribute>(this ParameterSyntax node, SemanticModel model)
            where TAttribute : Attribute {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));
            return HasAttributeExtendingImpl<TAttribute>(node.AllAttributes(), model);
        }

        public static bool HasAttributeExtending<TAttribute>(this PropertyDeclarationSyntax node, SemanticModel model)
            where TAttribute : Attribute {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));
            return HasAttributeExtendingImpl<TAttribute>(node.AllAttributes(), model);
        }

        #endregion

        //public static bool HasAnyAttribute<TAttribute>(this BaseMethodDeclarationSyntax node, SemanticModel model)
        //   where TAttribute : Attribute {
        //    if (node == null) throw new ArgumentNullException(nameof(node));
        //    if (model == null) throw new ArgumentNullException(nameof(model));

        //    var paramAttributes = node.ParameterList.Parameters
        //        .SelectMany(p => p.AllAttributes());
        //    var allAttributes = paramAttributes.Concat(node.AllAttributes());
            
        //    return HasAttributeImpl<TAttribute>(allAttributes, model);
        //}

        public static bool HasAnyAttributeExtending<TAttribute>(this BaseMethodDeclarationSyntax node, SemanticModel model)
           where TAttribute : Attribute {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));
            return HasAttributeExtendingImpl<TAttribute>(node.AllAttributes(), model) ||
                HasAttributeExtendingImpl<TAttribute>(node.ParameterList.Parameters.SelectMany(p => p.AllAttributes()), model);
        }
        
        private static bool HasAttributeImpl<TAttribute>(IEnumerable<AttributeSyntax> attributes, SemanticModel model)
            where TAttribute : Attribute {

            var symbol = typeof(TAttribute).GetTypeSymbol(model);

            IEnumerable<ITypeSymbol> attributeTypes = new ITypeSymbol[0];

            try {
                attributeTypes = attributes
                    .Select(a => model.GetTypeInfo(a).Type)
                    .ToArray();
            }
            catch (Exception e) {
                Console.WriteLine($"Traction : error : {e.Message} @{e.StackTrace}");
                return false;
            }

            return attributeTypes.Any(t => t.Equals(symbol));
        }

        private static bool HasOrInheritsAttributeImpl<TAttribute>(IEnumerable<AttributeSyntax> attributes, SemanticModel model)
            where TAttribute : Attribute {

            var symbol = typeof(TAttribute).GetTypeSymbol(model);

            IEnumerable<ITypeSymbol> attributeTypes = new ITypeSymbol[0];

            try {
                attributeTypes = attributes
                    .Select(a => model.GetTypeInfo(a).Type)
                    .ToArray();
            }
            catch (Exception e) {
                Console.WriteLine($"Traction : error : {e.Message} @{e.StackTrace}");
                return false;
            }

            return attributeTypes.Any(t => t.Equals(symbol));
        }

        private static bool HasAttributeExtendingImpl<TAttribute>(IEnumerable<AttributeSyntax> attributes, SemanticModel model)
            where TAttribute : Attribute {

            var symbol = typeof(TAttribute).GetTypeSymbol(model);

            IEnumerable<ITypeSymbol> attributeTypes = new ITypeSymbol[0];

            try {
                attributeTypes = attributes
                    .Select(a => model.GetTypeInfo(a).Type)
                    .ToArray();
            }
            catch (Exception e) {
                Console.WriteLine($"Traction : error : {e.Message} @{e.StackTrace}");
                return false;
            }

            return attributeTypes.Any(t => t.Ancestors().Contains(symbol));
        }
    }
}
