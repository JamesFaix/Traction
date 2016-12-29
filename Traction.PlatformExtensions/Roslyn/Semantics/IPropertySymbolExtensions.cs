using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Traction.Roslyn.Semantics {

    public static class IPropertySymbolExtensions {

        public static IEnumerable<AttributeData> DeclaredAndInheritedAttributes(this IPropertySymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            return @this
                .OverriddenAndImplementedInterfaceMembers()
                .SelectMany(s => s.GetAttributes());
        }

        public static IEnumerable<IPropertySymbol> ImplementedInterfaceMembers(this IPropertySymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            return @this
                .ContainingType
                .AllInterfaces
                .SelectMany(i => i.GetMembers()
                                  .OfType<IPropertySymbol>())
                .Where(m => @this.Equals(
                            @this.ContainingType.FindImplementationForInterfaceMember(m)));
        }

        public static bool IsInterfaceImplementation(this IPropertySymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.ImplementedInterfaceMembers().Any();
        }
                
        public static bool IsOverrideOrInterfaceImplementation(this IPropertySymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.OverriddenAndImplementedInterfaceMembers().Count() > 1; //1 for self
        }

        public static IPropertySymbol OverriddenMember(this IPropertySymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            return @this.OverriddenProperty;
        }

        public static IEnumerable<IPropertySymbol> OverriddenAndImplementedInterfaceMembers(this IPropertySymbol @this) {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            var result = new List<IPropertySymbol>();

            var overridden = @this.OverriddenProperty;
            if (overridden != null) {
                result.AddRange(OverriddenAndImplementedInterfaceMembers(overridden));
            }

            result.AddRange(@this.ImplementedInterfaceMembers());
            result.Add(@this);
            return result;
        }
    }
}
