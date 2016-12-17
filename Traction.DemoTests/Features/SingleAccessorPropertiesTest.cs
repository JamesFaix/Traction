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

        private SingleAccessorPropertyConsumer GetConsumer() => new SingleAccessorPropertyConsumer();

        [Test, TestCaseSource(nameof(AllCases))]
        public void Test(Action<SingleAccessorPropertyConsumer> action, Type exceptionType) {
            var consumer = GetConsumer();
            CustomAssert.Throws(exceptionType, () => action(consumer));
        }

        private static IEnumerable<TestCaseData> AllCases {
            get {
                Action<SingleAccessorPropertyConsumer> action;

                action = consumer => { var x = consumer.NormalReadWrite; };
                yield return new TestCaseData(action, null)
                    .SetName($"SingleAccessor_NormalReadWrite_GetDoesNotThrow");

                action = consumer => { consumer.NormalReadWrite = null; };
                yield return new TestCaseData(action, null)
                    .SetName($"SingleAccessor_NormalReadWrite_SetDoesNotThrow");

                action = consumer => {
                    consumer._contractReadonlyProeprtyField = "test";
                    var x = consumer.ContractReadonly;
                };
                yield return new TestCaseData(action, null)
                    .SetName($"SingleAccessor_ContractReadonly_DoesNotThrowIfContractMet");

                action = consumer => { var x = consumer.ContractReadonly; };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"SingleAccessor_ContractReadonly_ThrowsIfContractBroken");

                action = consumer => { consumer.ContractWriteonly = "test"; };
                yield return new TestCaseData(action, null)
                    .SetName($"SingleAccessor_ContractWriteonly_DoesNotThrowIfContractMet");

                action = consumer => { consumer.ContractWriteonly = null; };
                yield return new TestCaseData(action, typeof(PreconditionException))
                    .SetName($"SingleAccessor_ContractWriteonly_ThrowsIfContractBroken");

                action = consumer => {
                    consumer._contractReadWritePropertyField = "test";
                    var x = consumer.ContractReadWrite;
                };
                yield return new TestCaseData(action, null)
                    .SetName($"SingleAccessor_ContractReadWrite_GetDoesNotThrowIfContractMet");

                action = consumer => { var x = consumer.ContractReadWrite; };
                yield return new TestCaseData(action, typeof(PostconditionException))
                    .SetName($"SingleAccessor_ContractReadWrite_GetThrowsIfContractBroken");

                action = consumer => { consumer.ContractReadWrite = "test"; };
                yield return new TestCaseData(action, null)
                    .SetName($"SingleAccessor_ContractReadWrite_SetDoesNotThrowIfContractMet");

                action = consumer => { consumer.ContractReadWrite = null; };
                yield return new TestCaseData(action, typeof(PreconditionException))
                    .SetName($"SingleAccessor_ContractReadWrite_SetThrowsIfContractBroken");
            }
        }
    }
}
