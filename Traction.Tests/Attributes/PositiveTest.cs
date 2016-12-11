using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.Tests {

    /// <summary>
    /// Tests for basic Positive property and method use cases.
    /// </summary>
    [TestFixture]
    public class PositiveTest {

        #region Properties

        #region NormalProperty
        [Test]
        public void Positive_NormalProperty_GetDoesNotThrow() {
            var consumer = new PositiveConsumer();
            consumer._normalProperty = 0;

            Assert.DoesNotThrow(() => {
                var x = consumer.NormalReadWriteProperty;
            });
        }

        [Test]
        public void Positive_NormalProperty_SetDoesNotThrow() {
            var consumer = new PositiveConsumer();

            Assert.DoesNotThrow(() => {
                consumer.NormalReadWriteProperty = 0;
            });
        }
        #endregion
        
        #region ContractReadWriteProperty
        [Test]
        public void Positive_ContractReadWriteProperty_GetDoesNotThrowIfPositive() {
            var consumer = new PositiveConsumer();
            consumer._readWritePropertyWithContract = 1;

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void Positive_ContractReadWriteProperty_GetThrowsIf0() {
            var consumer = new PositiveConsumer();
            consumer._readWritePropertyWithContract = 0;

            Assert.Throws<PostconditionException>(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void Positive_ContractReadWriteProperty_GetThrowsIfNegative() {
            var consumer = new PositiveConsumer();
            consumer._readWritePropertyWithContract = 0;

            Assert.Throws<PostconditionException>(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void Positive_ContractReadWriteProperty_SetDoesNotThrowIfPositive() {
            var consumer = new PositiveConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractReadWriteProperty = 1;
            });
        }

        [Test]
        public void Positive_ContractReadWriteProperty_SetThrowsIf0() {
            var consumer = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() => {
                consumer.ContractReadWriteProperty = 0;
            });
        }

        [Test]
        public void Positive_ContractReadWriteProperty_SetThrowsIfNegative() {
            var consumer = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() => {
                consumer.ContractReadWriteProperty = 1;
            });
        }
        #endregion

        #endregion

        #region Methods

        #region NormalMethod
        [Test]
        public void Positive_NormalMethod_DoesNotThrow() {
            var consumer = new PositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.NormalMethod());
        }
        #endregion

        #region PreconditionMethod
        [Test]
        public void Positive_PreconditionMethod_DoesNotThrowIfPositive() {
            var consumer = new PositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionMethod(1));
        }

        [Test]
        public void Positive_PreconditionMethod_ThrowsIf0() {
            var consumer = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreconditionMethod(0));
        }

        [Test]
        public void Positive_PreconditionMethod_ThrowsIfNegative() {
            var consumer = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreconditionMethod(-1));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void Positive_PostconditionMethod_DoesNotThrowIfPositive() {
            var consumer = new PositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionMethod(1));
        }

        [Test]
        public void Positive_PostconditionMethod_ThrowsIf0() {
            var consumer = new PositiveConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionMethod(0));
        }

        [Test]
        public void Positive_PostconditionMethod_ThrowsIfNegative() {
            var consumer = new PositiveConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionMethod(-1));
        }
        #endregion

        #region PreAndPostconditionMethod
        [Test]
        public void Positive_PreAndPostconditionMethod_DoesNotThrowIfArgAndResultPositive() {
            var consumer = new PositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreAndPostconditionMethod(100));
        }

        [Test]
        public void Positive_PreAndPostconditionMethod_ThrowsIfArg0() {
            var consumer = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreAndPostconditionMethod(0));
        }

        [Test]
        public void Positive_PreAndPostconditionMethod_ThrowsIfArgNegative() {
            var consumer = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreAndPostconditionMethod(-1));
        }

        [Test]
        public void Positive_PreAndPostconditionMethod_ThrowsIfResult0() {
            var consumer = new PositiveConsumer();
            Func<string> textGenerator = () => null;

            Assert.Throws<PostconditionException>(() =>
                consumer.PreAndPostconditionMethod(1));
        }
        #endregion

        #region MultiplePreconditionsMethod
        [Test]
        public void Positive_MultiplePreconditionsMethod_DoesNotThrowIfArgsPositive() {
            var consumer = new PositiveConsumer();

            Assert.DoesNotThrow(() =>
                consumer.MultiplePreconditionsMethod(1, 3.14));
        }

        [Test]
        public void Positive_MultiplePreconditionsMethod_ThrowsIfAnyArg0() {
            var consumer = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.MultiplePreconditionsMethod(1, 0));

            Assert.Throws<PreconditionException>(() =>
                consumer.MultiplePreconditionsMethod(0, 1));
        }

        [Test]
        public void Positive_MultiplePreconditionsMethod_ThrowsIfAnyArgNegative() {
            var consumer = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.MultiplePreconditionsMethod(1, -1));

            Assert.Throws<PreconditionException>(() =>
                consumer.MultiplePreconditionsMethod(-1, 1));
        }
        #endregion

        #endregion
    }
}
