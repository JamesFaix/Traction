using System;
using System.Collections.Generic;
using StackExchange.Precompilation;

namespace Traction.SEPrecompilation {

    /// <summary>
    /// Basic implementation of <c>ICompileModule</c> that can perform a series of rewriting operations.
    /// </summary>
    public abstract class CompileModuleBase : ICompileModule {

        protected CompileModuleBase() {
            precompilationRewriterProviders = new List<RewriterFactoryMethod>();
            postcompilationRewriterProviders = new List<RewriterFactoryMethod>();
        }

        private readonly List<RewriterFactoryMethod> precompilationRewriterProviders;
        private readonly List<RewriterFactoryMethod> postcompilationRewriterProviders;

        protected void AddPrecompilationRewriterProvider(RewriterFactoryMethod provider) {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            precompilationRewriterProviders.Add(provider);
        }

        protected void AddPrecompilationRewriterProviders(IEnumerable<RewriterFactoryMethod> providers) {
            if (providers == null) throw new ArgumentNullException(nameof(providers));
            precompilationRewriterProviders.AddRange(providers);
        }

        protected void AddPostcompilationRewriterProvider(RewriterFactoryMethod provider) {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            postcompilationRewriterProviders.Add(provider);
        }

        protected void AddPostcompilationRewriterProvider(IEnumerable<RewriterFactoryMethod> providers) {
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

        private void Process(ICompileContext context, IEnumerable<RewriterFactoryMethod> rewriterProviders) {
            foreach (var provider in rewriterProviders) {
                foreach (var syntaxTree in context.Compilation.SyntaxTrees) {
                    var model = context.Compilation.GetSemanticModel(syntaxTree);
                    var rewriter = provider(model, context);
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
