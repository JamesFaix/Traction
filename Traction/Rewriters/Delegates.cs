using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Traction {
    
    public delegate CSharpSyntaxRewriter RewriterFactoryMethod(SemanticModel model, ICompileContext context);
}
