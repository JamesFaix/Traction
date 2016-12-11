using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.Tests {

    [TestFixture]
    public class NonDefaultValueTypeTest {

        private NonDefaultConsumer GetConsumer() => new NonDefaultConsumer();

        #region Properties

        #region NormalProperty
        [Test]
        public void NonDefault_NormalValueTypeProperty_GetDoesNotThrow() {
            var consumer = GetConsumer();
            consumer._normalValueTypeProperty = 0;

            Assert.DoesNotThrow(() => {
                var x = consumer.NormalReadWriteValueTypeProperty;
            });
        }

        [Test]
        public void NonDefault_NormalValueTypeProperty_SetDoesNotThrow() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.NormalReadWriteValueTypeProperty = 0;
            });
        }
        #endregion

        #region ContractReadonlyProperty
        [Test]
        public void NonDefault_ContractReadonlyValueTypeProperty_DoesNotThrowIfDefault() {
            var consumer = GetConsumer();
            consumer._contractReadonlyValueTypeProperty = 1;

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadonlyValueTypeProperty;
            });
        }

        [Test]
        public void NonDefault_ContractReadonlyValueTypeProperty_ThrowsIfDefault() {
            var consumer = GetConsumer();
            consumer._contractReadonlyValueTypeProperty = 0;

            Assert.Throws<ReturnValueException>(() => {
                var x = consumer.ContractReadonlyValueTypeProperty;
            });
        }
        #endregion

        #region ContractWriteonlyProperty
        [Test]
        public void NonDefault_ContractWriteonlyValueTypeProperty_DoesNotThrowIfNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractWriteonlyValueTypeProperty = 1;
            });
        }

        [Test]
        public void NonDefault_ContractWriteonlyValueTypeProperty_ThrowsIfDefault() {
            var consumer = GetConsumer();

            Assert.Throws<ArgumentException>(() => {
                consumer.ContractWriteonlyValueTypeProperty = 0;
            });
        }
        #endregion

        #region ContractReadWriteProperty
        [Test]
        public void NonDefault_ContractReadWriteValueTypeProperty_GetDoesNotThrowIfNotDefault() {
            var consumer = GetConsumer();
            consumer._contractReadWriteValueTypeProperty = 1;

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadWriteValueTypeProperty;
            });
        }

        [Test]
        public void NonDefault_ContractReadWriteValueTypeProperty_GetThrowsIfDefault() {
            var consumer = GetConsumer();
            consumer._contractReadWriteValueTypeProperty = 0;

            Assert.Throws<ReturnValueException>(() => {
                var x = consumer.ContractReadWriteValueTypeProperty;
            });
        }

        [Test]
        public void NonDefault_ContractReadWriteValueTypeProperty_SetDoesNotThrowIfNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractReadWriteValueTypeProperty = 1;
            });
        }

        [Test]
        public void NonDefault_ContractReadWriteValueTypeProperty_SetThrowsIfDefault() {
            var consumer = GetConsumer();

            Assert.Throws<ArgumentException>(() => {
                consumer.ContractReadWriteValueTypeProperty = 0;
            });
        }
        #endregion

        #endregion

        #region Methods

        #region NormalMethod
        [Test]
        public void NonDefault_NormalMethod_DoesNotThrow() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.NormalMethod());
        }
        #endregion

        #region PreconditionMethod
        [Test]
        public void NonDefault_PreconditionValueTypeMethod_DoesNotThrowIfArgNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionValueTypeMethod(1));
        }

        [Test]
        public void NonDefault_PreconditionValueTypeMethod_ThrowsIfArgDefault() {
            var consumer = GetConsumer();

            Assert.Throws<ArgumentException>(() =>
                consumer.PreconditionValueTypeMethod(0));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void NonDefault_PostconditionValueTypeMethod_DoesNotThrowIfResultNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionValueTypeMethod(1));
        }

        [Test]
        public void NonDefault_PostconditionValueTypeMethod_ThrowsIfResultDefault() {
            var consumer = GetConsumer();

            Assert.Throws<ReturnValueException>(() =>
                consumer.PostconditionValueTypeMethod(0));
        }
        #endregion

        #region PreAndPostconditionMethod
        [Test]
        public void NonDefault_PreAndPostconditioValueTypenMethod_DoesNotThrowIfArgAndResultNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreAndPostconditionValueTypeMethod(100));
        }

        [Test]
        public void NonDefault_PreAndPostconditionValueTypeMethod_ThrowsIfArgDefault() {
            var consumer = GetConsumer();

            Assert.Throws<ArgumentException>(() =>
                consumer.PreAndPostconditionValueTypeMethod(0));
        }

        [Test]
        public void NonDefault_PreAndPostconditionValueTypeMethod_ThrowsIfResultDefault() {
            var consumer = GetConsumer();

            Assert.Throws<ReturnValueException>(() =>
                consumer.PreAndPostconditionValueTypeMethod(1));
        }
        #endregion

        #endregion
    }
}
