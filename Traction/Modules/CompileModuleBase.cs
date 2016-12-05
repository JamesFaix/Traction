using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using StackExchange.Precompilation;

namespace Traction {
    
    public abstract class CompileModuleBase : ICompileModule {

        protected CompileModuleBase() {
            beforeRewriterProviders = new List<IRewriterProvider>();
            afterRewriterProviders = new List<IRewriterProvider>();
        }

        private readonly List<IRewriterProvider> beforeRewriterProviders;
        private readonly List<IRewriterProvider> afterRewriterProviders;

        protected void AddBeforeRewriterProvider(IRewriterProvider provider) {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            beforeRewriterProviders.Add(provider);
        }

        protected void AddAfterRewriterProvider(IRewriterProvider provider) {
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

        private void Process(ICompileContext context, IEnumerable<IRewriterProvider> rewriterProviders) {
            foreach (var provider in rewriterProviders) {
                foreach (var syntaxTree in context.Compilation.SyntaxTrees) {
                    var model = context.Compilation.GetSemanticModel(syntaxTree);
                    var rewriter = provider.CreateRewriter(model, context);
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
