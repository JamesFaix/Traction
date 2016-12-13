using StackExchange.Precompilation;

namespace Traction {

    /// <summary>
    /// <c>ICompileModule</c> for contract rewriting.
    /// </summary>
    public class ContractModule : CompileModuleBase, ICompileModule {

        public ContractModule() : base() {

#if DEBUG_ON_BUILD
            System.Diagnostics.Debugger.Launch();
#endif

            //Add syntax expanders first, so contracts can operate on expanded syntax.
            AddPrecompilationRewriterProviders(new RewriterFactoryMethod[] {
                AutoPropertyExpander.Create,
                ExpressionBodiedMemberExpander.Create,
                ContractRewriter.Create(new NonNullContract()),
                ContractRewriter.Create(new NonDefaultContract()),
                ContractRewriter.Create(new NonEmptyContract()),
                ContractRewriter.Create(new PositiveContract()),
                ContractRewriter.Create(new NegativeContract()),
                ContractRewriter.Create(new NonPositiveContract()),
                ContractRewriter.Create(new NonNegativeContract())
            });
        }
    }
}
