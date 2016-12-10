using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Traction {
    
    /// <summary>
    /// Method that produces a syntax rewriter that uses the given context.
    /// </summary>
    public delegate CSharpSyntaxRewriter RewriterFactoryMethod(SemanticModel model, ICompileContext context);
}
