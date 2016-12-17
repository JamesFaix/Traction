using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Traction {

    static class SymbolExtensions {

        public static IEnumerable<IMethodSymbol> InterfaceImplementations(this IMethodSymbol symbol) {
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));

            var interfaceMethods = symbol.ContainingType
                   .AllInterfaces
                   .SelectMany(i => i.GetMembers().OfType<IMethodSymbol>());

            return interfaceMethods
                .Where(method => symbol.Equals(
                    symbol.ContainingType.FindImplementationForInterfaceMember(method)));
        }

        public static IEnumerable<IPropertySymbol> InterfaceImplementations(this IPropertySymbol symbol) {
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));

            var interfaceMethods = symbol.ContainingType
                   .AllInterfaces
                   .SelectMany(i => i.GetMembers().OfType<IPropertySymbol>());

            return interfaceMethods
                .Where(method => symbol.Equals(
                    symbol.ContainingType.FindImplementationForInterfaceMember(method)));
        }

        public static IEnumerable<IMethodSymbol> AllAncestorSymbols(this IMethodSymbol symbol) {
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));

            var result = new List<IMethodSymbol>();

            //Add ultimate base class implementation
            if (symbol.IsOverride) {
                result.AddRange(AllAncestorSymbols(symbol.OverriddenMethod));
            }

            //Add any interface definitions
            result.AddRange(symbol.InterfaceImplementations());

            result.Add(symbol);

            return result;
        }

        public static IEnumerable<IParameterSymbol> AllAncestorSymbols(this IParameterSymbol symbol) {
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));

            var result = new List<IParameterSymbol>();

            var method = symbol.ContainingSymbol as IMethodSymbol;

            //Add ultimate base class implementation
            if (method.IsOverride) {
                var ancestorMethod = method.OverriddenMethod;

                var ancestorParam = ancestorMethod
                    .Parameters
                    .Single(p => p.Name == symbol.Name);

                result.AddRange(AllAncestorSymbols(ancestorParam));
            }

            //Add any interface definitions
            foreach (var m in method.InterfaceImplementations()) {
                result.Add(m.Parameters.Single(p => p.Name == symbol.Name));
            }

            result.Add(symbol);

            return result;
        }

        public static IEnumerable<IPropertySymbol> AllAncestorSymbols(this IPropertySymbol symbol) {
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));

            var result = new List<IPropertySymbol>();

            //Add ultimate base class implementation
            if (symbol.IsOverride) {
                result.AddRange(AllAncestorSymbols(symbol.OverriddenProperty));
            }

            //Add any interface definitions
            result.AddRange(symbol.InterfaceImplementations());
            
            result.Add(symbol);

            return result;
        }

        public static IEnumerable<AttributeData> AllAncestorAttributes(this IMethodSymbol symbol) {
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));

            var symbols = symbol.AllAncestorSymbols();
            
            return symbols.SelectMany(s => 
                s.GetAttributes()
                .Concat(s.GetReturnTypeAttributes()));
        }

        public static IEnumerable<AttributeData> AllAncestorAttributes(this IParameterSymbol symbol) {
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));

            return symbol
                .AllAncestorSymbols()
                .SelectMany(s => s.GetAttributes());
        }

        public static IEnumerable<AttributeData> AllAncestorAttributes(this IPropertySymbol symbol) {
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));

            return symbol
                .AllAncestorSymbols()
                .SelectMany(s => s.GetAttributes());
        }
    }
}
