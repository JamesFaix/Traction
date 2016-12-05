using StackExchange.Precompilation;

namespace Precompiler {

    public class ContractModule : CompileModuleBase, ICompileModule {

        public ContractModule() : base() {
            
            AddPrecompileGenerator((model) => new NonNullAttributeReWriter(model));
        }
    }
}
