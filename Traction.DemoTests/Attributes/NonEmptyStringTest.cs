using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    [TestFixture]
    public class NonEmptyStringTest {

        private NonEmptyConsumer GetDemo() => new NonEmptyConsumer();

        #region Properties

        #region NormalProperty
        [Test]
        public void NonEmpty_NormalProperty_GetDoesNotThrow() {
            var demo = GetDemo();
            demo._normalProperty = "";

            Assert.DoesNotThrow(() => {
                var x = demo.NormalReadWriteProperty;
            });
        }

        [Test]
        public void NonEmpty_NormalProperty_SetDoesNotThrow() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() => {
                demo.NormalReadWriteProperty = "";
            });
        }
        #endregion

        #region ContractReadWriteProperty
        [Test]
        public void NonEmpty_ContractReadWriteProperty_GetDoesNotThrowIfNotEmpty() {
            var demo = GetDemo();
            demo._readWritePropertyWithContract = "test";

            Assert.DoesNotThrow(() => {
                var x = demo.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonEmpty_ContractReadWriteProperty_GetThrowsIfEmpty() {
            var demo = GetDemo();
            demo._readWritePropertyWithContract = "";

            Assert.Throws<PostconditionException>(() => {
                var x = demo.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonEmpty_ContractReadWriteProperty_GetThrowsIfNull() {
            var demo = GetDemo();
            demo._readWritePropertyWithContract = null;

            Assert.Throws<PostconditionException>(() => {
                var x = demo.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonEmpty_ContractReadWriteProperty_SetDoesNotThrowIfNotEmpty() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() => {
                demo.ContractReadWriteProperty = "test";
            });
        }

        [Test]
        public void NonEmpty_ContractReadWriteProperty_SetThrowsIfEmpty() {
            var demo = GetDemo();

            Assert.Throws<PreconditionException>(() => {
                demo.ContractReadWriteProperty = "";
            });
        }

        [Test]
        public void NonEmpty_ContractReadWriteProperty_SetThrowsIfNull() {
            var demo = GetDemo();

            Assert.Throws<PreconditionException>(() => {
                demo.ContractReadWriteProperty = null;
            });
        }
        #endregion

        #endregion

        #region Methods

        #region NormalMethod
        [Test]
        public void NonEmpty_NormalMethod_DoesNotThrow() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() =>
                demo.NormalMethod());
        }
        #endregion

        #region PreconditionMethod
        [Test]
        public void NonEmpty_PreconditionMethod_DoesNotThrowIfArgNotEmpty() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() =>
                demo.PreconditionMethod("test"));
        }

        [Test]
        public void NonEmpty_PreconditionMethod_ThrowsIfArgEmpty() {
            var demo = GetDemo();

            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionMethod(""));
        }

        [Test]
        public void NonEmpty_PreconditionMethod_ThrowsIfArgNull() {
            var demo = GetDemo();

            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionMethod(null));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void NonEmpty_PostconditionMethod_DoesNotThrowIfResultNotEmpty() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() =>
                demo.PostconditionMethod("test"));
        }

        [Test]
        public void NonEmpty_PostconditionMethod_ThrowsIfResultEmpty() {
            var demo = GetDemo();

            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionMethod(""));
        }


        [Test]
        public void NonEmpty_PostconditionMethod_ThrowsIfResultNull() {
            var demo = GetDemo();

            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionMethod(null));
        }
        #endregion

        #region PreAndPostconditionMethod
        [Test]
        public void NonEmpty_PreAndPostconditionMethod_DoesNotThrowIfArgAndResultNotEmpty() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() =>
                demo.PreAndPostconditionMethod("test"));
        }

        [Test]
        public void NonEmpty_PreAndPostconditionMethod_ThrowsIfArgEmpty() {
            var demo = GetDemo();

            Assert.Throws<PreconditionException>(() =>
                demo.PreAndPostconditionMethod(""));
        }

        [Test]
        public void NonEmpty_PreAndPostconditionMethod_ThrowsIfResultEmpty() {
            var demo = GetDemo();

            Assert.Throws<PostconditionException>(() =>
                demo.PreAndPostconditionMethod("a"));
        }
        #endregion

        #region MultiplePreconditionsMethod
        [Test]
        public void NonEmpty_MultiplePreconditionsMethod_DoesNotThrowIfNoArgsEmpty() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() =>
                demo.MultiplePreconditionsMethod("test", "testing"));
        }

        [Test]
        public void NonEmpty_MultiplePreconditionsMethod_ThrowsIfAnyArgEmpty() {
            var demo = GetDemo();

            Assert.Throws<PreconditionException>(() =>
                demo.MultiplePreconditionsMethod("test", ""));

            Assert.Throws<PreconditionException>(() =>
                demo.MultiplePreconditionsMethod("", "test"));
        }
        #endregion

        #endregion
    }
}
