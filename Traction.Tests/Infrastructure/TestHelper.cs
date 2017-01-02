using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using StackExchange.Precompilation;

namespace Traction.Tests {

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

        public static Assembly GetAssembly(string sourceCode) {
            if (sourceCode == null) throw new ArgumentNullException(nameof(sourceCode));

            var compilation = CompilationFactory.CompileClassFromText(sourceCode);

            var context = new BeforeCompileContext {
                Compilation = compilation,
                Diagnostics = new List<Diagnostic>()
            };

            var module = new TractionCompileModule();

            module.BeforeCompile(context);

            return GetAssembly(context.Compilation);
        }

        private static Assembly GetAssembly(CSharpCompilation compilation) {
            using (var stream = new MemoryStream()) {
                var result = compilation.Emit(stream);
                if (!result.Success) throw new Exception("Compilation failed.");
                stream.Seek(0, SeekOrigin.Begin);
                return Assembly.Load(stream.ToArray());
            }
        }
    }
}
