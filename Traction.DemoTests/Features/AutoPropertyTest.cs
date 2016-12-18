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

        private AutoPropertyDemo GetDemo() => new AutoPropertyDemo();

        [Test, TestCaseSource(nameof(AllCases))]
        public void Test(Action<AutoPropertyDemo> action, Type exceptionType) {
            var demo = GetDemo();
            CustomAssert.Throws(exceptionType, () => action(demo));
        }

        private static IEnumerable<TestCaseData> AllCases {
            get {
                Action<AutoPropertyDemo> action;

                action = demo => { var x = demo.NormalReadonly; };
                yield return new TestCaseData(action, null)
                    .SetName($"AutoProperty_NormalReadonly_DoesNotThrow");

                action = demo => { var x = demo.ContractReadonly; };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"AutoProperty_ContractReadonly_ThrowsIfContractBroken");
                
                action = demo => { var x = demo.NormalReadWrite; };
                yield return new TestCaseData(action, null)
                    .SetName($"AutoProperty_NormalReadWrite_GetDoesNotThrow");

                action = demo => { demo.NormalReadWrite = null; };
                yield return new TestCaseData(action, null)
                    .SetName($"AutoProperty_NormalReadWrite_SetDoesNotThrow");

                action = demo => { var x = demo.ContractReadWrite; };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"AutoProperty_ContractReadWrite_GetThrowsIfContractBroken");

                action = demo => { demo.ContractReadWrite = null; };
                yield return new TestCaseData(action, typeof(PreconditionException))
                    .SetName($"AutoProperty_ContractReadWrite_SetThrowsIfContractBroken");
            }
        }    
    }
}
