using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics.Contracts;
using NUnit.Framework;

namespace Traction.DiagnosticsTests {

    [TestFixture]
    public class PostconditionRestrictionTests {
        
        //Postconditions on iterator block

        [Test, TestCaseSource(nameof(VoidCases))]
        public DiagnosticSummary VoidPostconditionsTest(CompilationGenerator generator) {
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

        private static IEnumerable<TestCaseData> VoidCases {
            get {
                yield return VoidPostconditionTestCase("NonNull", 2); //2nd is for non-reference type
                yield return VoidPostconditionTestCase("Positive", 2); //2nd is for non-comparable type
                yield return VoidPostconditionTestCase("NonDefault", 1);
            }
        }

        private static TestCaseData VoidPostconditionTestCase(string attributeName, int diagnosticsCount) {
            return new TestCaseData((CompilationGenerator)(() =>
                     CompilationFactory.SingleClassCompilation(
                         MemberFactory.MethodWithPostcondition("void", attributeName))))
                 .Returns(new DiagnosticSummary {
                     Count = diagnosticsCount,
                     FirstTitle = "Traction: Invalid contract attribute usage"
                 })
                 .SetName($"PostconditionsCannotBePlacedOnMethodsReturningVoid_{attributeName}");
        }
    }
}
