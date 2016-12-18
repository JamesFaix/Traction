using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    /// <summary>
    /// Tests for basic Positive property and method use cases.
    /// </summary>
    [TestFixture]
    public class PositiveTest {

        #region Properties

        #region NormalProperty
        [Test]
        public void Positive_NormalProperty_GetDoesNotThrow() {
            var demo = new PositiveConsumer();
            demo._normalProperty = 0;

            Assert.DoesNotThrow(() => {
                var x = demo.NormalReadWriteProperty;
            });
        }

        [Test]
        public void Positive_NormalProperty_SetDoesNotThrow() {
            var demo = new PositiveConsumer();

            Assert.DoesNotThrow(() => {
                demo.NormalReadWriteProperty = 0;
            });
        }
        #endregion
        
        #region ContractReadWriteProperty
        [Test]
        public void Positive_ContractReadWriteProperty_GetDoesNotThrowIfPositive() {
            var demo = new PositiveConsumer();
            demo._readWritePropertyWithContract = 1;

            Assert.DoesNotThrow(() => {
                var x = demo.ContractReadWriteProperty;
            });
        }

        [Test]
        public void Positive_ContractReadWriteProperty_GetThrowsIf0() {
            var demo = new PositiveConsumer();
            demo._readWritePropertyWithContract = 0;

            Assert.Throws<PostconditionException>(() => {
                var x = demo.ContractReadWriteProperty;
            });
        }

        [Test]
        public void Positive_ContractReadWriteProperty_GetThrowsIfNegative() {
            var demo = new PositiveConsumer();
            demo._readWritePropertyWithContract = 0;

            Assert.Throws<PostconditionException>(() => {
                var x = demo.ContractReadWriteProperty;
            });
        }

        [Test]
        public void Positive_ContractReadWriteProperty_SetDoesNotThrowIfPositive() {
            var demo = new PositiveConsumer();

            Assert.DoesNotThrow(() => {
                demo.ContractReadWriteProperty = 1;
            });
        }

        [Test]
        public void Positive_ContractReadWriteProperty_SetThrowsIf0() {
            var demo = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() => {
                demo.ContractReadWriteProperty = 0;
            });
        }

        [Test]
        public void Positive_ContractReadWriteProperty_SetThrowsIfNegative() {
            var demo = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() => {
                demo.ContractReadWriteProperty = -1;
            });
        }
        #endregion

        #endregion

        #region Methods

        #region NormalMethod
        [Test]
        public void Positive_NormalMethod_DoesNotThrow() {
            var demo = new PositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.NormalMethod());
        }
        #endregion

        #region PreconditionMethod
        [Test]
        public void Positive_PreconditionMethod_DoesNotThrowIfPositive() {
            var demo = new PositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreconditionMethod(1));
        }

        [Test]
        public void Positive_PreconditionMethod_ThrowsIf0() {
            var demo = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionMethod(0));
        }

        [Test]
        public void Positive_PreconditionMethod_ThrowsIfNegative() {
            var demo = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionMethod(-1));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void Positive_PostconditionMethod_DoesNotThrowIfPositive() {
            var demo = new PositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.PostconditionMethod(1));
        }

        [Test]
        public void Positive_PostconditionMethod_ThrowsIf0() {
            var demo = new PositiveConsumer();

            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionMethod(0));
        }

        [Test]
        public void Positive_PostconditionMethod_ThrowsIfNegative() {
            var demo = new PositiveConsumer();

            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionMethod(-1));
        }
        #endregion

        #region PreAndPostconditionMethod
        [Test]
        public void Positive_PreAndPostconditionMethod_DoesNotThrowIfArgAndResultPositive() {
            var demo = new PositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreAndPostconditionMethod(100));
        }

        [Test]
        public void Positive_PreAndPostconditionMethod_ThrowsIfArg0() {
            var demo = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreAndPostconditionMethod(0));
        }

        [Test]
        public void Positive_PreAndPostconditionMethod_ThrowsIfArgNegative() {
            var demo = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreAndPostconditionMethod(-1));
        }

        [Test]
        public void Positive_PreAndPostconditionMethod_ThrowsIfResult0() {
            var demo = new PositiveConsumer();
            Func<string> textGenerator = () => null;

            Assert.Throws<PostconditionException>(() =>
                demo.PreAndPostconditionMethod(1));
        }
        #endregion

        #region MultiplePreconditionsMethod
        [Test]
        public void Positive_MultiplePreconditionsMethod_DoesNotThrowIfArgsPositive() {
            var demo = new PositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.MultiplePreconditionsMethod(1, 3.14));
        }

        [Test]
        public void Positive_MultiplePreconditionsMethod_ThrowsIfAnyArg0() {
            var demo = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.MultiplePreconditionsMethod(1, 0));

            Assert.Throws<PreconditionException>(() =>
                demo.MultiplePreconditionsMethod(0, 1));
        }

        [Test]
        public void Positive_MultiplePreconditionsMethod_ThrowsIfAnyArgNegative() {
            var demo = new PositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.MultiplePreconditionsMethod(1, -1));

            Assert.Throws<PreconditionException>(() =>
                demo.MultiplePreconditionsMethod(-1, 1));
        }
        #endregion

        #endregion
    }
}
