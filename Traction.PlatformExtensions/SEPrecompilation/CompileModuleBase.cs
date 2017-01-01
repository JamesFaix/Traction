using System;
using System.Collections.Generic;
using StackExchange.Precompilation;

namespace Traction.SEPrecompilation {

    /// <summary>
    /// Basic implementation of <c>ICompileModule</c> that can perform a series of rewriting operations.
    /// </summary>
    public abstract class CompileModuleBase : ICompileModule {

        protected CompileModuleBase(
            RewriterChain precompilationChain = null,
            RewriterChain postcompilationChain = null) {

            this.precompChain = precompilationChain;
            this.postcompChain = postcompilationChain;
        }

        private readonly RewriterChain precompChain;
        private readonly RewriterChain postcompChain;

        public void BeforeCompile(BeforeCompileContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (precompChain != null) {
                precompChain.Process(new BeforeCompileContextWrapper(context));
            }
        }

        public void AfterCompile(AfterCompileContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (postcompChain != null) {
                postcompChain.Process(new AfterCompileContextWrapper(context));
            }
        }
    }
}
