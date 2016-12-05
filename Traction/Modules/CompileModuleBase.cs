using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using StackExchange.Precompilation;

namespace Traction {

    public delegate CSharpSyntaxRewriter RewriterGenerator(SemanticModel model);

    public abstract class CompileModuleBase : ICompileModule {

        protected CompileModuleBase() {
            beforeGenerators = new List<RewriterGenerator>();
            afterGenerators = new List<RewriterGenerator>();
        }

        private readonly List<RewriterGenerator> beforeGenerators;
        private readonly List<RewriterGenerator> afterGenerators;

        protected void AddPrecompileGenerator(RewriterGenerator generator) {
            if (generator == null) throw new ArgumentNullException(nameof(generator));
            beforeGenerators.Add(generator);
        }

        protected void AddPostcompileGenerator(RewriterGenerator generator) {
            if (generator == null) throw new ArgumentNullException(nameof(generator));
            afterGenerators.Add(generator);
        }

        public void BeforeCompile(BeforeCompileContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context));

            foreach (var generator in beforeGenerators) {
                foreach (var syntaxTree in context.Compilation.SyntaxTrees) {
                    var model = context.Compilation.GetSemanticModel(syntaxTree);
                    var rewriter = generator(model);
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

        public void AfterCompile(AfterCompileContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context));

            foreach (var generator in afterGenerators) {
                foreach (var syntaxTree in context.Compilation.SyntaxTrees) {
                    var model = context.Compilation.GetSemanticModel(syntaxTree);
                    var rewriter = generator(model);
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
