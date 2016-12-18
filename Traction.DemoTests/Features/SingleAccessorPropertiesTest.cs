using System;
using System.Collections.Generic;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    /// <summary>
    /// Test for contracts being applied uniformly on readonly, writeonly, and read/write properties.
    /// Correct property behavior for any attribute can be assumed 
    /// if that attribute can be applied to normal read/write properties, and these tests pass.
    /// </summary>
    [TestFixture]
    public class SingleAccessorPropertiesTest {

        private SingleAccessorPropertyDemo GetDemo() => new SingleAccessorPropertyDemo();

        [Test, TestCaseSource(nameof(AllCases))]
        public void Test(Action<SingleAccessorPropertyDemo> action, Type exceptionType) {
            var demo = GetDemo();
            CustomAssert.Throws(exceptionType, () => action(demo));
        }

        private static IEnumerable<TestCaseData> AllCases {
            get {
                Action<SingleAccessorPropertyDemo> action;

                action = demo => { var x = demo.NormalReadWrite; };
                yield return new TestCaseData(action, null)
                    .SetName($"SingleAccessor_NormalReadWrite_GetDoesNotThrow");

                action = demo => { demo.NormalReadWrite = null; };
                yield return new TestCaseData(action, null)
                    .SetName($"SingleAccessor_NormalReadWrite_SetDoesNotThrow");

                action = demo => {
                    demo._contractReadonlyProeprtyField = "test";
                    var x = demo.ContractReadonly;
                };
                yield return new TestCaseData(action, null)
                    .SetName($"SingleAccessor_ContractReadonly_DoesNotThrowIfContractMet");

                action = demo => { var x = demo.ContractReadonly; };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"SingleAccessor_ContractReadonly_ThrowsIfContractBroken");

                action = demo => { demo.ContractWriteonly = "test"; };
                yield return new TestCaseData(action, null)
                    .SetName($"SingleAccessor_ContractWriteonly_DoesNotThrowIfContractMet");

                action = demo => { demo.ContractWriteonly = null; };
                yield return new TestCaseData(action, typeof(PreconditionException))
                    .SetName($"SingleAccessor_ContractWriteonly_ThrowsIfContractBroken");

                action = demo => {
                    demo._contractReadWritePropertyField = "test";
                    var x = demo.ContractReadWrite;
                };
                yield return new TestCaseData(action, null)
                    .SetName($"SingleAccessor_ContractReadWrite_GetDoesNotThrowIfContractMet");

                action = demo => { var x = demo.ContractReadWrite; };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"SingleAccessor_ContractReadWrite_GetThrowsIfContractBroken");

                action = demo => { demo.ContractReadWrite = "test"; };
                yield return new TestCaseData(action, null)
                    .SetName($"SingleAccessor_ContractReadWrite_SetDoesNotThrowIfContractMet");

                action = demo => { demo.ContractReadWrite = null; };
                yield return new TestCaseData(action, typeof(PreconditionException))
                    .SetName($"SingleAccessor_ContractReadWrite_SetThrowsIfContractBroken");
            }
        }
    }
}
