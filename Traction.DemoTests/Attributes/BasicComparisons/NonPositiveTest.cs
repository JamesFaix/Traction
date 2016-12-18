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
            var demo = new NonPositiveConsumer();
            demo._normalProperty = 0;

            Assert.DoesNotThrow(() => {
                var x = demo.NormalReadWriteProperty;
            });
        }

        [Test]
        public void NonPositive_NormalProperty_SetDoesNotThrow() {
            var demo = new NonPositiveConsumer();

            Assert.DoesNotThrow(() => {
                demo.NormalReadWriteProperty = 0;
            });
        }
        #endregion
        
        #region ContractReadWriteProperty
        [Test]
        public void NonPositive_ContractReadWriteProperty_GetDoesNotThrowIfNegative() {
            var demo = new NonPositiveConsumer();
            demo._readWritePropertyWithContract = -1;

            Assert.DoesNotThrow(() => {
                var x = demo.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonPositive_ContractReadWriteProperty_GetDoesNotThrowIf0() {
            var demo = new NonPositiveConsumer();
            demo._readWritePropertyWithContract = 0;

            Assert.DoesNotThrow(() => {
                var x = demo.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonPositive_ContractReadWriteProperty_GetThrowsIfPositive() {
            var demo = new NonPositiveConsumer();
            demo._readWritePropertyWithContract = 1;

            Assert.Throws<PostconditionException>(() => {
                var x = demo.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonPositive_ContractReadWriteProperty_SetDoesNotThrowIfNegative() {
            var demo = new NonPositiveConsumer();

            Assert.DoesNotThrow(() => {
                demo.ContractReadWriteProperty = -1;
            });
        }

        [Test]
        public void NonPositive_ContractReadWriteProperty_SetDoesNotThrowIf0() {
            var demo = new NonPositiveConsumer();

            Assert.DoesNotThrow(() => {
                demo.ContractReadWriteProperty = 0;
            });
        }

        [Test]
        public void NonPositive_ContractReadWriteProperty_SetThrowsIfPositive() {
            var demo = new NonPositiveConsumer();

            Assert.Throws<PreconditionException>(() => {
                demo.ContractReadWriteProperty = 1;
            });
        }
        #endregion

        #endregion

        #region Methods

        #region NormalMethod
        [Test]
        public void NonPositive_NormalMethod_DoesNotThrow() {
            var demo = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.NormalMethod());
        }
        #endregion

        #region PreconditionMethod
        [Test]
        public void NonPositive_PreconditionMethod_DoesNotThrowIfNegative() {
            var demo = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreconditionMethod(-1));
        }

        [Test]
        public void NonPositive_PreconditionMethod_DoesNotThrowIf0() {
            var demo = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreconditionMethod(0));
        }

        [Test]
        public void NonPositive_PreconditionMethod_ThrowsIfPositive() {
            var demo = new NonPositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionMethod(1));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void NonPositive_PostconditionMethod_DoesNotThrowIfNegative() {
            var demo = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.PostconditionMethod(-1));
        }

        [Test]
        public void NonPositive_PostconditionMethod_DoesNotThrowIf0() {
            var demo = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.PostconditionMethod(0));
        }

        [Test]
        public void NonPositive_PostconditionMethod_ThrowsIfPositive() {
            var demo = new NonPositiveConsumer();

            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionMethod(1));
        }
        #endregion

        #region PreAndPostconditionMethod
        [Test]
        public void NonPositive_PreAndPostconditionMethod_DoesNotThrowIfArgAndResultNegative() {
            var demo = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreAndPostconditionMethod(-1));
        }

        [Test]
        public void NonPositive_PreAndPostconditionMethod_ThrowsIfArgPositive() {
            var demo = new NonPositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreAndPostconditionMethod(1));
        }

        [Test]
        public void NonPositive_PreAndPostconditionMethod_ThrowsIfResultPositive() {
            var demo = new NonPositiveConsumer();
            Func<string> textGenerator = () => null;

            Assert.Throws<PostconditionException>(() =>
                demo.PreAndPostconditionMethod(0));
        }
        #endregion

        #region MultiplePreconditionsMethod
        [Test]
        public void NonPositive_MultiplePreconditionsMethod_DoesNotThrowIfArgsNonPositive() {
            var demo = new NonPositiveConsumer();

            Assert.DoesNotThrow(() =>
                demo.MultiplePreconditionsMethod(-1, 0));
        }

        [Test]
        public void NonPositive_MultiplePreconditionsMethod_ThrowsIfAnyArgPositive() {
            var demo = new NonPositiveConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.MultiplePreconditionsMethod(1, 0));

            Assert.Throws<PreconditionException>(() =>
                demo.MultiplePreconditionsMethod(0, 1));
        }
        #endregion

        #endregion
    }
}
