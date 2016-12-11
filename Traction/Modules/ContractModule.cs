using StackExchange.Precompilation;

namespace Traction {

    /// <summary>
    /// <c>ICompileModule</c> for contract rewriting.
    /// </summary>
    public class ContractModule : CompileModuleBase, ICompileModule {

        public ContractModule() : base() {
#if DEBUG_ON_BUILD
            Debugger.Launch();
#endif
            AddBeforeRewriterProviders(new RewriterFactoryMethod[] {
                RewriterFactory.AutoPropertyExpander,
                RewriterFactory.ExpressionBodiedMemberExpander,
                RewriterFactory.NonNull,
                RewriterFactory.NonDefault,
                RewriterFactory.NonEmpty,
                RewriterFactory.Positive
            });
        }
    }
}
