using System;
using System.Collections.Generic;

namespace Traction.SEPrecompilation {

    public class RewriterChain {

        public RewriterChain(params RewriterFactoryMethod[] providers) {
            this.providers = new List<RewriterFactoryMethod>();
            this.providers.AddRange(providers);
        }

        private readonly List<RewriterFactoryMethod> providers;

        public void Add(RewriterFactoryMethod provider) {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            this.providers.Add(provider);
        }

        public void AddRange(IEnumerable<RewriterFactoryMethod> providers) {
            if (providers == null) throw new ArgumentNullException(nameof(providers));
            foreach (var p in providers) {
                if (p == null) throw new ArgumentException("Sequence cannot contain null elements.");
                this.providers.Add(p);
            }
        }

        public void Process(ICompileContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context));

            foreach (var provider in this.providers) {
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
