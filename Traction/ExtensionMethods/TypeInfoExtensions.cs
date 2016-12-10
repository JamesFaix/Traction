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
    }
}
