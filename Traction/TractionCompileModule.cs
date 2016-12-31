using System.Linq;
using Traction.Contracts;
using Traction.Contracts.Expansion;
using Traction.SEPrecompilation;
using Traction.Contracts.Analysis;

namespace Traction {

    /// <summary>
    /// <c>ICompileModule</c> for contract rewriting.
    /// </summary>
    public class TractionCompileModule : CompileModuleBase {

        public TractionCompileModule() : base() {

#if DEBUG_ON_BUILD
            System.Diagnostics.Debugger.Launch();
#endif

            var contractProvider = ContractProvider.Instance;

            //Add syntax expanders first, so contracts can operate on expanded syntax.
            AddPrecompilationRewriterProviders(
                new IRewriterProvider[] {
                    new AnalyzerProvider(contractProvider),
                    new AutoPropertyExpanderProvider(contractProvider),
                    new ExpressionBodiedMemberExpanderProvider(contractProvider),
                    new IteratorBlockExpanderProvider(contractProvider),
                    new InjectorProvider(contractProvider)
                });
        }
    }
}
