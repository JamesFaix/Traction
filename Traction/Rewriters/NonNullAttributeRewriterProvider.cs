using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Traction {

    public class NonNullAttributeRewriterProvider : IRewriterProvider {

        public CSharpSyntaxRewriter CreateRewriter(SemanticModel model, ICompileContext context) {
            return new NonNullAttributeReWriter(model, context);
        }
    }
}
