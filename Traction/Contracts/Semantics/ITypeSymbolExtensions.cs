using System.Linq;
using Microsoft.CodeAnalysis;
using Traction.Roslyn.Semantics;

namespace Traction.Contracts.Semantics {

    internal static class ITypeSymbolExtensions {

        public static bool CanBeNull(this ITypeSymbol @this) =>
           !@this.IsValueType || @this.IsNullable();

        public static bool IsNullable(this ITypeSymbol @this) =>
            @this.FullName().EndsWith("?");

        public static bool IsEquatable(this ITypeSymbol @this) =>
            @this.AllInterfaces
                .Any(i => i.FullName().StartsWith("global::System.IEquatable<"));

        public static bool IsComparable(this ITypeSymbol @this) =>
            @this.AllInterfaces
                .Any(i => i.FullName().StartsWith("global::System.IComparable<"));

        public static bool IsEnumerable(this ITypeSymbol @this) =>
            @this.AllInterfaces
                .Any(i => i.FullName().StartsWith("global::System.Collections.Generic.IEnumerable"));
    }
}
