using System;
using System.Collections.Generic;
using StackExchange.Precompilation;

namespace Traction {

    /// <summary>
    /// Basic implementation of <c>ICompileModule</c> that can perform a series of rewriting operations.
    /// </summary>
    public abstract class CompileModuleBase : ICompileModule {

        protected CompileModuleBase() {
            beforeRewriterProviders = new List<RewriterFactoryMethod>();
            afterRewriterProviders = new List<RewriterFactoryMethod>();
        }

        private readonly List<RewriterFactoryMethod> beforeRewriterProviders;
        private readonly List<RewriterFactoryMethod> afterRewriterProviders;

        protected void AddBeforeRewriterProvider(RewriterFactoryMethod provider) {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            beforeRewriterProviders.Add(provider);
        }

        protected void AddAfterRewriterProvider(RewriterFactoryMethod provider) {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            afterRewriterProviders.Add(provider);
        }

        public void BeforeCompile(BeforeCompileContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context));
            Process(new BeforeCompileContextWrapper(context), beforeRewriterProviders);
        }

        public void AfterCompile(AfterCompileContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context));
            Process(new AfterCompileContextWrapper(context), afterRewriterProviders);
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
