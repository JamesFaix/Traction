using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    /// <summary>
    /// Tests for basic NonNull property and method use cases.
    /// </summary>
    [TestFixture]
    public class NonNullTest {

        #region Properties

        #region NormalProperty
        [Test]
        public void NonNull_NormalProperty_GetDoesNotThrow() {
            var demo = new NonNullConsumer();
            demo._normalProperty = null;

            Assert.DoesNotThrow(() => {
                var x = demo.NormalReadWriteProperty;
            });
        }

        [Test]
        public void NonNull_NormalProperty_SetDoesNotThrow() {
            var demo = new NonNullConsumer();

            Assert.DoesNotThrow(() => {
                demo.NormalReadWriteProperty = null;
            });
        }
        #endregion
        
        #region ContractReadWriteProperty
        [Test]
        public void NonNull_ContractReadWriteProperty_GetDoesNotThrowIfNotNull() {
            var demo = new NonNullConsumer();
            demo._readWritePropertyWithContract = "test";

            Assert.DoesNotThrow(() => {
                var x = demo.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonNull_ContractReadWriteProperty_GetThrowsIfNull() {
            var demo = new NonNullConsumer();
            demo._readWritePropertyWithContract = null;

            Assert.Throws<PostconditionException>(() => {
                var x = demo.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonNull_ContractReadWriteProperty_SetDoesNotThrowIfNotNull() {
            var demo = new NonNullConsumer();

            Assert.DoesNotThrow(() => {
                demo.ContractReadWriteProperty = "test";
            });
        }

        [Test]
        public void NonNull_ContractReadWriteProperty_SetThrowsIfNull() {
            var demo = new NonNullConsumer();

            Assert.Throws<PreconditionException>(() => {
                demo.ContractReadWriteProperty = null;
            });
        }
        #endregion
                
        #endregion

        #region Methods

        #region NormalMethod
        [Test]
        public void NonNull_NormalMethod_DoesNotThrow() {
            var demo = new NonNullConsumer();

            Assert.DoesNotThrow(() =>
                demo.NormalMethod());
        }
        #endregion

        #region PreconditionMethod
        [Test]
        public void NonNull_PreconditionMethod_DoesNotThrowIfArgNotNull() {
            var demo = new NonNullConsumer();

            Assert.DoesNotThrow(() =>
                demo.PreconditionMethod("test"));
        }

        [Test]
        public void NonNull_PreconditionMethod_ThrowsIfArgNull() {
            var demo = new NonNullConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionMethod(null));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void NonNull_PostconditionMethod_DoesNotThrowIfResultNotNull() {
            var demo = new NonNullConsumer();

            Assert.DoesNotThrow(() =>
                demo.PostconditionMethod("test"));
        }

        [Test]
        public void NonNull_PostconditionMethod_ThrowsIfResultNull() {
            var demo = new NonNullConsumer();

            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionMethod(null));
        }
        #endregion

        #region PreAndPostconditionMethod
        [Test]
        public void NonNull_PreAndPostconditionMethod_DoesNotThrowIfArgAndResultNotNull() {
            var demo = new NonNullConsumer();
            Func<string> textGenerator = () => "test";

            Assert.DoesNotThrow(() =>
                demo.PreAndPostconditionMethod(textGenerator));
        }

        [Test]
        public void NonNull_PreAndPostconditionMethod_ThrowsIfArgNull() {
            var demo = new NonNullConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.PreAndPostconditionMethod(null));
        }

        [Test]
        public void NonNull_PreAndPostconditionMethod_ThrowsIfResultNull() {
            var demo = new NonNullConsumer();
            Func<string> textGenerator = () => null;

            Assert.Throws<PostconditionException>(() =>
                demo.PreAndPostconditionMethod(textGenerator));
        }
        #endregion

        #region MultiplePreconditionsMethod
        [Test]
        public void NonNull_MultiplePreconditionsMethod_DoesNotThrowIfNoArgsNull() {
            var demo = new NonNullConsumer();

            Assert.DoesNotThrow(() =>
                demo.MultiplePreconditionsMethod("test", "testing"));
        }

        [Test]
        public void NonNull_MultiplePreconditionsMethod_ThrowsIfAnyArgNull() {
            var demo = new NonNullConsumer();

            Assert.Throws<PreconditionException>(() =>
                demo.MultiplePreconditionsMethod("test", null));

            Assert.Throws<PreconditionException>(() =>
                demo.MultiplePreconditionsMethod(null, "test"));
        }
        #endregion
        
        #endregion
    }
}
