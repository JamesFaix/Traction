using System;
using System.Collections.Generic;
using StackExchange.Precompilation;

namespace Traction.SEPrecompilation {

    /// <summary>
    /// Basic implementation of <c>ICompileModule</c> that can perform a series of rewriting operations.
    /// </summary>
    public abstract class CompileModuleBase : ICompileModule {

        protected CompileModuleBase() {
            precompilationRewriterProviders = new List<IRewriterProvider>();
            postcompilationRewriterProviders = new List<IRewriterProvider>();
        }

        private readonly List<IRewriterProvider> precompilationRewriterProviders;
        private readonly List<IRewriterProvider> postcompilationRewriterProviders;

        protected void AddPrecompilationRewriterProvider(IRewriterProvider provider) {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            precompilationRewriterProviders.Add(provider);
        }

        protected void AddPrecompilationRewriterProviders(IEnumerable<IRewriterProvider> providers) {
            if (providers == null) throw new ArgumentNullException(nameof(providers));
            precompilationRewriterProviders.AddRange(providers);
        }

        protected void AddPostcompilationRewriterProvider(IRewriterProvider provider) {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            postcompilationRewriterProviders.Add(provider);
        }

        protected void AddPostcompilationRewriterProvider(IEnumerable<IRewriterProvider> providers) {
            if (providers == null) throw new ArgumentNullException(nameof(providers));
            postcompilationRewriterProviders.AddRange(providers);
        }

        public void BeforeCompile(BeforeCompileContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context));
            Process(new BeforeCompileContextWrapper(context), precompilationRewriterProviders);
        }

        public void AfterCompile(AfterCompileContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context));
            Process(new AfterCompileContextWrapper(context), postcompilationRewriterProviders);
        }

        private void Process(ICompileContext context, IEnumerable<IRewriterProvider> rewriterProviders) {
            foreach (var provider in rewriterProviders) {
                foreach (var syntaxTree in context.Compilation.SyntaxTrees) {
                    var model = context.Compilation.GetSemanticModel(syntaxTree);
                    var rewriter = provider.Create(model, context);
                    if (rewriter == null) throw new InvalidOperationException("Rewriter generator cannot return null.");

                    var rootNode = syntaxTree.GetRoot();
                    var rewritten = rewriter.Visit(rootNode);

                    if (rootNode != rewritten) {
                        context.Compilation = context.Compilation.ReplaceSyntaxTree(
                          syntaxTree,
                          syntaxTree.WithRootAndOptions(rewritten, syntaxTree.Options));
                    }
                }
            }
        }
    }
}
