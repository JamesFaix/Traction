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

            if (@this is IMethodSymbol) {
                return (@this.ContainingSymbol as IMethodSymbol)
                    .ImplementedInterfaceMembers()
                    .Select(m => m.Parameters
                                  .Single(p => p.Name == @this.Name));
            }
            else {
                return (@this.ContainingSymbol as IPropertySymbol)
                    .ImplementedInterfaceMembers()
                    .Select(m => m.Parameters
                                  .Single(p => p.Name == @this.Name));
            }
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

            var member = @this.ContainingSymbol;
            if (!member.IsOverride) return null;

            var parameters = member is IMethodSymbol
                ? (member as IMethodSymbol).OverriddenMethod.Parameters
                : (member as IPropertySymbol).OverriddenProperty.Parameters;

            return parameters
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

            result.AddRange(@this.ImplementedInterfaceMembers());
            return result;
        }
    }
}
