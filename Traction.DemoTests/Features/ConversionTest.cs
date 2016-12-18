using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    /// <summary>
    /// Tests for contracts being applied to conversion operators.
    /// The implementation for conversions is 99% the same as methods.
    /// Correct conversion operator behavior for any attribute can be assumed 
    /// if that attribute can be applied to normal methods, and these tests pass.
    /// </summary>
    [TestFixture]
    public class ConversionTest {

        private ConversionDemo GetDemo() => new ConversionDemo();

        [Test, TestCaseSource(nameof(AllCases))]
        public void Test(Action<ConversionDemo> action, Type exceptionType) {
            var demo = GetDemo();
            CustomAssert.Throws(exceptionType, () => action(demo));
        }

        private static IEnumerable<TestCaseData> AllCases {
            get {
                Action<ConversionDemo> action;

                action = demo => { var x = (int)demo; };
                yield return new TestCaseData(action, null)
                    .SetName($"Conversion_Normal_DoesNotThrow");

                action = demo => { var x = (long)demo; };
                yield return new TestCaseData(action, null)
                    .SetName($"Conversion_Precondition_DoesNotThrowIfContractMet");

                action = demo => { var x = (long)(null as ConversionDemo); };
                yield return new TestCaseData(action, typeof(PreconditionException))
                    .SetName($"Conversion_Precondition_ThrowsIfContractBroken");

                action = demo => { var x = (string)demo; };
                yield return new TestCaseData(action, null)
                    .SetName($"Conversion_Postcondition_DoesNotThrowIfContractMet");

                action = demo => { var x = (string)(null as ConversionDemo); };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"Conversion_Postcondition_ThrowsIfContractBroken");

                action = demo => {
                    demo.testField = "test";
                    var x = (ArrayList)demo;
                };
                yield return new TestCaseData(action, null)
                    .SetName($"Conversion_PreAndPostcondition_DoesNotThrowIfContractMet");

                action = demo => { var x = (ArrayList)(null as ConversionDemo); };
                yield return new TestCaseData(action, typeof(PreconditionException))
                    .SetName($"Conversion_PreAndPostcondition_ThrowsIfPreconditionBroken");

                action = demo => { var x = (ArrayList)demo; };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"Conversion_PreAndPostcondition_ThrowsIfPostconditionBroken");
            }
        }
    }
}
