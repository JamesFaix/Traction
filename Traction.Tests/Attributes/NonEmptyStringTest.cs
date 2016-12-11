using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.Tests {

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

        #region ContractReadonlyProperty
        [Test]
        public void NonEmpty_ContractReadonlyProperty_DoesNotThrowIfNotEmpty() {
            var consumer = GetConsumer();
            consumer._readonlyPropertyWithContract = "test";

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadonlyProperty;
            });
        }

        [Test]
        public void NonEmpty_ContractReadonlyProperty_ThrowsIfEmpty() {
            var consumer = GetConsumer();
            consumer._readonlyPropertyWithContract = "";

            Assert.Throws<PostconditionException>(() => {
                var x = consumer.ContractReadonlyProperty;
            });
        }
        #endregion

        #region ContractWriteonlyProperty
        [Test]
        public void NonEmpty_ContractWriteonlyProperty_DoesNotThrowIfNotEmpty() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractWriteonlyProperty = "test";
            });
        }

        [Test]
        public void NonEmpty_ContractWriteonlyProperty_ThrowsIfEmpty() {
            var consumer = GetConsumer();

            Assert.Throws<ArgumentException>(() => {
                consumer.ContractWriteonlyProperty = "";
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
        public void NonEmpty_ContractReadWriteProperty_SetDoesNotThrowIfNotEmpty() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractReadWriteProperty = "test";
            });
        }

        [Test]
        public void NonEmpty_ContractReadWriteProperty_SetThrowsIfEmpty() {
            var consumer = GetConsumer();

            Assert.Throws<ArgumentException>(() => {
                consumer.ContractReadWriteProperty = "";
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

            Assert.Throws<ArgumentException>(() =>
                consumer.PreconditionMethod(""));
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

            Assert.Throws<ArgumentException>(() =>
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

            Assert.Throws<ArgumentException>(() =>
                consumer.MultiplePreconditionsMethod("test", ""));

            Assert.Throws<ArgumentException>(() =>
                consumer.MultiplePreconditionsMethod("", "test"));
        }
        #endregion

        #endregion









    }
}
