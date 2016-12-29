using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Traction.Contracts.Injection;
using Traction.SEPrecompilation;

namespace Traction.Contracts {

    internal class ContractRewriterProvider : IRewriterProvider {

        public ContractRewriterProvider(Contract contract) {
            if (contract == null) throw new ArgumentNullException(nameof(contract));

            this.contract = contract;
        }

        private readonly Contract contract;

        public CSharpSyntaxRewriter Create(SemanticModel model, ICompileContext context) {
            return new ContractRewriter(model, context, contract);
        }
    }
}
