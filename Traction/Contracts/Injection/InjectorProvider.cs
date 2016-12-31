using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Traction.Contracts.Injection;
using Traction.SEPrecompilation;

namespace Traction.Contracts {

    internal class InjectorProvider : IRewriterProvider {

        public InjectorProvider(IContractProvider contractProvider) {
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            this.contractProvider = contractProvider;
        }

        private readonly IContractProvider contractProvider;

        public CSharpSyntaxRewriter Create(SemanticModel model, ICompileContext context) {
            return new Injector(model, context, contractProvider);
        }
    }
}
