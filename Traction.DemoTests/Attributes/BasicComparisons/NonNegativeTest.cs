using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    /// <summary>
    /// Tests for basic NonNegative property and method use cases.
    /// </summary>
    [TestFixture]
    public class NonNegativeTest {

        #region Properties

        #region NormalProperty
        [Test]
        public void NonNegative_NormalProperty_GetDoesNotThrow() {
            var consumer = new NonNegativeConsumer();
            consumer._normalProperty = 0;

            Assert.DoesNotThrow(() => {
                var x = consumer.NormalReadWriteProperty;
            });
        }

        [Test]
        public void NonNegative_NormalProperty_SetDoesNotThrow() {
            var consumer = new NonNegativeConsumer();

            Assert.DoesNotThrow(() => {
                consumer.NormalReadWriteProperty = 0;
            });
        }
        #endregion
        
        #region ContractReadWriteProperty
        [Test]
        public void NonNegative_ContractReadWriteProperty_GetDoesNotThrowIfPositive() {
            var consumer = new NonNegativeConsumer();
            consumer._readWritePropertyWithContract = 1;

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonNegative_ContractReadWriteProperty_GetDoesNotThrowIf0() {
            var consumer = new NonNegativeConsumer();
            consumer._readWritePropertyWithContract = 0;

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonNegative_ContractReadWriteProperty_GetThrowsIfNegative() {
            var consumer = new NonNegativeConsumer();
            consumer._readWritePropertyWithContract = -1;

            Assert.Throws<PostconditionException>(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonNegative_ContractReadWriteProperty_SetDoesNotThrowIfPositive() {
            var consumer = new NonNegativeConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractReadWriteProperty = 1;
            });
        }

        [Test]
        public void NonNegative_ContractReadWriteProperty_SetDoesNotThrowIf0() {
            var consumer = new NonNegativeConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractReadWriteProperty = 0;
            });
        }

        [Test]
        public void NonNegative_ContractReadWriteProperty_SetThrowsIfNegative() {
            var consumer = new NonNegativeConsumer();

            Assert.Throws<PreconditionException>(() => {
                consumer.ContractReadWriteProperty = -1;
            });
        }
        #endregion

        #endregion

        #region Methods

        #region NormalMethod
        [Test]
        public void NonNegative_NormalMethod_DoesNotThrow() {
            var consumer = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.NormalMethod());
        }
        #endregion

        #region PreconditionMethod
        [Test]
        public void NonNegative_PreconditionMethod_DoesNotThrowIfPositive() {
            var consumer = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionMethod(1));
        }

        [Test]
        public void NonNegative_PreconditionMethod_DoesNotThrowIf0() {
            var consumer = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionMethod(0));
        }

        [Test]
        public void NonNegative_PreconditionMethod_ThrowsIfNegative() {
            var consumer = new NonNegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreconditionMethod(-1));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void NonNegative_PostconditionMethod_DoesNotThrowIfPositive() {
            var consumer = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionMethod(1));
        }

        [Test]
        public void NonNegative_PostconditionMethod_DoesNotThrowIf0() {
            var consumer = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionMethod(0));
        }

        [Test]
        public void NonNegative_PostconditionMethod_ThrowsIfNegative() {
            var consumer = new NonNegativeConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionMethod(-1));
        }
        #endregion

        #region PreAndPostconditionMethod
        [Test]
        public void NonNegative_PreAndPostconditionMethod_DoesNotThrowIfArgAndResultPositive() {
            var consumer = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreAndPostconditionMethod(1));
        }

        [Test]
        public void NonNegative_PreAndPostconditionMethod_ThrowsIfArgNegative() {
            var consumer = new NonNegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreAndPostconditionMethod(-1));
        }

        [Test]
        public void NonNegative_PreAndPostconditionMethod_ThrowsIfResultNegative() {
            var consumer = new NonNegativeConsumer();
            Func<string> textGenerator = () => null;

            Assert.Throws<PostconditionException>(() =>
                consumer.PreAndPostconditionMethod(0));
        }
        #endregion

        #region MultiplePreconditionsMethod
        [Test]
        public void NonNegative_MultiplePreconditionsMethod_DoesNotThrowIfArgsNonNegative() {
            var consumer = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.MultiplePreconditionsMethod(1, 0));
        }

        [Test]
        public void NonNegative_MultiplePreconditionsMethod_ThrowsIfAnyArgNegative() {
            var consumer = new NonNegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.MultiplePreconditionsMethod(-1, 0));

            Assert.Throws<PreconditionException>(() =>
                consumer.MultiplePreconditionsMethod(0, -1));
        }
        #endregion

        #endregion
    }
}
