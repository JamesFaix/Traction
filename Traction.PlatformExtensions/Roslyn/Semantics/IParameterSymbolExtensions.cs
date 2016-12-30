using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Traction.Linq;

namespace Traction.Roslyn.Semantics {

    public static class IParameterSymbolExtensions {

        public static IEnumerable<AttributeData> DeclaredAndInheritedAttributes(this IParameterSymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            return @this
                .Concat(@this.OverriddenAndImplementedInterfaceMembers())
                .SelectMany(s => s.GetAttributes());
        }

        public static IEnumerable<IParameterSymbol> ImplementedInterfaceMembers(this IParameterSymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            return (@this.ContainingSymbol as IMethodSymbol)
                .ImplementedInterfaceMembers()
                .Select(m => m.Parameters
                              .Single(p => p.Name == @this.Name));
        }

        public static bool IsInterfaceImplementation(this IParameterSymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.ImplementedInterfaceMembers().Any();
        }

        public static bool IsOverrideOrInterfaceImplementation(this IParameterSymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.OverriddenAndImplementedInterfaceMembers().Any(); //.Count() > 1; //1 for self
        }

        public static IParameterSymbol OverriddenMember(this IParameterSymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            var method = @this.ContainingSymbol as IMethodSymbol;
            if (!method.IsOverride) return null;

            var ancestorMethod = method.OverriddenMethod;
            return ancestorMethod
                .Parameters
                .Single(p => p.Name == @this.Name) as IParameterSymbol;
        }

        public static IEnumerable<IParameterSymbol> OverriddenAndImplementedInterfaceMembers(this IParameterSymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            var result = new List<IParameterSymbol>();

            var overridden = @this.OverriddenMember();
            if (overridden != null) {
                result.Add(overridden);
                result.AddRange(OverriddenAndImplementedInterfaceMembers(overridden));
            }

            var method = @this.ContainingSymbol as IMethodSymbol;

            result.AddRange(method
                            .ImplementedInterfaceMembers()
                            .Select(m => m.Parameters
                                          .Single(p => p.Name == @this.Name)));
            //  result.Add(@this);
            return result;
        }
    }
}
