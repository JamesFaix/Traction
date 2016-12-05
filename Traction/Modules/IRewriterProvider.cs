using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Traction {

    public interface IRewriterProvider {

        CSharpSyntaxRewriter CreateRewriter(SemanticModel model, ICompileContext context);

    }
}
