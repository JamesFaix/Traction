using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using StackExchange.Precompilation;

namespace Traction.DiagnosticsTests {

    [TestFixture]
    public class DiagnosticSummaryTests {

        [Test, TestCaseSource(nameof(AllCases))]
        public DiagnosticSummary DiagnosticSummaryTest(CSharpCompilation compilation) {
            if (compilation == null) throw new ArgumentNullException(nameof(compilation));

            //Arrange
            var context = new BeforeCompileContext {
                Compilation = compilation,
                Diagnostics = new List<Diagnostic>()
            };
            var module = new ContractModule();

            //Act
            module.BeforeCompile(context);

            //Assert
            return new DiagnosticSummary {
                Count = context.Diagnostics.Count(),
                FirstTitle = context.Diagnostics.FirstOrDefault()
                                    ?.Descriptor.Title.ToString()
            };
        }

        private static IEnumerable<TestCaseData> AllCases =>
            ValidTypeTestCases.AllCases
            .Concat(PostconditionVoidTestCases.AllCases)
            .Concat(IteratorBlockPostconditionTestCases.AllCases);
    }
}
