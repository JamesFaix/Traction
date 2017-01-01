using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Traction.SEPrecompilation {

    public delegate CSharpSyntaxRewriter RewriterFactoryMethod(SemanticModel model, ICompileContext context);
}
