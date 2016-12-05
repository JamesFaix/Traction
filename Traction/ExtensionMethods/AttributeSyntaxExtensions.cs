﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace Traction {

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

        public static IEnumerable<AttributeSyntax> AllAttributes(this TypeDeclarationSyntax node) {
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

        public static bool HasAttribute<TAttribute>(this TypeDeclarationSyntax node, SemanticModel model) 
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

        public static bool HasReturnValueAttribute<TAttribute>(this MethodDeclarationSyntax node, SemanticModel model)
            where TAttribute: Attribute {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (model == null) throw new ArgumentNullException(nameof(model));
            var attributes = node.ReturnValueAttributes();
            return HasAttributeImpl<TAttribute>(attributes, model);
        }

        private static bool HasAttributeImpl<TAttribute>(IEnumerable<AttributeSyntax> attributes, SemanticModel model) 
            where TAttribute : Attribute {
           // Debugger.Launch();
            var symbol = typeof(TAttribute).GetTypeSymbol(model);
            var attributeTypes = attributes.Select(a => model.GetTypeInfo(a).Type);
            return attributeTypes.Any(t => t.Equals(symbol));
        }

        public static IEnumerable<AttributeSyntax> ReturnValueAttributes(this MethodDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            //   Debugger.Launch();
            return node.AttributeLists
                .Where(list => list.SpecifierText() == "return")
                .SelectMany(list => list.Attributes);
        }

        public static string SpecifierText(this AttributeListSyntax attributes) {
            if (attributes == null) throw new ArgumentNullException(nameof(attributes));
            return attributes.ChildNodes()
                .OfType<AttributeTargetSpecifierSyntax>()
                .FirstOrDefault()
                ?.Identifier.Text;
        }        
    }
}
