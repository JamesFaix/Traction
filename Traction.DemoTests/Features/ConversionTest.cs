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

        private ConversionConsumer GetConsumer() => new ConversionConsumer();

        [Test, TestCaseSource(nameof(AllCases))]
        public void Test(Action<ConversionConsumer> action, Type exceptionType) {
            var consumer = GetConsumer();
            CustomAssert.Throws(exceptionType, () => action(consumer));
        }

        private static IEnumerable<TestCaseData> AllCases {
            get {
                Action<ConversionConsumer> action;

                action = consumer => { var x = (int)consumer; };
                yield return new TestCaseData(action, null)
                    .SetName($"Conversion_Normal_DoesNotThrow");

                action = consumer => { var x = (long)consumer; };
                yield return new TestCaseData(action, null)
                    .SetName($"Conversion_Precondition_DoesNotThrowIfContractMet");

                action = consumer => { var x = (long)(null as ConversionConsumer); };
                yield return new TestCaseData(action, typeof(PreconditionException))
                    .SetName($"Conversion_Precondition_ThrowsIfContractBroken");

                action = consumer => { var x = (string)consumer; };
                yield return new TestCaseData(action, null)
                    .SetName($"Conversion_Postcondition_DoesNotThrowIfContractMet");

                action = consumer => { var x = (string)(null as ConversionConsumer); };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"Conversion_Postcondition_ThrowsIfContractBroken");

                action = consumer => {
                    consumer.testField = "test";
                    var x = (ArrayList)consumer;
                };
                yield return new TestCaseData(action, null)
                    .SetName($"Conversion_PreAndPostcondition_DoesNotThrowIfContractMet");

                action = consumer => { var x = (ArrayList)(null as ConversionConsumer); };
                yield return new TestCaseData(action, typeof(PreconditionException))
                    .SetName($"Conversion_PreAndPostcondition_ThrowsIfPreconditionBroken");

                action = consumer => { var x = (ArrayList)consumer; };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"Conversion_PreAndPostcondition_ThrowsIfPostconditionBroken");
            }
        }
    }
}
