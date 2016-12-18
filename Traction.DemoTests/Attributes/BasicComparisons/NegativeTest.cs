using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    /// <summary>
    /// Tests for basic Negative property and method use cases.
    /// </summary>
    [TestFixture]
    public class NegativeTest {

        #region Properties

        #region NormalProperty
        [Test]
        public void Negative_NormalProperty_GetDoesNotThrow() {
            var demo = new NegativeConsumer();
            demo._normalProperty = 0;

            Assert.DoesNotThrow(() => {
                var x = demo.NormalReadWriteProperty;
            });
        }

        [Test]
        public void Negative_NormalProperty_SetDoesNotThrow() {
            var demo = new NegativeConsumer();

            Assert.DoesNotThrow(() => {
                demo.NormalReadWriteProperty = 0;
            });
        }
        #endregion
        
        #region ContractReadWriteProperty
        [Test]
        public void Negative_ContractReadWriteProperty_GetDoesNotThrowIfNegative() {
            var demo = new NegativeConsumer();
            demo._readWritePropertyWithContract = -1;

            Assert.DoesNotThrow(() => {
                var x = demo.ContractReadWriteProperty;
            });
        }

        [Test]
        public void Negative_ContractReadWriteProperty_GetThrowsIf0() {
            var demo = new NegativeConsumer();
            demo._readWritePropertyWithContract = 0;

            Assert.Throws<PostconditionException>(() => {
                var x = demo.ContractReadWriteProperty;
            });
        }

        [Test]
        public void Negative_ContractReadWriteProperty_GetThrowsIfPositive() {
            var demo = new NegativeConsumer();
            demo._readWritePropertyWithContract = 1;

            Assert.Throws<PostconditionException>(() => {
                var x = demo.ContractReadWriteProperty;
            });
        }

        [Test]
        public void Negative_ContractReadWriteProperty_SetDoesNotThrowIfNegative() {
            var demo = new NegativeConsumer();

            Assert.DoesNotThrow(() => {
                demo.ContractReadWriteProperty = -1;
            });
        }

        [Test]
        public void Negative_ContractReadWriteProperty_SetThrowsIf0() {
            var demo = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() => {
                demo.ContractReadWriteProperty = 0;
            });
        }

        [Test]
        public void Negative_ContractReadWriteProperty_SetThrowsIfPositive() {
            var demo = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() => {
                demo.ContractReadWriteProperty = 1;
            });
        }
        #endregion

        #endregion

        #region Methods

        #region NormalMethod
        [Test]
        public void Negative_NormalMethod_DoesNotThrow() {
            var demo = new NegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.NormalMethod());
        }
        #endregion

        #region PreconditionMethod
        [Test]
        public void Negative_PreconditionMethod_DoesNotThrowIfNegative() {
            var demo = new NegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreconditionMethod(-1));
        }

        [Test]
        public void Negative_PreconditionMethod_ThrowsIf0() {
            var demo = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionMethod(0));
        }

        [Test]
        public void Negative_PreconditionMethod_ThrowsIfPositive() {
            var demo = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionMethod(1));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void Negative_PostconditionMethod_DoesNotThrowIfNegative() {
            var demo = new NegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.PostconditionMethod(-1));
        }

        [Test]
        public void Negative_PostconditionMethod_ThrowsIf0() {
            var demo = new NegativeConsumer();

            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionMethod(0));
        }

        [Test]
        public void Negative_PostconditionMethod_ThrowsIfPositive() {
            var demo = new NegativeConsumer();

            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionMethod(1));
        }
        #endregion

        #region PreAndPostconditionMethod
        [Test]
        public void Negative_PreAndPostconditionMethod_DoesNotThrowIfArgAndResultNegative() {
            var demo = new NegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreAndPostconditionMethod(-100));
        }

        [Test]
        public void Negative_PreAndPostconditionMethod_ThrowsIfArg0() {
            var demo = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreAndPostconditionMethod(0));
        }

        [Test]
        public void Negative_PreAndPostconditionMethod_ThrowsIfArgPositive() {
            var demo = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreAndPostconditionMethod(100));
        }

        [Test]
        public void Negative_PreAndPostconditionMethod_ThrowsIfResult0() {
            var demo = new NegativeConsumer();
            Func<string> textGenerator = () => null;

            Assert.Throws<PostconditionException>(() =>
                demo.PreAndPostconditionMethod(-1));
        }
        #endregion

        #region MultiplePreconditionsMethod
        [Test]
        public void Negative_MultiplePreconditionsMethod_DoesNotThrowIfArgsNegative() {
            var demo = new NegativeConsumer();

            Assert.DoesNotThrow(() =>
                demo.MultiplePreconditionsMethod(-1, -3.14));
        }

        [Test]
        public void Negative_MultiplePreconditionsMethod_ThrowsIfAnyArg0() {
            var demo = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.MultiplePreconditionsMethod(-1, 0));

            Assert.Throws<PreconditionException>(() =>
                demo.MultiplePreconditionsMethod(0, -1));
        }

        [Test]
        public void Negative_MultiplePreconditionsMethod_ThrowsIfAnyArgPositive() {
            var demo = new NegativeConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.MultiplePreconditionsMethod(1, -1));

            Assert.Throws<PreconditionException>(() =>
                demo.MultiplePreconditionsMethod(-1, 1));
        }
        #endregion

        #endregion
    }
}
