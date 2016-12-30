using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Traction.SEPrecompilation;

namespace Traction.Contracts.Expansion {

    internal class IteratorBlockExpanderProvider : IRewriterProvider {

        public CSharpSyntaxRewriter Create(SemanticModel model, ICompileContext context) =>
            new IteratorBlockExpander(model, context);
    }
}
