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
                ContractRewriter<NonNullAttribute>.Create(new NonNullContract()),
                ContractRewriter<NonDefaultAttribute>.Create(new NonDefaultContract()),
                ContractRewriter<NonEmptyAttribute>.Create(new NonEmptyContract()),
                ContractRewriter<PositiveAttribute>.Create(new PositiveContract()),
                ContractRewriter<NegativeAttribute>.Create(new NegativeContract()),
                ContractRewriter<NonPositiveAttribute>.Create(new NonPositiveContract()),
                ContractRewriter<NonNegativeAttribute>.Create(new NonNegativeContract())
            });
        }
    }
}
