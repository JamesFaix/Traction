using Traction.Contracts;
using Traction.Contracts.Analysis;
using Traction.Contracts.Expansion;
using Traction.Contracts.Injection;
using Traction.SEPrecompilation;

namespace Traction {

    /// <summary>
    /// <c>ICompileModule</c> for contract rewriting.
    /// </summary>
    public class TractionCompileModule : CompileModuleBase {

        public TractionCompileModule()
            : base(
                  precompilationChain: GetPrecompilationChain(),
                  postcompilationChain: null) {

#if DEBUG_ON_BUILD
            System.Diagnostics.Debugger.Launch();
#endif
        }

        private static RewriterChain GetPrecompilationChain() {
            var contractProvider = ContractProvider.Instance;
            return new RewriterChain(
                (model, context) => new Analyzer(model, context, contractProvider),
                (model, context) => new AutoPropertyExpander(model, context, contractProvider),
                (model, context) => new ExpressionBodiedMemberExpander(model, context, contractProvider),
                (model, context) => new IteratorBlockExpander(model, context, contractProvider),
                (model, context) => new Injector(model, context, contractProvider)
            );
        }
    }
}
