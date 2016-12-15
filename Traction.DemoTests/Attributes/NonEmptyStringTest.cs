using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.DemoTests {

    [TestFixture]
    public class NonEmptyStringTest {

        private NonEmptyConsumer GetConsumer() => new NonEmptyConsumer();

        #region Properties

        #region NormalProperty
        [Test]
        public void NonEmpty_NormalProperty_GetDoesNotThrow() {
            var consumer = GetConsumer();
            consumer._normalProperty = "";

            Assert.DoesNotThrow(() => {
                var x = consumer.NormalReadWriteProperty;
            });
        }

        [Test]
        public void NonEmpty_NormalProperty_SetDoesNotThrow() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.NormalReadWriteProperty = "";
            });
        }
        #endregion

        #region ContractReadWriteProperty
        [Test]
        public void NonEmpty_ContractReadWriteProperty_GetDoesNotThrowIfNotEmpty() {
            var consumer = GetConsumer();
            consumer._readWritePropertyWithContract = "test";

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonEmpty_ContractReadWriteProperty_GetThrowsIfEmpty() {
            var consumer = GetConsumer();
            consumer._readWritePropertyWithContract = "";

            Assert.Throws<PostconditionException>(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonEmpty_ContractReadWriteProperty_GetThrowsIfNull() {
            var consumer = GetConsumer();
            consumer._readWritePropertyWithContract = null;

            Assert.Throws<PostconditionException>(() => {
                var x = consumer.ContractReadWriteProperty;
            });
        }

        [Test]
        public void NonEmpty_ContractReadWriteProperty_SetDoesNotThrowIfNotEmpty() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractReadWriteProperty = "test";
            });
        }

        [Test]
        public void NonEmpty_ContractReadWriteProperty_SetThrowsIfEmpty() {
            var consumer = GetConsumer();

            Assert.Throws<PreconditionException>(() => {
                consumer.ContractReadWriteProperty = "";
            });
        }

        [Test]
        public void NonEmpty_ContractReadWriteProperty_SetThrowsIfNull() {
            var consumer = GetConsumer();

            Assert.Throws<PreconditionException>(() => {
                consumer.ContractReadWriteProperty = null;
            });
        }
        #endregion

        #endregion

        #region Methods

        #region NormalMethod
        [Test]
        public void NonEmpty_NormalMethod_DoesNotThrow() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.NormalMethod());
        }
        #endregion

        #region PreconditionMethod
        [Test]
        public void NonEmpty_PreconditionMethod_DoesNotThrowIfArgNotEmpty() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionMethod("test"));
        }

        [Test]
        public void NonEmpty_PreconditionMethod_ThrowsIfArgEmpty() {
            var consumer = GetConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreconditionMethod(""));
        }

        [Test]
        public void NonEmpty_PreconditionMethod_ThrowsIfArgNull() {
            var consumer = GetConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreconditionMethod(null));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void NonEmpty_PostconditionMethod_DoesNotThrowIfResultNotEmpty() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionMethod("test"));
        }

        [Test]
        public void NonEmpty_PostconditionMethod_ThrowsIfResultEmpty() {
            var consumer = GetConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionMethod(""));
        }


        [Test]
        public void NonEmpty_PostconditionMethod_ThrowsIfResultNull() {
            var consumer = GetConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionMethod(null));
        }
        #endregion

        #region PreAndPostconditionMethod
        [Test]
        public void NonEmpty_PreAndPostconditionMethod_DoesNotThrowIfArgAndResultNotEmpty() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreAndPostconditionMethod("test"));
        }

        [Test]
        public void NonEmpty_PreAndPostconditionMethod_ThrowsIfArgEmpty() {
            var consumer = GetConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreAndPostconditionMethod(""));
        }

        [Test]
        public void NonEmpty_PreAndPostconditionMethod_ThrowsIfResultEmpty() {
            var consumer = GetConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PreAndPostconditionMethod("a"));
        }
        #endregion

        #region MultiplePreconditionsMethod
        [Test]
        public void NonEmpty_MultiplePreconditionsMethod_DoesNotThrowIfNoArgsEmpty() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.MultiplePreconditionsMethod("test", "testing"));
        }

        [Test]
        public void NonEmpty_MultiplePreconditionsMethod_ThrowsIfAnyArgEmpty() {
            var consumer = GetConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.MultiplePreconditionsMethod("test", ""));

            Assert.Throws<PreconditionException>(() =>
                consumer.MultiplePreconditionsMethod("", "test"));
        }
        #endregion

        #endregion
    }
}
