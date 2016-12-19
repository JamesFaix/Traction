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

        public static IEnumerable<AttributeSyntax> AllAttributes<TNode>(this TNode @this)
            where TNode : CSharpSyntaxNode {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            SyntaxList<AttributeListSyntax> attributeLists;
            try {
                attributeLists = ((dynamic)(@this)).AttributeLists;
            }
            catch {
                throw new NotSupportedException($"Unsupported node type. ({typeof(TNode)})");
            }

            return attributeLists.SelectMany(list => list.Attributes);
        }

        public static bool HasAttribute<TNode, TAttribute>(this TNode @this, SemanticModel model)
            where TNode : CSharpSyntaxNode
            where TAttribute : Attribute {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));
           
            var symbol = typeof(TAttribute).GetTypeSymbol(model);

            IEnumerable<ITypeSymbol> attributeTypes = new ITypeSymbol[0];

            try {
                attributeTypes = @this.AllAttributes()
                    .Select(a => model.GetTypeInfo(a).Type)
                    .ToArray();
            }
            catch (Exception e) {
                Console.WriteLine($"Traction : error : {e.Message} @{e.StackTrace}");
                return false;
            }

            return attributeTypes.Any(t => t.Equals(symbol));
        }

        public static bool HasOrInheritsAttribute<TNode, TSymbol, TAttribute>(this TNode @this, SemanticModel model)
            where TNode : CSharpSyntaxNode
            where TSymbol: class, ISymbol
            where TAttribute : Attribute {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var symbol = typeof(TAttribute).GetTypeSymbol(model);

            var attributeTypeSymbols = (model
                .GetDeclaredSymbol(@this) as TSymbol)
                .AncestorAttributes()
                .Select(a => a.AttributeClass);

            return attributeTypeSymbols.Any(t => t.Equals(symbol));
        }        
        
        public static bool HasAttributeExtending<TNode, TAttribute>(this TNode @this, SemanticModel model)
            where TNode : CSharpSyntaxNode
            where TAttribute : Attribute {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var symbol = typeof(TAttribute).GetTypeSymbol(model);

            IEnumerable<ITypeSymbol> attributeTypes = new ITypeSymbol[0];

            try {
                attributeTypes = @this.AllAttributes()
                    .Select(a => model.GetTypeInfo(a).Type)
                    .ToArray();
            }
            catch (Exception e) {
                Console.WriteLine($"Traction : error : {e.Message} @{e.StackTrace}");
                return false;
            }

            return attributeTypes.Any(t => t.Ancestors().Contains(symbol));
        }
        
        public static bool Extends<TAttribute> (this AttributeSyntax @this, SemanticModel model)
            where TAttribute: Attribute {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var symbol1 = typeof(TAttribute).GetTypeSymbol(model);
            var symbol2 = model.GetTypeInfo(@this).Type;
            var result = symbol2.Ancestors().Contains(symbol1);
            return result;
        }     
    }
}
