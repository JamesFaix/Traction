using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    /// <summary>
    /// Tests for basic NonPositive property and method use cases.
    /// </summary>
    [TestFixture]
    public class NonPositiveTest {

        #region Properties

        #region NormalProperty
        [Test]
        public void NonPositive_NormalProperty_GetDoesNotThrow() {
            var consumer = new NonPositiveConsumer();
            consumer._normalProperty = 0;

            Assert.DoesNotThrow(() => {
                var x = consumer.NormalReadWriteProperty;
            });
        }

        [Test]
        public void NonPositive_NormalProperty_SetDoesNotThrow() {
            var consumer = new NonPositiveConsumer();

            Assert.DoesNotThrow(() => {
                consumer.NormalReadWriteProperty = 0;
            });
        }
        #endregion
        
        #region ContractReadWriteProperty
        [Test]
        public void NonPositive_ContractReadWriteProperty_GetDoesNotThrowIfNegative() {
            var consumer = new NonPositiveConsumer();
            consumer._readWritePropertyWithContract = -1;

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonPositive_ContractReadWriteProperty_GetDoesNotThrowIf0() {
            var consumer = new NonPositiveConsumer();
            consumer._readWritePropertyWithContract = 0;

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonPositive_ContractReadWriteProperty_GetThrowsIfPositive() {
            var consumer = new NonPositiveConsumer();
            consumer._readWritePropertyWithContract = 1;

            Assert.Throws<PostconditionException>(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonPositive_ContractReadWriteProperty_SetDoesNotThrowIfNegative() {
            var consumer = new NonPositiveConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractReadWriteProperty = -1;
            });
        }

        [Test]
        public void NonPositive_ContractReadWriteProperty_SetDoesNotThrowIf0() {
            var consumer = new NonPositiveConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractReadWriteProperty = 0;
            });
        }

        [Test]
        public void NonPositive_ContractReadWriteProperty_SetThrowsIfPositive() {
            var consumer = new NonPositiveConsumer();

            Assert.Throws<PreconditionException>(() => {
                consumer.ContractReadWriteProperty = 1;
            });
        }
        #endregion

        #endregion

        #region Methods

        #region NormalMethod
        [Test]
        public void NonPositive_NormalMethod_DoesNotThrow() {
            var consumer = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.NormalMethod());
        }
        #endregion

        #region PreconditionMethod
        [Test]
        public void NonPositive_PreconditionMethod_DoesNotThrowIfNegative() {
            var consumer = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionMethod(-1));
        }

        [Test]
        public void NonPositive_PreconditionMethod_DoesNotThrowIf0() {
            var consumer = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionMethod(0));
        }

        [Test]
        public void NonPositive_PreconditionMethod_ThrowsIfPositive() {
            var consumer = new NonPositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreconditionMethod(1));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void NonPositive_PostconditionMethod_DoesNotThrowIfNegative() {
            var consumer = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionMethod(-1));
        }

        [Test]
        public void NonPositive_PostconditionMethod_DoesNotThrowIf0() {
            var consumer = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionMethod(0));
        }

        [Test]
        public void NonPositive_PostconditionMethod_ThrowsIfPositive() {
            var consumer = new NonPositiveConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionMethod(1));
        }
        #endregion

        #region PreAndPostconditionMethod
        [Test]
        public void NonPositive_PreAndPostconditionMethod_DoesNotThrowIfArgAndResultNegative() {
            var consumer = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreAndPostconditionMethod(-1));
        }

        [Test]
        public void NonPositive_PreAndPostconditionMethod_ThrowsIfArgPositive() {
            var consumer = new NonPositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreAndPostconditionMethod(1));
        }

        [Test]
        public void NonPositive_PreAndPostconditionMethod_ThrowsIfResultPositive() {
            var consumer = new NonPositiveConsumer();
            Func<string> textGenerator = () => null;

            Assert.Throws<PostconditionException>(() =>
                consumer.PreAndPostconditionMethod(0));
        }
        #endregion

        #region MultiplePreconditionsMethod
        [Test]
        public void NonPositive_MultiplePreconditionsMethod_DoesNotThrowIfArgsNonPositive() {
            var consumer = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.MultiplePreconditionsMethod(-1, 0));
        }

        [Test]
        public void NonPositive_MultiplePreconditionsMethod_ThrowsIfAnyArgPositive() {
            var consumer = new NonPositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.MultiplePreconditionsMethod(1, 0));

            Assert.Throws<PreconditionException>(() =>
                consumer.MultiplePreconditionsMethod(0, 1));
        }
        #endregion

        #endregion
    }
}
