using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using static Traction.DiagnosticsTests.TestCaseFactory;

namespace Traction.DiagnosticsTests {

    [TestFixture]
    public class ValidTypeTests {

        [Test, TestCaseSource(nameof(AllCases))]
        public DiagnosticSummary ValidTypeTest(CompilationGenerator generator) {
            //Arrange
            var context = ContextFactory.Before(generator);
            var module = new ContractModule();

            //Act
            module.BeforeCompile(context);

            //Assert
            return new DiagnosticSummary {
                Count = context.Diagnostics.Count(),
                FirstMessage = context.Diagnostics.FirstOrDefault()
                                    ?.Descriptor.Title.ToString()
            };
        }

        private static IEnumerable<TestCaseData> AllCases =>
            NonNullCases
            .Concat(NonDefaultCases);

        private static IEnumerable<TestCaseData> NonNullCases {
            get {
                var name = "NonNull";
                yield return CreatePreconditionInvalidTypeTestCase("string", name, false);
                yield return CreatePostconditionInvalidTypeTestCase("string", name, false);

                yield return CreatePreconditionInvalidTypeTestCase("object", name, false);
                yield return CreatePostconditionInvalidTypeTestCase("object", name, false);

                yield return CreatePreconditionInvalidTypeTestCase("IDisposable", name, false);
                yield return CreatePostconditionInvalidTypeTestCase("IDisposable", name, false);

                yield return CreatePreconditionInvalidTypeTestCase("int", name, true);
                yield return CreatePostconditionInvalidTypeTestCase("int", name, true);

                yield return CreatePreconditionInvalidTypeTestCase("int?", name, true);
                yield return CreatePostconditionInvalidTypeTestCase("int?", name, true);

                yield return CreatePreconditionInvalidTypeTestCase("DateTime", name, true);
                yield return CreatePostconditionInvalidTypeTestCase("DateTime", name, true);
            }
        }

        private static IEnumerable<TestCaseData> NonDefaultCases {
            get {
                var name = "NonDefault";
                yield return CreatePreconditionInvalidTypeTestCase("string", name, false);
                yield return CreatePostconditionInvalidTypeTestCase("string", name, false);

                yield return CreatePreconditionInvalidTypeTestCase("object", name, false);
                yield return CreatePostconditionInvalidTypeTestCase("object", name, false);

                yield return CreatePreconditionInvalidTypeTestCase("IDisposable", name, false);
                yield return CreatePostconditionInvalidTypeTestCase("IDisposable", name, false);

                yield return CreatePreconditionInvalidTypeTestCase("Action", name, false);
                yield return CreatePostconditionInvalidTypeTestCase("Action", name, false);

                yield return CreatePreconditionInvalidTypeTestCase("int", name, false);
                yield return CreatePostconditionInvalidTypeTestCase("int", name, false);

                yield return CreatePreconditionInvalidTypeTestCase("int?", name, false);
                yield return CreatePostconditionInvalidTypeTestCase("int?", name, false);

                yield return CreatePreconditionInvalidTypeTestCase("DateTime", name, false);
                yield return CreatePostconditionInvalidTypeTestCase("DateTime", name, false);
            }
        }
    }
}
