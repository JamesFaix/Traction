﻿using System.Linq;
using StackExchange.Precompilation;
using Traction.Contracts;
using Traction.SEPrecompilation;

namespace Traction {

    /// <summary>
    /// <c>ICompileModule</c> for contract rewriting.
    /// </summary>
    public class ContractModule : CompileModuleBase {

        public ContractModule() : base() {

#if DEBUG_ON_BUILD
            System.Diagnostics.Debugger.Launch();
#endif

            //Add syntax expanders first, so contracts can operate on expanded syntax.
            AddPrecompilationRewriterProviders(new RewriterFactoryMethod[] {
                AutoPropertyExpander.Create,
                ExpressionBodiedMemberExpander.Create,
                IteratorBlockExpander.Create
            });

            AddPrecompilationRewriterProviders(
                ContractProvider.Instance.Contracts
                    .Select(c => ContractRewriter.Create(c)));
        }
    }
}
