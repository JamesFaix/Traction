using StackExchange.Precompilation;

namespace Traction {

    public class ContractModule : CompileModuleBase, ICompileModule {

        public ContractModule() : base() {
            
            AddPrecompileGenerator((model) => new NonNullAttributeReWriter(model));
        }
    }
}
