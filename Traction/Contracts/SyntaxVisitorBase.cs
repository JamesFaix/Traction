using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Traction.SEPrecompilation;

namespace Traction.Contracts {

    internal abstract class SyntaxVisitorBase : CSharpSyntaxRewriter {

        protected SyntaxVisitorBase(SemanticModel model, ICompileContext context, IContractProvider contractProvider) {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (contractProvider == null) throw new ArgumentNullException(nameof(contractProvider));

            this.model = model;
            this.context = context;
            this.contractProvider = contractProvider;
        }

        protected readonly SemanticModel model;
        protected readonly ICompileContext context;
        protected readonly IContractProvider contractProvider;
    }
}
