using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    [TestFixture]
    public class NonDefaultValueTypeTest {

        private NonDefaultConsumer GetDemo() => new NonDefaultConsumer();

        #region Properties

        #region NormalProperty
        [Test]
        public void NonDefault_NormalValueTypeProperty_GetDoesNotThrow() {
            var demo = GetDemo();
            demo._normalValueTypeProperty = 0;

            Assert.DoesNotThrow(() => {
                var x = demo.NormalReadWriteValueTypeProperty;
            });
        }

        [Test]
        public void NonDefault_NormalValueTypeProperty_SetDoesNotThrow() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() => {
                demo.NormalReadWriteValueTypeProperty = 0;
            });
        }
        #endregion

        #region ContractReadWriteProperty
        [Test]
        public void NonDefault_ContractReadWriteValueTypeProperty_GetDoesNotThrowIfNotDefault() {
            var demo = GetDemo();
            demo._contractReadWriteValueTypeProperty = 1;

            Assert.DoesNotThrow(() => {
                var x = demo.ContractReadWriteValueTypeProperty;
            });
        }

        [Test]
        public void NonDefault_ContractReadWriteValueTypeProperty_GetThrowsIfDefault() {
            var demo = GetDemo();
            demo._contractReadWriteValueTypeProperty = 0;

            Assert.Throws<PostconditionException>(() => {
                var x = demo.ContractReadWriteValueTypeProperty;
            });
        }

        [Test]
        public void NonDefault_ContractReadWriteValueTypeProperty_SetDoesNotThrowIfNotDefault() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() => {
                demo.ContractReadWriteValueTypeProperty = 1;
            });
        }

        [Test]
        public void NonDefault_ContractReadWriteValueTypeProperty_SetThrowsIfDefault() {
            var demo = GetDemo();

            Assert.Throws<PreconditionException>(() => {
                demo.ContractReadWriteValueTypeProperty = 0;
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
        public void NonDefault_PreconditionValueTypeMethod_DoesNotThrowIfArgNotDefault() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() =>
                demo.PreconditionValueTypeMethod(1));
        }

        [Test]
        public void NonDefault_PreconditionValueTypeMethod_ThrowsIfArgDefault() {
            var demo = GetDemo();

            Assert.Throws<PreconditionException>(() =>
                demo.PreconditionValueTypeMethod(0));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void NonDefault_PostconditionValueTypeMethod_DoesNotThrowIfResultNotDefault() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() =>
                demo.PostconditionValueTypeMethod(1));
        }

        [Test]
        public void NonDefault_PostconditionValueTypeMethod_ThrowsIfResultDefault() {
            var demo = GetDemo();

            Assert.Throws<PostconditionException>(() =>
                demo.PostconditionValueTypeMethod(0));
        }
        #endregion

        #region PreAndPostconditionMethod
        [Test]
        public void NonDefault_PreAndPostconditioValueTypenMethod_DoesNotThrowIfArgAndResultNotDefault() {
            var demo = GetDemo();

            Assert.DoesNotThrow(() =>
                demo.PreAndPostconditionValueTypeMethod(100));
        }

        [Test]
        public void NonDefault_PreAndPostconditionValueTypeMethod_ThrowsIfArgDefault() {
            var demo = GetDemo();

            Assert.Throws<PreconditionException>(() =>
                demo.PreAndPostconditionValueTypeMethod(0));
        }

        [Test]
        public void NonDefault_PreAndPostconditionValueTypeMethod_ThrowsIfResultDefault() {
            var demo = GetDemo();

            Assert.Throws<PostconditionException>(() =>
                demo.PreAndPostconditionValueTypeMethod(1));
        }
        #endregion

        #endregion
    }
}
