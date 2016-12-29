using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Traction.SEPrecompilation;
using Traction.Roslyn;

namespace Traction {

    /// <summary>
    /// Base class for syntax rewriters.
    /// </summary>
    internal abstract class RewriterBase : CSharpSyntaxRewriter {

        protected RewriterBase(SemanticModel model, ICompileContext context, string confirmationMessage) {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (confirmationMessage == null) throw new ArgumentNullException(nameof(confirmationMessage));

            this.model = model;
            this.context = context;
            this.nodeRewriter = new NodeRewriter(model, context, confirmationMessage);
        }

        protected readonly ICompileContext context;
        protected readonly SemanticModel model;
        protected readonly NodeRewriter nodeRewriter;
    }
}
