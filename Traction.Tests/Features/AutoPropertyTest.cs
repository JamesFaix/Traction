﻿using System;
using Traction.TestConsumer;
using NUnit.Framework;

namespace Traction.Tests {
    
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

            Assert.Throws<ReturnValueException>(() => {
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

            Assert.Throws<ReturnValueException>(() => {
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

            Assert.Throws<ArgumentNullException>(() => {
                consumer.ContractReadWrite = null;
            });
        }
        #endregion        
    }
}
