using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using StackExchange.Precompilation;

namespace Traction.DiagnosticsTests {

    static class ContextFactory {

        public static BeforeCompileContext Before(CompilationGenerator generator)
            => new BeforeCompileContext {
                Compilation = generator(),
                Diagnostics = new List<Diagnostic>()
            };
    }
}
