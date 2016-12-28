using StackExchange.Precompilation;
using System.Linq;
using Traction.Contracts;

namespace Traction {

    /// <summary>
    /// <c>ICompileModule</c> for contract rewriting.
    /// </summary>
    public class ContractModule : CompileModuleBase, ICompileModule {

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
                MasterContractProvider.Instance.Contracts
                    .Select(c => ContractRewriter.Create(c)));
        }
    }
}
