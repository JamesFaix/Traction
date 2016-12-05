using StackExchange.Precompilation;

namespace Precompiler {

    public class StringOptimizingModule : CompileModuleBase, ICompileModule {

        public StringOptimizingModule() : base() {
            AddPreCompileGenerator((model) => new StringBuilderInterpolationOptimizer(model));
        }
    }
}
