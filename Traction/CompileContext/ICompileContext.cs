using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Traction {

    public interface ICompileContext {

        CSharpCompilation Compilation { get; set; }
        CSharpCommandLineArguments Arguments { get; set; }
        IList<Diagnostic> Diagnostics { get; set; }
    }
}
