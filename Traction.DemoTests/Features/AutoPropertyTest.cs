using System;
using System.Collections.Generic;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    /// <summary>
    /// Test for contracts being applied to auto-properties.
    /// Correct auto-property behavior for any attribute can be assumed 
    /// if that attribute can be applied to normal properties, and these tests pass.
    /// </summary>
    [TestFixture]
    public class AutoPropertyTest {

        private AutoPropertyConsumer GetConsumer() => new AutoPropertyConsumer();

        [Test, TestCaseSource(nameof(AllCases))]
        public void Test(Action<AutoPropertyConsumer> action, Type exceptionType) {
            var consumer = GetConsumer();
            CustomAssert.Throws(exceptionType, () => action(consumer));
        }

        private static IEnumerable<TestCaseData> AllCases {
            get {
                Action<AutoPropertyConsumer> action;

                action = consumer => { var x = consumer.NormalReadonly; };
                yield return new TestCaseData(action, null)
                    .SetName($"AutoProperty_NormalReadonly_DoesNotThrow");

                action = consumer => { var x = consumer.ContractReadonly; };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"AutoProperty_ContractReadonly_ThrowsIfContractBroken");
                
                action = consumer => { var x = consumer.NormalReadWrite; };
                yield return new TestCaseData(action, null)
                    .SetName($"AutoProperty_NormalReadWrite_GetDoesNotThrow");

                action = consumer => { consumer.NormalReadWrite = null; };
                yield return new TestCaseData(action, null)
                    .SetName($"AutoProperty_NormalReadWrite_SetDoesNotThrow");

                action = consumer => { var x = consumer.ContractReadWrite; };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"AutoProperty_ContractReadWrite_GetThrowsIfContractBroken");

                action = consumer => { consumer.ContractReadWrite = null; };
                yield return new TestCaseData(action, typeof(PreconditionException))
                    .SetName($"AutoProperty_ContractReadWrite_SetThrowsIfContractBroken");
            }
        }    
    }
}
