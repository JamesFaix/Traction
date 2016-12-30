using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Traction.SEPrecompilation;

namespace Traction.Contracts.Analysis {

    internal class AnalyzerProvider : IRewriterProvider {

        public AnalyzerProvider(IContractProvider contractProvider) {
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));
            this.contractProvider = contractProvider;
        }

        private readonly IContractProvider contractProvider;

        public CSharpSyntaxRewriter Create(SemanticModel model, ICompileContext context) {
            return new Analyzer(model, context, contractProvider);
        }
    }
}
