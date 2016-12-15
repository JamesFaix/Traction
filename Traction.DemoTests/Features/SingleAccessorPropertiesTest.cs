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

        #region NormalProperty
        [Test]
        public void NonNull_NormalProperty_GetDoesNotThrow() {
            var consumer = GetConsumer();
            consumer._normalPropertyField = null;

            Assert.DoesNotThrow(() => {
                var x = consumer.NormalReadWrite;
            });
        }

        [Test]
        public void NonNull_NormalProperty_SetDoesNotThrow() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.NormalReadWrite = null;
            });
        }
        #endregion

        #region ContractReadonlyProperty
        [Test]
        public void NonNull_ContractReadonlyProperty_DoesNotThrowIfContractMet() {
            var consumer = GetConsumer();
            consumer._contractReadonlyProeprtyField = "test";

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadonly;
            });
        }

        [Test]
        public void NonNull_ContractReadonlyProperty_ThrowsIfContractBroken() {
            var consumer = GetConsumer();
            consumer._contractReadonlyProeprtyField = null;

            Assert.Throws<PostconditionException>(() => {
                var x = consumer.ContractReadonly;
            });
        }
        #endregion

        #region ContractWriteonlyProperty
        [Test]
        public void NonNull_ContractWriteonlyProperty_DoesNotThrowIfContractMet() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractWriteonly = "test";
            });
        }

        [Test]
        public void NonNull_ContractWriteonlyProperty_ThrowsIfContractBroken() {
            var consumer = GetConsumer();

            Assert.Throws<PreconditionException>(() => {
                consumer.ContractWriteonly = null;
            });
        }
        #endregion

        #region ContractReadWriteProperty
        [Test]
        public void NonNull_ContractReadWriteProperty_GetDoesNotThrowIfContractMet() {
            var consumer = GetConsumer();
            consumer._contractReadWritePropertyField = "test";

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadWrite;
            });
        }

        [Test]
        public void NonNull_ContractReadWriteProperty_GetThrowsIfContractBroken() {
            var consumer = GetConsumer();
            consumer._contractReadWritePropertyField = null;

            Assert.Throws<PostconditionException>(() => {
                var x = consumer.ContractReadWrite;
            });
        }

        [Test]
        public void NonNull_ContractReadWriteProperty_SetDoesNotThrowIfContractMet() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractReadWrite = "test";
            });
        }

        [Test]
        public void NonNull_ContractReadWriteProperty_SetThrowsIfContractBroken() {
            var consumer = GetConsumer();

            Assert.Throws<PreconditionException>(() => {
                consumer.ContractReadWrite = null;
            });
        }
        #endregion
    }
}
