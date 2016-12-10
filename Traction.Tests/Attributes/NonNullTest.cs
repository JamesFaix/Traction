using System;
using NUnit.Framework;
using Traction.TestConsumer;

namespace Traction.Tests {

    [TestFixture]
    public class NonNullTest {

        #region Properties

        #region NormalProperty
        [Test]
        public void NonNull_NormalProperty_GetDoesNotThrow() {
            var consumer = new NonNullConsumer();
            consumer._normalProperty = null;

            Assert.DoesNotThrow(() => {
                var x = consumer.NormalReadWriteProperty;
            });
        }

        [Test]
        public void NonNull_NormalProperty_SetDoesNotThrow() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() => {
                consumer.NormalReadWriteProperty = null;
            });
        }
        #endregion

        #region ContractReadonlyProperty
        [Test]
        public void NonNull_ContractReadonlyProperty_DoesNotThrowIfNotNull() {
            var consumer = new NonNullConsumer();
            consumer._readonlyPropertyWithContract = "test";

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadonlyProperty;
            });
        }

        [Test]
        public void NonNull_ContractReadonlyProperty_ThrowsIfNull() {
            var consumer = new NonNullConsumer();
            consumer._readonlyPropertyWithContract = null;

            Assert.Throws<ReturnValueException>(() => {
                var x = consumer.ContractReadonlyProperty;
            });
        }
        #endregion

        #region ContractWriteonlyProperty
        [Test]
        public void NonNull_ContractWriteonlyProperty_DoesNotThrowIfNotNull() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractWriteonlyProperty = "test";
            });
        }

        [Test]
        public void NonNull_ContractWriteonlyProperty_ThrowsIfNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<ArgumentNullException>(() => {
                consumer.ContractWriteonlyProperty = null;
            });
        }
        #endregion

        #region ContractReadWriteProperty
        [Test]
        public void NonNull_ContractReadWriteProperty_GetDoesNotThrowIfNotNull() {
            var consumer = new NonNullConsumer();
            consumer._readWritePropertyWithContract = "test";

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonNull_ContractReadWriteProperty_GetThrowsIfNull() {
            var consumer = new NonNullConsumer();
            consumer._readWritePropertyWithContract = null;

            Assert.Throws<ReturnValueException>(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonNull_ContractReadWriteProperty_SetDoesNotThrowIfNotNull() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractReadWriteProperty = "test";
            });
        }

        [Test]
        public void NonNull_ContractReadWriteProperty_SetThrowsIfNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<ArgumentNullException>(() => {
                consumer.ContractReadWriteProperty = null;
            });
        }
        #endregion
                
        #endregion

        #region Methods

        #region NormalMethod
        [Test]
        public void NonNull_NormalMethod_DoesNotThrow() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() =>
                consumer.NormalMethod());
        }
        #endregion

        #region PreconditionMethod
        [Test]
        public void NonNull_PreconditionMethod_DoesNotThrowIfArgNotNull() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionMethod("test"));
        }

        [Test]
        public void NonNull_PreconditionMethod_ThrowsIfArgNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<ArgumentNullException>(() =>
                consumer.PreconditionMethod(null));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void NonNull_PostconditionMethod_DoesNotThrowIfResultNotNull() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionMethod("test"));
        }

        [Test]
        public void NonNull_PostconditionMethod_ThrowsIfResultNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<ReturnValueException>(() =>
                consumer.PostconditionMethod(null));
        }
        #endregion

        #region PreAndPostconditionMethod
        [Test]
        public void NonNull_PreAndPostconditionMethod_DoesNotThrowIfArgAndResultNotNull() {
            var consumer = new NonNullConsumer();
            Func<string> textGenerator = () => "test";

            Assert.DoesNotThrow(() =>
                consumer.PreAndPostconditionMethod(textGenerator));
        }

        [Test]
        public void NonNull_PreAndPostconditionMethod_ThrowsIfArgNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<ArgumentNullException>(() =>
                consumer.PreAndPostconditionMethod(null));
        }

        [Test]
        public void NonNull_PreAndPostconditionMethod_ThrowsIfResultNull() {
            var consumer = new NonNullConsumer();
            Func<string> textGenerator = () => null;

            Assert.Throws<ReturnValueException>(() =>
                consumer.PreAndPostconditionMethod(textGenerator));
        }
        #endregion

        #region MultiplePreconditionsMethod
        [Test]
        public void NonNull_MultiplePreconditionsMethod_DoesNotThrowIfNoArgsNull() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() =>
                consumer.MultiplePreconditionsMethod("test", "testing"));
        }

        [Test]
        public void NonNull_MultiplePreconditionsMethod_ThrowsIfAnyArgNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<ArgumentNullException>(() =>
                consumer.MultiplePreconditionsMethod("test", null));

            Assert.Throws<ArgumentNullException>(() =>
                consumer.MultiplePreconditionsMethod(null, "test"));
        }
        #endregion
        
        #endregion
    }
}
