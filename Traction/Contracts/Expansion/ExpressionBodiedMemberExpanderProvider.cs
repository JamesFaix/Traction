using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Traction.SEPrecompilation;

namespace Traction.Contracts.Expansion {

    internal class ExpressionBodiedMemberExpanderProvider : IRewriterProvider {

        public CSharpSyntaxRewriter Create(SemanticModel model, ICompileContext context) =>
            new ExpressionBodiedMemberExpander(model, context);
    }
}
