using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Traction.SEPrecompilation;
using System;

namespace Traction.Contracts.Expansion {

    internal class ExpressionBodiedMemberExpanderProvider : IRewriterProvider {

        public ExpressionBodiedMemberExpanderProvider(IContractProvider contractProvider) {
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));
            this.contractProvider = contractProvider;
        }

        private readonly IContractProvider contractProvider;

        public CSharpSyntaxRewriter Create(SemanticModel model, ICompileContext context) {
            return new ExpressionBodiedMemberExpander(model, context, contractProvider);
        }
    }
}
