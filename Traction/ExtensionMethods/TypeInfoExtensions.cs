using System.Linq;
using Microsoft.CodeAnalysis;

namespace Traction {

    /// <summary>
    /// Extension methods for <see cref="TypeInfo"/> 
    /// </summary>
    static class TypeInfoExtensions {

        public static string FullName(this TypeInfo type) {

            var t = type.Type;

            return type.Type.ToDisplayString(
                new SymbolDisplayFormat(
                    SymbolDisplayGlobalNamespaceStyle.Included,
                    SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                    SymbolDisplayGenericsOptions.IncludeTypeParameters));
        }

        public static bool IsNullable(this TypeInfo type) =>
            !type.Type.IsValueType
            || type.FullName().EndsWith("?");

        public static bool IsEquatable(this TypeInfo type) =>
            type.Type.AllInterfaces
                .Any(i => i.FullName().StartsWith("global::System.IEquatable<"));

        public static bool IsComparable(this TypeInfo type) =>
            type.Type.AllInterfaces
                .Any(i => i.FullName().StartsWith("global::System.IComparable<"));
        
        public static bool IsEnumerable(this TypeInfo type) =>
            type.Type.AllInterfaces
                .Any(i => i.FullName().StartsWith("global::System.Collections.Generic.IEnumerable"));
    }
}
