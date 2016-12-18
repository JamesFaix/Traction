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
            var demo = new NonNegativeConsumer();
            demo._normalProperty = 0;

            Assert.DoesNotThrow(() => {
                var x = demo.NormalReadWriteProperty;
            });
        }

        [Test]
        public void NonNegative_NormalProperty_SetDoesNotThrow() {
            var demo = new NonNegativeConsumer();

            Assert.DoesNotThrow(() => {
                demo.NormalReadWriteProperty = 0;
            });
        }
        #endregion
        
        #region ContractReadWriteProperty
        [Test]
        public void NonNegative_ContractReadWriteProperty_GetDoesNotThrowIfPositive() {
            var demo = new NonNegativeConsumer();
            demo._readWritePropertyWithContract = 1;

            Assert.DoesNotThrow(() => {
                var x = demo.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonNegative_ContractReadWriteProperty_GetDoesNotThrowIf0() {
            var demo = new NonNegativeConsumer();
            demo._readWritePropertyWithContract = 0;

            Assert.DoesNotThrow(() => {
                var x = demo.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonNegative_ContractReadWriteProperty_GetThrowsIfNegative() {
            var demo = new NonNegativeConsumer();
            demo._readWritePropertyWithContract = -1;

            Assert.Throws<PostconditionException>(() => {
                var x = demo.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonNegative_ContractReadWriteProperty_SetDoesNotThrowIfPositive() {
            var demo = new NonNegativeConsumer();

            Assert.DoesNotThrow(() => {
                demo.ContractReadWriteProperty = 1;
            });
        }

        [Test]
        public void NonNegative_ContractReadWriteProperty_SetDoesNotThrowIf0() {
            var demo = new NonNegativeConsumer();

            Assert.DoesNotThrow(() => {
                demo.ContractReadWriteProperty = 0;
            });
        }

        [Test]
        public void NonNegative_ContractReadWriteProperty_SetThrowsIfNegative() {
            var demo = new NonNegativeConsumer();

            Assert.Throws<PreconditionException>(() => {
                demo.ContractReadWriteProperty = -1;
            });
        }
        #endregion

        #endregion

        #region Methods

        #region NormalMethod
        [Test]
        public void NonNegative_NormalMethod_DoesNotThrow() {
            var demo = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.NormalMethod());
        }
        #endregion

        #region PreconditionMethod
        [Test]
        public void NonNegative_PreconditionMethod_DoesNotThrowIfPositive() {
            var demo = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreconditionMethod(1));
        }

        [Test]
        public void NonNegative_PreconditionMethod_DoesNotThrowIf0() {
            var demo = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreconditionMethod(0));
        }

        [Test]
        public void NonNegative_PreconditionMethod_ThrowsIfNegative() {
            var demo = new NonNegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionMethod(-1));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void NonNegative_PostconditionMethod_DoesNotThrowIfPositive() {
            var demo = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.PostconditionMethod(1));
        }

        [Test]
        public void NonNegative_PostconditionMethod_DoesNotThrowIf0() {
            var demo = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.PostconditionMethod(0));
        }

        [Test]
        public void NonNegative_PostconditionMethod_ThrowsIfNegative() {
            var demo = new NonNegativeConsumer();

            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionMethod(-1));
        }
        #endregion

        #region PreAndPostconditionMethod
        [Test]
        public void NonNegative_PreAndPostconditionMethod_DoesNotThrowIfArgAndResultPositive() {
            var demo = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreAndPostconditionMethod(1));
        }

        [Test]
        public void NonNegative_PreAndPostconditionMethod_ThrowsIfArgNegative() {
            var demo = new NonNegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreAndPostconditionMethod(-1));
        }

        [Test]
        public void NonNegative_PreAndPostconditionMethod_ThrowsIfResultNegative() {
            var demo = new NonNegativeConsumer();
            Func<string> textGenerator = () => null;

            Assert.Throws<PostconditionException>(() =>
                demo.PreAndPostconditionMethod(0));
        }
        #endregion

        #region MultiplePreconditionsMethod
        [Test]
        public void NonNegative_MultiplePreconditionsMethod_DoesNotThrowIfArgsNonNegative() {
            var demo = new NonNegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.MultiplePreconditionsMethod(1, 0));
        }

        [Test]
        public void NonNegative_MultiplePreconditionsMethod_ThrowsIfAnyArgNegative() {
            var demo = new NonNegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.MultiplePreconditionsMethod(-1, 0));

            Assert.Throws<PreconditionException>(() =>
                demo.MultiplePreconditionsMethod(0, -1));
        }
        #endregion

        #endregion
    }
}
