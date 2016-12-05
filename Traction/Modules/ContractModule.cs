using StackExchange.Precompilation;

namespace Traction {

    public class ContractModule : CompileModuleBase, ICompileModule {

        public ContractModule() : base() {
            
            AddBeforeRewriterProvider(new NonNullAttributeRewriterProvider());
        }
    }
}
