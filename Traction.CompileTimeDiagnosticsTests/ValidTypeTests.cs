using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

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
                FirstTitle = context.Diagnostics.FirstOrDefault()
                                    ?.Descriptor.Title.ToString()
            };
        }

        private static TestCaseData PreconditionCase(string typeName, string attributeName, bool shouldHaveDiagnostic) {
            return new TestCaseData((CompilationGenerator)(() =>
                    CompilationFactory.SingleClassCompilation(
                        MemberFactory.MethodWithPrecondition(typeName, attributeName))))
                .Returns(new DiagnosticSummary {
                    Count = shouldHaveDiagnostic ? 1 : 0,
                    FirstTitle = shouldHaveDiagnostic ? "Traction: Invalid contract attribute usage" : null
                })
                .SetName($"{attributeName}_CanOnlyBePlacedOnParametersWithValidTypes" +
                    $"_{typeName}{(shouldHaveDiagnostic ? "Fails" : "Passes")}");
        }

        private static TestCaseData PostconditionCase(string typeName, string attributeName, bool shouldHaveDiagnostic) {
            return new TestCaseData((CompilationGenerator)(() =>
                     CompilationFactory.SingleClassCompilation(
                         MemberFactory.MethodWithPostcondition(typeName, attributeName))))
                 .Returns(new DiagnosticSummary {
                     Count = shouldHaveDiagnostic ? 1 : 0,
                     FirstTitle = shouldHaveDiagnostic ? "Traction: Invalid contract attribute usage" : null
                 })
                 .SetName($"{attributeName}_CanOnlyBePlacedOnReturnValuesWithValidTypes" +
                     $"_{typeName}{(shouldHaveDiagnostic ? "Fails" : "Passes")}");
        }

        private static IEnumerable<TestCaseData> AllCases =>
            NonNullCases
            .Concat(NonDefaultCases)
            .Concat(NonEmptyCases)
            .Concat(PositiveCases);

        private static IEnumerable<TestCaseData> NonNullCases {
            get {
                var name = "NonNull";

                //Reference types OK
                yield return PreconditionCase("string", name, false);
                yield return PostconditionCase("string", name, false);

                yield return PreconditionCase("object", name, false);
                yield return PostconditionCase("object", name, false);

                yield return PreconditionCase("IDisposable", name, false);
                yield return PostconditionCase("IDisposable", name, false);

                yield return PreconditionCase("Action", name, false);
                yield return PostconditionCase("Action", name, false);

                //Value types (even nullable) create diagnostic
                yield return PreconditionCase("int", name, true);
                yield return PostconditionCase("int", name, true);

                yield return PreconditionCase("int?", name, true);
                yield return PostconditionCase("int?", name, true);

                yield return PreconditionCase("DateTime", name, true);
                yield return PostconditionCase("DateTime", name, true);
            }
        }

        private static IEnumerable<TestCaseData> NonDefaultCases {
            get {
                var name = "NonDefault";

                //All types OK
                yield return PreconditionCase("string", name, false);
                yield return PostconditionCase("string", name, false);

                yield return PreconditionCase("object", name, false);
                yield return PostconditionCase("object", name, false);

                yield return PreconditionCase("IDisposable", name, false);
                yield return PostconditionCase("IDisposable", name, false);

                yield return PreconditionCase("Action", name, false);
                yield return PostconditionCase("Action", name, false);

                yield return PreconditionCase("int", name, false);
                yield return PostconditionCase("int", name, false);

                yield return PreconditionCase("int?", name, false);
                yield return PostconditionCase("int?", name, false);

                yield return PreconditionCase("DateTime", name, false);
                yield return PostconditionCase("DateTime", name, false);
            }
        }

        private static IEnumerable<TestCaseData> NonEmptyCases {
            get {
                var name = "NonEmpty";

                //IEnumerable<T> types OK
                yield return PreconditionCase("string", name, false);
                yield return PostconditionCase("string", name, false);

                yield return PreconditionCase("int[]", name, false);
                yield return PostconditionCase("int[]", name, false);

                yield return PreconditionCase("Dictionary<int, string>", name, false);
                yield return PostconditionCase("Dictionary<int, string>", name, false);

                //Non-generic enumerable types not OK
                yield return PreconditionCase("ArrayList", name, true);
                yield return PostconditionCase("ArrayList", name, true);
                
                //Non-enumerable types not OK
                yield return PreconditionCase("object", name, true);
                yield return PostconditionCase("object", name, true);

                yield return PreconditionCase("IDisposable", name, true);
                yield return PostconditionCase("IDisposable", name, true);

                yield return PreconditionCase("Action", name, true);
                yield return PostconditionCase("Action", name, true);

                yield return PreconditionCase("int", name, true);
                yield return PostconditionCase("int", name, true);

                yield return PreconditionCase("int?", name, true);
                yield return PostconditionCase("int?", name, true);

                yield return PreconditionCase("DateTime", name, true);
                yield return PostconditionCase("DateTime", name, true);
            }
        }

        //No extra tests for Negative, NonPositive and NonNegative because the type filtering is done by their base class
        private static IEnumerable<TestCaseData> PositiveCases {
            get {
                var name = "Positive";

                //Value types (even nullable) OK
                yield return PreconditionCase("int", name, false);
                yield return PostconditionCase("int", name, false);

                yield return PreconditionCase("int?", name, false);
                yield return PostconditionCase("int?", name, false);

                yield return PreconditionCase("DateTime", name, false);
                yield return PostconditionCase("DateTime", name, false);

                //Reference types not OK
                yield return PreconditionCase("string", name, true);
                yield return PostconditionCase("string", name, true);

                yield return PreconditionCase("object", name, true);
                yield return PostconditionCase("object", name, true);

                yield return PreconditionCase("IDisposable", name, true);
                yield return PostconditionCase("IDisposable", name, true);

                yield return PreconditionCase("Action", name, true);
                yield return PostconditionCase("Action", name, true);

            }
        }

    }
}
