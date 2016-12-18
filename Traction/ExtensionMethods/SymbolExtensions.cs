using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Traction {

    static class SymbolExtensions {

        public static IEnumerable<TSymbol> InterfaceImplementations<TSymbol>(this TSymbol @this)
            where TSymbol : ISymbol {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            var interfaceMembers = @this.ContainingType
                   .AllInterfaces
                   .SelectMany(i => i.GetMembers().OfType<TSymbol>());

            return interfaceMembers
                .Where(m => @this.Equals(
                    @this.ContainingType.FindImplementationForInterfaceMember(m)));
        }

        public static TSymbol OverriddenSymbol<TSymbol>(this TSymbol @this)
            where TSymbol : class, ISymbol {

            var type = typeof(TSymbol);

            if (type == typeof(IPropertySymbol)) {
                var property = @this as IPropertySymbol;
                if (!property.IsOverride) return null;
                return property.OverriddenProperty as TSymbol;
            }
            else if (type == typeof(IMethodSymbol)) {
                var method = @this as IMethodSymbol;
                if (!method.IsOverride) return null;
                return method.OverriddenMethod as TSymbol;
            }
            else if (type == typeof(IParameterSymbol)) {
                var method = @this.ContainingSymbol as IMethodSymbol;
                if (!method.IsOverride) return null;

                var ancestorMethod = method.OverriddenMethod;
                return ancestorMethod
                    .Parameters
                    .Single(p => p.Name == @this.Name) as TSymbol;
            }
            else {
                throw new NotSupportedException("Unsupported symbol type.");
            }
        }

        public static IEnumerable<TSymbol> AncestorSymbols<TSymbol>(this TSymbol @this)
            where TSymbol : class, ISymbol {

            var result = new List<TSymbol>();

            var overridden = @this.OverriddenSymbol();
            if (overridden != null) {
                result.AddRange(AncestorSymbols(overridden));
            }

            var type = typeof(TSymbol);

            if (type == typeof(IPropertySymbol)
            || type == typeof(IMethodSymbol)) {
                result.AddRange(@this.InterfaceImplementations());
            }
            else if (type == typeof(IParameterSymbol)) {
                var method = @this.ContainingSymbol as IMethodSymbol;
                foreach (var m in method.InterfaceImplementations()) {
                    result.Add(m.Parameters.Single(p => p.Name == @this.Name) as TSymbol);
                }
            }
            else {
                throw new NotSupportedException("Unsupported symbol type.");
            }

            result.Add(@this);
            return result;
        }

        public static IEnumerable<AttributeData> AncestorAttributes<TSymbol>(this TSymbol @this)
            where TSymbol : class, ISymbol {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            var symbols = @this.AncestorSymbols();

            var result = symbols.SelectMany(s => s.GetAttributes());

            if (typeof(TSymbol) == typeof(IMethodSymbol)) {
                result = result.Concat(
                    symbols.OfType<IMethodSymbol>()
                           .SelectMany(s => s.GetReturnTypeAttributes()));
            }

            return result;
        }
    }
}
