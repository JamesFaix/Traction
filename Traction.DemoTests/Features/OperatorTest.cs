﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    /// <summary>
    /// Tests for contracts being applied to operators.
    /// The implementation for operators is 99% the same as methods.
    /// Correct operator behavior for any attribute can be assumed 
    /// if that attribute can be applied to normal methods, and these tests pass.
    /// </summary>
    [TestFixture]
    public class OperatorTest {

        private OperatorDemo GetDemo() => new OperatorDemo();

        [Test, TestCaseSource(nameof(AllCases))]
        public void Test(Action<OperatorDemo, OperatorDemo> action, Type exceptionType) {
            var consumer1 = GetDemo();
            var consumer2 = GetDemo();
            CustomAssert.Throws(exceptionType, () => action(consumer1, consumer2));
        }

        private static IEnumerable<TestCaseData> AllCases {
            get {
                Action<OperatorDemo, OperatorDemo> action;

                action = (consumer1, consumer2) => { var x = consumer1 + consumer2; };
                yield return new TestCaseData(action, null)
                    .SetName($"Operator_Normal_DoesNotThrow");

                action = (consumer1, consumer2) => { var x = consumer1 - consumer2; };
                yield return new TestCaseData(action, null)
                    .SetName($"Operator_Precondition_DoesNotThrowIfContractMet");

                action = (consumer1, consumer2) => { var x = (null as OperatorDemo) - consumer2; };
                yield return new TestCaseData(action, typeof(PreconditionException))
                    .SetName($"Operator_Precondition_ThrowsIfContractBroken");

                action = (consumer1, consumer2) => { var x = consumer1 * consumer2; };
                yield return new TestCaseData(action, null)
                    .SetName($"Operator_Postcondition_DoesNotThrowIfContractMet");

                action = (consumer1, consumer2) => { var x = consumer1 * (null as OperatorDemo); };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"Operator_Postcondition_ThrowsIfContractBroken");

                action = (consumer1, consumer2) => { var x = consumer1 / consumer2; };
                yield return new TestCaseData(action, null)
                    .SetName($"Operator_PreAndPost_DoesNotThrowContractMet");

                action = (consumer1, consumer2) => { var x = (null as OperatorDemo) / consumer2; };
                yield return new TestCaseData(action, typeof(PreconditionException))
                    .SetName($"Operator_PreAndPost_ThrowsIfPreconditionBroken");

                action = (consumer1, consumer2) => { var x = consumer1 / consumer1; };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"Operator_PreAndPost_ThrowsIfPostconditionBroken");
            }
        }
    }
}
