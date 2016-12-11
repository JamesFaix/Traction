using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Traction {

    /// <summary>
    /// Factory for syntax rewriters.
    /// </summary>
    static class RewriterFactory {
        
        public static CSharpSyntaxRewriter AutoPropertyExpander(SemanticModel model, ICompileContext context) {
            return new AutoPropertyExpander(model, context);
        }

        public static CSharpSyntaxRewriter ExpressionBodiedMemberExpander(SemanticModel model, ICompileContext context) {
            return new ExpressionBodiedMemberExpander(model, context);
        }

        public static CSharpSyntaxRewriter NonNull(SemanticModel model, ICompileContext context) {
            return new NonNullReWriter(model, context);
        }

        public static CSharpSyntaxRewriter NonDefault(SemanticModel model, ICompileContext context) {
            return new NonDefaultRewriter(model, context);
        }

        public static CSharpSyntaxRewriter NonEmpty(SemanticModel model, ICompileContext context) {
            return new NonEmptyRewriter(model, context);
        }
    }
}
