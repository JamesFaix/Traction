using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    /// <summary>
    /// Tests for basic NonDefault property and method use cases for reference types.
    /// </summary>
    [TestFixture]
    public class NonDefaultReferenceTypeTest {

        private NonDefaultConsumer GetDemo() => new NonDefaultConsumer();

        #region Properties

        #region NormalProperty
        [Test]
        public void NonDefault_NormalReferenceTypeProperty_GetDoesNotThrow() {
            var demo = GetDemo();
            demo._normalReferenceTypeProperty = null;

            Assert.DoesNotThrow(() => {
                var x = demo.NormalReadWriteReferenceTypeProperty;
            });
        }

        [Test]
        public void NonDefault_NormalReferenceTypeProperty_SetDoesNotThrow() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() => {
                demo.NormalReadWriteReferenceTypeProperty = null;
            });
        }
        #endregion

        #region ContractReadWriteProperty
        [Test]
        public void NonDefault_ContractReadWriteReferenceTypeProperty_GetDoesNotThrowIfNotDefault() {
            var demo = GetDemo();
            demo._contractReadWriteReferenceTypeProperty = "test";

            Assert.DoesNotThrow(() => {
                var x = demo.ContractReadWriteReferenceTypeProperty;
            });
        }

        [Test]
        public void NonDefault_ContractReadWriteReferenceTypeProperty_GetThrowsIfDefault() {
            var demo = GetDemo();
            demo._contractReadWriteReferenceTypeProperty = null;

            Assert.Throws<PostconditionException>(() => {
                var x = demo.ContractReadWriteReferenceTypeProperty;
            });
        }

        [Test]
        public void NonDefault_ContractReadWriteReferenceTypeProperty_SetDoesNotThrowIfNotDefault() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() => {
                demo.ContractReadWriteReferenceTypeProperty = "test";
            });
        }

        [Test]
        public void NonDefault_ContractReadWriteReferenceTypeProperty_SetThrowsIfDefault() {
            var demo = GetDemo();

            Assert.Throws<PreconditionException>(() => {
                demo.ContractReadWriteReferenceTypeProperty = null;
            });
        }
        #endregion

        #endregion

        #region Methods

        #region NormalMethod
        [Test]
        public void NonDefault_NormalMethod_DoesNotThrow() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() =>
                demo.NormalMethod());
        }
        #endregion

        #region PreconditionMethod
        [Test]
        public void NonDefault_PreconditionReferenceTypeMethod_DoesNotThrowIfArgNotDefault() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() =>
                demo.PreconditionReferenceTypeMethod("test"));
        }

        [Test]
        public void NonDefault_PreconditionReferenceTypeMethod_ThrowsIfArgDefault() {
            var demo = GetDemo();

            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionReferenceTypeMethod(null));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void NonDefault_PostconditionReferenceTypeMethod_DoesNotThrowIfResultNotDefault() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() =>
                demo.PostconditionReferenceTypeMethod("test"));
        }

        [Test]
        public void NonDefault_PostconditionReferenceTypeMethod_ThrowsIfResultDefault() {
            var demo = GetDemo();

            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionReferenceTypeMethod(null));
        }
        #endregion

        #region PreAndPostconditionMethod
        [Test]
        public void NonDefault_PreAndPostconditioReferenceTypenMethod_DoesNotThrowIfArgAndResultNotDefault() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() =>
                demo.PreAndPostconditionReferenceTypeMethod("test"));
        }

        [Test]
        public void NonDefault_PreAndPostconditionReferenceTypeMethod_ThrowsIfArgDefault() {
            var demo = GetDemo();

            Assert.Throws<PreconditionException>(() =>
                demo.PreAndPostconditionReferenceTypeMethod(null));
        }

        [Test]
        public void NonDefault_PreAndPostconditionReferenceTypeMethod_ThrowsIfResultDefault() {
            var demo = GetDemo();

            Assert.Throws<PostconditionException>(() =>
                demo.PreAndPostconditionReferenceTypeMethod(""));
        }
        #endregion
        
        #endregion
    }
}
