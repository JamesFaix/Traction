using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Traction {

    static class RewriterFactory {

        public static CSharpSyntaxRewriter SyntaxExpander(SemanticModel model, ICompileContext context) {
            return new SyntaxExpander(model, context);
        }

        public static CSharpSyntaxRewriter NonNullAttribute(SemanticModel model, ICompileContext context) {
            return new NonNullAttributeReWriter(model, context);
        }
    }
}
