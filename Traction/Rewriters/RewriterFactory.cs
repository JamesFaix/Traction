using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Traction {

    static class RewriterFactory {
        
        public static CSharpSyntaxRewriter NonNullAttribute(SemanticModel model, ICompileContext context) {
            return new NonNullAttributeReWriter(model, context);
        }

        public static CSharpSyntaxRewriter AutoPropertyExpander(SemanticModel model, ICompileContext context) {
            return new AutoPropertyExpander(model, context);
        }

        public static CSharpSyntaxRewriter ExpressionBodiedMemberExpander(SemanticModel model, ICompileContext context) {
            return new ExpressionBodiedMemberExpander(model, context);
        }
    }
}
