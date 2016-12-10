using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Traction {

    /// <summary>
    /// Base class for syntax rewriters.
    /// </summary>
    abstract class RewriterBase : CSharpSyntaxRewriter {

        protected RewriterBase(SemanticModel model, ICompileContext context) {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (context == null) throw new ArgumentNullException(nameof(context));
     
            this.model = model;
            this.context = context;
        }

        protected readonly ICompileContext context;
        protected readonly SemanticModel model;

    }
}
