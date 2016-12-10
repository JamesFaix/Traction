using System.Diagnostics;
using StackExchange.Precompilation;

namespace Traction {

    public class ContractModule : CompileModuleBase, ICompileModule {

        public ContractModule() : base() {
#if DEBUG_ON_BUILD
            Debugger.Launch();
#endif
            AddBeforeRewriterProvider(RewriterFactory.AutoPropertyExpander);
            AddBeforeRewriterProvider(RewriterFactory.ExpressionBodiedMemberExpander);
            AddBeforeRewriterProvider(RewriterFactory.NonNull);
        }
    }
}
