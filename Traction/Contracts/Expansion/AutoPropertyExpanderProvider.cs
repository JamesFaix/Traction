using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Traction.SEPrecompilation;

namespace Traction.Contracts.Expansion {

    internal class AutoPropertyExpanderProvider : IRewriterProvider {

        public CSharpSyntaxRewriter Create(SemanticModel model, ICompileContext context) =>
            new AutoPropertyExpander(model, context);
    }
}
