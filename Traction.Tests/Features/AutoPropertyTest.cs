using System;
using Traction.Demo;
using NUnit.Framework;

namespace Traction.Tests {

    /// <summary>
    /// Test for contracts being applied to auto-properties.
    /// Correct auto-property behavior for any attribute can be assumed 
    /// if that attribute can be applied to normal properties, and these tests pass.
    /// </summary>
    [TestFixture]
    public class AutoPropertyTest {

        private AutoPropertyConsumer GetConsumer() => new AutoPropertyConsumer();

        #region NormalReadonly
        [Test]
        public void AutoProperty_NormalReadonly_DoesNotThrow() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                var x = consumer.NormalReadonly;
            });
        }
        #endregion

        #region ContractReadonly
        [Test]
        public void AutoProperty_ContractReadonly_ThrowsIfContractBroken() {
            var consumer = GetConsumer();

            Assert.Throws<PostconditionException>(() => {
                var x = consumer.ContractReadonly;
            });
        }
        #endregion

        #region NormalReadWrite
        [Test]
        public void AutoProperty_NormalReadWrite_GetDoesNotThrow() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                var x = consumer.NormalReadWrite;
            });
        }

        [Test]
        public void AutoProperty_NormalReadWrite_SetDoesNotThrow() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.NormalReadWrite = null;
            });
        }
        #endregion

        #region ContractReadWrite
        [Test]
        public void AutoProperty_ContractReadWrite_GetDoesNotThrowIfContractMet() {
            var consumer = GetConsumer();
            consumer.ContractReadWrite = "test";

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadWrite;
            });
        }

        [Test]
        public void AutoProperty_ContractReadWrite_GetThrowsIfContractBroken() {
            var consumer = GetConsumer();

            Assert.Throws<PostconditionException>(() => {
                var x = consumer.ContractReadWrite;
            });
        }

        [Test]
        public void AutoProperty_ContractReadWrite_SetDoesNotThrowIfContractMet() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractReadWrite = "test";
            });
        }

        [Test]
        public void AutoProperty_ContractReadWrite_SetThrowsIfContractBroken() {
            var consumer = GetConsumer();

            Assert.Throws<PreconditionException>(() => {
                consumer.ContractReadWrite = null;
            });
        }
        #endregion        
    }
}
