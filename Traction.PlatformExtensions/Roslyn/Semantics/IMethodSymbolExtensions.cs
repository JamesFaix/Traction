using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Traction.Roslyn.Semantics {

    public static class IMethodSymbolExtensions {

        public static IEnumerable<AttributeData> DeclaredAndInheritedAttributes(this IMethodSymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            var symbols = @this.OverriddenAndImplementedInterfaceMembers();

            return symbols
                .SelectMany(s => s.GetAttributes())
                .Concat(symbols.OfType<IMethodSymbol>()
                               .SelectMany(s => s.GetReturnTypeAttributes()));
        }

        public static IEnumerable<IMethodSymbol> ImplementedInterfaceMembers(this IMethodSymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            return @this
                .ContainingType
                .AllInterfaces
                .SelectMany(i => i.GetMembers()
                                  .OfType<IMethodSymbol>())
                .Where(m => @this.Equals(
                            @this.ContainingType.FindImplementationForInterfaceMember(m)));
        }

        public static bool IsInterfaceImplementation(this IMethodSymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.ImplementedInterfaceMembers().Any();
        }

        public static bool IsOverrideOrInterfaceImplementation(this IMethodSymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.OverriddenAndImplementedInterfaceMembers().Count() > 1; //1 for self
        }

        public static IMethodSymbol OverriddenMember(this IMethodSymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.OverriddenMethod;
        }

        public static IEnumerable<IMethodSymbol> OverriddenAndImplementedInterfaceMembers(this IMethodSymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            var result = new List<IMethodSymbol>();

            var overridden = @this.OverriddenMethod;
            if (overridden != null) {
                result.AddRange(OverriddenAndImplementedInterfaceMembers(overridden));
            }

            result.AddRange(@this.ImplementedInterfaceMembers());
            result.Add(@this);
            return result;
        }
    }
}
