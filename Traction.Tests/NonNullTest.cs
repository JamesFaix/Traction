using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traction.Tests {

    [TestFixture]
    public class NonNullTest {

        #region Properties

        #region Normal Property
        [Test]
        public void NonNull_NormalProperty_GetDoesNotThrow() {
            var consumer = new NonNullConsumer();
            consumer._normalProperty = null;

            Assert.DoesNotThrow(() => {
                var x = consumer.NormalProperty;
            });
        }

        [Test]
        public void NonNull_NormalProperty_SetDoesNotThrow() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() => {
                consumer.NormalProperty = null;
            });
        }
        #endregion

        #region ReadonlyPropertyWithContract
        [Test]
        public void NonNull_ReadonlyPropertyWithContract_DoesNotThrowIfNotNull() {
            var consumer = new NonNullConsumer();
            consumer._readonlyPropertyWithContract = "test";

            Assert.DoesNotThrow(() => {
                var x = consumer.ReadonlyPropertyWithContract;
            });
        }

        [Test]
        public void NonNull_ReadonlyPropertyWithContract_ThrowsIfNull() {
            var consumer = new NonNullConsumer();
            consumer._readonlyPropertyWithContract = null;

            Assert.Throws<ReturnValueException>(() => {
                var x = consumer.ReadonlyPropertyWithContract;
            });
        }
        #endregion

        #region WriteonlyPropertyWithContract
        [Test]
        public void NonNull_WriteonlyPropertyWithContract_DoesNotThrowIfNotNull() {
            var consumer = new NonNullConsumer();
            
            Assert.DoesNotThrow(() => {
                consumer.WriteonlyPropertyWithContract = "test";
            });
        }

        [Test]
        public void NonNull_WriteonlyPropertyWithContract_ThrowsIfNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<ArgumentNullException>(() => {
                consumer.WriteonlyPropertyWithContract = null;
            });
        }
        #endregion

        #region ReadWritePropertyWithContract
        [Test]
        public void NonNull_ReadWritePropertyWithContract_GetDoesNotThrowIfNotNull() {
            var consumer = new NonNullConsumer();
            consumer._readWritePropertyWithContract = "test";

            Assert.DoesNotThrow(() => {
                var x = consumer.ReadWritePropertyWithContract;
            });
        }

        [Test]
        public void NonNull_ReadWritePropertyWithContract_GetThrowsIfNull() {
            var consumer = new NonNullConsumer();
            consumer._readWritePropertyWithContract = null;

            Assert.Throws<ReturnValueException>(() => {
                var x = consumer.ReadWritePropertyWithContract;
            });
        }

        [Test]
        public void NonNull_ReadWritePropertyWithContract_SetDoesNotThrowIfNotNull() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ReadWritePropertyWithContract = "test";
            });
        }

        [Test]
        public void NonNull_ReadWritePropertyWithContract_SetThrowsIfNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<ArgumentNullException>(() => {
                consumer.ReadWritePropertyWithContract = null;
            });
        }
        #endregion

        #region ReadonlyPropertyWithContractAndMultipleReturns
        [Test]
        public void NonNull_ReadonlyPropertyWithContractAndMultipleReturns_DoesNotThrowIfNotNull() {
            var consumer = new NonNullConsumer();
            //This property internally toggles between returning two fields from two return statements.
            consumer._readonlyPropertyWithContractAndMultipleReturns1 = "test1";
            consumer._readonlyPropertyWithContractAndMultipleReturns2 = "test2";
            
            Assert.DoesNotThrow(() => {
                //Getting value twice will hit check return statements.
                var x = consumer.ReadonlyPropertyWithContractAndMultipleReturns;
                var y = consumer.ReadonlyPropertyWithContractAndMultipleReturns;
                Assert.AreNotEqual(x, y);
            });
        }
        
        [Test]
        public void NonNull_ReadonlyPropertyWithContractAndMultipleReturns_ThrowsIfNull() {
            var consumer = new NonNullConsumer();
            //This property internally toggles between returning two fields from two return statements.
            consumer._readonlyPropertyWithContractAndMultipleReturns1 = null;
            consumer._readonlyPropertyWithContractAndMultipleReturns2 = null;

            //Getting value twice will hit check return statements.
            for (var i = 0; i <= 1; i++) {
                Assert.Throws<ReturnValueException>(() => {
                    var x = consumer.ReadonlyPropertyWithContractAndMultipleReturns;
                });
            }
        }

        #endregion
        #endregion
    }
}
