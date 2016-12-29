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
    public static class AttributeSyntaxExtensions {

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
    }
}
