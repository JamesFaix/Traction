﻿using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction {

    static class PropertySyntaxExtensions {

        public static AccessorDeclarationSyntax Getter(this PropertyDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));

            return node.AccessorList.Accessors
                .SingleOrDefault(accessor => accessor.Kind().ToString().StartsWith("Get"));
        }

        public static AccessorDeclarationSyntax Setter(this PropertyDeclarationSyntax node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            //Debugger.Launch();
            return node.AccessorList.Accessors
                .SingleOrDefault(accessor => accessor.Kind().ToString().StartsWith("Set"));
        }
    }
}
