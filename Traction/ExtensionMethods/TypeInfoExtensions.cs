using Microsoft.CodeAnalysis;

namespace Traction {

    static class TypeInfoExtensions {

        public static string FullName(this TypeInfo type) {
            return type.Type.ToDisplayString(
                new SymbolDisplayFormat(
                    SymbolDisplayGlobalNamespaceStyle.Included,
                    SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces));
        }
    }
}
