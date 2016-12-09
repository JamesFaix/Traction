using System.Diagnostics;
using StackExchange.Precompilation;

namespace Traction {

    public class ContractModule : CompileModuleBase, ICompileModule {

        public ContractModule() : base() {
#if DEBUG
            Debugger.Launch();
#endif
            AddBeforeRewriterProvider(RewriterFactory.SyntaxExpander);
           // AddBeforeRewriterProvider(RewriterFactory.NonNullAttribute);
        }
    }
}
