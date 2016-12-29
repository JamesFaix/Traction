using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Traction.Roslyn {

    public static class SyntaxTokenExtensions {

        public static bool IsAccessModifier(this SyntaxToken @this) =>
            @this.IsKind(SyntaxKind.PublicKeyword) ||
            @this.IsKind(SyntaxKind.PrivateKeyword) ||
            @this.IsKind(SyntaxKind.ProtectedKeyword) ||
            @this.IsKind(SyntaxKind.InternalKeyword);

        public static bool IsInheritanceModifier(this SyntaxToken @this) =>
            @this.IsKind(SyntaxKind.VirtualKeyword) ||
            @this.IsKind(SyntaxKind.AbstractKeyword) ||
            @this.IsKind(SyntaxKind.OverrideKeyword) ||
            @this.IsKind(SyntaxKind.SealedKeyword);
    }
}
