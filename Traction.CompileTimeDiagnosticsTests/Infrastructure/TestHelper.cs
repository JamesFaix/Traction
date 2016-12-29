using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using StackExchange.Precompilation;

namespace Traction.DiagnosticsTests {

    internal static class TestHelper {

        public static IEnumerable<Diagnostic> GetDiagnostics(CSharpCompilation compilation) {
            if (compilation == null) throw new ArgumentNullException(nameof(compilation));

            //Arrange
            var context = new BeforeCompileContext {
                Compilation = compilation,
                Diagnostics = new List<Diagnostic>()
            };
            var module = new TractionCompileModule();

            //Act
            module.BeforeCompile(context);

            //Return data to be used by assertions
            return context.Diagnostics;
        }
    }
}
