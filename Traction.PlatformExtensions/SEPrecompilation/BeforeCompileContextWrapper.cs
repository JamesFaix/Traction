using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using StackExchange.Precompilation;

namespace Traction.SEPrecompilation {

    /// <summary>
    /// Context data for pre-compilation stage.
    /// </summary>
    public class BeforeCompileContextWrapper : ICompileContext {

        public BeforeCompileContextWrapper(BeforeCompileContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context));
            this.context = context;
        }

        private readonly BeforeCompileContext context;

        public CSharpCompilation Compilation {
            get { return context.Compilation; }
            set { context.Compilation = value; }
        }

        public CSharpCommandLineArguments Arguments {
            get { return context.Arguments; }
            set { context.Arguments = value; }
        }

        public IList<Diagnostic> Diagnostics {
            get { return context.Diagnostics; }
            set { context.Diagnostics = value; }
        }
    }
}
