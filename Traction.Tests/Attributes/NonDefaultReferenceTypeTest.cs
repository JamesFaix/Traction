using System;
using NUnit.Framework;
using Traction.TestConsumer;

namespace Traction.Tests {

    /// <summary>
    /// Tests for basic NonDefault property and method use cases for reference types.
    /// </summary>
    [TestFixture]
    public class NonDefaultReferenceTypeTest {

        private NonDefaultConsumer GetConsumer() => new NonDefaultConsumer();

        #region Properties

        #region NormalProperty
        [Test]
        public void NonDefault_NormalReferenceTypeProperty_GetDoesNotThrow() {
            var consumer = GetConsumer();
            consumer._normalReferenceTypeProperty = null;

            Assert.DoesNotThrow(() => {
                var x = consumer.NormalReadWriteReferenceTypeProperty;
            });
        }

        [Test]
        public void NonDefault_NormalReferenceTypeProperty_SetDoesNotThrow() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.NormalReadWriteReferenceTypeProperty = null;
            });
        }
        #endregion

        #region ContractReadonlyProperty
        [Test]
        public void NonDefault_ContractReadonlyReferenceTypeProperty_DoesNotThrowIfDefault() {
            var consumer = GetConsumer();
            consumer._contractReadonlyReferenceTypeProperty = "test";

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadonlyReferenceTypeProperty;
            });
        }

        [Test]
        public void NonDefault_ContractReadonlyReferenceTypeProperty_ThrowsIfDefault() {
            var consumer = GetConsumer();
            consumer._contractReadonlyReferenceTypeProperty = null;

            Assert.Throws<ReturnValueException>(() => {
                var x = consumer.ContractReadonlyReferenceTypeProperty;
            });
        }
        #endregion

        #region ContractWriteonlyProperty
        [Test]
        public void NonDefault_ContractWriteonlyReferenceTypeProperty_DoesNotThrowIfNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractWriteonlyReferenceTypeProperty = "test";
            });
        }

        [Test]
        public void NonDefault_ContractWriteonlyReferenceTypeProperty_ThrowsIfDefault() {
            var consumer = GetConsumer();

            Assert.Throws<ArgumentException>(() => {
                consumer.ContractWriteonlyReferenceTypeProperty = null;
            });
        }
        #endregion

        #region ContractReadWriteProperty
        [Test]
        public void NonDefault_ContractReadWriteReferenceTypeProperty_GetDoesNotThrowIfNotDefault() {
            var consumer = GetConsumer();
            consumer._contractReadWriteReferenceTypeProperty = "test";

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadWriteReferenceTypeProperty;
            });
        }

        [Test]
        public void NonDefault_ContractReadWriteReferenceTypeProperty_GetThrowsIfDefault() {
            var consumer = GetConsumer();
            consumer._contractReadWriteReferenceTypeProperty = null;

            Assert.Throws<ReturnValueException>(() => {
                var x = consumer.ContractReadWriteReferenceTypeProperty;
            });
        }

        [Test]
        public void NonDefault_ContractReadWriteReferenceTypeProperty_SetDoesNotThrowIfNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractReadWriteReferenceTypeProperty = "test";
            });
        }

        [Test]
        public void NonDefault_ContractReadWriteReferenceTypeProperty_SetThrowsIfDefault() {
            var consumer = GetConsumer();

            Assert.Throws<ArgumentException>(() => {
                consumer.ContractReadWriteReferenceTypeProperty = null;
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
        public void NonDefault_PreconditionReferenceTypeMethod_DoesNotThrowIfArgNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionReferenceTypeMethod("test"));
        }

        [Test]
        public void NonDefault_PreconditionReferenceTypeMethod_ThrowsIfArgDefault() {
            var consumer = GetConsumer();

            Assert.Throws<ArgumentException>(() =>
                consumer.PreconditionReferenceTypeMethod(null));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void NonDefault_PostconditionReferenceTypeMethod_DoesNotThrowIfResultNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionReferenceTypeMethod("test"));
        }

        [Test]
        public void NonDefault_PostconditionReferenceTypeMethod_ThrowsIfResultDefault() {
            var consumer = GetConsumer();

            Assert.Throws<ReturnValueException>(() =>
                consumer.PostconditionReferenceTypeMethod(null));
        }
        #endregion

        #region PreAndPostconditionMethod
        [Test]
        public void NonDefault_PreAndPostconditioReferenceTypenMethod_DoesNotThrowIfArgAndResultNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreAndPostconditionReferenceTypeMethod("test"));
        }

        [Test]
        public void NonDefault_PreAndPostconditionReferenceTypeMethod_ThrowsIfArgDefault() {
            var consumer = GetConsumer();

            Assert.Throws<ArgumentException>(() =>
                consumer.PreAndPostconditionReferenceTypeMethod(null));
        }

        [Test]
        public void NonDefault_PreAndPostconditionReferenceTypeMethod_ThrowsIfResultDefault() {
            var consumer = GetConsumer();

            Assert.Throws<ReturnValueException>(() =>
                consumer.PreAndPostconditionReferenceTypeMethod(""));
        }
        #endregion
        
        #endregion
    }
}
