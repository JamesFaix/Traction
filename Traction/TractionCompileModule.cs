using System.Linq;
using Traction.Contracts;
using Traction.Contracts.Expansion;
using Traction.SEPrecompilation;

namespace Traction {

    /// <summary>
    /// <c>ICompileModule</c> for contract rewriting.
    /// </summary>
    public class TractionCompileModule : CompileModuleBase {

        public TractionCompileModule() : base() {

#if DEBUG_ON_BUILD
            System.Diagnostics.Debugger.Launch();
#endif

            //Add syntax expanders first, so contracts can operate on expanded syntax.
            AddPrecompilationRewriterProviders(
                new IRewriterProvider[] {
                    new AutoPropertyExpanderProvider(),
                    new ExpressionBodiedMemberExpanderProvider(),
                    new IteratorBlockExpanderProvider()
                }
                .Concat(ContractProvider.Instance
                    .Contracts
                    .Select(c => new ContractRewriterProvider(c))));
        }
    }
}
