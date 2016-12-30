using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Traction.SEPrecompilation {

    public interface IRewriterProvider {

        CSharpSyntaxRewriter Create(
            SemanticModel model, 
            ICompileContext context);

    }
}
