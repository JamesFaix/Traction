using System.Diagnostics;
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
            AddBeforeRewriterProvider(RewriterFactory.AutoPropertyExpander);
            AddBeforeRewriterProvider(RewriterFactory.ExpressionBodiedMemberExpander);
            AddBeforeRewriterProvider(RewriterFactory.NonNull);
            AddBeforeRewriterProvider(RewriterFactory.NonDefault);
            AddBeforeRewriterProvider(RewriterFactory.NonEmpty);
        }
    }
}
