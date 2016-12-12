using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.Tests {

    /// <summary>
    /// Tests for basic Negative property and method use cases.
    /// </summary>
    [TestFixture]
    public class NegativeTest {

        #region Properties

        #region NormalProperty
        [Test]
        public void Negative_NormalProperty_GetDoesNotThrow() {
            var consumer = new NegativeConsumer();
            consumer._normalProperty = 0;

            Assert.DoesNotThrow(() => {
                var x = consumer.NormalReadWriteProperty;
            });
        }

        [Test]
        public void Negative_NormalProperty_SetDoesNotThrow() {
            var consumer = new NegativeConsumer();

            Assert.DoesNotThrow(() => {
                consumer.NormalReadWriteProperty = 0;
            });
        }
        #endregion
        
        #region ContractReadWriteProperty
        [Test]
        public void Negative_ContractReadWriteProperty_GetDoesNotThrowIfNegative() {
            var consumer = new NegativeConsumer();
            consumer._readWritePropertyWithContract = -1;

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void Negative_ContractReadWriteProperty_GetThrowsIf0() {
            var consumer = new NegativeConsumer();
            consumer._readWritePropertyWithContract = 0;

            Assert.Throws<PostconditionException>(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void Negative_ContractReadWriteProperty_GetThrowsIfPositive() {
            var consumer = new NegativeConsumer();
            consumer._readWritePropertyWithContract = 1;

            Assert.Throws<PostconditionException>(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void Negative_ContractReadWriteProperty_SetDoesNotThrowIfNegative() {
            var consumer = new NegativeConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractReadWriteProperty = -1;
            });
        }

        [Test]
        public void Negative_ContractReadWriteProperty_SetThrowsIf0() {
            var consumer = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() => {
                consumer.ContractReadWriteProperty = 0;
            });
        }

        [Test]
        public void Negative_ContractReadWriteProperty_SetThrowsIfPositive() {
            var consumer = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() => {
                consumer.ContractReadWriteProperty = 1;
            });
        }
        #endregion

        #endregion

        #region Methods

        #region NormalMethod
        [Test]
        public void Negative_NormalMethod_DoesNotThrow() {
            var consumer = new NegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.NormalMethod());
        }
        #endregion

        #region PreconditionMethod
        [Test]
        public void Negative_PreconditionMethod_DoesNotThrowIfNegative() {
            var consumer = new NegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionMethod(-1));
        }

        [Test]
        public void Negative_PreconditionMethod_ThrowsIf0() {
            var consumer = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreconditionMethod(0));
        }

        [Test]
        public void Negative_PreconditionMethod_ThrowsIfPositive() {
            var consumer = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreconditionMethod(1));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void Negative_PostconditionMethod_DoesNotThrowIfNegative() {
            var consumer = new NegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionMethod(-1));
        }

        [Test]
        public void Negative_PostconditionMethod_ThrowsIf0() {
            var consumer = new NegativeConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionMethod(0));
        }

        [Test]
        public void Negative_PostconditionMethod_ThrowsIfPositive() {
            var consumer = new NegativeConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionMethod(1));
        }
        #endregion

        #region PreAndPostconditionMethod
        [Test]
        public void Negative_PreAndPostconditionMethod_DoesNotThrowIfArgAndResultNegative() {
            var consumer = new NegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreAndPostconditionMethod(-100));
        }

        [Test]
        public void Negative_PreAndPostconditionMethod_ThrowsIfArg0() {
            var consumer = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreAndPostconditionMethod(0));
        }

        [Test]
        public void Negative_PreAndPostconditionMethod_ThrowsIfArgPositive() {
            var consumer = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreAndPostconditionMethod(100));
        }

        [Test]
        public void Negative_PreAndPostconditionMethod_ThrowsIfResult0() {
            var consumer = new NegativeConsumer();
            Func<string> textGenerator = () => null;

            Assert.Throws<PostconditionException>(() =>
                consumer.PreAndPostconditionMethod(-1));
        }
        #endregion

        #region MultiplePreconditionsMethod
        [Test]
        public void Negative_MultiplePreconditionsMethod_DoesNotThrowIfArgsNegative() {
            var consumer = new NegativeConsumer();

            Assert.DoesNotThrow(() =>
                consumer.MultiplePreconditionsMethod(-1, -3.14));
        }

        [Test]
        public void Negative_MultiplePreconditionsMethod_ThrowsIfAnyArg0() {
            var consumer = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.MultiplePreconditionsMethod(-1, 0));

            Assert.Throws<PreconditionException>(() =>
                consumer.MultiplePreconditionsMethod(0, -1));
        }

        [Test]
        public void Negative_MultiplePreconditionsMethod_ThrowsIfAnyArgPositive() {
            var consumer = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.MultiplePreconditionsMethod(1, -1));

            Assert.Throws<PreconditionException>(() =>
                consumer.MultiplePreconditionsMethod(-1, 1));
        }
        #endregion

        #endregion
    }
}
